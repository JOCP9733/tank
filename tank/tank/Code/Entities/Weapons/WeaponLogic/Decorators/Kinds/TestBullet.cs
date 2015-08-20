using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Weapons.WeaponLogic.Decorators.Kinds
{
    class TestBullet : ProjectileDecorator
    {
       
        private readonly Polygon _basePolygon = new Polygon(5, 3, 80, 4, 88, 10, 81, 17, 7, 16);

        public TestBullet(IProjectile p) : base(p)
        {
            
            if (Weapon._texture == null)
                Weapon._texture = new Texture("Resources/Bullet.png");

            var image = new Image(Weapon._texture);

            var collider = new PolygonCollider(_basePolygon, CollidableTags.Bullet);
            Utilities.RotatePolygon(_degOrientation + 90, collider, _basePolygon);
            _weapon.AddCollider(collider);

            _weapon.AddGraphic(image);
            image.Angle = _degOrientation + 90;

            _direction = new Vector2(4 * (float)Math.Sin(_degOrientation * Util.DEG_TO_RAD),
                4 * (float)Math.Cos(_degOrientation * Util.DEG_TO_RAD));
        }

    }
}
