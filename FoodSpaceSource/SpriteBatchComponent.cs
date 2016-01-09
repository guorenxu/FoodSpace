using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Prototype
{
    class SpriteBatchComponent : DrawableGameComponent
    {
        protected static SpriteBatch UniversalSpriteBatch;

        public SpriteBatchComponent(Game game):base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            UniversalSpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
