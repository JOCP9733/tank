using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.Logics.Decorators
{
    class GetDamage : LogicDecorator
    {
        private bool _disableSpaceKey;

        public GetDamage(ILogic pLogic) : base(pLogic)
        {
            Tank.AddCollider(new PolygonCollider(new Polygon(50,50,50,50,50,50), CollidableTags.Tank));
        }

        public override void Update()
        {
            base.Update();
            Tank.Collider.SetPosition(Tank.X, Tank.Y);
        }

        public void ReceiveDamage()
        {
            Tank.RemoveSelf();
        }
    }
}
