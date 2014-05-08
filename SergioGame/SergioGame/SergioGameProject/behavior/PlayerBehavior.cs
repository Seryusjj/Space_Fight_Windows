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

    class PlayerBehavior : Behavior
    {
        

        private const int SPEED = 5;
        private const int RIGHT = 1;
        private const int LEFT = -1;
        private const int UP = -1;
        private const int DOWN = 1;

        public enum LaserStat { OneLaser, TwoLasers, ThreeLasers }

        public LaserStat currentLaserStat{get;set;}

        private TimeSpan timeRatio;
        private TimeSpan shootRatio = TimeSpan.FromSeconds(0.5f);


        private const int NONE = 0;

        private enum AnimState { Idle, Right, Left, Up, Down, UpLeft, UpRight, DownLeft, DownRight };
        private AnimState currentState, lastState;

        public Boolean shoot;
        [RequiredComponent]
        public Animation2D anim2D;
        [RequiredComponent]
        public Transform2D trans2D;

        public PlayerBehavior()
            : base("playerBehavior")
        {
            currentLaserStat = LaserStat.OneLaser;
            this.shoot = true;
            this.anim2D = null;
            this.trans2D = null;
            this.currentState = AnimState.Idle;
        }

        private void KyboardStateSelector()
        {
            var keyboard = WaveServices.Input.KeyboardState;

            if (keyboard.Down == ButtonState.Pressed && keyboard.Left == ButtonState.Pressed)
            {
                currentState = AnimState.DownLeft;
            }
            else if (keyboard.Down == ButtonState.Pressed && keyboard.Right == ButtonState.Pressed)
            {
                currentState = AnimState.DownRight;
            }
            else if (keyboard.Up == ButtonState.Pressed && keyboard.Left == ButtonState.Pressed)
            {
                currentState = AnimState.UpLeft;
            }
            else if (keyboard.Up == ButtonState.Pressed && keyboard.Right == ButtonState.Pressed)
            {
                currentState = AnimState.UpRight;
            }
            else if (keyboard.Right == ButtonState.Pressed)
            {
                currentState = AnimState.Right;
            }
            else if (keyboard.Left == ButtonState.Pressed)
            {
                currentState = AnimState.Left;
            }
            else if (keyboard.Up == ButtonState.Pressed)
            {
                currentState = AnimState.Up;
            }
            else if (keyboard.Down == ButtonState.Pressed)
            {
                currentState = AnimState.Down;
            }
            else
            {
                currentState = AnimState.Idle;
            }
        }


        private void Shoot(TimeSpan gameTime)
        {
            if (timeRatio > TimeSpan.Zero)
            {
                timeRatio -= gameTime;
            }
            else
            {
                timeRatio = shootRatio;
                var a = trans2D.X;
                var ProyectileManager = EntityManager.Find<ProyectileManager>("ProyectileManager");
                switch (currentLaserStat) { 
                    case LaserStat.OneLaser:
                        ProyectileManager.ShootBullet(trans2D.X + trans2D.Rectangle.Width / 2, trans2D.Y - 20, 0f, -5f);
                        break;
                    case LaserStat.TwoLasers:
                        ProyectileManager.ShootBullet(trans2D.X, trans2D.Y+5, 0f, -5f);

                        ProyectileManager.ShootBullet(trans2D.X+trans2D.Rectangle.Width, trans2D.Y+5, 0f, -5f);
                        break;
                    case LaserStat.ThreeLasers:
                        ProyectileManager.ShootBullet(trans2D.X + trans2D.Rectangle.Width / 2, trans2D.Y - 20, 0f, -5f);
                        ProyectileManager.ShootBullet(trans2D.X, trans2D.Y+5, 0f, -5f);

                        ProyectileManager.ShootBullet(trans2D.X+trans2D.Rectangle.Width, trans2D.Y+5, 0f, -5f);
                        break;

                
                
                }

                //WaveServices.MusicPlayer.Play(SoundManager.getLaserShotMusic());
                 //dispara al medio

             

            }

        }

        private void ChageState()
        {
            if (currentState != lastState)
            {
                switch (currentState)
                {
                    case AnimState.UpRight:
                        anim2D.CurrentAnimation = "Right";
                        anim2D.Play(true);
                        break;
                    case AnimState.UpLeft:
                        anim2D.CurrentAnimation = "Left";
                        anim2D.Play(true);
                        break;
                    case AnimState.DownRight:
                        anim2D.CurrentAnimation = "Right";
                        anim2D.Play(true);
                        break;
                    case AnimState.DownLeft:
                        anim2D.CurrentAnimation = "Left";
                        anim2D.Play(true);
                        break;
                    case AnimState.Idle:
                        anim2D.CurrentAnimation = "Idle";
                        anim2D.Play(true);
                        break;
                    case AnimState.Right:
                        anim2D.CurrentAnimation = "Right";
                        anim2D.Play(true);
                        break;
                    case AnimState.Left:
                        anim2D.CurrentAnimation = "Left";
                        anim2D.Play(true);
                        break;
                    case AnimState.Up:
                        anim2D.CurrentAnimation = "Idle";
                        anim2D.Play(true);
                        break;
                    case AnimState.Down:
                        anim2D.CurrentAnimation = "Idle";
                        anim2D.Play(true);
                        break;

                }
            }
            lastState = currentState;
        }


        private void CheckHightLimits()
        {

            if (trans2D.Y < 0)
            {
                trans2D.Y = 0;
            }
            else if (trans2D.Y > WaveServices.ViewportManager.VirtualHeight - trans2D.Rectangle.Height)
            {
                trans2D.Y = WaveServices.ViewportManager.VirtualHeight - trans2D.Rectangle.Height;
            }
        }

        private void CheckWidthLimits()
        {
            if (trans2D.X < 0)
            {
                trans2D.X = 0;
            }
            else if (trans2D.X > WaveServices.ViewportManager.VirtualWidth - trans2D.Rectangle.Width)
            {
                trans2D.X = WaveServices.ViewportManager.VirtualWidth - trans2D.Rectangle.Width;
            }
        }
        private void CheckBorders()
        {
            //00 es arriba a la izquierda
            CheckHightLimits();
            // Check borders X
            CheckWidthLimits();

        }

        private void Move(int gameTimeMilliseconds)
        {
            switch (currentState)
            {
                case AnimState.Idle:
                    break;
                case AnimState.Right:
                    trans2D.X += RIGHT * SPEED * (gameTimeMilliseconds);
                    break;
                case AnimState.Left:
                    trans2D.X += LEFT * SPEED * (gameTimeMilliseconds);
                    break;
                case AnimState.Up:
                    trans2D.Y += UP * SPEED * (gameTimeMilliseconds);
                    break;
                case AnimState.Down:
                    trans2D.Y += DOWN * SPEED * (gameTimeMilliseconds);
                    break;
                case AnimState.UpRight:
                    trans2D.Y += UP * SPEED * (gameTimeMilliseconds);
                    trans2D.X += RIGHT * SPEED * (gameTimeMilliseconds);
                    break;
                case AnimState.UpLeft:
                    trans2D.Y += UP * SPEED * (gameTimeMilliseconds);
                    trans2D.X += LEFT * SPEED * (gameTimeMilliseconds);
                    break;
                case AnimState.DownRight:
                    trans2D.Y += DOWN * SPEED * (gameTimeMilliseconds);
                    trans2D.X += RIGHT * SPEED * (gameTimeMilliseconds);
                    break;
                case AnimState.DownLeft:
                    trans2D.Y += DOWN * SPEED * (gameTimeMilliseconds);
                    trans2D.X += LEFT * SPEED * (gameTimeMilliseconds);
                    break;
            }
        }
        protected override void Update(TimeSpan gameTime)
        {
            KyboardStateSelector();
            // Set current animation if that one is diferent
            ChageState();

            Move(gameTime.Milliseconds / 10);
            if (shoot)
            {
                Shoot(gameTime);
            }

            CheckBorders();
        }



    }





}

