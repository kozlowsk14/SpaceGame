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

        static int iMapWidth = 720;
        static int iMapHeight = 720;
        static int iPlayAreaTop = 30;
        static int iPlayAreaBottom = 630;
        static Random rndGen = new Random();
        int iX = 0;
        int iY = -100;
        int iBackgroundOffset = 0;
        Vector2 v2motion = new Vector2(0f, 0f);
        float fSpeed = 1f;
        float fEnemyMoveCount = 0.0f;
        float fEnemyDelay = 0.01f;
        bool bActive = false;
        //constant to figure out if going left to right or vice versa
        int iDirection = 0;

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
                return new Rectangle(Y, iX, 32, 32);
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
              int X, int Y, int W, int H, int Frames, int dir)
        {
            asSprite = new AnimatedSprite(texture, X, Y, W, H, Frames);
            if (dir == 0)
            {
                iDirection = -1;
                System.Diagnostics.Debug.Write("direction is negative\n");
            }
            else
            {
                iDirection = 1;
                System.Diagnostics.Debug.Write("direction is positive\n");
            }
        }

        public void Deactivate()
        {
            bActive = false;
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
            v2motion.Y = rndGen.Next(-50, 50);
            v2motion.X = rndGen.Next(0, 50);
            v2motion.Normalize();
            fSpeed = (float)(rndGen.Next(3, 6));
        }

        public void Generate(int iLocation, int iShipX)
        {
            iBackgroundOffset = iLocation;
            if (iDirection == 1)
            {
                iX = iMapWidth;
            }
            else
            {
                iX = 0;
            }
            // Generate a random Y location between iPlayAreaTop 
            // and iPlayAreaBottom (the area of our game screen)

            iY = rndGen.Next(iMapHeight);
            RandomizeMovement();
            bActive = true;
        }

        public void Draw(SpriteBatch sb, int iLocation)
        {
            if (bActive)
                asSprite.Draw(sb, iX, GetDrawY(), false);
        }

        public void Update(GameTime gametime, int iOffset)
        {
            iBackgroundOffset = iOffset;

            fEnemyMoveCount += (float)gametime.ElapsedGameTime.TotalSeconds;
            if (fEnemyMoveCount > fEnemyDelay)
            {
                System.Diagnostics.Debug.Write("" + iX);
                iX += (int)((float)v2motion.X * fSpeed * iDirection);
                System.Diagnostics.Debug.Write("" + iX);
                iY += (int)((float)v2motion.Y * fSpeed);

                if (rndGen.Next(200) == 1)
                {
                    RandomizeMovement();
                }

                if (iY < iPlayAreaTop)
                {
                    iY = iPlayAreaTop;
                    RandomizeMovement();
                }

                if (iY > iPlayAreaBottom)
                {
                    iY = iPlayAreaBottom;
                    RandomizeMovement();
                }

                if (iX < 0)
                    iX += iMapWidth;

                if (iX > iMapWidth)
                    iX -= iMapWidth;

                fEnemyMoveCount = 0f;
            }
            asSprite.Update(gametime);
        }
    }
}
