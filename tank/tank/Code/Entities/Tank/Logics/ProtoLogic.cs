using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.Logics
{
    class ProtoLogic : ITankLogic
    {
        //make the game reference the tank game so we dont need to have the reference at start anymore
        public Game Game {get { return Tank.Game; } }
        public Scene Scene => Tank.Scene;
        public Input Input => Tank.Input;
        public Tank Tank;

        public ProtoLogic(Tank tank)
        {
            Tank = tank;
        }

        public void Update()
        {
        }

        public void Render()
        {
        }

        public ITankLogic getUpperMost()
        {
            return this;
        }
    }
}
