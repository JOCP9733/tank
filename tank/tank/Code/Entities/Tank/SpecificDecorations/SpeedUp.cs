using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Tank
{
    class SpeedUp: TankDecorator
    {
        public SpeedUp(Tank t) : base(t) { }

    }
}
