using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Practicum.Tetris
{

    enum Colors : byte { Blank, Blue, Green, Orange, Purple, Red, Yellow }
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TetrisGame : Game
    {
        
        static void Main()
        {
            TetrisGame game = new TetrisGame();
            game.Run();
        }

        // game
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Viewport view;

        //gamestate
        byte gameState, reserveGameState;

        // objects
        PlayingField field;
        TetrisBlock tetrisBlock;

        //variables
        byte fieldWidth, fieldHeight;
        bool isColor;
        int score = 0;

        //timers
        int moveTimer = 0;
        int moveTimerLim, moveTimerLimBase;

        //sprites
        Texture2D[] blockSprites;

        public TetrisGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        { 
            base.Initialize();

            // window sizing
            view = GraphicsDevice.Viewport;
            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferredBackBufferWidth = 500;
            graphics.ApplyChanges();

            // gamestates
            gameState = 0; reserveGameState = 0;
            isColor = false;

            // field
            fieldWidth = 12; fieldHeight = 20;
            field = new PlayingField(fieldWidth, fieldHeight);

            // blocks
            tetrisBlock = new TetrisBlock(false, false, false, false, false, true, true, false, false, true, true, false, false, false, false, false);

            //timers
            moveTimerLim = 1000; moveTimerLimBase = 1000;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            blockSprites = new Texture2D[] { Content.Load<Texture2D>("noBlock"),
                                             Content.Load<Texture2D>("blockBlue"),
                                             Content.Load<Texture2D>("blockGreen"),
                                             Content.Load<Texture2D>("blockOrange"),
                                             Content.Load<Texture2D>("blockPurple"),
                                             Content.Load<Texture2D>("blockRed"),
                                             Content.Load<Texture2D>("blockYellow") };

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            gameState = reserveGameState;

            //movement tick
            if (moveTimer >= moveTimerLim)
            {
                moveTimer = moveTimer % moveTimerLim;

                //move block
                if(field.canBlockMove(tetrisBlock, 0))
                { tetrisBlock.moveDown(); }
                else
                {
                    // code for copying block to field and reseting the block
                }
                

            } else { moveTimer += gameTime.ElapsedGameTime.Milliseconds; }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);
            spriteBatch.Begin();

            for(int i = 0; i < fieldHeight; i++)
            {
                for(int j = 0; j < fieldWidth; j++)
                {
                    if (field.checkGrid(i, j))
                    {
                        if (isColor) { spriteBatch.Draw(blockSprites[field.checkGrid2(i, j)], new Vector2(j * 20, i * 20), Color.White); }
                        else { spriteBatch.Draw(blockSprites[1], new Vector2(j * 20, i * 20), Color.White); }
                    }
                    else { spriteBatch.Draw(blockSprites[0], new Vector2(j * 20, i * 20), Color.White); }
                }
            }
            
            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tetrisBlock.checkGrid(i, j))
                    {
                        if (isColor) { spriteBatch.Draw(blockSprites[tetrisBlock.checkGrid2(i, j)], new Vector2((tetrisBlock.OffsetX + j) * 20, (tetrisBlock.OffsetY + i) * 20), Color.White); }
                        else { spriteBatch.Draw(blockSprites[1], new Vector2((tetrisBlock.OffsetX + j) * 20, (tetrisBlock.OffsetY + i) * 20), Color.White); }
                    }
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Keeps score and fastens block movement.
        /// </summary>
        /// <param name="points">The earned points.</param>
        public void scorePoints(int points)
        {
            score += points;
            moveTimerLim = moveTimerLimBase - (score / 100);
        }

    }
}
