using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrConsoleAppFx
{
    public class Pixel
    {
        public Pixel(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Pixel p = (Pixel)obj;
                return (X == p.X) && (Y == p.Y);
            }
        }
        public override int GetHashCode()
        {
            return (X << 2) ^ Y;
        }
    }
}
