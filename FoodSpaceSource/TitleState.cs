using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IntroGameLibrary;
using IntroGameLibrary.Util;
using IntroGameLibrary.State;

namespace Prototype
{
    public interface ITitleIntroState : IGameState { }

    public sealed class TitleIntroState : BaseGameState, ITitleIntroState
    {
        private Texture2D texture;
        private SpriteFont font;

        private float Alpha = 0.0f;
        private bool Used = false;

        int TimeCount = 0;

        public TitleIntroState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(ITitleIntroState), this);
        }

        public override void Update(GameTime gameTime)
        {
            TimeCount += gameTime.ElapsedGameTime.Milliseconds;

            if (!Used)
            {
                Alpha += gameTime.ElapsedGameTime.Milliseconds * 0.08f;
            }
            else
            {
                Alpha -= gameTime.ElapsedGameTime.Milliseconds * 0.15f;
            }

            if (Alpha > 255.0f)
            {
                Alpha = 255.0f;
            }

            if (Input.WasPressed(0, InputHandler.ButtonType.Back, Keys.Escape))
                OurGame.Exit();

            //Startbutton or enter
            if (Input.WasPressed(0, InputHandler.ButtonType.Start, Keys.Enter) && Alpha > 254.0f)
            {
                // push our start menu onto the stack
                Used = true;
            }

            //Start with spacebar
            if (Input.KeyboardState.WasKeyPressed(Keys.Space) && Alpha > 254.0f && !Used)
            {
                // push our start menu onto the stack
                Used = true;
            }

            if (Used && Alpha <= 0)
            {
                GameManager.PushState(OurGame.StartMenuState.Value);
                Used = false;
                Alpha = 0.0f;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            OurGame.GraphicsDevice.Clear(Color.Black);
            Vector2 pos = new Vector2(0, 0);
            Color NewColor = Color.White;

            NewColor.A = (byte)Alpha;

            OurGame.sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            OurGame.sb.Draw(texture, pos, NewColor);

            if (Alpha >= 254.0f && (TimeCount % 1000) > 500 )
            {
                OurGame.sb.DrawString(font, "Press Enter to Start", new Vector2(490, 660), Color.White);
            }

            OurGame.sb.End();

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            texture = Content.Load<Texture2D>("FoodSpace");

            this.font = Content.Load<SpriteFont>("NFontLarge");

            base.LoadContent();
        }
    }
}
