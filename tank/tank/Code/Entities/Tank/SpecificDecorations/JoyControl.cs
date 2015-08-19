using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.SpecificDecorations
{

    class JoyControl: TankDecorator
    {
        private float _xFactor, _yFactor;

        public JoyControl(Tank t): base(t) {
            tank = t;
        }

        public override void shoot()
        {
            if (Input.ButtonPressed(0))
            {
                tank.FireBullet();
            }
        }

        public override void Update()
        {
            base.Update();
            shoot();
            drive();
        }

        public override void drive()
        {
            //calc a factor from joystick axis position
            _xFactor = Input.GetAxis(JoyAxis.X)/100f;
            _yFactor = Input.GetAxis(JoyAxis.Y)/100f;

            //check whether to drive forward or backward, and dont send an event if joystick is resting
            if (Math.Abs(_xFactor) > 0.01f)
                if (_xFactor < 0)
                    tank.move_turn_left(-_xFactor);
                else
                    tank.move_turn_right(_xFactor);

            if (Math.Abs(_yFactor) > 0.01f)
                if (_yFactor > 0)
                    tank.move_backwards(_yFactor);
                else
                    tank.move_forward(-_yFactor);
        }
    
    }
}
