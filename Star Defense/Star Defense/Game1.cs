using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Star_Defense
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AnimatedSprite Explosion;
        Background background;

        Player player;
        public int iPlayAreaTop = 30;
        public int iPlayAreaBottom = 630;
        int iMaxVertSpeed = 4;
        float fBoardUpdateDelay = 0f;
        float fBoardUpdateInterval = 0.01f;
        int iBulletVerticalOffset = 12;
        int[] iBulletFacingOffsets = new int[2] { 70, 0 };
        static int iMaxBullets = 40;
        Bullet[] bullets = new Bullet[iMaxBullets];
        float fBulletDelayTimer = 0.0f;
        float fFireDelay = 0.15f;

        int iMaxEnemies = 9;
        int iActiveEnemies = 9;
        static int iTotalMaxEnemies = 30;
        Enemy[] Enemies = new Enemy[iTotalMaxEnemies];
        Texture2D t2dEnemyShip;

        Random rndGen = new Random();
        Texture2D t2dExplosionSheet;
        Explosion[] Explosions = new Explosion[iTotalMaxEnemies + 1];

        int iGameStarted = 0;
        Texture2D t2dTitleScreen;
        Texture2D t2dShopScreen;

        int iProcessEvents = 1;
        int iLivesLeft = 3;
        int iGameWave = 0;
        int iPlayerScore = 0;
        float fPlayerRespawnTimer = 4f;
        float fPlayerRespawnCount = 0f;

        Texture2D t2dGameScreen;
        SpriteFont spriteFont;
        static int SHIFT = 200;
        Vector2[] vInventoyLoc = new Vector2[7] { new Vector2(5 + SHIFT, 677), new Vector2(55 + SHIFT, 677), new Vector2(105 + SHIFT, 677), new Vector2(155 + SHIFT, 677), new Vector2(205 + SHIFT, 677), new Vector2(255 + SHIFT, 677), new Vector2(275 + SHIFT, 677) };
        //Vector2 vLivesTextLoc = new Vector2(100, 677);
        //Vector2 vWaveTextLoc = new Vector2(1065, 663);
        //Vector2 vScoreTextLoc = new Vector2(1065, 695);
        Vector2 vStartTextLoc = new Vector2(30, 350);
        Vector2 vGameOverTextLoc = new Vector2(330, 330);

        //Vector2 vSuperBombTextLoc = new Vector2(250, 677);

        static int iMaxPowerups = 3;
        static int iMaxInventory = 6;
        PowerUp[] powerups = new PowerUp[iMaxPowerups];
        PowerUp[] inventory = new PowerUp[iMaxInventory];
        Texture2D[] textures;


        float fSuperBombTimer = 2f;
        float fPowerUpSpawnCounter = 0.0f;
        float fPowerUpSpawnDelay = 1.0f;

        static int iMaxExplosionSounds = 2;
        private static SoundEffect[] PlayerShots = new SoundEffect[2];
        private static SoundEffect[] ExplosionSounds = new SoundEffect[iMaxExplosionSounds];
        private static SoundEffect PowerUpPickupSound;
        
        //Game timer
        float timeLeft = 10.0f;

        //Shop "Powerup"
        PowerUp shopPowerUp;
        bool restetElapsed = false;
        int iTextureSize = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 720;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = new Background(
                Content,
                @"Textures\PrimaryBackground",
                @"Textures\ParallaxStars",
                @"Textures\shop2");

            player = new Player(Content.Load<Texture2D>(@"Textures\PlayerShip"));

            bullets[0] = new Bullet(Content.Load<Texture2D>(@"Textures\PlayerBullet"));

            for (int x = 1; x < iMaxBullets; x++)
                bullets[x] = new Bullet();

            t2dEnemyShip = Content.Load<Texture2D>(@"Textures\enemy");

            for (int i = 0; i < iTotalMaxEnemies; i++)
            {
                Enemies[i] = new Enemy(t2dEnemyShip, 0, 0, 32, 32, 1);
            }

            t2dExplosionSheet = Content.Load<Texture2D>(@"Textures\Explosions");
            for (int i = 0; i < iTotalMaxEnemies + 1; i++)
            {
                Explosions[i] = new Explosion(t2dExplosionSheet,
                                0, rndGen.Next(8) * 64, 64, 64, 16);
            }

            t2dTitleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen");
            t2dShopScreen = Content.Load<Texture2D>(@"Textures\ParallaxStars");

            t2dGameScreen = Content.Load<Texture2D>(@"Textures\GameScreen");

            spriteFont = Content.Load<SpriteFont>(@"Fonts\Pericles");

            //populate powerup texture array
            //Regex r = new Regex("");
            //string temp = Regex.Replace(Directory.GetCurrentDirectory(),);
            //System.String pathStr =Directory.GetCurrentDirectory() + "\\Textures\\sprites"; 
            string[] filePaths = Directory.GetFiles(@"Content\Textures\sprites", "*.xnb");
            Array.Sort<string>(filePaths);
            textures = new Texture2D[filePaths.Length];
            for (int i = 0; i < filePaths.Length; i++)
            {
                string s = filePaths[i];
                int length = s.IndexOf(".");
                System.Diagnostics.Debug.WriteLine("original string: " + s + s.Substring(8, length-8) + "lenght " + length.ToString());
                //textures[i] = Content.Load<Texture2D>(Path.GetFullPath(filePaths[i]));
                textures[i] = Content.Load<Texture2D>(s.Substring(8, length - 8));
            }



            for (int i = 0; i < iMaxPowerups; i++)
            {
                powerups[i] = new PowerUp(textures, 0);
            }
            iTextureSize = textures.Length - 1;
            shopPowerUp = new PowerUp(textures,textures.Length-1);

            for (int i = 0; i < iMaxInventory; i++)
            {
                inventory[i] = new PowerUp(textures,0);
            }

            PlayerShots[0] = Content.Load<SoundEffect>(@"Sounds\Scifi002");
            PlayerShots[1] = Content.Load<SoundEffect>(@"Sounds\Scifi050");
            ExplosionSounds[0] = Content.Load<SoundEffect>(@"Sounds\battle003");
            ExplosionSounds[1] = Content.Load<SoundEffect>(@"Sounds\battle004");
            PowerUpPickupSound = Content.Load<SoundEffect>(@"Sounds\Scifi041");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected void GenerateEnemies()
        {
            if (iMaxEnemies < iTotalMaxEnemies)
                iMaxEnemies++;

            iActiveEnemies = 0;

            for (int x = 0; x < iMaxEnemies; x++)
            {
                Enemies[x].Generate(background.BackgroundOffset,
                                    player.Y);
                iActiveEnemies += 1;
            }
        }

        protected void PlayerKilled()
        {
            // Reset the iGameWave and iMaxEnemies, since they will 
            // both be bumped automatically when the new wave is 
            // generated.
            iGameWave--;
            iMaxEnemies--;
            // Stop the player's ship
            player.ScrollRate = 0;
            player.Reset();
        }

        protected void StartNewWave()
        {
            iProcessEvents = 1;
            iGameStarted = 1;
            iGameWave = 0;
            GenerateEnemies();

            for (int x = 0; x < iMaxBullets; x++)
                RemoveBullet(x);
            foreach(PowerUp p in inventory){
                p.IsActive = false;
                p.IsSelected = false;
            }
            foreach (Explosion e in Explosions)
            {
                e.IsActive = false;
            }
            //TODO: reset all the little timers
        }

        protected void StartNewGame()
        {
            player.Reset();
            player.SuperBombs = 2;
            iLivesLeft = 3;
            player.ScrollRate = 0;
            iGameStarted = 1;
            iGameWave = 0;
            iPlayerScore = 0;

            shopPowerUp.IsActive = false;
            timeLeft = 40.0f;
            player.X = 360;
            player.Y = 600;

            StartNewWave();
        }

        protected void UpdateBullets(GameTime gameTime)
        {
            // Updates the location of all of thell active player bullets. 
            for (int x = 0; x < iMaxBullets; x++)
            {
                if (bullets[x].IsActive)
                    bullets[x].Update(gameTime);
            }
        }

        protected void FireBullet(int iVerticalOffset)
        {
            // Find and fire a free bullet
            for (int x = 0; x < iMaxBullets; x++)
            {
                if (!bullets[x].IsActive)
                {
                    bullets[x].Fire(player.X + iBulletFacingOffsets[player.Facing],
                                             player.Y + iBulletVerticalOffset + iVerticalOffset,
                                             player.Facing);
                    break;
                }
            }

            if (iVerticalOffset == 0)
                PlayerShots[player.WeaponLevel].Play(1.0f, 0f, 0f);

        }

        protected void CheckOtherKeys(KeyboardState ksKeys, GamePadState gsPad)
        {

            // Space Bar or Game Pad A button fire the 
            // player's weapon.  The weapon has it's
            // own regulating delay (fBulletDelayTimer) 
            // to pace the firing of the player's weapon.
            //if ((ksKeys.IsKeyDown(Keys.Space)) ||
            //    (gsPad.Buttons.A == ButtonState.Pressed))
            //{
            //    if (fBulletDelayTimer >= player.FireDelay)
            //    {
            //        FireBullet(0);
            //        fBulletDelayTimer = 0.0f;
            //        if (player.WeaponLevel == 1)
            //        {
            //            FireBullet(-4);
            //        }
            //    }
            //}

            // The Backspace (keyboard) or B (gamepad)
            // button is used to trigger a "Super Bomb"
            if ((ksKeys.IsKeyDown(Keys.Back)) ||
                (gsPad.IsButtonDown(Buttons.B)))
            {
                if ((fSuperBombTimer > player.SuperBombDelay) &&
                    (player.SuperBombs > 0))
                {
                    player.SuperBombs--;
                    fSuperBombTimer = 0f;
                    ExecuteSuperBomb();
                }
            }
        }

        protected void CheckVertMovementKeys(KeyboardState ksKeys,
                                   GamePadState gsPad)
        {
            bool bResetTimer = false;

            player.Thrusting = false;
            //NEED TO MOVE THIS TO INIT
            player.ScrollRate = -iMaxVertSpeed;

            if (bResetTimer)
                player.SpeedChangeCount = 0.0f;
        }

        protected void CheckNumKey(KeyboardState ksKeys,
                                 GamePadState gsPad)
        {
            if ((ksKeys.IsKeyDown(Keys.D1)))
            {
                inventory[0].IsActive = false;
            }
            else if ((ksKeys.IsKeyDown(Keys.D2)))
            {
                inventory[1].IsActive = false;
            }
            else if ((ksKeys.IsKeyDown(Keys.D3)))
            {
                inventory[2].IsActive = false;
            }
            else if ((ksKeys.IsKeyDown(Keys.D4)))
            {
                inventory[3].IsActive = false;
            }
            else if ((ksKeys.IsKeyDown(Keys.D5)))
            {
                inventory[4].IsActive = false;
            }
            else if ((ksKeys.IsKeyDown(Keys.D6)))
            {
                inventory[5].IsActive = false;
            }
        }
        protected void CheckShopKey(KeyboardState ksKeys,
                         GamePadState gsPad)
        {
            bool hitKey = false;
            if ((ksKeys.IsKeyDown(Keys.D1)))
            {
                inventory[0].IsSelected = !inventory[0].IsSelected;
                hitKey = true;
            }
            else if ((ksKeys.IsKeyDown(Keys.D2)))
            {
                inventory[1].IsSelected = !inventory[1].IsSelected;
                hitKey = true;
            }
            else if ((ksKeys.IsKeyDown(Keys.D3)))
            {
                inventory[2].IsSelected = !inventory[2].IsSelected;
                hitKey = true;
            }
            else if ((ksKeys.IsKeyDown(Keys.D4)))
            {
                inventory[3].IsSelected = !inventory[3].IsSelected;
                hitKey = true;
            }
            else if ((ksKeys.IsKeyDown(Keys.D5)))
            {
                inventory[4].IsSelected = !inventory[4].IsSelected;
                hitKey = true;
            }
            else if ((ksKeys.IsKeyDown(Keys.D6)))
            {
                inventory[5].IsSelected = !inventory[5].IsSelected;
                hitKey = true;
            }
            else if(ksKeys.IsKeyDown(Keys.Space))
            {
                int count = 0;
                //store the index of selected items in the following array
                int[] toCombine = new int[iMaxInventory];
                for (int i = 0; i < iMaxInventory; i++ )
                {
                    if (inventory[i].IsSelected && inventory[i].IsActive)
                    {
                        toCombine[count] = i;
                        count++;
                    }
                }
                //for right now, can only combine two items
                if (count == 2)
                {
                    int newType = combineItem(inventory[toCombine[0]], inventory[toCombine[1]]);
                    newType = Math.Min(newType, 29);//keep us in bounds
                    System.Diagnostics.Debug.WriteLine("NEW ITEM: " + newType);
                    inventory[toCombine[0]].IsSelected = false;
                    inventory[toCombine[1]].IsSelected = false;
                    inventory[toCombine[0]].PowerUpType = newType;
                    inventory[toCombine[1]].IsActive = false;
                }
            }
            else if (ksKeys.IsKeyDown(Keys.Enter))
            {
                iProcessEvents = 1;
                foreach (PowerUp i in inventory)
                {
                    if (i.IsActive && i.PowerUpType > 0)
                    {
                        iPlayerScore += i.PowerUpType;
                    }
                }
                //figure out how much gas to give
                float shopCost = Math.Min(iPlayerScore, 20.0f);
                timeLeft += shopCost;
                iPlayerScore -= (int) shopCost;

                //make the game a little harder
                iMaxVertSpeed += 1;
                StartNewWave();
            }
            if (hitKey)
            {
                //fix infinit toggle issue
                System.Threading.Thread.Sleep(100);
            }
        }

        //dummy method for now
        public int combineItem(PowerUp p1, PowerUp p2)
        {
            int newValue = 0;
            if (rndGen.Next(0, 2) == 1)
            {
                newValue = p1.PowerUpType + p2.PowerUpType;
                newValue = (int) (newValue * 1.5);
            }
            return newValue;
        }
        protected void CheckHorMovementKeys(KeyboardState ksKeys,
                                 GamePadState gsPad)
        {

            bool bResetTimer = false;

            if ((ksKeys.IsKeyDown(Keys.Left)))
            {
                if (player.X > iPlayAreaTop)
                {
                    player.X -= player.HorMovementRate;
                    player.Thrusting = true;
                    player.Facing = 1;
                    bResetTimer = true;
                }
            }

            if ((ksKeys.IsKeyDown(Keys.Right))) 
            {
                if (player.X < iPlayAreaBottom)
                {
                    player.X += player.HorMovementRate;
                    player.Thrusting = true;
                    player.Facing = 0;
                    bResetTimer = true;
                }
            }
            if ((ksKeys.IsKeyDown(Keys.Up)))
            {
                if (player.Y > iPlayAreaTop)
                {
                    player.Y -= player.iShipVerMoveRate;
                    player.Thrusting = true;
                    bResetTimer = true;
                }
            }
            if ((ksKeys.IsKeyDown(Keys.Down)))
            {
                if (player.Y < iPlayAreaBottom)
                {
                    player.Y += player.iShipVerMoveRate + iMaxVertSpeed;
                    player.Thrusting = true;
                    bResetTimer = true;
                }
            }

            if (bResetTimer)
                player.VerticalChangeCount = 0f;
        }

        public void UpdateBoard()
        {
            background.BackgroundOffset += player.ScrollRate;
            background.ParallaxOffset += player.ScrollRate * 2;
        }

        protected bool Intersects(Rectangle rectA, Rectangle rectB)
        {
            // Returns True if rectA and rectB contain any overlapping points
            return (rectA.Right > rectB.Left && rectA.Left < rectB.Right &&
                    rectA.Bottom > rectB.Top && rectA.Top < rectB.Bottom);
        }

        protected void DestroyEnemy(int iEnemy)
        {
            Enemies[iEnemy].Deactivate();
            Explosions[iEnemy].Activate(
                Enemies[iEnemy].X - 16,
                Enemies[iEnemy].Y - 16,
                Enemies[iEnemy].Motion,
                Enemies[iEnemy].Speed / 2,
                Enemies[iEnemy].Offset);
            
            iActiveEnemies--;
            iPlayerScore += 10;
            ExplosionSounds[rndGen.Next(0, iMaxExplosionSounds)].Play(1.0f, 0f, 0f);
        }

        protected void RemoveBullet(int iBullet)
        {
            bullets[iBullet].IsActive = false;
        }

        protected void CheckBulletHits()
        {
            // Check to see of any of the players bullets have 
            // impacted any of the enemies.
            for (int i = 0; i < iMaxBullets; i++)
            {
                if (bullets[i].IsActive)
                    for (int x = 0; x < iTotalMaxEnemies; x++)
                        if (Enemies[x].IsActive)
                            if (Intersects(bullets[i].BoundingBox,
                                           Enemies[x].CollisionBox))
                            {
                                DestroyEnemy(x);
                                RemoveBullet(i);
                            }
           }

            // If we have run out of active enemies, generate new ones
            if (iActiveEnemies < 1)
                StartNewWave();
        }

        protected void CheckPlayerHits()
        {
            for (int x = 0; x < iTotalMaxEnemies; x++)
            {
                if (Enemies[x].IsActive)
                {
                    // If the enemy and ship sprites  collide...
                    if (Intersects(player.BoundingBox, Enemies[x].CollisionBox))
                    {
                        // Stop event processing
                        iProcessEvents = 0;
                        timeLeft -= 5;
                        // Set up the ship's explosion
                        Explosions[iTotalMaxEnemies].Activate(
                            player.X - 16,
                            player.Y - 16,
                            Vector2.Zero,
                            0f,
                            background.BackgroundOffset);

                        fPlayerRespawnCount = 0.0f;
                        ExplosionSounds[0].Play(1.0f, 0f, 0f);

                        bool gameOver = true;

                        //check if player has any items
                        foreach (PowerUp p in inventory)
                        {
                            if (p.IsActive)
                            {
                                gameOver = false;
                            }
                        }

                        //player lost
                        if (gameOver)
                        {
                            iLivesLeft = 1;
                            iGameStarted = 0;
                            iProcessEvents = 1;
                            System.Threading.Thread.Sleep(4000);
                        }

                        //clear out inventory
                        foreach (PowerUp p in inventory)
                        {
                            p.IsActive = false;
                        }
                        return;
                    }
                }
            }

            for (int x = 0; x < iMaxPowerups; x++)
            {
                if ((powerups[x].IsActive) &&
                     (Intersects(player.BoundingBox, powerups[x].BoundingBox)))
                {
                    //check to see if powerup is in already in inv
                    bool alreadyHave = false;
                    for (int i = 0; i < iMaxInventory; i++)
                    {
                        if (inventory[i].IsActive)
                        {
                            if(powerups[x].PowerUpType == inventory[i].PowerUpType)
                            {
                                alreadyHave = true; 
                                break; 
                            }
                        }

                    }
                    //add if not in inventory
                    if (!alreadyHave)
                    {
                        for (int i = 0; i < iMaxInventory; i++)
                        {
                            if (!inventory[i].IsActive)
                            {
                                inventory[i].PowerUpType = powerups[x].PowerUpType;
                                inventory[i].IsActive = true;
                                break;
                            }

                        }
                    }
                    powerups[x].IsActive = false;
                    PowerUpPickupSound.Play(1.0f, 0f, 0f);
                }

            }
            if ((shopPowerUp.IsActive) &&
                (Intersects(player.BoundingBox, shopPowerUp.BoundingBox)))
            {
                shopPowerUp.IsActive = false;
                iProcessEvents = 2;
            }
        }

        protected void ExecuteSuperBomb()
        {
            for (int x = 0; x < iMaxEnemies; x++)
            {
                if (Intersects(Enemies[x].BoundingBox,
                      new Rectangle(0, 30, 1280, 630)))
                    DestroyEnemy(x);
            }
        }

        protected void GeneratePowerup()
        {
            for (int x = 0; x < iMaxPowerups; x++)
            {
                if (!powerups[x].IsActive)
                {
                    powerups[x].X = rndGen.Next(0, 720);
                    powerups[x].Y = background.BackgroundOffset;
                    powerups[x].PowerUpType = rndGen.Next(1, 30);
                    powerups[x].Offset = background.BackgroundOffset;
                    powerups[x].Activate();
                    break;
                }
            }
            //10%chance of generationg shop powerup
            if (rndGen.Next(0, 2) == 1)
            {
                if (!shopPowerUp.IsActive)
                {
                    shopPowerUp.X = 600;//always spaw on the right hand side
                    shopPowerUp.Y = background.BackgroundOffset;
                    shopPowerUp.PowerUpType = iTextureSize;
                    shopPowerUp.Offset = background.BackgroundOffset;
                    shopPowerUp.Activate();
                }
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Store values for the Keyboard and GamePad so we aren't
            // Querying them multiple times per Update
            KeyboardState keystate = Keyboard.GetState();
            GamePadState gamepadstate = GamePad.GetState(PlayerIndex.One);

            // If the Escape Key is pressed, or the user presses
            // the "Back" button on the game pad, exit the game.
            if ((keystate.IsKeyDown(Keys.Escape) ||
                 gamepadstate.Buttons.Back == ButtonState.Pressed))
                this.Exit();

            // Get elapsed game time since last call to Update
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (iGameStarted == 1)
            {
                #region GamePlay Mode (iGameStarted==1)

                if (iProcessEvents == 1)
                {
                    #region Processing Events (iProcessEvents==1)
                    background.IsShop = false;
                    //Accumulate time since the last bullet was fired
                    fBulletDelayTimer += elapsed;

                    //Accumulate time since the last super bomb was fired
                    fSuperBombTimer += elapsed;

                    //Accumulate time since the last powerup was generated
                    fPowerUpSpawnCounter += elapsed;
                    if (fPowerUpSpawnCounter > fPowerUpSpawnDelay)
                    {
                        GeneratePowerup();
                        fPowerUpSpawnCounter = 0.0f;
                    }
                    timeLeft = timeLeft - elapsed; 
                    //check if it is time to shop
                    if (timeLeft <= 0)
                    {
                        iProcessEvents = 0;
                        iGameStarted = 0;
                    }


                    // Accumulate time since the player's speed changed
                    player.SpeedChangeCount += elapsed;

                    // If enough time has passed that the player can change
                    // speed again, call CheckVertMovementKeys
                    if (player.SpeedChangeCount > player.SpeedChangeDelay)
                    {
                        CheckVertMovementKeys(keystate, gamepadstate);
                    }

                    // Accumulate time since the player moved vertically
                    player.VerticalChangeCount += elapsed;

                    // If enough time has passed, call CheckHorMovementKeys
                    if (player.VerticalChangeCount > player.VerticalChangeDelay)
                    {
                        CheckHorMovementKeys(keystate, gamepadstate);
                        CheckNumKey(keystate, gamepadstate);
                    }

                    // Check any other key presses
                    CheckOtherKeys(keystate, gamepadstate);

                    // Update all enemies and explosions
                    for (int i = 0; i < iTotalMaxEnemies; i++)
                    {
                        if (Enemies[i].IsActive)
                            Enemies[i].Update(gameTime,
                              background.BackgroundOffset);

                        if (Explosions[i].IsActive)
                            Explosions[i].Update(gameTime,
                              background.BackgroundOffset);

                    }

                    // Update the player's star fighter
                    player.Update(gameTime);

                    // Move any active bullets
                    UpdateBullets(gameTime);

                    // Update Powerups
                    for (int x = 0; x < iMaxPowerups; x++)
                        powerups[x].Update(gameTime,
                          background.BackgroundOffset);
                    shopPowerUp.Update(gameTime,background.BackgroundOffset);
                    // See if any active bullets hit any active enemies
                    //CheckBulletHits();

                    // Check to see if the player has collided with any enemies
                    CheckPlayerHits();

                    // Accumulate time since the game board was last updated
                    // This reflects the actual movement rate of the screen
                    // as opposed to speed changes by the player
                    fBoardUpdateDelay += elapsed;

                    // If enough time has elapsed, update the game board.
                    if (fBoardUpdateDelay > fBoardUpdateInterval)
                    {
                        fBoardUpdateDelay = 0f;
                        UpdateBoard();
                    }
                    #endregion
                }
                else if (iProcessEvents == 2)
                {
                    #region Processing Events (iProcessEvents==2)
                    background.IsShop = true;
                    //Accumulate time since the last powerup was generated
                    //fPowerUpSpawnCounter += elapsed;
                    //if (fPowerUpSpawnCounter > fPowerUpSpawnDelay)
                    //{
                    //    GeneratePowerup();
                    //    fPowerUpSpawnCounter = 0.0f;
                    //}


                    // Accumulate time since the player's speed changed
                    //player.SpeedChangeCount += elapsed;

                    // If enough time has passed that the player can change
                    // speed again, call CheckVertMovementKeys
                    //if (player.SpeedChangeCount > player.SpeedChangeDelay)
                    //{
                    //    CheckVertMovementKeys(keystate, gamepadstate);
                    //}

                    // Accumulate time since the player moved vertically
                    //player.VerticalChangeCount += elapsed;

                    // If enough time has passed, call CheckHorMovementKeys
                    //if (player.VerticalChangeCount > player.VerticalChangeDelay)
                    //{
                    //    CheckHorMovementKeys(keystate, gamepadstate);
                    //    //CheckNumKey(keystate, gamepadstate);
                    //}

                    // Check any other key presses
                    //CheckOtherKeys(keystate, gamepadstate);

                    // Update all enemies and explosions
                    //for (int i = 0; i < iTotalMaxEnemies; i++)
                    //{
                    //    if (Enemies[i].IsActive)
                    //        Enemies[i].Update(gameTime,
                    //          background.BackgroundOffset);

                    //    if (Explosions[i].IsActive)
                    //        Explosions[i].Update(gameTime,
                    //          background.BackgroundOffset);

                    //}

                    // Update the player's star fighter
                    player.Update(gameTime);

                    // Move any active bullets
                   // UpdateBullets(gameTime);

                    // Update Powerups
                    //for (int x = 0; x < iMaxPowerups; x++)
                    //    powerups[x].Update(gameTime,
                    //      background.BackgroundOffset);

                    // See if any active bullets hit any active enemies
                    //CheckBulletHits();

                    // Check to see if the player has collided with any enemies
                    //CheckPlayerHits();

                    // Accumulate time since the game board was last updated
                    // This reflects the actual movement rate of the screen
                    // as opposed to speed changes by the player
                    fBoardUpdateDelay += elapsed;

                    // If enough time has elapsed, update the game board.
                    //if (fBoardUpdateDelay > fBoardUpdateInterval)
                    //{
                    //    fBoardUpdateDelay = 0f;
                    //    UpdateBoard();
                    //}
                    if (fBoardUpdateDelay > fBoardUpdateInterval*10)
                    {
                        CheckShopKey(keystate, gamepadstate);
                    }
                    #endregion
                }
                else
                {
                    #region Not Processing Events (iProcessEvents==0)

                    if (Explosions[iTotalMaxEnemies].IsActive)
                        Explosions[iTotalMaxEnemies].Update(gameTime,
                            background.BackgroundOffset);

                    fPlayerRespawnCount += elapsed;

                    if (fPlayerRespawnCount > fPlayerRespawnTimer)
                    {
                        iLivesLeft -= 1;
                        if (iLivesLeft > 0)
                        {
                            PlayerKilled();
                            StartNewWave();
                        }
                        //else
                        //{
                        //    iGameStarted = 0;
                        //    iProcessEvents = 1;
                        //}
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region Title Screen Mode (iGameStarted==0)
                if ((keystate.IsKeyDown(Keys.Space)) ||
                   (gamepadstate.Buttons.Start == ButtonState.Pressed))
                {
                    StartNewGame();
                }
                #endregion
            }
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear the Graphics Device
            graphics.GraphicsDevice.Clear(Color.Black);

            // Start a SpriteBatch.Begin which will be used
            // by all of our drawing code.
            spriteBatch.Begin();

            if (iGameStarted == 1 && iProcessEvents != 2)
            {
                #region Game Play Mode (iGameStarted==1)

                // Draw the Background object
                background.Draw(spriteBatch);

                // Draw the Player's Star Fighter
                if (iProcessEvents == 1)
                {
                    player.Draw(spriteBatch);
                }

                // Draw any active bullets on the screen
                for (int i = 0; i < iMaxBullets; i++)
                {
                    if (bullets[i].IsActive)
                    {
                        bullets[i].Draw(spriteBatch);
                    }
                }

                // Draw any active enemies and explosions
                for (int i = 0; i < iMaxEnemies; i++)
                {
                    if (Enemies[i].IsActive)
                    {
                        Enemies[i].Draw(spriteBatch,
                          background.BackgroundOffset);
                    }

                    if (Explosions[i].IsActive)
                    {
                        Explosions[i].Draw(spriteBatch, false);
                    }
                }

                for (int i = 0; i < iMaxPowerups; i++)
                {
                    powerups[i].Draw(spriteBatch);
                }
                shopPowerUp.Draw(spriteBatch);
                
                // Draw the player's explosion if it is happening
                if (Explosions[iTotalMaxEnemies].IsActive)
                    Explosions[iTotalMaxEnemies].Draw(spriteBatch, true);

                // Draw the Game Screen overlay
                spriteBatch.Draw(t2dGameScreen, new Rectangle(0, 0, 1280, 720), Color.White);

                for (int i = 0; i < iMaxInventory - 1; i++ )
                {
                    if (inventory[i].IsActive && inventory[i].IsSelected)
                    {
                        spriteBatch.DrawString(spriteFont, inventory[i].PowerUpType.ToString(),
                            vInventoyLoc[i], Color.Red);
                    }
                    else if (inventory[i].IsActive)
                    {
                        spriteBatch.Draw(textures[inventory[i].PowerUpType], vInventoyLoc[i], Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(textures[0], vInventoyLoc[i], Color.White);
                    }
                }


                int temp = (int)timeLeft;
                //draw score
                String score = "Time: " + temp.ToString() + "  SCORE: " + iPlayerScore.ToString();
                spriteBatch.DrawString(spriteFont, score,
                            vInventoyLoc[iMaxInventory], Color.Gold);

                // If the player is dead and this is their last life, display
                // "GAME OVER" while waiting for the fPlayerRespawnCount to end.
                if (iProcessEvents == 0 && iLivesLeft == 1)
                    spriteBatch.DrawString(spriteFont, "G A M E   O V E R",
                        vGameOverTextLoc, Color.Gold);
                #endregion
            }
            else if (iProcessEvents == 2)
            {
                #region Shop Screen Mode (iProcessEvents==2)
                background.Draw(spriteBatch);
                if (gameTime.TotalGameTime.Milliseconds % 1000 < 500)
                {
                    spriteBatch.DrawString(spriteFont, "Please Shop",
                        vStartTextLoc, Color.Black);
                }
                for (int i = 0; i < iMaxInventory - 1; i++)
                {
                    if (inventory[i].IsActive && inventory[i].IsSelected)
                    {
                        spriteBatch.DrawString(spriteFont, inventory[i].PowerUpType.ToString(),
                            vInventoyLoc[i], Color.Red);
                    }
                    else if (inventory[i].IsActive)
                    {
                        spriteBatch.Draw(textures[inventory[i].PowerUpType], vInventoyLoc[i], Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(textures[0], vInventoyLoc[i], Color.White);
                    }
                }
                int temp = (int)timeLeft;
                //draw score
                String score = "Time: " + temp.ToString () + "  SCORE: " + iPlayerScore.ToString();
                spriteBatch.DrawString(spriteFont, score,
                            vInventoyLoc[iMaxInventory], Color.Gold);
                #endregion
            }
            else
            {
                #region Title Screen Mode (iGameStarted==0)
                spriteBatch.Draw(t2dTitleScreen, new Rectangle(0, 0, 1280, 720), Color.White);
                if (gameTime.TotalGameTime.Milliseconds % 1000 < 500)
                {
                    spriteBatch.DrawString(spriteFont, "Press START or SPACE to Begin",
                        vStartTextLoc, Color.Gold);
                }
                #endregion
            }
            // Close the SpriteBatch
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
