#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Media;
using WaveEngine.Components;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace SergioGameProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);




            // Use ViewportManager to ensure scaling in all devices
            WaveServices.ViewportManager.Activate(550,650, ViewportManager.StretchMode.Uniform);



            //-- Play background music --//
            MusicPlayer player = WaveServices.MusicPlayer;
            player.IsRepeat = true;
            player.Play(SoundManager.getGameLoopSound());



            // GameBackContext is visible always at the background. 
            // For this reason the behavior is set to Draw and Update when the scene is in background.
            var backContext = new ScreenContext("GameBackContext", new GameScene());
            backContext.Behavior = ScreenContextBehaviors.DrawInBackground | ScreenContextBehaviors.UpdateInBackground;
           


            WaveServices.ScreenContextManager.Push(backContext);
            WaveServices.ScreenContextManager.Push(new ScreenContext("MenuContext", new MenuScene()));//cuando haga pop pasamos al juego

            

        }

        


    }
}
