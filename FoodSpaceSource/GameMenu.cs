using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IntroGameLibrary;
using IntroGameLibrary.Util;
using IntroGameLibrary.State;

namespace Prototype
{
    public interface IStartMenuState : IGameState { }

    public sealed class StartMenuState : BaseGameState, IStartMenuState
    {
        private SpriteFont font;

        int Choice = 0;

        Color ColorOne;
        Color ColorTwo;

        public StartMenuState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IStartMenuState), this);

            ColorOne = Color.Gray;
            ColorTwo = Color.Gray;
        }

        public override void Update(GameTime gameTime)
        {

            if (Input.WasPressed(0, InputHandler.ButtonType.Back, Keys.Escape))
                GameManager.ChangeState(OurGame.TitleState.Value); //go back to title / intro screen

            if (Input.WasPressed(0, InputHandler.ButtonType.Start, Keys.Enter))
            {
                if (Choice == 0)
                {
                    GameManager.PopState(); //got here from our playing state, just pop myself off the stack
                    GameManager.ChangeState(OurGame.PlayingState.Value); //go back to title / intro screen
                }
                else if (Choice == 1)
                {
                    OurGame.Exit();
                }
            }

            if (Input.WasPressed(0, InputHandler.ButtonType.Start, Keys.Up))
            {
                Choice = 0;
            }

            if (Input.WasPressed(0, InputHandler.ButtonType.Start, Keys.Down))
            {
                Choice = 1;
            }

            if (Choice == 0)
            {
                ColorOne = Color.Red;
                ColorTwo = Color.Gray;
            }
            else if (Choice == 1)
            {
                ColorOne = Color.Gray;
                ColorTwo = Color.Red;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            OurGame.sb.Begin();
            OurGame.sb.DrawString(font, "How to play:", new Vector2(100, 50), Color.Yellow);
            OurGame.sb.DrawString(font, "Accelerate and decelerate with W and S", new Vector2(100, 100), Color.BlanchedAlmond);
            OurGame.sb.DrawString(font, "Turn with A and D", new Vector2(100, 150), Color.BlanchedAlmond);
            OurGame.sb.DrawString(font, "Your goal is to dodge the food. If you touch a piece of food, you die", new Vector2(100, 200), Color.BlanchedAlmond);
            OurGame.sb.DrawString(font, "You earn points by surviving and destroying food", new Vector2(100, 250), Color.BlanchedAlmond);
            OurGame.sb.DrawString(font, "Use the left and right mouse buttons to activate powerups", new Vector2(100, 300), Color.BlanchedAlmond);

            OurGame.sb.DrawString(font, "Use Up and Down to Navigate Menu", new Vector2(100, 400), Color.BlanchedAlmond);
            OurGame.sb.DrawString(font, "Start Game", new Vector2(100, 500), ColorOne);
            OurGame.sb.DrawString(font, "Quit Game", new Vector2(100, 600), ColorTwo);
            OurGame.sb.End();


            base.Draw(gameTime);
        }

        protected override void StateChanged(object sender, EventArgs e)
        {
            base.StateChanged(sender, e);

            if (GameManager.State != this.Value)
                Visible = true;
        }

        protected override void LoadContent()
        {
            font = OurGame.Content.Load<SpriteFont>("Arial");
            base.LoadContent();
        }
    }
}
