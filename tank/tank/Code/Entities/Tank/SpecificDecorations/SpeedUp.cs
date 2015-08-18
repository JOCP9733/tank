using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank
{
    class SpeedUp: TankDecorator
    {
        public SpeedUp(Tank t) : base(t) { }

        public override void drive()
        {
            if (Input.KeyDown(Key.Up))
            {

            }
            if (Input.KeyDown(Key.Left))
            {

            }
            if (Input.KeyDown(Key.Down))
            {

            }
            if (Input.KeyDown(Key.Right))
            {

            }
        }
    }
}
