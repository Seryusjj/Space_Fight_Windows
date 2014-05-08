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


        public static Entity CreateMine()
        {


            Entity mine = new Entity("mine"+mineCounter).AddComponent(new Transform2D()
            {
                X = WaveServices.Platform.ScreenWidth / 2,
                Y = WaveServices.Platform.ScreenHeight / 2,
            });



            mine.AddComponent(new Sprite("Content/mine.wpk"));
            mine.AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            mineCounter++;


            return mine;
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
                laserUpgradeThree.AddComponent(new PerPixelCollider("Content/laserUpgrade.wpk", 0));
                laserUpgradeThree.AddComponent(new LaserUpgradeBehaviour());
                laserUpgradeThree.AddComponent(new Sprite("Content/laserUpgrade.wpk"));
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
                    Margin = new Thickness(20, 40, 0, 0),
                    Foreground = Color.Green,
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
            asteroid.AddComponent(new PerPixelCollider("Content/Asteroid.wpk", 0));
            asteroid.AddComponent(new Sprite("Content/Asteroid.wpk"));
            Animation2D animations = Animation2D.Create<TexturePackerGenericXml>("Content/Asteroid.xml");
            animations.Add("Rotate", new SpriteSheetAnimationSequence() { First = 1, Length = 26, FramesPerSecond = 12 });
            animations.Add("Break", new SpriteSheetAnimationSequence() { First = 28, Length = 34, FramesPerSecond = 6 });
            asteroid.AddComponent(animations);
            asteroid.AddComponent(new AnimatedSpriteRenderer(DefaultLayers.Alpha));
            asteroid.AddComponent(new AsteroidBehavior());
            asteroidCounter++;
            return asteroid;
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
                    X = WaveServices.Platform.ScreenWidth / 2,
                    Y = WaveServices.Platform.ScreenHeight / 2,
                });
                player.AddComponent(new Sprite("Content/Player.wpk"));
                player.AddComponent(new PerPixelCollider("Content/Player.wpk", 10));
                Animation2D animations = Animation2D.Create<TexturePackerGenericXml>("Content/Player.xml");
                animations.Add("Idle", new SpriteSheetAnimationSequence() { First = 1, Length = 1, FramesPerSecond = 5 });
                animations.Add("Left", new SpriteSheetAnimationSequence() { First = 3, Length = 1, FramesPerSecond = 5 });
                animations.Add("Right", new SpriteSheetAnimationSequence() { First = 4, Length = 1, FramesPerSecond = 5 });
                animations.Add("Break", new SpriteSheetAnimationSequence() { First = 2, Length = 1, FramesPerSecond = 5 });
                player.AddComponent(animations);
                player.AddComponent(new AnimatedSpriteRenderer(DefaultLayers.Alpha));
                player.AddComponent(new PlayerBehavior());

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
            Vector2 corner = Vector2.Zero;
            WaveServices.ViewportManager.RecoverPosition(ref corner);

            Entity background = new Entity("backGround1").AddComponent(new Transform2D()
            {
                X = corner.X,
                Y = corner.Y,
                DrawOrder = 1,
                XScale = WaveServices.ViewportManager.ScreenWidth / (WaveServices.ViewportManager.RatioX * (float)600),
                YScale = WaveServices.ViewportManager.ScreenHeight / (WaveServices.ViewportManager.RatioY * (float)800)
            });



            background.AddComponent(new Sprite("Content/SkyLvl1.wpk"));
            background.AddComponent(new SpriteRenderer(DefaultLayers.Opaque));
            background.AddComponent(new ScrollBehavior1(WaveServices.ViewportManager.ScreenHeight));


            return background;
        }


        private static Entity GetBackgroudPart2()
        {
            Vector2 corner = Vector2.Zero;
            WaveServices.ViewportManager.RecoverPosition(ref corner);

            Entity background = new Entity("backGround2").AddComponent(new Transform2D()
            {
                X = corner.X,
                Y = -650,
                DrawOrder = 1,
                XScale = WaveServices.ViewportManager.ScreenWidth / (WaveServices.ViewportManager.RatioX * (float)600),
                YScale = WaveServices.ViewportManager.ScreenHeight / (WaveServices.ViewportManager.RatioY * (float)800)
            });



            background.AddComponent(new Sprite("Content/SkyLvl1.wpk"));
            background.AddComponent(new SpriteRenderer(DefaultLayers.Opaque));
            background.AddComponent(new ScrollBehavior2(WaveServices.ViewportManager.ScreenHeight));


            return background;
        }

        public static ProyectileManager GetRedLaserManager()
        {
            if (redLaserManager == null) {
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


        public static ProyectileManager GetCurrentLaserManager()
        {
            if (textBlockForScoreInScreen == null) {
                GetScoreText();
            }

            if (GetRedLaserManager().Entity.Enabled == true)
            {
                textBlockForScoreInScreen.Foreground = Color.Red;
                return GetRedLaserManager();
            }
            else {
                textBlockForScoreInScreen.Foreground = Color.Green;
                return GetGreenLaserManager();
            }
        
        }


    }


}
