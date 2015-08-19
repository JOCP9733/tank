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
        private float _xFactor = 1, _yFactor = 1;

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

        /// <summary>
        /// override all move functions to enable speed control by joystick
        /// </summary>
        public override void move_backwards()
        {
            base.move_backwards();
            X += (float)Math.Sin(_image.Angle * Util.DEG_TO_RAD) * _speed * _xFactor;
            Y += (float)Math.Cos(_image.Angle * Util.DEG_TO_RAD) * _speed * _yFactor;
        }

        public override void drive()
        {
            //Console.WriteLine("Arrowcontroll drive");
            Console.WriteLine(Input.GetAxis(JoyAxis.X, 0) / 100f);
            Console.WriteLine(Input.GetAxis(JoyAxis.Y, 0) / 100f);

            _xFactor = Input.GetAxis(JoyAxis.X, 0)/100f;
            _yFactor = Input.GetAxis(JoyAxis.Y, 0)/100f;

            if (Math.Abs(_xFactor) > 0.01f)
            {
                if (_xFactor < 0)
                    tank.move_turn_left();
                else
                    tank.move_turn_right();
            }

            if (Math.Abs(_yFactor) > 0.01f)
            {
                if (_yFactor > 0)
                    tank.move_backwards();
                else
                    tank.move_forward();
            }
        }
    
    }
}
