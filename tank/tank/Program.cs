using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using tank.Code.GameMode.TestingMode;

namespace tank
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game("tank");
            game.Color = Color.White;

            GeneralGameScene generalScene = new GeneralGameScene();

            game.Start(generalScene.Scene);
        }
    }
}