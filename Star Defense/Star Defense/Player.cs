using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Star_Defense
{
    class Player
    {
        AnimatedSprite asSprite;
        int iX = 360;
        int iY = 600;
        int iFacing = 0;
        bool bThrusting = false;
        int iScrollRate = 0;
        int iShipAccelerationRate = 1;
        int iShipHorMoveRate = 7;
        public int iShipVerMoveRate = 7;
        float fSpeedChangeCount = 0.0f;
        float fSpeedChangeDelay = 0.1f;
        float fVerticalChangeCount = 0.0f;
        float fVerticalChangeDelay = 0.01f;

        float[] fFireRateDelay = new float[3] { 0.15f, 0.1f, 0.05f };
        float fSuperBombDelayTimer = 2f;

        int iMaxSuperBombs = 5;
        int iMaxWeaponLevel = 1;
        int iShipMaxFireRate = 2;
        int iMaxAccelerationModifier = 5;

        int iSuperBombs = 2;
        int iWeaponLevel = 0;
        int iWeaponFireRate = 0;
        int iAccelerationModifier = 1;

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

        public int Facing
        {
            get { return iFacing; }
            set { iFacing = value; }
        }

        public bool Thrusting
        {
            get { return bThrusting; }
            set { bThrusting = value; }
        }

        public int ScrollRate
        {
            get { return iScrollRate; }
            set { iScrollRate = value; }
        }

        public int AccelerationRate
        {
            get { return iShipAccelerationRate * iAccelerationModifier; }
        }

        public int HorMovementRate
        {
            get { return iShipHorMoveRate; }
            set { iShipHorMoveRate = value; }
        }

        public float SpeedChangeCount
        {
            get { return fSpeedChangeCount; }
            set { fSpeedChangeCount = value; }
        }

        public float SpeedChangeDelay
        {
            get { return fSpeedChangeDelay; }
            set { fSpeedChangeDelay = value; }
        }

        public float VerticalChangeCount
        {
            get { return fVerticalChangeCount; }
            set { fVerticalChangeCount = value; }
        }

        public float VerticalChangeDelay
        {
            get { return fVerticalChangeDelay; }
            set { fVerticalChangeDelay = value; }
        }

        public Rectangle BoundingBox
        {
            get { return new Rectangle(iX, iY, 72, 16); }
        }

        public int SuperBombs
        {
            get { return iSuperBombs; }
            set
            {
                iSuperBombs = (int)MathHelper.Clamp(value,
                  0, iMaxSuperBombs);
            }
        }

        public int FireRate
        {
            get { return iWeaponFireRate; }
            set
            {
                iWeaponFireRate = (int)MathHelper.Clamp(value,
                  0, iShipMaxFireRate);
            }
        }

        public float FireDelay
        {
            get { return fFireRateDelay[iWeaponFireRate]; }
        }

        public int WeaponLevel
        {
            get { return iWeaponLevel; }
            set
            {
                iWeaponLevel = (int)MathHelper.Clamp(value,
                  0, iMaxWeaponLevel);
            }
        }

        public float SuperBombDelay
        {
            get { return fSuperBombDelayTimer; }
        }

        public int AccelerationBonus
        {
            get { return iAccelerationModifier; }
            set
            {
                iAccelerationModifier = (int)MathHelper.Clamp(value,
                  1, iMaxAccelerationModifier);
            }
        }

        public Player(Texture2D texture)
        {
            asSprite = new AnimatedSprite(texture, 0, 0, 72, 16, 4);
            asSprite.IsAnimating = false;
        }

        public void Reset()
        {
            iAccelerationModifier = 1;
            iWeaponFireRate = 0;
            iWeaponLevel = 0;
            iSuperBombs = (int)MathHelper.Max(1, iSuperBombs);
            iScrollRate = 0;
            iFacing = 0;
        }

        public void Draw(SpriteBatch sb)
        {
            asSprite.Draw(sb, iX, iY, false);
        }

        public void Update(GameTime gametime)
        {
            if (iFacing == 0)
            {
                if (bThrusting)
                    asSprite.Frame = 1;
                else
                    asSprite.Frame = 0;
            }
            else
            {
                if (bThrusting)
                    asSprite.Frame = 3;
                else
                    asSprite.Frame = 2;
            }
        }
    }
}
