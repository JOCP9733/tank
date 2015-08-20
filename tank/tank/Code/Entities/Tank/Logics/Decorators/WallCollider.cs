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
        public WallCollider(ITankLogic pLogic) : base(pLogic)
        {
        }

        public override void Update()
        {
            base.Update();
            if (Tank.Collider.Overlap(Tank.X, Tank.Y, CollidableTags.Wall))
            {
                Tank.WallCollision = true;

                var collidedWithList = Tank.Collider.CollideList(Tank.X, Tank.Y, CollidableTags.Wall);
                GridCollider collidedWith = (GridCollider) collidedWithList[0];

                var playerBottom = Tank.Collider.Bottom;
                var playerRight = Tank.Collider.Right;
                var playerLeft = Tank.Collider.Left;
                var playerTop = Tank.Collider.Top;

                var tileBottom = collidedWith.Bottom;
                var tileRight = collidedWith.Right;
                //collidedWith.Left is 0 for whatever reason
                var tileLeft = collidedWith.Right - collidedWith.Width;
                var tileTop = collidedWith.Bottom - collidedWith.Height;

                Direction xDirection, yDirection;
                xDirection = Tank.Direction.X > 0 ? Direction.Left : Direction.Right;
                yDirection = Tank.Direction.Y > 0 ? Direction.Up : Direction.Down;

                if (Math.Abs(Tank.Direction.X) > Math.Abs(Tank.Direction.Y))
                {
                    if (xDirection == Direction.Right)
                    {
                        Console.WriteLine("r");
                        int tileColumn = collidedWith.GridX(playerRight);
                        Tank.X = collidedWith.TileWidth*(tileColumn - 1);
                    }
                    else
                    {
                        Console.WriteLine("l");
                        int tileColumn = collidedWith.GridX(playerLeft);
                        Tank.X = collidedWith.TileWidth*(tileColumn) + Tank.Graphic.ScaledWidth;
                    }
                }
                else
                {
                    if (yDirection == Direction.Down)
                    {
                        Console.WriteLine("d");
                        int tileRow = collidedWith.GridY(playerBottom);
                        Tank.Y = collidedWith.TileHeight * (tileRow) - Tank.Graphic.ScaledHeight;
                    }
                    else
                    {
                        Console.WriteLine("u");
                        int tileRow = collidedWith.GridX(playerTop);
                        Tank.Y = collidedWith.TileHeight * (tileRow);
                    }
                }
            }
            else
            {
                Tank.WallCollision = false;
            }
            //if(!Tank.WallCollision)
            //    Console.WriteLine("-");
        }
    }
}
