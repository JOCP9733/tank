using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Tank.SpecificDecorations
{
    class SimpleKiControll :TankDecorator
    {

        private int steps;
        public SimpleKiControll(Tank t) : base(t)
        {
            tank = t;
        }

        public override void drive()
        {
            
        }

    }
}
