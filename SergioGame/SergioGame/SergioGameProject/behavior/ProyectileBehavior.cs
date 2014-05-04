using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace SergioGameProject.behavior
{
     
    class ProyectileBehavior : Behavior
    {
        public float SpeedX;
        public float SpeedY;

        [RequiredComponent]
        private Transform2D transform;


        public ProyectileBehavior() : base() {
            this.SpeedY = 10;
        }


        protected override void Update(TimeSpan gameTime)
        {
            float time = (float)gameTime.TotalMilliseconds / 10f;

            transform.Y += SpeedY * time;
  


            if (transform.Y < 0 )
            {
                Owner.Enabled = false;
            }
        }


    }
}
