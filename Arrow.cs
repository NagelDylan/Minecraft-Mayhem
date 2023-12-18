//Author: Dylan Nagel
//File Name: Arrow.cs
//Project Name: NagelD_PASS2
//Creation Date: Mar. 23 2023
//Modified Date: Apr. 16, 2023
//Description: Stores the arrow and information surrounding the arrow
using Microsoft.Xna.Framework;

namespace NagelD_PASS2
{
    public class Arrow: Weapon
    {
        //stores the speed of the arrow
        private int speed;

        //pre: a valid int direction for arrow, a valid vector2 shooter position for arrow, a valid int weapon type
        //post: none
        //description: creates arrow object
        public Arrow(int direction, Vector2 shooterPos):base(Game1.TYPE_ARROW)
        {
            //sets weapon rectangle
            weaponRec = new Rectangle((int)(shooterPos.X + 0.5 * (Game1.ENTITY_SIZE - Game1.ARROW_WIDTH)), (int)shooterPos.Y, Game1.ARROW_WIDTH, Game1.ARROW_HEIGHT);

            //checks if the direction is negative one
            if (direction == -1)
            {
                //sets weapon image to upward facing
                weaponImg = Game1.ARROW_IMGS[Game1.UP];
            }
            else
            {
                //sets weapon image to downward facing
                weaponImg = Game1.ARROW_IMGS[Game1.DOWN];
            }

            //sets speed to arrow speed times direction
            speed = Game1.ARROW_SPEED * direction;

            //sets should act to true
            shouldAct = true;
        }

        //pre: a valid game time
        //post: a bool representing if the arrow should act
        //description: acts the arrow
        public override bool Act(GameTime gameTime)
        {
            //move arrow y direction based on speed
            weaponRec.Y += speed;

            //checks if the arrow exceeds either side of the screen
            if(weaponRec.Y < -weaponRec.Height || weaponRec.Y > Game1.screenHeight)
            {
                //sets should remove to true
                shouldRemove = true;
            }

            //returns false
            return false;
        }
    }
}
