using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Otter;
using tank.Code.Entities.Tank;
using tank.Code.Entities.Tank.Logics;
using tank.Code.Entities.Tank.Logics.Decorators;

namespace tank.Code.GameMode.NetworkMultiplayer
{
    class NetworkSceneServerWithClient : GameMode
    {
        public NetServer Server;
        public event NetworkEvent OnData;
        private Maps _mapToLoad;
        private int _targetClientCount = 2, _loadedClients = 0;
        public NetworkSceneClient ClientScene;

        public NetworkSceneServerWithClient(int targetClientCount = 1, Maps mapToLoad = Maps.networkTestBench) : base (GameModes.Network)
        {
            //what map should be loaded?
            _mapToLoad = mapToLoad;

            //for how many clients should we wait?
            _targetClientCount = targetClientCount;

            //start a client game so we dont have to handle the server player any different
            ClientScene = new NetworkSceneClient();
            Scene = ClientScene.Scene;
            Peer = ClientScene.Peer;

            //in game update so we can pause the scene without problems
            Game.Instance.OnUpdate += ReadNetwork;

            //server config
            NetPeerConfiguration config = new NetPeerConfiguration("tank");
            config.Port = 14242;

            //console info
            Console.WriteLine("server");

            //start the server
            Server = new NetServer(config);
            Server.Start();

            OnData += IncomingMessageHandler;
        }

        void IncomingMessageHandler(object source, NetworkEventArgs n)
        {
            NetIncomingMessage msg = n.GetData();
            if (n.GetInfo() == MessageType.LoadFinish)
            {
                //just ignore the message content for now
                _loadedClients++;
                if (_loadedClients == _targetClientCount)
                    OnAllLoaded();
            }
            else if (n.GetInfo() == MessageType.TankControl)
            {
                //read from old message
                int networkId = msg.ReadInt32(32);
                NetworkAction networkAction = (NetworkAction) msg.ReadByte();

                //write to new message
                NetOutgoingMessage relayMessage = Server.CreateMessage(n.GetData().LengthBits);
                relayMessage.Write((byte) MessageType.TankControl);
                relayMessage.Write(networkId, 32);
                relayMessage.Write((byte) networkAction);
                Server.SendToAll(relayMessage, NetDeliveryMethod.ReliableUnordered);
            }
        }

        private void OnAllLoaded()
        {
            for(int i = 0; i < _loadedClients; i++)
                NetUtil.SendMessage(Server, MessageType.WhichTankCanIControl, i, i);
            SendPauseCommand(MessageType.PauseControl, PauseControl.Play);
        }

        private void OnConnect()
        {
            SendPauseCommand(MessageType.PauseControl, PauseControl.Pause);
            if (Server.ConnectionsCount == _targetClientCount)
            {
            }
        }

        private void SendPauseCommand(MessageType messageType, PauseControl data)
        {
            NetOutgoingMessage message = Server.CreateMessage();
            message.Write((byte) messageType);
            message.Write((byte) data);
            Server.SendToAll(message, NetDeliveryMethod.ReliableUnordered);
        }

        private void SendToAll(MessageType messageType, int data)
        {
            NetOutgoingMessage message = Server.CreateMessage();
            message.Write((int)messageType, 16);
            message.Write(data, 32);
            Server.SendToAll(message, NetDeliveryMethod.ReliableUnordered);
        }

        /// <summary>
        /// Method is called on each update to check for new messages
        /// </summary>
        private void ReadNetwork()
        {
            NetIncomingMessage msg;
            while ((msg = Server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        // Create a response and write some example data to it
                        NetOutgoingMessage response = Server.CreateMessage();
                        response.Write((byte) _mapToLoad);
                        response.Write("tank server. pray that there is only one on the network.");

                        // Send the response to the sender of the request
                        Server.SendDiscoveryResponse(response, msg.SenderEndPoint);
                        break;
                        case NetIncomingMessageType.StatusChanged:
                            if((NetConnectionStatus) msg.ReadByte() == NetConnectionStatus.RespondedConnect)
                                OnConnect();
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.Data:
                        //the first four bytes in each message contain the type
                        var info = (MessageType) msg.ReadByte();
                        //notify everyone who wanted to be notified upon message arrival
                        OnData?.Invoke(Server, new NetworkEventArgs(msg, info));
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
                Server.Recycle(msg);
            }
        }
    }
}
