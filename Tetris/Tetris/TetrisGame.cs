using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        InputHelper input;

        //gamestate
        byte gameState, reserveGameState;

        // objects
        PlayingField field;
        TetrisBlock tetrisBlock;

        //variables
        byte fieldWidth, fieldHeight;
        bool isColor;
        int score = 0;
        bool newBlockNeeded;

        //timers
        int moveTimer = 0;
        int moveTimerLim, newBlockTimer;
        const int moveTimerLimBase = 1000,
                  newBlockTimerLim = 250;

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

            input = new InputHelper();

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

            //timers
            moveTimerLim = moveTimerLimBase;
            newBlockTimer = newBlockTimerLim;

            newBlockNeeded = true;

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
            // base updates
            gameState = reserveGameState;
            input.Update(gameTime);

            // make new blocks
            if (newBlockNeeded)
            {
                if (newBlockTimer >= newBlockTimerLim)
                {
                    tetrisBlock = TetrisBlock.createBlock();
                    newBlockNeeded = false;
                }
                else
                { newBlockTimer += gameTime.ElapsedGameTime.Milliseconds; }
            }
            
            if(tetrisBlock != null)
            {
                // handle user inputs
                if (input.KeyPressed(Keys.A) && field.canBlockMove(tetrisBlock, 1))
                { tetrisBlock.move(1); }

                if (input.KeyPressed(Keys.D) && field.canBlockMove(tetrisBlock, 2))
                { tetrisBlock.move(2); }

                if (input.IsKeyDown(Keys.S))
                {
                    // move all the way down
                    while (field.canBlockMove(tetrisBlock, 0))
                    { tetrisBlock.move(); }

                    field.placeBlock(tetrisBlock);
                    resetBlock();
                }

                if (input.KeyPressed(Keys.Q)) { tetrisBlock.turnAntiClockwise(field); }

                if (input.KeyPressed(Keys.E)) { tetrisBlock.turnClockwise(field); }

                //movement tick
                if (moveTimer >= moveTimerLim &&
                    tetrisBlock != null) // if block gets placed by user letting the block fall (verry small chance, but would cause nullpointer exeptions)
                {
                    moveTimer = moveTimer % moveTimerLim;

                    //move block
                    if (field.canBlockMove(tetrisBlock, 0))
                    { tetrisBlock.move(); }
                    else
                    {
                        field.placeBlock(tetrisBlock);
                        resetBlock();
                    }
                }
                else { moveTimer += gameTime.ElapsedGameTime.Milliseconds; }
            }

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

            for(int y = 0; y < fieldHeight; y++)
            {
                for(int x = 0; x < fieldWidth; x++)
                {
                    if (field.checkGridStruct(y, x))
                    {
                        if (isColor) { spriteBatch.Draw(blockSprites[field.checkGridCol(y, x)], new Vector2(y * 20, x * 20), Color.White); }
                        else { spriteBatch.Draw(blockSprites[1], new Vector2(x * 20, y * 20), Color.White); }
                    }
                    else { spriteBatch.Draw(blockSprites[0], new Vector2(x * 20, y * 20), Color.White); }
                }
            }
            
            if(tetrisBlock != null)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (tetrisBlock.checkGridStruct(y, x))
                        {
                            if (isColor) { spriteBatch.Draw(blockSprites[tetrisBlock.checkGridCol(y, x)], new Vector2((tetrisBlock.OffsetX + x) * 20, (tetrisBlock.OffsetY + y) * 20), Color.White); }
                            else { spriteBatch.Draw(blockSprites[1], new Vector2((tetrisBlock.OffsetX + x) * 20, (tetrisBlock.OffsetY + y) * 20), Color.White); }
                        }
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

        private void resetBlock()
        {
            newBlockNeeded = true;
            newBlockTimer = 0;
            tetrisBlock = null;
        }

    }
}
