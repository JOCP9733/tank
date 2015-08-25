using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using tank.Code.Entities.Powerups;

namespace tank.Code.Entities.Tank.Logics.Decorators
{
    /// <summary>
    /// enable picking up powerups; the tank MUST HAVE GETDAMAGE
    /// </summary>
    class UsePowerUps : LogicDecorator
    {

        public UsePowerUps(ITankLogic pLogic) : base(pLogic)
        {
        }

        public override void Update()
        {
            base.Update();
            if(Tank.Collider == null)
                throw new InvalidOperationException("to use this decorator, the tank must have the getdamage collider");
            //get all colliding entities
            Console.WriteLine(Tank.Collider.Overlap(Tank.X, Tank.Y, CollidableTags.PowerUp));
            List<Entity> collisionList = Tank.Collider.CollideEntities(Tank.X, Tank.Y, CollidableTags.PowerUp);
            if (collisionList.Count > 0)
            {
                foreach (Entity powerUp in collisionList)
                {
                    //cast to powerup
                    PowerUp p = (PowerUp) powerUp;
                    //get enum position name
                    string powerUpName = p.PowerUpType.ToString();
                    //parse to tank decorator enum -> power up types must be tank decorators!
                    Code.Decorators tankDecorator = Utilities.ParseEnum<Code.Decorators>(powerUpName);
                    //add to tank
                    Tank.AddDecorator(tankDecorator);
                    //remove the powerup
                    p.RemoveSelf();
                }
            }
        }
    }
}
