#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.UI;

using WaveEngine.Framework.UI;
using WaveEngine.Framework.Physics2D;
#endregion

namespace SergioGameProject
{
    public class MyScene : Scene
    {

        public Entity player = AssetsManager.GetPlayer();
        public Entity asteroid = AssetsManager.GetAsteroid();
        public ProyectileManager proyectileManager = new ProyectileManager();

        protected override void CreateScene()
        {
            EntityManager.Add(AssetsManager.GetBackground());
            EntityManager.Add(player);
            EntityManager.Add(asteroid);
            EntityManager.Add(proyectileManager);
            var anim = asteroid.FindComponent<Animation2D>();
            anim.CurrentAnimation = "Rotate";
            anim.Play();

            this.AddSceneBehavior(new CollisionSceneBehavior(), SceneBehavior.Order.PostUpdate);


        }










        public class CollisionSceneBehavior : SceneBehavior
        {
            public MyScene myScene;
            private float positonX = 0;

            protected override void ResolveDependencies()
            {
                myScene = (MyScene)this.Scene;
            }


            protected override void Update(TimeSpan gameTime)
            {
                breakAsteroid();
                moveAsteroidAndReactivate();

            }


            private void breakAsteroid()
            {

                Entity laser = null;
                int count = 0;
                foreach (Entity en in myScene.proyectileManager.Entity.ChildEntities)
                {
                    count++;
                    if (count >= myScene.proyectileManager.numBullets)
                    {
                        break;
                    }
                    else if (en.IsActive && myScene.asteroid.IsActive)
                    {
                        laser = en;

                        PerPixelCollider laserCollider = en.FindComponent<PerPixelCollider>();
                        PerPixelCollider asteroidCollider = myScene.asteroid.FindComponent<PerPixelCollider>();


                        if (laserCollider.Intersects(asteroidCollider))
                        {
                            String laserpath = laserCollider.TexturePath;

                            myScene.asteroid.FindComponent<AsteroidBehavior>().breakAsteroid();//rompe el aseroide
                            en.Enabled = false; //consume el laser  
                            en.RemoveComponent<PerPixelCollider>();
                            en.AddComponent(new PerPixelCollider(laserpath, 0));


                        }



                    }
                }
            }

            /// <summary>
            /// este metodo tiene que dar un nueva posicion random al los asteroides inactivos 
            /// y reactivarlos
            /// </summary>
            private void moveAsteroidAndReactivate()
            {
                if (myScene.asteroid.Enabled == false)
                {
                    var transform = myScene.asteroid.FindComponent<Transform2D>();
                    transform.X = 100;
                    transform.Y = 100;

                    myScene.asteroid.Enabled = true;
                }

            }
        }

        public SceneBehavior behavior { get; set; }
    }
}

