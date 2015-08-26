using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Otter;

namespace tank.Code.GameMode
{
    class GameMode
    {
        public GameModes Mode;
        public Scene Scene;

        public GameMode(GameModes mode = GameModes.Testing)
        {
            Mode = mode;
            Scene = new Scene();
        }

        public static explicit operator GameMode(int v)
        {
            throw new NotImplementedException();
        }
    }
}
