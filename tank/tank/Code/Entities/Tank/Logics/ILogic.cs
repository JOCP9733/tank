using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Tank.Logics
{
    interface ILogic
    {
        void Update();
        void Render();
        ILogic getUpperMost();
    }
}
