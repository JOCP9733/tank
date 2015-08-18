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


        public SimpleTank(float xPos, float yPos):base()
        {
            Console.WriteLine("simpleTank");
            _image = new Image("Resources/tank.png");
            X = xPos;
            Y = yPos;
            _image.X = xPos;
            _image.Y = yPos;
            _image.CenterOrigin();
            AddGraphics(_image);
        }
        /// <summary>
        ///  Methode which is necessary to implement simple shooting
        /// </summary>
        public override void shoot(float x, float y)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Methode which is necessary to implement simple driving
        /// </summary>
        public override void drive()
        {
            if (Input.KeyDown(Key.W))
            {
                move_forward();
            }
            if (Input.KeyDown(Key.A))
            {
                move_turn_left();
            }
            if (Input.KeyDown(Key.S))
            {
                move_backwards();
            }
            if (Input.KeyDown(Key.D))
            {
                move_turn_right();
            }
            
        }

    }
}
