//Author: Dylan Nagel
//File Name: Creeper.cs
//Project Name: NagelD_PASS2
//Creation Date: Mar. 23 2023
//Modified Date: Apr. 16, 2023
//Description: Stores the creeper and information surrounding the creeper

using Microsoft.Xna.Framework;
using System;

namespace NagelD_PASS2
{
    public class Creeper : Monster
    {
        //create variable for x and y difference
        private float yDif;
        private float xDif;

        //holds if the creeper exploded yet
        private bool returnedExplode = false;

        //pre: a valid monster type number
        //post: none
        //description: create a creeper object
        public Creeper() : base(Game1.CREEPER)
        {
            //set speed variable
            speed = Game1.CREEPER_SPEED;

            //set position of creeper
            position.X = Game1.rng.Next(0, Game1.screenWidth - monsterRecs[Game1.ALIVE].Width);
            position.Y = -monsterRecs[Game1.ALIVE].Height;

            //set rectangle position to creeper position
            monsterRecs[Game1.ALIVE].X = (int)position.X;
            monsterRecs[Game1.ALIVE].Y = (int)position.Y;
        }

        //pre: a valid player position
        //post: none
        //description: moves the creeper
        protected override void Move(Vector2 playerPos)
        {
            //checks if the creeper is alive
            if(isAlive)
            {
                //calculate x and y difference
                xDif = playerPos.X - position.X;
                yDif = playerPos.Y - position.Y;

                //add to x and y position depending on location from player
                position.X += speed * xDif / (Math.Abs(xDif) + Math.Abs(yDif));
                position.Y += speed * yDif / (Math.Abs(xDif) + Math.Abs(yDif));
            }
        }

        //pre: a valid gametime
        //post: a weapon representing the creeper explosion
        //description: acts the creeper
        protected override Weapon Act(GameTime gameTime)
        {
            //check if the creeper is alive
            if (isAlive)
            {
                //check if the monster rectangle is greater than or equal to full height difference from bottom
                if (monsterRecs[Game1.ALIVE].Y >= Game1.screenHeight - monsterRecs[Game1.ALIVE].Height)
                {
                    //set isalive variable to false
                    isAlive = false;

                    //activates the dead timer
                    deadTimer.Activate();

                    //sets returned explode variable to true
                    returnedExplode = true;

                    //returns a new explosion
                    return new Explosion(position);
                }
            }
            else
            {
                //update the dead timer
                deadTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

                //check if the dead tiemr is finished
                if (deadTimer.IsFinished())
                {
                    //set is removed variabel to true
                    isRemoved = true;
                }

                //check if the creeper did not return an explosion yet
                if (!returnedExplode)
                {
                    //set return explosion to true
                    returnedExplode = true;

                    //return a new explosion
                    return new Explosion(position);
                }
            }

            //returns null
            return null;
        }
    }
}
