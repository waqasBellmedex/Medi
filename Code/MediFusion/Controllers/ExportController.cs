using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Office.Interop.Excel;
//using Application = Microsoft.Office.Interop.Excel.Application;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMCommon;
using System.Runtime.InteropServices;
using static MediFusionPM.ViewModels.VMPractice;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using MediFusionPM.Models;
using MediFusionPM.BusinessLogic;
using Newtonsoft.Json.Linq;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System.Drawing;
using Color = DocumentFormat.OpenXml.Spreadsheet.Color;
using ExportExcelUtilities;
//using ExcelDataReader;
using static MediFusionPM.ViewModels.VMVisit;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Authorize]
    public class ExportController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public ExportController(ClientDbContext context)
        {
            _context = context;
            _startTime = DateTime.Now;
        }


        [Route("ConvertToPdf")]
        [HttpGet]
        public async Task<IActionResult> ConvertExcelToPdf(string InputFileLocation, string OutputFileLocation)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = false;
            Microsoft.Office.Interop.Excel.Workbook wkb = app.Workbooks.Open(InputFileLocation);


            wkb.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, OutputFileLocation);
            wkb.Close();
            app.Quit();

            if (!System.IO.File.Exists(OutputFileLocation))
            {
                return NotFound();
            }

            Byte[] fileBytes = System.IO.File.ReadAllBytes(OutputFileLocation);
            //this returns a byte array of the PDF file content
            if (fileBytes == null)
                return NotFound();
            var stream = new MemoryStream(fileBytes); //saves it into a stream
            stream.Position = 0;
            return File(stream, "application/octec-stream", "837p.pdf");
        }

        [Route("ExportPdf")]
        [HttpPost]
        public async Task<IActionResult> ExportPdf(List<Object> ListOfObjects, UserInfoData UD)
        {
            List<string> FieldNames = new List<string>();
            List<int> IndexesToRemove = new List<int>();
            List<List<KeyValueHolder>> CompleteData = new List<List<KeyValueHolder>>();

            //Debug.WriteLine(PracticeDate.inputList.Count);


            List<JObject> JObjectsList = new List<JObject>();

            for (int i = 0; i < ListOfObjects.Count; i++)
            {
                Object tempObj = ListOfObjects[i];
                JObjectsList.Add(JObject.FromObject(tempObj));
            }

            //The input contains an array of JObjects. 
            //Since i cannot use reflection easily and directly on them as these objects contain data in sub classes, 
            //im extracting the data and storing it in another data structure
            //The JObject also contains the field names so i can extract them from these objects

            foreach (JObject TempJObject in JObjectsList)
            {
                List<KeyValueHolder> ObjectData = new List<KeyValueHolder>();

                foreach (JProperty Prop in TempJObject.Children())
                {

                    JToken Name = Prop.Name;
                    JToken Value = Prop.Value;

                    KeyValueHolder holder = new KeyValueHolder();
                    holder.Key = Name.ToString();
                    holder.Value = Value.ToString();

                    // hash.Add(Name.ToString(), Value.ToString());

                    ObjectData.Add(holder);
                    if (!FieldNames.Contains(Name.ToString()))
                    {
                        FieldNames.Add(Name.ToString());
                    }
                }

                CompleteData.Add(ObjectData);
            }

            //Removing columns that contain the word "id" in them irrespective of the case, but excluding those that have tax in them

            foreach (string Str in FieldNames)
            {
                if (Str.Contains("ID") || Str.Contains("Id"))
                {
                    int Ind = Str.IndexOf("Tax", StringComparison.InvariantCultureIgnoreCase);
                    int Ind2 = Str.IndexOf("Charge", StringComparison.InvariantCultureIgnoreCase);
                    int Ind3 = Str.IndexOf("Subscriber", StringComparison.InvariantCultureIgnoreCase);
                    if (Ind == -1 && Ind2 == -1 && Ind3 == -1)
                    {
                        IndexesToRemove.Add(FieldNames.IndexOf(Str));
                    }
                }
            }

            //Type MyType = PracticeDate.inputList.ElementAt(0).GetType();
            //IList<PropertyInfo> Props = new List<PropertyInfo>(MyType.GetProperties());

            //needed to delete items from the end as deleting them from the start caused problems with indexes so had to use a new loop to do so

            for (int Counter = IndexesToRemove.Count - 1; Counter >= 0; Counter--)
            {
                FieldNames.RemoveAt(IndexesToRemove.ElementAt(Counter));
                foreach (List<KeyValueHolder> InnerList in CompleteData)
                {
                    InnerList.RemoveAt(IndexesToRemove.ElementAt(Counter));
                }
            }


            foreach (string s in FieldNames)
            {
                Debug.WriteLine(s);
            }

            foreach (KeyValueHolder holder in CompleteData.ElementAt(0))
            {
                Debug.WriteLine(holder.Key + "  :  " + holder.Value);
            }
            //array for storing max width for each column
            float[] MaxColumnValueLengths = new float[FieldNames.Count];

            int ColumnCount = FieldNames.Count;

            var datetime = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_");

            string DirectoryPath = getDirectoryPath(UD.ClientID);
            if (!Directory.Exists(DirectoryPath))
           {
                
                    Directory.CreateDirectory(DirectoryPath);
              
                }   
            string FileFullname = Path.Combine(DirectoryPath, "ExportedFile" + datetime + ".pdf");
            System.IO.FileStream fs = new FileStream(FileFullname, FileMode.Create);
            //string FileFullname = "D:\\PdfSample"+datetime.ToString()+".pdf";

            //System.IO.FileStream fs = new FileStream(FileFullname, FileMode.Create);
            //Document doc = new Document(iTextSharp.text.PageSize.A1, 3,3,10,10);

           // doc.SetPageSize(iTextSharp.text.PageSize.A1.Rotate());
           // PdfWriter PdfWriter = PdfWriter.GetInstance(doc, fs);

            //doc.Open();
            PdfPTable table = new PdfPTable(ColumnCount);
            //table.WidthPercentage = 100;

            for (int r = -1; r < CompleteData.Count; r++)
            {

                for (int c = 0; c < ColumnCount; c++)
                {
                    if (r == -1)
                    {
                        var normalFont = FontFactory.GetFont(FontFactory.TIMES, 12);
                        var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
                        float fixedHeight = 20;
                        if (ColumnCount > 9)
                        {
                            normalFont = FontFactory.GetFont(FontFactory.TIMES, 16);
                            boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 16);
                            fixedHeight = 30;
                        }
                        else
                        {
                            normalFont = FontFactory.GetFont(FontFactory.TIMES, 12);
                            boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
                        }

                        var phrase = new Phrase
                        {
                            new Chunk(FieldNames.ElementAt(c), boldFont)
                        };
                        PdfPCell cell = new PdfPCell(phrase)
                        {
                            Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER,
                            BackgroundColor = BaseColor.LIGHT_GRAY
                        };

                        cell.FixedHeight = fixedHeight;
                        cell.Padding = 0;
                        cell.VerticalAlignment = PdfAppearance.ALIGN_CENTER;
                        if (FieldNames.ElementAt(c).Length > MaxColumnValueLengths[c])
                        {

                            String DataInCell = FieldNames.ElementAt(c);
                            //Debug.WriteLine("column number : " + c + "  value in column : " + DataInCell + "  value of current max length for column : " + MaxColumnValueLengths[c - 1]);
                            if (FieldNames.ElementAt(c).Length <= 5)
                                MaxColumnValueLengths[c] = 7f;
                            else
                                MaxColumnValueLengths[c] = (float)FieldNames.ElementAt(c).Length + 2;
                        }

                        table.AddCell(cell);

                    }
                    else
                    {
                        //Debug.WriteLine(r + " : " + c + "    " + CompleteData.Count + "    " + CompleteData.ElementAt(0).Count+ "   "+ColumnCount+"   "+FieldNames.Count);
                        var normalFont = FontFactory.GetFont(FontFactory.TIMES, 12);
                        var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
                        float fixedHeight = 20;
                        if (ColumnCount > 9)
                        {
                            normalFont = FontFactory.GetFont(FontFactory.TIMES, 16);
                            boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 16);
                            fixedHeight = 30;
                        }
                        else
                        {
                            normalFont = FontFactory.GetFont(FontFactory.TIMES, 12);
                            boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
                        }

                        var phrase = new Phrase();
                        phrase.Add(new Chunk(CompleteData.ElementAt(r).ElementAt(c).Value, normalFont));
                        PdfPCell cell = new PdfPCell(phrase);
                        cell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                        cell.FixedHeight = fixedHeight;
                        cell.Padding = 0;
                        cell.VerticalAlignment = PdfAppearance.ALIGN_CENTER;
                        if (CompleteData.ElementAt(r).ElementAt(c).Value.Length > MaxColumnValueLengths[c])
                        {

                            String DataInCell = CompleteData.ElementAt(r).ElementAt(c).Value;
                            //Debug.WriteLine("column number : " + c + "  value in column : " + DataInCell + "  value of current max length for column : " + MaxColumnValueLengths[c - 1]);
                            if (CompleteData.ElementAt(r).ElementAt(c).Value.Length <= 5)
                                MaxColumnValueLengths[c] = 7f;
                            else
                                MaxColumnValueLengths[c] = (float)CompleteData.ElementAt(r).ElementAt(c).Value.Length + 2;
                        }
                        table.AddCell(cell);
                    }

                }
            }

            float totalwidth = 0;
            foreach (float f in MaxColumnValueLengths)
            {
                Debug.WriteLine(f);
                totalwidth += (f * 10);

            }


            Debug.WriteLine(totalwidth);
            var pgSize = new iTextSharp.text.Rectangle(totalwidth + 30, 3508);


            Document doc = new Document(pgSize, 3, 3, 10, 10);



            PdfWriter PdfWriter = PdfWriter.GetInstance(doc, fs);


            doc.Open();
            table.SetWidths(MaxColumnValueLengths);

            Debug.WriteLine(table.TotalHeight + "   " + table.IsExtendLastRow(false) + "   " + table.ExtendLastRow + "  " + table.Rows.Count);
            //table.TotalHeight = ColumnCount * 60;
            table.TotalWidth = totalwidth;
            table.LockedWidth = true;


            table.SetExtendLastRow(false, false);
            //table.LockedWidth = true;
            PdfPTable nesting = new PdfPTable(1);
            PdfPCell BigCell = new PdfPCell(table);
            BigCell.Border = PdfPCell.NO_BORDER;
            nesting.AddCell(BigCell);
            nesting.TotalWidth = totalwidth;
            nesting.LockedWidth = true;
            doc.Add(nesting);
            //doc.Add(table);
            doc.Dispose();
            doc.Close();


            if (!System.IO.File.Exists(FileFullname))
            {
                return NotFound();
            }

            Byte[] fileBytes = System.IO.File.ReadAllBytes(FileFullname);
            //this returns a byte array of the PDF file content
            if (fileBytes == null)
                return NotFound();
            var stream = new MemoryStream(fileBytes); //saves it into a stream
            stream.Position = 0;
            string[] SplitName = FileFullname.Split("\\");
           
            return File(stream, "application/octec-stream", SplitName[SplitName.Length - 1]);
        }


        [Route("ExportExcel")]
        [HttpPost]
        public async Task<IActionResult> ExportExcel([FromRoute]List<Object> ListOfObjects, [FromRoute]UserInfoData UD, [FromBody]Object Criteria, string ReportName)
        {
            string practiceName = UD.PracticeName;
            //string reportName = "TEST REPORT NAME";
            //string dos = "Date Of Service: From: 01/01/2019  To  12/31/2019";
            //string EntryDate = "Entry Date: From - To";
            //string SubmissionDate = "Submission Date: From - To";
            //string ReportLast = "Report Run Date & Time: 01/31/2020    11:42am";
            string Content = "Powered by MediFusion PM";

            //Debug.WriteLine(PracticeDate.inputList.ToList() + "   " + PracticeDate.tempInt);

            List<string> FieldNames = new List<string>();
            List<int> IndexesToRemove = new List<int>();
            List<List<KeyValueHolder>> CompleteData = new List<List<KeyValueHolder>>();
            List<JObject> JObjectsList = new List<JObject>();
            //Debug.WriteLine(PracticeDate.inputList.Count);

            for (int i = 0; i < ListOfObjects.Count; i++)
            {
                Object tempObj = ListOfObjects[i];
                JObjectsList.Add(JObject.FromObject(tempObj));
            }

            JObject temp = JObject.FromObject(Criteria);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (JProperty Prop in temp.Children())
            {
                JToken Name = Prop.Name;
                JToken Value = Prop.Value;
                if (Value.ToString() == "true" || Value.ToString() == "false" || Value.ToString() == "0" || Value.ToString() == "True" || Value.ToString() == "False")
                {
                    Value = null;
                }
                else if (Value.ToString().Contains("DOS") || Value.ToString().Contains("ED") || Value.ToString().Contains("SD"))
                {
                    Value = Value.ToString().Replace("DOS", "Date Of Service");
                    
                    Value =  Value.ToString().Replace("ED", "Entry Date");

                    Value = Value.ToString().Replace("SD", "Submitted Date");

                    dic.Add(Name.ToString(), Value.ToString());
                }
                else
                {
                    dic.Add(Name.ToString(), Value.ToString());
                }
               
            }
            dic = dic.Where(e => e.Value != "").ToDictionary(c => c.Key.ToString(), a => a.Value.ToString());

            List<string> Pairtobereplaced = new List<string>();

            for (int z = 0; z < dic.Count(); z++)
            {
                //string fu = dic.ElementAt(z).Key;

                if (dic.ElementAt(z).Key.Contains("Dos"))
                {

                    // dic[dic.ElementAt(z).Key] =dic.ElementAt(z).Key.Replace("Dos", "DateOfService");
                    Pairtobereplaced.Add(dic.ElementAt(z).Key + "|" + dic.ElementAt(z).Key.Replace("Dos", "Date Of Service "));
                    // Pairtobereplaced.Add(dic.ElementAt(z).Key + "|" + dic.ElementAt(z).Key.Replace("EntryDateFrom", "Entry Date From "));
                }
                else if (dic.ElementAt(z).Key.Contains("DOS"))
                {

                    // dic[dic.ElementAt(z).Key] = dic.ElementAt(z).Key.Replace("DOS", "DateOfService");
                    Pairtobereplaced.Add(dic.ElementAt(z).Key + "|" + dic.ElementAt(z).Key.Replace("DOS", "Date Of Service "));
                    //Pairtobereplaced.Add(dic.ElementAt(z).Key + "|" + dic.ElementAt(z).Key.Replace("DOS", "Date Of Service "));
                }
                else if (dic.ElementAt(z).Key.Contains("EntryDate"))
                {

                    // dic[dic.ElementAt(z).Key] =dic.ElementAt(z).Key.Replace("Dos", "DateOfService");
                    Pairtobereplaced.Add(dic.ElementAt(z).Key + "|" + dic.ElementAt(z).Key.Replace("EntryDate", "Entry Date "));
                    // Pairtobereplaced.Add(dic.ElementAt(z).Key + "|" + dic.ElementAt(z).Key.Replace("EntryDateFrom", "Entry Date From "));
                }
                else
                {
                    string temp2 = dic.ElementAt(z).Key;
                    for (int q = temp2.Length - 1; q >= 0; q--)
                    {
                        if (temp2[q] >= 'A' && temp2[q] <= 'Z')
                        {
                            temp2 = temp2.Replace(temp2[q].ToString(), " " + temp2[q]);

                        }
                    }
                    Pairtobereplaced.Add(dic.ElementAt(z).Key + "|" + temp2);
                }

            }
            for (int Pair = 0; Pair < Pairtobereplaced.Count(); Pair++)
            {

                string[] splitarray = Pairtobereplaced.ElementAt(Pair).Split('|');
                string value = dic[Pairtobereplaced.ElementAt(Pair).Split('|')[0]];
                //if (value.Contains("True") || value.Contains("False"))
                //{
                //    value = null;
                //}
                //else
                //{
                    dic.Remove(Pairtobereplaced.ElementAt(Pair).Split('|')[0]);
                    dic.Add(splitarray[1], value);
                //}
            }

            dic = dic.OrderBy(e => e.Key).ToDictionary(c => c.Key.ToString(), a => a.Value.ToString());
            Dictionary<string, string> DatesDic = new Dictionary<string, string>();       //get dates values
            Dictionary<string, string> SingularDic = new Dictionary<string, string>();    // criteria to search from any other key values
            DatesDic = dic.Where(x => x.Key.Contains("Date")).ToDictionary(c => c.Key.ToString(), a => a.Value.ToString());
            SingularDic = dic.Where(x => !x.Key.Contains("Date")).ToDictionary(c => c.Key.ToString(), a => a.Value.ToString());
            long DateCounter = DatesDic.Count / 2; //to count dates in rows 
            long ValueCount = SingularDic.Count;

            //The input contains an array of JObjects. 
            //Since i cannot use reflection easily and directly on them as these objects contain data in sub classes, 
            //im extracting the data and storing it in another data structure
            //The JObject also contains the field names so i can extract them from these objects

            foreach (JObject TempJObject in JObjectsList)
            {
                List<KeyValueHolder> ObjectData = new List<KeyValueHolder>();

                foreach (JProperty Prop in TempJObject.Children())
                {

                    JToken Name = Prop.Name;
                    JToken Value = Prop.Value;
                    string temp2 = Prop.Name;
                    bool AllUppers = true;
                    foreach (char c in temp2)
                    {
                        if (!char.IsUpper(c))
                        {
                            AllUppers = false;
                        }
                    }
                    if (AllUppers == false)
                    {
                        for (int q = temp2.Length - 1; q >= 0; q--)
                        {
                            if (temp2 != null)
                            {
                                if (temp2[q] >= 'A' && temp2[q] <= 'Z')
                                {
                                    temp2 = temp2.Replace(temp2[q].ToString(), " " + temp2[q]);
                                }
                            }

                        }
                    }
                    KeyValueHolder holder = new KeyValueHolder();
                    holder.Key = temp2;//Name.ToString();
                    holder.Value = Value.ToString();

                    // hash.Add(Name.ToString(), Value.ToString());
                    if (temp2 != null)
                        ObjectData.Add(holder);
                    if (!FieldNames.Contains(temp2))//Name.ToString()))
                    {
                        FieldNames.Add(temp2);//Name.ToString());
                    }
                }
                CompleteData.Add(ObjectData);
            }

            //Removing columns that contain the word "id" in them irrespective of the case, but excluding those that have tax in them

            foreach (string Str in FieldNames)
            {
                if (Str.Contains("ID") || Str.Contains("Id"))
                {
                    int Ind = Str.IndexOf("Tax", StringComparison.InvariantCultureIgnoreCase);
                    int Ind2 = Str.IndexOf("Visit", StringComparison.InvariantCultureIgnoreCase);
                    int Ind3 = Str.IndexOf("Charge", StringComparison.InvariantCultureIgnoreCase);
                    int Ind4 = Str.IndexOf("Subscriber", StringComparison.InvariantCultureIgnoreCase);

                    if (Ind == -1 && Ind2 == -1 && Ind3 == -1 && Ind4 == -1)
                    {
                        IndexesToRemove.Add(FieldNames.IndexOf(Str));

                    }
                }
				if (Str.Contains("Check Amount") && ReportName.Equals("Detailed Collection Report"))
                {
                    IndexesToRemove.Add(FieldNames.IndexOf(Str));
                }
            }

            //Type MyType = PracticeDate.inputList.ElementAt(0).GetType();
            //IList<PropertyInfo> Props = new List<PropertyInfo>(MyType.GetProperties());

            //needed to delete items from the end as deleting them from the start caused problems with indexes so had to use a new loop to do so

            for (int Counter = IndexesToRemove.Count - 1; Counter >= 0; Counter--)
            {
                FieldNames.RemoveAt(IndexesToRemove.ElementAt(Counter));
                foreach (List<KeyValueHolder> InnerList in CompleteData)
                {
                    InnerList.RemoveAt(IndexesToRemove.ElementAt(Counter));
                }
            }

            //array for storing max width for each column
            double[] MaxColumnValueLengths = new double[FieldNames.Count];

            int ColumnCount = FieldNames.Count;

            var datetime = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_");

            string DirectoryPath = getDirectoryPath(UD.ClientID);
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            string FileFullname = Path.Combine(DirectoryPath, "ExportedFile" + datetime + ".xlsx");

            //string FileFullname = "D:\\Testing.xlsx";


            //initialization of openxml for creating the excel
            SpreadsheetDocument SpreadDoc = SpreadsheetDocument.Create(FileFullname,
        DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);

            WorkbookPart Wbp = SpreadDoc.AddWorkbookPart();
            WorksheetPart Wsp = Wbp.AddNewPart<WorksheetPart>();
            Workbook Wb = new Workbook();
            FileVersion fv = new FileVersion();
            fv.ApplicationName = "Microsoft Office Excel";
            Worksheet Ws = new Worksheet();
            SheetData sd = new SheetData();

            WorkbookStylesPart stylePart = Wbp.AddNewPart<WorkbookStylesPart>();
            stylePart.Stylesheet = CellMods.GenerateStylesheet();
            stylePart.Stylesheet.Save();

            long TextValuesCount = 0;
            long InsertCount = 0;
            int Staticolumncount = 20; //for static column if fields are less
            if (ColumnCount > Staticolumncount)
            {
                Staticolumncount = ColumnCount;
            }
            //for cell rows and coloumns  in top of header
            for (int K = 1; K <= 7; K++)//ROw
            {
                Row firstRow1 = new Row();
                firstRow1.RowIndex = (UInt32)K;

                for (int L = 1; L <= Staticolumncount; L++)//Column
                {
                    Cell Emptycell2 = new Cell();
                    Emptycell2.DataType = CellValues.InlineString;
                    if (L == Staticolumncount)
                    {
                        Emptycell2.StyleIndex = 10;
                    }
                    else
                        Emptycell2.StyleIndex = 3;
                    CellFormat cellFormat = Emptycell2.StyleIndex != null ? CellMods.GetCellFormat(Wbp, Emptycell2.StyleIndex).CloneNode(true) as CellFormat : new CellFormat();
                    InlineString RandominlineString2 = new InlineString();
                    Text t3 = new Text();
                    //if (L == 6)
                    if (L == 5)
                    {
                        if (K == 1)
                        {
                            t3.Text = UD.PracticeName;
                            Emptycell2.StyleIndex = 4;
                            firstRow1.Height = 43;
                            firstRow1.CustomHeight = true;
                        }
                        if (K == 2)
                        {
                            t3.Text = ReportName;
                            Emptycell2.StyleIndex = 6;
                            firstRow1.Height = 21;
                            firstRow1.CustomHeight = true;
                        }
                        if (K == 3)
                        {
                            if (InsertCount / 2 < DateCounter)
                            {
                                t3.Text = DatesDic.ElementAt((int)InsertCount).Key + " : " + DatesDic.ElementAt((int)InsertCount).Value.Split(" ")[0] + " To " + DatesDic.ElementAt((int)InsertCount + 1).Value.Split(" ")[0];
                                InsertCount += 2;
                                firstRow1.Height = 17;
                                firstRow1.CustomHeight = true;

                            }
                            else
                            {
                                if (TextValuesCount < ValueCount)
                                {
                                    string Ageunit = SingularDic.ElementAt((int)TextValuesCount).Key;

                                    if (Ageunit.Contains("Age Type"))
                                    {
                                        if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("D"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "DOS";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("E"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Entry Date";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("S"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Submit Date";
                                        }
                                    }
                                    else
                                    //    if (SingularDic.ElementAt((int)TextValuesCount).Value == "True" || SingularDic.ElementAt((int)TextValuesCount).Value == "False")
                                    //{
                                    //    t3.Text = null;
                                    //}
                                    //else
                                    {
                                        t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + SingularDic.ElementAt((int)TextValuesCount).Value;
                                    }
                                    TextValuesCount++;
                                    InsertCount++;
                                    firstRow1.Height = 17;
                                    firstRow1.CustomHeight = true;
                                }
                            }
                            Emptycell2.StyleIndex = 5;
                        }
                        if (K == 4)
                        {
                            if (InsertCount / 2 < DateCounter)
                            {
                                //if (DatesDic.ElementAt(0).Key.Contains("Date", StringComparison.InvariantCultureIgnoreCase))
                                //{
                                t3.Text = DatesDic.ElementAt((int)InsertCount).Key + " : " + DatesDic.ElementAt((int)InsertCount).Value.Split(" ")[0] + " To " + DatesDic.ElementAt((int)InsertCount + 1).Value.Split(" ")[0];
                                InsertCount += 2;
                                firstRow1.Height = 17;
                                firstRow1.CustomHeight = true;
                                //}
                                //else
                                //{
                                //t3.Text = dic.ElementAt(0).Key + ": " + dic.ElementAt(0).Value;
                                //}
                            }
                            else
                            {
                                if (TextValuesCount < ValueCount)
                                {
                                    string Ageunit = SingularDic.ElementAt((int)TextValuesCount).Key;
                                    if (Ageunit.Contains("Age Type"))
                                    {
                                        if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("D"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "DOS";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("E"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Entry Date";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("S"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Submit Date";
                                        }
                                    }
                                    else
                                    //if (SingularDic.ElementAt((int)TextValuesCount).Value == "True" || SingularDic.ElementAt((int)TextValuesCount).Value == "False")
                                    //{
                                    //    t3.Text = null;
                                    //}
                                    //else
                                    {
                                        t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + SingularDic.ElementAt((int)TextValuesCount).Value;
                                    }
                                    InsertCount++;
                                    TextValuesCount++;
                                    firstRow1.Height = 17;
                                    firstRow1.CustomHeight = true;
                                }
                            }
                            //t3.Text = EntryDate;
                            Emptycell2.StyleIndex = 5;
                        }
                        if (K == 5)
                        {
                            if (InsertCount / 2 < DateCounter)
                            {
                                t3.Text = DatesDic.ElementAt((int)InsertCount).Key + " : " + DatesDic.ElementAt((int)InsertCount).Value.Split(" ")[0] + " To " + DatesDic.ElementAt((int)InsertCount + 1).Value.Split(" ")[0];
                                InsertCount += 2;
                                firstRow1.Height = 17;
                                firstRow1.CustomHeight = true;
                            }
                            else
                            {
                                if (TextValuesCount < ValueCount)
                                {
                                    string Ageunit = SingularDic.ElementAt((int)TextValuesCount).Key;
                                    if (Ageunit.Contains("Age Type"))
                                    {
                                        if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("D"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "DOS";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("E"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Entry Date";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("S"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Submit Date";
                                        }
                                        //{
                                    }
                                    else
                                    //          if (SingularDic.ElementAt((int)TextValuesCount).Value == "True" || SingularDic.ElementAt((int)TextValuesCount).Value == "False")
                                    //{
                                    //    t3.Text = null;
                                    //}
                                    //else
                                    {
                                        t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + SingularDic.ElementAt((int)TextValuesCount).Value;
                                    }
                                    TextValuesCount++;
                                    InsertCount++;
                                    firstRow1.Height = 17;
                                    firstRow1.CustomHeight = true;
                                }
                            }
                            //else
                            Emptycell2.StyleIndex = 5;
                        }
                    }
                    //{
                    if (L == 13)
                    {
                        if (K == 3)
                        {
                            if (InsertCount / 2 < DateCounter)
                            {
                                t3.Text = DatesDic.ElementAt((int)InsertCount).Key + " : " + DatesDic.ElementAt((int)InsertCount).Value.Split(" ")[0] + " To " + DatesDic.ElementAt((int)InsertCount + 1).Value.Split(" ")[0];
                                InsertCount += 2;
                                firstRow1.Height = 17;
                                firstRow1.CustomHeight = true;
                            }
                            else
                            {
                                if (TextValuesCount < ValueCount)
                                {
                                    string Ageunit = SingularDic.ElementAt((int)TextValuesCount).Key;
                                    if (Ageunit.Contains("Age Type"))
                                    {
                                        if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("D"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "DOS";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("E"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Entry Date";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("S"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Submit Date";
                                        }
                                        //t3.Text = dic.ElementAt(0).Key + ": " + dic.ElementAt(0).Value;
                                    }
                                    else
                                    //              if (SingularDic.ElementAt((int)TextValuesCount).Value == "True" || SingularDic.ElementAt((int)TextValuesCount).Value == "False")
                                    //{
                                    //    t3.Text = null;
                                    //}
                                    //else
                                    {
                                        t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + SingularDic.ElementAt((int)TextValuesCount).Value;
                                    }
                                    TextValuesCount++;
                                    InsertCount++;
                                    firstRow1.Height = 17;
                                    firstRow1.CustomHeight = true;
                                }
                            }
                            Emptycell2.StyleIndex = 5;
                        }
                        //}
                        if (K == 4)
                        {
                            if (InsertCount / 2 < DateCounter)
                            {
                                t3.Text = DatesDic.ElementAt((int)InsertCount).Key + " : " + DatesDic.ElementAt((int)InsertCount).Value.Split(" ")[0] + " To " + DatesDic.ElementAt((int)InsertCount + 1).Value.Split(" ")[0];
                                InsertCount += 2;
                                firstRow1.Height = 17;
                                firstRow1.CustomHeight = true;
                            }
                            else
                            {
                                if (TextValuesCount < ValueCount)
                                {
                                    string Ageunit = SingularDic.ElementAt((int)TextValuesCount).Key;
                                    if (Ageunit.Contains("Age Type"))
                                    {
                                        if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("D"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "DOS";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("E"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Entry Date";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("S"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Submit Date";
                                        }
                                    }
                                    else
                                    //          if (SingularDic.ElementAt((int)TextValuesCount).Value == "True" || SingularDic.ElementAt((int)TextValuesCount).Value == "False")
                                    //{
                                    //    t3.Text = null;
                                    //}
                                    //else
                                    {
                                        t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + SingularDic.ElementAt((int)TextValuesCount).Value;
                                    }
                                    TextValuesCount++;
                                    InsertCount++;
                                    firstRow1.Height = 17;
                                    firstRow1.CustomHeight = true;
                                }
                            }
                            //t3.Text = SubmissionDate;
                            Emptycell2.StyleIndex = 5;
                        }
                        if (K == 5)
                        {
                            if (InsertCount / 2 < DateCounter)
                            {
                                t3.Text = DatesDic.ElementAt((int)InsertCount).Key + " : " + DatesDic.ElementAt((int)InsertCount).Value.Split(" ")[0] + " To " + DatesDic.ElementAt((int)InsertCount + 1).Value.Split(" ")[0];
                                InsertCount += 2;
                                firstRow1.Height = 17;
                                firstRow1.CustomHeight = true;
                            }
                            else
                            {
                                if (TextValuesCount < ValueCount)
                                {
                                    string Ageunit = SingularDic.ElementAt((int)TextValuesCount).Key;
                                    if (Ageunit.Contains("Age Type"))
                                    {
                                        if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("D"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "DOS";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("E"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Entry Date";
                                        }
                                        else if (SingularDic.ElementAt((int)TextValuesCount).Value.Contains("S"))
                                        {
                                            t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + "Submit Date";
                                        }
                                    }
                                    else
                                    //if (SingularDic.ElementAt((int)TextValuesCount).Value == "True" || SingularDic.ElementAt((int)TextValuesCount).Value == "False")
                                    //{
                                    //    t3.Text = null;
                                    //}
                                    //else
                                    {
                                        t3.Text = SingularDic.ElementAt((int)TextValuesCount).Key + " : " + SingularDic.ElementAt((int)TextValuesCount).Value;
                                    }
                                    TextValuesCount++;
                                    InsertCount++;
                                    firstRow1.Height = 17;
                                    firstRow1.CustomHeight = true;
                                }
                            }
                        }
                        Emptycell2.StyleIndex = 5;
                    }

                    RandominlineString2.AppendChild(t3);
                    Emptycell2.Append(RandominlineString2);
                    firstRow1.AppendChild(Emptycell2);

                }
                sd.Append(firstRow1);

            }

            int RowIndex = 7;
            Row HeaderRow = new Row();
            for (int i = 1; i <= Staticolumncount; i++)
            {
                HeaderRow.RowIndex = (UInt32)RowIndex;
                Cell C1 = new Cell();
                C1.DataType = CellValues.InlineString;


                C1.StyleIndex = 2;
                InlineString inlineString = new InlineString();
                Text t = new Text();

                if (i <= ColumnCount)
                {
                    t.Text = FieldNames.ElementAt(i - 1);
                    inlineString.AppendChild(t);
                    if ((t.Text.Contains("Is Between30 And60") || t.Text.Contains("Is Between61 And90") || t.Text.Contains("Is Between91 And120") || t.Text.Contains("Is Greater Than120")) && ReportName.Equals("Aging Detail Report"))
                    {
                        //   t.Text = t.Text.Replace("Is Between31 And60", "31-60");
                        t.Text = t.Text.Replace("Is Between30 And60", "30-60");
                        t.Text = t.Text.Replace("Is Between61 And90", "61-90");
                        t.Text = t.Text.Replace("Is Between91 And120", "91-120");
                        t.Text = t.Text.Replace("Is Greater Than120", ">120");
                    }
                    if ((t.Text.Contains("Is Between30 And60") || t.Text.Contains("Is Between61 And90") || t.Text.Contains("Is Between91 And120") || t.Text.Contains("Is Greater Than120")) && ReportName.Equals("Aging Report"))
                    {
                        t.Text = t.Text.Replace("Is Between30 And60", "31-60");
                        t.Text = t.Text.Replace("Is Between61 And90", "61-90");
                        t.Text = t.Text.Replace("Is Between91 And120", "91-120");
                        t.Text = t.Text.Replace("Is Greater Than120", ">120");
                    }


                    if (t.Text.Contains("Payment Entry Date")|| t.Text.Contains("Applied Payments"))
                    {
                        t.Text = t.Text.Replace("Payment Entry Date","Posted Date");
                        t.Text = t.Text.Replace("Applied Payments", "Applied Amount");
                    }
                    else if(t.Text.Contains("Referring Physician Name") || t.Text.Contains("Facility Name") || t.Text.Contains("M O D1") || t.Text.Contains("M O D2")|| t.Text.Contains("dx1")|| t.Text.Contains("dx2") || t.Text.Contains("dx3") || t.Text.Contains("dx4"))
                    {
                        t.Text = t.Text.Replace("Referring Physician Name", "Reference Provider");
                        t.Text = t.Text.Replace("Facility Name", "Location");
                        t.Text = t.Text.Replace("M O D1", "MOD 1");
                        t.Text = t.Text.Replace("M O D2", "MOD 2");
                        t.Text = t.Text.Replace("dx1", "DX 1");
                        t.Text = t.Text.Replace("dx2", "DX 2");
                        t.Text = t.Text.Replace("dx3", "DX 3");
                        t.Text = t.Text.Replace("dx4", "DX 4");
                    }
                    if ((t.Text.Contains("Prescribing M D") || t.Text.Contains("Charges")) && ReportName.Equals("Detail BillingReport"))
                    {
                        t.Text = t.Text.Replace("Prescribing M D", "Prescribing MD/Referr");
                        t.Text = t.Text.Replace("Charges", "Sum Of Charges");
                    }
                    if (t.Text.Contains("Individual NP  I"))
                    {
                        t.Text = t.Text.Replace("Individual NP  I", "Individual NPI");
                    }
                    if ((t.Text.Contains("Payer Name")) && ReportName.Equals("Total Revenue Collect By CPT Report"))
                    {
                        t.Text = t.Text.Replace("Payer Name", "Insurance Name");
                    }
                    if(t.Text.Contains("Sr No"))
                    {
                        t.Text = t.Text.Replace("Sr No", "S.No");
                    }
                    if((t.Text.Contains("C P T Units") || t.Text.Contains("Modifier1") || t.Text.Contains("Modifier2")) && ReportName.Equals("Detailed Collection Report"))
                    {
                        t.Text = t.Text.Replace("C P T Units", "CPT Units");
                        t.Text = t.Text.Replace("Modifier1", "Modifier 1");
                        t.Text = t.Text.Replace("Modifier2", "Modifier 2");
                    }
                    if(t.Text.Contains("Cpt"))
                    {
                        t.Text = t.Text.Replace("Cpt", "CPT");
                    }
                    
                    Debug.WriteLine(C1.StyleIndex + "   " + Wbp);
                }

                C1.Append(inlineString);
                HeaderRow.Append(C1);

            }

            sd.AppendChild(HeaderRow);


            decimal[] sumofvalues = new decimal[ColumnCount];


            Debug.WriteLine(JObjectsList.Count + "  " + ColumnCount);
            for (int r = 1; r <= CompleteData.Count; r++) //Rows
            {
                Row row = new Row();
                for (int c = 1; c <= Staticolumncount; c++) //Coloumn
                {
                    row.RowIndex = (UInt32)r + 7;
                    Cell C1 = new Cell();
                    C1.DataType = CellValues.InlineString;
                    CellFormat cellFormat = C1.StyleIndex != null ? CellMods.GetCellFormat(Wbp, C1.StyleIndex).CloneNode(true) as CellFormat : new CellFormat();
                    InlineString inlineString = new InlineString();
                    Text t = new Text();
                   
                    if (c <= ColumnCount)
                    {
                        t.Text = CompleteData.ElementAt(r - 1).ElementAt(c - 1).Value;

                        if (t.Text.Equals("M") || t.Text.Equals("F"))
                        {
                            t.Text = t.Text.Replace("M", "Male");
                            t.Text = t.Text.Replace("F", "Female");
                        }                     
                      
                       if (t.Text.Where(Char.IsLetter).ToList().Count == 0 && t.Text != "")
                        {
                            if (!t.Text.Contains("/") && !t.Text.Contains(","))
                            {
                                try
                                {
                                    if (!t.Text.Contains(" "))
                                    {
                                        sumofvalues[c - 1] = sumofvalues[c - 1] + decimal.Parse(t.Text.Trim().Replace("$", ""));
                                    }
                                }
                                catch (Exception e)
                                {
                                    // Don't uncomment the next statement.
                                    //throw;
                                }
                            }

                        }
                    }
                    inlineString.Append(t);
                    if (r == CompleteData.Count)
                    {
                        C1.StyleIndex = 8;
                    }
                    else if (c == Staticolumncount)
                    {
                        C1.StyleIndex = 9;
                    }
                    else
                        C1.StyleIndex = 7;


                    if (r == CompleteData.Count)
                    {
                        if (c == Staticolumncount)
                        {
                            C1.StyleIndex = 11;
                        }
                    }


                    C1.Append(inlineString);
                    row.Append(C1);

                    // Debug.WriteLine(CompleteData.ElementAt(r - 1).ElementAt(c - 1).Value.Length + "    " + MaxColumnValueLengths[c - 1]);
                    if (c <= ColumnCount)
                    {
                        if (CompleteData.ElementAt(r - 1).ElementAt(c - 1).Value.Length > MaxColumnValueLengths[c - 1])
                        {

                            String DataInCell = CompleteData.ElementAt(r - 1).ElementAt(c - 1).Value;
                            Debug.WriteLine("column number : " + c + "  value in column : " + DataInCell + "  value of current max length for column : " + MaxColumnValueLengths[c - 1]);
                            MaxColumnValueLengths[c - 1] = CompleteData.ElementAt(r - 1).ElementAt(c - 1).Value.Length + 4;
                        }
                    }

                }
                sd.AppendChild(row);

            }

            //adding sum of amount
            Row AmountCalc = new Row();
            AmountCalc.RowIndex = (UInt32)CompleteData.Count + 8;
            for (int C = 1; C <= ColumnCount; C++)
            {
                InlineString string1 = new InlineString();
                Cell cellAmount = new Cell();
                cellAmount.DataType = CellValues.InlineString;
                Text t4 = new Text();
                if (sumofvalues[C - 1] != 0)
                {
                    if (FieldNames[C - 1].Contains("MOD") || FieldNames[C - 1].Contains("Account") || FieldNames[C - 1].Contains("ID") || FieldNames[C - 1].Contains("Age") || FieldNames[C - 1].Contains("NPI") || FieldNames[C - 1].Contains("PhoneNumber") || FieldNames[C - 1].Contains("CheckNumber") || FieldNames[C - 1].Contains("SrNo") || FieldNames[C - 1].Contains("PrimaryPolicyNumber") || FieldNames[C - 1].Contains("SecondaryPolicyNumber") || FieldNames[C - 1].Contains("SSN") || FieldNames[C - 1].Contains("Cpt") || FieldNames[C - 1].Contains("POS") || FieldNames[C - 1].Contains("MOD1"))
                    {
                        t4.Text = "";
                    }
                    else
                    {
                        t4.Text = sumofvalues[C - 1].ToString();

                    }

                }
                string1.AppendChild(t4);
                cellAmount.Append(string1);
                AmountCalc.AppendChild(cellAmount);
            }
            sd.Append(AmountCalc);
            // up to this code

            for (int Temp2 = 1; Temp2 <= ColumnCount; Temp2++)
            {
                if (FieldNames.ElementAt(Temp2 - 1).Length > MaxColumnValueLengths[Temp2 - 1])
                {
                    MaxColumnValueLengths[Temp2 - 1] = FieldNames.ElementAt(Temp2 - 1).Length + 4;
                }
            }

            // last row values dispalyed
            Row lastrowdata = new Row();
            lastrowdata.RowIndex = (UInt32)CompleteData.Count + 9;
            for (int firsttext = 1; firsttext <= 9; firsttext++)
            {
                InlineString inlineString2 = new InlineString();
                Cell CellValue = new Cell();
                CellValue.DataType = CellValues.InlineString;
                Text Lasttext = new Text();
                if (firsttext == 1)
                {
                    DateTime now = DateTime.Now;
                    Lasttext.Text = "Report Run Date & Time: '" + now + "' "; ;
                }
                if (firsttext == 8)
                {
                    Lasttext.Text = Content;
                }
                inlineString2.AppendChild(Lasttext);
                CellValue.Append(inlineString2);
                lastrowdata.AppendChild(CellValue);
            }
            sd.Append(lastrowdata);

            //CompleteData.ElementAt(0);

            MergeCells mergeCells = new MergeCells();
            mergeCells.Append(new MergeCell() { Reference = new StringValue("A1:B6") });
            mergeCells.Append(new MergeCell() { Reference = new StringValue("E1:K1") });
            mergeCells.Append(new MergeCell() { Reference = new StringValue("E2:J2") });
            mergeCells.Append(new MergeCell() { Reference = new StringValue("E3:J3") });
            mergeCells.Append(new MergeCell() { Reference = new StringValue("E4:J4") });
            mergeCells.Append(new MergeCell() { Reference = new StringValue("E5:J5") });
            mergeCells.Append(new MergeCell() { Reference = new StringValue("M3:R3") });
            mergeCells.Append(new MergeCell() { Reference = new StringValue("M4:R4") });
            mergeCells.Append(new MergeCell() { Reference = new StringValue("M5:R5") });
            mergeCells.Append(new MergeCell() { Reference = new StringValue("A" + (CompleteData.Count + 9) + ":D" + (CompleteData.Count + 9)) }); //Last row values merge
            mergeCells.Append(new MergeCell() { Reference = new StringValue("H" + (CompleteData.Count + 9) + ":J" + (CompleteData.Count + 9)) });  //Last row values merge
            Ws.Append(sd);
            Wsp.Worksheet = Ws;
            Wsp.Worksheet.InsertAfter(mergeCells, Wsp.Worksheet.Elements<SheetData>().First());

            // InsertCell(6, 6, Ws);

            CellMods.SetColumnWidth(Ws, 10, MaxColumnValueLengths, false);
            //Wsp.Worksheet.Append(columns1);
            Wsp.Worksheet.Save();
            Sheets Sheets = new Sheets();
            Sheet Sheet = new Sheet();
            Sheet.Name = "Sheet1";
            Sheet.SheetId = 1;
            Sheet.Id = Wbp.GetIdOfPart(Wsp);
            Sheets.Append(Sheet);
            Wb.Append(fv);
            Wb.Append(Sheets);

            PageSetup pageSetup = new PageSetup();
            pageSetup.Orientation = OrientationValues.Landscape;
            pageSetup.FitToHeight = 1;
            pageSetup.FitToWidth = 1;
            pageSetup.PaperSize = 8;
            Wsp.Worksheet.AppendChild(pageSetup);
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message + "    " + ex.StackTrace);
            //}

            SpreadDoc.WorkbookPart.Workbook = Wb;


            SpreadDoc.WorkbookPart.Workbook.Save();
            SpreadDoc.Close();


            if (!System.IO.File.Exists(FileFullname))
            {
                return NotFound();
            }
            ExcelTools.AddImage(false, FileFullname, "Sheet1",
                                    Path.Combine(_context.env.ContentRootPath, "Resources", "BellmedexLogo.png"), "description",
                                    1 /* column */, 1 /* row */);
            Byte[] fileBytes = System.IO.File.ReadAllBytes(FileFullname);
            //this returns a byte array of the PDF file content
            if (fileBytes == null)
                return NotFound();
            var stream = new MemoryStream(fileBytes); //saves it into a stream
            stream.Position = 0;
            string[] SplitName = FileFullname.Split("\\");
            return File(stream, "application/octec-stream", SplitName[SplitName.Length - 1]);

            //worksheetPart.Worksheet.Append(columns);
            //Wbpart.Workbook.Sheets.AppendChild(Sheet);

            //Wbpart.Workbook.Save();
            //SpreadDoc.Close();
        }


        static Cell AddCellWithText(string text)
        {
            Cell C1 = new Cell();
            C1.DataType = CellValues.InlineString;

            InlineString inlineString = new InlineString();
            Text t = new Text();
            t.Text = text;
            inlineString.AppendChild(t);

            C1.AppendChild(inlineString);

            return C1;
        }

        [Route("GetDirectoryPath")]
        [HttpGet]

        public string getDirectoryPath(long ClientID)
        {


            //Debug.WriteLine(User.Claims.ToList().Count);



            Settings settings = _context.Settings.Where(s => s.ClientID == ClientID).SingleOrDefault();

            //Debug.WriteLine(settings + "    " + _context + "  " + UD.ClientID + "  " + settings + "    " + UD.Role);

            string directoryPath = Path.Combine("\\\\", settings.DocumentServerURL,
                        settings.DocumentServerDirectory, ClientID.ToString(), "ExportedArticles",
                        DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));

            return directoryPath;
        }
    }

    public class CriteriaObject
    {
        public CVisit CVisit { get; set; }
        public List<Object> ListOfObject { get; set; }
    }
    public class ExportInput
    {
        public int tempInt { get; set; }
        public List<JObject> inputList { get; set; }
        public long ClientID { get; set; }
    }
    public class KeyValueHolder
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    //Method for when the datatype is text based

    public class CellMods
    {
        public static void SetColumnWidth(Worksheet worksheet, uint Index, double[] dwidths, bool hidden = false)
        {
            DocumentFormat.OpenXml.Spreadsheet.Columns cs = worksheet.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Columns>();
            if (cs != null)
            {
                IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Column> ic = cs.Elements<DocumentFormat.OpenXml.Spreadsheet.Column>();
                if (ic.Count() > 0)
                {
                    for (int i = 0; i < dwidths.Length; i++)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Column c = ic.ElementAt(i + 1);
                        DoubleValue DVal = new DoubleValue(dwidths[i]);
                        c.Width = DVal;
                    }
                }
                else
                {
                    cs = new DocumentFormat.OpenXml.Spreadsheet.Columns();
                    for (int i = 0; i < dwidths.Length; i++)
                    {

                        DoubleValue DVal = new DoubleValue(dwidths[i]);
                        DocumentFormat.OpenXml.Spreadsheet.Column col = new DocumentFormat.OpenXml.Spreadsheet.Column() { Min = new UInt32Value((uint)i + 1), Max = new UInt32Value((uint)i + 1), Hidden = hidden, Width = DVal, CustomWidth = true };
                        cs.Append(col);
                    }
                    worksheet.InsertAfter(cs, worksheet.GetFirstChild<SheetProperties>());
                }
            }
            else
            {
                cs = new DocumentFormat.OpenXml.Spreadsheet.Columns();
                for (int i = 0; i < dwidths.Length; i++)
                {

                    DoubleValue DVal = new DoubleValue(dwidths[i]);
                    DocumentFormat.OpenXml.Spreadsheet.Column col = new DocumentFormat.OpenXml.Spreadsheet.Column() { Min = new UInt32Value((uint)i + 1), Max = new UInt32Value((uint)i + 1), Hidden = hidden, Width = DVal, CustomWidth = true };
                    cs.Append(col);
                }

                worksheet.InsertAfter(cs, worksheet.GetFirstChild<SheetProperties>());
            }
        }

        public static CellFormat GetCellFormat(WorkbookPart workbookPart, uint styleIndex)
        {
            return workbookPart.WorkbookStylesPart.Stylesheet.Elements<CellFormats>().First().Elements<CellFormat>().ElementAt((int)styleIndex);
        }

        public static Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;
            Fonts fonts = new Fonts(
    new DocumentFormat.OpenXml.Spreadsheet.Font( // Index 0 - default
        new FontSize() { Val = 10 }

    ),
    new DocumentFormat.OpenXml.Spreadsheet.Font( // Index 1 - header
        new FontSize() { Val = 10 },
        new Bold(),
        new Color() { Rgb = "FFFFFF" }
        ),
    new DocumentFormat.OpenXml.Spreadsheet.Font( // Index 2 - header
        new FontSize() { Val = 20 },
        new FontName() { Val = "Calibri" }
    ),
    new DocumentFormat.OpenXml.Spreadsheet.Font( // Index 3 - header
        new FontSize() { Val = 16 },
         new FontName() { Val = "Calibri" }
    ),
     new DocumentFormat.OpenXml.Spreadsheet.Font( // Index 4 - header
        new FontSize() { Val = 12 },
         new FontName() { Val = "Calibri" }
    ));


            Fills fills = new Fills(
         new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
         new Fill(new PatternFill(new BackgroundColor { Rgb = new HexBinaryValue() { Value = "D8D8D8" } }) { PatternType = PatternValues.None }), // Index 1 - default
         new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "B2B2B2" } }) { PatternType = PatternValues.Solid }),
         new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "ffffff" } }) { PatternType = PatternValues.Solid }) // Index 2 - header
         );

            Borders borders = new Borders(
        new Border(), // index 0 default
        new Border( // index 1 black border
            new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
            new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
            new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
            new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
            new DiagonalBorder()),
        new Border( // index 2
            new LeftBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thick },
            new RightBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thick },
            new TopBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thick },
            new BottomBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thick },
            new DiagonalBorder()),
        new Border( // index 3
            new LeftBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "ffffff" } }) { Style = BorderStyleValues.Thick },
            new RightBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "ffffff" } }) { Style = BorderStyleValues.Thick },
            new TopBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "ffffff" } }) { Style = BorderStyleValues.Thick },
            new BottomBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "ffffff" } }) { Style = BorderStyleValues.Thick },
            new DiagonalBorder()),
        new Border( // index 4 bottom border
            new LeftBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
            new RightBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
            new TopBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
            new BottomBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thick },
            new DiagonalBorder()),
        new Border( // index 5  Side border
            new LeftBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
            new RightBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thick },
            new TopBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
            new BottomBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
            new DiagonalBorder()),

        new Border( // index 6 Empty Cell Border lines
            new LeftBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "ffffff" } }) { Style = BorderStyleValues.Thick },
            new RightBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thick },
            new TopBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "ffffff" } }) { Style = BorderStyleValues.Thick },
            new BottomBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "ffffff" } }) { Style = BorderStyleValues.Thick },
            new DiagonalBorder()),

        new Border( // index 7  Corner border
            new LeftBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
            new RightBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thick },
            new TopBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
            new BottomBorder(new Color() { Auto = true, Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thick },
            new DiagonalBorder())
    );

            CellFormats cellFormats = new CellFormats(
        new CellFormat(), // default
        new CellFormat { FontId = 0, FillId = 1, BorderId = 3, ApplyBorder = true }, // body index 1
        new CellFormat { FontId = 1, FillId = 2, BorderId = 2, ApplyFill = true }, // header index 2
        new CellFormat { FontId = 0, FillId = 0, BorderId = 3, ApplyFill = true, ApplyBorder = true }, // for empty cell index 3
        new CellFormat { FontId = 2, FillId = 0, BorderId = 3, ApplyFill = true, ApplyBorder = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center, WrapText = true, Vertical = VerticalAlignmentValues.Center, } }, // Text size index 4

        new CellFormat { FontId = 4, FillId = 0, BorderId = 3, ApplyFill = true, ApplyBorder = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center, WrapText = true, Vertical = VerticalAlignmentValues.Center, } }, // Text size of 3,4,5 line index 5
        new CellFormat { FontId = 3, FillId = 0, BorderId = 3, ApplyFill = true, ApplyBorder = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center, WrapText = true, Vertical = VerticalAlignmentValues.Center, } },  // Text size of second line index 6
        new CellFormat { FontId = 0, FillId = 3, BorderId = 1, ApplyFill = true }, //index 7
        new CellFormat { FontId = 0, FillId = 0, BorderId = 4, ApplyFill = true }, //index 8 bottom border
        new CellFormat { FontId = 0, FillId = 0, BorderId = 5, ApplyFill = true }, //index 9 Side Border
        new CellFormat { FontId = 0, FillId = 0, BorderId = 6, ApplyFill = true }, //index 10 empty cell border line
                                                                                   //  new CellFormat { FontId = 0, FillId = 0, BorderId = 5, ApplyFill = true }  //index 11 for static solid line
        new CellFormat { FontId = 0, FillId = 0, BorderId = 7, ApplyFill = true } // index 11 border line to attach 
        );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);
            return styleSheet;
        }
    }
}


