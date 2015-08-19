using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank
{
    /// <summary>
    /// Decorator to handle Decorations of the tank or tanks
    /// exp:
    /// maintains a reference to a Component object and defines 
    /// an interface that conforms to Component's interface. 
    /// </summary>
    class TankDecorator : Tank
    {
        protected Tank tank;
        public TankDecorator(Tank t)
        {
            this.tank = t;
        }

        /// <summary>
        /// This method is called after the entity is added to a scene.
        /// </summary>
        public override void Added()
        {
            base.Added();
            //because the original tank is never added when using a decorator (instead the tankDecorator is added), 
            //otter can not run stuff the way it is supposed to. to avoid this, we add the tank given in the constructor
            //to the otter scene system manually.
            Scene.Add(tank);
        }

        public override void Update()
        {
            tank.Update();
        }

        public override void Render()
        {
            tank.Render();
        }

        public override void drive()
        {
            tank.drive();
        }

        public override void shoot()
        {
            tank.shoot();
        }

    }
}
