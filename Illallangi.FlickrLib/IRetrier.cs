using System;

namespace Illallangi.FlickrLib
{
    public interface IRetrier
    {
        T Retry<T>(Func<T> func);
    }
}