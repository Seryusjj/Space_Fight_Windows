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
    /// Class responsible for creating entities used in the game
    /// </summary>
    static class AssetsManager
    {
        private static int asteroidCounter=0;
       
        public static Entity GetMine()
        {

            
            Entity mine = new Entity("mine").AddComponent(new Transform2D()
            {
                X = WaveServices.Platform.ScreenWidth / 2,
                Y = WaveServices.Platform.ScreenHeight / 2,
            });



            mine.AddComponent(new Sprite("Content/mine.wpk"));
            mine.AddComponent(new SpriteRenderer(DefaultLayers.Opaque));


            return mine;
        }

        public static Entity LaserUpgradeObject() {
            Entity laserUpgrade = new Entity("laserUpgrade").AddComponent(new Transform2D()
            {
            
                X=100,
                Y=100
            });
            laserUpgrade.AddComponent(new PerPixelCollider("Content/laserUpgrade.wpk",0));
            laserUpgrade.AddComponent(new LaserUpgradeBehaviour());
            laserUpgrade.AddComponent(new Sprite("Content/laserUpgrade.wpk"));
            laserUpgrade.AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            laserUpgrade.Enabled = false;
            return laserUpgrade;
        }

        public static TextBlock GetScoreText() {
            TextBlock textblock = new TextBlock()
            {
                Margin = new Thickness(20, 40, 0, 0),
                Text = "Score: 0",
                BorderColor = Color.Green,
                Foreground = Color.Green,
            };

            return textblock;
        
        
        }


        public static Entity GetAsteroid(int intX,int initY,float ScaleX,float ScaleY)
        {
            Entity asteroid = new Entity("Asteroid"+asteroidCounter);
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



        public static Entity GetPlayer()
        {
            
            Entity player = new Entity("Player");
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
            return player;
        }


        public static Entity GetBackground() {
            Entity background = new Entity("background");
            background.AddChild(GetBackgroudPart1());
            background.AddChild(GetBackgroudPart2());
            return background;
        
        }


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


    }


}
