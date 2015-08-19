using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.Logics.Decorators
{
    class ControlWasd : LogicDecorator
    {
        private bool _disableSpaceKey;

        public ControlWasd(ILogic pLogic) : base(pLogic)
        {
        }

        public override void Update()
        {
            base.Update();
            shoot();
            drive();
            Tank.Logic = new ControlWasd((ProtoLogic) Tank.Logic);
        }


        public void shoot()
        {
            //return;
            if (Input.KeyDown(Key.Space) && !_disableSpaceKey)
            {
                Tank.FireBullet();
                _disableSpaceKey = true;
            }
            else if (Input.KeyUp(Key.Space))
                _disableSpaceKey = false;
        }

        public void drive()
        {
            if (Input.KeyDown(Key.W))
            {
                Tank.move_forward();
            }
            if (Input.KeyDown(Key.A))
            {
                Tank.move_turn_left();
            }
            if (Input.KeyDown(Key.S))
            {
                Tank.move_backwards();
            }
            if (Input.KeyDown(Key.D))
            {
                Tank.move_turn_right();
            }
        }
    }
}
