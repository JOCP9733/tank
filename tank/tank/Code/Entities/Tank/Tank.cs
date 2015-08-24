using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
        /// <summary>
        /// The logic powering this tank
        /// </summary>
        public ITankLogic Logic;

        /// <summary>
        /// Current weapon, e.g. Projectile generator
        /// </summary>
        public Weapon Weapon;

        /// <summary>
        /// Current direction vector
        /// </summary>
        public Vector2 Direction = new Vector2(0f, 1f);
        
        /// <summary>
        /// Current rotation of tank image
        /// </summary>
        public float Rotation => _image.Angle;

        /// <summary>
        /// Uniquely identify tank for server and client; -1 when no id is given
        /// </summary>
        public int NetworkId;

        /// <summary>
        /// Decorator that notifies the network code of actions
        /// </summary>
        public ControlNetworkHook NetworkHook;

        protected Image _image;
        public float _speed = 4;
        protected float _rotationspeed = 2;
        private float _health = 100;

        /// <summary>
        /// true when a collision occurs
        /// </summary>
        public bool WallCollision;

        public Tank(float xPos, float yPos) : base(xPos, yPos)
        {
            Console.WriteLine("simpleTank");
            _image = new Image("Resources/tank.png");
            _image.CenterOrigin();
            AddGraphic(_image);
            Weapon = new Weapon(this);
            Weapon.addDecorator(ProjectileDecorators.TestBullet);
            Weapon.addDecorator(ProjectileDecorators.BulletWallCollider);

            //logic can now be instantiated here already
            Logic = new ProtoLogic(this);
        }

        public override void Render()
        {
            base.Render();
            Collider?.Render();
            Logic.Render();
        }

        public void AddDecorator(Decorators deco)
        {
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
                case Decorators.ControlNetwork:
                    Logic = new ControlNetwork(Logic);
                    break;
                case Decorators.ControlNetworkHook:
                    NetworkHook = new ControlNetworkHook(Logic);
                    Logic = NetworkHook;
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
        /// This function gets called by the ogmo level loader
        /// </summary>
        public static void MyCreateEntity(Scene scene, XmlAttributeCollection ogmoParameters)
        {
            //ok ogmo gives us the position in x and y, and the list of decorators in DecoratorList
            int x = ogmoParameters.Int("x", -1);
            int y = ogmoParameters.Int("y", -1);
            
            //this is how you read a string from ogmo
            string decoList = ogmoParameters.GetNamedItem("DecoratorList").Value;

            //create an instance
            Tank t = new Tank(x, y);

            //try and load an id; if there is none, the id will be -1
            t.NetworkId = ogmoParameters.Int("NetworkId", -1);

            //add to the scene because the decorators need that
            scene.Add(t);

            //split the decorators; they are seperated by a ":" in the DecoratorList string
            string[] decoArray = decoList.Split(':');

            //add each decorator
            foreach (string decoratorName in decoArray)
            {
                //parse the name of the decorator to the decorator-enum, and then add to the tank
                t.AddDecorator((Decorators) Enum.Parse(typeof(Decorators), decoratorName));
            }
        }

        /// <summary>
        /// moves the tank forward
        /// <param name="factor">had to add a factor for the joystick. if not given, full speed is used.</param>
        /// </summary>
        public virtual void move_forward(float factor = 1f)
        {
            //notify server if existing
            NetworkHook?.OnForward();
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
            //notify server if existing
            NetworkHook?.OnLeft();
            _image.Angle = _image.Angle + _rotationspeed * factor;
        }

        /// <summary>
        /// turns the tank left
        /// </summary>
        /// <param name="factor">had to add a factor for the joystick. if not given, full speed is used.</param>
        public virtual void move_turn_right(float factor = 1f)
        {
            //notify server if existing
            NetworkHook?.OnRight();
            _image.Angle = _image.Angle - _rotationspeed * factor;
        }

        /// <summary>
        /// moves the tank backwards
        /// </summary>
        /// <param name="factor">had to add a factor for the joystick. if not given, full speed is used.</param>
        public virtual void move_backwards(float factor = 1f)
        {
            //notify server if existing
            NetworkHook?.OnReward();
            //deny if an obstacle is behind the tank
            //if (WallCollision && WallCollisionDirection == MovementDirection)
            //    return;
            X += (float)Math.Sin(_image.Angle * Util.DEG_TO_RAD) * _speed * factor;
            Y += (float)Math.Cos(_image.Angle * Util.DEG_TO_RAD) * _speed * factor;
        }

        /// <summary>
        /// This unconditionally fires a bullet, like "move_forward" unconditionally drives forward, which is
        /// the main contrast to shoot, which like "drive" has its own checks on whether to shoot or not. 
        /// </summary>
        public virtual void FireBullet()
        {
            //notify server if existing
            NetworkHook?.OnFire();
            Scene.Add(Weapon.getProjectile(X, Y, Rotation));
        }
    }
}
