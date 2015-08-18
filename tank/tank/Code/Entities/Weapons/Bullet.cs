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
        private readonly Image _image;
        private readonly Vector2 _direction;

        public Bullet(float x, float y, float dX, float dY, float degOrientation) : base(x, y)
        {

            if (_texture == null)
                _texture = new Texture("Resources/bullet.png");
            _image = new Image(_texture);

            Collider collider = new BoxCollider(_image.Width, _image.Height, CollidableTags.Bullet);
            AddCollider(collider);

            AddGraphic(_image);
            _image.Angle = degOrientation;

            _direction = new Vector2(4*dX, 4*dY);
        }

        public override void Update()
        {
            base.Update();

            X += _direction.X;
            Y += _direction.Y;

            if (Collider.Overlap(X, Y, CollidableTags.Tank))
            {
                Collider.CollideEntities(X, Y, CollidableTags.Tank).ForEach(e => e.RemoveSelf());
                RemoveSelf();
            }
        }
    }
}
