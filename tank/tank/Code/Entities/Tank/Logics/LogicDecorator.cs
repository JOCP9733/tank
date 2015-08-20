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

        protected Game Game;
        protected Scene Scene;
        protected Input Input;
        protected Tank Tank;

        public LogicDecorator(ITankLogic pLogic)
        {
            Logic = pLogic;
            ITankLogic start = Logic.getUpperMost();
            Game = ((ProtoLogic)start).Game;
            Scene = ((ProtoLogic)start).Scene;
            Input = ((ProtoLogic)start).Input;
            Tank = ((ProtoLogic)start).Tank;
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
