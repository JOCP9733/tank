using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Otter;
using tank.Code.Entities.Tank;

namespace tank.Code
{
    delegate void NetworkEvent(object source, NetworkEventArgs n);
    
    static class NetUtil
    {
        public static Tank GetTankByNetworkId(Scene scene, int id)
        {
            return scene.GetEntities<Tank>().Where(t => t.NetworkId == id).ToList()[0];
        }

        public static void SendMessage(NetPeer peer, NetworkMessageType networkMessageType, int data, int recipient)
        {
            SendMessage(peer, networkMessageType, data, peer.Connections[recipient]);
        }

        public static void SendMessage(NetPeer peer, NetworkMessageType networkMessageType, int data, NetConnection recipient)
        {
            NetOutgoingMessage message = peer.CreateMessage();
            message.Write((byte)networkMessageType);
            message.Write(data, 32);
            peer.SendMessage(message, recipient, NetDeliveryMethod.ReliableUnordered);
        }
    }

    class GameServer
    {
        /// <summary>
        /// Lidgren server instance
        /// </summary>
        public NetServer Server;

        /// <summary>
        /// Server data receive event
        /// </summary>
        public event NetworkEvent OnServerData;

        /// <summary>
        /// Map the clients should load
        /// </summary>
        private readonly Maps _mapToLoad;

        /// <summary>
        /// The client count at which to start the game
        /// </summary>
        private readonly int _targetClientCount;

        /// <summary>
        /// How many clients have yet loaded the map (and thus the entities)
        /// </summary>
        private int _loadedClients;

        public GameServer(int targetClientCount = 2, Maps mapToLoad = Maps.networkTestBench)
        {
            //what map should be loaded?
            _mapToLoad = mapToLoad;

            //for how many clients should we wait?
            _targetClientCount = targetClientCount;

            //in game update so we can pause the scene without problems
            Game.Instance.OnUpdate += ReadNetwork;

            //console info
            Console.WriteLine("server");

            //server config
            NetPeerConfiguration config = new NetPeerConfiguration("tank");
            config.Port = 14242;

            //start the server
            Server = new NetServer(config);
            Server.Start();

            OnServerData += ServerOnDataHandler;
        }

        /// <summary>
        /// Handler the server registers to react to network messages
        /// </summary>
        void ServerOnDataHandler(object source, NetworkEventArgs n)
        {
            NetIncomingMessage msg = n.GetData();
            NetworkMessageType msgType = n.GetInfo();
            if (msgType == NetworkMessageType.LoadFinish)
            {
                //just ignore the message content for now
                _loadedClients++;
                if (_loadedClients == _targetClientCount)
                    OnAllLoaded();
            }
            else if (msgType == NetworkMessageType.TankControl)
            {
                //read from old message
                int networkId = msg.ReadInt32(32);
                NetworkAction networkAction = (NetworkAction)msg.ReadByte();

                //write to new message
                NetOutgoingMessage relayMessage = Server.CreateMessage(n.GetData().LengthBits);
                relayMessage.Write((byte)NetworkMessageType.TankControl);
                relayMessage.Write(networkId, 32);
                relayMessage.Write((byte)networkAction);
                Server.SendToAll(relayMessage, NetDeliveryMethod.ReliableUnordered);
            }
        }

        /// <summary>
        /// Called when all clients have sent their load finish signal
        /// </summary>
        private void OnAllLoaded()
        {
            //tell each client which tank is "theirs"
            for (int i = 0; i < _loadedClients; i++)
                NetUtil.SendMessage(Server, NetworkMessageType.WhichTankCanIControl, i, i);
            //the clients can now unpause the scene
            SendPauseCommand(NetworkMessageType.PauseControl, NetworkPauseControl.Play);


        }

        /// <summary>
        /// Called whenever a client connects
        /// </summary>
        private void OnConnect()
        {
            SendPauseCommand(NetworkMessageType.PauseControl, NetworkPauseControl.Pause);
        }

        /// <summary>
        /// Helper function to control pause for each client
        /// </summary>
        private void SendPauseCommand(NetworkMessageType networkMessageType, NetworkPauseControl data)
        {
            NetOutgoingMessage message = Server.CreateMessage();
            message.Write((byte)networkMessageType);
            message.Write((byte)data);
            Server.SendToAll(message, NetDeliveryMethod.ReliableUnordered);
        }

        /// <summary>
        /// Helper to send data to all clients
        /// </summary>
        private void SendToAll(NetworkMessageType networkMessageType, int data)
        {
            NetOutgoingMessage message = Server.CreateMessage();
            message.Write((int)networkMessageType, 16);
            message.Write(data, 32);
            Server.SendToAll(message, NetDeliveryMethod.ReliableUnordered);
        }

        /// <summary>
        /// Called when a client discovers this server
        /// </summary>
        /// <param name="msg"></param>
        private void OnDiscoveryRequest(NetIncomingMessage msg)
        {
            //respond with map to load and server message
            NetOutgoingMessage response = Server.CreateMessage();
            response.Write((byte)_mapToLoad);
            response.Write("tank server. pray that there is only one on the network.");

            // Send the response to the sender of the request
            Server.SendDiscoveryResponse(response, msg.SenderEndPoint);
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
                        OnDiscoveryRequest(msg);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        if ((NetConnectionStatus)msg.ReadByte() == NetConnectionStatus.RespondedConnect)
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
                        var info = (NetworkMessageType)msg.ReadByte();
                        //notify everyone who wanted to be notified upon message arrival
                        OnServerData?.Invoke(Server, new NetworkEventArgs(msg, info));
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
                Server.Recycle(msg);
            }
        }
    }

    //This is a class which describes the event to the class that recieves it.
    //An EventArgs class must always derive from System.EventArgs.
    class NetworkEventArgs : EventArgs
    {
        private readonly NetIncomingMessage _data;
        private readonly NetworkMessageType _info;

        public NetworkEventArgs(NetIncomingMessage data, NetworkMessageType info)
        {
            _data = data;
            _info = info;
        }

        public NetworkMessageType GetInfo()
        {
            return _info;
        }

        public NetIncomingMessage GetData()
        {
            return _data;
        }
    }

    enum NetworkAction
    {
        Left,
        Right,
        Forward,
        Reward,
        Fire
    }

    /// <summary>
    /// Enum of the available message types, eg the first 4 bytes per data message (->uint = 4 bytes)
    /// </summary>
    enum NetworkMessageType : uint
    {
        TankControl,
        PauseControl,
        LoadFinish,
        WhichTankCanIControl //i have no excuse
    }

    enum NetworkPauseControl : uint
    {
        Pause,
        Play
    }
}
