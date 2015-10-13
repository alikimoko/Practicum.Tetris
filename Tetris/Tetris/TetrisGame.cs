using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Practicum.Tetris
{
    enum MenuActions : byte { Mono, Color, Info, Quit }
    enum Colors : byte { Blank, Blue, Green, Orange, Purple, Red, Yellow }
    enum GameStates : byte { Menu, Playing, GameOver, Info }
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
        InputHelper input;

        //gamestate
        GameStates gameState, reserveGameState;

        // objects
        PlayingField field;
        TetrisBlock tetrisBlockCurrent, tetrisBlockNext;
        Button[] menuButtons;

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
        Texture2D nextBlockWindow,
                  btnMono, btnColor, btnInfo, btnQuit;
        Texture2D[] blockSprites;
        SpriteFont fontRegularMenu, fontSelectedMenu;

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
            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferredBackBufferWidth = 500;
            graphics.ApplyChanges();

            // gamestates
            gameState = GameStates.Menu; reserveGameState = GameStates.Menu;

            // field
            fieldWidth = 12; fieldHeight = 20;

            //timers
            moveTimerLim = moveTimerLimBase;
            newBlockTimer = newBlockTimerLim;

            // menu buttons
            menuButtons = new Button[] { new Button(graphics, btnMono, MenuActions.Mono, 100),
                                         new Button(graphics, btnColor, MenuActions.Color, 200),
                                         new Button(graphics, btnInfo, MenuActions.Info, 300),
                                         new Button(graphics, btnQuit, MenuActions.Quit, 350) };
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // block related textures
            blockSprites = new Texture2D[] { Content.Load<Texture2D>("noBlock"),
                                             Content.Load<Texture2D>("blockBlue"),
                                             Content.Load<Texture2D>("blockGreen"),
                                             Content.Load<Texture2D>("blockOrange"),
                                             Content.Load<Texture2D>("blockPurple"),
                                             Content.Load<Texture2D>("blockRed"),
                                             Content.Load<Texture2D>("blockYellow") };

            nextBlockWindow = Content.Load<Texture2D>("nextBlockWindow");

            // menu buttons
            btnMono = Content.Load<Texture2D>("btnMono");
            btnColor = Content.Load<Texture2D>("btnColor");
            btnInfo = Content.Load<Texture2D>("btnInfo");
            btnQuit = Content.Load<Texture2D>("btnQuit");
            foreach(Button button in menuButtons) { button.placeButton(); }

            // fonts
            fontRegularMenu = Content.Load<SpriteFont>("FontMenuRegular");
            fontSelectedMenu = Content.Load<SpriteFont>("FontMenuSelected");

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

                    // menu buttons
                    foreach(Button button in menuButtons)
                    {
                        if (button.isClicked(input))
                        {
                            switch (button.Action)
                            {
                                case MenuActions.Mono:
                                    // start monochrome mode
                                    reserveGameState = GameStates.Playing;
                                    isColor = false;
                                    break;
                                case MenuActions.Color:
                                    // start color mode
                                    reserveGameState = GameStates.Playing;
                                    isColor = true;
                                    break;
                                case MenuActions.Info:
                                    reserveGameState = GameStates.Info;
                                    break;
                                case MenuActions.Quit:
                                    Exit();
                                    break;
                            }
                        }
                    }

                    if (reserveGameState == GameStates.Playing)
                    {
                        // create the field
                        field = new PlayingField(fieldWidth, fieldHeight, blockSprites, isColor);

                        // create the blocks
                        tetrisBlockCurrent = TetrisBlock.createBlock(blockSprites, field, moveTimerLim, isColor);
                        tetrisBlockNext = TetrisBlock.createBlock(blockSprites, field, moveTimerLim, isColor);
                        tetrisBlockCurrent.placeBlock();
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
                            if (!tetrisBlockCurrent.placeBlock())
                            {
                                // could not place the block
                                // GAME OVER
                                reserveGameState = GameStates.GameOver;
                                break;
                            }
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

                case GameStates.GameOver:
                    // code for game over
                    break;
            }

            if (input.KeyPressed(Keys.Escape)) { Exit(); }

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
                    
                    
                    spriteBatch.DrawString(fontRegularMenu, "Welcome to TheTetris, press <spacebar> to Play!", new Vector2(10,10), Color.White);
                    spriteBatch.DrawString(fontRegularMenu, "Or press <ESC> to Exit.", new Vector2(10, 30), Color.White);
                    
                    break;

                case GameStates.Playing:

                    // field
                    field.Draw(spriteBatch);
                    
                    // active block
                    if (tetrisBlockCurrent != null)
                    { tetrisBlockCurrent.Draw(spriteBatch); }

                    // next block
                    spriteBatch.Draw(nextBlockWindow, new Vector2(field.Width * 20 + 20, 20), Color.White);
                    tetrisBlockNext.Draw(spriteBatch);
                    /*    
                    spriteBatch.DrawString(fontRegularMenu,
                                           " Controls: \n A to move left \n S to move down \n D to move left \n Q to turn left \n E to turn right \n <ESC> to Exit \n <SPACEBAR> to pause \n Enjoy! \n \n SCORE: " + score,
                                           new Vector2(250, 10), Color.White);
                                           // calculate a position in comparison to the field needed ^
                    */
                    break;

                case GameStates.GameOver:
                    spriteBatch.Draw(blockSprites[2], Vector2.Zero, Color.White);
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
