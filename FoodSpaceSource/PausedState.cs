using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using IntroGameLibrary.State;
using IntroGameLibrary.Util;
using Microsoft.Xna.Framework.Input;

namespace Prototype
{
    public interface IPausedState : IGameState { }

    class PausedState : BaseGameState, IPausedState
    {

        private Texture2D pausedTexture;
        private SpriteFont font;

        public PausedState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IPausedState), this);
        }
        protected override void LoadContent()
        {
            this.pausedTexture = Content.Load<Texture2D>("Transparent25Percent");
            this.font = Content.Load<SpriteFont>("NFont");
            base.LoadContent();

        }

        public override void Update(GameTime gameTime)
        {
            if (Input.WasPressed(0, InputHandler.ButtonType.Back, Keys.Escape))
                GameManager.PopState();
            //TODO add pausedstate

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            OurGame.sb.Begin();
            Rectangle fullscreen = new Rectangle(0, 0, OurGame.Window.ClientBounds.Width, OurGame.Window.ClientBounds.Height);
            OurGame.sb.Draw(pausedTexture, fullscreen, Color.Black);
            OurGame.sb.DrawString(font, "Paused Press Esc to resume", new Vector2(100, 350), Color.Red);
            OurGame.sb.End();
            base.Draw(gameTime);
        }
    }
}
