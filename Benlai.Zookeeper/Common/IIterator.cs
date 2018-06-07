using System;
namespace Benlai.Zookeeper.Common
{
    public interface IIterator<out TValue>
    {
        bool HasNext();

        TValue Next();
    }
}
