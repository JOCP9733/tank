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

        public static void SendMessage(NetPeer peer, MessageType messageType, int data, int recipient)
        {
            SendMessage(peer, messageType, data, peer.Connections[recipient]);
        }

        public static void SendMessage(NetPeer peer, MessageType messageType, int data, NetConnection recipient)
        {
            NetOutgoingMessage message = peer.CreateMessage();
            message.Write((byte)messageType);
            message.Write(data, 32);
            peer.SendMessage(message, recipient, NetDeliveryMethod.ReliableUnordered);
        }
    }

    //This is a class which describes the event to the class that recieves it.
    //An EventArgs class must always derive from System.EventArgs.
    class NetworkEventArgs : EventArgs
    {
        private readonly NetIncomingMessage _data;
        private readonly MessageType _info;

        public NetworkEventArgs(NetIncomingMessage data, MessageType info)
        {
            _data = data;
            _info = info;
        }

        public MessageType GetInfo()
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
    enum MessageType : uint
    {
        TankControl,
        PauseControl,
        LoadFinish,
        WhichTankCanIControl //i have no excuse
    }

    enum PauseControl : uint
    {
        Pause,
        Play
    }
}
