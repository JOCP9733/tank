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
    /// Static Type for the Tank class
    /// Explanation for Arne:
    /// defines the interface for objects that can have 
    /// responsibilities added to them dynamically. 
    /// </summary>
    abstract class Tank : Entity
    {
        protected Image _image;
        public float _speed = 4;
        protected float _rotationspeed = 2;
        protected Vector2 Direction = new Vector2(0f, 1f);

        /// <summary>
        /// abstract Method, which should be ovrwritten by 
        /// the Decorators
        /// </summary>
        abstract public void shoot();
        /// <summary>
        /// abstract Method, which should be ovrwritten by 
        /// the Decorators
        /// </summary>
        abstract public void drive();

        public override void Update()
        {
            base.Update();
            drive();
            shoot();

            Console.WriteLine(Input.ButtonPressed(0));
            
            //update direction vector for easy shooting
            Direction.X = (float)Math.Sin(Graphic.Angle * Util.DEG_TO_RAD) * _speed;
            Direction.Y = (float)Math.Cos(Graphic.Angle * Util.DEG_TO_RAD) * _speed;
            
        }

        /// <summary>
        /// This unconditionally fires a bullet, like "move_forward" unconditionally drives forward, which is
        /// the main contrast to shoot, which like "drive" has its own checks on whether to shoot or not. 
        /// TODO: this should be cleaned up, but i dont know where i would have to put this method regarding the decos
        /// </summary>
        public virtual void FireBullet()
        {
            Scene.Add(new Bullet(X, Y, Direction.X, Direction.Y, _image.Angle, Collider));
        }

        /// <summary>
        /// moves the tank forward
        /// </summary>
        public virtual void move_forward()
        {
            X -= (float)Math.Sin(_image.Angle * Util.DEG_TO_RAD) * _speed;
            Y -= (float)Math.Cos(_image.Angle * Util.DEG_TO_RAD) * _speed;
        }
        /// <summary>
        /// turns the Tank right
        /// </summary>
        public virtual void move_turn_left()
        {
            _image.Angle = _image.Angle + _rotationspeed;
        }
        /// <summary>
        /// turns the tank left
        /// </summary>
        public virtual void move_turn_right()
        {
            _image.Angle = _image.Angle - _rotationspeed;
        }
        /// <summary>
        /// moves the tank backwards
        /// </summary>
        public virtual void move_backwards()
        {
            X += (float)Math.Sin(_image.Angle * Util.DEG_TO_RAD) * _speed;
            Y += (float)Math.Cos(_image.Angle * Util.DEG_TO_RAD) * _speed;
        }
    }
}
