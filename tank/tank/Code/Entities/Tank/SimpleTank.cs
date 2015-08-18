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
        private Image _image =  new Image("Resources/tank.png");
        protected float _speed = 4;
        protected float _rotationspeed = 2;
        private Vector2 _direction = new Vector2(0f,1f);

        public SimpleTank(float xPos, float yPos)
        {
            _image.CenterOrigin();
            AddGraphics(_image);
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
        /// <summary>
        ///  Methode which is necessary to implement simple shooting
        /// </summary>
        public override void shoot(float x , float y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// moves the tank forward
        /// </summary>
        public void move_forward()
        {
            _image.X -= (float)Math.Sin(_image.Angle * Util.DEG_TO_RAD) * _speed;
            _image.Y -= (float)Math.Cos(_image.Angle * Util.DEG_TO_RAD) * _speed;
        }
        /// <summary>
        /// turns the Tank right
        /// </summary>
        public void move_turn_left()
        {
            _image.Angle = _image.Angle + _rotationspeed;
        }
        /// <summary>
        /// turns the tank left
        /// </summary>
        public void move_turn_right()
        {
            _image.Angle = _image.Angle - _rotationspeed;
        }
        /// <summary>
        /// moves the tank backwards
        /// </summary>
        public void move_backwards()
        {
            _image.X += (float)Math.Sin(_image.Angle * Util.DEG_TO_RAD) * _speed;
            _image.Y += (float)Math.Cos(_image.Angle * Util.DEG_TO_RAD) * _speed;
        }
    }
}
