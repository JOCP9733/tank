using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank
{
  
    //TODO: get this shit working 
    class SpeedUp: TankDecorator
    {
        public SpeedUp(Tank t) : base(t) {
            t._speed = t._speed + 5;
            tank = t;
        }
    }
}
