using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Otter;
using Otter.UI;
using tank.Code.Entities.Tank;
using tank.Code.Entities.Tank.Logics;
using tank.Code.Entities.Tank.Logics.Decorators;

namespace tank.Code.GameMode.TestingMode
{
    class TestingMode : GameMode
    {
        /// <summary>
        /// otter calls this to create entities
        /// </summary>
        private static readonly string _creationMethodName = "MyCreateEntity";

        public TestingMode()
        {
            //tank entity creation was moved to ogmo; see testlevel.oep for adding a decorator to your tank.
            //try to load a project
            OgmoProject proj = new OgmoProject("Resources/Maps/test.oep");

            //register our function to call for creating entities
            //this just is a string with the method name, the method itself has to be in the entities (see tank)
            proj.CreationMethodName = _creationMethodName;

            //uuh this somehow "registers a collision tag"
            proj.RegisterTag(CollidableTags.Wall, "CollisionLayer");

            //try to load a level into "Scene"
            proj.LoadLevel("Resources/Maps/collisionTestBench.oel", Scene);
        }
    }    
}
