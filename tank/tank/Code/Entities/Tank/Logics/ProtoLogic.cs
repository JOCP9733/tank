using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.Logics
{
    class ProtoLogic : ILogic
    {
        public Game Game;
        public Scene Scene;
        public Input Input;
        public Tank Tank;

        public ProtoLogic(Game game, Tank tank)
        {
            Game = game;
            Scene = game.Scene;
            Input = Scene.Input;
            Tank = tank;
        }

        public void Update()
        {
            
        }

        public void Render()
        {
        }
    }
}
