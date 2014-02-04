using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Star_Defense
{
    class Background
    {
        // Textures to hold the two background images
        Texture2D t2dBackground, t2dParallax, t2dShopScreen;

        int iViewportWidth = 720;
        int iViewportHeight = 1920;

        int iBackgroundWidth = 720;
        int iBackgroundHeight = 1920;

        int iParallaxWidth = 720;
        int iParallaxHeight = 1680;

        int iShopHeight = 720;
        int iShopWidth = 720;

        int iBackgroundOffset;
        int iParallaxOffset;

        Boolean isShop = false;
        public bool IsShop
        {
            get { return isShop; }
            set { isShop = value; }
        }

        public int BackgroundOffset
        {
            get { return iBackgroundOffset; }
            set
            {
                iBackgroundOffset = value;
                if (iBackgroundOffset < 0)
                {
                    iBackgroundOffset += iBackgroundHeight;
                }
                if (iBackgroundOffset > iBackgroundHeight)
                {
                    iBackgroundOffset -= iBackgroundHeight;
                }
            }
        }

        public int ParallaxOffset
        {
            get { return iParallaxOffset; }
            set
            {
                iParallaxOffset = value;
                if (iParallaxOffset < 0)
                {
                    iParallaxOffset += iParallaxHeight;
                }
                if (iParallaxOffset > iParallaxHeight)
                {
                    iParallaxOffset -= iParallaxHeight;
                }
            }
        }

        // Determines if we will draw the Parallax overlay.
        bool drawParallax = true;

        public bool DrawParallax
        {
            get { return drawParallax; }
            set { drawParallax = value; }
        }

        // Constructor when passed a Content Manager and two strings
        public Background(ContentManager content,
                          string sBackground,
                          string sParallax,
                          string sShopScreen)
        {

            t2dBackground = content.Load<Texture2D>(sBackground);
            iBackgroundWidth = t2dBackground.Width;
            iBackgroundHeight = t2dBackground.Height;
            t2dParallax = content.Load<Texture2D>(sParallax);
            iParallaxWidth = t2dParallax.Width;
            iParallaxHeight = t2dParallax.Height;
            t2dShopScreen = content.Load<Texture2D>(sShopScreen);
            iShopWidth = t2dShopScreen.Width;
            iShopHeight = t2dShopScreen.Height;
        }

        // Constructor when passed a content manager and a single string
        public Background(ContentManager content, string sBackground)
        {

            t2dBackground = content.Load<Texture2D>(sBackground);
            iBackgroundWidth = t2dBackground.Width;
            iBackgroundHeight = t2dBackground.Height;
            t2dParallax = t2dBackground;
            iParallaxWidth = t2dParallax.Width;
            iParallaxHeight = t2dParallax.Height;
            drawParallax = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            System.Diagnostics.Debug.WriteLine("Check si shop: " + isShop.ToString());
            // Draw the background panel, offset by the player's location
            if (isShop )
            {
                spriteBatch.Draw(
                    t2dShopScreen,
                    new Rectangle(0,
                                  0, iShopWidth,
                                  iShopHeight),
                    Color.White);
            }
            else
            {
                spriteBatch.Draw(
                    t2dBackground,
                    new Rectangle(0,
                                  -1 * iBackgroundOffset, iBackgroundWidth,
                                  iViewportHeight),
                    Color.White);

                // If the right edge of the background panel will end 
                // within the bounds of the display, draw a second copy 
                // of the background at that location.
                if (iBackgroundOffset > iBackgroundHeight - iViewportHeight)
                {
                    spriteBatch.Draw(
                        t2dBackground,
                        new Rectangle(
                          0,
                          (-1 * iBackgroundOffset) + iBackgroundHeight,
                          iBackgroundWidth,
                          iViewportHeight),
                        Color.White);
                }

                if (drawParallax && false)
                {
                    // Draw the parallax star field
                    spriteBatch.Draw(
                        t2dParallax,
                        new Rectangle(0,
                                      -1 * iParallaxOffset, iParallaxWidth,
                                      iViewportHeight),
                        Color.SlateGray);
                    // if the player is past the point where the star 
                    // field will end on the active screen we need 
                    // to draw a second copy of it to cover the 
                    // remaining screen area.
                    if (iParallaxOffset > iParallaxHeight - iViewportHeight)
                    {
                        spriteBatch.Draw(
                            t2dParallax,
                            new Rectangle(
                              0,
                              (-1 * iParallaxOffset) + iParallaxHeight,
                              iParallaxWidth,
                              iViewportHeight),
                            Color.SlateGray);
                    }
                }
            }
        }
    }
}
