using System.Threading;

namespace Benlai.Zookeeper.Common
{
    /// <summary>
    /// 原子long类型
    /// </summary>
    public class AtomicLong
    {
        private long value;

        public AtomicLong(long initialValue)
        {
            this.value = initialValue;
        }

        public AtomicLong() : this(0)
        {
        }

        public long Get()
        {
            return Interlocked.Read(ref this.value);
        }

        public long AddAndGet(long delta)
        {
            return Interlocked.Add(ref this.value, delta);
        }

        public void Set(long newValue)
        {
            Interlocked.Exchange(ref this.value, newValue);
        }

        public long Increment()
        {
            return Interlocked.Increment(ref this.value);
        }

        public override string ToString()
        {
            return string.Format("{0}", this.value);
        }

        public static AtomicLong operator ++(AtomicLong left)
        {
            left.Increment();
            return left;
        }

        public static bool operator >=(AtomicLong left, long right)
        {
            return left.Get() >= right;
        }

        public static bool operator <=(AtomicLong left, long right)
        {
            return left.Get() <= right;
        }

        public static long operator %(AtomicLong left, long right)
        {
            return left.Get() % right;
        }
    }
}
