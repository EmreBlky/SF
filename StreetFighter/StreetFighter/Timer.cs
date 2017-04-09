using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StreetFighter
{
    class Timer
    {
        public bool Enabled;//Timer'ın Çalışır olup olmadığı durumu
        public float frameDelay;//Timer'ın Tick süresi
        private float frameTimer = 0f;

        public delegate void TimerTick();
        public event TimerTick Tick;

        public Timer(float val = 1000)
        {
            Enabled = false;
            frameDelay = val;
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                frameTimer += elapsed;
                if (frameTimer >= frameDelay)
                {
                    Tick();
                    frameTimer = 0f;
                }

            }
        }

    }
}
