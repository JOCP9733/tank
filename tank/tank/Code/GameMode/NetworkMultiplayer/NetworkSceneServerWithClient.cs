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
        private int _targetClientCount = 1;

        public NetworkSceneServerWithClient(Maps mapToLoad = Maps.networkTestBench, int targetClientCount = 1) : base (GameModes.Network)
        {
            //what map should be loaded?
            _mapToLoad = mapToLoad;

            //for how many clients should we wait?
            _targetClientCount = 1;

            //start a client game so we dont have to handle the server player any different
            Scene = new NetworkSceneClient().Scene;

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
        }

        private void OnConnect()
        {
            if (Server.ConnectionsCount == _targetClientCount)
            {
                Scene.Pause();
                //tell each client which tank they can control
                for (int i = 0; i < _targetClientCount; i++)
                {
                    //message identification, which tank, which id to send to
                    NetUtil.SendMessage(Server, MessageType.WhichTankCanIControl, i, i);
                    //notify of game start
                    SendPauseCommand(MessageType.PauseControl, PauseControl.Play);
                }
            }
        }

        private void SendPauseCommand(MessageType messageType, PauseControl data)
        {
            NetOutgoingMessage message = Server.CreateMessage();
            message.Write((int) messageType, 16);
            message.Write((int) data, 16);
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
                        response.Write((int) _mapToLoad, 16);
                        response.Write("tank server. pray that there is only one on the network.");

                        // Send the response to the sender of the request
                        Server.SendDiscoveryResponse(response, msg.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
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
                        var info = (MessageType) msg.ReadUInt32(32);
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
