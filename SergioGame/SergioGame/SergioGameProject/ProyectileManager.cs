﻿#region Usings Statements
using SergioGameProject.behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Components.Particles;
using WaveEngine.Materials;
#endregion

namespace SergioGameProject
{
    public class ProyectileManager : BaseDecorator
    {
        public readonly int numBullets = 20;
        private int bulletIndex;
        public Proyectiles selectedBullet = Proyectiles.greenLaser;
        public enum Proyectiles { greenLaser, redLaser }
        private int BulletIndex
        {
            get
            {
                bulletIndex = ++bulletIndex % numBullets;
                return bulletIndex;
            }
        }



        private Entity Initialize() {
            Entity entity = new Entity("ProyectileManager");

            for (int i = 0; i < numBullets; i++)
            {
                Entity toAdd = selectBullet("bullet" + i);
                toAdd.Enabled = false;
                entity.AddChild(toAdd);

            }

            for (int i = 0; i < numBullets; i++)
            {
                Entity toAdd = selectBulletLight("light" + i);
                toAdd.Enabled = false;
                entity.AddChild(toAdd);

            }
            return entity;
        }



        public ProyectileManager()
            : base()
        {
            this.entity = Initialize();
        }

        private Entity selectBullet(String tag)
        {
            Entity bullet = CreateRedLaser(tag);//default case
            switch (selectedBullet)
            {
                case Proyectiles.greenLaser:
                    bullet = CreateGreenLaser(tag);
                    break;
                case Proyectiles.redLaser:
                    bullet = CreateRedLaser(tag);
                    break;
            }
            return bullet;
        }


        private Entity selectBulletLight(String tag)
        {
            Entity bullet = CreateRedLaserLight(tag);//default case
            switch (selectedBullet)
            {
                case Proyectiles.greenLaser:
                    bullet = CreateGreenLaserLight(tag);
                    break;
                case Proyectiles.redLaser:
                    bullet = CreateRedLaserLight(tag);
                    break;
            }
            return bullet;
        }




        public void ShootBullet(float initX, float initY, float velocityX, float velocityY)
        {
            Entity bullet = this.entity.ChildEntities.ElementAt(BulletIndex);
            Entity shotLight = this.entity.ChildEntities.ElementAt(BulletIndex + numBullets);


            var bulletTransform = bullet.FindComponent<Transform2D>();
            bulletTransform.X = initX;
            bulletTransform.Y = initY;

            var lightTransform = shotLight.FindComponent<Transform2D>();
            lightTransform.X = initX - lightTransform.Rectangle.Width / 2;
            lightTransform.Y = initY - lightTransform.Rectangle.Height / 2;


            var bulletBehavior = bullet.FindComponent<ProyectileBehavior>();
            bulletBehavior.SpeedX = velocityX;
            bulletBehavior.SpeedY = velocityY;

            shotLight.Enabled = true;
            bullet.Enabled = true;
        }

        private Entity CreateRedLaser(String tag)
        {
            return new Entity(tag)
                .AddComponent(new Transform2D()
                {
                    Origin = Vector2.Center,
                    DrawOrder = 0.6f,
                })
                 .AddComponent(new PerPixelCollider("Content/Proyectiles/laserRed.wpk", 0))
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/Proyectiles/laserRed.wpk"))
                .AddComponent(new ProyectileBehavior())
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
        }

        private Entity CreateGreenLaser(String tag)
        {

            Entity proyectile = new Entity(tag)
             .AddComponent(new Transform2D()
             {
                 Origin = Vector2.Center,
                 DrawOrder = 0.6f,
             })
             .AddComponent(new PerPixelCollider("Content/Proyectiles/laserGreen.wpk",0))
             .AddComponent(new RectangleCollider())
             .AddComponent(new Sprite("Content/Proyectiles/laserGreen.wpk"))
             .AddComponent(new ProyectileBehavior())
             .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            return proyectile;
        }


        public Entity CreateGreenLaserLight(String tag)
        {
            Entity shotLight = new Entity(tag);
            shotLight.AddComponent(new Transform2D()
            {
                DrawOrder = 0.6f,
            });

            shotLight.AddComponent(new Sprite("Content/Proyectiles/laserGreenShot.wpk"));
            shotLight.AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            shotLight.AddComponent(new LaserLightBehavior());
            return shotLight;
        }

        public Entity CreateRedLaserLight(String tag)
        {
            Entity shotLight = new Entity(tag);
            shotLight.AddComponent(new Transform2D()
            {
                DrawOrder = 0.6f,
            });
            shotLight.AddComponent(new Sprite("Content/Proyectiles/laserRedShot.wpk"));
            shotLight.AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            shotLight.AddComponent(new LaserLightBehavior());
            return shotLight;
        }
    }
}


