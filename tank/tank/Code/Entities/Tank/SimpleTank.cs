using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using tank.Code.Entities.Weapons;

namespace tank.Code.Entities.Tank
{
    /// <summary>
    /// Simple Tank, which should be initialized as a Tank with needed
    /// decorators
    /// exp:
    /// defines an object to which additional responsibilities can be attached.
    /// </summary>
    class SimpleTank : Tank
    {
        private float _health = 100;


        public SimpleTank(float xPos, float yPos) : base()
        {
            Console.WriteLine("simpleTank");
            _image = new Image("Resources/tank.png");
            X = xPos;
            Y = yPos;
            //entity handles that
            //_image.X = xPos;
            //_image.Y = yPos;
            _image.CenterOrigin();
            AddGraphic(_image);
        }

        /// <summary>
        /// modified to do nothing so the decorators have to do everything, but the simpletank does not fuck shit up
        /// </summary>
        public override void drive(){}

        /// <summary>
        /// modified to do nothing so the decorators have to do everything, but the simpletank does not fuck shit up
        /// </summary>
        public override void shoot(){}
    }
}
