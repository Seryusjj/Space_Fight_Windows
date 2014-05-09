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
    public class MenuScene : Scene
    {

        public int maxAsteroids = 10;
        public int puntos = 0;
        public Boolean isPlayerDestroyed = false;
        public Entity asteroid = null;


        protected override void CreateScene()
        {
            this.RenderManager.ClearFlags = ClearFlags.DepthAndStencil;
            this.CreateUI();
        }

        /// <summary>
        /// Creates the UI elements.
        /// </summary>
        private void CreateUI()
        {
            var button = AssetsManager.CreatePlayButton(WaveServices.ViewportManager.VirtualWidth / 2, WaveServices.ViewportManager.VirtualHeight / 2);
            button.Click += (o, e) =>
            {
                WaveServices.ScreenContextManager.Pop();
                GameScene scene = WaveServices.ScreenContextManager.FindContextByName("GameBackContext").FindScene<GameScene>();
                AssetsManager.GetPlayer().Enabled = true;
                AssetsManager.GetPlayer().FindChild("PlayerShield").Enabled = true;
                //Entity player = scene.EntityManager.Find("Player");


                WaveServices.TimerFactory.CreateTimer("Timer1", TimeSpan.FromSeconds(5),
                () =>
                {
                    AssetsManager.GetPlayer().FindChild("PlayerShield").Enabled = false;
                    scene.canBeDestroyed = true;

                });
            };
            this.EntityManager.Add(button);
            var a = WaveServices.ViewportManager.VirtualHeight / 2 - button.Height;
            this.EntityManager.Add(AssetsManager.CreateMainMenuBackground(a));
        }


    }
}

