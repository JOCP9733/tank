using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Weapons.WeaponLogic
{
    class ProjectileDecorator : IProjectile
    {
        public Vector2 _direction;
        public readonly Tank.Tank _originTank;
        public readonly float _degOrientation;
        public Weapon _weapon;

        IProjectile Projectile;
        public ProjectileDecorator(IProjectile p)
        {
            Projectile = p;
            ProtoProjectile tmp = (ProtoProjectile)p.getUpperMost();
            _originTank = tmp.Weapon._originTank;
            _degOrientation = tmp._degOrientation;
            _weapon = tmp.Weapon;
        }
        public IProjectile getUpperMost()
        {
            return Projectile.getUpperMost();
        }

        public virtual void Render()
        {
            Projectile.Render();
        }

        public virtual void Update()
        {
            Projectile.Update();
        }
    }
}
