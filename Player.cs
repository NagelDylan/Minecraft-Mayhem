//Author: Dylan Nagel
//File Name: Player.cs
//Project Name: NagelD_PASS2
//Description: Stores the player entity and information surrounding the player

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helper;

namespace NagelD_PASS2
{
    public class Player
    {
        //stores buff and original values
        private readonly int[] PLAYER_SPEED = new int[] { 3, 6 };
        private readonly int[] SHOOT_TIMER_LENGTH = new int[] { 1000, 500 };
        private readonly int[] PLAYER_STRENGTH = new int[] { 1, 2 };
        private readonly int[] SCORE_MULTIPLIER = new int[] { 1, 2 };

        //stores player image and rectangle
        private Texture2D playerImg;
        private Rectangle playerRec;

        //store if the buff is enabled for each buff
        private bool[] isBuffEnabled = new bool[Game1.NUM_BUFFS];

        //stores the shoot tiemr
        private Timer[] shootTimer = new Timer[2];

        //stores the player stats
        int[] levScore = new int[Game1.NUM_LEVELS];
        int totScore = 0;
        int levKills = 0;
        int levShotsFired = 0;
        int levShotsHit = 0;
        int gameShotsFired = 0;
        int gameShotsHit = 0;
        //stores if the player is scared
        bool isScared;

        //pre: a valid player image
        //post: none
        //description: creates a plyer object
        public Player(Texture2D playerImg)
        {
            //set player image to perameter value
            this.playerImg = playerImg;

            //set player rec
            playerRec = new Rectangle((int)(0.5 * (Game1.screenWidth - Game1.ENTITY_SIZE)), Game1.screenHeight - Game1.ENTITY_SIZE, Game1.ENTITY_SIZE, Game1.ENTITY_SIZE);

            //sets shoot timer variables
            for(int i = 0; i < shootTimer.Length; i++)
            {
                //sets shoot tiemr variable at index i
                shootTimer[i] = new Timer(SHOOT_TIMER_LENGTH[i] / 3, true);
            }
        }

        //pre: a valid gametime
        //post: none
        //description: updates the player
        public Weapon UpdatePlayer(GameTime gameTime)
        {
            //checks if the player is not scared
            if(!isScared)
            {
                //updates the shoot timer
                shootTimer[Convert.ToInt32(isBuffEnabled[Game1.SHOOT_BUFF])].Update(gameTime.ElapsedGameTime.TotalMilliseconds);

                //check if the user is clicking the left arrow
                if (Game1.kb.IsKeyDown(Keys.Left))
                {
                    //move player based on speed
                    playerRec.X -= PLAYER_SPEED[Convert.ToInt32(isBuffEnabled[Game1.SPEED_BUFF])];
                }

                //check if the user is clickign right arrow
                if (Game1.kb.IsKeyDown(Keys.Right))
                {
                    //move player based on speed
                    playerRec.X += PLAYER_SPEED[Convert.ToInt32(isBuffEnabled[Game1.SPEED_BUFF])];
                }

                //check if the player is greater than the screen width
                if(playerRec.Right > Game1.screenWidth)
                {
                    //set player x position to right side of the screen
                    playerRec.X = Game1.screenWidth - playerRec.Width;
                }
                else if(playerRec.X < 0)
                {
                    //set player x position to left side of screen
                    playerRec.X = 0;
                }

                //check if user is clicking the space button
                if (Game1.kb.IsKeyDown(Keys.Space))
                {
                    //check if the shoot timer is finished
                    if(shootTimer[Convert.ToInt32(isBuffEnabled[Game1.SHOOT_BUFF])].IsFinished())
                    {
                        //reset the shoot timer
                        shootTimer[Convert.ToInt32(isBuffEnabled[Game1.SHOOT_BUFF])].ResetTimer(true);

                        //add to stats
                        levShotsFired++;
                        gameShotsFired++;

                        //return a new arrow
                        return new Arrow(-1, GetPosition());
                    }
                }
            }

            //return null
            return null;
        }

        //pre: none
        //post: none
        //description: draws the player
        public void DrawPlayer()
        {
            //draws the player
            Game1.spriteBatch.Draw(playerImg, playerRec, Color.White);
        }

        //pre: none
        //post: a vector2 representing the player position
        //description: returns tplayer position
        public Vector2 GetPosition()
        {
            //return player position
            return new Vector2(playerRec.X, playerRec.Y);
        }

        //pre: none
        //post: an int representing the player damage
        //description: returns the player damage
        public int GetDamage()
        {
            //returns the player damage
            return PLAYER_STRENGTH[Convert.ToInt32(isBuffEnabled[Game1.STRENGTH_BUFF])];
        }

        //pre: none
        //post: a rectangle representing the player rectangle
        //description: returns teh player rectangle
        public Rectangle GetRect()
        {
            //returns the player rectangle
            return playerRec;
        }

