using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

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
    }

    enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        Crap
    }
    enum CollidableTags
    {
        Bullet,
        Tank,
        Wall
    }

    enum Decorators
    {
        ControlArrow,
        ControlJoy,
        ControlWasd,
        ControlSimpleKi,
        SpeedUp,
        GetDamage,
        WallCollider
    }

    enum GameModes
    {
        Network,
        Testing
    }
    enum ProjectileDecorators
    {
        TestBullet
    }
}