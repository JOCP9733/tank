using System;
using Lidgren.Network;
using Otter;
using tank.Code.Entities.Tank;
using tank.Code.UI;

namespace tank.Code.GameMode.NetworkMultiplayer
{
    class NetworkSceneClient : GameMode
    {
        /// <summary>
        /// Lidgren client
        /// </summary>
        public NetClient Client;

        /// <summary>
        /// called on data receival
        /// </summary>
        public event NetworkEvent OnClientData;

        /// <summary>
        /// If this client "is" the server, load a server. null otherwise
        /// </summary>
        public GameServer GameServer;

        /// <summary>
        /// Because Otter calls the OnEmptyEntitiesToAdd whenever it has no entities
        /// left to add, we need to check whether we already have sent our load finish signal
        /// </summary>
        private bool _loadFinishSent;

        /// <summary>
        /// Name of the method used to load entities
        /// </summary>
        private static readonly string _creationMethodName = "MyCreateEntity";

        public NetworkSceneClient(bool createServer = false) : base (GameModes.Network)
        {
            if(createServer)
                GameServer = new GameServer();

            //read network on each update
            Game.Instance.OnUpdate += ReadNetwork;

            //server config
            NetPeerConfiguration config = new NetPeerConfiguration("tank");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            //server creation
            Client = new NetClient(config);
            Client.Start();

            if (!createServer)
            {
                //check whether the user has a known server
                ListMenu serverMenu = new ListMenu("Do you want to connect to a given IP (if yes, enter in console)?", "tank.Code.YESORNOCHOOSENOW", ConnectionMethodSelectionCallback);
                Scene.Add(serverMenu);
            }
            else//we know that a local server must exist
                Client.DiscoverLocalPeers(14242);
            //register handler for receiving data
            OnClientData += IncomingHandler;

            //are we client, or are we dancer?
            if(!createServer)
                Console.WriteLine("client");
        }

        public void ConnectionMethodSelectionCallback(int selection)
        {
            if (selection == 0) //yes
            {
                new UiManager(Scene).ShowTextBox("enter ip", enteredString => Client.DiscoverKnownPeer(enteredString, 14242));
            }
            else
            {
                Client.DiscoverLocalPeers(14242);
            }
        }

        private void LoadFinish()
        {
            if (!_loadFinishSent)
            {
                NetUtil.SendMessage(Client, NetworkMessageType.LoadFinish, 1, Client.ServerConnection);
                _loadFinishSent = true;
                //experimental: remove our handler, save resources
                Scene.OnEmptyEntitiesToAdd -= LoadFinish;
            }
        }
        
        /// <summary>
        /// Method is called on each update to check for new messages
        /// </summary>
        private void ReadNetwork()
        {
            NetIncomingMessage msg;
            while ((msg = Client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        Client.Connect(msg.SenderEndPoint);
                        Maps map = (Maps)msg.ReadByte();
                        LoadMap(map);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        //register action for when we have loaded all entities
                        NetConnectionStatus status = (NetConnectionStatus) msg.ReadByte();
                        Console.WriteLine(status);
                        Console.WriteLine(msg.ReadString());
                        //on successful connect, we add the loadfinish method, which should immediately fire as
                        //everything probably has loaded yet
                        if (status == NetConnectionStatus.Connected)
                        {
                            Console.WriteLine("connection successful");
                            Scene.OnEmptyEntitiesToAdd += LoadFinish;
                        }
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.Data:
                        //the first four bytes in each message contain the type.
                        NetworkMessageType info = (NetworkMessageType) msg.ReadByte();
                        //notify everyone who wanted to be notified upon message arrival
                        OnClientData?.Invoke(Client, new NetworkEventArgs(msg, info));
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
                Client.Recycle(msg);
            }
        }

        private void LoadMap(Maps map)
        {
            //tank entity creation was moved to ogmo; see testlevel.oep for adding a decorator to your tank.
            //try to load a project, second path is image path. it goes into maps because ogmo instructs to go up a level, so we end up in resources
            OgmoProject proj = new OgmoProject("Resources/Maps/test.oep", "Resources/Maps/");

            //register our function to call for creating entities
            //this just is a string with the method name, the method itself has to be in the entities (see tank)
            proj.CreationMethodName = _creationMethodName;

            //uuh this somehow "registers a collision tag"
            proj.RegisterTag(CollidableTags.Wall, "CollisionLayer");

            //try to load a level into "Scene" 
            // yeah uh this can only do the testbench :D
            if (map == Maps.networkTestBench)
                proj.LoadLevel("Resources/Maps/networkTestBench.oel", Scene);
        }

        void IncomingHandler(object source, NetworkEventArgs n)
        {
            switch (n.GetInfo())
            {
                case NetworkMessageType.WhichTankCanIControl:
                    int id = n.GetData().ReadInt32(32);
                    NetUtil.GetTankByNetworkId(Scene, id).AddDecorator(Decorators.ControlWasd);
                    NetUtil.GetTankByNetworkId(Scene, id).AddDecorator(Decorators.ControlNetworkHook);
                    AddNetworkControl(id);
                    break;
                case NetworkMessageType.PauseControl:
                    if (NetworkPauseControl.Play == (NetworkPauseControl) n.GetData().ReadByte())
                        Scene.Resume();
                    else
                        Scene.Pause();
                    break;
            }
        }

        /// <summary>
        /// adds controlnetwork decorators to all tanks controlled by the network
        /// </summary>
        /// <param name="excludedId">exclude this one, it is controlled by the user</param>
        private void AddNetworkControl(int excludedId)
        {
            foreach (Tank t in Scene.GetEntities<Tank>())
                if(t.NetworkId != excludedId)
                    t.AddDecorator(Decorators.ControlNetwork);
        }
    }    
}
