//Author: Dylan Nagel
//File Name: GameObject.cs
//Project Name: NagelD_PASS2
//Description: Controls the game flow and holds all game objects and stats

using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Helper;

namespace NagelD_PASS2
{
    public class GameObject
    {
        //store mob quantity variables
        private const int NUM_START_MOB = 10;
        private const int MOB_INC = 5;
        private readonly int[] MAX_MOB_SCREEN = new int[] { 2, 3, 3, 5, 3 };

        //store the odds of mob spawn per level number
        private readonly Vector2[,] MOB_SPAWN_ODDS = new Vector2[,]{
            {new Vector2(0,70), new Vector2(71,90), new Vector2(91,100), new Vector2(101,102), new Vector2(101,102)},
            {new Vector2(0,50), new Vector2(51,80), new Vector2(81,100), new Vector2(101,102), new Vector2(101,102)},
            {new Vector2(0,40), new Vector2(41,60), new Vector2(61,80), new Vector2(81,100), new Vector2(101,102)},
            {new Vector2(0,50), new Vector2(51,65), new Vector2(66,80), new Vector2(81,95), new Vector2(96,100)},
            {new Vector2(0,10), new Vector2(11,30), new Vector2(31,55), new Vector2(56,80), new Vector2(81,100)}};

        //store the mob rate of spawn depending on level
        private readonly float[] MOB_SPAWN_TIME = new float[] { 2, 1.7f, 1.3f, 1.2f, 1 };

        //stores monster information
        public static readonly int[] MONST_HEALTH = new int[] { 1, 3, 4, 2, 5 };
        public static readonly int[] KILL_SCORES = new int[] { 10, 40, 25, 25, 100 };

        //store the buff names
        private readonly string[] BUFF_NAMES = new string[] { "Speed", "Rapid Fire", "Strength", "Double Points" };

        //stores the damage done by mobs
        private const int CREEP_DAMAGE = -40;
        private const int SKEL_DAMAGE = -20;

        //stores the end level location information
        private const int END_LEV_BACK_HEIGHT = 350;
        private const int STATS_BACK_HEIGHT = 350;
        private const int SIDE_BOX_SPACE = 10;
        private const int END_LEV_LINE_SPACE = 60;
        private const int RIGHT_COL_X = 310;

        //stores the before and after locations for prefix and suffix
        private const int BEFORE = 0;
        private const int AFTER = 1;

        //stores the location of level statistics in array
        private const int LEV_JUST_FINISHED = 0;
        private const int LEV_KILLS = 1;
        private const int LEV_SHOTS_FIRED = 2;
        private const int LEV_HITS = 3;
        private const int LEV_HIT_PERC = 4;

        //stores the stats screen location information
        private const int END_LINE_SPACE = 47;
        private const int STATS_RIGHT_COL_X = 265;

        //stores the location of the statistics in array
        private const int TOT_VILL_KILL = 0;
        private const int TOP_HIT_PERC = 5;
        private const int GAMES_PLAYED = 6;
        private const int TOT_SHOTS_FIRED = 7;
        private const int TOT_SHOTS_HIT = 8;
        private const int TOT_HIT_PERC = 9;
        private const int AV_SHOTS_PER_GAME = 10;
        private const int AV_KILLS_PER_GAME = 11;
        private const int HIGH_SCORE = 12;
        private const int TOT_KILLS = 13;

        //stores the hit marker information
        private const int HIT_MARKER_LENGTH = 20;
        private const int HIT_MARKER_TIME = 100;

        //stores a streamer writer
        private static StreamWriter outFile;

        //stores the gameplay buff information
        private const int GAMEPLAY_BUFF_LENGTH = 24;
        private readonly float[] GAMEPLAY_BUFF_VIS = new float []{ 0.3f, 1f };

        //stores shop information
        private Rectangle[] itemFrameRecs = new Rectangle[4];
        private Rectangle[] buffRecs = new Rectangle[4];
        private bool[] isBuffHover = new bool[4];
        private Vector2[] buffNameLocs = new Vector2[4];
        private Rectangle[] buffNameBack = new Rectangle[4];
        private Vector2[] buffPriceLocs = new Vector2[4];
        private Rectangle[] buffPriceBack = new Rectangle[4];

        //stores the timer for spawn rate
        private Timer spawnTimer;

        //stores the values that are passed in as perameters
        private Texture2D woodBackImg;
        private Texture2D blankPixelImg;
        private Texture2D buttonImg;
        private Texture2D[] titleImgs;
        private Texture2D[] buffImgs;
        private Texture2D itemFrameImg;
        private SpriteFont pixelFont;
        private SpriteFont statsFont;
        private Texture2D hitMarkerImg;
        private SoundEffect buttonClickSnd;
        private SoundEffect arrowHitSnd;
        private SoundEffect damageTakenSnd;
        private SoundEffect creepExploadSnd;

        //stores the main rectangles of the game 
        private Rectangle fullScreenRec;
        private Rectangle buttonRec;
        private Rectangle endLevelBackRec;
        private Rectangle statsBackRec;
        private Rectangle shopBackRec;

        //stores the total score information for shop 
        private Vector2 totScoreLoc;
        private string totScorePref = "Current Total Score: ";

        //stores the gameplay screen score information
        private Vector2 gameScorePos;
        private Rectangle gameScoreBackRec;

        //stores the end level screen information
        private Vector2[] levScoreLocs = new Vector2[Game1.NUM_LEVELS];
        private Vector2[] levCurResultLocs = new Vector2[5];
        private string[] levScorePref = new string[] { "Level ", " Score: " };
        private string[] levCurResultPrefs = new string[] { "Prev Level: ", "Kills: ", "Shots Fired: ", "Shots Hits: ", "Hit %: " };
        private string levButPrompt = "Click to go to shop";
        private Vector2 levButPromptLoc;
        private Color[] buffPromptCol = new Color[] { Color.White, Color.White, Color.White, Color.White };

        //stores the statistics screen information
        private Rectangle[] titleRecs = new Rectangle[4];
        private Vector2[] statsLocs = new Vector2[14];
        private string[] statsPrefixs = new string[] { "Tot Vill Kill: ", "Tot Creep Kill: ", "Tot Skel Kill: ", "Tot Pill Kill: ", "Tot End Kill: ", "Top Hit %: ", "Games Played: ", "Tot Shots Fired: ", "Tot Shots Hit: ", "All-Time Hit %: ", "Av Shots Per Game : ", "Av Kills Per Game: ", "High Score: ", "Total Kills: " };
        private float[] overallStats = new float[14];
        private string statButPrompt = "Click to go to menu";
        private Vector2 statButPromptLoc;

