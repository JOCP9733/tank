using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using tank.Code.Entities.Tank;
using tank.Code.Entities.Tank.Logics;
using tank.Code.Entities.Tank.Logics.Decorators;

namespace tank.Code.GameMode.TestingMode
{
    class TestingMode : GameMode
    {
        Tank tank = new Tank(50, 50);
        Tank tank2 = new Tank(100, 50);

        public TestingMode()
        {
            Scene.Add(tank);
            Scene.Add(tank2);

            //try to load a project
            OgmoProject proj = new OgmoProject("Resources/Maps/test.oep");

            //uuh this somehow "registers a collision tag"
            proj.RegisterTag(CollidableTags.Wall, "CollisionLayer");

            //try to load a level into "Scene"
            proj.LoadLevel("Resources/Maps/testlevel.oel", Scene);

            Scene.OnBegin = initialise;
        }

        private void initialise()
        {
            tank.AddDecorator(Decorators.ControlWasd);
            tank.AddDecorator(Decorators.GetDamage);
            tank.AddDecorator(Decorators.WallCollider);

            tank2.AddDecorator(Decorators.GetDamage);
            tank2.AddDecorator(Decorators.ControlSimpleKi);
            tank2.AddDecorator(Decorators.SpeedUp);
        }
    }    
}
