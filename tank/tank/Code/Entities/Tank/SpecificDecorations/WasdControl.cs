using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.SpecificDecorations
{

    class WasdControl: TankDecorator
    {
        private bool _disableSpaceKey;

        public WasdControl(Tank t): base(t) {
            tank = t;
        }

        public override void shoot()
        {
            //return;
            if (Input.KeyDown(Key.Space) && !_disableSpaceKey)
            {
                tank.FireBullet();
                _disableSpaceKey = true;
            }
            else if (Input.KeyUp(Key.Space))
                _disableSpaceKey = false;
        }

        public override void Update()
        {
            base.Update();
            shoot();
            drive();
        }

        public override void drive()
        {
            if (Input.KeyDown(Key.W))
            {
                tank.move_forward();
            }
            if (Input.KeyDown(Key.A))
            {
                tank.move_turn_left();
            }
            if (Input.KeyDown(Key.S))
            {
                tank.move_backwards();
            }
            if (Input.KeyDown(Key.D))
            {
                tank.move_turn_right();
            }
        }
    
    }
}
