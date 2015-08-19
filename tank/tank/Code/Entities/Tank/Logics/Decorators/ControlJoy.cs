using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.Logics.Decorators
{
    class ControlJoy : LogicDecorator
    {
        private bool _disableButtonZero;
        private float _xFactor, _yFactor;

        public ControlJoy(ILogic pLogic) : base(pLogic)
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
            if (Input.ButtonPressed(0) && !_disableButtonZero)
            {
                Tank.FireBullet();
                _disableButtonZero = true;
            }
            else if (!Input.ButtonPressed(0))
                _disableButtonZero = false;
        }

        public void drive()
        {
            //calc a factor from joystick axis position
            _xFactor = Input.GetAxis(JoyAxis.X) / 100f;
            _yFactor = Input.GetAxis(JoyAxis.Y) / 100f;

            //check whether to drive forward or backward, and dont send an event if joystick is resting
            if (Math.Abs(_xFactor) > 0.01f)
                if (_xFactor < 0)
                    Tank.move_turn_left(-_xFactor);
                else
                    Tank.move_turn_right(_xFactor);

            if (Math.Abs(_yFactor) > 0.01f)
                if (_yFactor > 0)
                    Tank.move_backwards(_yFactor);
                else
                    Tank.move_forward(-_yFactor);
        }
    }
}
