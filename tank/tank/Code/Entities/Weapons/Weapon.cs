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

        public void addDecorator(ProjectileDecorators k)
        {
            ProjectileKind.Add(k);
        }
        public IProjectile getProjectile(float x, float y, float degOrientation)
        {
            IProjectile tmp = new ProtoProjectile(x, y, degOrientation, _originTank,this);
            foreach(ProjectileDecorators a in ProjectileKind)
            {
                decorate(a, tmp);
            }
            return tmp;
        }

        private IProjectile decorate(ProjectileDecorators k, IProjectile basic)
        {
            switch (k)
            {
                case ProjectileDecorators.TestBullet:
                    return new TestBullet(basic);              
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
