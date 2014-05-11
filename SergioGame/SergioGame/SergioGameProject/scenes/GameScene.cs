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
    public class GameScene : Scene
    {
        public bool gameOver = false;
        public int maxAsteroids = 10;
        public int puntos = 0;
        public Entity asteroid = null;
        public Boolean canBeDestroyed = false;

        protected override void CreateScene()
        {
            EntityManager.Add(AssetsManager.GetBackground());
            EntityManager.Add(AssetsManager.GetPlayer());

            EntityManager.Add(AssetsManager.GetRedLaserManager());
            EntityManager.Add(AssetsManager.GetGreenLaserManager());//inhabilitado por defecto

            InitAsteroids();
            EntityManager.Add(AssetsManager.GetExplosion());
            EntityManager.Add(AssetsManager.GetLaserUpgradeThree());
            EntityManager.Add(AssetsManager.GetLaserUpgradeTwo());

            Sounds();

            EntityManager.Add(AssetsManager.GetScoreText());

            AssetsManager.SetTexOfScoreInScreen("Score: " + puntos);

            this.AddSceneBehavior(new CollisionSceneBehavior(), SceneBehavior.Order.PostUpdate);
        }

        protected override void Start()
        {
            RandomizeAsteroidsPositions();
        
        }


        public void Restart()
        {
            puntos = 0;
            canBeDestroyed = false;
            AssetsManager.GetRedLaserManager().Entity.Enabled = true;
            AssetsManager.GetGreenLaserManager().Entity.Enabled = false;
            AssetsManager.GetPlayer().FindComponent<PlayerBehavior>().currentLaserStat = PlayerBehavior.LaserStat.OneLaser;
            AssetsManager.GetPlayer().FindComponent<PlayerBehavior>().shoot = true;

        }

        private void Sounds()
        {

            // sound bank --//
            SoundBank bank = new SoundBank(Assets);
            WaveServices.SoundPlayer.RegisterSoundBank(bank);

            //-- registering the different sounds --//
            bank.Add(SoundManager.getRockBrakingSound());
            //bank.Add(SoundManager.getLaserShotSound()); 
            //Este casa no se por que, deberia funcionar como el anterior ...

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






        private void InitAsteroids()
        {
            System.Random random = new System.Random();
            for (int i = 0; i < maxAsteroids; i++)
            {
                //-- Scale value and andasteroid creation --//
                float maximunScale = 1;
                float minimunScale = 0.3f;
                float scale = (float)random.NextDouble() * (maximunScale - minimunScale) + minimunScale;
                Entity asteroid = AssetsManager.CreateAsteroid(100, 0, scale, scale);

                // random velocity --//
                asteroid.FindComponent<AsteroidBehavior>().speed = WaveServices.Random.Next(1, 6);

                EntityManager.Add(asteroid);
            }

        }

        private void RandomizeAsteroidsPositions() {
            System.Random random = new System.Random();
            for (int i = 0; i < maxAsteroids; i++)
            {
                Entity asteroid = EntityManager.Find("Asteroid" + i);
                //-- Random Position values and positioning --//
                Transform2D asteroidTransform = asteroid.FindComponent<Transform2D>();
                float ancho = (WaveServices.ViewportManager.VirtualWidth - asteroidTransform.Rectangle.Width);
                float position = (float)random.NextDouble() * (ancho - 0) + 0;

                asteroidTransform.X = position;
            }
        
        
        
        }

        //----------------------------------- BEHAVIOR ----------------------------------------//


        public class CollisionSceneBehavior : SceneBehavior
        {
            public GameScene myScene;
            private bool collected = false;


            protected override void ResolveDependencies()
            {
                myScene = (GameScene)this.Scene;
            }


            protected override void Update(TimeSpan gameTime)
            {
                BreakAsteroid();
                if (AssetsManager.GetPlayer().Enabled == true) { CollideAsteroidWithPlayer(); }

                MoveAsteroidAndReactivate();
                InsertLaserUpgrade();
                RealocateUpgradeGreen();
                RealocateUpgradeReed();
                if (AssetsManager.GetLaserUpgradeThree().Enabled || AssetsManager.GetLaserUpgradeTwo().Enabled)
                {
                    IsUpgradeCollidingWithPlayer();
                }
                if (myScene.gameOver)
                {

                    WaveServices.ScreenContextManager.Push(new ScreenContext("MenuContext", new GameOverScene()));
                    myScene.gameOver = false;
                }


            }

            private void InsertLaserUpgrade()
            {

                if (myScene.puntos > 100 && AssetsManager.GetLaserUpgradeThree().Enabled == false && !collected && myScene.puntos < 120)
                {
                    RealocateUpgradeReed();
                    AssetsManager.GetLaserUpgradeTwo().Enabled = true;

                    collected = false;
                }
                else if (myScene.puntos > 1000 && AssetsManager.GetLaserUpgradeThree().Enabled == false && !collected && myScene.puntos < 1020)
                {
                    RealocateUpgradeGreen();
                    AssetsManager.GetLaserUpgradeThree().Enabled = true;
                        collected = false;

                }

            }

            private void RealocateUpgradeReed()
            {
                if (AssetsManager.GetLaserUpgradeTwo().Enabled == false)
                {
                    var transform = AssetsManager.GetLaserUpgradeTwo().FindComponent<Transform2D>();
                    int ancho = (int)(WaveServices.ViewportManager.VirtualWidth - transform.Rectangle.Width);
                    transform.X = WaveServices.Random.Next(0, ancho);
                    transform.Y = 0;

                }
            }

            private void RealocateUpgradeGreen()
            {
                if (AssetsManager.GetLaserUpgradeThree().Enabled == false)
                {
                    var transform = AssetsManager.GetLaserUpgradeThree().FindComponent<Transform2D>();
                    int ancho = (int)(WaveServices.ViewportManager.VirtualWidth - transform.Rectangle.Width);
                    transform.X = WaveServices.Random.Next(0, ancho);
                    transform.Y = 0;

                }
            }

            private void IsUpgradeCollidingWithPlayer()
            {
                Entity laserUpgradeGreen = AssetsManager.GetLaserUpgradeThree();
                Entity player = AssetsManager.GetPlayer();
                Entity laserUpgradeTwo = AssetsManager.GetLaserUpgradeTwo();

                PerPixelCollider playerCollider = player.FindComponent<PerPixelCollider>();
                PerPixelCollider laserUpgradeColliderGreen = laserUpgradeGreen.FindComponent<PerPixelCollider>();
                PerPixelCollider laserUpgradeColliderRed = laserUpgradeTwo.FindComponent<PerPixelCollider>();
                if (laserUpgradeColliderGreen.Intersects(playerCollider))
                {
                    laserUpgradeGreen.Enabled = false;
                    PlayerBehavior playerBehaviour = player.FindComponent<PlayerBehavior>();
                    playerBehaviour.currentLaserStat = PlayerBehavior.LaserStat.ThreeLasers;


                    //ponemos el laser de coloer verde
                    playerBehaviour.shoot = false;
                    AssetsManager.GetRedLaserManager().Entity.Enabled = false;
                    AssetsManager.GetGreenLaserManager().Entity.Enabled = true;

                    playerBehaviour.shoot = true;

                }
                else if (laserUpgradeColliderRed.Intersects(playerCollider))
                {
                    laserUpgradeTwo.Enabled = false;
                    PlayerBehavior playerBehaviour = player.FindComponent<PlayerBehavior>();
                    playerBehaviour.currentLaserStat = PlayerBehavior.LaserStat.TwoLasers;

                }

            }




            private void CollideAsteroidWithPlayer()
            {
                for (int i = 0; i < myScene.maxAsteroids; i++)
                {
                    //asteroide a evaluar
                    Entity asteroid = myScene.EntityManager.Find("Asteroid" + i);
                    String asteroidState = asteroid.FindComponent<Animation2D>().CurrentAnimation;
                    PerPixelCollider asteroidCollider = asteroid.FindComponent<PerPixelCollider>();
                    AsteroidBehavior asteroidBehavior = asteroid.FindComponent<AsteroidBehavior>();
                    PerPixelCollider playerColider = AssetsManager.GetPlayer().FindComponent<PerPixelCollider>();
                    if (asteroid.Enabled == true && asteroidState.Equals("Rotate"))
                    {
                        if (asteroidCollider.Intersects(playerColider) && myScene.canBeDestroyed)
                        {
                            AssetsManager.GetPlayer().Enabled = false;
                            Explosion();
                            myScene.gameOver = true;
                            asteroidBehavior.breakAsteroid();
                            AssetsManager.GetPlayer().FindComponent<PlayerBehavior>().shoot = false;
                        }

                    }
                }

            }


            private void BreakAsteroid()
            {

                Entity laser = null;
                int count = 0;
                foreach (Entity laserEntity in AssetsManager.GetCurrentLaserManager().Entity.ChildEntities)
                {
                    count++;
                    for (int i = 0; i < myScene.maxAsteroids; i++)
                    {
                        //asteroide a evaluar
                        Entity asteroid = myScene.EntityManager.Find("Asteroid" + i);


                        if (count >= AssetsManager.GetCurrentLaserManager().numBullets)
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

                                myScene.puntos += 10;//has destruido el asteriode sumas puntos
                                AssetsManager.SetTexOfScoreInScreen("Score: " + myScene.puntos);


                            }
                        }

                    }
                }
            }

            /// <summary>
            /// este metodo tiene que dar un nueva posicion random al los asteroides inactivos 
            /// y reactivarlos
            /// </summary>
            private void MoveAsteroidAndReactivate()
            {
                if (myScene.asteroid != null)
                {
                    if (myScene.asteroid.Enabled == false)
                    {
                        var transform = myScene.asteroid.FindComponent<Transform2D>();
                        int ancho = (int)(WaveServices.ViewportManager.VirtualWidth - myScene.asteroid.FindComponent<Transform2D>().Rectangle.Width);
                        transform.X = WaveServices.Random.Next(0, ancho);
                        transform.Y = 0;

                        myScene.asteroid.Enabled = true;
                    }
                }

                ReactivateAsteroids();




            }

            private void Explosion()
            {
                // Creates the explosions and adjusts to the ship position.

                AssetsManager.GetExplosion().Enabled = true;

                var explosionTransform = AssetsManager.GetExplosion().FindComponent<Transform2D>();
                var shipTransform = AssetsManager.GetPlayer().FindComponent<Transform2D>();

                explosionTransform.X = shipTransform.X - explosionTransform.Rectangle.Width;
                explosionTransform.Y = shipTransform.Y - explosionTransform.Rectangle.Height;

                var anim2D = AssetsManager.GetExplosion().FindComponent<Animation2D>();
                anim2D.CurrentAnimation = "Explosion";
                anim2D.Play(false);
            }

            /// <summary>
            /// Reactiva los asteroides que se escapan por el fondo de la pantalla
            /// dandoles una nueva posicion random X y y = 0
            /// </summary>
            private void ReactivateAsteroids()
            {
                for (int i = 0; i < myScene.maxAsteroids; i++)
                {
                    //asteroide a evaluar
                    Entity asteroid = myScene.EntityManager.Find("Asteroid" + i);
                    if (asteroid.Enabled == false)
                    {
                        var transform = asteroid.FindComponent<Transform2D>();
                        int ancho = (int)(WaveServices.ViewportManager.VirtualWidth - transform.Rectangle.Width);
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

