using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Otter;
using tank.Code.GameMode.NetworkMultiplayer;

namespace tank.Code.Entities.Tank.Logics.Decorators
{
    class ControlNetwork : LogicDecorator
    {
        
        public ControlNetwork(ITankLogic pLogic) : base(pLogic)
        {
            if(Program.GameMode.Mode != GameModes.Network)
                throw new NotSupportedException("The gamemode must be network multiplayer for this to work!");

            if(Program.GameMode is NetworkSceneClient)
                ((NetworkSceneClient) Program.GameMode).OnClientData += CommandHandler;
        }

        void CommandHandler(object source, NetworkEventArgs n)
        {
            if (n.GetInfo() == NetworkMessageType.TankControl)
            {
                NetIncomingMessage msg = n.GetData();
                if (msg.PositionInBytes == 8)
                    return;
                //only if this tank is actually meant...
                if (msg.ReadInt32(32) == Tank.NetworkId)
                {
                    NetworkAction a = (NetworkAction) msg.ReadByte();
                    switch (a)
                    {
                        case NetworkAction.Left:
                            Tank.move_turn_left();
                            break;
                        case NetworkAction.Right:
                            Tank.move_turn_right();
                            break;
                        case NetworkAction.Forward:
                            Tank.move_forward();
                            break;
                        case NetworkAction.Reward:
                            Tank.move_backwards();
                            break;
                        case NetworkAction.Fire:
                            Tank.FireBullet();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}
