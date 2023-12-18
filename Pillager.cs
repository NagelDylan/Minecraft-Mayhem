//Author: Dylan Nagel
//File Name: Pillager.cs
//Project Name: NagelD_PASS2
//Description: Stores the pillager and information surrounding the pillager

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NagelD_PASS2
{
    public class Pillager : Monster
    {
        //create variables to store shield image
        private Texture2D shieldImg;

        //create rectangle to store shield rectangle
        private Rectangle shieldRec = new Rectangle(-100, -100, Game1.PILL_SHIELD_WIDTH, Game1.PILL_SHIELD_HEIGHT);

        //create variabel to store baseline
        private int baseline;

        //variable to store if shield is on
        private bool isShieldOn = true;

        //pre: a valid interger representing the monster type number
        //post: none
        //description: creates a pillager object
        public Pillager(): base(Game1.PILLAGER)
        {
            //sets shield image variable
            shieldImg = Game1.SHIELD_IMG;

            //set baseline to a random y position
            baseline = Game1.rng.Next(Game1.PILL_SIN_AMP, Game1.screenHeight - Game1.PILL_SIN_AMP - monsterRecs[Game1.ALIVE].Height * 2);

            //set y position to baseline
            position.Y = baseline;

            //set speed equal to pillager speed
            speed = Game1.PILL_SPEED;

            //check if random number is less than 50
            if (Game1.rng.Next(0, 100) < 50)
            {
                //set x position to left side of screen
                position.X = -monsterRecs[Game1.ALIVE].Width;
            }
            else
            {
                //set x position to right side of screen
                position.X = Game1.screenWidth;

                //set speed to negative
                speed = speed * -1;
            }

            //set monster rectangle locations to locations
            monsterRecs[Game1.ALIVE].X = (int)position.X;
            monsterRecs[Game1.ALIVE].Y = (int)position.Y;
        }

        //pre: a valid int of player damage
        //post: none
        //description: removes health points from monster
        public override bool RemoveHp(int playerDamage)
        {
            //check if the shield is on
            if(isShieldOn)
            {
                //set is shield on variable to false
                isShieldOn = false;
            }
            else
            {
                //remove from health points
                hp -= playerDamage;

                //check if the health is below zero
                if (hp <= 0)
                {
                    //sets is alive variable to false
                    isAlive = false;

                    //activate the daed timer
                    deadTimer.Activate();

                    //set the dead monster rectangle location
                    monsterRecs[Game1.DEAD].X = (int)(monsterRecs[Game1.ALIVE].X + 0.5 * (monsterRecs[Game1.ALIVE].Width - monsterRecs[Game1.DEAD].Width));
                    monsterRecs[Game1.DEAD].Y = (int)(monsterRecs[Game1.ALIVE].Y + 0.5 * (monsterRecs[Game1.ALIVE].Height - monsterRecs[Game1.DEAD].Height));
                }
            }

            //returns if the monster is alive
            return isAlive;
        }

        //pre: a valid vector2 player position
        //post: none
        //description: moves the pillager
        protected override void Move(Vector2 playerPos)
        {
            //change x and y position depending on current variables
            position.X += (int)speed;
            position.Y = baseline + (int)(Game1.PILL_SIN_AMP * Math.Sin(0.03 * position.X));

            //check if position is on left or right side of screen
            if (position.X <= -monsterRecs[Game1.ALIVE].Width || position.X >= Game1.screenWidth)
            {
                //set is alive variable to false
                isRemoved = true;
            }

            //check if the shield is on
            if(isShieldOn)
            {
                //change x and y position of shield
                shieldRec.X = (int)(position.X + monsterRecs[Game1.ALIVE].Width - shieldRec.Width);
                shieldRec.Y = (int)(position.Y + monsterRecs[Game1.ALIVE].Height - shieldRec.Height * 0.5);
            }
        }

        //pre: none
        //post: none
        //description: draws pillager
        public override void Draw()
        {
            //draws pillager
            Game1.spriteBatch.Draw(monsterImgs[Convert.ToInt32(isAlive)], monsterRecs[Game1.ALIVE], Color.White);

            //check if the pillager is alive
            if (isAlive)
            {
                //checks if helath pionts is equal to pillager max healh pints
                if (isShieldOn)
                {
                    //draw each shielded heart for pillager
                    for (int i = 0; i < GameObject.MONST_HEALTH[Game1.PILLAGER]; i++)
                    {
                        //draw shielded heart at index i
                        Game1.spriteBatch.Draw(Game1.PILL_HEART_IMG, heartRecs[i], Color.White);
                    }

                    //draws pillager shield
                    Game1.spriteBatch.Draw(shieldImg, shieldRec, Color.White);
                }
                else
                {
                    //draw each full heart
                    for (int i = 0; i < hp; i++)
                    {
                        //draw heart at index i
                        Game1.spriteBatch.Draw(Game1.HEART_IMGS[Game1.FULL], heartRecs[i], Color.White);
                    }

                    //draw each empty heart
                    for (int i = hp; i < GameObject.MONST_HEALTH[Game1.PILLAGER]; i++)
                    {
                        //draw heart at index i
                        Game1.spriteBatch.Draw(Game1.HEART_IMGS[Game1.EMPTY], heartRecs[i], Color.White);
                    }
                }
            }
            
        }
    }
}
