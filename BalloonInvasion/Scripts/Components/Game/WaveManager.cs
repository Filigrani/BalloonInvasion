using BalloonInvasion.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonInvasion
{
    internal class WaveManager : Component
    {
        public bool IsWaveActive = true;
        public TimeSpan NextBalloon = new TimeSpan();
        public float SpawnRate = 1.1f;

        public float GetSpawnRate()
        {
            System.Random RNG = new System.Random();
            return SpawnRate * RNG.Next(1, 4);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsWaveActive)
            {
                if(gameTime.TotalGameTime > NextBalloon)
                {
                    NextBalloon = gameTime.TotalGameTime + TimeSpan.FromSeconds(GetSpawnRate());
                    GameManager.CreateBalloon();
                }
            }
        }
    }
}
