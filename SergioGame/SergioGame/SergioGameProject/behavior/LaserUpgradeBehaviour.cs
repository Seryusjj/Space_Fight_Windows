using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Media;
using WaveEngine.Components.Animation;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;

namespace SergioGameProject
{
    class LaserUpgradeBehaviour : Behavior
    {

        [RequiredComponent]
        private Animation2D anim2D;



        public int speed { get; set; }

        private const int direction = 1;//down


        private const int BORDER_OFFSET = 25;


        [RequiredComponent]
        private Transform2D trans2D;



        public LaserUpgradeBehaviour()
            : base("LaserUpgradeBehaviour")
        {
            speed = 3;
         
            trans2D = null;

            

        }

       

        private void moveElement(int gameTimeMilliseconds)
        {
            trans2D.X += 0;
            trans2D.Y += direction * speed * gameTimeMilliseconds; 
            if (trans2D.Y > WaveServices.ViewportManager.VirtualHeight) {
                Owner.Enabled = false;
            }
        }





        protected override void Update(TimeSpan gameTime)
        {


                //desplaza el asteroide
                moveElement(gameTime.Milliseconds/10);

                if (!anim2D.State.ToString().Equals("Playing"))
                {
                    anim2D.Play();
                }
            




        }
    }
}