        //stores the shop score position
        private Vector2 shopScorePos;

        //stores the shop button information
        private Vector2 shopButPrompotLoc;
        private string shopButPrompt;

        //stores a player object
        private Player player;

        //stores the is level on variable
        private bool isLevelOn = true;

        //stores the level information
        private int numMonstSpawned = 0;
        private int levelNum = 0;

        //stores if the button is hovered
        private bool isButHover;

        //stores rectangles for gameplay buffs
        private Rectangle[] gameplayBuffRecs = new Rectangle[Game1.NUM_BUFFS];
        private Rectangle gameplayBuffBackRec;

        //stores the weapons in the game
        private List<Weapon> monstWeapons = new List<Weapon>();
        private List<Weapon> playWeapons = new List<Weapon>();

        //stores the hit markers in the game
        private List<Rectangle> hitMarkerRecs = new List<Rectangle>();
        private List<Timer> hitmarkerTimers = new List<Timer>();

        //stores the monsters in the game
        private List<Monster> aliveMonsts = new List<Monster>();
        private List<Monster> deadMonsts = new List<Monster>();

        //stores a level object
        private Level level;

        //pre: a valid player image, a valid array of background block images, a valid wooden background image, a valid blank pixel image, a validd button image, a valid array of title images, a valid array of buff images, a valid item frame iamge, a valid font for pixel font, a valid font for stats font, a valid image for hit marker, a valid sound effect for button click, a valid sound for arrow hit, a valid sound for damage taken, a valid sound for creeper explosion
        //post: none
        //description: creates a object object
        public GameObject(Texture2D playerImg, Texture2D[] backBlockImgs, Texture2D woodBackImg, Texture2D blankPixelImg, Texture2D buttonImg, Texture2D[] titleImgs, Texture2D[] buffImgs, Texture2D itemFrameImg, SpriteFont pixelFont, SpriteFont statsFont, Texture2D hitMarkerImg, SoundEffect buttonClickSnd, SoundEffect arrowHitSnd, SoundEffect damageTakenSnd, SoundEffect creepExploadSnd)
        {
            //store all values passed in through the peramters
            this.woodBackImg = woodBackImg;
            this.blankPixelImg = blankPixelImg;
            this.buttonImg = buttonImg;
            this.titleImgs = titleImgs;
            this.buffImgs = buffImgs;
            this.itemFrameImg = itemFrameImg;
            this.pixelFont = pixelFont;
            this.statsFont = statsFont;
            this.hitMarkerImg = hitMarkerImg;
            this.buttonClickSnd = buttonClickSnd;
            this.arrowHitSnd = arrowHitSnd;
            this.damageTakenSnd = damageTakenSnd;
            this.creepExploadSnd = creepExploadSnd;

            //sets new player and new level object
            player = new Player(playerImg);
            level = new Level(backBlockImgs);

            //sets full screen rectangle
            fullScreenRec = new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight);

            //sets the spawn timer
            spawnTimer = new Timer(Timer.INFINITE_TIMER, false);

            //sets the rectangles for the titles
            titleRecs[Game1.STATS_TITLE] = new Rectangle((int)(0.5 * (Game1.screenWidth - titleImgs[Game1.STATS_TITLE].Width * 0.6)), 5, (int)(titleImgs[Game1.STATS_TITLE].Width * 0.6), (int)(titleImgs[Game1.STATS_TITLE].Height * 0.6));
            titleRecs[Game1.SHOP_TITLE] = new Rectangle((int)(0.5 * (Game1.screenWidth - titleImgs[Game1.SHOP_TITLE].Width * 0.6)), 5, (int)(titleImgs[Game1.SHOP_TITLE].Width * 0.6), (int)(titleImgs[Game1.SHOP_TITLE].Height * 0.6));
            titleRecs[Game1.END_LEVEL_TITLE] = new Rectangle((int)(0.5 * (Game1.screenWidth - titleImgs[Game1.END_LEVEL_TITLE].Width * 0.6)), 5, (int)(titleImgs[Game1.END_LEVEL_TITLE].Width * 0.6), (int)(titleImgs[Game1.END_LEVEL_TITLE].Height * 0.6));
            titleRecs[Game1.GAME_OVER_TITLE] = new Rectangle((int)(0.5 * (Game1.screenWidth - titleImgs[Game1.GAME_OVER_TITLE].Width * 0.6)), 5, (int)(titleImgs[Game1.GAME_OVER_TITLE].Width * 0.6), (int)(titleImgs[Game1.GAME_OVER_TITLE].Height * 0.6));

            //sets the rectangles for the background darkness
            endLevelBackRec = new Rectangle(0, (int)(0.5 * (Game1.screenHeight - END_LEV_BACK_HEIGHT)), Game1.screenWidth, END_LEV_BACK_HEIGHT);
            statsBackRec = new Rectangle(0, (int)(0.5 * (Game1.screenHeight - STATS_BACK_HEIGHT)), Game1.screenWidth, STATS_BACK_HEIGHT);
            shopBackRec = statsBackRec;

            //sets the item frame rectangles
            itemFrameRecs[Game1.SPEED_BUFF] = new Rectangle(SIDE_BOX_SPACE, shopBackRec.Y + SIDE_BOX_SPACE, 150, 140);
            itemFrameRecs[Game1.SHOOT_BUFF] = new Rectangle(Game1.screenWidth - SIDE_BOX_SPACE - 150, shopBackRec.Y + SIDE_BOX_SPACE, 150, 140);
            itemFrameRecs[Game1.STRENGTH_BUFF] = new Rectangle(SIDE_BOX_SPACE, shopBackRec.Y + shopBackRec.Height - SIDE_BOX_SPACE - 140, 150, 140);
            itemFrameRecs[Game1.SCORE_BUFF] = new Rectangle(Game1.screenWidth - SIDE_BOX_SPACE - 150, shopBackRec.Y + shopBackRec.Height - SIDE_BOX_SPACE - 140, 150, 140);

            //sets the location for the score in the shop
            shopScorePos = new Vector2(Game1.GetCentreTextX(pixelFont, "Total Score: " + player.GetTotalScore()), 0.5f * (Game1.screenHeight - pixelFont.MeasureString("Total Score: " + player.GetTotalScore()).Y));

