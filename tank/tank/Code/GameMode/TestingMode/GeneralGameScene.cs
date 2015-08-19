﻿using System;
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
            Scene.Add((new JoyControl(new SimpleTank(50, 50))));
            Scene.Add(new WasdControl(new SimpleTank(150, 150)));
        }
    }    
}
