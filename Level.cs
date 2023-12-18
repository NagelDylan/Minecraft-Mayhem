//Author: Dylan Nagel
//File Name: Level.cs
//Project Name: NagelD_PASS2
//Description: Stores the level and information surrounding the level

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NagelD_PASS2
{
    public class Level
    {
        //store block length
        const int BLOCK_LENGTH = 50;

        //store the number of blocks taht are randomized
        private const int RAND_BLOCK_NUM = 3;

        //stores the background block information
        private Texture2D[] backBlockImgs;
        private int[,] blockBackground = new int[Game1.screenWidth / BLOCK_LENGTH, Game1.screenHeight / BLOCK_LENGTH];
        private Rectangle[,] blockBackgroundRec = new Rectangle[Game1.screenWidth / BLOCK_LENGTH, Game1.screenHeight / BLOCK_LENGTH];

        //pre: a valid array of backgroudn block images
        //post: none
        //description: creates a level object
        public Level(Texture2D[] backBlockImgs)
        {
            //sets background block images to perameter value
            this.backBlockImgs = backBlockImgs;

            //loop through the columns of the block collection background
            for (int i = 0; i < blockBackground.GetLength(1); i++)
            {
                //loop through the rows of the block collection background
                for (int x = 0; x < blockBackground.GetLength(0); x++)
                {
                    //set the block background at x and i index
                    blockBackgroundRec[x, i] = new Rectangle(BLOCK_LENGTH * x, BLOCK_LENGTH * i, BLOCK_LENGTH, BLOCK_LENGTH);
                }
            }
        }

        //pre: none
        //post: none
        //description: makes background for pause screen
        public void MakeBackground()
        {
            //loops through the block background columns
            for (int i = 0; i < blockBackground.GetLength(1); i++)
            {
                //looops through the block background rows
                for (int x = 0; x < blockBackground.GetLength(0); x++)
                {
                    //sets block background to a random block at index x and i
                    blockBackground[x, i] = Game1.rng.Next(0, 90) % RAND_BLOCK_NUM;
                }
            }
        }

        //pre: none
        //post: none
        //description: draws the background
        public void DrawBackground()
        {
            //loops through the block background columns
            for (int i = 0; i < blockBackground.GetLength(1); i++)
            {
                //looops through the block background rows
                for (int x = 0; x < blockBackground.GetLength(0); x++)
                {
                    //draw block background at index x and i
                    Game1.spriteBatch.Draw(backBlockImgs[blockBackground[x, i]], blockBackgroundRec[x, i], Color.White);
                }
            }
        }

        //pre: a valid weapon
        //post: none
        //description: destroys surrounding blocks to creeper explosion
        public void DestroySurBlocks(Rectangle weaponRec)
        {
            //loops through columns of blocks in background
            for (int i = 0; i < blockBackgroundRec.GetLength(1); i++)
            {
                //loops through rows of blocks in background
                for (int x = 0; x < blockBackgroundRec.GetLength(0); x++)
                {
                    //checks if the explosion rectangle touches block at index x and i
                    if (weaponRec.Intersects(blockBackgroundRec[x, i]))
                    {
                        //checks if the block is not cobble stone
                        if (blockBackground[x, i] != Game1.COBBLESTONE)
                        {
                            //sets block to dirt
                            blockBackground[x, i] = Game1.DIRT;
                        }

                    }
                }
            }
        }
    }
}
