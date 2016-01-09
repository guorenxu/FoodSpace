using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using IntroGameLibrary;
using IntroGameLibrary.Sprite;
using IntroGameLibrary.Util;
using IntroGameLibrary.State;

namespace Prototype
{
    class PowerUpManager : DrawableSprite
    {
        List<Upgrade> ShotList;
        List<Upgrade> ToBeRemoved;

        InputHandler input;

        Game MyGame;
        Color NewColor;

        Vector2 LocationOne;
        Vector2 LocationTwo;

        public int SlotOne = (int)UpgradeTypes.Empty;
        public int SlotTwo = (int)UpgradeTypes.Empty;

        Random Rand;

        public Texture2D UpgradeBoxTexture;
        public Texture2D ShieldUpgradeTexture;
        public Texture2D ShooterUpgradeTexture;

        public Texture2D ReticleTexture;
        public Texture2D AimDotTexture;

        public Player PlayerShip;

        int AddUpgradeCooldown = 7000;
        int AddUpgradeBase = 7000;

        int TimeCount = 0;

        public bool Collision = false;

        public PowerUpManager(Game game)
            : base(game)
        {
            ShotList = new List<Upgrade>();
            ToBeRemoved = new List<Upgrade>();

            Rand = new Random();

            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            UpgradeBoxTexture = content.Load<Texture2D>("Skillbox");
            ShieldUpgradeTexture = content.Load<Texture2D>("Shield");
            ShooterUpgradeTexture = content.Load<Texture2D>("Shooter");

            ReticleTexture = content.Load<Texture2D>("Reticle");
            AimDotTexture = content.Load<Texture2D>("AimDots");

            NewColor = new Microsoft.Xna.Framework.Color(Color.Red, 255);

            LocationOne = new Vector2(800, 75);
            LocationTwo = new Vector2(950, 75);

            MyGame = Game;
        }

