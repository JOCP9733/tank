using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Otter;

namespace tank.Code.UI
{
    class UiManager
    {
        public readonly Scene Scene;
        private ListMenu.OnSelection _listMenuCallback;
        private TextDialog.OnAccept _textDialogCallback;

        public UiManager(Scene scene = null)
        {
            //lool -> if(scene == null){new Scene();} else {scene}
            Scene = scene ?? new Scene();
        }

        public void ShowListMenu(string menuDescription, string enumName, ListMenu.OnSelection callback)
        {
            //save callback
            _listMenuCallback = callback;
            //create a list menu, sneakily sneak in the callback that unpauses the game and calls the callback
            ListMenu m = new ListMenu(menuDescription, enumName, CallListMenuCallBack);
            //add menu to scene
            Scene.Add(m);
            //pause the rest game
            Scene.PauseGroup((int) PauseGroups.NotMenu);
        }

        public void ShowTextBox(string description, TextDialog.OnAccept callback)
        {
            //save callback
            _textDialogCallback = callback;
            //create a text box, sneakily sneak in the callback that unpauses the game and calls the callback
            TextDialog t = new TextDialog(CallTextDialogCallBack, description);
            //add menu to scene
            Scene.Add(t);
            //pause the rest game
            Scene.PauseGroup((int)PauseGroups.NotMenu);
        }

        /// <summary>
        /// this callback is set inbetween the actual callback and the menu, because we need to unpause the scene
        /// </summary>
        private void CallTextDialogCallBack(string result)
        {
            Scene.ResumeGroup((int) PauseGroups.NotMenu);
            _textDialogCallback(result);
        }

        /// <summary>
        /// this callback is set inbetween the actual callback and the menu, because we need to unpause the scene
        /// </summary>
        private void CallListMenuCallBack(int position)
        {
            Scene.ResumeGroup((int) PauseGroups.NotMenu);
            _listMenuCallback(position);
        }
    }
}
