using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Tank.Logics.Decorators
{
    class SpeedUp : LogicDecorator
    {
        public SpeedUp(ILogic pLogic) : base(pLogic)
        {
            Tank._speed += 5;
        }
    }
}
