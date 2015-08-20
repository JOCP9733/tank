using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Tank.Logics
{
    interface ITankLogic
    {
        void Update();
        void Render();
        ITankLogic getUpperMost();
    }
}
