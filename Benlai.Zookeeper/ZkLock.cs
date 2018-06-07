using Spring.Threading.Locks;

namespace Benlai.Zookeeper
{
    public sealed class ZkLock : ReentrantLock
    {
        public ICondition DataChangedCondition { get; private set; }

        public ICondition StateChangedCondition { get; private set; }

        public ICondition ZNodeEventCondition { get; private set; }

        public ZkLock()
        {
            this.DataChangedCondition = this.NewCondition();
            this.StateChangedCondition = this.NewCondition();
            this.ZNodeEventCondition = this.NewCondition();
        }
    }
}
