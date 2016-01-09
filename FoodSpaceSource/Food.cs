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

namespace Prototype
{
    class Food : DrawableSprite2
    {
        public List<Food> ShotList;

        //public Vector2 Location;

        public FoodManager GameFoodManager;

        public Player PlayerShip;

        //public float Rotate;

        public float RotationRate;

        //public Vector2 Direction;

        public int FoodID;

        public bool ScoreMarked = false;

        public Game game;

        public Food(FoodManager fm, Player playership, Vector2 initiallocation, Vector2 direction, float baserotation, float rotationrate, Game game, List<Food> shotlist)
            : base(game)
        {
            Location = initiallocation;
            Direction = direction;

            ShotList = shotlist;
            Location = initiallocation;

            GameFoodManager = fm;

            this.game = game;

            Rotate = baserotation;

            RotationRate = rotationrate;

            PlayerShip = playership;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Rotate = Rotate + gameTime.ElapsedGameTime.Milliseconds * RotationRate;

            Location = Location + Direction * (gameTime.ElapsedGameTime.Milliseconds * 0.25f);

            if (!Util.InGameArea(Location, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height))
            {
                ShotList.Add(this);
            }

            Rectangle FoodRect = new Rectangle((int)Location.X - (GameFoodManager.GreenOnionTexture.Width / 2), (int)Location.Y - (GameFoodManager.GreenOnionTexture.Height / 2), GameFoodManager.GreenOnionTexture.Width, GameFoodManager.GreenOnionTexture.Height);
            Rectangle PlayerRect = new Rectangle((int)PlayerShip.Location.X - (PlayerShip.spriteTexture.Width / 2), (int)PlayerShip.Location.Y - (PlayerShip.spriteTexture.Height / 2), PlayerShip.spriteTexture.Width, PlayerShip.spriteTexture.Height);

            if (FoodRect.Intersects(PlayerRect))
            {
                //GameFoodManager.Collision = true;
            }
        }
    }
}