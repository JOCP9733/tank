using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace tank.Code.Entities.Tank.Logics
{
    class LogicDecorator : ILogic
    {
        protected ILogic Logic;

        protected Game Game;
        protected Scene Scene;
        protected Input Input;
        protected Tank Tank;


        public LogicDecorator(ILogic pLogic)
        {
            Logic = pLogic;
            Game = ((ProtoLogic)pLogic).Game;
            Scene = ((ProtoLogic)pLogic).Scene;
            Input = ((ProtoLogic)pLogic).Input;
            Tank = ((ProtoLogic)pLogic).Tank;
        }

        public virtual void Update()
        {
            Logic.Update();
        }

        public virtual void Render()
        {
            Logic.Render();
        }
    }
}
