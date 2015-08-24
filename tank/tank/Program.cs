using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using tank.Code.GameMode;
using tank.Code.GameMode.NetworkMultiplayer;
using tank.Code.GameMode.TestingMode;

namespace tank
{
    class Program
    {
        public static GameMode GameMode;
        private static Game _game;

        static void Main(string[] args)
        {
            _game = new Game("tank",1280,720);
            _game.Color = Color.White;
            _game.MouseVisible = true;

            if(Console.ReadLine() == "s")
                GameMode = new NetworkSceneServerWithClient(2);
            else
                GameMode = new NetworkSceneClient();
            _game.Start(GameMode.Scene);
        }
    }
}