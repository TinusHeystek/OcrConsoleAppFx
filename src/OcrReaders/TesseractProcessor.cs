using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronOcr;
using Tesseract;

namespace OcrConsoleAppFx.OcrReaders
{
    /// <summary>
    /// TesseractProcessor
    /// </summary>
    public class TesseractProcessor
    {
        private static TesseractEngine _ocr;

        public TesseractProcessor()
        {
            Configure();
        }

        private void Configure()
        {
            _ocr = new TesseractEngine($@"{AppDomain.CurrentDomain.BaseDirectory}\tessdata", "eng");
            _ocr.SetVariable("tessedit_char_whitelist", "0123456789,. ");
            _ocr.SetVariable("classify_bln_numeric_mode", "1");
        }

        public string ReadText(byte[] byteArray)
        {
            var output = "";
            try
            {
                var pix = Pix.LoadFromMemory(byteArray);
                var result = _ocr.Process(pix, PageSegMode.SingleLine);
                output = result.GetText()?.Trim();
                result.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return output;
        }
    }
}