            //loops through each item frame to set specific values
            for (int i = 0; i < itemFrameRecs.Length; i++)
            {
                //set up buff rectangles
                buffRecs[i] = new Rectangle((int)(itemFrameRecs[i].X + itemFrameRecs[i].Width * 0.5 - buffImgs[0].Width * 0.5 * 0.5), (int)(itemFrameRecs[i].Y + itemFrameRecs[i].Height * 0.5 - buffImgs[0].Height * 0.8 * 0.5), (int)(buffImgs[0].Width * 0.5), (int)(buffImgs[0].Height * 0.8));

                //set up buff name locations
                buffNameLocs[i] = new Vector2(itemFrameRecs[i].X + itemFrameRecs[i].Width * 0.5f - pixelFont.MeasureString(BUFF_NAMES[i]).X * 0.5f, itemFrameRecs[i].Y);

                //set up buff name background rectangles
                buffNameBack[i] = new Rectangle((int)buffNameLocs[i].X - 3, (int)buffNameLocs[i].Y, (int)pixelFont.MeasureString(BUFF_NAMES[i]).X + 6, (int)pixelFont.MeasureString(BUFF_NAMES[i]).Y + 3);

                //set up buff price locations
                buffPriceLocs[i] = new Vector2(itemFrameRecs[i].X + itemFrameRecs[i].Width * 0.5f - pixelFont.MeasureString(Game1.BUFF_PRICES[i] + " Score").X * 0.5f, itemFrameRecs[i].Y + itemFrameRecs[i].Height - pixelFont.MeasureString(Game1.BUFF_PRICES[i] + " Score").Y);

                //set up buff price background rectangles
                buffPriceBack[i] = new Rectangle((int)buffPriceLocs[i].X - 3, (int)buffPriceLocs[i].Y - 3, (int)pixelFont.MeasureString(Game1.BUFF_PRICES[i] + " Score").X + 6, (int)pixelFont.MeasureString(Game1.BUFF_PRICES[i] + " Score").Y + 3);
            }

            //sets up button rectangle location
            buttonRec = new Rectangle(70, 522, 409, 101);

            //sets up total score Y location
            totScoreLoc.Y = endLevelBackRec.Y + SIDE_BOX_SPACE;

            //sets up game score information
            gameScorePos = new Vector2(SIDE_BOX_SPACE, SIDE_BOX_SPACE);
            gameScoreBackRec = new Rectangle(SIDE_BOX_SPACE - 5, SIDE_BOX_SPACE - 5, (int)pixelFont.MeasureString("Score: 0").X + 10, (int)pixelFont.MeasureString("Score: 0").Y + 10);

            //sets up button prompt locs
            levButPromptLoc = new Vector2(Game1.GetCentreTextX(pixelFont, levButPrompt), (int)(buttonRec.Center.Y - 0.5 * pixelFont.MeasureString(levButPrompt).Y));
            statButPromptLoc = new Vector2(Game1.GetCentreTextX(pixelFont, statButPrompt), (int)(buttonRec.Center.Y - 0.5 * pixelFont.MeasureString(statButPrompt).Y));
            shopButPrompotLoc.Y = buttonRec.Center.Y - pixelFont.MeasureString("QWERTYUIOPASDFGHJKLZXCVBNM1234567890").Y * 0.5f;

            //loops through each end level score location
            for (int i = 0; i < levScoreLocs.Length; i++)
            {
                //sets location value
                levScoreLocs[i] = new Vector2(SIDE_BOX_SPACE, totScoreLoc.Y + END_LEV_LINE_SPACE + END_LEV_LINE_SPACE * i);
            }

            //loops through each end level result location
            for(int i = 0; i < levCurResultLocs.Length; i++)
            {
                //sets location value
                levCurResultLocs[i] = new Vector2(RIGHT_COL_X, totScoreLoc.Y + END_LEV_LINE_SPACE + END_LEV_LINE_SPACE * i);
            }

            //loops through each stats screen location up to and including top hit percentage
            for(int i = TOT_VILL_KILL; i <= TOP_HIT_PERC; i++)
            {
                //set up location for stats
                statsLocs[i] = new Vector2(SIDE_BOX_SPACE, statsBackRec.Y + END_LINE_SPACE * (i + 1));
            }

            //loops through each stats screen location starting st gamesplayed 
            for (int i = GAMES_PLAYED; i <= AV_KILLS_PER_GAME; i++)
            {
                statsLocs[i] = new Vector2(STATS_RIGHT_COL_X, statsBackRec.Y + END_LINE_SPACE * (i - GAMES_PLAYED + 1));
            }

            //sets up statistics screen stats locs for highscore and total kills
            statsLocs[HIGH_SCORE] = new Vector2(Game1.GetCentreTextX(pixelFont, statsPrefixs[HIGH_SCORE] + overallStats[HIGH_SCORE]), statsBackRec.Y + SIDE_BOX_SPACE);
            statsLocs[TOT_KILLS] = new Vector2(Game1.GetCentreTextX(pixelFont, statsPrefixs[TOT_KILLS] + 0), statsBackRec.Bottom - SIDE_BOX_SPACE - pixelFont.MeasureString(statsPrefixs[TOT_KILLS] + 0).Y);

            //loops through each gameplay buff rectangle to set rectangle
            for(int i = 0; i < gameplayBuffRecs.Length; i++)
            {
                //set gameplay buff rectangle at index i
                gameplayBuffRecs[i] = new Rectangle(Game1.screenWidth - (SIDE_BOX_SPACE + GAMEPLAY_BUFF_LENGTH) * (1 + i), SIDE_BOX_SPACE, GAMEPLAY_BUFF_LENGTH, GAMEPLAY_BUFF_LENGTH);
            }

            //set gameplay buff background rectangle
            gameplayBuffBackRec = new Rectangle(408, SIDE_BOX_SPACE - 5, 137, (int)pixelFont.MeasureString("Score: 0").Y + 10);
        }

