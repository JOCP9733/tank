using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using tank.Code.Entities.Tank;
using tank.Code.Entities.Tank.SpecificDecorations;

namespace tank.Code.GameMode.TestingMode
{
    class GeneralGameScene
    {
        public Scene Scene;

        public GeneralGameScene()
        {
            Scene = new Scene();
            Scene.Add(new ArrowControll(new SimpleTank(50, 50)));
        }
    }
}
