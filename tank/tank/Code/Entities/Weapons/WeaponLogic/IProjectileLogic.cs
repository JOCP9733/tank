using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Weapons.WeaponLogic
{
    /// <summary>
    /// Interface for Projectiles
    /// </summary>
    interface IProjectileLogic
    {
        void Render();
        void Update();
        IProjectileLogic getUpperMost();
    }
}
