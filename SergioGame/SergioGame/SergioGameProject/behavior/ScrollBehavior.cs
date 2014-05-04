using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace SergioGameProject
{

    public class ScrollBehavior1 : Behavior
    {
        private static int instances = 0;
        private static float scrollSpeed=300;
        private const float MAXSPEED = 0.5f;

        private float sceneHeight=650;

        
        public static float ScrollSpeed
        {
            get
            {
                return scrollSpeed;
            }

            set
            {
                scrollSpeed = value;
            }
        }


        public ScrollBehavior1(float height)
            : base("ObstacleBehavior_" + instances++)
        {
            this.sceneHeight = height;
        
            
        }
        protected override void Update(TimeSpan gameTime)
        {
            // Adds the lateral scroll speed.
            var transform2D = this.Owner.FindComponent<Transform2D>();



            transform2D.Y += (float)(ScrollSpeed * gameTime.TotalSeconds);


            if (transform2D.Y > sceneHeight)
            {
                transform2D.Y = 0;               

            }


        }
    }



}