        //pre: a valid gametime
        //post: a bool representing if gameplay is over
        //description: updates gameplay
        public bool UpdateGameplay(GameTime gameTime)
        {
            //updates spawn timer  timer
            spawnTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

            //update the hit markers
            UpdateHitMarkers(gameTime);

            UpdatePlayer(gameTime);

            //update the alive monsters
            UpdateAliveMonst(gameTime);

            //spawn in new monsters if is time
            AddRandMonst(CheckMobSpawn());

            //update the player weapons
            UpdatePlayerWeapons(gameTime);

            //check if a player arrow hit a monster
            CheckPlayArrowHit();

            //update the dead monsters
            UpdateDeadMonst(gameTime);

            //update the monster weapons
            UpdateMonstWeapons(gameTime);

            //return if the level is on
            return isLevelOn;
        }

        //pre: none
        //post: none
        //description: draw the gameplay background
        public void DrawGameplayBack()
        {
            //draw background
            level.DrawBackground();
            Game1.spriteBatch.Draw(blankPixelImg, fullScreenRec, Color.Black * 0.3f);
        }

        //pre: none
        //post: none
        //description: draw the gameplay screen
        public void DrawGameplay()
        {
            //draw the gameplay background
            DrawGameplayBack();

            //check if the plaeyr is not scared
            if (!player.GetFear())
            {
                //draw each dead monster
                for (int i = 0; i < deadMonsts.Count; i++)
                {
                    //draw dead monster at index i
                    deadMonsts[i].Draw();
                }

                //draw each player weapon
                for (int i = 0; i < playWeapons.Count; i++)
                {
                    //draw player weapon at index i
                    playWeapons[i].Draw();
                }

                //draw each mosnter weapons
                for (int i = 0; i < monstWeapons.Count; i++)
                {
                    //draw monster weaopn at index i
                    monstWeapons[i].Draw();
                }

                //draw each alive mosnter weapon
                for (int i = 0; i < aliveMonsts.Count; i++)
                {
                    //draw monster weapon at index i
                    aliveMonsts[i].Draw();
                }

                //draw each hit marker rectangle
                for(int i = 0; i < hitMarkerRecs.Count; i++)
                {
                    //draw hit marker at index i
                    Game1.spriteBatch.Draw(hitMarkerImg, hitMarkerRecs[i], Color.White);
                }
            }
            else
            {
                //draw each dead monster
                for (int i = 0; i < deadMonsts.Count; i++)
                {
                    //draw dead monster at index i
                    deadMonsts[i].Draw();
                }

                //darw each player weapon
                for (int i = 0; i < playWeapons.Count; i++)
                {
                    //draw player weapon at index i
                    playWeapons[i].Draw();
                }

                //draw each alive mosnter
                for (int i = 0; i < aliveMonsts.Count; i++)
                {
                    //draw alive monster at index i
                    aliveMonsts[i].Draw();
                }

                //draw a blank background image
                Game1.spriteBatch.Draw(blankPixelImg, fullScreenRec, Color.Black * 0.6f);

                //draw each monster weapon
                for (int i = 0; i < monstWeapons.Count; i++)
                {
                    //draw monster weapon at index i
                    monstWeapons[i].Draw();
                }
            }

            //draw the player
            player.DrawPlayer();

            //draw the score
            Game1.spriteBatch.Draw(blankPixelImg, gameScoreBackRec, Color.Black * 0.5f);
            Game1.spriteBatch.DrawString(pixelFont, "Score: " + player.GetLevelScore(levelNum), gameScorePos, Color.Goldenrod);

            //draw the gameplay buff background rectangle
            Game1.spriteBatch.Draw(blankPixelImg, gameplayBuffBackRec, Color.Black * 0.5f);

            //draw each gameplay buff
            for(int i = 0; i < gameplayBuffRecs.Length; i++)
            {
                //draw gameplay buff at index i
                Game1.spriteBatch.Draw(buffImgs[i], gameplayBuffRecs[i], Color.White * GAMEPLAY_BUFF_VIS[Convert.ToInt32(player.GetBuffStatus(i))]);
            }
        }

        //pre: none
        //post: none
        //description: updates the shop screen
        public bool UpdateShop()
        {
            //check if each buff is being hovered
            for(int i = 0; i < isBuffHover.Length; i++)
            {
                //check if the player does not have the buff status at index i
                if(!player.GetBuffStatus(i))
                {
                    //check if the total score is greater than or equal to the buff price at index i
                    if (player.GetTotalScore() >= Game1.BUFF_PRICES[i])
                    {
                        //check if the user is hovering over the item frame rectangle
                        if (itemFrameRecs[i].Contains(Game1.mouse.Position))
                        {
                            //set visual values
                            buffPromptCol[i] = Color.Goldenrod;
                            isBuffHover[i] = true;

                            //check if the user clicked the item frame
                            if (Game1.mouse.LeftButton == ButtonState.Pressed && Game1.prevMouse.LeftButton != ButtonState.Pressed)
                            {
                                //create instance of button click sound
                                buttonClickSnd.CreateInstance().Play();

                                //add buff at index i to player
                                player.AddBuff(i);

                                //set the shop score position to match new score
                                shopScorePos.X = Game1.GetCentreTextX(pixelFont, "Total Score: " + player.GetTotalScore());
                            }
                        }
                        else
                        {
                            //set visual information
                            isBuffHover[i] = false;
                            buffPromptCol[i] = Color.White;
                        }
                    }
                    else
                    {
                        //set visual information
                        buffPromptCol[i] = Color.Red;
                    }
                }
                else
                {
                    //set visual information
                    isBuffHover[i] = false;
                    buffPromptCol[i] = Color.Gray;
                }
            }

            //check if the user is hovering the button
            if (buttonRec.Contains(Game1.mouse.Position))
            {
                //set is hover variabel to true
                isButHover = true;

                //check if the user clicked the button
                if(Game1.mouse.LeftButton == ButtonState.Pressed && Game1.prevMouse.LeftButton != ButtonState.Pressed)
                {
                    //create instance of button click sound
                    buttonClickSnd.CreateInstance().Play();

                    //make the background
                    level.MakeBackground();

                    //return false
                    return false;
                }
            }
            else
            {
                //set the hover variable to true
                isButHover = false;
            }

            //return true
            return true;
        }

