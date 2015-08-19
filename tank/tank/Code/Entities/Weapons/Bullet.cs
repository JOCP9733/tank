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
        private readonly Collider _tankOriginCollider;

        public Bullet(float x, float y, float dX, float dY, float degOrientation, Collider tankOriginCollider) : base(x, y)
        {

            if (_texture == null)
                _texture = new Texture("Resources/Bullet.png");
            _image = new Image(_texture);

            Collider collider = new BoxCollider(_image.Width, _image.Height, CollidableTags.Bullet);
            AddCollider(collider);

            AddGraphic(_image);
            _image.Angle = degOrientation + 90;

            _direction = new Vector2(4*dX, 4*dY);

            _tankOriginCollider = tankOriginCollider;
        }

        public override void Update()
        {
            base.Update();

            X += _direction.X;
            Y += _direction.Y;

            Collider.Collide(X, Y, _tankOriginCollider);

            if (Collider.Overlap(X, Y, CollidableTags.Tank))
            {
                //fancy scala-like stuff :D this finds all the tanks which are not the one that fired, and deletes them if the bullet hits. this should
                //probably somehow be in the tank to enable changing between bouncy and teamkill etc...
                Collider.CollideEntities(X, Y, CollidableTags.Tank).Where(e => !e.Collider.Overlap(X, Y, _tankOriginCollider)).Each(e => e.RemoveSelf());
                
                //RemoveSelf();
            }
        }
    }
}
