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
    class ShieldUpgrade : Upgrade
    {
        public ShieldUpgrade(PowerUpManager pm, Player playership, Vector2 initiallocation, Game game, List<Upgrade> shotlist)
            : base(pm, playership, initiallocation, game, shotlist)
        {
            UpgradeID = (int)UpgradeTypes.Shield;

            UpgradeTexture = pm.ShieldUpgradeTexture;
        }
    }
}
