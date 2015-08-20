using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tank.Code.Entities.Tank.Logics;

namespace tank.Code.Entities.Weapons.WeaponLogic
{
    class ProjectileDecorator : IProjectileLogic
    {
        protected IProjectileLogic ProjectileLogic;

        protected Game Game;
        protected Scene Scene;
        protected Input Input;
        protected Projectile Projectile;

        public ProjectileDecorator(IProjectileLogic p)
        {
            ProjectileLogic = p;
            ProtoProjectile tmp = (ProtoProjectile) ProjectileLogic.getUpperMost();

            Game = tmp.Game;
            Scene = tmp.Scene;
            Input = tmp.Input;
            Projectile = tmp.Projectile;
        }

        public IProjectileLogic getUpperMost()
        {
            return ProjectileLogic.getUpperMost();
        }

        public virtual void Render()
        {
            ProjectileLogic.Render();
        }

        public virtual void Update()
        {
            ProjectileLogic.Update();
        }
    }
}
