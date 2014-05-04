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
     
    class LaserLightBehavior : Behavior
    {

        private TimeSpan shootRatio = TimeSpan.FromSeconds(0.1f);
        public LaserLightBehavior()
            : base()
        {
        }


        protected override void Update(TimeSpan gameTime)
        {
            if (shootRatio > TimeSpan.Zero)
            {
                shootRatio -= gameTime;
            }
            else
            {
                shootRatio = TimeSpan.FromSeconds(0.1f);
                Owner.Enabled = false;
            }
            
        }


    }
}
