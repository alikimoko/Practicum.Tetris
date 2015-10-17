using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Practicum.Tetris
{
    enum MenuActions : byte { Mono, Color, Info, Quit, Menu }
    enum Colors : byte { Blank, Blue, Green, Orange, Purple, Red, Yellow }
    enum GameStates : byte { Menu, Playing, GameOver, Info }
    enum Movement : byte { Stay, Down, Left, Right }

    public class TetrisGame : Game
    {
        // Start the program
        static void Main()
        {
            TetrisGame game = new TetrisGame();
            game.Run();
        }

        // game
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputHelper input;
        Random rand;

        //gamestate
        GameStates gameState, reserveGameState;

        // objects
        PlayingField field;
        TetrisBlock tetrisBlockCurrent, tetrisBlockNext;
        Button[] menuButtons;
        Button backButton, menuButton;

        // game variables
        byte fieldWidth, fieldHeight;
        bool isColor;
        bool couldGoDown;
        int screenWidth = 500,
            screenHeigth = 500,
            score = 0,
            finishedRows = 0;
        float[] pointsPerColor;

        // timers
        int moveTimerLim, newBlockTimer;
        const int moveTimerLimBase = 1000,
                  moveTimerLimMin = 50,
                  newBlockTimerLim = 250;

        //sprites
        Texture2D nextBlockWindow,
                  btnMono, btnColor, btnInfo, btnQuit,
                  btnBack, btnMenu, controls,
                  blockSprites,
                  fade;
        SpriteFont fontRegularMenu, fontSelectedMenu;

        /// <summary>Make a new tetris game.</summary>
        public TetrisGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // window sizing
            graphics.PreferredBackBufferHeight = screenWidth;
            graphics.PreferredBackBufferWidth = screenHeigth;
            graphics.ApplyChanges();
        }

        /// <summary>Initialize non-content related variables and objects.</summary>
        protected override void Initialize()
        {
            base.Initialize();

            input = new InputHelper(150);
            rand = new Random();
            
            // gamestates
            gameState = GameStates.Menu;
            reserveGameState = GameStates.Menu;

            // field
            fieldWidth = 12; fieldHeight = 20;

            // timers
            moveTimerLim = moveTimerLimBase;
            newBlockTimer = newBlockTimerLim;

            // score keeping
            pointsPerColor = new float[] { 2.5f, 2.5f, 2.5f, 2.5f, 2.5f, 2.5f };
        }

        /// <summary>Load external content.</summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // block related textures
            blockSprites = Content.Load<Texture2D>("blockStrip");

            nextBlockWindow = Content.Load<Texture2D>("nextBlockWindow");

            // menu buttons
            btnMono = Content.Load<Texture2D>("btnMono");
            btnColor = Content.Load<Texture2D>("btnColor");
            btnInfo = Content.Load<Texture2D>("btnInfo");
            btnQuit = Content.Load<Texture2D>("btnQuit");
            btnBack = Content.Load<Texture2D>("btnBack");
            btnMenu = Content.Load<Texture2D>("btnMenu");

            controls = Content.Load<Texture2D>("controls");

            // needs the textures, so placing here (prevents nullpointers)
            menuButtons = new Button[] { new Button(screenWidth, btnMono, MenuActions.Mono, 150),
                                         new Button(screenWidth, btnColor, MenuActions.Color, 250),
                                         new Button(screenWidth, btnInfo, MenuActions.Info, 350),
                                         new Button(screenWidth, btnQuit, MenuActions.Quit, 400) };
            backButton = new Button(screenWidth, btnBack, MenuActions.Menu, 450);
            menuButton = new Button(screenWidth, btnMenu, MenuActions.Menu, 350);

            // fonts
            fontRegularMenu = Content.Load<SpriteFont>("FontMenuRegular");
            fontSelectedMenu = Content.Load<SpriteFont>("FontMenuSelected");

            // misc
            fade = Content.Load<Texture2D>("fade");

        }

        /// <summary>Updates the game.</summary>
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
                                    // open info screen
                                    reserveGameState = GameStates.Info;
                                    break;
                                case MenuActions.Quit:
                                    // quir the game
                                    Exit();
                                    break;
                            }
                        }
                    }

                    if (reserveGameState == GameStates.Playing)
                    {
                        // set up for playing the selected mode

                        // create the field
                        field = new PlayingField(fieldWidth, fieldHeight, blockSprites, isColor);

                        // create the blocks
                        tetrisBlockCurrent = TetrisBlock.createBlock(blockSprites, field, isColor);
                        tetrisBlockNext = TetrisBlock.createBlock(blockSprites, field, isColor);
                        tetrisBlockCurrent.placeBlock();
                        tetrisBlockCurrent.setMoveTimer(moveTimerLim);
                    }

                    break;

                case GameStates.Info:
                    // button for going back to the menu
                    if (backButton.isClicked(input)) { reserveGameState = GameStates.Menu; }
                    break;

                case GameStates.Playing:

                    // make new blocks
                    if (tetrisBlockCurrent == null)
                    {
                        if (newBlockTimer >= newBlockTimerLim)
                        {
                            tetrisBlockCurrent = tetrisBlockNext;
                            tetrisBlockNext = TetrisBlock.createBlock(blockSprites, field, isColor);
                            if (!tetrisBlockCurrent.placeBlock())
                            {
                                // could not place the block
                                // GAME OVER
                                reserveGameState = GameStates.GameOver;
                                break;
                            }
                            tetrisBlockCurrent.setMoveTimer(moveTimerLim);
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
                            scorePoints(field.checkClearedRows(tetrisBlockCurrent.OffsetY));
                            if (field.reachedTop())
                            {
                                // the top of the field could not be cleared
                                // GAME OVER
                                reserveGameState = GameStates.GameOver;
                            }
                            resetBlock();
                            // TODO: score
                        }
                    }

                    break;

                case GameStates.GameOver:
                    // TODO: GAME OVER text and score view

                    // buttons
                    if (menuButton.isClicked(input))
                    {
                        // back to menu
                        reserveGameState = GameStates.Menu;

                        // reset for next playthrough
                        score = 0;
                        finishedRows = 0;
                        moveTimerLim = moveTimerLimBase;
                    }
                    if (menuButtons[3].isClicked(input)) { Exit(); } // quit game

                    break;
            }

            // you can always exit the game
            if (input.KeyPressed(Keys.Escape)) { Exit(); }

            base.Update(gameTime);
        }

        /// <summary>Draw the game</summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);
            spriteBatch.Begin();

            switch (gameState)
            {
                case GameStates.Menu:
                    // menu drawing
                    // TODO: logo

                    foreach(Button button in menuButtons)
                    { button.Draw(spriteBatch); }

                    break;

                case GameStates.Info:
                    // info screen
                    spriteBatch.Draw(controls, Vector2.Zero, Color.White);
                    spriteBatch.DrawString(fontSelectedMenu, "MONOCHROME MODE:\nPlay tetris like you\'re used to.", new Vector2(10, 350), Color.Black);
                    spriteBatch.DrawString(fontSelectedMenu, "RAINBOW MODE:\nColorfull tetris with color based scoring.", new Vector2(10, 400), Color.Purple);
                    backButton.Draw(spriteBatch);
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

                    // TODO: in game info (not just text)
                    spriteBatch.DrawString(fontRegularMenu, "Press <ESC> to exit", new Vector2(10,screenHeigth - 30), Color.Black);

                    // TODO: Show fancy score
                    spriteBatch.DrawString(fontRegularMenu, score.ToString(), new Vector2(300, 400), Color.Black);

                    break;

                case GameStates.GameOver:
                    // draw the last playing field faded at the background
                    field.Draw(spriteBatch);
                    spriteBatch.Draw(nextBlockWindow, new Vector2(field.Width * 20 + 20, 20), Color.White);
                    tetrisBlockNext.Draw(spriteBatch);
                    spriteBatch.Draw(fade, new Rectangle(0,0,screenWidth, screenHeigth), Color.White); // add a semitransparent gray layer

                    // TODO: GAME OVER text and score view
                    spriteBatch.DrawString(fontRegularMenu, "GAME OVER", new Vector2(170, 10), Color.Black);
                    // buttons
                    menuButton.Draw(spriteBatch);
                    menuButtons[3].Draw(spriteBatch);

                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>Keeps score and fastens block movement.</summary>
        /// <param name="scoreChange">An array containing the cleared blocks and rows.</param>
        public void scorePoints(int[] scoreChange)
        {
            float points = 0;
            if (scoreChange[0] != 0)
            {
                // award 50 points per cleared row
                points = scoreChange[0] * 50;

                if (isColor)
                {
                    // uni colored rows
                    points += scoreChange[1] * 450;

                    // points per color
                    for(int color = 0; color < 6; color++)
                    {
                        points += scoreChange[color + 2] * pointsPerColor[color];
                        pointsPerColor[color] = MathHelper.Clamp(pointsPerColor[color] - scoreChange[color] * 0.1f + 0.2f, 1, 5);
                    }
                }
            }

            score += (int)points;
            moveTimerLim = moveTimerLimBase - (score / 100);
            if (moveTimerLim < moveTimerLimMin)
            { moveTimerLim = moveTimerLimMin; }
        }

        /// <summary>Clears placer blocks and starts timer for the next block.</summary>
        private void resetBlock()
        {
            newBlockTimer = 0;
            tetrisBlockCurrent = null;
        }
    }
}
