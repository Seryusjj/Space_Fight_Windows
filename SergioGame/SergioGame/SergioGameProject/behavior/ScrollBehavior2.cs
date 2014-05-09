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

    public class ScrollBehavior2 : Behavior
    {

        private static float scrollSpeed=300;


        private float sceneHeight;

        
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


        public ScrollBehavior2(float height)
            : base("ScrollBehavior2")
        {
            this.sceneHeight = height;
        
            
        }
        protected override void Update(TimeSpan gameTime)
        {
            // Adds the lateral scroll speed.
            var transform2D = this.Owner.FindComponent<Transform2D>();



            transform2D.Y += (float)(ScrollSpeed * gameTime.TotalSeconds);


            if (transform2D.Y >= 0)
            {
                transform2D.Y = -sceneHeight;               

            }


        }
    }



}

