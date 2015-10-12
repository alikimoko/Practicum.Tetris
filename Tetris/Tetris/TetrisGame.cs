using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Practicum.Tetris
{

    enum Colors : byte { Blank, Blue, Green, Orange, Purple, Red, Yellow }
    enum GameStates : byte { Menu, Playing }
    enum Movement : byte { Stay, Down, Left, Right }

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
        GameStates gameState, reserveGameState;

        // objects
        PlayingField field;
        TetrisBlock tetrisBlockCurrent, tetrisBlockNext;

        //variables
        byte fieldWidth, fieldHeight;
        bool isColor;
        bool couldGoDown;
        int score = 0;

        //timers
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

            input = new InputHelper(150);

            // window sizing
            view = GraphicsDevice.Viewport;
            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferredBackBufferWidth = 500;
            graphics.ApplyChanges();

            // gamestates
            gameState = GameStates.Menu; reserveGameState = GameStates.Menu;
            //isColor = false;

            // field
            fieldWidth = 12; fieldHeight = 20;

            //timers
            moveTimerLim = moveTimerLimBase;
            newBlockTimer = newBlockTimerLim;

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

            switch (gameState)
            {
                case GameStates.Menu:

                    if (input.IsKeyDown(Keys.Space))
                    {
                        reserveGameState = GameStates.Playing;

                        isColor = true;

                        // create the field
                        field = new PlayingField(fieldWidth, fieldHeight, blockSprites, isColor);

                        // create the blocks
                        tetrisBlockCurrent = TetrisBlock.createBlock(blockSprites, field, moveTimerLim, isColor);
                        tetrisBlockNext = TetrisBlock.createBlock(blockSprites, field, moveTimerLim, isColor);
                    }

                    break;

                case GameStates.Playing:

                    // make new blocks
                    if (tetrisBlockCurrent == null)
                    {
                        if (newBlockTimer >= newBlockTimerLim)
                        {
                            tetrisBlockCurrent = tetrisBlockNext;
                            tetrisBlockNext = TetrisBlock.createBlock(blockSprites, field, moveTimerLim, isColor);
                        }
                        else
                        { newBlockTimer += gameTime.ElapsedGameTime.Milliseconds; }
                    }

                    if (tetrisBlockCurrent != null)
                    {
                        // make the tetris block handle it's user inputs and movement
                        couldGoDown = tetrisBlockCurrent.handleMovement(gameTime, input);

                        if (!couldGoDown)
                        {
                            // reached bottom or hit existing block
                            field.placeBlock(tetrisBlockCurrent);
                            field.checkClearedRows(tetrisBlockCurrent.OffsetY);
                            resetBlock();
                            // TODO: score
                        }

                    }

                    break;
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

            switch (gameState)
            {
                case GameStates.Menu:
                    // menu drawing
                    break;

                case GameStates.Playing:

                    for (int y = 0; y < fieldHeight; y++)
                    {
                        for (int x = 0; x < fieldWidth; x++)
                        {
                            if (field.checkGridStruct(y, x))
                            {
                                if (isColor) { spriteBatch.Draw(blockSprites[field.checkGridCol(y, x)], new Vector2(x * 20, y * 20), Color.White); }
                                else { spriteBatch.Draw(blockSprites[1], new Vector2(x * 20, y * 20), Color.White); }
                            }
                            else { spriteBatch.Draw(blockSprites[0], new Vector2(x * 20, y * 20), Color.White); }
                        }
                    }

                    if (tetrisBlockCurrent != null)
                    {
                        for (int y = 0; y < 4; y++)
                        {
                            for (int x = 0; x < 4; x++)
                            {
                                if (tetrisBlockCurrent.checkGridStruct(y, x))
                                {
                                    if (isColor) { spriteBatch.Draw(blockSprites[tetrisBlockCurrent.checkGridCol(y, x)], new Vector2((tetrisBlockCurrent.OffsetX + x) * 20, (tetrisBlockCurrent.OffsetY + y) * 20), Color.White); }
                                    else { spriteBatch.Draw(blockSprites[1], new Vector2((tetrisBlockCurrent.OffsetX + x) * 20, (tetrisBlockCurrent.OffsetY + y) * 20), Color.White); }
                                }
                            }
                        }
                    }

                    break;
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
            newBlockTimer = 0;
            tetrisBlockCurrent = null;
        }

    }
}
