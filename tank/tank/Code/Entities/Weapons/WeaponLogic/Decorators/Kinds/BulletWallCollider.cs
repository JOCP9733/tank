using System;
using Otter;

namespace tank.Code.Entities.Weapons.WeaponLogic.Decorators.Kinds
{
    class BulletWallCollider : ProjectileDecorator
    {
        public BulletWallCollider(IProjectileLogic pLogic) : base(pLogic)
        {
        }

        public override void Update()
        {
            base.Update();

            if (Projectile.Collider.Overlap(Projectile.X, Projectile.Y, CollidableTags.Wall))
            {
                Projectile.RemoveSelf();
            }
        }
    }
}