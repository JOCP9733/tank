using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.UI
{
    class Toast : Entity
    {
        private readonly Text _content;
        private readonly Image _background;
        private readonly int _duration;
        private Timer _dontCollectThisYet;

        public Color TextColor => _content.Color;

        public Toast(string content, int duration = 1000)
        {
            _duration = duration;

            _content = new Text(24)
            {
                String = content,
                Color = Color.Black,
                X = 50,
                Y = 50
            };

            _background = Image.CreateRectangle(_content.Width + 20, _content.Height + 20, Color.White);
            _background.X = _content.X - 10;
            _background.Y = _content.Y - 10;

            OnAdded += OnAdd;
        }

        private void OnAdd()
        {
            _dontCollectThisYet = new Timer(o => { RemoveSelf(); }, null, _duration, 0);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Render()
        {
            base.Render();
            _background.Render();
            _content.Render();
        }
    }
}
