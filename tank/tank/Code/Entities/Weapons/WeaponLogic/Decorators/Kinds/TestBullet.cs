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
        private static Texture _texture;
        private readonly Vector2 _direction;

        public TestBullet(IProjectileLogic p) : base(p)
        {
            if (_texture == null)
                _texture = new Texture("Resources/Bullet.png");

            var image = new Image(_texture);

            var collider = new PolygonCollider(_basePolygon, CollidableTags.Bullet);
            Utilities.RotatePolygon(Projectile.DegOrientation + 90, collider, _basePolygon);
            Projectile.AddCollider(collider);

            Projectile.AddGraphic(image);
            image.Angle = Projectile.DegOrientation + 90;

            _direction = new Vector2(4 * (float)Math.Sin(Projectile.DegOrientation * Util.DEG_TO_RAD),
                4 * (float)Math.Cos(Projectile.DegOrientation * Util.DEG_TO_RAD));
        }

        public override void Render()
        {
            base.Render();
            Projectile.Collider?.Render();
        }

        public override void Update()
        {
            base.Update();

            Projectile.X -= _direction.X;
            Projectile.Y -= _direction.Y;

            if (Projectile.Collider.Overlap(Projectile.X, Projectile.Y, CollidableTags.Tank))
            {
                foreach (var collideEntity in Projectile.Collider.CollideEntities(Projectile.X, Projectile.Y, CollidableTags.Tank))
                {
                    if (collideEntity != Projectile.OriginTank)
                    {
                        collideEntity.RemoveSelf();
                        Projectile.RemoveSelf();
                    }
                }
            }
        }
    }
}
