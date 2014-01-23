using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Star_Defense
{
    class PowerUp
    {
        static int iMapWidth = 1920;
        static Color[] colorPowerUpColors = new Color[5] 
          { Color.White, Color.Aquamarine, Color.Maroon, 
            Color.Yellow, Color.Orange };

        AnimatedSprite asSprite;
        int iX = 0;
        int iY = -100;
        bool bActive = false;
        int iBackgroundOffset = 0;
        int iPowerUpType = 0;

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
            set { bActive = value; }
        }

        public int Offset
        {
            get { return iBackgroundOffset; }
            set { iBackgroundOffset = value; }
        }

        public int PowerUpType
        {
            get { return iPowerUpType; }
            set
            {
                iPowerUpType = value;
                asSprite.Tint = colorPowerUpColors[iPowerUpType];
            }
        }

        public Rectangle BoundingBox
        {
            get
            {
                int X = iX - iBackgroundOffset;
                if (X > iMapWidth)
                    X -= iMapWidth;
                if (X < 0)
                    X += iMapWidth;
                return new Rectangle(X, iY, 32, 32);
            }
        }

        public PowerUp(Texture2D texture)
        {
            asSprite = new AnimatedSprite(texture, 0, 0, 32, 32, 23);
        }

        public void Activate()
        {
            bActive = true;
        }

        private int GetDrawX()
        {
            int X = iX - iBackgroundOffset;
            if (X > iMapWidth)
                X -= iMapWidth;
            if (X < 0)
                X += iMapWidth;

            return X;
        }

        public void Update(GameTime gametime, int iOffset)
        {
            if (bActive)
            {
                asSprite.Update(gametime);
                iBackgroundOffset = iOffset;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (bActive)
            {
                asSprite.Draw(sb, GetDrawX(), iY, false);
            }
        }


    }
}
