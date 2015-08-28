using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using tank.Code;
using tank.Code.GameMode;
using tank.Code.GameMode.LocalMultiplayer;
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

            //show a menu
            UiManager uiManager = new UiManager();
            //ui manager needs fully qualified path to the enum the menu is based on
            uiManager.ShowListMenu("gamemode?", "tank.Code.GameModes", OnSelectionCallback);
            uiManager.ShowToast("use arrow and enter\nkeys to navigate!", 3000);
            _game.Start(uiManager.Scene);
        }

        private static void OnSelectionCallback(int selection)
        {
            GameModes mode = (GameModes) selection;
            _game.RemoveScene();
            switch (mode)
            {
                case GameModes.Network:
                    //the following 3 lines are everything you need for a menu
                    UiManager uiManager = new UiManager();
                    uiManager.ShowListMenu("create server?", "tank.Code.YESORNOCHOOSENOW", ServerSelectionCallback);
                    _game.AddScene(uiManager.Scene);
                    break;
                case GameModes.Testing:
                    GameMode = new TestingMode();
                    _game.AddScene(GameMode.Scene);
                    break;
                case GameModes.LocalMultiplayer:
                    GameMode = new LocalMultiplayer();
                    _game.AddScene(GameMode.Scene);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        private static void ServerSelectionCallback(int selection) {
            _game.RemoveScene();
            GameMode = new NetworkMultiplayer(selection == 0);
            _game.AddScene(GameMode.Scene);
        }
    }
}