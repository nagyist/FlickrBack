using System;

namespace Illallangi.FlickrLib
{
    public interface IDelayer
    {
        TimeSpan GetDelay();
        void Reset();
    }
}