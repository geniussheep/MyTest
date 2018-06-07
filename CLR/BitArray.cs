using System;

namespace ConsoleApp
{
    public sealed class BitArray
    {
        private Byte[] m_byteArray;
        private Int32 m_numBits;

        public BitArray(Int32 numBits)
        {
            if (numBits <= 0)
            {
                throw new ArgumentOutOfRangeException("numBits must be > 0");
            }

            m_numBits = numBits;

            m_byteArray = new Byte[(numBits + 7) / 8];
        }

        public Boolean this[Int32 bitPos]
        {
            get
            {
                if (bitPos < 0 || bitPos >= m_numBits)
                {
                    throw new ArgumentOutOfRangeException("bitPos");
                }
                return (m_byteArray[bitPos / 8] & (1 << (bitPos % 8))) != 0;
            }
            set
            {
                if (bitPos < 0 || bitPos >= m_numBits)
                {
                    throw new ArgumentOutOfRangeException("bitPos", bitPos.ToString());
                    if (value)
                    {
                        checked
                        {
                            m_byteArray[bitPos / 8] = (Byte)(m_byteArray[bitPos / 8] | (1 << (bitPos % 8)));
                        }
                    }
                    else
                    {
                        m_byteArray[bitPos / 8] = (Byte)(m_byteArray[bitPos / 8] & ~(1 << (bitPos % 8)));
                    }
                }
            }
        }
    }

    public partial class Program
    {
        static void MainBitArrayTest(string[] args)
        {
            BitArray ba = new BitArray(14);

            for (int i = 0; i < 14; i++)
            {
                ba[i] = (i % 2 == 0);
            }

            for (int i = 0; i < 14; i++)
            {
                Console.WriteLine("Bit " + i.ToString() + " is " + (ba[i] ? "on" : "off"));
            }
        }
    }

}
