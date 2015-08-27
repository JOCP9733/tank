using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Otter;
using tank.Code.GameMode.NetworkMultiplayer;

namespace tank.Code.Entities.Tank.Logics.Decorators
{
    /// <summary>
    /// Decorator to notify the network code of manual move commands
    /// </summary>
    class ControlNetworkHook : LogicDecorator
    {
        private readonly NetClient _client;

        public ControlNetworkHook(ITankLogic pLogic) : base(pLogic)
        {
            if(Program.GameMode.Mode != GameModes.Network)
                throw new NotSupportedException("The gamemode must be network multiplayer for this to work!");

            //at this point we are fucked if the gamemode isnt a client
            if (Program.GameMode is NetworkSceneClient)
                _client = ((NetworkSceneClient) Program.GameMode).Client;
        }

        private void SendNotification(NetworkAction whatNetworkAction)
        {
            //we need a second peer for this to send a message to the server itself
            NetOutgoingMessage message = _client.CreateMessage();
            message.Write((byte) NetworkMessageType.TankControl);
            message.Write(Tank.NetworkId, 32);
            message.Write((byte)whatNetworkAction);
            _client.SendMessage(message, _client.ServerConnection, NetDeliveryMethod.ReliableUnordered, 0);
        }
        
        public void OnForward()
        {
            SendNotification(NetworkAction.Forward);
        }

        public void OnReward()
        {
            SendNotification(NetworkAction.Reward);
        }

        public void OnLeft()
        {
            SendNotification(NetworkAction.Left);
        }

        public void OnRight()
        {
            SendNotification(NetworkAction.Right);
        }

        public void OnFire()
        {
            SendNotification(NetworkAction.Fire);
        }
    }
}
