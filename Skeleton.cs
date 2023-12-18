//Author: Dylan Nagel
//File Name: Skeleton.cs
//Project Name: NagelD_PASS2
//Creation Date: Mar. 23 2023
//Modified Date: Apr. 16, 2023
//Description: Stores the skeleton and information surrounding the skeleton

using System;
using Microsoft.Xna.Framework;
using Helper;

namespace NagelD_PASS2
{
    public class Skeleton : Monster
    {
        //stores angle
        private double angle = Math.PI * 0.5;

        //stores shoot timer
        private Timer shootTimer;

        //stores radius
        private double radius;

        //stores the current movestata
        private int moveState = Game1.SKEL_PHASE_1;

        //pre: a valid monster type number
        //post: none
        //description: creates a skeleton object
        public Skeleton() : base(Game1.SKELETON)
        {
            //create new timer
            shootTimer = new Timer(Game1.SKEL_SHOOT_TIME, true);

            //set speed of skeleton
            speed = Game1.SKEL_SPEED;

            //set monster rectangle location of monster rectangle
            monsterRecs[Game1.ALIVE].X = Game1.screenWidth - monsterRecs[Game1.ALIVE].Width;
            monsterRecs[Game1.ALIVE].Y = -monsterRecs[Game1.ALIVE].Height;

            //set monster location to monster rectangle location
            position.X = monsterRecs[Game1.ALIVE].X;
            position.Y = monsterRecs[Game1.ALIVE].Y;

            //sets radius of skeleton movement
            radius = Game1.screenWidth * 0.5 - monsterRecs[Game1.ALIVE].Width * 0.5;
        }

        //pre: a valid vector2 player location
        //post: none
        //description: move skeleton
        protected override void Move(Vector2 playerLoc)
        {
            //check if the skeleton is alive
            if(isAlive)
            {
                //move skeleton based on move state
                switch (moveState)
                {
                    case Game1.SKEL_PHASE_1:
                        //add to the y location of skeleton
                        position.Y += speed;

                        //check if the y position is greater than half the screen
                        if (position.Y > Game1.screenHeight * 0.5 - monsterRecs[Game1.ALIVE].Height * 0.5 - monsterRecs[Game1.ALIVE].Height)
                        {
                            //set y position to half the screen
                            position.Y = Game1.screenHeight * 0.5f - monsterRecs[Game1.ALIVE].Height * 0.5f - monsterRecs[Game1.ALIVE].Height;

                            //set move state to phase 2
                            moveState = Game1.SKEL_PHASE_2;
                        }
                        break;

                    case Game1.SKEL_PHASE_2:
                        //change angle and radius values
                        angle -= Game1.SKEL_ANGLE_RATE_CHANGE;
                        radius -= Game1.SKEL_RADIUS_RATE_CHANGE;

                        //check if radius is negative
                        if (radius <= 0)
                        {
                            //set movestate to phase 3
                            moveState = Game1.SKEL_PHASE_3;
                        }

                        //set position depending on radius and angle of circle
                        position.X = Game1.screenWidth * 0.5f - monsterRecs[Game1.ALIVE].Width * 0.5f + (float)(radius * Math.Sin(angle));
                        position.Y = Game1.screenHeight * 0.5f - monsterRecs[Game1.ALIVE].Height * 0.5f - monsterRecs[Game1.ALIVE].Height + (float)(radius * Math.Cos(angle));
                        break;

                    case Game1.SKEL_PHASE_3:
                        //add to the x position of the skeleton based on its speed
                        position.X += speed;

                        //check if skeleton exceeded screen limit
                        if (position.X > Game1.screenWidth)
                        {
                            //set skeleton live status to false
                            isRemoved = true;
                        }
                        break;
                }
            }
        }

        //pre: a valid game time
        //post: a weapon representing the skeleton arrow
        //description: act the skeleton
        protected override Weapon Act(GameTime gameTime)
        {
            //check if the skeleton is alive
            if (isAlive)
            {
                //update the shoot timer
                shootTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

                //check if the shoot timer is finished
                if (shootTimer.IsFinished())
                {
                    //reset the shoot timer
                    shootTimer.ResetTimer(true);

                    //returns a new arrow
                    return new Arrow(1, position);
                }
            }
            else
            {
                //updates the dead timer
                deadTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

                //checks if the dead tiemr is finished
                if(deadTimer.IsFinished())
                {
                    //sets is removed variabel to true
                    isRemoved = true;
                }
            }

            //returns null
            return null;
        }
    }
}
