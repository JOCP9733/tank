using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Tank.SpecificDecorations
{
    class SimpleKiControll :TankDecorator
    {
        public SimpleKiControll(Tank t) : base(t)
        {
            tank = t;
        }
        public override void Update()
        {
            base.Update();
            drive();
            shoot();
        }

        public override void drive()
        {
            tank.move_forward();
            tank.move_turn_right();
        }

        public override void shoot()
        {
            

        }
    }
}
