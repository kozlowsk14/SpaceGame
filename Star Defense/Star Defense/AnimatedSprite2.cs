using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Star_Defense
{
    class AnimatedSprite2 : AnimatedSprite
    {
        Texture2D[] textures;
        public int index = 0;
        public AnimatedSprite2(
            Texture2D[] texture,
            int FrameOffsetX,
            int FrameOffsetY,
            int FrameWidth,
            int FrameHeight,
            int FrameCount,
            int i)
            : base(texture[0], FrameOffsetX, FrameOffsetY, FrameWidth, FrameHeight, FrameCount)
        {
            textures = texture;
            index = i;
        }
        public void Draw(SpriteBatch spriteBatch,
            int XOffset,
            int YOffset,
            bool NeedBeginEnd)
        {
            if (NeedBeginEnd)
                spriteBatch.Begin();
            spriteBatch.Draw(
                textures[index],
                new Rectangle(
                  iScreenX + XOffset,
                  iScreenY + YOffset,
                  iFrameWidth,
                  iFrameHeight),
                GetSourceRect(),
                Color.White);

            if (NeedBeginEnd)
                spriteBatch.End();

        }
    }
}
