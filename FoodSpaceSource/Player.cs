using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using IntroGameLibrary;
using IntroGameLibrary.Sprite2;
using IntroGameLibrary.Util;

namespace Prototype
{
    class Player : DrawableSprite2
    {
        Random rand;

        InputHandler input;

        SpriteFont NewFont;

        Texture2D ShieldTexture;

        Texture2D BeamTexture;

        List<Rectangle> PointsToCheckList;

        SoundEffect soundEffect;
        SoundEffectInstance soundEffectIntance;

        SoundEffect soundEffecttwo;
        SoundEffectInstance soundEffectIntancetwo;

        SoundEffect soundEffectthree;
        SoundEffectInstance soundEffectIntancethree;

        public int Score = 0;
        public int HighScore = 0;

        public bool IsDead = false;

        private int BeamDuration = 0;
        private int BeamDurationBase = 800;
        private bool LightningActive = false;
        private Vector2 LightningLocation = Vector2.Zero;
        private float LightningAngle = 0.0f;

        public bool ShieldActive = false;

        int ShieldDuration = 0;
        int ShieldDurationBase = 5000;

        public int ScoreCooldown = 1000;

        Game game;

        int ThrusterCooldown = 0;
        int ThrusterCooldownBase = 150;

        int AddFoodCooldown = 0;
        //int AddFoodCooldownBase = 500;

        Vector2 CurrentMoveDirection = new Vector2(0.0f, 0.0f);

        public ThrusterManager GameThrusterManager;
        public FoodManager GameFoodManager;
        public PowerUpManager GamePowerupManager;

        public Player(Game game)
            : base(game)
        {
            rand = new Random();

            this.game = game;

            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));

