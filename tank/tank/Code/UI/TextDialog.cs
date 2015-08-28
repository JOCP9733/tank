using System;
using Otter;

namespace tank.Code.UI
{
    class TextDialog : Entity
    {
        private readonly Text _text = new Text(24), _descriptionText;
        private Image _background;
        private readonly OnAccept _callback;
        private bool _blockEnter = true;
        private readonly Predicate<string> _valueAcceptancePredicate;
        private string _startingInputValue;

        public delegate void OnAccept(string enteredString);
        
        public TextDialog(OnAccept callback, string description, Predicate<string> predicate, string startingValue)
        {
            //save the predicate that is used to determine whether the result is acceptable
            _valueAcceptancePredicate = predicate;
            //init the description
            _descriptionText = new Text(description, 32) {Color = Color.Black};
            //register callback for after having been added to a scene
            OnAdded += OnAdd;
            //save callback
            _callback = callback;
            //set group to avoid pausing this
            Group = (int) PauseGroups.Menu;
            //reset input
            _startingInputValue = startingValue;
        }

        private void OnAdd()
        {
            //reset keystring
            Input.KeyString = _startingInputValue;
            
            //set text position
            _text.X = Scene.Width*0.2f;
            _text.Y = Scene.Height*0.45f;
            _text.Color = Color.Black;

            //set background size
            _background = Image.CreateRectangle((int) (Scene.Width * 0.6f), (int) (Scene.Height * 0.1f), Color.Grey);
            //set background position
            _background.X = _text.X;
            _background.Y = _text.Y;

            //set description position
            _descriptionText.X = _background.X;
            _descriptionText.Y = _background.Y - _background.Height;
        }

        public override void Update()
        {
            base.Update();
            _text.String = Input.KeyString;

            if (_blockEnter && Input.KeyUp(Key.Return))
                _blockEnter = false;

            if (!_blockEnter && Input.KeyDown(Key.Return))
            {
                //block the enter key
                _blockEnter = true;
                //remove enters from ip string
                string value = Input.KeyString.Replace("\n", "");
                //try to parse an ip out of the entered string
                if (_valueAcceptancePredicate(value))
                {
                    RemoveSelf();
                    _callback(value);
                }
                else
                {
                    //remove enter from input string
                    Input.KeyString = value;
                    //notify user that he did shit
                    Scene.Add(new Toast("Sorry, that doesn't\nseem to fit the expected type!", 3000));
                }
            }
        }

        public override void Render()
        {
            base.Render();
            _background.Render();
            _descriptionText.Render();
            _text.Render();
        }
    }
}
