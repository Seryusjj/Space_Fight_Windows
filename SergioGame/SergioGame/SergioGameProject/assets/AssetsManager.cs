using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Framework;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveEngine.Components.Animation;
using WaveEngine.Components.UI;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.UI;
using SergioGameProject.assets;

namespace SergioGameProject
{
    /// <summary>
    /// Class responsible for managaing the different entitys in the game
    /// </summary>
    static class AssetsManager
    {
        private static int asteroidCounter = 0;
        private static int mineCounter = 0;
        private static Entity player;
        private static Entity background;
        private static TextBlock textBlockForScoreInScreen;
        private static Entity scoreInScreen;
        private static Entity laserUpgradeThree;
        private static ProyectileManager redLaserManager;
        private static ProyectileManager greenLaserManager;
        private static Entity explosion;



        public static Entity CreateMine()
        {


            Entity mine = new Entity("mine" + mineCounter).AddComponent(new Transform2D()
            {
                X = WaveServices.ViewportManager.ScreenWidth / 2,
                Y = WaveServices.ViewportManager.ScreenHeight / 2,
            });



            mine.AddComponent(new Sprite(PathManager.mine));
            mine.AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            mineCounter++;


            return mine;
        }

        public static Entity GetExplosion()
        {
            if (explosion == null)
            {
                explosion = new Entity("explosion")
                    .AddComponent(new Transform2D() { 
                        XScale = 3,
                        YScale = 2.5f,
                    })
                    .AddComponent(new Sprite(PathManager.explosionSprites))
                    .AddComponent(Animation2D.Create<TexturePackerGenericXml>(PathManager.explosionXml)
                        .Add("Explosion", new SpriteSheetAnimationSequence() { First = 1, Length = 16, FramesPerSecond = 16 }))
                    .AddComponent(new AnimatedSpriteRenderer());
                explosion.Enabled = false;
            }

            return explosion;
        }

        public static Entity GetLaserUpgradeThree()
        {
            if (laserUpgradeThree == null)
            {
                laserUpgradeThree = new Entity("laserUpgrade").AddComponent(new Transform2D()
            {

                X = 100,
                Y = 100
            });
                laserUpgradeThree.AddComponent(new PerPixelCollider(PathManager.laserUpgradeThreeGreen, 0));
                laserUpgradeThree.AddComponent(new LaserUpgradeBehaviour());
                laserUpgradeThree.AddComponent(new Sprite(PathManager.laserUpgradeThreeGreen));
                laserUpgradeThree.AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
                laserUpgradeThree.Enabled = false;
            }
            return laserUpgradeThree;
        }

        /// <summary>
        /// Return the TextBolck where de Score should be show
        /// </summary>
        /// <returns></returns>
        public static Entity GetScoreText()
        {
            if (scoreInScreen == null)
            {
                textBlockForScoreInScreen = new TextBlock()
                {
                    Margin = new Thickness(100, 100, 0, 0),
                    Foreground = Color.Red,
                };
                scoreInScreen = textBlockForScoreInScreen.Entity;
            }
            return scoreInScreen;
        }


        public static void SetTexOfScoreInScreen(String text)
        {
            if (textBlockForScoreInScreen == null)
            {
                GetScoreText();
            }
            textBlockForScoreInScreen.Text = text;

        }

        /// <summary>
        /// Several instances, manage it transparently with names Asteroid1 ... Asteroidn
        /// </summary>
        /// <param name="intX"></param>
        /// <param name="initY"></param>
        /// <param name="ScaleX"></param>
        /// <param name="ScaleY"></param>
        /// <returns></returns>
        public static Entity CreateAsteroid(int intX, int initY, float ScaleX, float ScaleY)
        {
            Entity asteroid = new Entity("Asteroid" + asteroidCounter);
            asteroid.AddComponent(new Transform2D()
            {

                X = intX,
                Y = initY,
                XScale = ScaleX,
                YScale = ScaleY


            });
            asteroid.AddComponent(new PerPixelCollider(PathManager.asteroid, 0));
            asteroid.AddComponent(new Sprite(PathManager.asteroid));
            Animation2D animations = Animation2D.Create<TexturePackerGenericXml>(PathManager.asteroidXml);
            animations.Add("Rotate", new SpriteSheetAnimationSequence() { First = 1, Length = 26, FramesPerSecond = 12 });
            animations.Add("Break", new SpriteSheetAnimationSequence() { First = 28, Length = 34, FramesPerSecond = 6 });
            asteroid.AddComponent(animations);
            asteroid.AddComponent(new AnimatedSpriteRenderer(DefaultLayers.Alpha));
            asteroid.AddComponent(new AsteroidBehavior());
            asteroidCounter++;
            return asteroid;
        }


        private static Entity GetPlayerShield(float x, float y)
        {
            Entity shield = new Entity("PlayerShield").AddComponent(new Transform2D()
            {
                X = x,
                Y = y
            })
            .AddComponent(new Sprite(PathManager.playerShield))
            .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            shield.Enabled = false;

            return shield;


        }

