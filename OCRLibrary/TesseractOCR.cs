extern alias Tesseract;
using tesseract = Tesseract.TesseractOCR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRLibrary
{
    public class TesseractOCR : OCREngine
    {
        private string srcLangCode;  //OCR识别语言 jpn=日语 eng=英语
        private tesseract.Engine engine;

        public override Task<string> OCRProcessAsync(Bitmap img)
        {
            try
            {
                var stream = new MemoryStream();
                img.Save(stream, Imaging.ImageFormat.Bmp);
                var pix = tesseract.Pix.Image.LoadFromMemory(stream.GetBuffer(), 0, Convert.ToInt32(stream.Length));
                var recog = engine.Process(pix);
                stream.Dispose();
                return Task.FromResult(recog.Text);
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return Task.FromResult(string.Empty);
            }
        }

        public override bool OCR_Init(string param1 = "", string param2 = "")
        {
            try
            {
                engine = new tesseract.Engine(Environment.CurrentDirectory + "\\tessdata", srcLangCode);
                return true;
            }
            catch(Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }

        public override void SetOCRSourceLang(string lang)
        {
            if (lang == "jpn")
            {
                srcLangCode = "jpn";
            }
            else if (lang == "eng")
            {
                srcLangCode = "eng";
            }
        }
    }
}
