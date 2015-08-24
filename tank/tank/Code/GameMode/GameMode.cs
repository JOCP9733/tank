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
        //network only, but i didnt want to add another subclass
        public NetPeer Peer;

        public GameMode(GameModes mode = GameModes.Testing)
        {
            Mode = mode;
            Scene = new Scene();
        }
    }
}
