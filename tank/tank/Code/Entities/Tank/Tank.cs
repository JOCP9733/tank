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
        protected Image _image ;
        protected float _speed = 4;
        protected float _rotationspeed = 2;
        protected Vector2 Direction = new Vector2(0f, 1f);

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

            //update direction vector for easy shooting
            Direction.X = (float)Math.Sin(_image.Angle * Util.DEG_TO_RAD) * _speed;
            Direction.Y = (float)Math.Cos(_image.Angle * Util.DEG_TO_RAD) * _speed;
        }

        /// <summary>
        /// moves the tank forward
        /// </summary>
        public void move_forward()
        {
            X -= (float)Math.Sin(_image.Angle * Util.DEG_TO_RAD) * _speed;
            Y -= (float)Math.Cos(_image.Angle * Util.DEG_TO_RAD) * _speed;
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
            X += (float)Math.Sin(_image.Angle * Util.DEG_TO_RAD) * _speed;
            Y += (float)Math.Cos(_image.Angle * Util.DEG_TO_RAD) * _speed;
        }
    }
}
