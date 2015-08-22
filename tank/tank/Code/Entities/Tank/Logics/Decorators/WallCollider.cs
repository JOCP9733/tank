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
        private static Entity _testingEntity;
        public WallCollider(ITankLogic pLogic) : base(pLogic)
        {
            _testingEntity = new Entity(0, 0, null, new BoxCollider(0, 0, CollidableTags.Tester));
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
                List<Rectangle> collidingRectangles = GetCollidingRectangles(wallGridCollider, leftmostTile, topmostTile);

                //filter out identical rectangles created when the tile checker found multiple colliding candidates
                collidingRectangles = collidingRectangles.Distinct().ToList();

                //reset out of all rectangles
                foreach (Rectangle obstacle in collidingRectangles)
                {
                    Vector2 projection = ShortestProjection(obstacle);
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

        /// <summary>
        /// Shortest way to reset the tank into non-offending area
        /// </summary>
        /// <param name="obstacle">the rectangle to avoid</param>
        /// <returns></returns>
        private Vector2 ShortestProjection(Rectangle obstacle)
        {
            //copy the player position to more legible variables
            var playerBottom = Tank.Collider.Bottom;
            var playerTop = Tank.Collider.Top;
            var playerRight = Tank.Collider.Right;
            var playerLeft = Tank.Collider.Left;

            //calculate values corresponding to collisions in certain directions
            float leftToRight = playerRight - obstacle.Left;
            float rightToLeft = obstacle.Right - playerLeft;
            float topToDown = playerBottom - obstacle.Top;
            float downToTop = obstacle.Bottom - playerTop;

            //find out whether we are goind right or left
            float shorterXProjection = Math.Min(leftToRight, rightToLeft);
            //find out whether we are going down or up
            float shorterYProjection = Math.Min(topToDown, downToTop);

            Vector2 bestProjection = new Vector2(0, 0);

            //should we move on the startX or the y axis? (eg what would be shorter)
            //also checks whether wo should move right or left / up or down
            if (shorterYProjection > shorterXProjection)
                bestProjection.X = leftToRight < rightToLeft ? -leftToRight : rightToLeft;
            else
                bestProjection.Y += topToDown < downToTop ? -topToDown : downToTop;

            //return the projection we calculated
            return bestProjection;
        }


        /// <summary>
        /// <para>List of rectangles we collide with.</para>
        /// <para>This function checks a specified area for collidable tiles to detect the exact position of the obstacle,
        /// because the gridcollider does not provide such information, and then calls <c>ExtendCollisionTile</c> to detect
        /// the complete obstacle (as opposed to only the tile we collide with)</para>
        /// </summary>
        /// <param name="collider">Collider to use</param>
        /// <param name="leftStart">left tile to start check at</param>
        /// <param name="topStart">top tile to start check at</param>
        /// <param name="checkWidth">how many tiles to check in x</param>
        /// <param name="checkHeight">how many tiles to check in y</param>
        /// <returns>List of rectangles we collide with</returns>
        private List<Rectangle> GetCollidingRectangles(GridCollider collider, int leftStart, int topStart, int checkWidth = 3, int checkHeight = 3)
        {
            //create list for collisions
            List<Rectangle> collisionList = new List<Rectangle>();

            //check each tile underneath the tank for collision
            for (int x = leftStart; x < checkWidth + leftStart; x++)
                for (int y = topStart; y < checkHeight + topStart; y++)
                    //if we found a tile that actually collides, save it to the list
                    if (collider.GetTile(x, y))
                    {
                        _testingEntity.Collider.SetPosition(x * collider.TileWidth, y * collider.TileHeight);
                        _testingEntity.Collider.Width = collider.TileWidth;
                        _testingEntity.Collider.Height = collider.TileHeight;
                        if(Tank.Collider.Collide(Tank.X, Tank.Y, _testingEntity) != null)
                            collisionList.Add(ExtendCollisionTile(collider, x, y));
                    }

            //return
            return collisionList;
        }

        /// <summary>
        /// This code tries to extend the collision tile to the largest possible rectangle (eg find the complete obstacle), 
        /// so that following algorithm parts
        /// can work with these rectangles to easily decide whether to move the offending entity on the x or the y axis.
        /// </summary>
        /// <param name="collider">The collider to work with, probably the grid collision collider.</param>
        /// <param name="startX">int x position of the tile in the grid (!= pixel position)</param>
        /// <param name="startY">int y position of the tile in the grid (!= pixel position)</param>
        /// <returns></returns>
        private Rectangle ExtendCollisionTile(GridCollider collider, int startX, int startY)
        {
            //copy the positions into variables that will be changed to calculate the resulting rectangle
            int left = startX, right = startX + 1, top = startY, bottom = startY + 1;

            //go as far in all directions as the tile is collidable
            while (collider.GetTile(left - 1, startY) && left > 1)
                left--;
            while (collider.GetTile(right, startY) && right < collider.TileColumns)
                right++;
            while (collider.GetTile(startX, top - 1) && top > 1)
                top--;
            while (collider.GetTile(startX, bottom) && bottom < collider.TileRows)
                bottom++;

            //create the rectangle the tank collides with
            return new Rectangle(left * collider.TileWidth, top * collider.TileHeight, (right - left) * collider.TileWidth, (bottom - top) * collider.TileHeight);
        }
    }
}
