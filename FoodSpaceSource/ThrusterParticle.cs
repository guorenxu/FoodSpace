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

namespace Prototype
{
    class ThrusterParticle
    {
        List<ThrusterParticle> ShotList;

        public Vector2 Location;

        ThrusterManager GameShotManager;

        public int Duration = 3000;

        public ThrusterParticle(ThrusterManager sm, Vector2 initiallocation, Game game, List<ThrusterParticle> shotlist)
        {
            Location = initiallocation;

            ShotList = shotlist;
            Location = initiallocation;

            GameShotManager = sm;
        }

        public void Update(int deltatime, Game game)
        {
            //Remove from list using proper method
            Duration = Duration - deltatime;

            if (Duration <= 0)
            {
                ShotList.Add(this);
            }  
        }
    }
}

