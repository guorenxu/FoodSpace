using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using IntroGameLibrary;
using IntroGameLibrary.Sprite;
using IntroGameLibrary.Util;

namespace Prototype
{
    enum FoodTypes {GreenOnion, ChocolateBar, Watermelon};

    enum UpgradeTypes {Shield, Shooter, Empty};

    class Util
    {
        public static float OffscreenExtension = 600.0f;

        public static bool InGameArea(Vector2 Location, int ScreenWidth, int ScreenHeight)
        {
            if (Location.X >= -(OffscreenExtension) && Location.X <= ScreenWidth + OffscreenExtension && Location.Y >= -(OffscreenExtension) && Location.Y <= ScreenHeight + OffscreenExtension)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
