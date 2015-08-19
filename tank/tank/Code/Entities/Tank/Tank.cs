﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using tank.Code.Entities.Tank.Logics;
using tank.Code.Entities.Tank.Logics.Decorators;
using tank.Code.Entities.Weapons;

namespace tank.Code.Entities.Tank
{
    /// <summary>
    /// Simple Tank, which should be initialized as a Tank with needed
    /// decorators
    /// exp:
    /// defines an object to which additional responsibilities can be attached.
    /// </summary>
    class Tank : Entity
    {
        private float _health = 100;
        public ILogic Logic;

        protected Image _image;
        public float _speed = 4;
        protected float _rotationspeed = 2;
        public Vector2 Direction = new Vector2(0f, 1f);

        public Tank(float xPos, float yPos) : base()
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

        public override void Added()
        {
            base.Added();
            Logic = new ProtoLogic(Game, this);
        }

        public void AddDecorator(Decorators deco)
        {
            if(Game == null)
                throw new ArgumentNullException("emtpy game");

            ProtoLogic protoLogic = (ProtoLogic) Logic;
            switch (deco)
            {
                case Decorators.ControlArrow:
                    Logic = new ControlArrow(protoLogic);
                    break;
                case Decorators.ControlJoy:
                    Logic = new ControlJoy(protoLogic);
                    break;
                case Decorators.ControlSimpleKi:
                    Logic = new ControlSimpleKI(protoLogic);
                    break;
                case Decorators.ControlWasd:
                    Logic = new ControlWasd(protoLogic);
                    break;
                case Decorators.GetDamage:
                    Logic = new GetDamage(protoLogic);
                    break;
                case Decorators.SpeedUp:
                    Logic = new SpeedUp(protoLogic);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deco), deco, null);
            }
        }

        public override void Update()
        {
            base.Update();
            Logic.Update();

            //update direction vector for easy shooting
            Direction.X = (float)Math.Sin(Graphic.Angle * Util.DEG_TO_RAD) * _speed;
            Direction.Y = (float)Math.Cos(Graphic.Angle * Util.DEG_TO_RAD) * _speed;
        }

        /// <summary>
        /// moves the tank forward
        /// <param name="factor">had to add a factor for the joystick. if not given, full speed is used.</param>
        /// </summary>
        public virtual void move_forward(float factor = 1f)
        {
            X -= (float)Math.Sin(_image.Angle * Util.DEG_TO_RAD) * _speed * factor;
            Y -= (float)Math.Cos(_image.Angle * Util.DEG_TO_RAD) * _speed * factor;
        }

        /// <summary>
        /// turns the Tank right
        /// </summary>
        /// <param name="factor">had to add a factor for the joystick. if not given, full speed is used.</param>
        public virtual void move_turn_left(float factor = 1f)
        {
            _image.Angle = _image.Angle + _rotationspeed * factor;
        }

        /// <summary>
        /// turns the tank left
        /// </summary>
        /// <param name="factor">had to add a factor for the joystick. if not given, full speed is used.</param>
        public virtual void move_turn_right(float factor = 1f)
        {
            _image.Angle = _image.Angle - _rotationspeed * factor;
        }

        /// <summary>
        /// moves the tank backwards
        /// </summary>
        /// <param name="factor">had to add a factor for the joystick. if not given, full speed is used.</param>
        public virtual void move_backwards(float factor = 1f)
        {
            X += (float)Math.Sin(_image.Angle * Util.DEG_TO_RAD) * _speed * factor;
            Y += (float)Math.Cos(_image.Angle * Util.DEG_TO_RAD) * _speed * factor;
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
    }
}