        //pre: a valid int representing the score change, a valid int representing the level number
        //post: none
        //description: changes the scrore
        public void ChangeScore(int scoreChange, int levelNum)
        {
            //changes the score based on level number and score change
            levScore[levelNum] += scoreChange;

            //checks if the score is below 0
            if(levScore[levelNum] < 0)
            {
                //set score to zero
                levScore[levelNum] = 0;
            }
        }

        //pre: none
        //post: a valid int representing the score multiplier
        //description: return teh score multiplier
        public int GetScoreMult()
        {
            //returns teh score multiplier
            return SCORE_MULTIPLIER[Convert.ToInt32(isBuffEnabled[Game1.SCORE_BUFF])];
        }

        //pre: a valid buff number
        //post: a bool representing if that buff is enabled
        //description: returns the buff status
        public bool GetBuffStatus(int buffNum)
        {
            //returns the buff status at buff number
            return isBuffEnabled[buffNum];
        }

        //pre: a valid int representing the level number
        //post: none
        //description: adds to the total score
        public void SetTotalScore(int levelNum)
        {
            //adds level score to total score
            totScore += levScore[levelNum];
        }

        //pre:
        //post:
        //description: 
        public int GetTotalScore()
        {
            return totScore;
        }

        //pre: a valid int representing the level number
        //post: an int representing the level score
        //description: returnst the level score at level number
        public int GetLevelScore(int levelNum)
        {
            //returns teh level score
            return levScore[levelNum];
        }

        //pre: none
        //post: none
        //description: adds to kill statistics
        public void AddKill()
        {
            //adds one to level kills
            levKills++;
        }

        //pre: none
        //post: none
        //description: adds to shots fired statistics
        public void AddShotsHit()
        {
            //adds one shot fired to stats
            levShotsHit++;
            gameShotsHit++;
        }

        //pre: none
        //post: an int representinf the number of kills during level
        //description: returns the number of kills in level
        public int GetLevKills()
        {
            //returns the number of kills in level
            return levKills;
        }

        //pre: none
        //post: an int representing the number of shots fired
        //description: returns the number of shots fired
        public int GetLevShotsFired()
        {
            //return the number of shots fired
            return levShotsFired;
        }

        //pre: none
        //post: an int represening the number of shots hit during level
        //description: returns the number of shots hit in level
        public int GetLevHits()
        {
            //returns the number of shots hit
            return levShotsHit;
        }

        //pre: none
        //post: a float representing the level's hit percentage
        //description: returns the level hit percentage
        public float GetLevHitPerc()
        {
            //checks if teh shots fired does not eqal zero
            if(levShotsFired != 0)
            {
                //returns the hit percentage
                return (float)Math.Round(levShotsHit / (double)levShotsFired * 100, 2);
            }

            //returns zero
            return 0;
        }

        //pre: a valid buff number
        //post: none
        //description: adds a new buff
        public void AddBuff(int buffNum)
        {
            //set buff at buff number to true
            isBuffEnabled[buffNum] = true;
            
            //removes score amount from total score
            totScore -= Game1.BUFF_PRICES[buffNum];
        }

        //pre: none
        //post: none
        //description: rests the buffs
        public void ResetBuffs()
        {
            //resets each buff to false
            for(int i = 0; i < Game1.NUM_BUFFS; i++)
            {
                //resets buff at index i to false
                isBuffEnabled[i] = false;
            }
        }

        //pre: none
        //post: none
        //description: resets the player
        public void Reset()
        {
            //rests player x position
            playerRec.X = (int)(0.5 * (Game1.screenWidth - playerRec.Width));

            //resets the level stats
            levShotsFired = 0;
            levShotsHit = 0;
        }

        //pre: a valid bool representing if the player is scared
        //post: none
        //description: sets the fear status of player
        public void SetFear(bool isScared)
        {
            //sets player scared bool to peramter value
            this.isScared = isScared;
        }

        //pre: none
        //post: a bool representing the player is scared
        //description: returns if the player is scared
        public bool GetFear()
        {
            //returns if the player is scared
            return isScared; 
        }

        //pre: none
        //post: a float representing the game hit percentage
        //description: returns the game hit percentage
        public float GetGameHitPerc()
        {
            //checks if the game shots fired does not equal to zero
            if (gameShotsFired != 0)
            {
                //returns teh game hit percentage
                return (float)Math.Round(gameShotsHit / (double)gameShotsFired * 100, 2);
            }

            //returns zero
            return 0;
        }

        //pre: none
        //post: none
        //description: resets the game statistics
        public void ResetGameStats()
        {
            //rests the game statistics to zero
            gameShotsHit = 0;
            gameShotsFired = 0;
        }

        //pre: none
        //post: none
        //description: completely resets the player values
        public void CompleteReset()
        {
            //resets the buffs
            ResetBuffs();

            //resets the total score
            totScore = 0;

            //resets each level score
            for(int i = 0; i < levScore.Length; i++)
            {
                //resets level score at index i
                levScore[i] = 0;
            }

            //resets the level kills
            levKills = 0;
        }
    }
}