            PointsToCheckList = new List<Rectangle>();
        }

        protected override void LoadContent()
        {
            Location.X = 300.0f;
            Location.Y = 200.0f;

            spriteTexture = content.Load<Texture2D>("Playership");

            ShieldTexture = content.Load<Texture2D>("ActiveShield");

            BeamTexture = content.Load<Texture2D>("Beam");

            NewFont = content.Load<SpriteFont>("NFont");

            soundEffect = content.Load<SoundEffect>("Laser");
            soundEffectIntance = soundEffect.CreateInstance();

            soundEffecttwo = content.Load<SoundEffect>("ShieldEffect");
            soundEffectIntancetwo = soundEffecttwo.CreateInstance();

            soundEffectthree = content.Load<SoundEffect>("Squish");
            soundEffectIntancethree = soundEffectthree.CreateInstance();

            Scale = 1.0f;
            Orgin = new Vector2(this.spriteTexture.Width / 2, this.spriteTexture.Height / 2);

            this.SpriteTextureData =
    new Color[this.spriteTexture.Width * this.spriteTexture.Height];
            this.spriteTexture.GetData(this.SpriteTextureData);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            ThrusterCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            AddFoodCooldown -= gameTime.ElapsedGameTime.Milliseconds;

            ScoreCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (ScoreCooldown <= 0)
            {
                ScoreCooldown += 1000;
                Score++;
            }

            if (Score > HighScore)
            {
                HighScore = Score;
            }

            if (ShieldActive && ShieldDuration > 0)
            {
                ShieldDuration -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                ShieldActive = false;
            }

            if (LightningActive && BeamDuration > 0)
            {
                BeamDuration -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                LightningActive = false;
            }

            if (input.KeyboardState.IsKeyDown(Keys.D))
            {
                Rotate = Rotate + (gameTime.ElapsedGameTime.Milliseconds * 0.15f);
            }
            else if (input.KeyboardState.IsKeyDown(Keys.A))
            {
                Rotate = Rotate - (gameTime.ElapsedGameTime.Milliseconds * 0.15f);
            }

            Vector2 North = new Vector2(0, -1);
            float Radians = MathHelper.ToRadians(Rotate);

            float RadiansLeft = MathHelper.ToRadians(Rotate - 90);
            float RadiansRight = MathHelper.ToRadians(Rotate + 90);

            Matrix RotationMatrix = Matrix.CreateRotationZ(Radians);
            Vector2 MoveDirection = Vector2.Transform(North, RotationMatrix);

            Matrix RotationMatrixLeft = Matrix.CreateRotationZ(RadiansLeft);
            Vector2 MoveDirectionLeft = Vector2.Transform(North, RotationMatrixLeft);

            Matrix RotationMatrixRight = Matrix.CreateRotationZ(RadiansRight);
            Vector2 MoveDirectionRight = Vector2.Transform(North, RotationMatrixRight);


            MoveDirection.Normalize();

            if (input.KeyboardState.IsKeyDown(Keys.W))
            {
                if (ThrusterCooldown <= 0)
                {
                    GameThrusterManager.AddShot(Location - MoveDirection * 30 + MoveDirectionLeft * 9);
                    GameThrusterManager.AddShot(Location - MoveDirection * 30 + MoveDirectionRight * 9);

                    ThrusterCooldown = ThrusterCooldownBase;
                }

                Vector2 TempMoveDirection = CurrentMoveDirection;

                CurrentMoveDirection += MoveDirection * 0.0011f * gameTime.ElapsedGameTime.Milliseconds;

                if (CurrentMoveDirection.Length() > 1)
                {
                    CurrentMoveDirection.Normalize();
                }
            }

            if (input.KeyboardState.IsKeyDown(Keys.S))
            {
                if (ThrusterCooldown <= 0)
                {
                    GameThrusterManager.AddShot(Location - MoveDirection * 30 + MoveDirectionLeft * 9);
                    GameThrusterManager.AddShot(Location - MoveDirection * 30 + MoveDirectionRight * 9);

                    ThrusterCooldown = ThrusterCooldownBase;
                }

                Vector2 TempMoveDirection = CurrentMoveDirection;

                CurrentMoveDirection -= MoveDirection * 0.0005f * gameTime.ElapsedGameTime.Milliseconds;

                if (CurrentMoveDirection.Length() > 1)
                {
                    CurrentMoveDirection.Normalize();
                }
            }

            if (input.MouseState.LeftButton == ButtonState.Pressed)
            {
                if (GamePowerupManager.SlotOne == (int)UpgradeTypes.Shield && ShieldActive != true)
                {
                    ShieldActive = true;
                    ShieldDuration = ShieldDurationBase;

                    GamePowerupManager.SlotOne = (int)UpgradeTypes.Empty;

                    soundEffectIntancetwo.Pitch = 0f;
                    soundEffectIntancetwo.Play();
                }
                else if (GamePowerupManager.SlotOne == (int)UpgradeTypes.Shooter && LightningActive != true)
                {
                    ProcessLightning(Location, new Vector2(input.MouseState.X, input.MouseState.Y));

                    GamePowerupManager.SlotOne = (int)UpgradeTypes.Empty;

                    if (soundEffectIntance.State == SoundState.Stopped)
                    {
                        soundEffectIntance.Pitch = 0f;
                        soundEffectIntance.Play();
                    }
                }
            }

            if (input.MouseState.RightButton == ButtonState.Pressed)
            {
                if (GamePowerupManager.SlotTwo == (int)UpgradeTypes.Shield && ShieldActive != true)
                {
                    ShieldActive = true;
                    ShieldDuration = ShieldDurationBase;

                    GamePowerupManager.SlotTwo = (int)UpgradeTypes.Empty;

                    soundEffectIntancetwo.Pitch = 0f;
                    soundEffectIntancetwo.Play();
                }
                else if (GamePowerupManager.SlotTwo == (int)UpgradeTypes.Shooter && LightningActive != true)
                {
                    ProcessLightning(Location, new Vector2(input.MouseState.X, input.MouseState.Y));

                    GamePowerupManager.SlotTwo = (int)UpgradeTypes.Empty;

                    if (soundEffectIntance.State == SoundState.Stopped)
                    {
                        soundEffectIntance.Pitch = 0f;
                        soundEffectIntance.Play();
                    }
                }
            }

            //if (input.KeyboardState.IsKeyDown(Keys.T))
            //{
            //    if (AddFoodCooldown <= 0)
            //    {
            //        GameFoodManager.AddShot();
            //        AddFoodCooldown += AddFoodCooldownBase;
            //    }
            //}


            Location = Location + (CurrentMoveDirection * gameTime.ElapsedGameTime.Milliseconds * 0.20f);
            //else if (input.KeyboardState.IsKeyDown(Keys.A))
            //{
            //    Rotate = Rotate - (gameTime.ElapsedGameTime.Milliseconds * 0.1f);
            //}

            if (Location.X < -50.0f)
            {
                Location.X += game.GraphicsDevice.Viewport.Width + 100;
            }

            if (Location.X > game.GraphicsDevice.Viewport.Width + 50.0f)
            {
                Location.X -= game.GraphicsDevice.Viewport.Width + 100;
            }

            if (Location.Y < -50.0f)
            {
                Location.Y += game.GraphicsDevice.Viewport.Height + 100;
            }

            if (Location.Y > game.GraphicsDevice.Viewport.Height + 50.0f)
            {
                Location.Y -= game.GraphicsDevice.Viewport.Height + 100;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);

            Color NewColor = Color.White;

            if (GamePowerupManager.Collision)
            {
                GamePowerupManager.Collision = false;

                //Score = Score + 10;
            }

            if (GameFoodManager.Collision)
            {
                IsDead = true;
            }

            spriteBatch.Begin();

            spriteBatch.DrawString(NewFont, "Score: " + Score.ToString(), new Vector2(100, 80), Color.Blue);
            spriteBatch.DrawString(NewFont, "Highscore: " + HighScore.ToString(), new Vector2(100, 60), Color.Yellow);

            spriteBatch.Draw(spriteTexture,
                new Rectangle((int)Location.X, (int)Location.Y,
                    spriteTexture.Width,
                    spriteTexture.Height),
                null,
                NewColor,
                MathHelper.ToRadians(Rotate),
                new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2),
                SpriteEffects,
                0);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            Color AlphaColor = Color.White;

            AlphaColor.A = (byte)((float)(ShieldDuration / (float)ShieldDurationBase) * (float)255.0f);

            if (ShieldActive)
            {
                spriteBatch.Draw(ShieldTexture,
                new Rectangle((int)Location.X, (int)Location.Y,
                    ShieldTexture.Width,
                    ShieldTexture.Height),
                null,
                AlphaColor,
                MathHelper.ToRadians(0),
                new Vector2(ShieldTexture.Width / 2, ShieldTexture.Height / 2),
                SpriteEffects,
                0);
            }

            if (LightningActive)
            {
                Color newcolor = Color.White;

                newcolor.A = (byte)((float)(BeamDuration / (float)BeamDurationBase) * (float)255.0f);

                spriteBatch.Draw(BeamTexture,
                new Rectangle((int)LightningLocation.X, (int)LightningLocation.Y,
                    BeamTexture.Width,
                    BeamTexture.Height),
                null,
                newcolor,
                LightningAngle,
                new Vector2(BeamTexture.Width / 2, BeamTexture.Height / 2),
                SpriteEffects,
                0);
            }

            spriteBatch.End();
        }

        private void ProcessLightning(Vector2 playerlocation, Vector2 targetlocation)
        {
            LightningActive = true;
            BeamDuration = BeamDurationBase;

            Vector2 normalizeddifference = targetlocation - playerlocation;
            normalizeddifference.Normalize();

            LightningAngle = (float)Math.Atan2(normalizeddifference.X, -normalizeddifference.Y);

            LightningLocation = playerlocation + (1080.0f * normalizeddifference);

            PointsToCheckList.Clear();

            for (int i = 0; i < 200; i++)
            {
                PointsToCheckList.Add(new Rectangle((int)Location.X + (int)((float)i * 8.0f * normalizeddifference.X), (int)Location.Y + (int)((float)i * 8.0f * normalizeddifference.Y), 10, 10));
            }

            Rectangle newrectangle = new Rectangle(0, 0, 0, 0);
            int tempwidth;
            int tempheight;

            foreach (Food fooditem in GameFoodManager.ShotList)
            {
                foreach (Rectangle rectangleitem in PointsToCheckList)
                {
                    newrectangle.X = (int)fooditem.Location.X;
                    newrectangle.Y = (int)fooditem.Location.Y;

                    tempwidth = fooditem.spriteTexture.Width;
                    tempheight = fooditem.spriteTexture.Height;

                    if (tempwidth > tempheight)
                    {
                        newrectangle.Width = fooditem.spriteTexture.Width;
                        newrectangle.Height = fooditem.spriteTexture.Width;
                    }
                    else
                    {
                        newrectangle.Width = fooditem.spriteTexture.Height;
                        newrectangle.Height = fooditem.spriteTexture.Height;
                    }

                    if (rectangleitem.Intersects(newrectangle))
                    {
                        fooditem.ShotList.Add(fooditem);

                        if (Util.InGameArea(fooditem.Location, game.GraphicsDevice.Viewport.Width + 100, game.GraphicsDevice.Viewport.Height + 100))
                        {
                            if (!fooditem.ScoreMarked)
                            {
                                Score += 1;
                            }

                            fooditem.ScoreMarked = true;

                            soundEffectIntancethree.Play();
                        }
                    }
                }
            }
        }
    }
}

