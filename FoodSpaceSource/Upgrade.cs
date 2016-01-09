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
    class Upgrade : DrawableSprite2
    {
        public List<Upgrade> ShotList;

        int Duration = 8000;

        //public Vector2 Location;

        public PowerUpManager GamePowerupManager;

        public Player PlayerShip;

        protected Texture2D UpgradeTexture;

        //public float Rotate;

        //public Vector2 Direction;

        public int UpgradeID;

        public Game game;

        public Upgrade(PowerUpManager pm, Player playership, Vector2 initiallocation, Game game, List<Upgrade> shotlist)
            : base(game)
        {
            ShotList = shotlist;
            Location = initiallocation;

            GamePowerupManager = pm;

            this.game = game;

            PlayerShip = playership;
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);

            Duration -= gameTime.ElapsedGameTime.Milliseconds;

            Rectangle PowerupRect = new Rectangle((int)Location.X - (UpgradeTexture.Width / 2), (int)Location.Y - (UpgradeTexture.Height / 2), UpgradeTexture.Width, UpgradeTexture.Height);
            Rectangle PlayerRect = new Rectangle((int)PlayerShip.Location.X - (PlayerShip.spriteTexture.Width / 2), (int)PlayerShip.Location.Y - (PlayerShip.spriteTexture.Height / 2), PlayerShip.spriteTexture.Width, PlayerShip.spriteTexture.Height);

            if (Duration <= 0)
            {
                ShotList.Add(this);
            }

            if (PowerupRect.Intersects(PlayerRect))
            {
                GamePowerupManager.Collision = true;

                if (GamePowerupManager.SlotOne == (int)UpgradeTypes.Empty)
                {
                    GamePowerupManager.SlotOne = UpgradeID;
                }
                else if (GamePowerupManager.SlotTwo == (int)UpgradeTypes.Empty)
                {
                    GamePowerupManager.SlotTwo = UpgradeID;
                }

                ShotList.Add(this);
            }
        }
    }
}