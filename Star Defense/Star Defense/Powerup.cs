using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Star_Defense
{
    public class PowerUp
    {
        static int iMapHeight = 1920;
        static Color[] colorPowerUpColors = new Color[31] 
          { Color.Green, Color.Aquamarine, Color.Maroon, 
            Color.Yellow, Color.Orange, Color.Red,Color.YellowGreen,Color.SteelBlue,
            Color.SandyBrown,Color.MistyRose,Color.White,Color.White,Color.White,Color.White,
            Color.White,Color.White,Color.White,Color.White,Color.White,Color.White,Color.White,Color.White,Color.White,Color.White,Color.White,Color.White,
            Color.White,Color.White,Color.White,Color.White,Color.White};

        AnimatedSprite2 asSprite;
        int iX = 0;
        int iY = -100;
        bool bActive = false;
        int iBackgroundOffset = 0;
        int iPowerUpType = 0;
        bool isSelected = false;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
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
                asSprite.index = iPowerUpType;
            }
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

        public PowerUp(Texture2D[] texture,int i)
        {
            asSprite = new AnimatedSprite2(texture, 0, 0, 32, 32, 1, i);
        }

        public void Activate()
        {
            bActive = true;
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

        public void Update(GameTime gametime, int iOffset)
        {
            if (bActive)
            {
                asSprite.Update(gametime);
                iBackgroundOffset = iOffset;
                //System.Diagnostics.Debug.WriteLine(GetDrawY());
                if (GetDrawY() >= iMapHeight-20)
                {
               
                    bActive = false;
                 
                }
                    
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (bActive)
            {
                asSprite.Draw(sb, GetDrawX(), GetDrawY(), false);
            }
        }


    }
}
