using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank
{
    /// <summary>
    /// Decorator to handle Decorations of the tank or tanks
    /// </summary>
    class TankDecorator : Tank
    {
        protected Tank tank;
        public TankDecorator(Tank t)
        {
            this.tank = t;
            AddGraphic((Image)t.Graphic);
        }

        public override void drive()
        {
            tank.drive();
        }

        public override void shoot(float x, float y)
        {
            tank.shoot(x,y);
        }
    }
}
