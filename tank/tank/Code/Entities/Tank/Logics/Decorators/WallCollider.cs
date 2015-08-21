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
        private int rx=-1, ry=-1;
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
                var playerTop = Tank.Collider.Top;
                var playerRight = Tank.Collider.Right;
                var playerLeft = Tank.Collider.Left;

                //how many tiles are beneath the tank?
                //the calculation didnt work generously enough...
                int amountOfXTiles = 3;//(int) Math.Round(Math.Round(((PolygonCollider)Tank.Collider).Polygon.Width)/(double)wCol.TileWidth);
                int amountOfYTiles = 3;//(int) Math.Round(Math.Round(((PolygonCollider)Tank.Collider).Polygon.Height)/(double)wCol.TileHeight);

                //calculate check start position
                int leftmostTile = wCol.GridX(playerLeft);
                int topmostTile = wCol.GridY(playerTop);

                //results
                int resX = -1, resY = -1;
                rx = leftmostTile;
                ry = topmostTile;

                //check each tile underneath the tank for collision
                for (int x = leftmostTile; x < amountOfXTiles + leftmostTile; x++)
                {
                    for (int y = topmostTile; y < amountOfYTiles + topmostTile; y++)
                    {
                        if (wCol.GetTile(x, y))
                        {
                            resX = x;
                            resY = y;
                            Console.WriteLine("x"+x+";y"+y);
                            break;
                        }
                    }
                }

                //because the collision algorithm works with rectangles, but larger obstacles are made up by multiple rectangles with ogmo/otter, we have to find out how
                //big our rectangle actually is:
                int left = resX, right = resX + 1, top = resY, bottom = resY + 1;
                Console.WriteLine(wCol.GetTile(19,9));
                while (wCol.GetTile(left - 1, resY) && left > 1)
                    left--;
                while (wCol.GetTile(right, resY) && right < wCol.TileColumns)
                    right++;
                while (wCol.GetTile(resX, top - 1) && top > 1)
                    top--;
                while (wCol.GetTile(resX, bottom) && bottom < wCol.TileRows)
                    bottom++;

                Rectangle wallRectangle = new Rectangle(left * wCol.TileWidth, top * wCol.TileHeight, (right-left)*wCol.TileWidth, (bottom - top) * wCol.TileHeight);

                //actual collision reset code begins here
                
                //idea: find the shortest possible way to reset the tank position.
                //find out whether we are colliding from the right or from the left (top/bottom)
                float shorterXProjection = Math.Min(playerRight - wallRectangle.Left, wallRectangle.Right - playerLeft);
                float shorterYProjection = Math.Min(playerBottom - wallRectangle.Top, wallRectangle.Bottom - playerTop);

                //should we move on the x or the y axis? (eg what would be shorter)
                if (shorterYProjection > shorterXProjection)
                {
                    //reset by the shorter way
                    Tank.X += playerRight - wallRectangle.Left < wallRectangle.Right - playerLeft
                        ? -(playerRight - wallRectangle.Left)
                        : (wallRectangle.Right - playerLeft);
                }
                else
                {
                    //reset by the shorter way
                    Tank.Y += playerBottom - wallRectangle.Top < wallRectangle.Bottom - playerTop
                        ? -(playerBottom - wallRectangle.Top)
                        : (wallRectangle.Bottom - playerTop);
                }
            }
            else
            {
                Tank.WallCollision = false;
            }
            //if(!Tank.WallCollision)
            //    Console.WriteLine("-");
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
            //if(rx != -1 && ry != -1)
              //  Draw.Rectangle(rx * 32, ry * 32, 96, 96, Color.None, Color.Red, 5f);
        }
    }
}
