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
    class FoodManager : DrawableSprite
    {
        public List<Food> ShotList;
        public List<Food> ToBeRemoved;

        Game MyGame;
        Color NewColor;

        Random Rand;

        public Texture2D GreenOnionTexture;
        public Texture2D ChocolateBarTexture;
        public Texture2D WatermelonTexture;

        public bool Collision = false;

        public Player PlayerShip;

        int TimeCount = 0;

        int AddGreenOnionCooldown = 0;
        int AddGreenOnionCooldownBase = 500;

        public FoodManager(Game game)
            : base(game)
        {
            ShotList = new List<Food>();
            ToBeRemoved = new List<Food>();

            Rand = new Random();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            GreenOnionTexture = content.Load<Texture2D>("GreenOnion");
            ChocolateBarTexture = content.Load<Texture2D>("ChocolateBar");
            WatermelonTexture = content.Load<Texture2D>("Watermelon");
            NewColor = new Microsoft.Xna.Framework.Color(Color.Red, 255);

            MyGame = Game;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Collision = false;

            foreach (Food singleshot in ShotList)
            {
                singleshot.Update(gameTime);
            }

            foreach (Food singleshot in ToBeRemoved)
            {
                ShotList.Remove(singleshot);
            }

            AddGreenOnionCooldown -= gameTime.ElapsedGameTime.Milliseconds;

            if (AddGreenOnionCooldown <= 0)
            {
                AddGreenOnionCooldown += AddGreenOnionCooldownBase;
                AddShot();
            }

            if (TimeCount >= 120000)
            {
                AddGreenOnionCooldownBase = 150;
            }

            if (TimeCount >= 90000)
            {
                AddGreenOnionCooldownBase = 200;
            }

            if (TimeCount >= 60000)
            {
                AddGreenOnionCooldownBase = 300;
            }

            if (TimeCount >= 45000)
            {
                AddGreenOnionCooldownBase = 400;
            }

            TimeCount += gameTime.ElapsedGameTime.Milliseconds;
        }

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);

            foreach (Food singleshot in ShotList)
            {
                Texture2D ToDraw;

                if (singleshot.FoodID == (int)FoodTypes.GreenOnion)
                {
                    ToDraw = GreenOnionTexture;
                }
                else if (singleshot.FoodID == (int)FoodTypes.ChocolateBar)
                {
                    ToDraw = ChocolateBarTexture;
                }
                else if (singleshot.FoodID == (int)FoodTypes.Watermelon)
                {
                    ToDraw = WatermelonTexture;
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
        }

        public void AddShot()
        {
            Vector2 initiallocation = new Vector2(0.0f, 0.0f);
            Vector2 direction = new Vector2(0.0f, 0.0f);

            int RandomPlusMinus = Rand.Next(0, 2);

            int RandomFoodID = Rand.Next(0, 6);

            int RandomSide = Rand.Next(0, 4);
            int RandomPointOnSegmentX = Rand.Next(0, Game.GraphicsDevice.Viewport.Width + 1);
            int RandomPointOnSegmentY = Rand.Next(0, Game.GraphicsDevice.Viewport.Height + 1);

            int RandomDirectionX = Rand.Next(0, 101);
            int RandomDirectionY = Rand.Next(0, 101);

            float RandomPositive = (float)(Rand.NextDouble());
            float RandomClamped = (float)(Rand.NextDouble() * 2.0 - 1.0);

            float RandomRotation = (float)Rand.Next(0, 360);
            float RandomRotationRate = (float)(Rand.NextDouble() * 0.13f + 0.01f);

            float RandomSpeedFactor = (float)(Rand.NextDouble() * 0.5f + 0.1f);

            if (RandomSide == 0)
            {
                initiallocation.X = -(Util.OffscreenExtension);
                initiallocation.Y = RandomPointOnSegmentY;

                direction.X = RandomPositive;
                direction.Y = RandomClamped;
            }
            else if (RandomSide == 1)
            {
                initiallocation.X = Game.GraphicsDevice.Viewport.Width + (Util.OffscreenExtension);
                initiallocation.Y = RandomPointOnSegmentY;

                direction.X = -(RandomPositive);
                direction.Y = RandomClamped;
            }
            else if (RandomSide == 2)
            {
                initiallocation.X = RandomPointOnSegmentX;
                initiallocation.Y = -(Util.OffscreenExtension);

                direction.X = RandomClamped;
                direction.Y = RandomPositive;
            }
            else
            {
                initiallocation.X = RandomPointOnSegmentX;
                initiallocation.Y = Game.GraphicsDevice.Viewport.Height + (Util.OffscreenExtension);

                direction.X = RandomClamped;
                direction.Y = -(RandomPositive);
            }

            if (RandomPlusMinus == 0)
            {
                RandomRotationRate *= -1;
            }

            direction.Normalize();

            direction *= RandomSpeedFactor;

            if (RandomFoodID == 0 || RandomFoodID == 3)
            {
                if (TimeCount >= 15000)
                {
                    ShotList.Add(new GreenOnion(this, PlayerShip, initiallocation, direction, RandomRotation, RandomRotationRate, MyGame, ToBeRemoved));
                }
                //ShotList.Add(new ChocolateBarFood(this, PlayerShip, new Vector2(300, 300), direction * 0.004f, 0.0f, 0.0f, MyGame, ToBeRemoved));
            }
            else if (RandomFoodID == 1 || RandomFoodID == 2)
            {
                ShotList.Add(new ChocolateBarFood(this, PlayerShip, initiallocation, direction * 0.4f, RandomRotation, RandomRotationRate * 0.4f, MyGame, ToBeRemoved));
                //ShotList.Add(new ChocolateBarFood(this, PlayerShip, new Vector2(300, 300), direction * 0.004f, 0.0f, 0.0f, MyGame, ToBeRemoved));
            }
            else if (RandomFoodID == 4 && TimeCount >= 30000)
            {
                ShotList.Add(new GreenOnion(this, PlayerShip, initiallocation, direction * 1.2f, RandomRotation, RandomRotationRate * 1.2f, MyGame, ToBeRemoved));
            }
            else if (RandomFoodID == 5 && TimeCount >= 45000)
            {
                ShotList.Add(new Watermelon(this, PlayerShip, initiallocation, direction * 0.3f, RandomRotation, RandomRotationRate * 0.3f, MyGame, ToBeRemoved));
            }
        }
    }
}
