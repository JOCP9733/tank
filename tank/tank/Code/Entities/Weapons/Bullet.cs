using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Weapons
{
    internal class Bullet : Entity
    {
        private static Texture _texture;
        private readonly Vector2 _direction;
        private readonly Tank.Tank _originTank;
        private readonly Polygon _basePolygon = new Polygon(5,3, 80,4, 88,10, 81,17, 7,16);

        public Bullet(float x, float y, float degOrientation, Tank.Tank originTank) : base(x, y)
        {
            if (_texture == null)
                _texture = new Texture("Resources/Bullet.png");

            var image = new Image(_texture);

            _originTank = originTank;

            var collider = new PolygonCollider(_basePolygon, CollidableTags.Bullet);
            Utilities.RotatePolygon(degOrientation + 90, collider, _basePolygon);
            AddCollider(collider);

            AddGraphic(image);
            image.Angle = degOrientation + 90;

            _direction = new Vector2(4* (float)Math.Sin(degOrientation * Util.DEG_TO_RAD), 
                4 * (float)Math.Cos(degOrientation * Util.DEG_TO_RAD));
        }

        public override void Render()
        {
            base.Render();
            Collider.Render();
        }

        public override void Update()
        {
            base.Update();

            X -= _direction.X;
            Y -= _direction.Y;

            if (Collider.Overlap(X, Y, CollidableTags.Tank))
            {
                foreach (var collideEntity in Collider.CollideEntities(X, Y, CollidableTags.Tank))
                {
                    if (collideEntity != _originTank)
                    {
                        collideEntity.RemoveSelf();
                        RemoveSelf();
                    }
                }
            }
        }
    }
}
