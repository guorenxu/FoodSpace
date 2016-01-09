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
    public interface IEndState : IGameState { }

    class EndState : BaseGameState, IEndState
    {

        private Texture2D EndTexture;
        private SpriteFont font;

        public EndState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IEndState), this);
        }
        protected override void LoadContent()
        {
            this.EndTexture = Content.Load<Texture2D>("Transparent25Percent");
            this.font = Content.Load<SpriteFont>("NFont");
            base.LoadContent();

        }

        public override void Update(GameTime gameTime)
        {
            if (Input.WasPressed(0, InputHandler.ButtonType.Back, Keys.Enter))
            {
                GameManager.PopState();
            }
            else if (Input.WasPressed(0, InputHandler.ButtonType.Back, Keys.Escape))
            {
                OurGame.Exit();
            }
            //TODO add pausedstate

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            OurGame.sb.Begin();
            Rectangle fullscreen = new Rectangle(0, 0, OurGame.Window.ClientBounds.Width, OurGame.Window.ClientBounds.Height);
            OurGame.sb.Draw(EndTexture, fullscreen, Color.Black);
            OurGame.sb.DrawString(font, "You died! Press Enter to try again or Escape to quit.", new Vector2(100, 350), Color.Red);
            OurGame.sb.End();
            base.Draw(gameTime);
        }
    }
}
