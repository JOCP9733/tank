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
        public GetDamage(ILogic pLogic) : base(pLogic)
        {
            Tank.AddCollider(new BoxCollider(Tank.Graphic.Width, Tank.Graphic.Height, CollidableTags.Tank));
            Tank.Collider.CenterOrigin();
        }
        
        public void ReceiveDamage()
        {
            Tank.RemoveSelf();
        }
    }
}
