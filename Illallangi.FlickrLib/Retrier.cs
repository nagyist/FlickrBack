using System;
using System.Collections.Generic;
using System.Threading;

namespace Illallangi.FlickrLib
{
    public sealed class Retrier<TException> : IRetrier where TException : Exception
    {
        private readonly IConfig currentConfig;
        private readonly IDelayer currentDelayer;

        public Retrier(IConfig config, IDelayer delayer)
        {
            this.currentConfig = config;
            this.currentDelayer = delayer;
        }

        private IConfig Config
        {
            get { return this.currentConfig; }
        }

        private IDelayer Delayer
        {
            get { return this.currentDelayer; }
        }

        private IEnumerable<TimeSpan> Delays
        {
            get
            {
                for (var i = 1; this.Config.Retries == -1 || i < this.Config.Retries; i++)
                {
                    yield return this.Delayer.GetDelay();
                }
            }
        }

        public T Retry<T>(Func<T> func)
        {
            foreach (var delay in this.Delays)
            {
                try
                {
                    var result = func();
                    this.Delayer.Reset();
                    return result;
                }
                catch (TException f)
                {
                    Console.Write("{0}, pausing for {1} seconds", f.Message, delay.TotalSeconds);
                    Thread.Sleep(delay);
                    Console.WriteLine(", retrying.");
                }
            }
            this.Delayer.Reset();
            return func();
        }
    }
}