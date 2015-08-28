using System;
using System.Collections.Generic;
using System.Linq;
using Otter;
using tank.Code.Entities.Tank;
using tank.Code.UI;

namespace tank.Code.GameMode.LocalMultiplayer
{
    internal class LocalMultiplayer : GameMode
    {
        /// <summary>
        /// otter calls this to create entities
        /// </summary>
        private static readonly string _creationMethodName = "MyCreateEntity";

        private int _playerCount = 2;
        private bool _powerUpsEnabled = true;
        private Maps _mapToLoad;
        private Decorators[] _controlList = new Decorators[42];

        private readonly UiManager _menu;

        public LocalMultiplayer()
        {
            //build a game settings menu
            _menu = new UiManager(Scene);
            showMainMenu();
        }

        private void showMainMenu()
        {
            _menu.ShowListMenu("Game settings", "tank.Code.GameMode.LocalMultiplayer.GameSettings", SettingsCallback);
        }

        private void SettingsCallback(int selected)
        {
            switch ((GameSettings) selected)
            {
                case GameSettings.PlayerCount:
                    _menu.ShowTextBox("enter player count", enteredString =>
                    {
                        _playerCount = Convert.ToInt32(enteredString);
                        showMainMenu();
                    },
                    currentString =>
                    {
                        //try to parse to int, only allow if int and larger than 0
                        try
                        {
                            int result = Convert.ToInt32(currentString);
                            if (result < 1)
                                return false;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                        return true;
                    },
                    _playerCount.ToString());
                    break;
                case GameSettings.InputSelection:
                    List<string> playerList = new List<string>(_playerCount);
                    for(int i = 0; i < _playerCount; i++)
                        playerList.Add("Player "+i);
                    playerList.Add("Go back");
                    _menu.ShowListMenu("What player to change?", playerList, playerIndex =>
                    {
                        //go back
                        if (playerIndex == _playerCount)
                        {
                            showMainMenu();
                        }
                        //choose a player control
                        else
                        {
                            _menu.ShowListMenu("choose control", "tank.Code.Decorators", selected1 =>
                            {
                                //chose a player control, return to selection screen
                                _controlList[playerIndex] = (Decorators) selected1;
                                SettingsCallback((int) GameSettings.InputSelection);
                            });
                        }
                    });
                    break;
                case GameSettings.Map:
                    _menu.ShowListMenu("Map?", "tank.Code.Maps", selection =>
                    {
                        _mapToLoad = (Maps) selection;
                        showMainMenu();
                    });
                    break;
                case GameSettings.PowerUps:
                    _menu.ShowListMenu("PowerUps enabled?", "tank.Code.YESORNOCHOOSENOW", i =>
                    {
                        _powerUpsEnabled = i == 0;//yes is 0 in enum
                        showMainMenu();
                    });
                    break;
                case GameSettings.Start:
                    LoadMap(_mapToLoad);
                    Scene.OnEmptyEntitiesToAdd += OnMapLoaded;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selected), selected, null);
            }
        }

        private void OnMapLoaded()
        {
            //activate tank control
            List<Tank> tankList = Scene.GetEntities<Tank>();
            //add corresponding control decorators
            for (int i = 0; i < _playerCount; i++)
            {
                tankList[i].AddDecorator(_controlList[i]);
            }
            //remove tanks that are superfluous because the player wanted less
            for (int i = _playerCount; i < tankList.Count; i++)
                tankList[i].RemoveSelf();
            //dont call me again
            Scene.OnEmptyEntitiesToAdd -= OnMapLoaded;
            //ever
        }

        private void LoadMap(Maps map)
        {
            //tank entity creation was moved to ogmo; see testlevel.oep for adding a decorator to your tank.
            //try to load a project
            OgmoProject proj = new OgmoProject("Resources/Maps/test.oep", "Resources/Maps/");

            //register our function to call for creating entities
            //this just is a string with the method name, the method itself has to be in the entities (see tank)
            proj.CreationMethodName = _creationMethodName;

            //uuh this somehow "registers a collision tag"
            proj.RegisterTag(CollidableTags.Wall, "CollisionLayer");

            //try to load a level into "Scene"; map gets auto-ToString() to enum position name
            proj.LoadLevel("Resources/Maps/" + map + ".oel", Scene);
        }
    }

    enum GameSettings
    {
        PlayerCount,
        InputSelection,
        Map,
        PowerUps,
        Start
    }
}
