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

        public void move_forward()
        {
            _image.X += (float)Math.Cos(_image.Angle) * _speed;
            _image.Y += (float)Math.Sin(_image.Angle) * _speed;
            //_image.X = _image.X - _direction.X * _speed;
            //_image.Y = _image.Y - _direction.Y * _speed;

        }
        public void move_turn_left()
        {
            Console.WriteLine("Angle :"+_image.Angle);
            _image.Angle = _image.Angle + _rotationspeed;
            Console.WriteLine("Angle updated:" + _image.Angle);


            //float g = _rotationspeed;
            //Console.WriteLine("_direction before :" + _direction);
            //_direction = new Vector2((float)(Math.Cos(g) * _direction.X -Math.Sin(g) * _direction.Y), (float)(_direction.Y *Math.Cos(g) + _direction.X * Math.Sin(g)));
            //Console.WriteLine("_direction after :" + _direction);
            //_direction.Normalize();
        }
        public void move_turn_right()
        {
            Console.WriteLine("Angle :" + _image.Angle);
            _image.Angle = _image.Angle - _rotationspeed;
            Console.WriteLine("Angle updated:" + _image.Angle);
            //float g = -_rotationspeed;
            //Console.WriteLine("_direction before :" + _direction);
            //_direction = new Vector2((float)(Math.Cos(g) * _direction.X - Math.Sin(g) * _direction.Y), (float)(_direction.Y * Math.Cos(g) + _direction.X * Math.Sin(g)));
            //Console.WriteLine("_direction after :" + _direction);
            //_direction.Normalize();

        }
        public void move_backwards()
        {
            _image.X -= (float)Math.Cos(_image.Angle) * _speed;
            _image.Y -= (float)Math.Sin(_image.Angle) * _speed;
            //_image.X = _image.X + _direction.X * _speed;
            //_image.Y = _image.Y + _direction.Y * _speed;
        }
    }
}
