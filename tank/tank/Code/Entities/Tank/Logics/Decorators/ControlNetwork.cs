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
        
        public ControlNetwork(ILogic pLogic) : base(pLogic)
        {
            if(Program.GameMode.Mode != GameModes.Network)
                throw new NotSupportedException("The gamemode must be network multiplayer for this to work!");

            NetworkScene netScene = (NetworkScene) Program.GameMode;

            netScene.OnData += mHandler;

        }

        void mHandler(object source, NetworkScene.NetworkEventArgs n)
        {
            
        }
        
        public void shoot()
        {
            Tank.FireBullet();
        }

        public void drive()
        {
            if (Input.KeyDown(Key.Up))
            {
                Tank.move_forward();
            }
            if (Input.KeyDown(Key.Left))
            {
                Tank.move_turn_left();
            }
            if (Input.KeyDown(Key.Down))
            {
                Tank.move_backwards();
            }
            if (Input.KeyDown(Key.Right))
            {
                Tank.move_turn_right();
            }
        }
    }
}
