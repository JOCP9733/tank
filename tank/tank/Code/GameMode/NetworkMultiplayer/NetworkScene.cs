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
    class NetworkScene : GameMode
    {
        Tank tank = new Tank(50, 50);
        Tank tank2 = new Tank(100, 50);
        private NetServer _server;
        public event NetworkEventHandler OnData; 

        public NetworkScene() : base (GameModes.Network)
        {
            Scene.Add(tank);
            Scene.Add(tank2);
            Scene.OnBegin = Initialise;
            Scene.OnUpdate += ReadNetwork;

            NetPeerConfiguration config = new NetPeerConfiguration("tank");
            config.Port = 14242;

            _server = new NetServer(config);
            _server.Start();
        }

        /// <summary>
        /// Method is called on each update to check for new messages
        /// </summary>
        private void ReadNetwork()
        {
            NetIncomingMessage msg;
            while ((msg = _server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.Data:
                        //the first four bytes in each message contain the type. hopefully at least.
                        var info = (MessageType) msg.ReadUInt32(32);
                        //notify everyone who wanted to be notified upon message arrival
                        OnData?.Invoke(_server, new NetworkEventArgs(msg, info));
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
                _server.Recycle(msg);
            }
        }

        public delegate void NetworkEventHandler(object source, NetworkEventArgs n);

        //This is a class which describes the event to the class that recieves it.
        //An EventArgs class must always derive from System.EventArgs.
        public class NetworkEventArgs : EventArgs
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

        /// <summary>
        /// Enum of the available message types, eg the first 4 bytes per data message (->uint = 4 bytes)
        /// </summary>
        internal enum MessageType : uint
        {
            Forward,
            Reward,
            Left,
            Right,
            Shoot,
            InitData
        }

        /// <summary>
        /// enum for the initialiser messages, second set of 4 bytes
        /// </summary>
        internal enum InitMessageType : uint
        {
            Tank
        }
        
        private void Initialise()
        {
            tank.AddDecorator(Decorators.ControlJoy);
            tank.AddDecorator(Decorators.GetDamage);

            tank2.AddDecorator(Decorators.GetDamage);
            tank2.AddDecorator(Decorators.ControlSimpleKi);
            tank2.AddDecorator(Decorators.SpeedUp);
        }
    }    
}
