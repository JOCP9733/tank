using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tank.Code.Entities.Weapons.WeaponLogic;
using tank.Code.Entities.Weapons.WeaponLogic.Decorators.Kinds;

namespace tank.Code.Entities.Weapons
{
    class Weapon : Entity
    {
        public static Texture _texture;
        public readonly Tank.Tank _originTank;

        List<ProjectileDecorators> ProjectileKind;

        public Weapon(Tank.Tank t)
        {
            ProjectileKind = new List<ProjectileDecorators>();
            _originTank = t;
        }

        /// <summary>
        /// Add a decorator to all following projectiles
        /// </summary>
        /// <param name="k"></param>
        public void addDecorator(ProjectileDecorators k)
        {
            ProjectileKind.Add(k);
        }

        public Projectile getProjectile(float x, float y, float degOrientation)
        {
            //get a new projectile
            Projectile p = new Projectile(x, y, _originTank);

            //create logic for the projectile
            IProjectileLogic tmp = new ProtoProjectile(_originTank.Game, p);

            foreach(ProjectileDecorators a in ProjectileKind)
            {
                decorate(a, tmp);
            }

            p.Logic = tmp;
            return p;
        }

        private IProjectileLogic decorate(ProjectileDecorators decoType, IProjectileLogic inner)
        {
            switch (decoType)
            {
                case ProjectileDecorators.TestBullet:
                    return new TestBullet(inner);              
                default:
                    throw new NotImplementedException();
            }
        }
    }
}