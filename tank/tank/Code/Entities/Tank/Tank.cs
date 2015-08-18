using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;


namespace tank.Code.Entities.Tank
{
    /// <summary>
    /// Static Type for the Tank class
    /// </summary>
    abstract class Tank : Entity
    {
        /// <summary>
        /// abstract Method, which should be ovrwritten by 
        /// the Decorators
        /// </summary>
        abstract public void shoot(float x, float y);
        /// <summary>
        /// abstract Method, which should be ovrwritten by 
        /// the Decorators
        /// </summary>
        abstract public void drive();

        public override void Update()
        {
            base.Update();
            drive();
        }
    }
}
