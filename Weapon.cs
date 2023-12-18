//Author: Dylan Nagel
//File Name: Weapon.cs
//Project Name: NagelD_PASS2
//Creation Date: Mar. 23 2023
//Modified Date: Apr. 16, 2023
//Description: Stores the weapon and information surrounding the weapon

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NagelD_PASS2
{
    public class Weapon
    {
        //stores the weapon image and rectangle
        private protected Rectangle weaponRec;
        private protected Texture2D weaponImg;

        //stores booleans for should be removed and should act
        private protected bool shouldRemove;
        private protected bool shouldAct;

        //stores the weapon type
        private int weaponType;

        //pre: a valid weapon type
        //post: none
        //description: creates a weapon object
        public Weapon(int weaponType)
        {
            //sets weapon type to peramter value
            this.weaponType = weaponType;
        }

        //pre: a valid gametime
        //post: a valid bool representing if the weapon should act
        //description: acts the weapon
        public virtual bool Act(GameTime gameTime)
        {
            //returns if the weapons should act
            return shouldAct;
        }

        //pre: none
        //post: none
        //description: draws the weapon
        public virtual void Draw()
        {
            //draws weapon
            Game1.spriteBatch.Draw(weaponImg, weaponRec, Color.White);
        }

        //pre: none
        //post: a rectangle represnting the weapon rectangle
        //description: returns the weapon rectangle
        public Rectangle GetRect()
        {
            //returns the weapon rectangle
            return weaponRec;
        }

        //pre: none
        //post: a bool represnting if the weapon should be removed
        //description: returns if the weapon should be removed
        public virtual bool CheckForRemove()
        {
            return shouldRemove;
        }

        //pre: none
        //post: none
        //description: changes the act status
        public void ChangeActStatus()
        {
            //set should act variable to its opposite value
            shouldAct = !shouldAct;
        }

        //pre: none
        //post: a bool representing if the monster should act
        //description: returns if the mosnter should act
        public bool CheckForActStatus()
        {
            //returns should act
            return shouldAct;
        }

        //pre: none
        //post: a int representing the weapon type
        //description: returns the weapon type
        public int GetWeaponType()
        {
            //returns the weapon type
            return weaponType;
        }

        //pre: none
        //post: none
        //description: sets the weapon to remove
        public void SetToRemove()
        {
            //sets should remove variable to true
            shouldRemove = true;
        }
    }
}
