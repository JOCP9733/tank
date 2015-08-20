using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank.Code.Entities.Weapons.WeaponLogic
{
    class ProtoProjectile :IProjectileLogic
    {
        public Game Game;
        public Scene Scene;
        public Projectile Projectile;
        public Input Input;

        public ProtoProjectile(Game game, Projectile p)
        {
            Game = game;
            Scene = game.Scene;
            Input = Scene.Input;
            Projectile = p;
        }

        public IProjectileLogic getUpperMost()
        {
            return this;
        }

        public void Render()
        {
        }

        public void Update()
        { 
        }
    }
}
