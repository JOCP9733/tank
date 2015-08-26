using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.UI
{
    class UIManager : Scene
    {
        public UIManager()
        {

        }

        public void CreateListMenu(string enumName, ListMenu.OnSelection callback)
        {
            ListMenu m = new ListMenu(enumName, callback);
            Add(m);
        }
    }
}