        //pre: none
        //post: none
        //description: draw the shop screen
        public void DrawShop()
        {
            //draw shop screen
            Game1.spriteBatch.Draw(woodBackImg, fullScreenRec, Color.White);
            Game1.spriteBatch.Draw(blankPixelImg, fullScreenRec, Color.Black * 0.3f);

            //draw shop title
            Game1.spriteBatch.Draw(titleImgs[Game1.SHOP_TITLE], titleRecs[Game1.SHOP_TITLE], Color.White);

            //draw the shop background
            Game1.spriteBatch.Draw(blankPixelImg, shopBackRec, Color.Black * 0.5f);

            //draw the total score for the shop
            Game1.spriteBatch.DrawString(pixelFont, "Total Score: " + player.GetTotalScore(), shopScorePos, Color.Goldenrod);

            //draw each potion information and background
            for (int i = 0; i < Game1.NUM_BUFFS; i++)
            {
                //draw potion information and background
                Game1.spriteBatch.Draw(itemFrameImg, itemFrameRecs[i], Color.White);

                //check if the player has the buff at index i
                if (!player.GetBuffStatus(i))
                {
                    //draw buff images
                    Game1.spriteBatch.Draw(buffImgs[i], buffRecs[i], Color.White);
                }

                //draw black squares over user interactible objects to show if hover
                Game1.spriteBatch.Draw(blankPixelImg, itemFrameRecs[i], Color.Black * Game1.BUT_VIS[Convert.ToInt32(isBuffHover[i])]);
                Game1.spriteBatch.Draw(blankPixelImg, buffNameBack[i], Color.Black * 0.7f);
                Game1.spriteBatch.DrawString(pixelFont, BUFF_NAMES[i], buffNameLocs[i], buffPromptCol[i]);
                Game1.spriteBatch.Draw(blankPixelImg, buffPriceBack[i], Color.Black * 0.7f);
                Game1.spriteBatch.DrawString(pixelFont, Game1.BUFF_PRICES[i] + " Score", buffPriceLocs[i], buffPromptCol[i]);
            }

            //draw the button
            Game1.spriteBatch.Draw(buttonImg, buttonRec, Color.White);
            Game1.spriteBatch.Draw(blankPixelImg, buttonRec, Color.Black * Game1.BUT_VIS[Convert.ToInt32(isButHover)]);
            Game1.spriteBatch.DrawString(pixelFont, shopButPrompt, shopButPrompotLoc, Game1.BUT_PROMPT_COL[Convert.ToInt32(isButHover)]);
        }

        //pre: none
        //post: a bool representing if the end level screen is over
        //description: update the end level screen
        public bool UpdateEndLevel()
        {
            //check if the user is hovering the button
            if (buttonRec.Contains(Game1.mouse.Position))
            {
                //set button hover status to true
                isButHover = true;

                //check if the user clicked the button
                if (Game1.mouse.LeftButton == ButtonState.Pressed)
                {
                    //create instance of button click sound
                    buttonClickSnd.CreateInstance().Play();

                    //reset the level statistics
                    ResetLevelStats();

                    //check if the levelnumber is not the max level
                    if (levelNum != Game1.NUM_LEVELS)
                    {
                        //set up the shop
                        SetUpShop();
                    }
                    else
                    {
                        //set up the endgame
                        SetUpEndgame();
                    }

                    //return false
                    return false;
                }
            }
            else
            {
                //set is button hover to tfalse
                isButHover = false;
            }

            //return true
            return true;
        }

        //pre: none
        //post: none
        //description: draw the end level screen
        public void DrawEndLevel()
        {
            //draw the gameplay background
            DrawGameplayBack();

            //draw the title image for end level
            Game1.spriteBatch.Draw(titleImgs[Game1.END_LEVEL_TITLE], titleRecs[Game1.END_LEVEL_TITLE], Color.White);

            //draw the background darkness
            Game1.spriteBatch.Draw(blankPixelImg, endLevelBackRec, Color.Black * 0.5f);

            //draw the total score
            Game1.spriteBatch.DrawString(pixelFont, totScorePref + player.GetTotalScore(), totScoreLoc, Color.Goldenrod);

            //draw score for each level completed
            for(int i = 0; i <= levelNum; i++)
            {
                //draw score for level at index i
                Game1.spriteBatch.DrawString(pixelFont, levScorePref[BEFORE] + (i + 1) + levScorePref[AFTER] + player.GetLevelScore(i), levScoreLocs[i], Color.White);
            }

            //draw score for each level not completed
            for (int i = levelNum + 1; i < Game1.NUM_LEVELS; i++)
            {
                //draw score for level at index i
                Game1.spriteBatch.DrawString(pixelFont, levScorePref[BEFORE] + (i + 1) + levScorePref[AFTER] + "N/A", levScoreLocs[i], Color.White);
            }

            //draw teh right side of screen stats
            Game1.spriteBatch.DrawString(pixelFont, levCurResultPrefs[LEV_JUST_FINISHED] + (levelNum + 1), levCurResultLocs[LEV_JUST_FINISHED], Color.White);
            Game1.spriteBatch.DrawString(pixelFont, levCurResultPrefs[LEV_KILLS] + player.GetLevKills(), levCurResultLocs[LEV_KILLS], Color.White);
            Game1.spriteBatch.DrawString(pixelFont, levCurResultPrefs[LEV_SHOTS_FIRED] + player.GetLevShotsFired(), levCurResultLocs[LEV_SHOTS_FIRED], Color.White);
            Game1.spriteBatch.DrawString(pixelFont, levCurResultPrefs[LEV_HITS] + player.GetLevHits(), levCurResultLocs[LEV_HITS], Color.White);
            Game1.spriteBatch.DrawString(pixelFont, levCurResultPrefs[LEV_HIT_PERC] + player.GetLevHitPerc(), levCurResultLocs[LEV_HIT_PERC], Color.White);

            //draw the button
            Game1.spriteBatch.Draw(buttonImg, buttonRec, Color.White);
            Game1.spriteBatch.Draw(blankPixelImg, buttonRec, Color.Black * Game1.BUT_VIS[Convert.ToInt32(isButHover)]);
            Game1.spriteBatch.DrawString(pixelFont, levButPrompt, levButPromptLoc, Game1.BUT_PROMPT_COL[Convert.ToInt32(isButHover)]);
        }

        //pre: none
        //post: a bool represetning if the statstics scree is over
        //description: update the statistics screen
        public bool UpdateStats()
        {
            //check if user is hovering the button
            if (buttonRec.Contains(Game1.mouse.Position))
            {
                //set button hover status to true
                isButHover = true;

                //check if the user clicked the mouse
                if (Game1.mouse.LeftButton == ButtonState.Pressed && Game1.prevMouse.LeftButton != ButtonState.Pressed)
                {
                    //create instance of button click sound
                    buttonClickSnd.CreateInstance().Play();

                    //return false
                    return false;
                }
            }
            else
            {
                //set the button hover status to false
                isButHover = false;
            }

            //return true
            return true;
        }

