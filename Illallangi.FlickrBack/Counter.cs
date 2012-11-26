namespace Illallangi.FlickrBack
{
    public sealed class Counter<T>
    {
        private readonly T currentValue;
        private readonly int currentCount;
        private readonly int currentTotal;

        public Counter(T value, int count, int total)
        {
            this.currentValue = value;
            this.currentCount = count;
            this.currentTotal = total;
        }

        public T Value
        {
            get { return this.currentValue; }
        }

        public int Count
        {
            get { return this.currentCount; }
        }

        public int Total
        {
            get { return this.currentTotal; }
        }
    }
}
