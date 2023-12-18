//Author: Dylan Nagel
//File Name: Fear.cs
//Project Name: NagelD_PASS2
//Description: Stores the fear and information surrounding the fear

using Microsoft.Xna.Framework;
using Helper;

namespace NagelD_PASS2
{
    public class Fear:Weapon
    {
        //stores the fear timer
        private Timer fearTime;

        //pre: a valid interger weapon type
        //post: none
        //description: creates a fear object
        public Fear() : base(Game1.TYPE_FEAR)
        {
            //sets weapon image and rectangle
            weaponImg = Game1.ENDER_FEAR_IMG;
            weaponRec = new Rectangle(0, (int)(0.5 * (Game1.screenHeight - Game1.FEAR_IMG_HEIGHT)), Game1.screenWidth, Game1.FEAR_IMG_HEIGHT);

            //sets fear time timer
            fearTime = new Timer(500, true);

            //sets should act to true
            shouldAct = true;
        }

        //pre: none
        //post: none
        //description: draws the fear
        public override void Draw()
        {
            //draws weapon
            Game1.spriteBatch.Draw(weaponImg, weaponRec, Color.White * 0.5f);
        }

        //pre: a valid gametime
        //post: a bool representing if the weapon should act
        //description: act the fear weapon
        public override bool Act(GameTime gametime)
        {
            //update the fear timer
            fearTime.Update(gametime.ElapsedGameTime.TotalMilliseconds);

            //check if the fear timer is finished
            if(fearTime.IsFinished())
            {
                //set should remove variable to true
                shouldRemove = true;
            }

            //return true
            return true;
        }
    }
}
