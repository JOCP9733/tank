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
        /// <param name="polygonCollider">The collider to base the rotation on</param>
        /// <param name="basePolygon">An unmodified copy of the polygon</param>
        /// <returns></returns>
        public static Polygon RotatePolygon(float degree, PolygonCollider polygonCollider, Polygon basePolygon)
        {
            var polygon = new Polygon(basePolygon);
            polygon.Rotate(degree, polygonCollider.OriginX, polygonCollider.OriginY);
            polygonCollider.Polygon = polygon;
            return polygon;
        }
    }

    enum CollidableTags
    {
        Bullet,
        Tank
    }

    enum Decorators
    {
        ControlArrow,
        ControlJoy,
        ControlWasd,
        ControlSimpleKi,
        SpeedUp,
        GetDamage
    }
    enum ProjectileDecorators
    {
        TestBullet
    }
}