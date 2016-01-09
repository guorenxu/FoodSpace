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
    class GreenOnion : Food
    {
        SoundEffect soundEffectthree;
        SoundEffectInstance soundEffectIntancethree;

        public GreenOnion(FoodManager fm, Player playership, Vector2 initiallocation, Vector2 direction, float baserotation, float rotationrate, Game game, List<Food> shotlist)
            : base(fm, playership, initiallocation, direction, baserotation, rotationrate, game, shotlist)
        {
            FoodID = (int)FoodTypes.GreenOnion;

            this.spriteTexture = fm.GreenOnionTexture;
            Scale = 1.0f;
            Orgin = new Vector2(this.spriteTexture.Width / 2, this.spriteTexture.Height / 2);

            this.SpriteTextureData =
    new Color[this.spriteTexture.Width * this.spriteTexture.Height];
            this.spriteTexture.GetData(this.SpriteTextureData);

            soundEffectthree = content.Load<SoundEffect>("Squish");
            soundEffectIntancethree = soundEffectthree.CreateInstance();
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

            Rectangle FoodRect = new Rectangle((int)Location.X - (GameFoodManager.GreenOnionTexture.Height / 2), (int)Location.Y - (GameFoodManager.GreenOnionTexture.Height / 2), GameFoodManager.GreenOnionTexture.Height, GameFoodManager.GreenOnionTexture.Height);
            Rectangle PlayerRect = new Rectangle((int)PlayerShip.Location.X - (PlayerShip.spriteTexture.Width / 2), (int)PlayerShip.Location.Y - (PlayerShip.spriteTexture.Height / 2), PlayerShip.spriteTexture.Width, PlayerShip.spriteTexture.Height);

            if (FoodRect.Intersects(PlayerRect))
            {
                //GameFoodManager.Collision = true;

                if (this.PerPixelCollision2(PlayerShip))
                {
                    if (PlayerShip.ShieldActive)
                    {
                        PlayerShip.Score += 1;
                        ShotList.Add(this);

                        soundEffectIntancethree.Play();
                    }
                    else
                    {
                        GameFoodManager.Collision = true;
                    }
                }
                else
                {
                }
            }
            else
            {
            }
        }
    }
}