//Author: Dylan Nagel
//File Name: Game1.cs
//Project Name: NagelD_PASS2
//Creation Date: Mar. 23 2023
//Modified Date: Apr. 16, 2023
//Description: Drives the minecraft shooter game, best described as the offspring of galaga and minecraft

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace NagelD_PASS2
{
    public class Game1 : Game
    {
        //store graphics and spritbatch
        private GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        //stores size of arrays
        public const int NUM_MONST = 5;
        private const int NUM_ARR = 2;
        private const int NUM_BLOCK = 4;
        private const int NUM_TITLES = 4;
        private const int NUM_BUTTONS = 3;
        public const int NUM_BUFFS = 4;
        public const int NUM_LEVELS = 5;

        //stores buff type locations
        public const int SPEED_BUFF = 0;
        public const int SHOOT_BUFF = 1;
        public const int STRENGTH_BUFF = 2;
        public const int SCORE_BUFF = 3;

        //stores location of gamestates
        private const int MENU = 0;
        private const int INSTRUCTIONS = 1;
        private const int GAMEPLAY = 2;
        private const int ENDLEVEL = 3;
        private const int SHOP = 4;
        private const int ENDGAME = 5;
        private const int STATS = 6;
        private const int FILE_ERROR = 7;
        private const int EXIT = 8;

        //Stores location of mob type
        public const int VILLAGER = 0;
        public const int CREEPER = 1;
        public const int SKELETON = 2;
        public const int PILLAGER = 3;
        public const int ENDERMAN = 4;

        //stores mob life status locations
        public const int DEAD = 0;
        public const int ALIVE = 1;

        //stores buff type locations
        public const int TYPE_ARROW = 0;
        public const int TYPE_EXPLOAD = 1;
        public const int TYPE_FEAR = 2;

        //stores time the death blob remains on screen
        public static int DEATH_TIME = 1000;

        //stores size of monster weapons
        public const int FEAR_IMG_HEIGHT = 500;
        public const int CREEP_EXP_LENGTH = 100;
        public const int ARROW_WIDTH = 10;
        public const int ARROW_HEIGHT = 25;

        //variables to store arrow speed
        public const int ARROW_SPEED = 5;

        //variables to store enderman locations
        public static Vector2 ENDER_LOC1 = new Vector2(50, 300);
        public static Vector2 ENDER_LOC2 = new Vector2(350, 200);
        public static Vector2 ENDER_LOC3 = new Vector2(100, 50);

        //stores enderman timer amounts
        public const int END_TEL_TIMER = 3000;

        //variables to store shield width and height
        public const int PILL_SHIELD_WIDTH = 20;
        public const int PILL_SHIELD_HEIGHT = 30;

        //variable to store pillager movement info
        public const int PILL_SIN_AMP = 100;
        public const int PILL_SPEED = 1;

        //stores index of the skeleton phase
        public const int SKEL_PHASE_1 = 0;
        public const int SKEL_PHASE_2 = 1;
        public const int SKEL_PHASE_3 = 2;

        //stores skeleton information
        public const double SKEL_SHOOT_TIME = 1000 / 1.5;
        public const float SKEL_SPEED = 2.1f;
        public const float SKEL_ANGLE_RATE_CHANGE = 0.03f;
        public const float SKEL_RADIUS_RATE_CHANGE = 0.36f;

        //stores creeper speed
        public const float CREEPER_SPEED = 3.4f;

        //stores villager speed
        public const float VILL_SPEED = 1.9f;

        //Stores location of up or down direction
        public const int UP = 0;
        public const int DOWN = 1;

        //stores location of material type
        private const int GRASS_1 = 0;
        private const int GRASS_2 = 1;
        public const int COBBLESTONE = 2;
        public const int DIRT = 3;

        //stores location of title images in array
        public const int STATS_TITLE = 0;
        public const int SHOP_TITLE = 1;
        public const int END_LEVEL_TITLE = 2;
        public const int GAME_OVER_TITLE = 3;

        //stores title to top of screen spacing
        public const int TITLE_TOP_SPACE = 15;

        //stroe entity side lengths
        public static int ENTITY_SIZE = 50;

        //stores button information
        private const int TOP_BUT_Y_LOC = 260;
        private const int MENU_BUT_SPACE = 30;
        public static readonly Color[] BUT_PROMPT_COL = new Color[] { Color.White, Color.Goldenrod };
        public static readonly float[] BUT_VIS = new float[] { 0, 0.5f };

        //stores prices for buffs
        public static readonly int[] BUFF_PRICES = new int[] { 100, 300, 200, 500 };

        //stores monster hp heart info
        public static int HEART_SIZE = 15;
        public static int HEART_MONST_SPACE = 3;

        //stores instruction screen constant information
        private const int INSTRUC_BACK_HEIGHT = 290;
        private const int INSTRUC_TITLE_SPACE = 5;
        private const int INSTRUC_LINE_SPACE = 60;
        private const int INSTRUC_TITLE_LINE_SPACE = 20;
        private readonly string[] INTRUC_LINES = new string[] { "Move: Arrows left/right          Shoot: Space", "Goal: Kill mobs before they escape", "Tip: Watch for movement patterns" };
        private string INSTRUC_LEAVE_PROMT = "Press ENTER to begin";

        //stores error message spacing line spacing
        private const int ERROR_MES_SPACE = 50;

        //store menu button next gamestates
        private readonly int[] MENU_NEXT_GAMESTATE = new int[] { INSTRUCTIONS, STATS, EXIT };

        //stores top screen spawn min height
        public static int TOP_SCREEN_SPAWN = 20;

        //stores error message starting y locations
        private const int ERROR_MESSAGE_START_Y = 260;

        //stores full or empty variables for heart
        public const int FULL = 0;
        public const int EMPTY = 1;

        //stores location of bottom button in menu screen array
        public const int BOT_BUT = 2;

        //stores the current gamestate
        private int gameState = MENU;

        //stores random variable
        public static Random rng = new Random();

        //stores heart images for monster hp
        public static Texture2D[] HEART_IMGS = new Texture2D[2];
        public static Texture2D PILL_HEART_IMG;

        //stores monster images and monster related images
        public static Texture2D[,] MONSTER_IMGS = new Texture2D[2, NUM_MONST];
        public static Texture2D EXPLOAD_IMG;
        public static Texture2D ENDER_FEAR_IMG;
        public static Texture2D SHIELD_IMG;

        //stores arrow images
        public static Texture2D[] ARROW_IMGS = new Texture2D[NUM_ARR];

        //stores player image
        private Texture2D playerImg;

        //stores background images
        private Texture2D[] backBlockImgs = new Texture2D[NUM_BLOCK];
        private Texture2D woodBackImg;
        private Texture2D menuBackImg;

        //stores blank pixel image
        private Texture2D blankPixelImg;

        //stores hit marker image
        private Texture2D hitMarkerImg;

        //stores title information
        private Texture2D menuTitleImg;
        private Texture2D instrucTitleImg;
        private Rectangle menuTitleRec;
        private Rectangle instrucTitleRec;
        private Texture2D[] titleImgs = new Texture2D[NUM_TITLES];

        //stores button information
        private Texture2D buttonImg;
        private Rectangle[] buttonRecs = new Rectangle[NUM_BUTTONS];
        private bool[] isHover = new bool[NUM_BUTTONS];

        //stores buff and buff related images
        private Texture2D[] buffImgs = new Texture2D[NUM_BUFFS];
        private Texture2D itemFrameImg;

        //stores menu button information
        private string[] menuButtonPrompts = new string[] { "PLAY", "STATS", "EXIT" };
        private Vector2[] menuButtonPromptLocs = new Vector2[NUM_BUTTONS];

        //stores error screen information
        private Texture2D errorTitleImg;
        private Rectangle errorTitleRec;
        private string[] errorMessage = new string[3];
        private Vector2[] errorMessageloc = new Vector2[3];
        private Rectangle errorBackRec;

        //stores the stream reader and holding values for it
        private static StreamReader inFile;
        private static string[] overallReadStats = new string[10];

        //store game fonts
        private SpriteFont pixelFont;
        private SpriteFont statsFont;

        //stores error screen button prompt info
        private string errorButPrompt = "Click to go to menu";
        private Vector2 errorButPromptLoc;

        //stores screen width and height
        public static int screenWidth;
        public static int screenHeight;

        //stores full screen rectangle
        private Rectangle fullScreenRec;

        //stores user mouse state
        public static MouseState mouse;
        public static MouseState prevMouse;

        //stores keyboard state
        public static KeyboardState kb;
        public static KeyboardState prevKb;

        //store instructions screen information
        private Rectangle instrucBack;
        private Vector2[] instrucLineLocs = new Vector2[3];
        private Vector2 instrucLeaveLoc;

        //stores game music
        private Song menuMusic;
        private Song gameplayMusic;

        //stores sound effects
        private SoundEffect buttonClickSnd;
        private SoundEffect arrowHitSnd;
        private SoundEffect damageTakenSnd;
        private SoundEffect creepExploadSnd;
        public static SoundEffect ENDER_ANGRY_SND;

        //stores a GameObject object
        private GameObject gameObject;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //changes screen size to specified size
            this.graphics.PreferredBackBufferWidth = 550;
            this.graphics.PreferredBackBufferHeight = 650;

            //applies screen size changes
            this.graphics.ApplyChanges();

            //sets screen size variables
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            base.Initialize();
        }

        //pre: none
        //post: none
        //description: loads in the game content
        protected override void LoadContent()
        {
            //stores spritbatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load in fonts
            pixelFont = Content.Load<SpriteFont>("Fonts/PixelFont");
            statsFont = Content.Load<SpriteFont>("Fonts/StatsFont");

            //load in monster images
            MONSTER_IMGS[ALIVE, VILLAGER] = Content.Load<Texture2D>("Images/Sprites/Villager");
            MONSTER_IMGS[ALIVE, CREEPER] = Content.Load<Texture2D>("Images/Sprites/Creeper");
            MONSTER_IMGS[ALIVE, SKELETON] = Content.Load<Texture2D>("Images/Sprites/Skeleton");
            MONSTER_IMGS[ALIVE, PILLAGER] = Content.Load<Texture2D>("Images/Sprites/Pillager");
            MONSTER_IMGS[ALIVE, ENDERMAN] = Content.Load<Texture2D>("Images/Sprites/Enderman");

            //load in death images
            MONSTER_IMGS[DEAD, VILLAGER] = Content.Load<Texture2D>("Images/Sprites/RedSplat");
            MONSTER_IMGS[DEAD, CREEPER] = Content.Load<Texture2D>("Images/Sprites/GreenSplat");
            MONSTER_IMGS[DEAD, SKELETON] = Content.Load<Texture2D>("Images/Sprites/YellowSplat");
            MONSTER_IMGS[DEAD, PILLAGER] = Content.Load<Texture2D>("Images/Sprites/GraySplat");
            MONSTER_IMGS[DEAD, ENDERMAN] = Content.Load<Texture2D>("Images/Sprites/PurpleSplat");

            //load in enemy weapon images
            EXPLOAD_IMG = Content.Load<Texture2D>("Images/Sprites/Explode");
            ENDER_FEAR_IMG = Content.Load<Texture2D>("Images/Sprites/EnderScare");
            ARROW_IMGS[UP] = Content.Load<Texture2D>("Images/Sprites/ArrowUp");
            ARROW_IMGS[DOWN] = Content.Load<Texture2D>("Images/Sprites/ArrowDown");

            //load ing monster heart images
            HEART_IMGS[FULL] = Content.Load<Texture2D>("Images/Sprites/FullHeart");
            HEART_IMGS[EMPTY] = Content.Load<Texture2D>("Images/Sprites/EmptyHeart");
            PILL_HEART_IMG = Content.Load<Texture2D>("Images/Sprites/MetalHeart");

            //load in pillager shield image
            SHIELD_IMG = Content.Load<Texture2D>("Images/Sprites/Shield");

            //load in player images
            playerImg = Content.Load<Texture2D>("Images/Sprites/Steve");

            //load in material images
            backBlockImgs[GRASS_1] = Content.Load<Texture2D>("Images/Backgrounds/Grass1");
            backBlockImgs[GRASS_2] = Content.Load<Texture2D>("Images/Backgrounds/Grass2");
            backBlockImgs[COBBLESTONE] = Content.Load<Texture2D>("Images/Backgrounds/Cobblestone");
            backBlockImgs[DIRT] = Content.Load<Texture2D>("Images/Backgrounds/Dirt");

            //load in background images
            woodBackImg = Content.Load<Texture2D>("Images/Backgrounds/WoodBackground");
            menuBackImg = Content.Load<Texture2D>("Images/Backgrounds/MenuBackground");

            //load in blank pixel image
            blankPixelImg = Content.Load<Texture2D>("Images/Sprites/BlankPixel");

            //load in title images
            menuTitleImg = Content.Load<Texture2D>("Images/Sprites/Title");
            instrucTitleImg = Content.Load<Texture2D>("Images/Sprites/InstrucTitle");
            titleImgs[STATS_TITLE] = Content.Load<Texture2D>("Images/Sprites/StatsTitle");
            titleImgs[SHOP_TITLE] = Content.Load<Texture2D>("Images/Sprites/ShopTitle");
            titleImgs[END_LEVEL_TITLE] = Content.Load<Texture2D>("Images/Sprites/EndLevelTitle");
            titleImgs[GAME_OVER_TITLE] = Content.Load<Texture2D>("Images/Sprites/GameOverTitle");

            //load in error title image
            errorTitleImg = Content.Load<Texture2D>("Images/Sprites/FileErrorTitle");

            //load in button image
            buttonImg = Content.Load<Texture2D>("Images/Sprites/Button");

            //load in hit marker image
            hitMarkerImg = Content.Load<Texture2D>("Images/Sprites/HitMarker");

            //load in buff images
            buffImgs[SPEED_BUFF] = Content.Load<Texture2D>("Images/Sprites/EnchantedBoots");
            buffImgs[SHOOT_BUFF] = Content.Load<Texture2D>("Images/Sprites/EnchantedBow");
            buffImgs[STRENGTH_BUFF] = Content.Load<Texture2D>("Images/Sprites/PotionOfStrength");
            buffImgs[SCORE_BUFF] = Content.Load<Texture2D>("Images/Sprites/XPBottle");

            //load in item frame image
            itemFrameImg = Content.Load<Texture2D>("Images/Sprites/ItemFrame");

            //load in game music
            gameplayMusic = Content.Load<Song>("Audio/Music/GameplayMusic");
            menuMusic = Content.Load<Song>("Audio/Music/MenuMusic");

            //load in sound effects
            buttonClickSnd = Content.Load<SoundEffect>("Audio/Sounds/ButtonClick");
            arrowHitSnd = Content.Load<SoundEffect>("Audio/Sounds/ArrowHitSnd");
            damageTakenSnd = Content.Load<SoundEffect>("Audio/Sounds/DamageTakenSnd");
            creepExploadSnd = Content.Load<SoundEffect>("Audio/Sounds/CreeperExploadSnd");
            ENDER_ANGRY_SND = Content.Load<SoundEffect>("Audio/Sounds/AngryEnderman");

            //create menu title rectangle
            menuTitleRec = new Rectangle((int)(0.5 * (screenWidth - menuTitleImg.Width * 0.8)), TITLE_TOP_SPACE, (int)(menuTitleImg.Width * 0.8), (int)(menuTitleImg.Height * 0.8));

            //creates each button rectangle
            for (int i = 0; i < buttonRecs.Length; i++)
            {
                //creates button rectangle at index i
                buttonRecs[i] = new Rectangle((int)(0.5 * (screenWidth - buttonImg.Width * 0.45)), TOP_BUT_Y_LOC + (int)((buttonImg.Height * 0.35) + MENU_BUT_SPACE) * i, (int)(buttonImg.Width * 0.45), (int)(buttonImg.Height * 0.35));
                
                //sets location for menu screen button prompts
                menuButtonPromptLocs[i] = new Vector2(GetCentreTextX(pixelFont, menuButtonPrompts[i]), buttonRecs[i].Center.Y - 0.5f * pixelFont.MeasureString(menuButtonPrompts[i]).Y);
            }

            //create full screen rectangle
            fullScreenRec = new Rectangle(0, 0, screenWidth, screenHeight);

            //sets error screen rectangles
            errorTitleRec = new Rectangle((int)(0.5 * (screenWidth - errorTitleImg.Width * 0.8)), instrucBack.Y + INSTRUC_TITLE_SPACE, (int)(errorTitleImg.Width * 0.8), (int)(errorTitleImg.Height * 0.8));
            errorBackRec = new Rectangle(0, (int)(0.5 * (screenHeight - 200)), screenWidth, 200);

            //set instruction screen location and rectangle values
            instrucBack = new Rectangle(0, (int)(screenHeight * 0.5 - INSTRUC_BACK_HEIGHT * 0.5), screenWidth, INSTRUC_BACK_HEIGHT);
            instrucTitleRec = new Rectangle((int)(0.5 * (screenWidth - instrucTitleImg.Width * 0.8)), instrucBack.Y + INSTRUC_TITLE_SPACE, (int)(instrucTitleImg.Width * 0.8), (int)(instrucTitleImg.Height * 0.8));
            instrucLeaveLoc = new Vector2(GetCentreTextX(pixelFont, INSTRUC_LEAVE_PROMT), instrucBack.Bottom - pixelFont.MeasureString(INSTRUC_LEAVE_PROMT).Y - INSTRUC_TITLE_SPACE);

            //set location of each instruction line
            for (int i = 0; i < instrucLineLocs.Length; i++)
            {
                //set location of instruction line at index i
                instrucLineLocs[i] = new Vector2(GetCentreTextX(pixelFont, INTRUC_LINES[i]), INSTRUC_TITLE_LINE_SPACE + instrucTitleRec.Bottom + INSTRUC_LINE_SPACE * i);
            }

            //stores error screen button propmt location
            errorButPromptLoc = new Vector2(GetCentreTextX(pixelFont, errorButPrompt), menuButtonPromptLocs[BOT_BUT].Y);

            //stores the game object
            gameObject = new GameObject(playerImg, backBlockImgs, woodBackImg, blankPixelImg, buttonImg, titleImgs, buffImgs, itemFrameImg, pixelFont, statsFont, hitMarkerImg, buttonClickSnd, arrowHitSnd, damageTakenSnd, creepExploadSnd);

            try
            {
                //read and store overall score
                inFile = File.OpenText("OverallScores.txt");
                overallReadStats = inFile.ReadToEnd().Split('\n');

                //store overall stats in level class
                gameObject.StoreOverallStatVals(overallReadStats);
            }
            catch (FileNotFoundException)
            {
                //set the three lines of error messages
                errorMessage[0] = "Your save file could not be found!";
                errorMessage[1] = "If this is first game, have fun!";
                errorMessage[2] = "Otherwise your data was lost, sorry :(";

                //fix up the game to match file error
                SetUpErrorScreen();
            }
            catch (Exception)
            {
                //set the three lines of error messages
                errorMessage[0] = "The file could not be read!";
                errorMessage[1] = "Sorry for the inconveinience :(";
                errorMessage[2] = "Have fun playing the game!";

                //fix up the game to match file error
                SetUpErrorScreen();
            }
            finally
            {
                //check if infile does not equal null
                if(inFile != null)
                {
                    //close infile
                    inFile.Close();
                }
            }
        }

        //pre: a valid gametime
        //post: none
        //description: updates the game
        protected override void Update(GameTime gameTime)
        {
            //update mouse state
            prevMouse = mouse;
            mouse = Mouse.GetState();

            //update keyboard state
            prevKb = kb;
            kb = Keyboard.GetState();

            if (gameState == GAMEPLAY)
            {
                //check if media player is not currently playing
                if (MediaPlayer.State != MediaState.Playing)
                {
                    //start music at gameplay music
                    MediaPlayer.Play(gameplayMusic);
                }
            }
            else
            {
                //check if media player is not currently playing
                if (MediaPlayer.State != MediaState.Playing)
                {
                    //start music at gameplay music
                    MediaPlayer.Play(menuMusic);
                }
            }

            //update game based on gamestate
            switch (gameState)
            {
                case MENU:
                    //update menu screen
                    UpdateMenu();
                    break;

                case INSTRUCTIONS:
                    //update instructions screen
                    UpdateInstructions();
                    break;

                case GAMEPLAY:
                    //update gameplay screen
                    UpdateGameplay(gameTime);
                    break;

                case ENDLEVEL:
                    //update the end level screen
                    UpdateEndlevel();
                    break;

                case SHOP:
                    //update the shop screen
                    UpdateShop();
                    break;

                case ENDGAME:
                    //update the endgame screen
                    UpdateEndgame();
                    break;

                case STATS:
                    //update the statistics screen
                    UpdateStats();
                    break;

                case FILE_ERROR:
                    //update the error screen
                    UpdateErrorScreen();
                    break;

                case EXIT:
                    //exit the game
                    Exit();
                    break;
            }

            base.Update(gameTime);
        }

        //pre: a valid gametime
        //post: none
        //description: draws the game
        protected override void Draw(GameTime gameTime)
        {
            //begin the spritebatch
            spriteBatch.Begin();

            //draw game based on gamestate
            switch (gameState)
            {
                case MENU:
                    //draw menu screen
                    DrawMenu();
                    break;

                case INSTRUCTIONS:
                    //draws the instructions screen
                    DrawInstructions();
                    break;

                case GAMEPLAY:
                    //draws the gameplay screen
                    DrawGameplay();
                    break;

                case ENDLEVEL:
                    //draws the end level screen
                    gameObject.DrawEndLevel();
                    break;

                case SHOP:
                    //draws the shop
                    DrawShop();
                    break;

                case ENDGAME:
                    //draws the stats screen with a game over title
                    gameObject.DrawStats(GAME_OVER_TITLE);
                    break;

                case STATS:
                    //draws the stats screen with a stats title
                    gameObject.DrawStats(STATS_TITLE);
                    break;

                case FILE_ERROR:
                    //draws the error screen
                    DrawErrorScreen();
                    break;
            }

            //end the spritebatch
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //pre: none
        //post: none
        //description: updates the menu screen
        private void UpdateMenu()
        {
            //checks through each of the button rectangles in the menu screen
            for(int i = 0; i < buttonRecs.Length; i++)
            {
                //checks if button rectangel at index i is being hovered
                if(buttonRecs[i].Contains(mouse.Position))
                {
                    //set is hover variabel at index i to true
                    isHover[i] = true;

                    //check if user clicked the button
                    if(mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        //create instance of button click sound
                        buttonClickSnd.CreateInstance().Play();

                        //set gamestate to next gamestate
                        gameState = MENU_NEXT_GAMESTATE[i];

                        //check if next gamesate is instructions
                        if(MENU_NEXT_GAMESTATE[i] == INSTRUCTIONS)
                        {
                            //make background
                            gameObject.MakeBackground();
                        }

                    }
                }
                else
                {
                    //set is hover variable at indexi to false
                    isHover[i] = false;
                }
            }
        }

        //pre: none
        //pose: none
        //description: draws the menu screen
        private void DrawMenu()
        {
            //draw menu screen background
            spriteBatch.Draw(menuBackImg, fullScreenRec, Color.White);
            spriteBatch.Draw(blankPixelImg, fullScreenRec, Color.Black * 0.2f);

            //draw menu screen title
            spriteBatch.Draw(menuTitleImg, menuTitleRec, Color.White);

            //draw each menu screen button
            for(int i = 0; i < buttonRecs.Length; i++)
            {
                //draw menu screen button at index i
                spriteBatch.Draw(buttonImg, buttonRecs[i], Color.White);
                spriteBatch.Draw(blankPixelImg, buttonRecs[i], Color.Black * BUT_VIS[Convert.ToInt32(isHover[i])]);
                spriteBatch.DrawString(pixelFont, menuButtonPrompts[i], menuButtonPromptLocs[i], BUT_PROMPT_COL[Convert.ToInt32(isHover[i])]);
            }
        }

        //pre: a valid gametime
        //post: none
        //desctiption: updates the gameplay
        private void UpdateGameplay(GameTime gameTime)
        {
            //check if the update gameplay method returned false
            if(!gameObject.UpdateGameplay(gameTime))
            {
                //set gamestate to end level
                gameState = ENDLEVEL;

                //pause the media player
                MediaPlayer.Pause();
            }
        }

        //pre: none
        //post: none
        //description: draw the gameplay screen
        private void DrawGameplay()
        {
            //draw the gameplay screen
            gameObject.DrawGameplay();
        }

        //pre: none
        //post: none
        //description: updates the endgame screen
        private void UpdateEndgame()
        {
            //check if the update stats returned false
            if (!gameObject.UpdateStats())
            {
                //set gamestate to menu
                gameState = MENU;

                //reset the game
                gameObject.ResetGame();
            }
        }

        //pre:none
        //post: none
        //description: updates the stats screen
        private void UpdateStats()
        {
            //check if the update stats returned false
            if (!gameObject.UpdateStats())
            {
                //set gamestate to menu
                gameState = MENU;
            }
        }

        //pre: none
        //post: none
        //descriptino: draw the instructions screen
        private void UpdateInstructions()
        {
            //check if the user clicked enter
            if(kb.IsKeyDown(Keys.Enter))
            {
                //create instance of the button click sound
                buttonClickSnd.CreateInstance().Play();

                //set gamestate to gameplay
                gameState = GAMEPLAY;

                //pause the media player
                MediaPlayer.Pause();
            }
        }

        //pre: none
        //post: none
        //description: draws the instruction screen
        private void DrawInstructions()
        {
            //draws the background for instructions screen
            gameObject.DrawGameplay();
            spriteBatch.Draw(blankPixelImg, instrucBack, Color.Black * 0.5f);

            //draw instructions title
            spriteBatch.Draw(instrucTitleImg, instrucTitleRec, Color.White);

            //draw each line of instruction screen
            for (int i = 0; i < INTRUC_LINES.Length; i++)
            {
                //draw line of instruction screen at index i
                spriteBatch.DrawString(pixelFont, INTRUC_LINES[i], instrucLineLocs[i], Color.White);
            }

            //draw leave instructions screen prompt
            spriteBatch.DrawString(pixelFont, INSTRUC_LEAVE_PROMT, instrucLeaveLoc, Color.Goldenrod);
        }

        //pre: none
        //post: none
        //description: updates the endlevel
        private void UpdateEndlevel()
        {
            //check if update end level screen returned false
            if (!gameObject.UpdateEndLevel())
            {
                //check if the level does not equal the total number of levels
                if (gameObject.GetLevel() != NUM_LEVELS)
                {
                    //set gamestate to shop
                    gameState = SHOP;
                }
                else
                {
                    //set gamestate to endgame
                    gameState = ENDGAME;
                }
            }
        }

        //pre: none
        //post: none
        //description: updates the shop screen
        private void UpdateShop()
        {
            //check if the update shop returned false
            if (!gameObject.UpdateShop())
            {
                //set gamestate to gameplay
                gameState = GAMEPLAY;

                //set up next level
                gameObject.SetUpLevel();

                //pause the music
                MediaPlayer.Pause();
            }
        }

        //pre: none
        //post: none
        //description: draws the shop screen
        private void DrawShop()
        {
            //draws the shop screen
            gameObject.DrawShop();
        }
       
        //pre: none
        //post: none
        //description: updates the error screen
        private void UpdateErrorScreen()
        {
            //check if the bottom button is being hovered
            if (buttonRecs[BOT_BUT].Contains(mouse.Position))
            {
                //set the is hover variabel at bottom button to true
                isHover[BOT_BUT] = true;

                //check if the user clicked the left button
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    //create instance of button click sound
                    buttonClickSnd.CreateInstance().Play();

                    //set gamesatte to menu
                    gameState = MENU;

                    //reset the overall stat values
                    gameObject.ResetOverallStatVals();
                }
            }
            else
            {
                //set the hover variable to false
                isHover[BOT_BUT] = false;
            }
        }

        //pre: none
        //post: none
        //description: draws the file errror screen
        private void DrawErrorScreen()
        {
            //draw background for error screen
            spriteBatch.Draw(woodBackImg, fullScreenRec, Color.White);
            spriteBatch.Draw(blankPixelImg, errorBackRec, Color.Black * 0.5f);

            //draw error messages
            for (int i = 0; i < errorMessage.Length; i++)
            {
                //draw error message at index i
                spriteBatch.DrawString(pixelFont, errorMessage[i], errorMessageloc[i], Color.White);
            }

            //draw the title of file error screen
            spriteBatch.Draw(errorTitleImg, errorTitleRec, Color.White);

            //draw error button
            spriteBatch.Draw(buttonImg, buttonRecs[BOT_BUT], Color.White);
            spriteBatch.Draw(blankPixelImg, buttonRecs[BOT_BUT], Color.Black * BUT_VIS[Convert.ToInt32(isHover[BOT_BUT])]);
            spriteBatch.DrawString(pixelFont, errorButPrompt, errorButPromptLoc, BUT_PROMPT_COL[Convert.ToInt32(isHover[BOT_BUT])]);
        }

        //pre:none
        //post: none
        //description: sets up the error screen
        private void SetUpErrorScreen()
        {
            //sets gamestate to file error screen
            gameState = FILE_ERROR;

            //set error message location
            for (int i = 0; i < errorMessageloc.Length; i++)
            {
                //sets x and y location of error message at index i
                errorMessageloc[i].X = GetCentreTextX(pixelFont, errorMessage[i]);
                errorMessageloc[i].Y = ERROR_MESSAGE_START_Y + ERROR_MES_SPACE * (i);
            }

            //reset each overall stat
            for(int i = 0; i < overallReadStats.Length; i++)
            {
                //set overall stats at index i to zero
                overallReadStats[i] = "0";
            }
        }

        //pre: a valid spritefont, a valid string text
        //post: the x location of the centered text
        //description: returns the x position for the text to be centred
        public static int GetCentreTextX(SpriteFont font, string text)
        {
            //return the x position for the text to be centred
            return (int)(0.5 * (screenWidth - font.MeasureString(text).X));
        }
    }
}