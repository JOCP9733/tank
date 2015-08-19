using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank
{
  
    //TODO: get this shit working 
    class SpeedUp: TankDecorator
    {
        public SpeedUp(Tank t) : base(t) {
            t._speed = t._speed + 5;
            tank = t;
        }

        public override void drive()
        {
            base.drive();

            //if (Scene == null)
            //    return;
            //if (Input.KeyDown(Key.W))
            //{
            //    tank.move_forward();
            //}
            //if (Input.KeyDown(Key.A))
            //{
            //    tank.move_turn_left();
            //}
            //if (Input.KeyDown(Key.S))
            //{
            //    tank.move_backwards();
            //}
            //if (Input.KeyDown(Key.D))
            //{
            //    tank.move_turn_right();
            //}

        }
    }
}
