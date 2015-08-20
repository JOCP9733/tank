using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using tank.Code.GameMode;
using tank.Code.GameMode.TestingMode;

namespace tank
{
    class Program
    {
        public static GameMode GameMode;

        static void Main(string[] args)
        {
            Game game = new Game("tank",1280,720);
            game.Color = Color.White;

            GameMode = new TestingMode();

            game.Start(GameMode.Scene);
        }
    }
}