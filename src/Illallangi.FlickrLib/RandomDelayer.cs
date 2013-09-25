using System;

namespace Illallangi.FlickrLib
{
    public sealed class RandomDelayer : IDelayer
    {
        private Random currentRandom;
        private readonly IConfig currentConfig;

        public RandomDelayer(IConfig config)
        {
            this.currentConfig = config;
        }

        private Random Random
        {
            get { return this.currentRandom ?? (this.currentRandom = new Random()); }
        }

        private IConfig Config
        {
            get { return this.currentConfig; }
        }

        public TimeSpan GetDelay()
        {
            return TimeSpan.FromSeconds(this.Random.Next(this.Config.MinDelay, this.Config.MaxDelay));
        }

        public void Reset()
        {
            // NOOP
        }
    }
}