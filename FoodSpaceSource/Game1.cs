﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using IntroGameLibrary;
using IntroGameLibrary.Sprite;
using IntroGameLibrary.Util;
using IntroGameLibrary.State;

#endregion

namespace Prototype
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        CelAnimationManager celAnimationManager;

        public SpriteBatch sb;

        InputHandler input;

        public GameStateManager GameManager;

        public IPlayingState PlayingState;
        public IPausedState PausedState;
        public ITitleIntroState TitleState;
        public IStartMenuState StartMenuState;
        public IEndState EndState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;

            Content.RootDirectory = "Content";

            sb = new SpriteBatch(GraphicsDevice);

            input = new InputHandler(this);

            this.Components.Add(input);

            celAnimationManager = new CelAnimationManager(this);
            this.Components.Add(celAnimationManager);

            GameManager = new GameStateManager(this);

            PlayingState = new PlayingState(this);
            PausedState = new PausedState(this);
            TitleState = new TitleIntroState(this);
            StartMenuState = new StartMenuState(this);
            EndState = new EndState(this);

            GameManager.ChangeState(TitleState.Value);
            //GameManager.ChangeState(PlayingState.Value);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here



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
            // Allows the game to exit
            //if (input.KeyboardState.IsKeyDown(Keys.Escape))
           // {
            //    this.Exit();
            //}

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here


            base.Draw(gameTime);
        }
    }
}
