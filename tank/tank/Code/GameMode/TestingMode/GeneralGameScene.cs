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
    class GeneralGameScene
    {
        public Scene Scene;
        Tank tank = new Tank(50, 50);

        public GeneralGameScene()
        {
            Scene = new Scene();
            Scene.Add(tank);
            Scene.OnBegin = initialise;
        }

        private void initialise()
        {
            tank.AddDecorator(Decorators.ControlJoy);
        }
    }    
}
