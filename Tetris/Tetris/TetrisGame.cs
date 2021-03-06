using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Practicum.Tetris
{
    enum MenuActions : byte { Mono, Color, Info, Quit, Menu }
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
        int[] scoreTracker = new int[7];
        int[] speedLvl = new int[3];
        float[] pointsPerColor;
        string gameOverString1, gameOverString2;
        Color black = Color.Black, white = Color.White;

        // timers
        int moveTimerLim, newBlockTimer;
        const int moveTimerLimBase = 1000,
                  moveTimerLimMin = 50,
                  newBlockTimerLim = 250;

        // sprites
        SpriteFont fontInfo, fontGameInfo, fontGameOver;
        Texture2D nextBlockWindow, controls, fade, // special items
                  btnMono, btnColor, btnInfo, btnQuit, btnBack, btnMenu, // buttons
                  blockSprites, // blocks
                  scoreViewerMono, scoreViewerColor, scoreViewerCurrent, numbersMono, numbersColor, numbersCurrent, speedLevelWindow, // score and level
                  gameLogo, gameOverLogo;

        // sound
        SoundEffect blockHitSound;
        Song bgMusic;
        
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
            fontGameOver = Content.Load<SpriteFont>("fontGameOver");
            fontInfo = Content.Load<SpriteFont>("fontInfo");
            fontGameInfo = Content.Load<SpriteFont>("gameInfo");

            // misc
            fade = Content.Load<Texture2D>("fade");
            gameLogo = Content.Load<Texture2D>("gameLogo");
            gameOverLogo = Content.Load<Texture2D>("gameOverLogo");

            // score and level view
            scoreViewerMono = Content.Load<Texture2D>("scoreViewerMono");
            scoreViewerColor = Content.Load<Texture2D>("scoreViewerColor");

            speedLevelWindow = Content.Load<Texture2D>("speedLevelWindow");

            numbersMono = Content.Load<Texture2D>("numbersMono");
            numbersColor = Content.Load<Texture2D>("numbersColor");

            // music and soundeffects
            bgMusic = Content.Load<Song>("bgMusic");
            blockHitSound = Content.Load<SoundEffect>("soundHit");
            MediaPlayer.Play(bgMusic);
            MediaPlayer.Volume = 0.4f;
            MediaPlayer.IsRepeating = true;

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

                        // score viewer
                        scoreViewerCurrent = isColor ? scoreViewerColor : scoreViewerMono;
                        numbersCurrent = isColor ? numbersColor : numbersMono;
                        scoreSplicer();
                        speedLvlSplicer();
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
                            blockHitSound.Play();
                            field.placeBlock(tetrisBlockCurrent);
                            scorePoints(field.checkClearedRows(tetrisBlockCurrent.OffsetY));
                            if (field.reachedTop())
                            {
                                // the top of the field could not be cleared
                                // GAME OVER
                                reserveGameState = GameStates.GameOver;
                                gameOverString1 = "You made it to speed level " + (Math.Min(finishedRows, (moveTimerLimBase - moveTimerLimMin) / 2)).ToString();
                                gameOverString2 = "and earned " + score.ToString() + " points!";
                            }
                            resetBlock();
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
                    // logo
                    spriteBatch.Draw(gameLogo, new Vector2((screenWidth - 380) / 2, 60), white);

                    // buttons
                    foreach(Button button in menuButtons)
                    { button.Draw(spriteBatch); }

                    break;

                case GameStates.Info:
                    // info screen
                    spriteBatch.Draw(controls, Vector2.Zero, white);
                    backButton.Draw(spriteBatch);
                    
                    // game mode base info
                    spriteBatch.DrawString(fontInfo, "MONOCHROME MODE:\nPlay tetris like you\'re used to.", new Vector2(10, 360), black);
                    spriteBatch.DrawString(fontInfo, "RAINBOW MODE:\nColorfull tetris with color based scoring.", new Vector2(10, 405), Color.Purple);
                    break;

                case GameStates.Playing:
                    drawGameField(spriteBatch);
                    break;

                case GameStates.GameOver:
                    // draw the last playing field at the background
                    drawGameField(spriteBatch);
                    // fade it away
                    spriteBatch.Draw(fade, new Rectangle(0,0,screenWidth, screenHeigth), white); // add a semitransparent gray layer

                    // GAME OVER drawing
                    // logo
                    spriteBatch.Draw(gameOverLogo, new Vector2((screenWidth - 460) / 2, 20), white);
                    // centered game over text
                    spriteBatch.DrawString(fontGameOver, gameOverString1, new Vector2(screenWidth / 2, 265), black, 0, new Vector2(fontGameOver.MeasureString(gameOverString1).X / 2, 0), 1, SpriteEffects.None, 0);
                    spriteBatch.DrawString(fontGameOver, gameOverString2, new Vector2(screenWidth / 2, 300), black, 0, new Vector2(fontGameOver.MeasureString(gameOverString2).X / 2, 0), 1, SpriteEffects.None, 0);
                    
                    // buttons
                    menuButton.Draw(spriteBatch);
                    menuButtons[3].Draw(spriteBatch); // Quit button

                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>Keeps score and fastens block movement.</summary>
        /// <param name="scoreChange">An array containing the cleared rows and blocks (rainbow mode only).</param>
        public void scorePoints(int[] scoreChange)
        {
            if (scoreChange[0] != 0)
            {
                float points = 0;

                // total rows cleared increses
                finishedRows += scoreChange[0];

                // award points per cleared row
                for(int i = 1; i <= scoreChange[0]; i++)
                { points += 100 * i; } // get more points for each cleared row

                if (isColor)
                {
                    // uni colored rows get bonus points
                    points += scoreChange[1] * 400;

                    // points per color
                    for(int color = 0; color < 6; color++)
                    {
                        points += scoreChange[color + 2] * pointsPerColor[color];
                        pointsPerColor[color] = MathHelper.Clamp(pointsPerColor[color] - scoreChange[color + 2] * 0.1f + 0.2f, 1, 5);
                    }
                }

                // update score
                score += (int)points;
                scoreSplicer();

                // update block falling speed
                moveTimerLim = Math.Max(moveTimerLimBase - finishedRows * 2, moveTimerLimMin); // go down faster by 2 msec per cleared row to a min of 50 msec per step (realy fast)
                speedLvlSplicer();
            }
        }

        /// <summary>Splices the score into an array for the score counter to use.</summary>
        private void scoreSplicer()
        {
            string tmp = score.ToString();
            
            // make sure there are 7 symbols for the score (score in the milions)
            tmp = tmp.PadLeft(7, '0');
            for(int i = 0; i < 7; i++)
            { scoreTracker[i] = tmp[i] - 48; }
        }

        /// <summary>Splices the faling speed into an array fot the level viewer to use.</summary>
        private void speedLvlSplicer()
        {
            string tmp = Math.Min(finishedRows, (moveTimerLimBase - moveTimerLimMin) / 2).ToString();

            // make sure there are 3 symbols for the level (level in the hundreds)
            tmp = tmp.PadLeft(3, '0');
            for (int i = 0; i < 3; i++)
            { speedLvl[i] = tmp[i] - 48; }
        }

        /// <summary>Draws the components of the game field.</summary>
        private void drawGameField(SpriteBatch spriteBatch)
        {
            // field
            field.Draw(spriteBatch);

            // active block
            if (tetrisBlockCurrent != null)
            { tetrisBlockCurrent.Draw(spriteBatch); }

            // next block
            spriteBatch.Draw(nextBlockWindow, new Vector2(field.Width * 20 + 20, 20), white);
            tetrisBlockNext.Draw(spriteBatch);

            // in game info (how to get points)
            drawInfo(spriteBatch, field.Width * 20 + 20, 190);

            // show fancy score
            drawScore(spriteBatch, 20, field.Height * 20 + 20);

            // show speed level
            drawLevelCounter(spriteBatch, field.Width * 20 + 160, 20);
        }

        /// <summary>Draws the in game info.</summary>
        private void drawInfo(SpriteBatch spriteBatch, int offsetX, int offsetY)
        {
            spriteBatch.DrawString(fontGameInfo, "HOW TO EARN POINTS", new Vector2(offsetX, offsetY), black);

            // rows
            for(int i = 0; i < 8; i++)
            { spriteBatch.Draw(blockSprites, new Vector2(offsetX + 20 * i, offsetY + 25), new Rectangle(20, 0, 20, 20), white); }

            spriteBatch.DrawString(fontGameInfo,
                                  "Clearing rows:\n1x = 100, 2x = 300\n3x = 600, 4x = 1000" + (isColor ? "\nUni colored rows:\n400 bonus" : ""),
                                  new Vector2(offsetX, offsetY + 45), black);

            // rainbow mode block bonus
            if (isColor)
            {
                for (int i = 1; i < 4; i++)
                {
                    spriteBatch.Draw(blockSprites, new Vector2(offsetX, offsetY + 125 + i * 25), new Rectangle(i * 20, 0, 20, 20), white);
                    spriteBatch.DrawString(fontGameInfo, (Math.Round(pointsPerColor[i - 1], 1)).ToString(), new Vector2(offsetX + 30, offsetY + 125 + i * 25), black);
                }
                for (int i = 4; i < 7; i++)
                {
                    spriteBatch.Draw(blockSprites, new Vector2(offsetX + 100, offsetY + 50 + i * 25), new Rectangle(i * 20, 0, 20, 20), white);
                    spriteBatch.DrawString(fontGameInfo, (Math.Round(pointsPerColor[i - 1], 1)).ToString(), new Vector2(offsetX + 130, offsetY + 50 + i * 25), black);
                }
            }
        }

        /// <summary>Draws the score window at specified position.</summary>
        private void drawScore(SpriteBatch spriteBatch, int offsetX, int offsetY)
        {
            // score window
            spriteBatch.Draw(scoreViewerCurrent, new Vector2(offsetX, offsetY), white);

            // score itself
            for (int i = 0; i < 7; i++)
            { spriteBatch.Draw(numbersCurrent, new Vector2(offsetX + 5 + i * 20, offsetY + 30), new Rectangle(scoreTracker[i] * 20, 0, 20, 30), white); }
        }

        /// <summary>Draws the level counter at specified position.</summary>
        private void drawLevelCounter(SpriteBatch spriteBatch, int offsetX, int offsetY)
        {
            // level window
            spriteBatch.Draw(speedLevelWindow, new Vector2(offsetX, offsetY), white);

            // level itself
            for (int i = 0; i < 3; i++)
            { spriteBatch.Draw(numbersMono, new Vector2(offsetX + 10 + 20 * i, offsetY + 50), new Rectangle(speedLvl[i] * 20, 0, 20, 30), white); }
        }

        /// <summary>Clears placed blocks and starts timer for the next block.</summary>
        private void resetBlock()
        {
            newBlockTimer = 0;
            tetrisBlockCurrent = null;
        }
    }
}
