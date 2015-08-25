using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Otter;

namespace tank.Code.Entities.Powerups
{
    class PowerUp : Entity
    {
        public PowerUps PowerUpType;

        public PowerUp(PowerUps type, float x, float y) : base(x, y)
        {
            //dynamically load the needed texture from the enum name
            Image img = new Image(new Texture("Resources/Powerups/" + Enum.GetName(typeof(PowerUps), type) + ".png"));
            img.CenterOrigin();
            AddGraphic(img);

            //set the type
            PowerUpType = type;

            //add a collider
            BoxCollider collider = new BoxCollider(img.Width, img.Height, CollidableTags.PowerUp);
            collider.CenterOrigin();
            AddCollider(collider);
        }

        /// <summary>
        /// This function gets called by the ogmo level loader
        /// </summary>
        public static void MyCreateEntity(Scene scene, XmlAttributeCollection ogmoParameters)
        {
            //ok ogmo gives us the position in x and y, and the list of decorators in DecoratorList
            int x = ogmoParameters.Int("x", -1);
            int y = ogmoParameters.Int("y", -1);

            //this is how you read a string from ogmo
            string type = ogmoParameters.GetNamedItem("Type").Value;
            PowerUps typeEnum = Utilities.ParseEnum<PowerUps>(type);

            //create an instance
            PowerUp p = new PowerUp(typeEnum, x, y);

            //add to the scene
            scene.Add(p);
        }

        public override void Render()
        {
            base.Render();
            Collider.Render();
        }
    }
}
