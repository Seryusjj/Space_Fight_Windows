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
using System.Collections;
using System.Collections.Generic;
using WaveEngine.Common.Media;
using WaveEngine.Framework.Sound;
#endregion

namespace SergioGameProject
{
    public class MyScene : Scene
    {

        public int maxasteroids = 10;
        public int puntos = 0;
        public Boolean isPlayerDestroy = false;


        public Entity player = AssetsManager.GetPlayer();

        public Entity asteroid = null;
        public ProyectileManager proyectileManager = new ProyectileManager();



        private void initAsteroids(WaveEngine.Framework.Managers.EntityManager EntityManager)
        {
            System.Random random = new System.Random();
            for (int i = 0; i < maxasteroids; i++)
            {
                //-- Scale values --//
                //random.NextDouble() * (maximum - minimum) + minimum;
                float scaleX = (float)random.NextDouble() * (1 - 0.3f) + 0.3f;
                //float scaleY = (float)random.NextDouble() * (1 - 0.3f) + 0.3f;
                Entity asteroid = AssetsManager.GetAsteroid(0, 0, scaleX, scaleX);
                //-- Position values --//

                int ancho = WaveServices.Random.Next((int)asteroid.FindComponent<Transform2D>().Rectangle.Width,(int)(WaveServices.ViewportManager.VirtualWidth - asteroid.FindComponent<Transform2D>().Rectangle.Width));
                int alto = (int)(WaveServices.ViewportManager.VirtualHeight - asteroid.FindComponent<Transform2D>().Rectangle.Height);

                Transform2D asteroidInitPosition = asteroid.FindComponent<Transform2D>();
                asteroidInitPosition.X = ancho;
               // asteroidInitPosition.Y = alto;

                EntityManager.Add(asteroid);
            }

        }



        protected override void CreateScene()
        {
            EntityManager.Add(AssetsManager.GetBackground());
            EntityManager.Add(player);

            EntityManager.Add(proyectileManager);
            initAsteroids(EntityManager);

            //SoundBank bank = new SoundBank(Assets);
            //WaveServices.SoundPlayer.RegisterSoundBank(bank);

            //Register sounds
            
          //  bank.Add(SoundManager.getGameLoopSound());



            this.AddSceneBehavior(new CollisionSceneBehavior(), SceneBehavior.Order.PostUpdate);
            //WaveServices.MusicPlayer.Play(new MusicInfo("Content/Music/game loop.mp3"));


        }

        protected override void Start()
        {
            base.Start();

            // Play Sound
             WaveServices.SoundPlayer.Play(SoundManager.getGameLoopSound());
            //WaveServices.SoundPlayer.Play(sound2);

            //WaveServices.TimerFactory.CreateTimer("Timer1", TimeSpan.FromSeconds(4),
            //() =>
            //{
            //    WaveServices.SoundPlayer.Play(sound3);
            //});

            //WaveServices.TimerFactory.CreateTimer("Timer2", TimeSpan.FromSeconds(2),
            //() =>
            //{
            //    WaveServices.SoundPlayer.Play(sound4);
            //},
            //false);
        }












        public class CollisionSceneBehavior : SceneBehavior
        {
            public MyScene myScene;


            protected override void ResolveDependencies()
            {
                myScene = (MyScene)this.Scene;
            }


            protected override void Update(TimeSpan gameTime)
            {
                breakAsteroid();
                collideWithPlayer();
                moveAsteroidAndReactivate();
                

            }

            private void collideWithPlayer() {
                for (int i = 0; i < myScene.maxasteroids; i++)
                {
                    //asteroide a evaluar
                    Entity asteroid = myScene.EntityManager.Find("Asteroid" + i);
                    String asteroidState = asteroid.FindComponent<Animation2D>().CurrentAnimation;
                    PerPixelCollider asteroidCollider = asteroid.FindComponent<PerPixelCollider>();
                    AsteroidBehavior asteroidBehavior = asteroid.FindComponent<AsteroidBehavior>();
                    PerPixelCollider playerColider = myScene.EntityManager.Find("Player").FindComponent<PerPixelCollider>();
                    if (asteroid.Enabled == true && asteroidState.Equals("Rotate"))
                    {
                        if(asteroidCollider.Intersects(playerColider)){
                            myScene.isPlayerDestroy = true;
                            asteroidBehavior.breakAsteroid();
                        }

                    }
                }
                
            }


            private void breakAsteroid()
            {

                Entity laser = null;
                int count = 0;
                foreach (Entity laserEntity in myScene.proyectileManager.Entity.ChildEntities)
                {
                    count++;
                    for (int i = 0; i < myScene.maxasteroids; i++)
                    {
                        //asteroide a evaluar
                        Entity asteroid = myScene.EntityManager.Find("Asteroid" + i);

                        
                        if (count >= myScene.proyectileManager.numBullets)
                        {
                            break; //no hay mas proyectiles a evaluar
                        }

                            //proyectil a evaluar
                        else if (laserEntity.IsActive && asteroid.IsActive)
                        {
                            laser = laserEntity;

                            PerPixelCollider laserCollider = laserEntity.FindComponent<PerPixelCollider>();
                            PerPixelCollider asteroidCollider = asteroid.FindComponent<PerPixelCollider>();


                            if (laserCollider.Intersects(asteroidCollider))
                            {
                                myScene.asteroid = asteroid;
                                String laserpath = laserCollider.TexturePath;
                                myScene.asteroid.FindComponent<AsteroidBehavior>().breakAsteroid();//rompe el aseroide
                                laserEntity.Enabled = false; //consume el laser  
                                laserEntity.RemoveComponent<PerPixelCollider>();//consume el laser
                                laserEntity.AddComponent(new PerPixelCollider(laserpath, 0));//reinicializa el laser


                            }
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
                if (myScene.asteroid != null)
                {
                    if (myScene.asteroid.Enabled == false)
                    {
                        var transform = myScene.asteroid.FindComponent<Transform2D>();
                        int ancho = (int)(WaveServices.ViewportManager.VirtualWidth - myScene.asteroid.FindComponent<Transform2D>().Rectangle.Width);
                        int alto = (int)(WaveServices.ViewportManager.VirtualHeight - myScene.asteroid.FindComponent<Transform2D>().Rectangle.Height);
                        transform.X = WaveServices.Random.Next(0, ancho);
                        transform.Y = 0;
                        myScene.puntos += 10;//has destruido el asteriode sumas puntos
                        myScene.asteroid.Enabled = true;
                    }
                }

                reactivateAsteroids();

                


            }

            private void reactivateAsteroids() {
                for (int i = 0; i < myScene.maxasteroids; i++)
                {
                    //asteroide a evaluar
                    Entity asteroid = myScene.EntityManager.Find("Asteroid" + i);
                    if (asteroid.Enabled == false)
                    {
                        var transform = asteroid.FindComponent<Transform2D>();
                        int ancho = (int)(WaveServices.ViewportManager.VirtualWidth - asteroid.FindComponent<Transform2D>().Rectangle.Width);
                        int alto = (int)(WaveServices.ViewportManager.VirtualHeight - asteroid.FindComponent<Transform2D>().Rectangle.Height);
                        transform.X = WaveServices.Random.Next(0, ancho);
                        transform.Y = 0;
                        asteroid.Enabled = true;

                    }
                }
            }

        }

        
    }
}

