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
    class AsteroidBehavior : Behavior
    {



        public int speed { get; set; }

        private const int Xdirection = 0;


        private const int BORDER_OFFSET = 25;

        private SoundInstance breakingSound;

        [RequiredComponent]
        private Animation2D anim2D;
        [RequiredComponent]
        private Transform2D trans2D;

        private Boolean breaked;

        public AsteroidBehavior()
            : base("AsteroidBehavior")
        {
            speed = 1;
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


        private void moveAsteroid(int gameTimeMilliseconds)
        {
            trans2D.X += Xdirection * speed * gameTimeMilliseconds;
            trans2D.Y += 1 * speed * gameTimeMilliseconds;
            if (trans2D.Y > WaveServices.ViewportManager.VirtualHeight) {
                Owner.Enabled = false;
            }
        }

        public void breakAsteroid()
        {
        //    if (breakingSound != null)
         //   {
                
                   // breakingSound.Parent.DestroyInstance(breakingSound);
                  //    breakingSound.Stop();
                  //     breakingSound.Dispose();                        
                
         //   }
         //   else
         //   {
                breakingSound = WaveServices.SoundPlayer.Play(SoundManager.getRockBrakingSound());
            //}
           
            /*else
            {
                
                {
                    
                }
                else
                {
                    breakingSound.Play();
                }
            }
            String a =breakingSound.State.ToString();*/
            
            breaked = true;

        }



        protected override void Update(TimeSpan gameTime)
        {

            
            //si esta roto y tiene la aimacion de rotar, poner la anim de romper
            if (breaked && anim2D.CurrentAnimation != "Break")
            {
                anim2D.CurrentAnimation = "Break";
            }

            //si ya tiene la anim de romper y ha terminado
            //eliminar el asteroide de la escena
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
                //si la animacion llega al final reproducela de nuevo
                if (!anim2D.State.ToString().Equals("Playing"))
                {
                    anim2D.Play();
                }
                //desplaza el asteroide
                moveAsteroid(gameTime.Milliseconds/10);

            }




        }
    }
}
