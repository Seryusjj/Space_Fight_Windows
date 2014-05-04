using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Components.Animation;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace SergioGameProject
{
    class AsteroidBehavior : Behavior
    {
        private const int SPEED = 1;
        private const int RIGHT = 1;
        private const int LEFT = -1;
        private const int NONE = 0;
        private const int BORDER_OFFSET = 25;

        [RequiredComponent]
        private Animation2D anim2D;
        [RequiredComponent]
        private Transform2D trans2D;

        private Boolean breaked;

        public AsteroidBehavior()
            : base("AsteroidBehavior")
        {
            anim2D = null;
            trans2D = null;
            breaked = false;
        }

        public void breakAsteroidtest()
        {
            var keyboard = WaveServices.Input.KeyboardState;
            if (keyboard.Space == ButtonState.Pressed)
            {

                breaked = true;
            }
        }

        public void breakAsteroid()
        {

            breaked = true;

        }



        protected override void Update(TimeSpan gameTime)
        {

            //breakAsteroidtest();

            if (breaked && anim2D.CurrentAnimation != "Break")
            {
                anim2D.CurrentAnimation = "Break";
            }

            if (anim2D.CurrentAnimation.Equals("Break"))
            {
                if (!anim2D.State.ToString().Equals("Playing"))
                {
                    anim2D.CurrentAnimation = "Rotate";
                    Owner.Enabled = false;
                    breaked = false;
                }
            }


            else
            {
                if (!anim2D.State.ToString().Equals("Playing"))
                {
                    anim2D.Play();
                }
            }




        }
    }
}
