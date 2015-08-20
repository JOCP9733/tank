using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Weapons.WeaponLogic
{
    class ProtoProjectile :IProjectile
    {
        public Weapon Weapon;
        public float _degOrientation;
        public ProtoProjectile(float x, float y, float degOrientation, Tank.Tank originTank, Weapon w)
        {
            Weapon = w;
            _degOrientation = degOrientation;
        }

        public IProjectile getUpperMost()
        {
            return this;
        }

        public void Render()
        {
        }

        public void Update()
        { 
        }
    }
}
