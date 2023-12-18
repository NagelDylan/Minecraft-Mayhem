//Author: Dylan Nagel
//File Name: Villager.cs
//Project Name: NagelD_PASS2
//Description: Stores the villager and information surrounding the villager

using Microsoft.Xna.Framework;

namespace NagelD_PASS2
{
    public class Villager : Monster
    {
        //pre: a valid monster type
        //post: none
        //description: creats a villager object
        public Villager():base(Game1.VILLAGER)
        {
            //set speed variable to villager speed
            speed = Game1.VILL_SPEED;

            //set y position of villager
            position.Y = Game1.rng.Next(Game1.TOP_SCREEN_SPAWN, Game1.screenHeight - monsterRecs[Game1.ALIVE].Height * 2);
            monsterRecs[Game1.ALIVE].Y = (int)position.Y;

            //check if random variable is less than 50
            if (Game1.rng.Next(0, 100) < 50)
            {
                //set villager x location to the left of screen
                position.X = -monsterRecs[Game1.ALIVE].Width;
            }
            else
            {
                //set villager x location to right side
                position.X = Game1.screenWidth;

                //set speed to negative speed
                speed = speed * -1;
            }

            //set x position of monster rec
            monsterRecs[Game1.ALIVE].X = (int)position.X;
        }

        //pre: a valid player location
        //post: none
        //description: move villager
        protected override void Move(Vector2 playerLoc)
        {
            if(isAlive)
            {
                //move villager with speed
                position.X += speed;

                //check if speed is greater than 0
                if (speed > 0)
                {
                    //check if monster rec is on the right of the scren
                    if (monsterRecs[Game1.ALIVE].X >= Game1.screenWidth)
                    {
                        //set is alive variable to false
                        isRemoved = true;
                    }
                }
                else
                {
                    //check if monster rec is on the left of screen
                    if (monsterRecs[Game1.ALIVE].X <= -monsterRecs[Game1.ALIVE].Width)
                    {
                        //set is alive variable to false
                        isRemoved = true;
                    }
                }
            }
        }
    }
}
