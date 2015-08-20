using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            if (Tank.Collider.Overlap(Tank.X, Tank.Y, CollidableTags.Wall))
            {
                Tank.WallCollision = true;

                var collidedWith = Tank.Collider.CollideList(Tank.X, Tank.Y, CollidableTags.Wall).ElementAt(0);

                var playerBottom = Tank.Collider.Bottom;
                var playerRight = Tank.Collider.Right;
                var playerLeft = Tank.Collider.Left;
                var playerTop = Tank.Collider.Top;

                var tileBottom = collidedWith.Bottom;
                var tileRight = collidedWith.Right;
                var tileLeft = collidedWith.Left;
                var tileTop = collidedWith.Top;

                var bCollision = tileBottom - playerTop;
                var tCollision = playerBottom - tileTop;
                var lCollision = playerRight - tileLeft;
                var rCollision = tileRight - playerLeft;

                if (tCollision < bCollision && tCollision < lCollision && tCollision < rCollision)
                {
                    //Top collision
                    Tank.WallCollisionDirection = Direction.Top;
                }
                if (bCollision < tCollision && bCollision < lCollision && bCollision < rCollision)
                {
                    //bottom collision
                    Tank.WallCollisionDirection = Direction.Bottom;
                }
                if (lCollision < rCollision && lCollision < tCollision && lCollision < bCollision)
                {
                    //Left collision
                    Tank.WallCollisionDirection = Direction.Left;
                }
                if (rCollision < lCollision && rCollision < tCollision && rCollision < bCollision)
                {
                    //Right collision
                    Tank.WallCollisionDirection = Direction.Right;
                }
            }
            else
            {
                Tank.WallCollision = false;
            }

            Console.WriteLine(Tank.WallCollision);
        }
    }
}
