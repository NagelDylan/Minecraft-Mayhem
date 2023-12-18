//Author: Dylan Nagel
//File Name: Enderman.cs
//Project Name: NagelD_PASS2
//Creation Date: Mar. 23 2023
//Modified Date: Apr. 16, 2023
//Description: Stores the enderman and information surrounding the enderman

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Helper;

namespace NagelD_PASS2
{
    public class Enderman : Monster
    {
        //create variables to store timers
        private Timer stopTime;

        //stores random number
        private int randNum;

        //stores ismoving variable
        private bool isMoving = true;

        //stores a list of locations
        private List<Vector2> locs = new List<Vector2>();

        //stores if enderman should return fear
        private bool shouldReturnFear = true;

        //pre: valid texture for monster image
        //post: none
        //description: creates enderman object
        public Enderman() : base(Game1.ENDERMAN)
        {
            //sets position of enderman
            position.X = Game1.rng.Next(0, Game1.screenWidth - monsterRecs[Game1.ALIVE].Width);
            position.Y = Game1.TOP_SCREEN_SPAWN;

            //sets monster rec position to position
            monsterRecs[Game1.ALIVE].X = (int)position.X;
            monsterRecs[Game1.ALIVE].Y = (int)position.Y;

            //sets timer values
            stopTime = new Timer(Game1.END_TEL_TIMER, true);

            //adds locations to location list
            locs.Add(Game1.ENDER_LOC1);
            locs.Add(Game1.ENDER_LOC2);
            locs.Add(Game1.ENDER_LOC3);
        }

        //pre: a valid vector2 player location
        //post: none
        //description: move enderman
        protected override void Move(Vector2 playerLoc)
        {
            if(isAlive)
            {
                //check if stop timer is finished
                if (stopTime.IsFinished())
                {
                    //reset stop timer
                    stopTime.ResetTimer(true);

                    //sets if the enderman should return fear to true
                    shouldReturnFear = true;

                    //check if enderman is moving
                    if (isMoving)
                    {

                        //move enderman depending on the number of locations left
                        switch (locs.Count)
                        {
                            case 3:
                                //create a random number
                                randNum = Game1.rng.Next(0, 21);

                                //set position based on random number created
                                position.X = locs[randNum % 3].X;
                                position.Y = locs[randNum % 3].Y;

                                //remove location from list
                                locs.RemoveAt(randNum % 3);
                                break;
                            case 2:
                                //create a random number
                                randNum = Game1.rng.Next(0, 14);

                                //set position based on random number created
                                position.X = locs[randNum % 2].X;
                                position.Y = locs[randNum % 2].Y;

                                //remove location from list
                                locs.RemoveAt(randNum % 2);
                                break;

                            case 1:
                                //set position to location at zero
                                position.X = locs[0].X;
                                position.Y = locs[0].Y;

                                //remove locs at index 0
                                locs.RemoveAt(0);
                                break;

                            case 0:
                                //set position to just above player
                                position.X = playerLoc.X;
                                position.Y = playerLoc.Y - monsterRecs[Game1.ALIVE].Height - 10;

                                //set is moving variable to false
                                isMoving = false;
                                break;
                        }
                    }
                    else
                    {
                        //set is removed variable to false
                        isRemoved = true;
                    }
                }
            }
            
        }

        //pre: a valid gametime
        //post: a weapon representing the enderman fear
        //description: act the enderman
        protected override Weapon Act(GameTime gameTime)
        {
            //check if the enderman is alive
            if (isAlive)
            {
                //update stop timer and fear timer
                stopTime.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

                //check if the enderman should return fear
                if (shouldReturnFear)
                {
                    //set should return fear to false
                    shouldReturnFear = false;

                    //create angry enderman sound effect
                    Game1.ENDER_ANGRY_SND.CreateInstance().Play();

                    //return a new fear
                    return new Fear();
                }
            }
            else
            {
                //update the dead timer
                deadTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

                //check if the dead timer is finished
                if (deadTimer.IsFinished())
                {
                    //set is removed variable to true
                    isRemoved = true;
                }
            }

            //returns null
            return null;
        }
    }
}
