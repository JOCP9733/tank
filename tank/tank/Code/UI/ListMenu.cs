using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Otter;

namespace tank.Code.UI
{
    /// <summary>
    /// Class to create a menu from an enum
    /// </summary>
    class ListMenu : Entity
    {
        private readonly bool[] _blockedKeys = new bool[3];
        private readonly List<MenuChoice> _choiceList = new List<MenuChoice>();
        private int _currentSelection;
        private readonly OnSelection _callback;
        private readonly Text _infoText;

        public delegate void OnSelection(int selected);
        
        /// <summary>
        /// Create a list menu
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="callback"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public ListMenu(string menuDescription, string enumName, OnSelection callback, int X = 500, int Y = 100, int width = 280, int height = 520) : base(X, Y, Image.CreateRectangle(width, height))
        {
            //enum magic
            Type enumType = Type.GetType(enumName);
            if (enumType == null)
                throw new ArgumentException("Specified enum type could not be found", nameof(enumName));

            //save callback
            _callback = callback;

            //set enter to blocked on begin to avoid double-entering menus
            _blockedKeys[2] = true;

            //set background
            Graphic.Color = Color.Gray;
            
            //draw a info text
            _infoText = new Text(menuDescription, 48)
            {
                X = X,
                Color = Color.Black,
                Y = Y - 50
            };

            //define this entity as part of the menu pause group
            Group = (int) PauseGroups.Menu;

            //how to draw a menu follows

            float paddingPercentage = 0.1f;

            //Magic.Cast(how many items are in the enum?)
            int numberOfEnumItems = Enum.GetNames(enumType).Length;
            
            int paddingHeight = (int) ((height / numberOfEnumItems) * paddingPercentage);
            int paddingWidth = (int) ((width / numberOfEnumItems) * paddingPercentage);

            //height per item
            int itemHeight = ((height - ((paddingHeight * (numberOfEnumItems + 1)))) / numberOfEnumItems);
            int itemWidth = width - paddingWidth * 2;

            //list of the items in the enum
            Array enumList = Enum.GetValues(enumType);//A wild magical unicorn appears
            
            //add menu item for each enum item
            for (int i = 0; i < enumList.Length; i++){
                MenuChoice newChoice = new MenuChoice(X + paddingWidth, 
                    Y + paddingHeight + (i * paddingHeight) + i * itemHeight, 
                    itemWidth, 
                    itemHeight, 
                    enumList.GetValue(i).ToString());
                _choiceList.Add(newChoice);
            }
            OnAdded += OnAdd;
        }

        private void OnAdd()
        {
            foreach (MenuChoice m in _choiceList)
                Scene.Add(m);
        }

        public override void Update()
        {
            base.Update();

            //check right key
            if (Input.KeyDown(Key.Down) && !_blockedKeys[0])
            {
                //block key
                _blockedKeys[0] = true;
                //wrap around the menu items
                _currentSelection = (_currentSelection + 1)%_choiceList.Count;
            }
            //reset keyblock
            if (_blockedKeys[0] && Input.KeyUp(Key.Down))
                _blockedKeys[0] = false;


            //check left key
            if (Input.KeyDown(Key.Up) && !_blockedKeys[1])
            {
                //block key
                _blockedKeys[1] = true;

                //avoid negative values
                if (_currentSelection == 0)
                    _currentSelection += _choiceList.Count;

                //wrap around the menu item positions
                _currentSelection = (_currentSelection - 1)%_choiceList.Count;
            }
            //reset down keyblock
            if (_blockedKeys[1] && Input.KeyUp(Key.Up))
                _blockedKeys[1] = false;


            //check enter key
            if (Input.KeyDown(Key.Return) && !_blockedKeys[2])
            {
                //block key
                _blockedKeys[2] = true;

                //call given callback
                _callback(_currentSelection);

                //delete this menu
                foreach (MenuChoice menuChoice in _choiceList)
                    menuChoice.RemoveSelf();
                RemoveSelf();
            }
            //reset keyblock
            if (_blockedKeys[2] && Input.KeyUp(Key.Return))
                _blockedKeys[2] = false;


            //update selection state for all
            for (int i = 0; i < _choiceList.Count; i++)
                _choiceList[i].SetSelectionState(i == _currentSelection);
        }

        public override void Render()
        {
            base.Render();
            _infoText.Render();
        }


        /// <summary>
        /// class representing a menu choice
        /// </summary>
        class MenuChoice : Entity
        {
            private readonly Text _choice;
            private readonly Color _unselectedColor, _selectedColor;
            /// <summary>
            /// Creates a new menuchoice at the given position with the given size with the default colors (green/red)
            /// </summary>
            public MenuChoice(int x, int y, int width, int height, string name) : this(x, y, width, height, name, Color.Black, Color.Green)
            {
            }

            /// <summary>
            /// Creates a new menuchoice at the given position with the given size with custom colors
            /// </summary>
            public MenuChoice(int x, int y, int width, int height, string name, Color unselected, Color selected) : base(x, y)
            {
                //define this entity as part of the menu pause group
                Group = (int)PauseGroups.Menu;

                //create text item
                _choice = new Text(name, 32);

                //position it
                _choice.X = x + (width - _choice.Width) / 2;
                _choice.Y = y + (height - _choice.Height) / 2;

                //create background
                Image background = Image.CreateRectangle(width, height, unselected);
                AddGraphic(background);

                //save colors
                _unselectedColor = unselected;
                _selectedColor = selected;
            }

            public void SetSelectionState(bool isSelected)
            {
                ((Image)Graphic).Color = isSelected ? _selectedColor : _unselectedColor;
            }

            public override void Render()
            {
                base.Render();
                _choice.Render();
            }
        }
    }

    
}
