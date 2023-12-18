//Author: Dylan Nagel
//File Name: Monster.cs
//Project Name: NagelD_PASS2
//Description: Stores the monster and information surrounding the monster

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;

namespace NagelD_PASS2
{
    public class Monster
    {
        //stores the speed and position
        private protected float speed;
        private protected Vector2 position;

        //stores the visual informatino
        private protected Rectangle[] monsterRecs = new Rectangle[2];
        private protected Texture2D[] monsterImgs = new Texture2D[2];

        //stores is removed and is alive booleans
        private protected bool isRemoved = false;
        private protected bool isAlive = true;

        //stores dead timer
        private protected Timer deadTimer;

        //stores kill score
        private protected int killScore;

        //stores the health points information
        private protected Rectangle[] heartRecs;
        private protected int hp;

        //stores the monster type
        private int monsterType;

        //pre: A valid int for monster type between 0 and 4
        //post: none
        //description: creates a monster object
        public Monster(int monsterType)
        {
            //sets monster type to perameter value
            this.monsterType = monsterType;

            //sets monster images
            monsterImgs[Game1.DEAD] = Game1.MONSTER_IMGS[Game1.DEAD, monsterType];
            monsterImgs[Game1.ALIVE] = Game1.MONSTER_IMGS[Game1.ALIVE, monsterType];

            //sets monster rectangles
            monsterRecs[Game1.ALIVE] = new Rectangle(-Game1.ENTITY_SIZE, -Game1.ENTITY_SIZE, Game1.ENTITY_SIZE, Game1.ENTITY_SIZE);
            monsterRecs[Game1.DEAD] = monsterRecs[Game1.ALIVE];

            //sets dead timer
            deadTimer = new Timer(Game1.DEATH_TIME, false);

            //sets the kill score
            killScore = GameObject.KILL_SCORES[monsterType];

            //sets health points information
            hp = GameObject.MONST_HEALTH[monsterType];
            heartRecs = new Rectangle[hp];

            //sets rectangle for each health point
            for(int i = 0; i < heartRecs.Length; i++)
            {
                //sets heart rectangle at index i
                heartRecs[i] = new Rectangle((int)(monsterRecs[Game1.ALIVE].Center.X - Game1.HEART_SIZE * GameObject.MONST_HEALTH[monsterType] * 0.5) + Game1.HEART_SIZE * i, monsterRecs[Game1.ALIVE].Top - Game1.HEART_MONST_SPACE - Game1.HEART_SIZE, Game1.HEART_SIZE, Game1.HEART_SIZE);
            }
        }

        //pre: none
        //post: none
        //description: draws the monster
        public virtual void Draw()
        {
            //draws the monster image
            Game1.spriteBatch.Draw(monsterImgs[Convert.ToInt32(isAlive)], monsterRecs[Convert.ToInt32(isAlive)], Color.White);

            //check if teh monster is alive
            if(isAlive)
            {
                //draw each heart the monster still has
                for (int i = 0; i < hp; i++)
                {
                    //draws heart at index i
                    Game1.spriteBatch.Draw(Game1.HEART_IMGS[Game1.FULL], heartRecs[i], Color.White);
                }

                //draws each heart the monster does not have
                for (int i = hp; i < GameObject.MONST_HEALTH[monsterType]; i++)
                {
                    //draws heart at index i
                    Game1.spriteBatch.Draw(Game1.HEART_IMGS[Game1.EMPTY], heartRecs[i], Color.White);
                }
            }
        }

        //pre: a valid vector2 player location
        //post: none
        //description: moves the monster
        protected virtual void Move(Vector2 playerLoc)
        {

        }

        //pre: a valid game time
        //post: a weapon representing the monsters weapon
        //description: does the monster's actions and returns new weapons
        protected virtual Weapon Act(GameTime gameTime)
        {
            if(!isAlive)
            {
                //update the dead timer
                deadTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

                //check if the dead timer is finished
                if (deadTimer.IsFinished())
                {
                    //set if the monster is removed to true
                    isRemoved = true;
                }
            }

            //return null
            return null;
        }

        //pre: a valid gametime, a valid vector2 player loc
        //post: none
        //description: updates the monster
        public Weapon Update(GameTime gameTime, Vector2 playerLoc)
        {

            //moves the monster
            Move(playerLoc);

            //sets monster rectangle position to its edited position
            monsterRecs[Convert.ToInt32(isAlive)].X = (int)position.X;
            monsterRecs[Convert.ToInt32(isAlive)].Y = (int)position.Y;

            //sets each heart rectangle position
            for (int i = 0; i < heartRecs.Length; i++)
            {
                //sets x and y position of heart at index i
                heartRecs[i].X = (int)(monsterRecs[Game1.ALIVE].Center.X - Game1.HEART_SIZE * GameObject.MONST_HEALTH[monsterType] * 0.5) + Game1.HEART_SIZE * i;
                heartRecs[i].Y = monsterRecs[Game1.ALIVE].Top - Game1.HEART_MONST_SPACE - Game1.HEART_SIZE;
            }

            //acts the monster
            return Act(gameTime);
        }

        //pre: none
        //post: a rectangle representing the monster rectangle
        //description: returns the monster rectangle
        public virtual Rectangle GetRect()
        {
            //returns the monster rectangle
            return monsterRecs[Convert.ToInt32(Game1.ALIVE)];
        }

        //pre: a valid interger representing the damage done by the player
        //post: none
        //desciption: removes hp from monster
        public virtual bool RemoveHp(int playerDamage)
        {
            //removes damage from hp
            hp -= playerDamage;

            //checks if hp is less than or equal to zero
            if (hp <= 0)
            {
                //sets is alive variable to false
                isAlive = false;

                //activates the dead timer
                deadTimer.Activate();

                //sets the x and y position of the monster rectangle to above the monster
                monsterRecs[Game1.DEAD].X = (int)(monsterRecs[Game1.ALIVE].X + 0.5 * (monsterRecs[Game1.ALIVE].Width - monsterRecs[Game1.DEAD].Width));
                monsterRecs[Game1.DEAD].Y = (int)(monsterRecs[Game1.ALIVE].Y + 0.5 * (monsterRecs[Game1.ALIVE].Height - monsterRecs[Game1.DEAD].Height));
            }

            //returns if the mosnter is alive
            return isAlive;
        }


        //pre: none
        //post: return vector position of monster location
        //description: gets the location of monster
        public Vector2 GetLocation()
        {
            //rerturns monster position
            return position;
        }

        //pre: none
        //post: returns a bool representing if the monster is alive
        //description: gets the life status of monster
        public bool GetRemoveStatus()
        {
            //returns the life status of the monster
            return isRemoved;
        }

        //pre: none
        //post: an interger representing the kill score
        //description: returns the kill score of the monster
        public int GetKillScore()
        {
            //returns kill score
            return killScore;
        }

        //pre: none
        //post: an interger represnting the monster type
        //description: returns teh monster type
        public int GetMonstType()
        {
            //returns monster type
            return monsterType;
        }
    }
}