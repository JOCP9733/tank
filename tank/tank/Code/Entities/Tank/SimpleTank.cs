using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank
{
    /// <summary>
    /// Simple Tank, which should be initialized as a Tank with needed
    /// decorators
    /// </summary>
    class SimpleTank : Tank
    {
        private float _health = 100;
        protected float _speed = 4;

        public SimpleTank(float xPos, float yPos)
        {
            AddGraphics(new Image("/Resources/tank.png"));
        }
        /// <summary>
        /// Methode which is necessary to implement simple driving
        /// </summary>
        public override void drive()
        {
            if (Input.KeyDown(Key.W))
            {

            }
            if (Input.KeyDown(Key.A))
            {

            }
            if (Input.KeyDown(Key.S))
            {

            }
            if (Input.KeyDown(Key.D))
            {

            }

        }
        /// <summary>
        ///  Methode which is necessary to implement simple shooting
        /// </summary>
        public override void shoot(float x , float y)
        {
            throw new NotImplementedException();
        }
    }
}
