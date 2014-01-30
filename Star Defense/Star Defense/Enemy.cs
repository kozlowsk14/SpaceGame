using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Star_Defense
{
    class Enemy
    {
        AnimatedSprite asSprite;

        static int iMapHeight = 1920;
        static int iPlayAreaLeft = 30;
        static int iPlayAreaRight = 700;
        static Random rndGen = new Random();
        int iX = 0;
        int iY = -100;
        int iBackgroundOffset = 0;
        Vector2 v2motion = new Vector2(0f, 0f);
        float fSpeed = 1f;
        float fEnemyMoveCount = 0.0f;
        float fEnemyDelay = 0.01f;
        bool bActive = false;

        public int X
        {
            get { return iX; }
            set { iX = value; }
        }

        public int Y
        {
            get { return iY; }
            set { iY = value; }
        }

        public bool IsActive
        {
            get { return bActive; }
        }

        public Rectangle BoundingBox
        {
            get
            {
                int Y = iY - iBackgroundOffset;
                if (Y > iMapHeight)
                    Y -= iMapHeight;
                if (Y < 0)
                    Y += iMapHeight;
                return new Rectangle(iX, Y, 32, 32);
            }
        }

        public int Offset
        {
            get { return iBackgroundOffset; }
            set { iBackgroundOffset = value; }
        }

        public float Speed
        {
            get { return fSpeed; }
        }

        public Vector2 Motion
        {
            get { return v2motion; }
            set { v2motion = value; }
        }

        public Rectangle CollisionBox
        {
            get
            {
                int Y = iY - iBackgroundOffset;
                if (Y > iMapHeight)
                    Y -= iMapHeight;
                if (Y < 0)
                    Y += iMapHeight;
                return new Rectangle(iX + 2, Y + 2, 28, 28);
            }
        }

        public Enemy(Texture2D texture,
              int X, int Y, int W, int H, int Frames)
        {
            asSprite = new AnimatedSprite(texture, X, Y, W, H, Frames);
        }

        public void Deactivate()
        {
            bActive = false;
        }

        private int GetDrawX()
        {
            return iX;
        }

        private int GetDrawY()
        {
            int Y = iY - iBackgroundOffset;
            if (Y > iMapHeight)

                Y -= iMapHeight;
            if (Y < 0)
                Y += iMapHeight;

            return Y;
        }

        public void RandomizeMovement()
        {
            v2motion.X = rndGen.Next(-20, 20);
            v2motion.Y = rndGen.Next(-2, 2);
            v2motion.Normalize();
            fSpeed = (float)(rndGen.Next(3, 6));
        }

        public void Generate(int iLocation, int iShipY)
        {
            // Generate a random X location that is NOT 
            // within 200 pixels of the player's ship.
            do
            {
                iY = rndGen.Next(iMapHeight);
                iBackgroundOffset = iLocation;
            } while (Math.Abs(GetDrawY() - iShipY) < 200);

            // Generate a random Y location between iPlayAreaLeft 
            // and iPlayAreaRight (the area of our game screen)

            iX = rndGen.Next(iPlayAreaLeft, iPlayAreaRight);
            RandomizeMovement();
            bActive = true;
        }

        public void Draw(SpriteBatch sb, int iLocation)
        {
            if (bActive)
                asSprite.Draw(sb, GetDrawX(), GetDrawY(), false);
        }

        public void Update(GameTime gametime, int iOffset)
        {
            iBackgroundOffset = iOffset;

            fEnemyMoveCount += (float)gametime.ElapsedGameTime.TotalSeconds;
            if (fEnemyMoveCount > fEnemyDelay)
            {
                iX += (int)((float)v2motion.X * fSpeed);
                iY += (int)((float)v2motion.Y * fSpeed);

                if (rndGen.Next(200) == 1)
                {
                    RandomizeMovement();
                }

                if (iX < iPlayAreaLeft)
                {
                    iX = iPlayAreaLeft;
                    RandomizeMovement();
                }

                if (iX > iPlayAreaRight)
                {
                    iX = iPlayAreaRight;
                    RandomizeMovement();
                }

                if (iY < 0)
                    iY += iMapHeight;

                if (iY > iMapHeight)
                    iY -= iMapHeight;

                fEnemyMoveCount = 0f;
            }
            asSprite.Update(gametime);
        }
    }
}