        public override void Update(GameTime gameTime)
        {
            TimeCount += gameTime.ElapsedGameTime.Milliseconds;

            base.Update(gameTime);

            if (TimeCount >= 130000)
            {
                AddUpgradeBase = 30000;
            }

            if (TimeCount >= 90000)
            {
                AddUpgradeBase = 15000;
            }

            if (TimeCount >= 60000)
            {
                AddUpgradeBase = 12000;
            }

            if (TimeCount >= 30000)
            {
                AddUpgradeBase = 10000;
            }

            foreach (Upgrade singleshot in ShotList)
            {
                singleshot.Update(gameTime);
            }

            foreach (Upgrade singleshot in ToBeRemoved)
            {
                ShotList.Remove(singleshot);
            }

            AddUpgradeCooldown -= gameTime.ElapsedGameTime.Milliseconds;

            if (AddUpgradeCooldown <= 0)
            {
                AddUpgradeCooldown += AddUpgradeBase;
                AddShot();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);

            foreach (Upgrade singleshot in ShotList)
            {
                Texture2D ToDraw;

                if (singleshot.UpgradeID == (int)UpgradeTypes.Shield)
                {
                    ToDraw = ShieldUpgradeTexture;
                }
                else if (singleshot.UpgradeID == (int)UpgradeTypes.Shooter)
                {
                    ToDraw = ShooterUpgradeTexture;
                }
                else
                {
                    ToDraw = null;
                }

                spriteBatch.Begin();
                spriteBatch.Draw(ToDraw,
                    new Rectangle((int)singleshot.Location.X, (int)singleshot.Location.Y,
                        ToDraw.Width,
                        ToDraw.Height),
                    null,
                    Color.White,
                    MathHelper.ToRadians(singleshot.Rotate),
                    new Vector2(ToDraw.Width / 2, ToDraw.Height / 2),
                    SpriteEffects,
                    0);

                spriteBatch.End();
            }

            spriteBatch.Begin();

            spriteBatch.Draw(UpgradeBoxTexture,
                new Rectangle((int)LocationOne.X, (int)LocationOne.Y,
                    UpgradeBoxTexture.Width,
                    UpgradeBoxTexture.Height),
                null,
                Color.White,
                MathHelper.ToRadians(0),
                new Vector2(UpgradeBoxTexture.Width / 2, UpgradeBoxTexture.Height / 2),
                SpriteEffects,
                0);

            spriteBatch.Draw(UpgradeBoxTexture,
                new Rectangle((int)LocationTwo.X, (int)LocationTwo.Y,
                    UpgradeBoxTexture.Width,
                    UpgradeBoxTexture.Height),
                null,
                Color.White,
                MathHelper.ToRadians(0),
                new Vector2(UpgradeBoxTexture.Width / 2, UpgradeBoxTexture.Height / 2),
                SpriteEffects,
                0);

            Texture2D UpgradeToDrawOne = null;
            Texture2D UpgradeToDrawTwo = null;

            if (SlotOne == (int)UpgradeTypes.Shield)
            {
                UpgradeToDrawOne = ShieldUpgradeTexture;
            }
            else if (SlotOne == (int)UpgradeTypes.Shooter)
            {
                UpgradeToDrawOne = ShooterUpgradeTexture;
            }

            if (SlotTwo == (int)UpgradeTypes.Shield)
            {
                UpgradeToDrawTwo = ShieldUpgradeTexture;
            }
            else if (SlotTwo == (int)UpgradeTypes.Shooter)
            {
                UpgradeToDrawTwo = ShooterUpgradeTexture;
            }

            if (UpgradeToDrawOne != null)
            {
                spriteBatch.Draw(UpgradeToDrawOne,
                    new Rectangle((int)LocationOne.X, (int)LocationOne.Y,
                        UpgradeToDrawOne.Width,
                        UpgradeToDrawOne.Height),
                    null,
                    Color.White,
                    MathHelper.ToRadians(0),
                    new Vector2(UpgradeToDrawOne.Width / 2, UpgradeToDrawOne.Height / 2),
                    SpriteEffects,
                    0);
            }

            if (UpgradeToDrawTwo != null)
            {
                spriteBatch.Draw(UpgradeToDrawTwo,
                      new Rectangle((int)LocationTwo.X, (int)LocationTwo.Y,
                      UpgradeToDrawTwo.Width,
                      UpgradeToDrawTwo.Height),
                    null,
                 Color.White,
                 MathHelper.ToRadians(0),
                 new Vector2(UpgradeToDrawTwo.Width / 2, UpgradeToDrawTwo.Height / 2),
                 SpriteEffects,
                 0);
            }

            if (SlotOne == (int)UpgradeTypes.Shooter || SlotTwo == (int)UpgradeTypes.Shooter)
            {
                int mousex = input.MouseState.X;
                int mousey = input.MouseState.Y;

                Vector2 mousevector = new Vector2(mousex, mousey);

                spriteBatch.Draw(ReticleTexture,
                  new Rectangle((int)mousex, (int)mousey,
                  ReticleTexture.Width,
                  ReticleTexture.Height),
                    null,
                 Color.White,
                 MathHelper.ToRadians(0),
                 new Vector2(ReticleTexture.Width / 2, ReticleTexture.Height / 2),
                 SpriteEffects,
                 0);

                Vector2 normalizedmousetoship = new Vector2(mousex - PlayerShip.Location.X, mousey - PlayerShip.Location.Y);
                normalizedmousetoship.Normalize();

                if (normalizedmousetoship.X > 0.0f && normalizedmousetoship.Y > 0.0f)
                {
                    mousevector -= 20.0f * normalizedmousetoship;

                    while (mousevector.X > PlayerShip.Location.X && mousevector.Y > PlayerShip.Location.Y)
                    {
                        spriteBatch.Draw(AimDotTexture,
                            new Rectangle((int)mousevector.X, (int)mousevector.Y,
                            AimDotTexture.Width,
                        AimDotTexture.Height),
                            null,
                         Color.White,
                         MathHelper.ToRadians(0),
                         new Vector2(AimDotTexture.Width / 2, AimDotTexture.Height / 2),
                         SpriteEffects,
                         0);

                        mousevector -= 20.0f * normalizedmousetoship;
                    }
                }
                else if (normalizedmousetoship.X > 0.0f && normalizedmousetoship.Y < 0.0f)
                {
                    mousevector -= 20.0f * normalizedmousetoship;

                    while (mousevector.X > PlayerShip.Location.X && mousevector.Y < PlayerShip.Location.Y)
                    {
                        spriteBatch.Draw(AimDotTexture,
                            new Rectangle((int)mousevector.X, (int)mousevector.Y,
                            AimDotTexture.Width,
                        AimDotTexture.Height),
                            null,
                         Color.White,
                         MathHelper.ToRadians(0),
                         new Vector2(AimDotTexture.Width / 2, AimDotTexture.Height / 2),
                         SpriteEffects,
                         0);

                        mousevector -= 20.0f * normalizedmousetoship;
                    }
                }
                else if (normalizedmousetoship.X < 0.0f && normalizedmousetoship.Y > 0.0f)
                {
                    mousevector -= 20.0f * normalizedmousetoship;

                    while (mousevector.X < PlayerShip.Location.X && mousevector.Y > PlayerShip.Location.Y)
                    {
                        spriteBatch.Draw(AimDotTexture,
                            new Rectangle((int)mousevector.X, (int)mousevector.Y,
                            AimDotTexture.Width,
                        AimDotTexture.Height),
                            null,
                         Color.White,
                         MathHelper.ToRadians(0),
                         new Vector2(AimDotTexture.Width / 2, AimDotTexture.Height / 2),
                         SpriteEffects,
                         0);

                        mousevector -= 20.0f * normalizedmousetoship;
                    }
                }
                else if (normalizedmousetoship.X < 0.0f && normalizedmousetoship.Y < 0.0f)
                {
                    mousevector -= 20.0f * normalizedmousetoship;

                    while (mousevector.X < PlayerShip.Location.X && mousevector.Y < PlayerShip.Location.Y)
                    {
                        spriteBatch.Draw(AimDotTexture,
                            new Rectangle((int)mousevector.X, (int)mousevector.Y,
                            AimDotTexture.Width,
                        AimDotTexture.Height),
                            null,
                         Color.White,
                         MathHelper.ToRadians(0),
                         new Vector2(AimDotTexture.Width / 2, AimDotTexture.Height / 2),
                         SpriteEffects,
                         0);

                        mousevector -= 20.0f * normalizedmousetoship;
                    }
                }
            }

            spriteBatch.End();
        }

        public void AddShot()
        {
            Vector2 initiallocation = new Vector2(0.0f, 0.0f);

            initiallocation.X = (float)Rand.Next(100, Game.GraphicsDevice.Viewport.Width - 100);
            initiallocation.Y = (float)Rand.Next(100, Game.GraphicsDevice.Viewport.Height - 100);

            int tempint = Rand.Next(0, 2);

            if (tempint == 0)
            {
                ShotList.Add(new ShieldUpgrade(this, PlayerShip, initiallocation, MyGame, ToBeRemoved));
            }
            else if (tempint == 1)
            {
                ShotList.Add(new ShooterUpgrade(this, PlayerShip, initiallocation, MyGame, ToBeRemoved));
            }
        }
    }
}
