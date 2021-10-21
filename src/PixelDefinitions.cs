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
        new Pixel(2,9),
        //Pixel(3,2),
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

        //new Pixel(2,2),
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

        new Pixel(3,3),
        new Pixel(3,12),
        new Pixel(4,12),
        new Pixel(5,3),
        new Pixel(5,4),
        new Pixel(5,5),
        new Pixel(5,6),
        new Pixel(5,7),
        new Pixel(5,8),
        new Pixel(5,9),
        new Pixel(5,12),
        new Pixel(6,12),
        new Pixel(7,12),
        };

        public static readonly List<Pixel> char2 = new List<Pixel>() {

        new Pixel(2,2),
        new Pixel(2,10),
        new Pixel(3,9),
        new Pixel(3,11),
        new Pixel(4,8),
        //new Pixel(5,7),
        new Pixel(7,4)
        };

        public static readonly List<Pixel> char2Not = new List<Pixel>() {

        new Pixel(3,3),
        new Pixel(4,3),
        new Pixel(5,3),
        new Pixel(5,9),
        new Pixel(6,8),
        new Pixel(7,7),
        new Pixel(8,4),
        new Pixel(8,5),
        new Pixel(8,6),
        new Pixel(3,12),
        new Pixel(4,12),
        new Pixel(5,12),
        new Pixel(6,12),
        new Pixel(7,12),
        new Pixel(8,12),
        };

        public static readonly List<Pixel> char3 = new List<Pixel>() {

        new Pixel(7,3),
        new Pixel(7,4),
        new Pixel(7,8),
        };

        public static readonly List<Pixel> char3Not = new List<Pixel>() {

        new Pixel(3,3),
        new Pixel(4,3),
        new Pixel(5,3),
        new Pixel(4,7),
        new Pixel(5,7),
        new Pixel(5,12),
        new Pixel(7,6),
        new Pixel(7,11),
        new Pixel(8,4),
        new Pixel(8,5),
        new Pixel(8,8),
        new Pixel(8,9),
        new Pixel(8,10)

        };

        public static readonly List<Pixel> char3or8 = new List<Pixel>() {

        new Pixel(2,2),
        new Pixel(3,2),
        new Pixel(4,11),
        new Pixel(6,2),
        new Pixel(6,6),
        new Pixel(6,10),
        new Pixel(7,3),
        new Pixel(7,7),
        new Pixel(7,8),
        new Pixel(7,9),

        };

        public static readonly List<Pixel> char3or8Not = new List<Pixel>() {


        new Pixel(3,3),
        new Pixel(4,3),
        new Pixel(4,7),
        new Pixel(5,12), 
        new Pixel(7,11),
        new Pixel(8,4),
        new Pixel(8,5),
        new Pixel(8,8),
        new Pixel(8,9),
        new Pixel(8,10),
        };

        public static readonly List<Pixel> char4 = new List<Pixel>() {

        new Pixel(1,8),
        new Pixel(2,6),
        new Pixel(2,8),
        new Pixel(3,8),
        new Pixel(4,3),
        new Pixel(4,8),
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
        new Pixel(4,5),
        new Pixel(4,6),
        new Pixel(5,4),
        new Pixel(7,3),
        new Pixel(7,4),
        new Pixel(7,5),
        new Pixel(7,6),
        new Pixel(7,7),
        new Pixel(7,10),
        new Pixel(7,11),
        new Pixel(8,9)
        };

        public static readonly List<Pixel> char5 = new List<Pixel>() {

        new Pixel(2,2),
        new Pixel(2,3),
        new Pixel(2,4),
        new Pixel(2,5),
        new Pixel(3,5),
        new Pixel(4,5),
        new Pixel(5,5),
        new Pixel(6,6),
        new Pixel(6,10),
        new Pixel(7,7),
        new Pixel(7,8),
        new Pixel(7,9)
        };

        public static readonly List<Pixel> char5Not = new List<Pixel>() {

        new Pixel(3,3),
        new Pixel(3,4),
        new Pixel(3,6),
        new Pixel(4,3),
        new Pixel(5,3),
        new Pixel(6,3),
        new Pixel(7,3),
        new Pixel(4,6),
        new Pixel(8,8),
        new Pixel(8,9),
        new Pixel(7,11)
        };

        public static readonly List<Pixel> char6 = new List<Pixel>() {

        new Pixel(2,3),
        new Pixel(2,6),
        new Pixel(2,9),
        new Pixel(3,2),
        new Pixel(4,5),
        new Pixel(5,5),
        new Pixel(6,6),
        new Pixel(6,10),
        new Pixel(7,7),
        new Pixel(7,8)
        };

        public static readonly List<Pixel> char6Not = new List<Pixel>() {

        new Pixel(3,4),
        new Pixel(3,7),
        new Pixel(4,3),
        new Pixel(5,3),
        new Pixel(5,6),
        new Pixel(7,3),
        new Pixel(8,8),
        new Pixel(8,9),
        new Pixel(8,10),
        new Pixel(7,11)
        };


        public static readonly List<Pixel> char7 = new List<Pixel>() {

        new Pixel(2,2),
        new Pixel(3,2),
        new Pixel(4,2),
        new Pixel(4,8),
        };

        public static readonly List<Pixel> char7Not = new List<Pixel>() {

        new Pixel(6,7),
        new Pixel(7,5),
        new Pixel(5,9),
        new Pixel(4,3),
        //new Pixel(5,3)
        };


        public static readonly List<Pixel> char8 = new List<Pixel>() {

        new Pixel(2,2),
        new Pixel(2,4),
        new Pixel(2,10),
        new Pixel(3,2),
        new Pixel(3,5),
        new Pixel(3,6),
        new Pixel(3,10),
        new Pixel(4,11),
        new Pixel(6,2),
        new Pixel(6,6),
        new Pixel(6,10),
        new Pixel(7,3),
        //new Pixel(7,7),
        new Pixel(7,8),
        //new Pixel(7,9),

        };


        public static readonly List<Pixel> char8Not = new List<Pixel>() {

        new Pixel(3,8),
        new Pixel(3,3),
        new Pixel(4,3),
        new Pixel(4,7),
        new Pixel(5,3),
        new Pixel(7,11),
        new Pixel(8,4),
        new Pixel(8,5),
        new Pixel(8,8),
        new Pixel(8,9),
        new Pixel(8,10)

        };


        public static readonly List<Pixel> char9 = new List<Pixel>() {

        new Pixel(2,3),
        new Pixel(2,6),
        new Pixel(3,2),
        new Pixel(3,7),
        new Pixel(4,7),
        new Pixel(5,7),
        new Pixel(7,5),
        new Pixel(7,6),
        new Pixel(7,7),
        new Pixel(7,8)

        };

        public static readonly List<Pixel> char9Not = new List<Pixel>() {

        new Pixel(3,4),
        new Pixel(4,3),
        new Pixel(5,3),
        new Pixel(4,8),
        new Pixel(5,8),
        new Pixel(8,5),
        new Pixel(8,6),
        new Pixel(8,7),
        new Pixel(8,8),
        new Pixel(8,9),
        new Pixel(7,11)
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


        public static readonly List<Pixel> Position = new List<Pixel>() {
        new Pixel(0,1),
        new Pixel(0,2),
        new Pixel(0,3),
        new Pixel(0,4),
        new Pixel(0,5),
        new Pixel(0,6),
        new Pixel(0,7),
        new Pixel(0,8),
        new Pixel(0,9),
        new Pixel(12,3),
        new Pixel(15,6),
        new Pixel(21,3),
        new Pixel(30,0),
        new Pixel(28,3),
        new Pixel(29,3),
        new Pixel(30,3),
        new Pixel(30,4),
        new Pixel(30,5),
        new Pixel(30,6),
        new Pixel(30,7),
        new Pixel(30,8),
        new Pixel(30,9),
        new Pixel(36,3),
        new Pixel(37,3),
        new Pixel(38,1),
        new Pixel(38,2),
        new Pixel(38,3),
        new Pixel(38,4),
        new Pixel(38,5),
        new Pixel(38,6),
        new Pixel(38,7),
        new Pixel(39,3),
        new Pixel(40,3),
        new Pixel(41,3),
        new Pixel(48,0),
        new Pixel(46,3),
        new Pixel(47,3),
        new Pixel(48,3),
        new Pixel(48,4),
        new Pixel(48,5),
        new Pixel(48,6),
        new Pixel(48,7),
        new Pixel(48,8),
        new Pixel(48,9),
        new Pixel(57,3),
        new Pixel(68,6),
        new Pixel(68,7),
        new Pixel(68,8),
        new Pixel(68,9),
        new Pixel(84,0),
        new Pixel(84,1),
        new Pixel(84,2),
        new Pixel(84,3),
        new Pixel(84,4),
        new Pixel(84,5),
        new Pixel(84,6),
        new Pixel(84,7),
        new Pixel(84,8),
        new Pixel(84,9),
        new Pixel(84,10),
        new Pixel(84,11),
        new Pixel(85,0),
        new Pixel(85,11),
        };

        public static readonly List<Pixel> PositionNot = new List<Pixel>() {
        new Pixel(1,2),
        new Pixel(1,3),
        new Pixel(1,4),
        new Pixel(1,7),
        new Pixel(1,8),
        new Pixel(1,9),
        new Pixel(1,10),
        new Pixel(29,4),
        new Pixel(37,4),
        new Pixel(39,2),
        new Pixel(39,4),
        new Pixel(39,5),
        new Pixel(39,6),
        new Pixel(39,7),
        new Pixel(40,4),
        new Pixel(41,4),
        new Pixel(42,4),
        new Pixel(47,4),
        new Pixel(69,7),
        new Pixel(69,8),
        new Pixel(69,9),
        new Pixel(69,10),
        new Pixel(85,1),
        new Pixel(85,2),
        new Pixel(85,3),
        new Pixel(85,4),
        new Pixel(85,5),
        new Pixel(85,6),
        new Pixel(85,7),
        new Pixel(85,8),
        new Pixel(85,9),
        new Pixel(85,10),
        new Pixel(86,1)
        };




        #region Windowed

        public static readonly List<Pixel> char0Windowed = new List<Pixel>() {

        new Pixel(2,3),
        new Pixel(2,9),
        //Pixel(3,2),
        new Pixel(3,10),
        new Pixel(4,6),
        new Pixel(4,11),
        new Pixel(7,5),
        new Pixel(7,6)
        };


        public static readonly List<Pixel> char0WindowedNot = new List<Pixel>() {

        new Pixel(4,3),
        new Pixel(3,4),
        new Pixel(5,7),
        new Pixel(8,6),
        new Pixel(8,7)
        };

        public static readonly List<Pixel> char1Windowed = new List<Pixel>() {

            new Pixel(3,2),
            new Pixel(3,11),
            new Pixel(4,3),
            new Pixel(4,4),
            new Pixel(4,5),
            new Pixel(4,6),
            new Pixel(4,7),
            new Pixel(4,8),
            new Pixel(4,9),
            new Pixel(4,10),
            new Pixel(4,11),
            new Pixel(5,11),
        };

        public static readonly List<Pixel> char1WindowedNot = new List<Pixel>() {

            new Pixel(4,12),
            new Pixel(5,12),
            new Pixel(6,12),
            new Pixel(7,12),
        };

        public static readonly List<Pixel> char2WindowedNot = new List<Pixel>() {

        new Pixel(3,3),
        new Pixel(4,3),
        new Pixel(5,3),
        new Pixel(5,9),
        new Pixel(6,8),
        new Pixel(7,7),
        new Pixel(8,4),
        new Pixel(8,5),
        new Pixel(8,6),
        new Pixel(4,12),
        new Pixel(5,12),
        new Pixel(6,12),
        new Pixel(7,12),
        new Pixel(8,12),
        };


        public static readonly List<Pixel> char3or8Windowed = new List<Pixel>() {

        new Pixel(2,2),
        new Pixel(3,2),
        new Pixel(4,11),
        new Pixel(6,2),
        new Pixel(6,5),
        new Pixel(6,6),
        new Pixel(6,10),
        new Pixel(7,3),
        new Pixel(7,7),
        new Pixel(7,8),
        new Pixel(7,9),

        };

        public static readonly List<Pixel> char3or8WindowedNot = new List<Pixel>() {


        new Pixel(3,3),
        new Pixel(4,3),
        new Pixel(4,7),
        new Pixel(5,12),
        new Pixel(7,11),
        new Pixel(8,4),
        new Pixel(8,5),
        new Pixel(8,8),
        new Pixel(8,9),
        new Pixel(8,10),
        };

        public static readonly List<Pixel> char4Windowed = new List<Pixel>() {

            new Pixel(2,8),
            new Pixel(3,8),
            new Pixel(4,8),
            new Pixel(5,8),
            new Pixel(6,8)
        };

        public static readonly List<Pixel> char4WindowedNot = new List<Pixel>() {

            new Pixel(7,3),
            new Pixel(7,4),
            new Pixel(7,5),
            new Pixel(7,10),
            new Pixel(7,11)
        };

        public static readonly List<Pixel> char5Windowed = new List<Pixel>() {

        new Pixel(2,2),
        new Pixel(2,3),
        new Pixel(2,4),
        new Pixel(2,5),
        new Pixel(2,10),
        new Pixel(3,2),
        new Pixel(3,5),
        new Pixel(4,2),
        new Pixel(4,5),
        new Pixel(5,2),
        new Pixel(5,5),
        new Pixel(6,2),
        new Pixel(6,6),
        new Pixel(6,10),
        new Pixel(7,7),
        new Pixel(7,8),
        };

        public static readonly List<Pixel> char5WindowedNot = new List<Pixel>() {

        //new Pixel(3,3),
        new Pixel(3,4),
        new Pixel(3,6),
        new Pixel(4,3),
        new Pixel(5,3),
        };

        public static readonly List<Pixel> char6Windowed = new List<Pixel>() {

            new Pixel(1,6),
            new Pixel(2,3),
            new Pixel(2,6),
            //new Pixel(2,7),
            //new Pixel(2,8),
            new Pixel(2,9),
            new Pixel(3,2),
            new Pixel(3,5),
            new Pixel(4,2),
            new Pixel(4,5),
            new Pixel(4,11),
            new Pixel(5,2),
            new Pixel(5,5),
            new Pixel(5,10),
            new Pixel(6,2),
            new Pixel(6,6),
            new Pixel(6,10),
            //new Pixel(7,2),
            new Pixel(7,7),
            new Pixel(7,8)
        };

        public static readonly List<Pixel> char6WindowedNot = new List<Pixel>() {

            new Pixel(3,4),
            new Pixel(3,7),
            new Pixel(4,3),
            new Pixel(4,6),                    
            //new Pixel(5,6), 
            new Pixel(5,12)           

        };

        public static readonly List<Pixel> char8Windowed = new List<Pixel>() {

        new Pixel(1,8),
        new Pixel(2,2),
        //new Pixel(2,7),
        new Pixel(2,10),
        new Pixel(3,2),
        new Pixel(3,5),
        new Pixel(3,6),
        new Pixel(3,10),
        new Pixel(4,11),
        new Pixel(6,2),
        new Pixel(6,5),
        new Pixel(6,6),
        new Pixel(7,3),
        //new Pixel(7,7),
        new Pixel(7,8),
        //new Pixel(7,9),

        };


        public static readonly List<Pixel> char8WindowedNot = new List<Pixel>() {

        new Pixel(3,3),
        new Pixel(3,8),
        new Pixel(4,3),
        new Pixel(4,7),
        new Pixel(3,8),
        new Pixel(7,11),
        new Pixel(8,4),
        new Pixel(8,5),
        new Pixel(8,8),
        new Pixel(8,9),
        new Pixel(8,10)
        };

        public static readonly List<Pixel> char9Windowed = new List<Pixel>() {

        new Pixel(2,3),
        new Pixel(2,6),
        new Pixel(3,2),
        new Pixel(3,7),
        new Pixel(4,7),
        new Pixel(5,7),
        new Pixel(7,5),
        new Pixel(7,6),
        new Pixel(7,7)
        };

        public static readonly List<Pixel> char9WindowedNot = new List<Pixel>() {

        new Pixel(3,4),
        new Pixel(4,3),
        new Pixel(5,3),
        new Pixel(4,8),
        new Pixel(5,8),
        new Pixel(8,5),
        new Pixel(8,6),
        new Pixel(8,7),
        new Pixel(8,8),
        new Pixel(8,9),
        new Pixel(7,11)
        };


        public static readonly List<Pixel> commaWindowed = new List<Pixel>() {

        new Pixel(3,10),
        new Pixel(3,11)
        };

        public static readonly List<Pixel> commaWindowedNot = new List<Pixel>() {

        new Pixel(5,11),
        new Pixel(5,12),
        };

        #endregion

    }
}
