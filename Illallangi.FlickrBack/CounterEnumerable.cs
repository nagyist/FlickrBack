using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Illallangi.FlickrBack
{
    public sealed class CounterEnumerable<T> : IEnumerable<Counter<T>>
    {
        private readonly List<T> currentSource;
        private readonly int currentTotal;

        public CounterEnumerable(IEnumerable<T> source)
        {
            this.currentSource = source.ToList();
            this.currentTotal = this.currentSource.Count();
        }

        public IEnumerator<Counter<T>> GetEnumerator()
        {
            var count = 0;
            foreach (var value in this.Source)
            {
                count ++;
                yield return new Counter<T>(value, count, this.Total);
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            var count = 0;
            foreach (var value in this.Source)
            {
                count++;
                yield return new Counter<T>(value, count, this.Total);
            }
        }

        private IEnumerable<T> Source
        {
            get { return this.currentSource; }
        }

        private int Total
        {
            get { return this.currentTotal; }
        }
    }
}