        //pre: a valid title number
        //post: none
        //description: draws the statistics screen
        public void DrawStats(int titleNum)
        {
            //draws the background
            Game1.spriteBatch.Draw(woodBackImg, fullScreenRec, Color.White);
            Game1.spriteBatch.Draw(blankPixelImg, statsBackRec, Color.Black * 0.5f);

            //draws the title for statistics
            Game1.spriteBatch.Draw(titleImgs[titleNum], titleRecs[titleNum], Color.White);

            //draws the button
            Game1.spriteBatch.Draw(buttonImg, buttonRec, Color.White);
            Game1.spriteBatch.Draw(blankPixelImg, buttonRec, Color.Black * Game1.BUT_VIS[Convert.ToInt32(isButHover)]);
            Game1.spriteBatch.DrawString(pixelFont, statButPrompt, statButPromptLoc, Game1.BUT_PROMPT_COL[Convert.ToInt32(isButHover)]);

            //draws the stats up to high score
            for(int i = 0; i < HIGH_SCORE; i++)
            {
                //draws statistics at index i
                Game1.spriteBatch.DrawString(statsFont, statsPrefixs[i] + overallStats[i], statsLocs[i], Color.White);
            }

            //draws stats from highscore onwards
            for(int i = HIGH_SCORE; i < statsLocs.Length; i++)
            {
                //draws statistics at index i
                Game1.spriteBatch.DrawString(pixelFont, statsPrefixs[i] + overallStats[i], statsLocs[i], Color.Goldenrod);
            }
        }

        //pre: none
        //post: none 
        //description: check for mob spawn 
        private int CheckMobSpawn()
        {
            //check if the number of monsters spawned is less than level maximum
            if (numMonstSpawned < NUM_START_MOB + levelNum * MOB_INC)
            {
                //check if the number of monsters on the screen is less than the maximum allowed on the screen
                if (aliveMonsts.Count < MAX_MOB_SCREEN[levelNum])
                {
                    //check if the spawn timer is active
                    if (spawnTimer.IsActive())
                    {
                        //check if the spawn timer time passed is greater than the spawn time amount for level
                        if (spawnTimer.GetTimePassed() >= MOB_SPAWN_TIME[levelNum] * 1000)
                        {
                            //add one to number of monstesr spawned
                            numMonstSpawned++;

                            //reset the timer
                            spawnTimer.ResetTimer(false);

                            //set new random number
                            int randomNum = Game1.rng.Next(0, 101);

                            //loop through each monster type
                            for (int i = 0; i < Game1.NUM_MONST; i++)
                            {
                                //check if the random number is within the mob spawn odds
                                if (MOB_SPAWN_ODDS[levelNum, i].X <= randomNum && MOB_SPAWN_ODDS[levelNum, i].Y >= randomNum)
                                {
                                    //return index
                                    return i;
                                }
                            }
                        }
                    }
                    else
                    {
                        //activate spawn timer
                        spawnTimer.Activate();
                    }
                }
            }
            else
            {
                //check if the number of monsters and monster weaopns is zero
                if (aliveMonsts.Count == 0 && deadMonsts.Count == 0 && monstWeapons.Count == 0)
                {
                    //set is level on to false
                    isLevelOn = false;

                    //set up end level screen
                    SetUpEndLevel();
                }
            }

            //return -1
            return -1;
        }


        //pre: none
        //post: none
        //description: set up level
        public void SetUpLevel()
        {
            //set game score background width to size to match score
            gameScoreBackRec.Width = (int)pixelFont.MeasureString("Score: " + player.GetLevelScore(levelNum)).X + 10;

            //make the background
            level.MakeBackground();
        }

        //pre: a valid monster index
        //post: none
        //description: adds a random monster to list
        private void AddRandMonst(int monstIndx)
        {
            //adds monster based on monster index
            switch (monstIndx)
            {
                case Game1.VILLAGER:
                    //ads new villager
                    aliveMonsts.Add(new Villager());
                    break;

                case Game1.CREEPER:
                    //ads new creeper
                    aliveMonsts.Add(new Creeper());
                    break;

                case Game1.SKELETON:
                    //adds new skeleton
                    aliveMonsts.Add(new Skeleton());
                    break;

                case Game1.PILLAGER:
                    //adds new pillager
                    aliveMonsts.Add(new Pillager());
                    break;

                case Game1.ENDERMAN:
                    //adds new enderman
                    aliveMonsts.Add(new Enderman());
                    break;
            }
        }

        //pre: none
        //post: none
        //description: sets up end level screen
        private void SetUpEndLevel()
        {
            //stores the total score
            player.SetTotalScore(levelNum);

            //sets the x poition of total score
            totScoreLoc.X = Game1.GetCentreTextX(pixelFont, totScorePref + player.GetTotalScore());

            //check if the level number is equal to maximum level number
            if(levelNum == Game1.NUM_LEVELS - 1)
            {
                //set up the button propmt information accordingly
                levButPrompt = "Click to see overall statistics";
                levButPromptLoc.X = Game1.GetCentreTextX(pixelFont, "Click to see overall statistics");
            }
        }

        //pre: none
        //post: none
        //description: set up the shop screen
        private void SetUpShop()
        {
            //set up the shop score x position
            shopScorePos.X = Game1.GetCentreTextX(pixelFont, "Total Score: " + player.GetTotalScore());

            //set up the shop button prompt information
            shopButPrompt = "Click to begin level " + (levelNum + 1);
            shopButPrompotLoc.X = Game1.GetCentreTextX(pixelFont, shopButPrompt);
        }

        //pre: none
        //post: none
        //description: set up the endgame
        private void SetUpEndgame()
        {
            //check if the current highscore is less than the new score
            if (overallStats[HIGH_SCORE] < player.GetTotalScore())
            {
                //set current highscore to new score
                overallStats[HIGH_SCORE] = player.GetTotalScore();
            }

            //check if the current top hit percentage is less than the new hit percentage
            if(overallStats[TOP_HIT_PERC] < player.GetGameHitPerc())
            {
                //set top hit percentage to new hit percentage
                overallStats[TOP_HIT_PERC] = player.GetGameHitPerc();
            }

            //reset the game statistics
            player.ResetGameStats();

            //add one to games played
            overallStats[GAMES_PLAYED]++;

            //set the calculated statistics
            SetCalcStats();

            //save the data to file
            SaveDataToFile();
        }

