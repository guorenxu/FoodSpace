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
using IntroGameLibrary.Sprite;
using IntroGameLibrary.Util;

namespace Prototype
{
    class ThrusterManager : DrawableSprite
    {
        List<ThrusterParticle> ShotList;
        List<ThrusterParticle> ToBeRemoved;

        Game MyGame;
        Color NewColor;

        public ThrusterManager(Game game)
            : base(game)
        {
            ShotList = new List<ThrusterParticle>();
            ToBeRemoved = new List<ThrusterParticle>();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteTexture = content.Load<Texture2D>("YellowShot");
            NewColor = new Microsoft.Xna.Framework.Color(Color.Red, 255);

            MyGame = Game;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (ThrusterParticle singleshot in ShotList)
            {
                singleshot.Update(gameTime.ElapsedGameTime.Milliseconds, this.Game);
            }

            foreach (ThrusterParticle singleshot in ToBeRemoved)
            {
                ShotList.Remove(singleshot);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            foreach (ThrusterParticle singleshot in ShotList)
            {
                NewColor.G = (byte)((float)((3000.0f - singleshot.Duration) / 3000.0f) * (float)255.0f);
                NewColor.B = (byte)((float)((3000.0f - singleshot.Duration) / 3000.0f) * (float)255.0f);
                NewColor.A = (byte)((float)(singleshot.Duration / 3000.0f) * (float)255.0f);
                spriteBatch.Draw(spriteTexture, singleshot.Location, NewColor);      
            }

            spriteBatch.End();
        }

        public void AddShot(Vector2 initiallocation)
        {
            ShotList.Add(new ThrusterParticle(this, initiallocation, MyGame, ToBeRemoved));
        }
    }
}