        /// <summary>
        /// just one instance per gameplay
        /// </summary>
        /// <returns></returns>
        public static Entity GetPlayer()
        {
            if (player == null)
            {

                player = new Entity("Player");
                player.AddComponent(new Transform2D()
                {
                    X = WaveServices.ViewportManager.VirtualWidth / 2,
                    Y = WaveServices.ViewportManager.VirtualHeight / 2,
                });
                player.AddComponent(new Sprite(PathManager.player));
                player.AddComponent(new PerPixelCollider(PathManager.player, 0));
                Animation2D animations = Animation2D.Create<TexturePackerGenericXml>(PathManager.playerXml);
                animations.Add("Idle", new SpriteSheetAnimationSequence() { First = 1, Length = 1, FramesPerSecond = 5 });
                animations.Add("Left", new SpriteSheetAnimationSequence() { First = 3, Length = 1, FramesPerSecond = 5 });
                animations.Add("Right", new SpriteSheetAnimationSequence() { First = 4, Length = 1, FramesPerSecond = 5 });
                animations.Add("Break", new SpriteSheetAnimationSequence() { First = 2, Length = 1, FramesPerSecond = 5 });
                player.AddComponent(animations);
                player.AddComponent(new AnimatedSpriteRenderer(DefaultLayers.Alpha));
                player.AddComponent(new PlayerBehavior());
                player.Enabled = false;
                var playerRectangle = player.FindComponent<Transform2D>().Rectangle;
                player.AddChild(GetPlayerShield(-25, -40));

            }

            return player;
        }


        public static Entity GetBackground()
        {
            if (background == null)
            {
                background = new Entity("background");
                background.AddChild(GetBackgroudPart1());
                background.AddChild(GetBackgroudPart2());
            }
            return background;

        }

        /// <summary>
        /// Los valores de escalado se dividen por 600 y 800 porque la imagen usada es
        /// de tamaño 600 * 800
        /// </summary>
        /// <returns></returns>
        private static Entity GetBackgroudPart1()
        {
            

            Entity background = new Entity("backGround1").AddComponent(new Transform2D()
            {
                X = 0,
                Y = 0,
                DrawOrder = 1,
                XScale = WaveServices.ViewportManager.VirtualWidth / 600f,
                YScale = WaveServices.ViewportManager.VirtualHeight / 800f
            });



            background.AddComponent(new Sprite(PathManager.level1Bg));
            background.AddComponent(new SpriteRenderer(DefaultLayers.Opaque));
            background.AddComponent(new ScrollBehavior1(WaveServices.ViewportManager.VirtualHeight));


            return background;
        }

        /// <summary>
        /// 550 y 650 
        /// </summary>
        /// <returns></returns>
        private static Entity GetBackgroudPart2()
        {
            Entity background = new Entity("backGround2").AddComponent(new Transform2D()
            {
                X = 0,
                Y = -WaveServices.ViewportManager.VirtualHeight,
                DrawOrder = 1,
                XScale = WaveServices.ViewportManager.VirtualWidth / 600f,
                YScale = WaveServices.ViewportManager.VirtualHeight / 800f
            });



            background.AddComponent(new Sprite(PathManager.level1Bg));
            background.AddComponent(new SpriteRenderer(DefaultLayers.Opaque));
            background.AddComponent(new ScrollBehavior2(WaveServices.ViewportManager.VirtualHeight));


            return background;
        }

        public static ProyectileManager GetRedLaserManager()
        {
            if (redLaserManager == null)
            {
                ProyectileManager.selectedBullet = ProyectileManager.Proyectiles.redLaser;
                redLaserManager = new ProyectileManager("RedProyectileManager");
                redLaserManager.Entity.Enabled = true;
            }
            return redLaserManager;
        }

        public static ProyectileManager GetGreenLaserManager()
        {
            if (greenLaserManager == null)
            {
                ProyectileManager.selectedBullet = ProyectileManager.Proyectiles.greenLaser;
                greenLaserManager = new ProyectileManager("GreenProyectileManager");
                greenLaserManager.Entity.Enabled = false;
            }
            return greenLaserManager;
        }


        /// <summary>
        /// Creates a new play button.
        /// </summary>
        /// <param name="x">The x of the new button.</param>
        /// <param name="y">The y of the new button.</param>
        /// <returns></returns>
        public static Button CreatePlayButton(float x, float y)
        {

            var button = new Button()
            {
                Margin = new Thickness(0, y, 0, 0),
                HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Center,
                Text = string.Empty,
                IsBorder = false,
                BackgroundImage = PathManager.buttonStage1,
                PressedBackgroundImage = PathManager.buttonStage2,

            };

            return button;
        }

        /// <summary>
        /// Creates a new mainMenuBackground
        /// </summary>
        /// <returns></returns>
        public static Entity CreateMainMenuBackground(float y)
        {
            var a = WaveServices.ViewportManager.RatioX;
            var background = new Entity()
                .AddComponent(new Transform2D()
                {
                    X = 0,
                    Y = y,
                    XScale = WaveServices.ViewportManager.VirtualWidth ,
                    YScale = 0.5f,
                    DrawOrder = 1f
                })
                .AddComponent(new Sprite(PathManager.mainMenuBg))
                .AddComponent(new SpriteRenderer(DefaultLayers.GUI));

            return background;
        }



        public static ProyectileManager GetCurrentLaserManager()
        {
            if (textBlockForScoreInScreen == null)
            {
                GetScoreText();
            }

            if (GetRedLaserManager().Entity.Enabled == true)
            {
                textBlockForScoreInScreen.Foreground = Color.Red;
                return GetRedLaserManager();
            }
            else
            {
                textBlockForScoreInScreen.Foreground = Color.GreenYellow;
                return GetGreenLaserManager();
            }

        }


    }


}
