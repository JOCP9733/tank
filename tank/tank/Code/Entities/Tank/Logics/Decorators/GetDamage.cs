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
        private readonly Polygon _basePolygon;
        public GetDamage(ILogic pLogic) : base(pLogic)
        {
            _basePolygon = new Polygon(0,0, 50,0, 50,50, 0,50);
            Tank.AddCollider(new PolygonCollider(_basePolygon, CollidableTags.Tank));
            Tank.Collider.CenterOrigin();
        }

        public override void Update()
        {
            base.Update();
            Utilities.RotatePolygon(Tank.Graphic.Angle, Tank.Collider, _basePolygon);
        }

        public void ReceiveDamage()
        {
            Tank.RemoveSelf();
        }
    }
}
