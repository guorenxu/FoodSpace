#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using IntroGameLibrary;
using IntroGameLibrary.Util;
using IntroGameLibrary.State;
#endregion

namespace Prototype
{
    public interface IPlayingState : IGameState { }

    class PlayingState : BaseGameState, IPlayingState
    {
        Player PlayerShip;
        ThrusterManager GameThrusterManager;
        FoodManager GameFoodManager;
        PowerUpManager GamePowerupManager;

        SoundEffect soundEffect;
        SoundEffectInstance soundEffectIntance;

        public PlayingState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IPlayingState), this);

            PlayerShip = new Player(OurGame);
            GameThrusterManager = new ThrusterManager(OurGame);

            PlayerShip.GameThrusterManager = GameThrusterManager;

            GameFoodManager = new FoodManager(OurGame);
            PlayerShip.GameFoodManager = GameFoodManager;
            GameFoodManager.PlayerShip = PlayerShip;

            GamePowerupManager = new PowerUpManager(OurGame);
            PlayerShip.GamePowerupManager = GamePowerupManager;
            GamePowerupManager.PlayerShip = PlayerShip;

            OurGame.Components.Add(PlayerShip);
            OurGame.Components.Add(GameThrusterManager);
            OurGame.Components.Add(GameFoodManager);
            OurGame.Components.Add(GamePowerupManager);

            PlayerShip.Enabled = false;
            GameFoodManager.Enabled = false;
            GameThrusterManager.Enabled = false;
            GamePowerupManager.Enabled = false;
            PlayerShip.Visible = false;
            GameFoodManager.Visible = false;
            GameThrusterManager.Visible = false;
            GamePowerupManager.Visible = false;

            soundEffect = Content.Load<SoundEffect>("Music");
            soundEffectIntance = soundEffect.CreateInstance();
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.WasPressed(0, InputHandler.ButtonType.Back, Keys.Escape))
                GameManager.PushState(OurGame.PausedState.Value);

            if (PlayerShip.IsDead == true)
            {
                GameManager.PushState(OurGame.EndState.Value);
            }

            //TODO add pausedstate

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }

        protected override void StateChanged(object sender, EventArgs e)
        {
            base.StateChanged(sender, e);

            //handled change to paused state
            if (GameManager.State == OurGame.PausedState)
            {
                //just set enabled to false;
                this.Enabled = false;
                PlayerShip.Enabled = false;
                GameFoodManager.Enabled = false;
                GameThrusterManager.Enabled = false;
                GamePowerupManager.Enabled = false;
                PauseMusic();
            }
            else if (GameManager.State == OurGame.EndState)
            {
                if (PlayerShip.IsDead == true)
                {
                    Reset();
                }
            }
            else if (GameManager.State != this.Value)
            {
                //change to any other state
                Visible = true;
                Enabled = false;
                //Call Load or add components
                PlayerShip.Enabled = false;
                GameFoodManager.Enabled = false;
                GameThrusterManager.Enabled = false;
                GamePowerupManager.Enabled = false;
                StopMusic();

            }
            else
            {
                PlayerShip.Enabled = true;
                GameFoodManager.Enabled = true;
                GameThrusterManager.Enabled = true;
                GamePowerupManager.Enabled = true;
                //Call Unload or remove components
                PlayerShip.Visible = true;
                GameFoodManager.Visible = true;
                GameThrusterManager.Visible = true;
                GamePowerupManager.Visible = true;

                PlayMusic();
            }



        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public void Reset()
        {
            OurGame.Components.Remove(PlayerShip);
            OurGame.Components.Remove(GameThrusterManager);
            OurGame.Components.Remove(GameFoodManager);
            OurGame.Components.Remove(GamePowerupManager);

            int highscore = PlayerShip.HighScore;

            PlayerShip = new Player(OurGame);
            GameThrusterManager = new ThrusterManager(OurGame);

            PlayerShip.GameThrusterManager = GameThrusterManager;

            GameFoodManager = new FoodManager(OurGame);
            PlayerShip.GameFoodManager = GameFoodManager;
            GameFoodManager.PlayerShip = PlayerShip;

            GamePowerupManager = new PowerUpManager(OurGame);
            PlayerShip.GamePowerupManager = GamePowerupManager;
            GamePowerupManager.PlayerShip = PlayerShip;

            OurGame.Components.Add(PlayerShip);
            OurGame.Components.Add(GameThrusterManager);
            OurGame.Components.Add(GameFoodManager);
            OurGame.Components.Add(GamePowerupManager);

            PlayerShip.Enabled = false;
            GameFoodManager.Enabled = false;
            GameThrusterManager.Enabled = false;
            GamePowerupManager.Enabled = false;
            PlayerShip.Visible = false;
            GameFoodManager.Visible = false;
            GameThrusterManager.Visible = false;
            GamePowerupManager.Visible = false;

            PlayerShip.HighScore = highscore;
        }

        public void PlayMusic()
        {
                soundEffectIntance.Stop();
                soundEffectIntance.Play();
        }

        public void StopMusic()
        {
            soundEffectIntance.Stop();
        }

        public void PauseMusic()
        {
            soundEffectIntance.Pause();
        }
    }
}
