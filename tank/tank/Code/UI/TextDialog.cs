using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.UI
{
    class TextDialog : Entity
    {
        private readonly Text _text = new Text(24), _descriptionText;
        private Image _background;
        private readonly OnAccept _callback;
        private bool _blockEnter = true;

        public delegate void OnAccept(string enteredString);

        public TextDialog(OnAccept callback, string description)
        {
            _descriptionText = new Text(description, 32);
            _descriptionText.Color = Color.Black;
            OnAdded += OnAdd;
            _callback = callback;
            Group = (int) PauseGroups.Menu;
        }

        private void OnAdd()
        {
            //reset input
            Input.KeyString = "";

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
                string ip = Input.KeyString.Replace("\n", "");
                //try to parse an ip out of the entered string
                IPAddress address;
                if (IPAddress.TryParse(ip, out address))
                {
                    RemoveSelf();
                    _callback(ip);
                }
                else
                {
                    //remove enter from input string
                    Input.KeyString = ip;
                    //notify user that he did shit
                    Scene.Add(new Toast("ip no valid"));
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
