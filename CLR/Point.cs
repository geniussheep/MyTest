using System;

namespace ConsoleApp
{
    public interface IChangePointBox
    {
        void Change(Int32 x, Int32 y);
    }

    public struct Point : IChangePointBox
    {
        private Int32 p_x, p_y;

        public Point(Int32 x, Int32 y)
        {
            p_x = x;
            p_y = y;
        }

        public override string ToString()
        {
            return String.Format("({0},{1})", p_x, p_y);
        }

        public void Change(int x, int y)
        {
            p_x = x;
            p_y = y;
        }
    }
}
