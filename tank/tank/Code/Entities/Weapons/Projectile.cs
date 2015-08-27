using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using tank.Code.Entities.Weapons.WeaponLogic;

namespace tank.Code.Entities.Weapons
{
    class Projectile : Entity
    {
        public readonly Tank.Tank OriginTank;
        public readonly float DegOrientation;

        public Weapon Weapon;
        public IProjectileLogic Logic;
        
        public Projectile(float x, float y, Tank.Tank originTank) : base(x, y)
        {
            DegOrientation = originTank.Rotation;
            OriginTank = originTank;
            Weapon = originTank.Weapon;

            //define this entity as part of the game pause group
            Group = (int)PauseGroups.NotMenu;
        }

        public override void Update()
        {
            base.Update();
            Logic.Update();
        }

        public override void Render()
        {
            base.Render();
            Logic.Render();
            Collider?.Render();
        }
    }
}
