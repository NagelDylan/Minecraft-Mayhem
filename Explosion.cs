//Author: Dylan Nagel
//File Name: Explosion.cs
//Project Name: NagelD_PASS2
//Description: Stores the explostion and information surrounding the explostion

using Microsoft.Xna.Framework;
using Helper;

namespace NagelD_PASS2
{
    public class Explosion: Weapon
    {
        //stores the alive timer
        private Timer aliveTime;

        //stores the is exploded boolean
        private bool isExploaded = false;

        //pre: a valid vector2 position, a valid interger explosion type
        //post: none
        //description: create an explostion object
        public Explosion(Vector2 position) : base(Game1.TYPE_EXPLOAD)
        {
            //sets weapon rectangle
            weaponRec = new Rectangle((int)(position.X + 0.5 * (Game1.ENTITY_SIZE - Game1.CREEP_EXP_LENGTH)), (int)(position.Y + 0.5 * (Game1.ENTITY_SIZE - Game1.CREEP_EXP_LENGTH)), Game1.CREEP_EXP_LENGTH, Game1.CREEP_EXP_LENGTH);

            //sets alive timer
            aliveTime = new Timer(500, true);

            //sets weapon image
            weaponImg = Game1.EXPLOAD_IMG;

            //sets should act to true
            shouldAct = true;
        }

        //pre: a valid game time
        //post: a bool representing if the explosion is already exploded
        //description: acts the explosion
        public override bool Act(GameTime gameTime)
        {
            //updates the alive timer
            aliveTime.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

            //checks if the alive tiemr is finished
            if (aliveTime.IsFinished())
            {
                //sets should remove variable to true
                shouldRemove = true;
            }

            //returns if the explosion is exploded
            return isExploaded;
        }
    }
}
