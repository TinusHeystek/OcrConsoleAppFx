using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronOcr;

namespace OcrConsoleAppFx.OcrReaders
{
    /// <summary>
    /// test with IronOcrProcessor ( for time being not yielding good results )
    /// </summary>
    public class IronOcrProcessor
    {
        private IronTesseract _ocr;
        public IronOcrProcessor()
        {
            Configure();
        }
        
        private void Configure()
        {
            _ocr = new IronTesseract();
            // Fast Dictionary
            _ocr.Language = OcrLanguage.EnglishFast;

            // Latest Engine 
            _ocr.Configuration.TesseractVersion = TesseractVersion.Tesseract5;

            //AI OCR only without font analysis
            _ocr.Configuration.EngineMode = TesseractEngineMode.LstmOnly;

            //Turn off unneeded options
            _ocr.Configuration.ReadBarCodes = false;
            _ocr.Configuration.RenderSearchablePdfsAndHocr = false;

            // Assume text is laid out neatly in an orthagonal document
            _ocr.Configuration.PageSegmentationMode = TesseractPageSegmentationMode.SingleLine;

            _ocr.Configuration.WhiteListCharacters = "0123456789,.[] ";
        }

        public string ReadText(byte[] bytes)
        {
            var output = "";
            try
            {
                using (var Input = new OcrInput(bytes))
                {
                    var Result = _ocr.Read(Input);
                    output = Result.Text;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return output;
        }
    }
}
