using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using tank.Code;
using tank.Code.GameMode;
using tank.Code.GameMode.NetworkMultiplayer;
using tank.Code.GameMode.TestingMode;
using tank.Code.UI;

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

            //if s, then create a server
            //uncomment to test network stuff
            //

            //show a menu
            UIManager uiManager = new UIManager();
            //ui manager needs fully qualified path to the enum
            uiManager.CreateListMenu("tank.Code.GameModes", OnSelectionCallback);
            _game.Start(uiManager);
        }

        private static void OnSelectionCallback(int selection)
        {
            GameModes mode = (GameModes) selection;
            _game.RemoveScene();
            switch (mode)
            {
                case GameModes.Network:
                    Console.WriteLine("press enter for client, tap s and press enter for server");
                    GameMode = new NetworkSceneClient(Console.ReadLine() == "s");
                    break;
                case GameModes.Testing:
                    GameMode = new TestingMode();
                    break;
            }
            
            _game.AddScene(GameMode.Scene);
        }
    }
}