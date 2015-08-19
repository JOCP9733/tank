using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.SpecificDecorations
{
    class GetDamage : TankDecorator
    {
        public GetDamage(Tank t) : base(t)
        {
            tank = t;
        }

        public override void Update()
        {
            base.Update();
            Image tankImage = tank.GetGraphic<Image>();
            tank.AddCollider(new BoxCollider(tankImage.Width, tankImage.Height, CollidableTags.Tank));
        }
    }
}
