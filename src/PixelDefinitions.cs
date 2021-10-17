using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrConsoleAppFx
{
    public class PixelDefinitions
    {
        public static readonly List<Pixel> char0 = new List<Pixel>() {

        new Pixel(2,3),
        new Pixel(3,2),
        new Pixel(3,10),
        new Pixel(4,6),
        new Pixel(4,11),
        new Pixel(7,5),
        new Pixel(7,6),
        new Pixel(7,7)
        };


        public static readonly List<Pixel> char0Not = new List<Pixel>() {

        new Pixel(4,3),
        new Pixel(3,4),
        new Pixel(3,5),
        new Pixel(5,7),
        new Pixel(8,6),
        new Pixel(8,7)
        };


        public static readonly List<Pixel> char1 = new List<Pixel>() {

        new Pixel(2,2),
        new Pixel(4,2),
        new Pixel(4,3),
        new Pixel(4,4),
        new Pixel(4,5),
        new Pixel(4,6),
        new Pixel(4,7),
        new Pixel(4,8),
        new Pixel(4,9),
        new Pixel(4,10),
        new Pixel(5,11)
        };

        public static readonly List<Pixel> char1Not = new List<Pixel>() {

        new Pixel(3,3)
        };

        public static readonly List<Pixel> char2 = new List<Pixel>() {

        new Pixel(2,10),
        new Pixel(3,11),
        new Pixel(4,8),
        new Pixel(5,7),
        new Pixel(7,4)
        };

        public static readonly List<Pixel> char2Not = new List<Pixel>() {

        new Pixel(5,9),
        new Pixel(6,8),
        new Pixel(7,7)
        };

        public static readonly List<Pixel> char3 = new List<Pixel>() {

        new Pixel(7,3),
        new Pixel(7,4),
        new Pixel(7,8),
        };

        public static readonly List<Pixel> char3or8 = new List<Pixel>() {

        new Pixel(6,2),
        new Pixel(7,3),
        new Pixel(7,8),
        };

        public static readonly List<Pixel> char3or8Not = new List<Pixel>() {

        new Pixel(3,3),
        new Pixel(4,7),
        new Pixel(7,11)
        };

        public static readonly List<Pixel> char4 = new List<Pixel>() {

        new Pixel(1,8),
        new Pixel(4,3),
        new Pixel(6,2),
        new Pixel(6,3),
        new Pixel(6,4),
        new Pixel(6,5),
        new Pixel(6,6),
        new Pixel(6,7),
        new Pixel(6,8),
        new Pixel(6,9),
        new Pixel(6,10),
        new Pixel(7,8)
        };

        public static readonly List<Pixel> char4Not = new List<Pixel>() {

        new Pixel(3,7),
        new Pixel(5,4),
        new Pixel(7,3),
        new Pixel(7,4),
        new Pixel(7,5),
        new Pixel(7,6),
        new Pixel(7,7)
        };

        public static readonly List<Pixel> char5 = new List<Pixel>() {

        new Pixel(2,2),
        new Pixel(2,3),
        new Pixel(2,4),
        new Pixel(2,5),
        new Pixel(3,5),
        new Pixel(4,5),
        new Pixel(6,6),
        new Pixel(7,7),
        new Pixel(7,8)
        };

        public static readonly List<Pixel> char5Not = new List<Pixel>() {

        new Pixel(3,3),
        new Pixel(3,4),
        new Pixel(3,6),
        new Pixel(4,6),
        new Pixel(7,3)
        };

        public static readonly List<Pixel> char6 = new List<Pixel>() {

        new Pixel(3,2),
        new Pixel(4,5),
        new Pixel(5,5),
        new Pixel(6,6),
        new Pixel(7,7),
        new Pixel(7,8)
        };

        public static readonly List<Pixel> char6Not = new List<Pixel>() {

        new Pixel(3,4),
        new Pixel(3,7),
        new Pixel(4,3),
        new Pixel(5,6),
        };


        public static readonly List<Pixel> char7 = new List<Pixel>() {

        new Pixel(4,8),
        new Pixel(5,6),
        new Pixel(6,4)
        };

        public static readonly List<Pixel> char7Not = new List<Pixel>() {

        new Pixel(6,7),
        new Pixel(7,5),
        new Pixel(5,9)
        };


        public static readonly List<Pixel> char8 = new List<Pixel>() {

        new Pixel(3,6),
        new Pixel(3,10),
        new Pixel(4,11),
        new Pixel(6,2),
        new Pixel(7,3),
        new Pixel(7,8),
        new Pixel(7,9),
        new Pixel(2,7)
        };


        public static readonly List<Pixel> char9 = new List<Pixel>() {

        new Pixel(2,6),
        new Pixel(3,7),
        new Pixel(4,7),
        new Pixel(5,7),
        new Pixel(7,5),
        new Pixel(7,6),
        new Pixel(7,7)
        };

        public static readonly List<Pixel> char9Not = new List<Pixel>() {

        new Pixel(3,4),
        new Pixel(4,3),
        new Pixel(5,8)
        };

        public static readonly List<Pixel> leftBracket = new List<Pixel>() {

        new Pixel(4,1),
        new Pixel(4,2),
        new Pixel(4,3),
        new Pixel(4,4),
        new Pixel(4,5),
        new Pixel(4,6),
        new Pixel(4,7),
        new Pixel(4,8),
        new Pixel(4,9),
        new Pixel(4,10),
        new Pixel(4,11),
        new Pixel(4,12),
        new Pixel(5,1),
        new Pixel(5,12)
        };

        public static readonly List<Pixel> leftBracketNot = new List<Pixel>()
        {
        };

        public static readonly List<Pixel> dot = new List<Pixel>() {

        new Pixel(4,10)
        };


        public static readonly List<Pixel> dotNot = new List<Pixel>() {
        };

        public static readonly List<Pixel> comma = new List<Pixel>() {

        new Pixel(4,10),
        new Pixel(4,11)
        };

        public static readonly List<Pixel> commaNot = new List<Pixel>() {

        new Pixel(5,11),
        new Pixel(5,12)
        };
    }
}
