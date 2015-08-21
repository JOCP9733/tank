using System;
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
        public ITankLogic Logic;
        public Weapon Weapon;

        public float Rotation => _image.Angle;

        protected Image _image;
        public float _speed = 4;
        protected float _rotationspeed = 2;
        public Vector2 Direction = new Vector2(0f, 1f);

        public bool WallCollision;

        public Tank(float xPos, float yPos) : base(xPos, yPos)
        {
            Console.WriteLine("simpleTank");
            _image = new Image("Resources/tank.png");
            _image.CenterOrigin();
            AddGraphic(_image);
            Weapon = new Weapon(this);
            Weapon.addDecorator(ProjectileDecorators.TestBullet);
        }

        public override void Render()
        {
            base.Render();
            Collider?.Render();
            Logic.Render();
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

            switch (deco)
            {
                case Decorators.ControlArrow:
                    Logic = new ControlArrow(Logic);
                    break;
                case Decorators.ControlJoy:
                    Logic = new ControlJoy(Logic);
                    break;
                case Decorators.ControlSimpleKi:
                    Logic = new ControlSimpleKI(Logic);
                    break;
                case Decorators.ControlWasd:
                    Logic = new ControlWasd(Logic);
                    break;
                case Decorators.GetDamage:
                    Logic = new GetDamage(Logic);
                    break;
                case Decorators.SpeedUp:
                    Logic = new SpeedUp(Logic);
                    break;
                case Decorators.WallCollider:
                    Logic = new WallCollider(Logic);
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
            //deny if an obstacle is in front of the tank
            //if (WallCollision && WallCollisionDirection == MovementDirection)
            //    return;
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
            //deny if an obstacle is behind the tank
            //if (WallCollision && WallCollisionDirection == MovementDirection)
            //    return;
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
            Scene.Add(Weapon.getProjectile(X, Y, Rotation));
            //Scene.Add(new Bullet(X, Y, _image.Angle, this));
        }
    }
}
