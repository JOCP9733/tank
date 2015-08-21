using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.Logics
{
    class LogicDecorator : ITankLogic
    {
        protected ITankLogic Logic;
        private ProtoLogic _uppermostLogic;

        protected Game Game => _uppermostLogic.Game;
        protected Scene Scene => _uppermostLogic.Scene;
        protected Input Input => _uppermostLogic.Input;
        protected Tank Tank;

        public LogicDecorator(ITankLogic pLogic)
        {
            Logic = pLogic;
            _uppermostLogic = (ProtoLogic) Logic.getUpperMost();
            Tank = _uppermostLogic.Tank;
        }

        public virtual void Update()
        {
            Logic.Update();
        }

        public virtual void Render()
        {
            Logic.Render();
        }
        public virtual ITankLogic getUpperMost()
        {
            return Logic.getUpperMost();
        }
    }
}
