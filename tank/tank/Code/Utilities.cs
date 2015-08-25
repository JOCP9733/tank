using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using Otter;
using tank.Code.Entities.Tank;
using tank.Code.GameMode.NetworkMultiplayer;

namespace tank.Code
{
    /// <summary>
    /// This class should include practical methods, which are often needed.
    /// </summary>
    class Utilities
    {
        /// <summary>
        /// Return a polygon and set it as the collider polygon
        /// </summary>
        /// <param name="degree">Amount of rotation</param>
        /// <param name="collider">The (polygon!)collider to base the rotation on</param>
        /// <param name="basePolygon">An unmodified copy of the polygon</param>
        /// <returns></returns>
        public static Polygon RotatePolygon(float degree, Collider collider, Polygon basePolygon)
        {
            //cast collider to polygoncollider so we dont have to do the annoying casting on every call
            var polygonCollider = collider as PolygonCollider;
            //abort if the collider was not polygon
            if(polygonCollider == null)
                throw new ArgumentException("the collider must be a polygoncollider!");
            //copy the polygon
            var polygon = new Polygon(basePolygon);
            //rotate
            polygon.Rotate(degree, polygonCollider.OriginX, polygonCollider.OriginY);
            //update polygon
            polygonCollider.Polygon = polygon;
            //return the rotated polygon
            return polygon;
        }

        /// <summary>
        /// parses a string to an enum
        /// </summary>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }

    /// <summary>
    /// Class containing utilities useful for working with grid collision
    /// </summary>
    static class CollisionUtilites
    {
        /// <summary>
        /// This entity gets recycled to save resources when testing for actual collisions
        /// </summary>
        private static Entity _testingEntity;

        /// <summary>
        /// Shortest way to reset the tank into non-offending area
        /// </summary>
        /// <param name="resetThisEntity">Entity that you want out of the obstacle</param>
        /// <param name="obstacle">the rectangle to avoid</param>
        /// <returns></returns>
        public static Vector2 ShortestProjection(Entity resetThisEntity, Rectangle obstacle)
        {
            //copy the player position to more legible variables
            var playerBottom = resetThisEntity.Collider.Bottom;
            var playerTop = resetThisEntity.Collider.Top;
            var playerRight = resetThisEntity.Collider.Right;
            var playerLeft = resetThisEntity.Collider.Left;

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
        /// <param name="collidingEntity">Entity colliding with the Grid, for example a Tank</param>
        /// <param name="leftStart">left tile to start check at</param>
        /// <param name="topStart">top tile to start check at</param>
        /// <param name="checkWidth">how many tiles to check in x</param>
        /// <param name="checkHeight">how many tiles to check in y</param>
        /// <returns>List of rectangles we collide with</returns>
        public static List<Rectangle> GetCollidingRectangles(GridCollider collider, Entity collidingEntity, int leftStart, int topStart, int checkWidth = 3, int checkHeight = 3)
        {
            //create the test entity if it does not exist yet
            if(_testingEntity == null)
                _testingEntity = new Entity(0, 0, null, new BoxCollider(0, 0, CollidableTags.Tester));

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
                        if (collidingEntity.Collider.Collide(collidingEntity.X, collidingEntity.Y, _testingEntity) != null)
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
        public static Rectangle ExtendCollisionTile(GridCollider collider, int startX, int startY)
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





    enum CollidableTags
    {
        Bullet,
        Tank,
        Wall,
        Tester,
        PowerUp
    }

    enum Decorators
    {
        ControlArrow,
        ControlJoy,
        ControlWasd,
        ControlSimpleKi,
        SpeedUp,
        GetDamage,
        WallCollider,
        ControlNetworkHook,
        ControlNetwork,
        UsePowerUps
    }

    enum GameModes
    {
        Network,
        Testing
    }

    enum ProjectileDecorators
    {
        TestBullet,
        BulletWallCollider
    }

    enum PowerUps
    {
        SpeedUp
    }

    enum Maps
    {
        collisionTestBench,
        testlevel,
        networkTestBench
    }
}