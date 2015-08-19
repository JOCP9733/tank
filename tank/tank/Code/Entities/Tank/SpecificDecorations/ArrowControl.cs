using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.SpecificDecorations
{

    class ArrowControl: TankDecorator
    {
        public ArrowControl(Tank t): base(t) {
            tank = t;
        }

        public override void shoot()
        {
            if (Input.KeyDown(Key.RControl))
            {
                tank.FireBullet();
            }
        }

        public override void drive()
        {
            //Console.WriteLine("Arrowcontroll drive");
            if (Input.KeyDown(Key.Up))
            {
                tank.move_forward();
            }
            if (Input.KeyDown(Key.Left))
            {
                tank.move_turn_left();
            }
            if (Input.KeyDown(Key.Down))
            {
                tank.move_backwards();
            }
            if (Input.KeyDown(Key.Right))
            {
                tank.move_turn_right();
            }
        }
    
    }
}