        //pre: none
        //post: none
        //description: check if the player arrows hit monsters
        private void CheckPlayArrowHit()
        {
            //check through the living monsters
            for(int i = 0; i < aliveMonsts.Count; i++)
            {
                //check through the player weapons
                for(int x = 0; x < playWeapons.Count; x++)
                {
                    //check if the arrow at index x is touching the living monster at index i
                    if (aliveMonsts[i].GetRect().Intersects(playWeapons[x].GetRect()))
                    {
                        //add shots hit to statists 
                        player.AddShotsHit();

                        //add hit marker
                        hitMarkerRecs.Add(new Rectangle((int)(aliveMonsts[i].GetRect().Center.X - HIT_MARKER_LENGTH * 0.5), (int)(aliveMonsts[i].GetRect().Center.Y - HIT_MARKER_LENGTH * 0.5), HIT_MARKER_LENGTH, HIT_MARKER_LENGTH));
                        hitmarkerTimers.Add(new Timer(HIT_MARKER_TIME, true));

                        //create instance of button click sound
                        arrowHitSnd.CreateInstance().Play();

                        //check if the monster is not alive
                        if (!aliveMonsts[i].RemoveHp(player.GetDamage()))
                        {
                            //add value to number of monsters killed
                            overallStats[aliveMonsts[i].GetMonstType()]++;

                            //add a kill to stats
                            player.AddKill();

                            //add to the score of player
                            player.ChangeScore(aliveMonsts[i].GetKillScore() * player.GetScoreMult(), levelNum);

                            //change the game score background with accordingly
                            gameScoreBackRec.Width = (int)pixelFont.MeasureString("Score: " + player.GetLevelScore(levelNum)).X + 10;

                            //move alive monster to dead monster list
                            deadMonsts.Add(aliveMonsts[i]);
                            aliveMonsts.RemoveAt(i);
                        }

                        //remove the player weapon at index x
                        playWeapons.RemoveAt(x);

                        //break out of loop
                        break;
                    }
                }
            }
        }

        //pre: none
        //post: an int representing the level number
        //description: returns the level number
        public int GetLevel()
        {
            //return level number
            return levelNum;
        }

        //pre: none
        //post: none
        //description: resets the level statistics
        private void ResetLevelStats()
        {
            //adds one to level number
            levelNum++;

            //resets level variabels
            numMonstSpawned = 0;
            isLevelOn = true;

            //adds to overall stats
            overallStats[TOT_SHOTS_FIRED] += player.GetLevShotsFired();
            overallStats[TOT_SHOTS_HIT] += player.GetLevHits();

            //resets player
            player.Reset();

            //clears all player weapons and hit markers
            playWeapons.Clear();
            hitMarkerRecs.Clear();
            hitmarkerTimers.Clear();
        }

        //pre: none 
        //post: none
        //description: makes the background
        public void MakeBackground()
        {
            //makes the background
            level.MakeBackground();
        }

        //pre: none
        //post: none
        //description: saves data to a file
        private void SaveDataToFile()
        {
            try
            {
                //create new file to store overall stats
                outFile = File.CreateText("OverallScores.txt");

                //add each overall stat up to hit percentage to file
                for (int i = 0; i < TOT_HIT_PERC; i++)
                {
                    outFile.WriteLine(overallStats[i]);
                }

                //writes the high score stat to file
                outFile.Write(overallStats[HIGH_SCORE]);

                //close file
                outFile.Close();
            }
            catch (Exception)
            {

            }
        }

        //pre: a valid string array holding the overall stats
        //post: none
        //description: stores the overall statistics values
        public void StoreOverallStatVals(string[] overallStats)
        {
            //stores each stat up to total hit percentage
            for(int i = 0; i < TOT_HIT_PERC; i++)
            {
                //stores stat at index i
                this.overallStats[i] = (float)Convert.ToDouble(overallStats[i]);
            }

            //stores high score stat
            this.overallStats[HIGH_SCORE] = (float)Convert.ToDouble(overallStats[overallStats.Length - 1]);

            //sets the calculated stats
            SetCalcStats();
        }

        //pre: none
        //post: none
        //description: resets overall stat values
        public void ResetOverallStatVals()
        {
            //resets the overall stat values
            for (int i = 0; i < TOT_HIT_PERC; i++)
            {
                //sets overall stats at index i to zero
                overallStats[i] = 0;
            }

            //save reset values to file
            SaveDataToFile();
        }

        //pre: none
        //post: none 
        //description: sets calculated statistics
        private void SetCalcStats()
        {
            //resets the overall stats at total kills to zero
            overallStats[TOT_KILLS] = 0;

            //adds each monster kill amount to total kills
            for (int i = 0; i < Game1.NUM_MONST; i++)
            {
                //add monster kill amount at index i to total kills
                overallStats[TOT_KILLS] += overallStats[i];
            }

            //checks if the total shots fired does not equal to zero
            if (overallStats[TOT_SHOTS_FIRED] != 0)
            {
                //sets the total hit percentage
                overallStats[TOT_HIT_PERC] = (float)Math.Round(overallStats[TOT_SHOTS_HIT] / (double)overallStats[TOT_SHOTS_FIRED] * 100, 2);
            }
            else
            {
                //sets the total hit percentage to zero
                overallStats[TOT_HIT_PERC] = 0;
            }

            //checks if the overall games played does not equal zero
            if(overallStats[GAMES_PLAYED] != 0)
            {
                //sets the averages per game
                overallStats[AV_SHOTS_PER_GAME] = (float)Math.Round(overallStats[TOT_SHOTS_FIRED] / (double)overallStats[GAMES_PLAYED], 2);
                overallStats[AV_KILLS_PER_GAME] = (float)Math.Round(overallStats[TOT_KILLS] / (double)overallStats[GAMES_PLAYED], 2);
            }
            else
            {
                //set the averaes per game to zero
                overallStats[AV_SHOTS_PER_GAME] = 0;
                overallStats[AV_KILLS_PER_GAME] = 0;
            }

            //sets teh x position of high score and total kills
            statsLocs[HIGH_SCORE].X = Game1.GetCentreTextX(pixelFont, statsPrefixs[HIGH_SCORE] + overallStats[HIGH_SCORE]);
            statsLocs[TOT_KILLS].X = Game1.GetCentreTextX(pixelFont, statsPrefixs[TOT_KILLS] + overallStats[TOT_KILLS]);
        }

