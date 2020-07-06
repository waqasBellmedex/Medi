



//using iTextSharp.text;
//using iTextSharp.text.pdf;
using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using MediFusionPM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{








    public class TextFooterEventHandler : IEventHandler
    {

        protected Document doc;
        protected string PRACTICE_NAME;
        protected string send;
        protected ClientDbContext _context;
        public TextFooterEventHandler(Document doc, string PRACTICE_NAME, string send, ClientDbContext _context)
        {
            this.doc = doc;
            this.PRACTICE_NAME = PRACTICE_NAME;
            this.send = send;
            this._context = _context;
        }

        public void HandleEvent(Event currentEvent)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            Rectangle pageSize = docEvent.GetPage().GetPageSize();
            PdfFont font = null;
            PdfFont font1 = null;

            try
            {
                font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);
                string path = System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement", "blackadder_itc2.ttf");
                font1 = PdfFontFactory.CreateFont(path);
            }
            catch (IOException e)
            {
                Console.Error.WriteLine(e.Message);
            }

            float coordX = ((pageSize.GetLeft() + doc.GetLeftMargin())
                             + (pageSize.GetRight() - doc.GetRightMargin())) / 2;
            float headerY = pageSize.GetTop() - doc.GetTopMargin() + 10;
            float footerY = doc.GetBottomMargin() - 10;
            Canvas canvas = new Canvas(docEvent.GetPage(), pageSize);
            canvas
                .SetFont(font1)
                .SetFontSize(20)

                .ShowTextAligned("Thank you from the staff at", coordX, footerY, TextAlignment.CENTER)
                .SetFontSize(15)
                .SetFont(font)
                .ShowTextAligned(PRACTICE_NAME, coordX, footerY - 15, TextAlignment.CENTER)
                  .SetFontSize(10)
                .ShowTextAligned(send, coordX, footerY - 25, TextAlignment.CENTER)
                .Close();
        }
    }
}









    




