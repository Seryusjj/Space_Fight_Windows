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

        public TextBlock scoreInScreen = AssetsManager.GetScoreText();
        public Entity laserUpgrade = AssetsManager.LaserUpgradeObject();

        public Entity player = AssetsManager.GetPlayer();

        public Entity asteroid = null;
        public ProyectileManager proyectileManager = new ProyectileManager();





        protected override void CreateScene()
        {
            EntityManager.Add(AssetsManager.GetBackground());
            EntityManager.Add(player);

            EntityManager.Add(proyectileManager);
            initAsteroids(EntityManager);
            EntityManager.Add(laserUpgrade);

            Sounds();

            EntityManager.Add(scoreInScreen.Entity);
            this.AddSceneBehavior(new CollisionSceneBehavior(), SceneBehavior.Order.PostUpdate);
        }


        private void Sounds()
        {
            //-- game lopp --//
            MusicPlayer player = WaveServices.MusicPlayer;
            player.IsRepeat = true;
            player.Play(SoundManager.getGameLoopSound());

            // sound bank --//
            SoundBank bank = new SoundBank(Assets);
            WaveServices.SoundPlayer.RegisterSoundBank(bank);

            //-- registering the different sounds --//
            bank.Add(SoundManager.getRockBrakingSound());
            //bank.Add(SoundManager.getLaserShotSound());

        }


        private Score GetScore()
        {
            Score score = null;

            if (WaveServices.Storage.Exists<Score>())
            {
                score = WaveServices.Storage.Read<Score>();
            } return score;

        }

        private void PersistScore()
        {
            Score score = GetScore();

            if (score == null)
            {
                score = new Score();
                score.score = this.puntos;
                WaveServices.Storage.Write<Score>(score);

            }

            else if (score.score < this.puntos)
            {

                score.score = this.puntos;
                WaveServices.Storage.Write<Score>(score);
            }

        }



        private void initAsteroids(WaveEngine.Framework.Managers.EntityManager EntityManager)
        {
            System.Random random = new System.Random();
            for (int i = 0; i < maxasteroids; i++)
            {
                //-- Scale value and andasteroid creation --//
                //random.NextDouble() * (maximum - minimum) + minimum;
                float scale = (float)random.NextDouble() * (1 - 0.3f) + 0.3f;
                Entity asteroid = AssetsManager.GetAsteroid(0, 0, scale, scale);


                //-- Random Position values and positioning --//
                int ancho = WaveServices.Random.Next((int)asteroid.FindComponent<Transform2D>().Rectangle.Width, (int)(WaveServices.ViewportManager.VirtualWidth - asteroid.FindComponent<Transform2D>().Rectangle.Width));
                int alto = (int)(WaveServices.ViewportManager.VirtualHeight - asteroid.FindComponent<Transform2D>().Rectangle.Height);

                Transform2D asteroidInitPosition = asteroid.FindComponent<Transform2D>();
                asteroidInitPosition.X = ancho;
                // random velocity --//

                asteroid.FindComponent<AsteroidBehavior>().speed = WaveServices.Random.Next(1,6);

                EntityManager.Add(asteroid);
            }

        }

        //----------------------------------- BEHVIOR ----------------------------------------//


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
                InsertLaserUpgrade();
                if (myScene.laserUpgrade.Enabled) { 
                
                }


            }

            private void InsertLaserUpgrade() {
                if (myScene.puntos > 100 && myScene.laserUpgrade.Enabled == false) {
                    myScene.laserUpgrade.Enabled=true;

                }
            
            }

            private void IsUpgradeCollidingWithPlayer() {
                Entity laserUpgrade = myScene.laserUpgrade;
                Entity player = myScene.player;

                PerPixelCollider playerCollider = player.FindComponent<PerPixelCollider>();
                PerPixelCollider laserUpgradeCollider = laserUpgrade.FindComponent<PerPixelCollider>();
                if (laserUpgradeCollider.Intersects(playerCollider)) {
                    laserUpgrade.Enabled = false;
                    myScene.EntityManager.Remove(laserUpgrade);
                }


            
            }






            private void collideWithPlayer()
            {
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
                        if (asteroidCollider.Intersects(playerColider))
                        {
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


                            if (laserCollider.Intersects(asteroidCollider) && asteroid.FindComponent<Animation2D>().CurrentAnimation.Equals("Rotate"))
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
                        myScene.scoreInScreen.Text = "Score: " + myScene.puntos;
                        myScene.asteroid.Enabled = true;
                    }
                }

                reactivateAsteroids();




            }

            /// <summary>
            /// Reactiva los asteroides que se escapan por el fondo de la pantalla
            /// dandoles una nueva posicion random X y y = 0
            /// </summary>
            private void reactivateAsteroids()
            {
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
                        asteroid.FindComponent<AsteroidBehavior>().speed = WaveServices.Random.Next(1, 6);
            
                        asteroid.Enabled = true;

                    }
                }
            }

        }


    }
}