        //pre: none
        //post: none
        //description: resets the game
        public void ResetGame()
        {
            //sets level number to zero
            levelNum = 0;

            //rests player stats
            player.ResetBuffs();
            player.CompleteReset();

            //sets gameplay score backgroudn rectangle according to new score
            gameScoreBackRec.Width = (int)pixelFont.MeasureString("Score: " + player.GetLevelScore(levelNum)).X + 10;
        }

        //pre: a valid gametime
        //post: none
        //description: updates the living monsters
        private void UpdateAliveMonst(GameTime gameTime)
        {
            //check through each the alive monsters
            for (int i = 0; i < aliveMonsts.Count; i++)
            {
                //store update alive monster return value
                Weapon newMonstWeapon = aliveMonsts[i].Update(gameTime, player.GetPosition());

                //check if new monster weapon is not null
                if (newMonstWeapon != null)
                {
                    //add new monster weapon
                    monstWeapons.Add(newMonstWeapon);
                }

                //check if aliev monsters need to be removed
                if (aliveMonsts[i].GetRemoveStatus())
                {
                    //remove the alive monster
                    aliveMonsts.RemoveAt(i);
                }
            }
        }

        //pre: a valid gametime
        //post: none
        //description: udpates the hit markers
        private void UpdateHitMarkers(GameTime gameTime)
        {
            //loops through the hit marker timers
            for (int i = 0; i < hitmarkerTimers.Count; i++)
            {
                //updates the hit marker timer
                hitmarkerTimers[i].Update(gameTime.ElapsedGameTime.TotalMilliseconds);

                //check if hit marker timer at index i is finished
                if (hitmarkerTimers[i].IsFinished())
                {
                    //remove hit marker
                    hitmarkerTimers.RemoveAt(i);
                    hitMarkerRecs.RemoveAt(i);
                }
            }
        }

        //pre: a valid gametime
        //post: none
        //descriptin: updates the player
        private void UpdatePlayer(GameTime gameTime)
        {
            //store update player return value
            Weapon newPlayWeapon = player.UpdatePlayer(gameTime);

            //check if new weapon is not null
            if (newPlayWeapon != null)
            {
                //add the weapon to player weapon list
                playWeapons.Add(newPlayWeapon);
            }
        }

        //pre: a valid game time
        //post: none
        //descriptin: updates the player weapons
        private void UpdatePlayerWeapons(GameTime gameTime)
        {
            //updates each player weapon
            for (int i = 0; i < playWeapons.Count; i++)
            {
                //updates player weapons at index i
                playWeapons[i].Act(gameTime);
                playWeapons[i].CheckForRemove();
            }
        }

        //pre: a valid game time
        //post: none
        //descriptin: updates dead monsters
        private void UpdateDeadMonst(GameTime gameTime)
        {
            //updates each dead monster
            for (int i = 0; i < deadMonsts.Count; i++)
            {
                //stores update return value
                Weapon newMonstWeapon = deadMonsts[i].Update(gameTime, player.GetPosition());

                //checks if the new monster weapon is not null
                if (newMonstWeapon != null)
                {
                    //adds new monster weapons to list
                    monstWeapons.Add(newMonstWeapon);
                }

                //checks if the dead monsters at index i shoudl be removed
                if (deadMonsts[i].GetRemoveStatus())
                {
                    //remove the dead monsters at index i
                    deadMonsts.RemoveAt(i);
                }
            }
        }

        //pre: a valid game time
        //post: none
        //descriptin: updates the monster weapons
        private void UpdateMonstWeapons(GameTime gameTime)
        {
            //updates each monster weapon
            for (int i = 0; i < monstWeapons.Count; i++)
            {
                //acts the monster weapons at index i
                monstWeapons[i].Act(gameTime);

                //check if the monster weapon at index i should act
                if (monstWeapons[i].CheckForActStatus())
                {
                    //update the monster weapon based on the weapon type
                    switch (monstWeapons[i].GetWeaponType())
                    {
                        case Game1.TYPE_EXPLOAD:
                            //create instance of button click sound
                            creepExploadSnd.CreateInstance().Play();

                            //change the act status at weapon at index i
                            monstWeapons[i].ChangeActStatus();

                            //destroy the surrounding blocks of creeper explosion
                            level.DestroySurBlocks(monstWeapons[i].GetRect());

                            //checks if the player is within 100 blocks
                            if (Math.Sqrt(Math.Pow(monstWeapons[i].GetRect().Center.X - player.GetRect().Center.X, 2) + Math.Pow(monstWeapons[i].GetRect().Center.Y - player.GetRect().Center.Y, 2)) <= 100)
                            {
                                //create instance of button click sound
                                damageTakenSnd.CreateInstance().Play();

                                //removes 40 score from player
                                player.ChangeScore(CREEP_DAMAGE, levelNum);

                                //resizes game score background according to new scofre
                                gameScoreBackRec.Width = (int)pixelFont.MeasureString("Score: " + player.GetLevelScore(levelNum)).X + 10;
                            }
                            break;

                        case Game1.TYPE_FEAR:
                            //check if the weapon at index i should be removed
                            if (!monstWeapons[i].CheckForRemove())
                            {
                                //set player fear to true
                                player.SetFear(true);
                            }
                            else
                            {
                                //set player fear to false
                                player.SetFear(false);
                            }
                            break;

                        case Game1.TYPE_ARROW:
                            //check if the arrow hit the user
                            if (monstWeapons[i].GetRect().Intersects(player.GetRect()))
                            {
                                //create instance of button click sound
                                damageTakenSnd.CreateInstance().Play();

                                //removes skeleton damage from score
                                player.ChangeScore(SKEL_DAMAGE, levelNum);

                                //resizes game score background rectangle according to new score
                                gameScoreBackRec.Width = (int)pixelFont.MeasureString("Score: " + player.GetLevelScore(levelNum)).X + 10;

                                //resets the buffs
                                player.ResetBuffs();

                                //removes the monster weapon at index i
                                monstWeapons[i].SetToRemove();
                            }
                            break;
                    }
                }

                //check if the monster weapon at index i should be removed
                if (monstWeapons[i].CheckForRemove())
                {
                    //remove monster weapon at index i
                    monstWeapons.RemoveAt(i);
                }
            }
        }
    }
}