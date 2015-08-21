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
            PolygonCollider tCol = (PolygonCollider)Tank.Collider;
            Polygon pol = tCol.Polygon;
            List<Vector2> cornerList = pol.Points;

            if (Tank.Collider.Overlap(Tank.X, Tank.Y, CollidableTags.Wall))
            {
                Tank.WallCollision = true;

                var collidedWithList = Tank.Collider.CollideList(Tank.X, Tank.Y, CollidableTags.Wall);


                GridCollider wCol = (GridCollider)collidedWithList[0];
                var playerBottom = Tank.Collider.Bottom;
                var playerRight = Tank.Collider.Right;
                var playerLeft = Tank.Collider.Left;
                var playerTop = Tank.Collider.Top;

                //calculate the direction of the collision by checking each edge of the polygon for collision


                Direction collisionDirection = Direction.Crap;
                for (int i = 0; i < cornerList.Count; i++)
                {
                    //add tank position because the polygon points are relative
                    float cornerX = cornerList[i].X + Tank.X - Tank.Graphic.HalfWidth, cornerY = cornerList[i].Y + Tank.Y - Tank.Graphic.HalfHeight;
                    
                    if (wCol.GetTileAtPosition(cornerX, cornerY))
                    {
                        collisionDirection = GetDirection(playerBottom, playerRight, playerLeft, playerTop, cornerX, cornerY);
                        break;
                    }
                }
                //must be 4

                /*if (wCol.GetTile((int)cornerList[0].X, (int)cornerList[0].Y))
                    collisionDirection = GetDirection(playerBottom, playerRight, playerLeft, playerTop, cornerList[0].X + Tank.X, cornerList[0].Y + Tank.Y);
                else if (wCol.GetTile((int)cornerList[1].X, (int)cornerList[1].Y))
                    collisionDirection = GetDirection(playerBottom, playerRight, playerLeft, playerTop, cornerList[1].X + Tank.X, cornerList[1].Y + Tank.Y);
                else if (wCol.GetTile((int)cornerList[2].X, (int)cornerList[2].Y))
                    collisionDirection = GetDirection(playerBottom, playerRight, playerLeft, playerTop, cornerList[2].X + Tank.X, cornerList[2].Y + Tank.Y);
                else//if (wCol.GetTile((int)cornerList[3].X, (int)cornerList[3].Y))
                    collisionDirection = GetDirection(playerBottom, playerRight, playerLeft, playerTop, cornerList[3].X + Tank.X, cornerList[3].Y + Tank.Y);*/
                Console.WriteLine(collisionDirection);
            }
            else
            {
                Tank.WallCollision = false;
            }
            if(!Tank.WallCollision)
                Console.WriteLine("-");
        }

        /// <summary>
        /// basically this finds out what direction the edge at cornerX/cornerY points to by finding the smallest difference between a direction and cornerX/Y
        /// </summary>
        private Direction GetDirection(float bottom, float right, float left, float top, float cornerX, float cornerY)
        {
            //use absolute values
            bottom = Math.Abs(bottom);
            right = Math.Abs(right);
            left = Math.Abs(left);
            top = Math.Abs(top);
            cornerX = Math.Abs(cornerX);
            cornerY = Math.Abs(cornerY);

            //uuh yeah this made sense once. 
            //basically because the bottom/right/left/top valus only represent x or y of a corner, so we need to take the one
            //with the smaller difference to represent the direction
            var bottomCandidate = Math.Min(Math.Abs(bottom - cornerX), Math.Abs(bottom - cornerY));
            var rightCandidate = Math.Min(Math.Abs(right - cornerX), Math.Abs(right - cornerY));
            var leftCandidate = Math.Min(Math.Abs(left - cornerX), Math.Abs(left - cornerY));
            var topCandidate = Math.Min(Math.Abs(top - cornerX), Math.Abs(top - cornerY));

            if (bottomCandidate < rightCandidate && bottomCandidate < leftCandidate && bottomCandidate < topCandidate)
                return Direction.Down;
            if (rightCandidate < bottomCandidate && rightCandidate < leftCandidate && rightCandidate < topCandidate)
                return Direction.Right;
            if (leftCandidate < rightCandidate && leftCandidate < bottomCandidate && leftCandidate < topCandidate)
                return Direction.Left;
            //if (topCandidate < rightCandidate && topCandidate < leftCandidate && topCandidate < leftCandidate)
            return Direction.Up;
        }

        public override void Render()
        {
            base.Render();
            Scene.GetColliders(CollidableTags.Wall)[0].Render();
        }
    }
}
