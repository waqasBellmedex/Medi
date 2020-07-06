using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediFusionPM.Models;
using static MediFusionPM.ReportViewModels.RVMCollectionReport;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using MediFusionPM.Controllers;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Bibliography;

namespace MediFusionPM.ReportControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RCollectionController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public RCollectionController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;

            // Only For Testing
            if (_context.Practice.Count() == 0)
            {
                //  _context.Facilities.Add(new Facility { Name = "ABC Facility", OrganizationName = "" });
                // _context.SaveChanges();
            }
        }

        [Route("GetCollections")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRCollectionReport>>> GetCollections()
        {
            try
            {
                return await (
                              from paymentcheck in _context.PaymentCheck
                              join aspnetusertable in _context.Users on paymentcheck.AddedBy equals aspnetusertable.Email into ASPUserTable
                              from aput in ASPUserTable.DefaultIfEmpty()
                              join practicetable in _context.Practice on paymentcheck.PracticeID equals practicetable.ID

                              select new GRCollectionReport
                              {
                                  SrNo = paymentcheck.ID,
                                  AppliedAmount = paymentcheck.AppliedAmount,
                                  CheckDate = paymentcheck.CheckDate.Format("MM/dd/yyyy"),
                                  CheckNumber = paymentcheck.CheckNumber,
                                  PayerName = aput.Name.IsNull() ? "" : aput.Name,
                                  PostingDate = paymentcheck.PostedDate.Format("MM/dd/yyyy"),
                                  PracticeName = practicetable.Name.IsNull()?"": practicetable.Name,
                                  PostingUserName = aput.Name.IsNull()?"": aput.Name
                              }
                              ).ToAsyncEnumerable().ToList();
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(Path.Combine(_context.env.ContentRootPath, "Logs", "PatientVisit.txt"), ex.ToString());
                Debug.WriteLine(ex.Message + "   " + ex.StackTrace);
                return Ok(ex);
            }
        }

        [Route("FindCollections")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRCollectionReport>>> FindCollections(CRCollectionReport CRCollectionReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
         //  if (UD == null || UD.Rights == null || UD.Rights.SchedulerSearch == false)
         // return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
           return FindCollections(CRCollectionReport, UD);
        }

        private List<GRCollectionReport> FindCollections(CRCollectionReport CRCollectionReport, UserInfoData UD)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Client = (from w in _contextMain.MainPractice
                          where w.ID == PracticeId
                          select w
                        ).FirstOrDefault();
            string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
            string newConnection = GetConnectionStringManager(contextName);


            if (CRCollectionReport.CollectionType == "PATIENT")
            {

                List<GRCollectionReport> data = new List<GRCollectionReport>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    string oString = "select patientpayment.ID as SrNo,isnull(pPaymentCharge.AllocatedAmount,0) as AppliedAmount ,isnull(patientpayment.PaymentAmount,0)  as CheckAmount , convert(varchar, patientpayment.PaymentDate, 101)  as CheckDate, (pat.LastName + ', ' + pat.FirstName) as PayerName,  convert(varchar, patientpayment.AddedDate, 101)  as PostingDate,pra.[Name] as PracticeName, patientpayment.AddedBy as PostingUserName,pra.ID as PracticeID,pra.ClientID as ClientID, case when patientpayment.PaymentMethod = 'Cash' then 'Cash' when patientpayment.PaymentMethod = 'Check' then patientpayment.CheckNumber when patientpayment.PaymentMethod = 'Credit Card' then patientpayment.CCTransactionID else '' end as CheckNumber from PatientPayment patientpayment join Visit visit on patientpayment.VisitID =visit.ID join Charge c on visit.ID = c.ID left join PatientPaymentCharge pPaymentCharge on patientpayment.ID = pPaymentCharge.PatientPaymentID left join Patient pat on visit.PatientID = pat.ID left join Cpt cpt on c.CPTID = cpt.ID left join Practice pra on visit.PracticeID = pra.ID left join [Provider] pro on visit.ProviderID = pro.ID left join [Location] lo on c.LocationID = lo.ID  left join RefProvider RefPr on c.RefProviderID = RefPr.ID ";

                    oString += "  where c.practiceid = {0} ";
                    oString = string.Format(oString, PracticeId);

                    if (!CRCollectionReport.ProviderID.IsNull())
                        oString += string.Format(" and pro.ID = '{0}'", CRCollectionReport.ProviderID);

                    if (!CRCollectionReport.LocationID.IsNull())
                        oString += string.Format(" and lo.ID = '{0}'", CRCollectionReport.LocationID);
                    if (!CRCollectionReport.CheckNo.IsNull() || CRCollectionReport.CheckNo.Equals("CASH"))
                        oString += string.Format(" and patientpayment.CheckNumber = '{0}'", CRCollectionReport.CheckNo);
                    if (!CRCollectionReport.RefProviderID.IsNull())
                        oString += string.Format(" and visit.RefProviderID = '{0}'", CRCollectionReport.RefProviderID);
                    if (!CRCollectionReport.UserPosted.IsNull())
                        oString += string.Format(" and patientpayment.AddedBy like '%{0}%'", CRCollectionReport.UserPosted);



                    if (CRCollectionReport.DOSDateFrom != null && CRCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  >= '" + CRCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "' and visit.DateOfServiceFrom  < '" + CRCollectionReport.DOSDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.DOSDateFrom != null && CRCollectionReport.DOSDateTo == null)
                    {
                        oString += (" and ( visit.DateOfServiceFrom  >= '" + CRCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CRCollectionReport.DOSDateFrom == null && CRCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  <= '" + CRCollectionReport.DOSDateTo.GetValueOrDefault().Date + "')");
                    }


                    if (CRCollectionReport.CheckDateFrom != null && CRCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (patientpayment.PaymentDate  >= '" + CRCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "' and patientpayment.PaymentDate  < '" + CRCollectionReport.CheckDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.CheckDateFrom != null && CRCollectionReport.CheckDateTo == null)
                    {
                        oString += (" and ( patientpayment.PaymentDate  >= '" + CRCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CRCollectionReport.CheckDateFrom == null && CRCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (patientpayment.PaymentDate  <= '" + CRCollectionReport.CheckDateTo.GetValueOrDefault().Date + "')");
                    }



                    if (CRCollectionReport.PostedDateFrom != null && CRCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (patientpayment.AddedDate  >= '" + CRCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "' and patientpayment.AddedDate  < '" + CRCollectionReport.PostedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.PostedDateFrom != null && CRCollectionReport.PostedDateTo == null)
                    {
                        oString += (" and ( patientpayment.AddedDate  >= '" + CRCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "' )  ");

                    }
                    else if (CRCollectionReport.PostedDateFrom == null && CRCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (patientpayment.AddedDate  <= '" + CRCollectionReport.PostedDateTo.GetValueOrDefault().Date + " ' ) ");
                    }

                    oString += ("Group By patientpayment.ID, pra.[Name],patientpayment.AddedBy,isnull(patientpayment.PaymentAmount,0)  ,isnull( c.TotalAmount, 0) , case when patientpayment.PaymentMethod = 'Cash' then 'Cash' when patientpayment.PaymentMethod = 'Check' then patientpayment.CheckNumber	when patientpayment.PaymentMethod = 'Credit Card' then patientpayment.CCTransactionID else ''  end  , isnull( pPaymentCharge.AllocatedAmount ,0 ) ,pra.ID ,pra.ClientID , pat.LastName , pat.FirstName , pat.AccountNum , visit.ID , pat.LastName , pat.FirstName ,  convert(varchar, visit.DateOfServiceFrom, 101) , cpt.CPTCode , c.Units , convert(varchar, patientpayment.PaymentDate, 101)  ,convert(varchar, patientpayment.AddedDate, 101)  , pro.[Name] ,lo.[Name] ,RefPr.[Name] ,pat.ID , c.ID ");


                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {

                            var chaPayment = new GRCollectionReport();
                            chaPayment.SrNo = Convert.ToInt64(oReader["SrNo"].ToString());
                            chaPayment.AppliedAmount = Convert.ToDecimal(oReader["AppliedAmount"].ToString());
                            chaPayment.CheckDate = oReader["CheckDate"].ToString();
                            chaPayment.CheckNumber = oReader["CheckNumber"].ToString();
                            chaPayment.PayerName = oReader["PayerName"].ToString();
                            chaPayment.PostingDate = oReader["PostingDate"].ToString();
                            chaPayment.PracticeName = oReader["PracticeName"].ToString();
                            chaPayment.PostingUserName = oReader["PostingUserName"].ToString();
                            chaPayment.CheckAmount = Convert.ToDecimal(oReader["CheckAmount"].ToString());


                            data.Add(chaPayment);
                        }
                        myConnection.Close(); myConnection.Close();
                    }
                }

                return data;

                //List<GRCollectionReport> data = (from patientpayment in _context.PatientPayment
                //                                 join visit in _context.Visit
                //                                 on patientpayment.VisitID equals visit.ID
                //                                 join pro in _context.Provider
                //                                 on visit.ProviderID equals pro.ID
                //                                 join practice in _context.Practice
                //                                 on visit.PracticeID equals practice.ID
                //                                 join pat in _context.Patient on visit.PatientID equals pat.ID

                //                                 where
                //                                 (ExtensionMethods.IsBetweenDOS(CRCollectionReport.PostedDateTo, CRCollectionReport.PostedDateFrom, patientpayment.AddedDate, patientpayment.AddedDate))
                //                                 && (ExtensionMethods.IsBetweenDOS(CRCollectionReport.DOSDateTo, CRCollectionReport.DOSDateFrom, visit.DateOfServiceFrom, visit.DateOfServiceFrom))
                //                                 && (CRCollectionReport.ProviderID.IsNull() ? true : visit.ProviderID.Equals(CRCollectionReport.ProviderID))
                //                                 && (CRCollectionReport.CheckNo.IsNull() || CRCollectionReport.CheckNo.Equals("CASH") ? true : patientpayment.CheckNumber.IsNull() ? false : patientpayment.CheckNumber.Equals(CRCollectionReport.CheckNo))
                //                                 && (CRCollectionReport.UserPosted.IsNull() ? true : patientpayment.AddedBy.IsNull() ? false : patientpayment.AddedBy.Contains(CRCollectionReport.UserPosted))
                //                                 && (ExtensionMethods.IsBetweenDOS(CRCollectionReport.CheckDateTo, CRCollectionReport.CheckDateFrom, patientpayment.PaymentDate, patientpayment.PaymentDate))
                //                                 && (CRCollectionReport.RefProviderID.IsNull() ? true : visit.RefProviderID.Equals(CRCollectionReport.RefProviderID))
                //                                 && (practice.ID == UD.PracticeID)
                //                                 //&& (practice.ClientID == UD.ClientID)
                //                                 select new GRCollectionReport
                //                                 {
                //                                     SrNo = patientpayment.ID,
                //                                     AppliedAmount = patientpayment.PaymentAmount,
                //                                     CheckDate = patientpayment.PaymentDate.Format("MM/dd/yyyy"),
                //                                     CheckNumber = GetPaymentMethod(patientpayment.CCTransactionID, patientpayment.CheckNumber, patientpayment.PaymentMethod),
                //                                     PayerName = pat.LastName + ", " + pat.FirstName,
                //                                     PostingDate = patientpayment.AddedDate.Format("MM/dd/yyyy"),
                //                                     CheckAmount = patientpayment.PaymentAmount,
                //                                     PracticeName = practice.Name,
                //                                     PostingUserName = patientpayment.AddedBy,
                //                                     PracticeID = practice.ID,
                //                                     ClientID = practice.ClientID,

                //                                 }).Distinct().ToList();
                //return data;

            }
            else if (CRCollectionReport.CollectionType == "PAYER")
            {
                List<GRCollectionReport> chargePayment = new List<GRCollectionReport>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    string oString = "select paymentcheck.ID as SrNo,  sum(isnull(pCharge.PaidAmount,0)) as AppliedAmount, convert(varchar, paymentcheck.CheckDate, 101)  as CheckDate, paymentcheck.CheckNumber as CheckNumber,paymentcheck.PayerName as PayerName, convert(varchar, pCharge.PostedDate, 101) as PostingDate,pra.[Name] as PracticeName, paymentcheck.PostedBy as PostingUserName, pra.ID as PracticeID,pra.ClientID as ClientID, isnull(paymentcheck.CheckAmount,0) as CheckAmount from PaymentCheck paymentcheck join Practice pra on paymentcheck.PracticeID = pra.ID join PaymentVisit pVisit on paymentcheck.ID = pVisit.PaymentCheckID  join PaymentCharge pCharge on pVisit.ID = pCharge.PaymentVisitID join Charge c on  pCharge.ChargeID = c.ID join Visit visit on  c.VisitID = visit.ID join Patient pat on visit.PatientID = pat.ID join Cpt cpt on c.CPTID = cpt.ID join Practice practice on paymentcheck.PracticeID = practice.ID join [Provider] pro on visit.ProviderID = pro.ID left join [Location] lo on c.LocationID = lo.ID left join RefProvider RefPr on c.RefProviderID = RefPr.ID left join Modifier mod1 on c.Modifier1ID = mod1.ID left join Modifier mod2 on c.Modifier2ID = mod2.ID where pCharge.status = 'P' and pCharge.chargeid is not null";

                    oString += "  and c.practiceid = {0} ";
                    oString = string.Format(oString, PracticeId);

                    if (!CRCollectionReport.ProviderID.IsNull())
                        oString += string.Format(" and pro.ID = '{0}'", CRCollectionReport.ProviderID);
                    if (!CRCollectionReport.LocationID.IsNull())
                        oString += string.Format(" and lo.ID = '{0}'", CRCollectionReport.LocationID);
                    if (!CRCollectionReport.CheckNo.IsNull())
                        oString += string.Format(" and paymentcheck.CheckNumber = '{0}'", CRCollectionReport.CheckNo);
                    if (!CRCollectionReport.RefProviderID.IsNull())
                        oString += string.Format(" and visit.RefProviderID = '{0}'", CRCollectionReport.RefProviderID);
                    if (!CRCollectionReport.UserPosted.IsNull())
                        oString += string.Format(" and paymentcheck.PostedBy ='{0}'", CRCollectionReport.UserPosted);


                    if (CRCollectionReport.DOSDateFrom != null && CRCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  >= '" + CRCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "' and visit.DateOfServiceFrom  < '" + CRCollectionReport.DOSDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.DOSDateFrom != null && CRCollectionReport.DOSDateTo == null)
                    {
                        oString += (" and ( visit.DateOfServiceFrom  >= '" + CRCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CRCollectionReport.DOSDateFrom == null && CRCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  <= '" + CRCollectionReport.DOSDateTo.GetValueOrDefault().Date + "')");
                    }

                    if (CRCollectionReport.CheckDateFrom != null && CRCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (paymentcheck.CheckDate  >= '" + CRCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "' and paymentcheck.CheckDate  < '" + CRCollectionReport.CheckDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.CheckDateFrom != null && CRCollectionReport.CheckDateTo == null)
                    {
                        oString += (" and ( paymentcheck.CheckDate  >= '" + CRCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CRCollectionReport.CheckDateFrom == null && CRCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (paymentcheck.CheckDate  <= '" + CRCollectionReport.CheckDateTo.GetValueOrDefault().Date + "')");
                    }







                    if (CRCollectionReport.PostedDateFrom != null && CRCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (pCharge.PostedDate  >= '" + CRCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "' and pCharge.PostedDate  < '" + CRCollectionReport.PostedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.PostedDateFrom != null && CRCollectionReport.PostedDateTo == null)
                    {
                        oString += (" and ( pCharge.PostedDate  >= '" + CRCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CRCollectionReport.PostedDateFrom == null && CRCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (pCharge.PostedDate  <= '" + CRCollectionReport.PostedDateTo.GetValueOrDefault().Date + "')");
                    }

                    oString += "GROUP BY paymentcheck.ID, convert(varchar, paymentcheck.CheckDate, 101) ,paymentcheck.CheckNumber,paymentcheck.PayerName,  convert(varchar, pCharge.PostedDate, 101) ,pra.[Name], paymentcheck.PostedBy , pra.ID,pra.ClientID, paymentcheck.CheckAmount ";


                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {

                            var chaPayment = new GRCollectionReport();
                            chaPayment.SrNo = Convert.ToInt64(oReader["SrNo"].ToString());
                            chaPayment.AppliedAmount = Convert.ToDecimal(oReader["AppliedAmount"].ToString());
                            chaPayment.CheckDate = oReader["CheckDate"].ToString();
                            chaPayment.CheckNumber = oReader["CheckNumber"].ToString();
                            chaPayment.PayerName = oReader["PayerName"].ToString();
                            chaPayment.PostingDate = oReader["PostingDate"].ToString();
                            chaPayment.PracticeName = oReader["PracticeName"].ToString();
                            chaPayment.PostingUserName =oReader["PostingUserName"].ToString();
                            chaPayment.CheckAmount = Convert.ToDecimal(oReader["CheckAmount"].ToString()); 

                            chargePayment.Add(chaPayment);
                        }
                        myConnection.Close();
                    }
                }

                return chargePayment; 


                //List<GRCollectionReport> data = (from paymentcheck in _context.PaymentCheck
                //                                 join practice in _context.Practice
                //                                 on paymentcheck.PracticeID equals practice.ID
                //                                 join pVisit in _context.PaymentVisit on paymentcheck.ID equals pVisit.PaymentCheckID
                //                                 join pCharge in _context.PaymentCharge on pVisit.ID equals pCharge.PaymentVisitID
                //                                 where
                //                                 (ExtensionMethods.IsBetweenDOS(CRCollectionReport.PostedDateTo, CRCollectionReport.PostedDateFrom, pCharge.PostedDate, pCharge.PostedDate))
                //                                 // (CRCollectionReport.ProviderID.IsNull() ? true : LeftProvider.ID.Equals(CRCollectionReport.ProviderID))
                //                                 && (CRCollectionReport.CheckNo.IsNull() ? true : paymentcheck.CheckNumber.ToUpper().Equals(CRCollectionReport.CheckNo))
                //                                 && (ExtensionMethods.IsBetweenDOS(CRCollectionReport.CheckDateTo, CRCollectionReport.CheckDateFrom, paymentcheck.CheckDate, paymentcheck.CheckDate))
                //                                 && (CRCollectionReport.UserPosted.IsNull() ? true : paymentcheck.PostedBy.IsNull() ? false : paymentcheck.PostedBy.ToUpper().Equals(CRCollectionReport.UserPosted))
                //                                 // 
                //                                 && (practice.ID == UD.PracticeID)
                //                                 && pCharge.Status == "P" && pCharge.ChargeID != null

                //                                 group new
                //                                 {
                //                                     SrNo = paymentcheck.ID,
                //                                     AppliedAmount = pCharge.PaidAmount,
                //                                     CheckDate = paymentcheck.CheckDate.Format("MM/dd/yyyy"),
                //                                     CheckNumber = paymentcheck.CheckNumber,
                //                                     PayerName = paymentcheck.PayerName,
                //                                     PostingDate = pCharge.PostedDate.Format("MM/dd/yyyy"),
                //                                     PracticeName = practice.Name,
                //                                     PostingUserName = paymentcheck.PostedBy,
                //                                     PracticeID = practice.ID,
                //                                     ClientID = practice.ClientID,
                //                                     CheckAmount = paymentcheck.CheckAmount,

                //                                 } by new { PostingDate = pCharge.PostedDate.Format("MM/dd/yyyy"), paymentcheck.CheckNumber } into gp
                //                                 select new GRCollectionReport
                //                                 {
                //                                     SrNo = gp.Select(a => a.SrNo).FirstOrDefault(),
                //                                     AppliedAmount = gp.Sum(a => a.AppliedAmount),
                //                                     CheckDate = gp.Select(a => a.CheckDate).FirstOrDefault(),
                //                                     CheckNumber = gp.Select(a => a.CheckNumber).FirstOrDefault(),
                //                                     PayerName = gp.Select(a => a.PayerName).FirstOrDefault(),
                //                                     PostingDate = gp.Select(a => a.PostingDate).FirstOrDefault(),
                //                                     PracticeName = gp.Select(a => a.PracticeName).FirstOrDefault(),
                //                                     PostingUserName = gp.Select(a => a.PostingUserName).FirstOrDefault(),
                //                                     PracticeID = gp.Select(a => a.PracticeID).FirstOrDefault(),
                //                                     ClientID = gp.Select(a => a.ClientID).FirstOrDefault(),
                //                                     CheckAmount = gp.Select(a => a.CheckAmount).FirstOrDefault(),
                //                                 }).ToList();



                //if (CRCollectionReport.DOSDateTo.HasValue && CRCollectionReport.DOSDateFrom.HasValue || CRCollectionReport.ProviderID != null)
                //{
                //    data = (from d in data
                //            join v in _context.Visit
                //            on d.PracticeID equals v.PracticeID
                //            join p in _context.Provider
                //            on d.PracticeID equals p.PracticeID
                //            where (ExtensionMethods.IsBetweenDOS(CRCollectionReport.DOSDateTo, CRCollectionReport.DOSDateFrom, v.DateOfServiceFrom, v.DateOfServiceFrom))
                //            && (CRCollectionReport.RefProviderID.IsNull() ? true : v.RefProviderID.Equals(CRCollectionReport.RefProviderID))
                //            select d).Distinct().ToList();
                //}

                //return data;

            }
            else
            {

                //List<GRCollectionReport> data = (from patientpayment in _context.PatientPayment
                //                                 join visit in _context.Visit
                //                                 on patientpayment.VisitID equals visit.ID
                //                                 join pro in _context.Provider
                //                                 on visit.ProviderID equals pro.ID
                //                                 join practice in _context.Practice
                //                                 on visit.PracticeID equals practice.ID
                //                                 join pat in _context.Patient on visit.PatientID equals pat.ID
                //                                 where
                //                                 (ExtensionMethods.IsBetweenDOS(CRCollectionReport.PostedDateTo, CRCollectionReport.PostedDateFrom, patientpayment.AddedDate, patientpayment.AddedDate))
                //                                 && (ExtensionMethods.IsBetweenDOS(CRCollectionReport.DOSDateTo, CRCollectionReport.DOSDateFrom, visit.DateOfServiceFrom, visit.DateOfServiceFrom))
                //                                 && (CRCollectionReport.ProviderID.IsNull() ? true : visit.ProviderID.Equals(CRCollectionReport.ProviderID))
                //                                 && (CRCollectionReport.CheckNo.IsNull() || CRCollectionReport.CheckNo.Equals("CASH") ? true : patientpayment.CheckNumber.IsNull() ? false : patientpayment.CheckNumber.Equals(CRCollectionReport.CheckNo))
                //                                 && (CRCollectionReport.UserPosted.IsNull() ? true : patientpayment.AddedBy.IsNull() ? false : patientpayment.AddedBy.Contains(CRCollectionReport.UserPosted))
                //                                 && (ExtensionMethods.IsBetweenDOS(CRCollectionReport.CheckDateTo, CRCollectionReport.CheckDateFrom, patientpayment.PaymentDate, patientpayment.PaymentDate))
                //                                 && (CRCollectionReport.RefProviderID.IsNull() ? true : visit.RefProviderID.Equals(CRCollectionReport.RefProviderID))
                //                                 && (practice.ID == UD.PracticeID)
                //                                 //&& (practice.ClientID == UD.ClientID)
                //                                 select new GRCollectionReport
                //                                 {
                //                                     SrNo = patientpayment.ID,
                //                                     AppliedAmount = patientpayment.PaymentAmount,
                //                                     CheckDate = patientpayment.PaymentDate.Format("MM/dd/yyyy"),
                //                                     CheckNumber = GetPaymentMethod(patientpayment.CCTransactionID, patientpayment.CheckNumber, patientpayment.PaymentMethod),
                //                                     PayerName = pat.LastName + ", " + pat.FirstName,
                //                                     PostingDate = patientpayment.AddedDate.Format("MM/dd/yyyy"),
                //                                     CheckAmount = patientpayment.PaymentAmount,
                //                                     PracticeName = practice.Name,
                //                                     PostingUserName = patientpayment.AddedBy,
                //                                     PracticeID = practice.ID,
                //                                     ClientID = practice.ClientID,

                //                                 }).ToList();



                List<GRCollectionReport> data = new List<GRCollectionReport>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    string oString = "select patientpayment.ID as SrNo,isnull(pPaymentCharge.AllocatedAmount,0)  as AppliedAmount ,isnull(patientpayment.PaymentAmount,0)  as CheckAmount , convert(varchar, patientpayment.PaymentDate, 101)  as CheckDate, (pat.LastName + ', ' + pat.FirstName) as PayerName,  convert(varchar, patientpayment.AddedDate, 101)  as PostingDate,pra.[Name] as PracticeName, patientpayment.AddedBy as PostingUserName,pra.ID as PracticeID,pra.ClientID as ClientID,  case 	when patientpayment.PaymentMethod = 'Cash' then 'Cash' when patientpayment.PaymentMethod = 'Check' then patientpayment.CheckNumber	when patientpayment.PaymentMethod = 'Credit Card' then patientpayment.CCTransactionID			else ''  end 		as CheckNumber from PatientPayment patientpayment join Visit visit  on   patientpayment.VisitID =visit.ID join Charge c on   visit.ID = c.ID  left join PatientPaymentCharge pPaymentCharge on patientpayment.ID = pPaymentCharge.PatientPaymentID left join Patient pat on visit.PatientID = pat.ID left join Cpt cpt on c.CPTID = cpt.ID left join Practice pra on visit.PracticeID = pra.ID left join [Provider] pro on visit.ProviderID = pro.ID left join [Location] lo on c.LocationID = lo.ID  left join RefProvider RefPr on c.RefProviderID = RefPr.ID    ";

                    oString += "  where c.practiceid = {0} ";
                    oString = string.Format(oString, PracticeId);

                    if (!CRCollectionReport.ProviderID.IsNull())
                        oString += string.Format(" and pro.ID = '{0}'", CRCollectionReport.ProviderID);

                    if (!CRCollectionReport.LocationID.IsNull())
                        oString += string.Format(" and lo.ID = '{0}'", CRCollectionReport.LocationID);
                    if (!CRCollectionReport.CheckNo.IsNull() || CRCollectionReport.CheckNo.Equals("CASH"))
                        oString += string.Format(" and patientpayment.CheckNumber = '{0}'", CRCollectionReport.CheckNo);
                    if (!CRCollectionReport.RefProviderID.IsNull())
                        oString += string.Format(" and visit.RefProviderID = '{0}'", CRCollectionReport.RefProviderID);
                    if (!CRCollectionReport.UserPosted.IsNull())
                        oString += string.Format(" and patientpayment.AddedBy like '%{0}%'", CRCollectionReport.UserPosted);



                    if (CRCollectionReport.DOSDateFrom != null && CRCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  >= '" + CRCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "' and visit.DateOfServiceFrom  < '" + CRCollectionReport.DOSDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.DOSDateFrom != null && CRCollectionReport.DOSDateTo == null)
                    {
                        oString += (" and ( visit.DateOfServiceFrom  >= '" + CRCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CRCollectionReport.DOSDateFrom == null && CRCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  <= '" + CRCollectionReport.DOSDateTo.GetValueOrDefault().Date + "')");
                    }


                    if (CRCollectionReport.CheckDateFrom != null && CRCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (patientpayment.PaymentDate  >= '" + CRCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "' and patientpayment.PaymentDate  < '" + CRCollectionReport.CheckDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.CheckDateFrom != null && CRCollectionReport.CheckDateTo == null)
                    {
                        oString += (" and ( patientpayment.PaymentDate  >= '" + CRCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CRCollectionReport.CheckDateFrom == null && CRCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (patientpayment.PaymentDate  <= '" + CRCollectionReport.CheckDateTo.GetValueOrDefault().Date + "')");
                    }



                    if (CRCollectionReport.PostedDateFrom != null && CRCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (patientpayment.AddedDate  >= '" + CRCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "' and patientpayment.AddedDate  < '" + CRCollectionReport.PostedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.PostedDateFrom != null && CRCollectionReport.PostedDateTo == null)
                    {
                        oString += (" and ( patientpayment.AddedDate  >= '" + CRCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CRCollectionReport.PostedDateFrom == null && CRCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (patientpayment.AddedDate  <= '" + CRCollectionReport.PostedDateTo.GetValueOrDefault().Date + "')");
                    }

                    oString += ("Group By patientpayment.ID, pra.[Name],patientpayment.AddedBy,isnull(patientpayment.PaymentAmount,0)  ,isnull( c.TotalAmount, 0) , case 		when patientpayment.PaymentMethod = 'Cash' then 'Cash' 	when patientpayment.PaymentMethod = 'Check' then patientpayment.CheckNumber		when patientpayment.PaymentMethod = 'Credit Card' then patientpayment.CCTransactionID				else ''  end  , isnull( pPaymentCharge.AllocatedAmount ,0 ) ,pra.ID ,pra.ClientID , pat.LastName , pat.FirstName , pat.AccountNum , visit.ID , pat.LastName , pat.FirstName ,  convert(varchar, visit.DateOfServiceFrom, 101) , cpt.CPTCode , c.Units , convert(varchar, patientpayment.PaymentDate, 101)  ,convert(varchar, patientpayment.AddedDate, 101)  , pro.[Name] ,lo.[Name] ,RefPr.[Name] ,pat.ID , c.ID         ");


                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {

                            var chaPayment = new GRCollectionReport();
                            chaPayment.SrNo = Convert.ToInt64(oReader["SrNo"].ToString());
                            chaPayment.AppliedAmount = Convert.ToDecimal(oReader["AppliedAmount"].ToString());
                            chaPayment.CheckDate = oReader["CheckDate"].ToString();
                            chaPayment.CheckNumber = oReader["CheckNumber"].ToString();
                            chaPayment.PayerName = oReader["PayerName"].ToString();
                            chaPayment.PostingDate = oReader["PostingDate"].ToString();
                            chaPayment.PracticeName = oReader["PracticeName"].ToString();
                            chaPayment.PostingUserName = oReader["PostingUserName"].ToString();
                            chaPayment.CheckAmount = Convert.ToDecimal(oReader["CheckAmount"].ToString());


                            data.Add(chaPayment);
                        }
                        myConnection.Close(); myConnection.Close();
                    }
                }



                //List<GRCollectionReport> data2 = (from paymentcheck in _context.PaymentCheck
                //                                  join practice in _context.Practice
                //                                  on paymentcheck.PracticeID equals practice.ID
                //                                  join pVisit in _context.PaymentVisit on paymentcheck.ID equals pVisit.PaymentCheckID
                //                                  join pCharge in _context.PaymentCharge on pVisit.ID equals pCharge.PaymentVisitID
                //                                  where
                //                                  (ExtensionMethods.IsBetweenDOS(CRCollectionReport.PostedDateTo, CRCollectionReport.PostedDateFrom, pCharge.PostedDate, pCharge.PostedDate))
                //                                  && (CRCollectionReport.CheckNo.IsNull() ? true : paymentcheck.CheckNumber.ToUpper().Equals(CRCollectionReport.CheckNo))
                //                                  && (ExtensionMethods.IsBetweenDOS(CRCollectionReport.CheckDateTo, CRCollectionReport.CheckDateFrom, paymentcheck.CheckDate, paymentcheck.CheckDate))
                //                                  && (CRCollectionReport.UserPosted.IsNull() ? true : paymentcheck.PostedBy.IsNull() ? false : paymentcheck.PostedBy.ToUpper().Equals(CRCollectionReport.UserPosted))
                //                                  // 
                //                                  && (practice.ID == UD.PracticeID)
                //                                  && (practice.ClientID == UD.ClientID) && pCharge.Status == "P"


                //                                  group new
                //                                  {
                //                                      SrNo = paymentcheck.ID,
                //                                      AppliedAmount = pCharge.PaidAmount,
                //                                      CheckDate = paymentcheck.CheckDate.Format("MM/dd/yyyy"),
                //                                      CheckNumber = paymentcheck.CheckNumber,
                //                                      PayerName = paymentcheck.PayerName,
                //                                      PostingDate = pCharge.PostedDate.Format("MM/dd/yyyy"),
                //                                      PracticeName = practice.Name,
                //                                      PostingUserName = paymentcheck.PostedBy,
                //                                      PracticeID = practice.ID,
                //                                      ClientID = practice.ClientID,
                //                                      CheckAmount = paymentcheck.CheckAmount,

                //                                  } by new { PostingDate = pCharge.PostedDate.Format("MM/dd/yyyy"), paymentcheck.CheckNumber } into gp
                //                                  select new GRCollectionReport
                //                                  {
                //                                      SrNo = gp.Select(a => a.SrNo).FirstOrDefault(),
                //                                      AppliedAmount = gp.Sum(a => a.AppliedAmount),
                //                                      CheckDate = gp.Select(a => a.CheckDate).FirstOrDefault(),
                //                                      CheckNumber = gp.Select(a => a.CheckNumber).FirstOrDefault(),
                //                                      PayerName = gp.Select(a => a.PayerName).FirstOrDefault(),
                //                                      PostingDate = gp.Select(a => a.PostingDate).FirstOrDefault(),
                //                                      PracticeName = gp.Select(a => a.PracticeName).FirstOrDefault(),
                //                                      PostingUserName = gp.Select(a => a.PostingUserName).FirstOrDefault(),
                //                                      PracticeID = gp.Select(a => a.PracticeID).FirstOrDefault(),
                //                                      ClientID = gp.Select(a => a.ClientID).FirstOrDefault(),
                //                                      CheckAmount = gp.Select(a => a.CheckAmount).FirstOrDefault(),
                //                                  }).ToList();

                //if (CRCollectionReport.DOSDateTo.HasValue && CRCollectionReport.DOSDateFrom.HasValue || CRCollectionReport.ProviderID != null)
                //{
                //    data2 = (from d in data2
                //             join v in _context.Visit
                //             on d.PracticeID equals v.PracticeID
                //             join p in _context.Provider
                //             on d.PracticeID equals p.PracticeID
                //             where (ExtensionMethods.IsBetweenDOS(CRCollectionReport.DOSDateTo, CRCollectionReport.DOSDateFrom, v.DateOfServiceFrom, v.DateOfServiceFrom))
                //             && (CRCollectionReport.RefProviderID.IsNull() ? true : v.RefProviderID.Equals(CRCollectionReport.RefProviderID))
                //             select d).Distinct().ToList();
                //}
                List<GRCollectionReport> data2 = new List<GRCollectionReport>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    string oString = "select paymentcheck.ID as SrNo,  sum(isnull(pCharge.PaidAmount,0)) as AppliedAmount, convert(varchar, paymentcheck.CheckDate, 101)  as CheckDate, paymentcheck.CheckNumber as CheckNumber,paymentcheck.PayerName as PayerName, convert(varchar, pCharge.PostedDate, 101) as PostingDate,pra.[Name] as PracticeName, paymentcheck.PostedBy as PostingUserName, pra.ID as PracticeID,pra.ClientID as ClientID, isnull(paymentcheck.CheckAmount,0) as CheckAmount from PaymentCheck paymentcheck join Practice pra on paymentcheck.PracticeID = pra.ID join PaymentVisit pVisit on paymentcheck.ID = pVisit.PaymentCheckID  join PaymentCharge pCharge on pVisit.ID = pCharge.PaymentVisitID join Charge c on  pCharge.ChargeID = c.ID join Visit visit on  c.VisitID = visit.ID join Patient pat on visit.PatientID = pat.ID join Cpt cpt on c.CPTID = cpt.ID join Practice practice on paymentcheck.PracticeID = practice.ID join [Provider] pro on visit.ProviderID = pro.ID left join [Location] lo on c.LocationID = lo.ID left join RefProvider RefPr on c.RefProviderID = RefPr.ID left join Modifier mod1 on c.Modifier1ID = mod1.ID left join Modifier mod2 on c.Modifier2ID = mod2.ID where pCharge.status = 'P' and pCharge.chargeid is not null";

                    oString += "  and c.practiceid = {0} ";
                    oString = string.Format(oString, PracticeId);

                    if (!CRCollectionReport.ProviderID.IsNull())
                        oString += string.Format(" and pro.ID = '{0}'", CRCollectionReport.ProviderID);
                    if (!CRCollectionReport.LocationID.IsNull())
                        oString += string.Format(" and lo.ID = '{0}'", CRCollectionReport.LocationID);
                    if (!CRCollectionReport.CheckNo.IsNull())
                        oString += string.Format(" and paymentcheck.CheckNumber = '{0}'", CRCollectionReport.CheckNo);
                    if (!CRCollectionReport.RefProviderID.IsNull())
                        oString += string.Format(" and visit.RefProviderID = '{0}'", CRCollectionReport.RefProviderID);
                    if (!CRCollectionReport.UserPosted.IsNull())
                        oString += string.Format(" and paymentcheck.PostedBy ='{0}'", CRCollectionReport.UserPosted);


                    if (CRCollectionReport.DOSDateFrom != null && CRCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  >= '" + CRCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "' and visit.DateOfServiceFrom  < '" + CRCollectionReport.DOSDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.DOSDateFrom != null && CRCollectionReport.DOSDateTo == null)
                    {
                        oString += (" and ( visit.DateOfServiceFrom  >= '" + CRCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CRCollectionReport.DOSDateFrom == null && CRCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  <= '" + CRCollectionReport.DOSDateTo.GetValueOrDefault().Date + "')");
                    }

                    if (CRCollectionReport.CheckDateFrom != null && CRCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (paymentcheck.CheckDate  >= '" + CRCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "' and paymentcheck.CheckDate  < '" + CRCollectionReport.CheckDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.CheckDateFrom != null && CRCollectionReport.CheckDateTo == null)
                    {
                        oString += (" and ( paymentcheck.CheckDate  >= '" + CRCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CRCollectionReport.CheckDateFrom == null && CRCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (paymentcheck.CheckDate  <= '" + CRCollectionReport.CheckDateTo.GetValueOrDefault().Date + "')");
                    }







                    if (CRCollectionReport.PostedDateFrom != null && CRCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (pCharge.PostedDate  >= '" + CRCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "' and pCharge.PostedDate  < '" + CRCollectionReport.PostedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CRCollectionReport.PostedDateFrom != null && CRCollectionReport.PostedDateTo == null)
                    {
                        oString += (" and ( pCharge.PostedDate  >= '" + CRCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CRCollectionReport.PostedDateFrom == null && CRCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (pCharge.PostedDate  <= '" + CRCollectionReport.PostedDateTo.GetValueOrDefault().Date + "')");
                    }

                    oString += "GROUP BY paymentcheck.ID, convert(varchar, paymentcheck.CheckDate, 101) ,paymentcheck.CheckNumber,paymentcheck.PayerName,  convert(varchar, pCharge.PostedDate, 101) ,pra.[Name], paymentcheck.PostedBy , pra.ID,pra.ClientID, paymentcheck.CheckAmount ";


                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {

                            var chaPayment = new GRCollectionReport();
                            chaPayment.SrNo = Convert.ToInt64(oReader["SrNo"].ToString());
                            chaPayment.AppliedAmount = Convert.ToDecimal(oReader["AppliedAmount"].ToString());
                            chaPayment.CheckDate = oReader["CheckDate"].ToString();
                            chaPayment.CheckNumber = oReader["CheckNumber"].ToString();
                            chaPayment.PayerName = oReader["PayerName"].ToString();
                            chaPayment.PostingDate = oReader["PostingDate"].ToString();
                            chaPayment.PracticeName = oReader["PracticeName"].ToString();
                            chaPayment.PostingUserName = oReader["PostingUserName"].ToString();
                            chaPayment.CheckAmount = Convert.ToDecimal(oReader["CheckAmount"].ToString());

                            data2.Add(chaPayment);
                        }
                        myConnection.Close();
                    }
                }


                List<GRCollectionReport> total = data2.Union(data).ToList();
                return total;
            }


        }



        private string GetPaymentMethod(string CCTransactionID, string CheckNumber, string PaymentMethod)
        {

            if (PaymentMethod == "Cash")
            {
                return "Cash";
            }
            else if (PaymentMethod == "Check")
            {
                return CheckNumber;
            }
            else if (PaymentMethod == "Credit Card")
            {
                return CCTransactionID;
            }
            else return "Cash";
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CRCollectionReport CRCollectionReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRCollectionReport> data = FindCollections(CRCollectionReport, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CRCollectionReport, "Collection Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CRCollectionReport CRCollectionReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRCollectionReport> data = FindCollections(CRCollectionReport, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }

        private bool CheckNullLong(long? Value)
        {
            Debug.WriteLine(Value.IsNull());
            return Value.IsNull();
        }
        private IConfiguration configuration;

        public string GetConnectionStringManager(string contextName)
        {
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("MedifusionLocal");
            string[] splitString = connectionString.Split(';');
            splitString[1] = splitString[1];

            if (contextName.IsNull())
                contextName = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;

            connectionString = splitString[0] + "; " + splitString[1] + contextName + "; " + splitString[2] + "; " + splitString[3];
            return connectionString;
            //var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //return builder.Build().GetSection("ConnectionStrings").GetSection("MedifusionLocal").Value;
        }


    }
}