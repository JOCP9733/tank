using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Tank.Logics.Decorators
{
    class ControlSimpleKI : LogicDecorator
    {
        public ControlSimpleKI(ILogic pLogic) : base(pLogic)
        {
        }

        public override void Update()
        {
            base.Update();
            move();
        }

        private void move()
        {
            Tank.move_forward();
            Tank.move_turn_right();

        }


    }
}
