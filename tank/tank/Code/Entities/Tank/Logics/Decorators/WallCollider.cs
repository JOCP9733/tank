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

            //returns true if we collide witht the grid at any position
            if (Tank.Collider.Overlap(Tank.X, Tank.Y, CollidableTags.Wall))
            {
                //notify the tank
                Tank.WallCollision = true;

                //extract the wallCollider from the list of colliders with the tag wallcollider
                GridCollider wallGridCollider = (GridCollider)Tank.Collider.CollideList(Tank.X, Tank.Y, CollidableTags.Wall)[0];

                //simplify the player edge names
                var playerBottom = Tank.Collider.Bottom;
                var playerTop = Tank.Collider.Top;
                var playerRight = Tank.Collider.Right;
                var playerLeft = Tank.Collider.Left;

                //how many tiles are beneath the tank?
                //the calculation didnt work generously enough, so we just check 3x3 tiles every time
                int amountOfXTiles = 3;//(int) Math.Round(Math.Round(((PolygonCollider)Tank.Collider).Polygon.Width)/(double)wCol.TileWidth);
                int amountOfYTiles = 3;//(int) Math.Round(Math.Round(((PolygonCollider)Tank.Collider).Polygon.Height)/(double)wCol.TileHeight);

                //calculate colliding tiles check start position, eg the top-left-most tile the tank could hit
                int leftmostTile = wallGridCollider.GridX(playerLeft);
                int topmostTile = wallGridCollider.GridY(playerTop);

                //prepare space for the tile collision check, init with bad value
                int resX = -1, resY = -1;

                //check each tile underneath the tank for collision
                for (int x = leftmostTile; x < amountOfXTiles + leftmostTile; x++)
                {
                    for (int y = topmostTile; y < amountOfYTiles + topmostTile; y++)
                    {
                        //if we found a tile that actually collides, we can use that
                        if (wallGridCollider.GetTile(x, y))
                        {
                            //save result
                            resX = x;
                            resY = y;
                            //we just need the first one to collide
                            break;
                        }
                    }
                }

                //because the collision algorithm works with rectangles, but larger obstacles are made up by multiple colliders with ogmo/otter, we have to find out how
                //big our rectangle actually is:
                int left = resX, right = resX + 1, top = resY, bottom = resY + 1;

                //go as far in all directions as the tile is collidable
                while (wallGridCollider.GetTile(left - 1, resY) && left > 1)
                    left--;
                while (wallGridCollider.GetTile(right, resY) && right < wallGridCollider.TileColumns)
                    right++;
                while (wallGridCollider.GetTile(resX, top - 1) && top > 1)
                    top--;
                while (wallGridCollider.GetTile(resX, bottom) && bottom < wallGridCollider.TileRows)
                    bottom++;

                //create the rectangle the tank collides with
                Rectangle wallRectangle = new Rectangle(left * wallGridCollider.TileWidth, top * wallGridCollider.TileHeight, (right-left)*wallGridCollider.TileWidth, (bottom - top) * wallGridCollider.TileHeight);

                //actual collision reset code begins here
                 
                //idea: find the shortest possible way to reset the tank position.

                //save all collision values "comingfrom"
                float leftToRight = playerRight - wallRectangle.Left;
                float rightToLeft = wallRectangle.Right - playerLeft;
                float topToDown = playerBottom - wallRectangle.Top;
                float downToTop = wallRectangle.Bottom - playerTop;

                //find out whether we are goind right or left
                float shorterXProjection = Math.Min(leftToRight, rightToLeft);
                //find out whether we are going down or up
                float shorterYProjection = Math.Min(topToDown, downToTop);

                //should we move on the x or the y axis? (eg what would be shorter)
                if (shorterYProjection > shorterXProjection)
                {
                    //reset by the shorter way
                    Tank.X += leftToRight < rightToLeft ? -leftToRight : rightToLeft;
                }
                else
                {
                    //reset by the shorter way
                    Tank.Y += topToDown < downToTop ? -topToDown : downToTop;
                }
            }
            else
            {
                Tank.WallCollision = false;
            }
        }
    }
}
