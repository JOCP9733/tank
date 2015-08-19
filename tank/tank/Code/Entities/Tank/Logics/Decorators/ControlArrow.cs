using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.Logics.Decorators
{
    class ControlArrow : LogicDecorator
    {
        private bool _disableRControlKey;

        public ControlArrow(ILogic pLogic) : base(pLogic)
        {
        }

        public override void Update()
        {
            base.Update();
            shoot();
            drive();
        }


        public void shoot()
        {
            //return;
            if (Input.KeyDown(Key.RControl) && !_disableRControlKey)
            {
                Tank.FireBullet();
                _disableRControlKey = true;
            }
            else if (Input.KeyUp(Key.Space))
                _disableRControlKey = false;
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
