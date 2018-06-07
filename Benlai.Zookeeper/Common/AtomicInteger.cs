using System.Threading;

namespace Benlai.Zookeeper.Common
{
    /// <summary>
    /// 原子int类型
    /// </summary>
    internal class AtomicInteger
    {
        private int value;

        public AtomicInteger(int initialValue)
        {
            this.value = initialValue;
        }

        public AtomicInteger()
            : this(0)
        {
        }

        public long Get()
        {
            return this.value;
        }


        public long AddAndGet(int delta)
        {
            return Interlocked.Add(ref this.value, delta);
        }

        public void Set(int newValue)
        {
            Interlocked.Exchange(ref this.value, newValue);
        }

        public int Increment()
        {
            return Interlocked.Increment(ref this.value);
        }

        public int GetAndIncrement()
        {
            return Interlocked.Increment(ref this.value) - 1;
        }

        public static AtomicInteger operator ++(AtomicInteger left)
        {
            left.Increment();
            return left;
        }

        public static bool operator >=(AtomicInteger left, long right)
        {
            return left.Get() >= right;
        }

        public static bool operator <=(AtomicInteger left, long right)
        {
            return left.Get() <= right;
        }
    }
}
