using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.Logics.Decorators
{
    class WallCollider : LogicDecorator
    {
        public WallCollider(ILogic pLogic) : base(pLogic)
        {
        }

        public override void Update()
        {
            base.Update();
        }

        public void ReceiveDamage()
        {
            Tank.RemoveSelf();
        }
    }
}
