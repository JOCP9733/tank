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

                //calculate colliding tiles check start position, eg the top-left-most tile the tank could hit
                int leftmostTile = wallGridCollider.GridX(Tank.Collider.Left);
                int topmostTile = wallGridCollider.GridY(Tank.Collider.Top);

                //get the collision rectangles
                List<Rectangle> collidingRectangles = CollisionUtilites.GetCollidingRectangles(wallGridCollider, Tank, leftmostTile, topmostTile);

                //filter out identical rectangles created when the tile checker found multiple colliding candidates
                collidingRectangles = collidingRectangles.Distinct().ToList();

                //reset out of all rectangles
                foreach (Rectangle obstacle in collidingRectangles)
                {
                    Vector2 projection = CollisionUtilites.ShortestProjection(Tank, obstacle);
                    //something smells fishy: the projection is larger than a complete tile? fuck that.
                    if(Math.Abs(projection.X) < wallGridCollider.TileWidth && Math.Abs(projection.Y) < wallGridCollider.TileHeight)
                        Tank.AddPosition(projection);
                }
            }
            else
            {
                Tank.WallCollision = false;
            }
        }
    }
}
