using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Controllers;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static MediFusionPM.ReportViewModels.RVMDetailedCollectionReport;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.ReportControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RDetailedCollectionReportController : ControllerBase
    {

        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public RDetailedCollectionReportController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }

        [Route("FindDetailedCollections")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRDetailedCollectionReport>>> FindDetailedCollections(CRDetailedCollectionReport cRDetailedCollectionReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //  if (UD == null || UD.Rights == null || UD.Rights.SchedulerSearch == false)
            // return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return FindDetailedCollections(cRDetailedCollectionReport, UD);
        }

        private List<GRDetailedCollectionReport> FindDetailedCollections(CRDetailedCollectionReport cRDetailedCollectionReport, UserInfoData UD)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Client = (from w in _contextMain.MainPractice
                          where w.ID == PracticeId
                          select w
                        ).FirstOrDefault();
            string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
            string newConnection = GetConnectionStringManager(contextName);

            if (cRDetailedCollectionReport.CollectionType == "PATIENT")
            {

                List<GRDetailedCollectionReport> chargePayment = new List<GRDetailedCollectionReport>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    string oString = "select  isnull(icd1t.ICDCode, '') as dx1, isnull(icd2t.ICDCode, '') as dx2, isnull(icd3t.ICDCode, '') as dx3, isnull(icd4t.ICDCode, '') as dx4,isnull(patientpayment.PaymentAmount,0)  as CheckAmount ,isnull( c.TotalAmount, 0) as ChargeAmount, SUM( isnull( c.PrimaryAllowed, 0) +	isnull( c.SecondaryAllowed	, 0) +isnull( c.TertiaryAllowed	, 0)) as AllowedAmount, SUM( isnull( c.PrimaryPaid, 0) +	isnull( c.SecondaryPaid	, 0) +isnull( c.TertiaryPaid	, 0)) as PaidAmount,SUM( isnull( c.PrimaryWriteOff, 0) +	isnull( c.SecondaryWriteOff	, 0) + isnull( c.TertiaryWriteOff	, 0)) as AdjustmentAmount,case 		when patientpayment.PaymentMethod = 'Cash' then 'Cash' 	when patientpayment.PaymentMethod = 'Check' then patientpayment.CheckNumber		when patientpayment.PaymentMethod = 'Credit Card' then patientpayment.CCTransactionID				else ''  end 	as CheckNumber , isnull(pPaymentCharge.AllocatedAmount,0)  as AppliedPayments , pra.ID as PracticeID,pra.ClientID as ClientID,(pat.LastName + ', ' + pat.FirstName) as PayerName, pat.AccountNum as PatientAccountNo, visit.ID as VisitNo, pat.LastName as PatientLastName, pat.FirstName as PatientFirstName, convert(varchar, visit.DateOfServiceFrom, 101) as DOS, cpt.CPTCode as CPT, c.Units as CPTUnits,convert(varchar, patientpayment.PaymentDate, 101)  as CheckDate,convert(varchar, patientpayment.AddedDate, 101)as PaymentEntryDate,pro.[Name] as [Provider],lo.[Name] as [Location],RefPr.[Name] as ReferringProvider,pat.ID as PatientId, c.ID as ChargeId   from PatientPayment patientpayment join Visit visit  on   patientpayment.VisitID =visit.ID join Charge c on   visit.ID = c.ID  left join PatientPaymentCharge pPaymentCharge on patientpayment.ID = pPaymentCharge.PatientPaymentID left join Patient pat on visit.PatientID = pat.ID left join Cpt cpt on c.CPTID = cpt.ID left join Practice pra on visit.PracticeID = pra.ID left join [Provider] pro on visit.ProviderID = pro.ID left join [Location] lo on c.LocationID = lo.ID  left join RefProvider RefPr on c.RefProviderID = RefPr.ID   left join ICD icd1t on Visit.ICD1ID = icd1t.ID left join ICD icd2t on Visit.ICD2ID = icd2t.ID left join ICD icd3t on Visit.ICD3ID = icd3t.ID  left join ICD icd4t on Visit.ICD4ID = icd4t.ID  ";

                    oString += "  where c.practiceid = {0} ";
                    oString = string.Format(oString, PracticeId);

                    if (!cRDetailedCollectionReport.ProviderID.IsNull())
                        oString += string.Format(" and pro.ID = '{0}'", cRDetailedCollectionReport.ProviderID);

                    if (!cRDetailedCollectionReport.LocationID.IsNull())
                        oString += string.Format(" and lo.ID = '{0}'", cRDetailedCollectionReport.LocationID);
                    if (!cRDetailedCollectionReport.CheckNo.IsNull() || cRDetailedCollectionReport.CheckNo.Equals("CASH"))
                        oString += string.Format(" and patientpayment.CheckNumber = '{0}'", cRDetailedCollectionReport.CheckNo);
                    if (!cRDetailedCollectionReport.RefProviderID.IsNull())
                        oString += string.Format(" and visit.RefProviderID = '{0}'", cRDetailedCollectionReport.RefProviderID);
                    if (!cRDetailedCollectionReport.UserPosted.IsNull())
                        oString += string.Format(" and patientpayment.AddedBy like '%{0}%'", cRDetailedCollectionReport.UserPosted);



                    if (cRDetailedCollectionReport.DOSDateFrom != null && cRDetailedCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  >= '" + cRDetailedCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "' and visit.DateOfServiceFrom  < '" + cRDetailedCollectionReport.DOSDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.DOSDateFrom != null && cRDetailedCollectionReport.DOSDateTo == null)
                    {
                        oString += (" and ( visit.DateOfServiceFrom  >= '" + cRDetailedCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.DOSDateFrom == null && cRDetailedCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  <= '" + cRDetailedCollectionReport.DOSDateTo.GetValueOrDefault().Date + "')");
                    }


                    if (cRDetailedCollectionReport.CheckDateFrom != null && cRDetailedCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (patientpayment.PaymentDate  >= '" + cRDetailedCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "' and patientpayment.PaymentDate  < '" + cRDetailedCollectionReport.CheckDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.CheckDateFrom != null && cRDetailedCollectionReport.CheckDateTo == null)
                    {
                        oString += (" and ( patientpayment.PaymentDate  >= '" + cRDetailedCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.CheckDateFrom == null && cRDetailedCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (patientpayment.PaymentDate  <= '" + cRDetailedCollectionReport.CheckDateTo.GetValueOrDefault().Date + "')");
                    }



                    if (cRDetailedCollectionReport.PostedDateFrom != null && cRDetailedCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (patientpayment.AddedDate  >= '" + cRDetailedCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "' and patientpayment.AddedDate  < '" + cRDetailedCollectionReport.PostedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.PostedDateFrom != null && cRDetailedCollectionReport.PostedDateTo == null)
                    {
                        oString += (" and ( patientpayment.AddedDate  >= '" + cRDetailedCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.PostedDateFrom == null && cRDetailedCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (patientpayment.AddedDate  <= '" + cRDetailedCollectionReport.PostedDateTo.GetValueOrDefault().Date + "')");
                    }

                    oString += ("Group By icd1t.ICDCode, icd2t.ICDCode,icd3t.ICDCode, icd4t.ICDCode, patientpayment.ID, pra.[Name],patientpayment.AddedBy,isnull(patientpayment.PaymentAmount,0)  ,isnull( c.TotalAmount, 0) , case 		when patientpayment.PaymentMethod = 'Cash' then 'Cash' 	when patientpayment.PaymentMethod = 'Check' then patientpayment.CheckNumber		when patientpayment.PaymentMethod = 'Credit Card' then patientpayment.CCTransactionID				else ''  end  , isnull( pPaymentCharge.AllocatedAmount ,0 ) ,pra.ID ,pra.ClientID , pat.LastName , pat.FirstName , pat.AccountNum , visit.ID , pat.LastName , pat.FirstName ,  convert(varchar, visit.DateOfServiceFrom, 101) , cpt.CPTCode , c.Units , convert(varchar, patientpayment.PaymentDate, 101)  ,convert(varchar, patientpayment.AddedDate, 101)  , pro.[Name] ,lo.[Name] ,RefPr.[Name] ,pat.ID , c.ID   ");


                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {

                            var chaPayment = new GRDetailedCollectionReport();
                            chaPayment.CheckAmount = Convert.ToDecimal(oReader["CheckAmount"].ToString());
                            chaPayment.PayerName = oReader["PayerName"].ToString();
                            chaPayment.PatientName = oReader["PatientLastName"].ToString() + ", " + oReader["PatientFirstName"].ToString();
                            chaPayment.PatientAccountNo = oReader["PatientAccountNo"].ToString();
                            chaPayment.VisitNo = Convert.ToInt64(oReader["VisitNo"].ToString());
                            chaPayment.DOS = oReader["DOS"].ToString();
                            chaPayment.CPT = oReader["CPT"].ToString();
                            chaPayment.CPTUnits = oReader["CPTUnits"].ToString();
                            chaPayment.ChargeAmount = Convert.ToDecimal(oReader["ChargeAmount"].ToString());
                            chaPayment.PaidAmount = Convert.ToDecimal(oReader["PaidAmount"].ToString());
                            chaPayment.AdjustmentAmount = Convert.ToDecimal(oReader["AdjustmentAmount"].ToString());
                            chaPayment.CheckDate = oReader["CheckDate"].ToString();
                            chaPayment.CheckNumber = oReader["CheckNumber"].ToString();
                            chaPayment.PaymentEntryDate = oReader["PaymentEntryDate"].ToString();
                            chaPayment.AppliedPayments = Convert.ToDecimal(oReader["AppliedPayments"].ToString());
                            chaPayment.Provider = oReader["Provider"].ToString();
                            chaPayment.Location = oReader["Location"].ToString();
                            chaPayment.ReferringProvider = oReader["ReferringProvider"].ToString();
                            chaPayment.PatientId = Convert.ToInt64(oReader["PatientId"].ToString());
                            chaPayment.ChargeId = Convert.ToInt64(oReader["ChargeId"].ToString());
                            chaPayment.dx1 = oReader["dx1"].ToString();
                            chaPayment.dx2 = oReader["dx2"].ToString();
                            chaPayment.dx3 = oReader["dx3"].ToString();
                            chaPayment.dx4 = oReader["dx4"].ToString();

                            chargePayment.Add(chaPayment);
                        }
                        myConnection.Close();
                    }
                }

                return chargePayment;



                //List<GRDetailedCollectionReport> data = (from visit in _context.Visit
                //                                         join c in _context.Charge on visit.ID equals c.VisitID
                //                                         join patientpayment in _context.PatientPayment on visit.ID equals patientpayment.VisitID
                //                                         join pPaymentCharge in _context.PatientPaymentCharge on patientpayment.ID equals pPaymentCharge.PatientPaymentID
                //                                         join pro in _context.Provider on visit.ProviderID equals pro.ID
                //                                         join practice in _context.Practice on visit.PracticeID equals practice.ID

                //                                         join pat in _context.Patient on visit.PatientID equals pat.ID
                //                                         join cpt in _context.Cpt on c.CPTID equals cpt.ID
                //                                         join loc in _context.Location on visit.LocationID equals loc.ID
                //                                         join RefPr in _context.RefProvider on visit.RefProviderID equals RefPr.ID into Table2
                //                                         from RefPro in Table2.DefaultIfEmpty()
                //                                         //join mod1 in _context.Modifier on c.Modifier1ID equals mod1.ID into Table4
                //                                         //from modi1 in Table4.DefaultIfEmpty()
                //                                         //join mod2 in _context.Modifier on c.Modifier2ID equals mod2.ID into Table5
                //                                         //from modi2 in Table5.DefaultIfEmpty()
                //                                         where pPaymentCharge.ChargeID == c.ID && 
                //                                         (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.DOSDateTo, cRDetailedCollectionReport.DOSDateFrom, visit.DateOfServiceTo, visit.DateOfServiceFrom)) &&
                //                                         (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.PostedDateTo, cRDetailedCollectionReport.PostedDateFrom, patientpayment.AddedDate, patientpayment.AddedDate))
                //                                     && (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.DOSDateTo, cRDetailedCollectionReport.DOSDateFrom, visit.DateOfServiceFrom, visit.DateOfServiceFrom))
                //                                     && (cRDetailedCollectionReport.ProviderID.IsNull() ? true : visit.ProviderID.Equals(cRDetailedCollectionReport.ProviderID))
                //                                     && (cRDetailedCollectionReport.CheckNo.IsNull() || cRDetailedCollectionReport.CheckNo.Equals("CASH") ? true : patientpayment.CheckNumber.IsNull() ? false : patientpayment.CheckNumber.Equals(cRDetailedCollectionReport.CheckNo))
                //                                     && (cRDetailedCollectionReport.UserPosted.IsNull() ? true : patientpayment.AddedBy.IsNull() ? false : patientpayment.AddedBy.Contains(cRDetailedCollectionReport.UserPosted))
                //                                     && (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.CheckDateTo, cRDetailedCollectionReport.CheckDateFrom, patientpayment.PaymentDate, patientpayment.PaymentDate))
                //                                     && (cRDetailedCollectionReport.RefProviderID.IsNull() ? true : visit.RefProviderID.Equals(cRDetailedCollectionReport.RefProviderID))
                //                                     && (practice.ID == UD.PracticeID)
                //                                         select new GRDetailedCollectionReport
                //                                         {
                //                                             CheckAmount = patientpayment.PaymentAmount,//v
                //                                             PracticeID = practice.ID,
                //                                             ClientID = practice.ClientID,
                //                                             PayerName = pat.LastName + ", " + pat.FirstName, //v
                //                                             PatientAccountNo = pat.AccountNum,
                //                                             VisitNo = visit.ID,
                //                                             PatientName = pat.LastName + ", " + pat.FirstName,
                //                                             DOS = visit.DateOfServiceFrom.Format("MM/dd/yyyy"),
                //                                             CPT = cpt.CPTCode,
                //                                             CPTUnits = c.Units,
                //                                             //Modifier1 = modi1.Code,
                //                                             //Modifier2 = modi2.Code,
                //                                             ChargeAmount = c.TotalAmount,
                //                                             AllowedAmount = c.PrimaryAllowed.Val() + c.SecondaryAllowed.Val() + c.TertiaryAllowed.Val(),
                //                                             PaidAmount = c.PrimaryPaid.Val() + c.SecondaryPaid.Val() + c.TertiaryPaid.Val(),
                //                                             AdjustmentAmount = c.PrimaryWriteOff.Val() + c.SecondaryWriteOff.Val() + c.TertiaryWriteOff.Val(),
                //                                             CheckNumber = GetPaymentMethod(patientpayment.CCTransactionID, patientpayment.CheckNumber, patientpayment.PaymentMethod), //verified
                //                                             CheckDate = patientpayment.PaymentDate.Format("MM/dd/yyyy"),
                //                                             PaymentEntryDate = patientpayment.AddedDate.Format("MM/dd/yyyy"),
                //                                             AppliedPayments = pPaymentCharge.AllocatedAmount,
                //                                             Provider = pro.Name,
                //                                             Location = loc.Name,
                //                                             ReferringProvider = RefPro.Name,
                //                                             PatientId = pat.ID,
                //                                             ChargeId = c.ID

                //                                         }).Distinct().ToList();
                //return data;

            }
            else if (cRDetailedCollectionReport.CollectionType == "PAYER")
            {

                List<GRDetailedCollectionReport> chargePayment = new List<GRDetailedCollectionReport>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    string oString = "select  isnull(icd1t.ICDCode, '') as dx1, isnull(icd2t.ICDCode, '') as dx2, isnull(icd3t.ICDCode, '') as dx3, isnull(icd4t.ICDCode, '') as dx4,isnull(paymentcheck.CheckAmount,0)  as CheckAmount, ISNULL( pCharge.PaidAmount , 0) as PaidAmount,isnull( pCharge.PaidAmount ,0 )as AppliedPayments,case 	 when pVisit.ProcessedAs = 1 then isnull( c.PrimaryBilledAmount, 0)	when pVisit.ProcessedAs = 2 then isnull(c.SecondaryBilledAmount, 0)	else 0  end 		as ChargeAmount,case	when pVisit.ProcessedAs = 1 then isnull( c.PrimaryAllowed, 0)		when pVisit.ProcessedAs = 2 then isnull( c.SecondaryAllowed	, 0)	else 0  end 		as AllowedAmount, case	when pVisit.ProcessedAs = 1 then isnull( c.PrimaryWriteOff, 0)		when pVisit.ProcessedAs = 2 then isnull( c.SecondaryWriteOff , 0)		else 0  end 		as AdjustmentAmount, practice.ID as PracticeID, practice.ClientID as ClientID,paymentcheck.PayerName as PayerName, pat.AccountNum as PatientAccountNo, visit.ID as VisitNo,pat.LastName as PatientLastName, pat.FirstName as PatientFirstName,convert(varchar, visit.DateOfServiceFrom, 101) as DOS, cpt.CPTCode as CPT, c.Units as CPTUnits,mod1.Code as Modifier1, mod2.Code as Modifier2, paymentcheck.CheckNumber as CheckNumber,convert(varchar, paymentcheck.CheckDate, 101)  as CheckDate,convert(varchar, pCharge.PostedDate, 101)  as PaymentEntryDate,pro.[Name] as [Provider], lo.[Name] as [Location], RefPr.[Name] as ReferringProvider,pat.ID as PatientId, c.ID as ChargeId from PaymentCheck paymentcheck join Practice pra on paymentcheck.PracticeID = pra.ID join PaymentVisit pVisit on paymentcheck.ID = pVisit.PaymentCheckID  join PaymentCharge pCharge on pVisit.ID = pCharge.PaymentVisitID join Charge c on  pCharge.ChargeID = c.ID join Visit visit on  c.VisitID = visit.ID join Patient pat on visit.PatientID = pat.ID join Cpt cpt on c.CPTID = cpt.ID join Practice practice on paymentcheck.PracticeID = practice.ID join [Provider] pro on visit.ProviderID = pro.ID left join [Location] lo on c.LocationID = lo.ID left join RefProvider RefPr on c.RefProviderID = RefPr.ID left join Modifier mod1 on c.Modifier1ID = mod1.ID left join Modifier mod2 on c.Modifier2ID = mod2.ID left join ICD icd1t on Visit.ICD1ID = icd1t.ID left join ICD icd2t on Visit.ICD2ID = icd2t.ID left join ICD icd3t on Visit.ICD3ID = icd3t.ID  left join ICD icd4t on Visit.ICD4ID = icd4t.ID where pCharge.status = 'P' and pCharge.chargeid is not null    ";

                    oString += "  and c.practiceid = {0} ";
                    oString = string.Format(oString, PracticeId);

                    if (!cRDetailedCollectionReport.ProviderID.IsNull())
                        oString += string.Format(" and pro.ID = '{0}'", cRDetailedCollectionReport.ProviderID);
                    if (!cRDetailedCollectionReport.LocationID.IsNull())
                        oString += string.Format(" and lo.ID = '{0}'", cRDetailedCollectionReport.LocationID);
                    if (!cRDetailedCollectionReport.CheckNo.IsNull())
                        oString += string.Format(" and paymentcheck.CheckNumber = '{0}'", cRDetailedCollectionReport.CheckNo);
                    if (!cRDetailedCollectionReport.RefProviderID.IsNull())
                        oString += string.Format(" and visit.RefProviderID = '{0}'", cRDetailedCollectionReport.RefProviderID);
                    if (!cRDetailedCollectionReport.UserPosted.IsNull())
                        oString += string.Format(" and paymentcheck.PostedBy ='{0}'", cRDetailedCollectionReport.UserPosted);


                    if (cRDetailedCollectionReport.DOSDateFrom != null && cRDetailedCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  >= '" + cRDetailedCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "' and visit.DateOfServiceFrom  < '" + cRDetailedCollectionReport.DOSDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.DOSDateFrom != null && cRDetailedCollectionReport.DOSDateTo == null)
                    {
                        oString += (" and ( visit.DateOfServiceFrom  >= '" + cRDetailedCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.DOSDateFrom == null && cRDetailedCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  <= '" + cRDetailedCollectionReport.DOSDateTo.GetValueOrDefault().Date + "')");
                    }

                    if (cRDetailedCollectionReport.CheckDateFrom != null && cRDetailedCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (paymentcheck.CheckDate  >= '" + cRDetailedCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "' and paymentcheck.CheckDate  < '" + cRDetailedCollectionReport.CheckDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.CheckDateFrom != null && cRDetailedCollectionReport.CheckDateTo == null)
                    {
                        oString += (" and ( paymentcheck.CheckDate  >= '" + cRDetailedCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.CheckDateFrom == null && cRDetailedCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (paymentcheck.CheckDate  <= '" + cRDetailedCollectionReport.CheckDateTo.GetValueOrDefault().Date + "')");
                    }







                    if (cRDetailedCollectionReport.PostedDateFrom != null && cRDetailedCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (pCharge.PostedDate  >= '" + cRDetailedCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "' and pCharge.PostedDate  < '" + cRDetailedCollectionReport.PostedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.PostedDateFrom != null && cRDetailedCollectionReport.PostedDateTo == null)
                    {
                        oString += (" and ( pCharge.PostedDate  >= '" + cRDetailedCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.PostedDateFrom == null && cRDetailedCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (pCharge.PostedDate  <= '" + cRDetailedCollectionReport.PostedDateTo.GetValueOrDefault().Date + "')");
                    }




                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {

                            var chaPayment = new GRDetailedCollectionReport();
                            chaPayment.CheckAmount = Convert.ToDecimal(oReader["CheckAmount"].ToString());
                            chaPayment.PayerName = oReader["PayerName"].ToString();
                            chaPayment.PatientName = oReader["PatientLastName"].ToString() + ", " + oReader["PatientFirstName"].ToString();
                            chaPayment.PatientAccountNo = oReader["PatientAccountNo"].ToString();
                            chaPayment.VisitNo = Convert.ToInt64(oReader["VisitNo"].ToString());
                            chaPayment.DOS = oReader["DOS"].ToString();
                            chaPayment.CPT = oReader["CPT"].ToString();
                            chaPayment.CPTUnits = oReader["CPTUnits"].ToString();
                            chaPayment.Modifier1 = oReader["Modifier1"].ToString();
                            chaPayment.Modifier2 = oReader["Modifier2"].ToString();
                            chaPayment.ChargeAmount = Convert.ToDecimal(oReader["ChargeAmount"].ToString());
                            chaPayment.PaidAmount = Convert.ToDecimal(oReader["PaidAmount"].ToString());
                            chaPayment.AdjustmentAmount = Convert.ToDecimal(oReader["AdjustmentAmount"].ToString());
                            chaPayment.CheckDate = oReader["CheckDate"].ToString();
                            chaPayment.CheckNumber = oReader["CheckNumber"].ToString();
                            chaPayment.PaymentEntryDate = oReader["PaymentEntryDate"].ToString();
                            chaPayment.AppliedPayments = Convert.ToDecimal(oReader["AppliedPayments"].ToString());
                            chaPayment.Provider = oReader["Provider"].ToString();
                            chaPayment.Location = oReader["Location"].ToString();
                            chaPayment.ReferringProvider = oReader["ReferringProvider"].ToString();
                            chaPayment.PatientId = Convert.ToInt64(oReader["PatientId"].ToString());
                            chaPayment.ChargeId = Convert.ToInt64(oReader["ChargeId"].ToString());

                            chaPayment.dx1 = oReader["dx1"].ToString();
                            chaPayment.dx2 = oReader["dx2"].ToString();
                            chaPayment.dx3 = oReader["dx3"].ToString();
                            chaPayment.dx4 = oReader["dx4"].ToString();
                            chargePayment.Add(chaPayment);
                        }
                        myConnection.Close();
                    }
                }

                return chargePayment;


                //List<GRDetailedCollectionReport> data = (from visit in _context.Visit
                //                                             //join patientpaymen in _context.PatientPayment on visit.ID equals patientpaymen.VisitID into Table1
                //                                             //from patientpayment in Table1.DefaultIfEmpty()
                //                                         join c in _context.Charge on visit.ID equals c.VisitID
                //                                         join pCharge in _context.PaymentCharge on c.ID equals pCharge.ChargeID
                //                                         join pVisit in _context.PaymentVisit on pCharge.PaymentVisitID equals pVisit.ID
                //                                         join paymentcheck in _context.PaymentCheck on pVisit.PaymentCheckID equals paymentcheck.ID
                //                                         join pat in _context.Patient on visit.PatientID equals pat.ID
                //                                         join cpt in _context.Cpt on c.CPTID equals cpt.ID
                //                                         join practice in _context.Practice on paymentcheck.PracticeID equals practice.ID
                //                                         join pro in _context.Provider on visit.ProviderID equals pro.ID

                //                                         //join pPaymentCharge in _context.PatientPaymentCharge on c.ID equals pPaymentCharge.ChargeID

                //                                         join lo in _context.Location on c.LocationID equals lo.ID into Table3
                //                                         from loc in Table3.DefaultIfEmpty()
                //                                         join RefPr in _context.RefProvider on c.RefProviderID equals RefPr.ID into Table2
                //                                         from RefPro in Table2.DefaultIfEmpty()
                //                                         join mod1 in _context.Modifier on c.Modifier1ID equals mod1.ID into Table4
                //                                         from modi1 in Table4.DefaultIfEmpty()
                //                                         join mod2 in _context.Modifier on c.Modifier2ID equals mod2.ID into Table5
                //                                         from modi2 in Table5.DefaultIfEmpty()
                //                                         where
                //                                          (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.DOSDateTo, cRDetailedCollectionReport.DOSDateFrom, visit.DateOfServiceTo, visit.DateOfServiceFrom)) &&
                //                                       (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.PostedDateTo, cRDetailedCollectionReport.PostedDateFrom, pCharge.PostedDate, pCharge.PostedDate))
                //                                      && (cRDetailedCollectionReport.CheckNo.IsNull() ? true : paymentcheck.CheckNumber.ToUpper().Equals(cRDetailedCollectionReport.CheckNo))
                //                                      && (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.CheckDateTo, cRDetailedCollectionReport.CheckDateFrom, paymentcheck.CheckDate, paymentcheck.CheckDate))
                //                                      && (cRDetailedCollectionReport.UserPosted.IsNull() ? true : pCharge.PostedBy.IsNull() ? false : pCharge.PostedBy.ToUpper().Equals(cRDetailedCollectionReport.UserPosted))
                //                                      && (practice.ID == UD.PracticeID)
                //                                      && (practice.ClientID == UD.ClientID) && paymentcheck.Status == "P"

                //                                         select new GRDetailedCollectionReport
                //                                         {
                //                                             CheckAmount = paymentcheck.CheckAmount,
                //                                             PracticeID = practice.ID,
                //                                             ClientID = practice.ClientID,
                //                                             PayerName = paymentcheck.PayerName,//v
                //                                             PatientAccountNo = pat.AccountNum,
                //                                             VisitNo = visit.ID,
                //                                             PatientName = pat.LastName + ", " + pat.FirstName,
                //                                             DOS = visit.DateOfServiceFrom.Format("MM/dd/yyyy"),
                //                                             CPT = cpt.CPTCode,
                //                                             CPTUnits = c.Units,
                //                                             Modifier1 = modi1.Code,
                //                                             Modifier2 = modi2.Code,
                //                                             ChargeAmount = pVisit.ProcessedAs.IsNull() || pVisit.ProcessedAs == "1" ? c.PrimaryBilledAmount :
                //                                                               pVisit.ProcessedAs == "2" ? c.SecondaryBilledAmount : null,

                //                                             //AllowedAmount = c.PrimaryAllowed.Val() + c.SecondaryAllowed.Val() + c.TertiaryAllowed.Val(), //
                //                                             AllowedAmount = pVisit.ProcessedAs.IsNull() || pVisit.ProcessedAs == "1" ? c.PrimaryAllowed :
                //                                                               pVisit.ProcessedAs == "2" ? c.SecondaryAllowed : null,


                //                                             //PaidAmount = c.PrimaryPaid.Val() + c.SecondaryPaid.Val() + c.TertiaryPaid.Val(),
                //                                             PaidAmount = pVisit.ProcessedAs.IsNull() || pVisit.ProcessedAs == "1" ? c.PrimaryPaid :
                //                                                               pVisit.ProcessedAs == "2" ? c.SecondaryPaid : null,


                //                                             //AdjustmentAmount = c.PrimaryWriteOff.Val() + c.SecondaryWriteOff.Val() + c.TertiaryWriteOff.Val(),
                //                                             AdjustmentAmount = pVisit.ProcessedAs.IsNull() || pVisit.ProcessedAs == "1" ? c.PrimaryWriteOff :
                //                                                               pVisit.ProcessedAs == "2" ? c.SecondaryWriteOff : null,

                //                                             CheckNumber = paymentcheck.CheckNumber, //v
                //                                             CheckDate = paymentcheck.CheckDate.Format("MM/dd/yyyy"),//v
                //                                             PaymentEntryDate = pCharge.PostedDate.Format("MM/dd/yyyy"),
                //                                             AppliedPayments = pCharge.PaidAmount,
                //                                             Provider = pro.Name,
                //                                             Location = loc.Name,
                //                                             ReferringProvider = RefPro.Name,
                //                                             PatientId = pat.ID,
                //                                             ChargeId = c.ID

                //                                         }).Distinct().ToList();
                //return data;
            }
            else
            {

                //List<GRDetailedCollectionReport> data = (from visit in _context.Visit
                //                                         join patientpayment in _context.PatientPayment on visit.ID equals patientpayment.VisitID
                //                                         join pPaymentCharge in _context.PatientPaymentCharge on patientpayment.ID equals pPaymentCharge.PatientPaymentID
                //                                         join pro in _context.Provider on visit.ProviderID equals pro.ID
                //                                         join practice in _context.Practice on visit.PracticeID equals practice.ID
                //                                         join c in _context.Charge on visit.ID equals c.VisitID
                //                                         join pat in _context.Patient on visit.PatientID equals pat.ID
                //                                         join cpt in _context.Cpt on c.CPTID equals cpt.ID

                //                                         join loc in _context.Location on visit.LocationID equals loc.ID
                //                                         join RefPr in _context.RefProvider on visit.RefProviderID equals RefPr.ID into Table2
                //                                         from RefPro in Table2.DefaultIfEmpty()
                //                                         //join mod1 in _context.Modifier on c.Modifier1ID equals mod1.ID into Table4
                //                                         //from modi1 in Table4.DefaultIfEmpty()
                //                                         //join mod2 in _context.Modifier on c.Modifier2ID equals mod2.ID into Table5
                //                                         //from modi2 in Table5.DefaultIfEmpty()
                //                                         where pPaymentCharge.ChargeID == c.ID &&
                //                                         (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.DOSDateTo, cRDetailedCollectionReport.DOSDateFrom, visit.DateOfServiceTo, visit.DateOfServiceFrom)) &&
                //                                         (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.PostedDateTo, cRDetailedCollectionReport.PostedDateFrom, patientpayment.AddedDate, patientpayment.AddedDate))
                //                                 && (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.DOSDateTo, cRDetailedCollectionReport.DOSDateFrom, visit.DateOfServiceFrom, visit.DateOfServiceFrom))
                //                                 && (cRDetailedCollectionReport.ProviderID.IsNull() ? true : visit.ProviderID.Equals(cRDetailedCollectionReport.ProviderID))
                //                                 && (cRDetailedCollectionReport.CheckNo.IsNull() || cRDetailedCollectionReport.CheckNo.Equals("CASH") ? true : patientpayment.CheckNumber.IsNull() ? false : patientpayment.CheckNumber.Equals(cRDetailedCollectionReport.CheckNo))
                //                                 && (cRDetailedCollectionReport.UserPosted.IsNull() ? true : patientpayment.AddedBy.IsNull() ? false : patientpayment.AddedBy.Contains(cRDetailedCollectionReport.UserPosted))
                //                                 && (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.CheckDateTo, cRDetailedCollectionReport.CheckDateFrom, patientpayment.PaymentDate, patientpayment.PaymentDate))
                //                                 && (cRDetailedCollectionReport.RefProviderID.IsNull() ? true : visit.RefProviderID.Equals(cRDetailedCollectionReport.RefProviderID))
                //                                 && (practice.ID == UD.PracticeID)
                //                                         select new GRDetailedCollectionReport
                //                                         {
                //                                             CheckAmount = patientpayment.PaymentAmount,//v
                //                                             PracticeID = practice.ID,
                //                                             ClientID = practice.ClientID,
                //                                             PayerName = pat.LastName + ", " + pat.FirstName,//v
                //                                             PatientAccountNo = pat.AccountNum,
                //                                             VisitNo = visit.ID,
                //                                             PatientName = pat.LastName + ", " + pat.FirstName,
                //                                             DOS = visit.DateOfServiceFrom.Format("MM/dd/yyyy"),
                //                                             CPT = cpt.CPTCode,
                //                                             CPTUnits = c.Units,
                //                                             //Modifier1 = modi1.Code,
                //                                             //Modifier2 = modi2.Code,
                //                                             ChargeAmount = c.TotalAmount,
                //                                             AllowedAmount = c.PrimaryAllowed.Val() + c.SecondaryAllowed.Val() + c.TertiaryAllowed.Val(),
                //                                             PaidAmount = c.PrimaryPaid.Val() + c.SecondaryPaid.Val() + c.TertiaryPaid.Val(),
                //                                             AdjustmentAmount = c.PrimaryWriteOff.Val() + c.SecondaryWriteOff.Val() + c.TertiaryWriteOff.Val(),
                //                                             CheckNumber = GetPaymentMethod(patientpayment.CCTransactionID, patientpayment.CheckNumber, patientpayment.PaymentMethod), //verified
                //                                             CheckDate = patientpayment.PaymentDate.Format("MM/dd/yyyy"),
                //                                             PaymentEntryDate = patientpayment.AddedDate.Format("MM/dd/yyyy"),
                //                                             AppliedPayments = pPaymentCharge.AllocatedAmount,
                //                                             Provider = pro.Name,
                //                                             Location = loc.Name,
                //                                             ReferringProvider = RefPro.Name,
                //                                             PatientId = pat.ID,
                //                                             ChargeId = c.ID

                //                                         }).Distinct().ToList();

                List<GRDetailedCollectionReport> data = new List<GRDetailedCollectionReport>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    string oString = "select  isnull(icd1t.ICDCode, '') as dx1, isnull(icd2t.ICDCode, '') as dx2, isnull(icd3t.ICDCode, '') as dx3, isnull(icd4t.ICDCode, '') as dx4,isnull(patientpayment.PaymentAmount,0)  as CheckAmount ,isnull( c.TotalAmount, 0) as ChargeAmount, SUM( isnull( c.PrimaryAllowed, 0) +	isnull( c.SecondaryAllowed	, 0) +isnull( c.TertiaryAllowed	, 0)) as AllowedAmount, SUM( isnull( c.PrimaryPaid, 0) +	isnull( c.SecondaryPaid	, 0) +isnull( c.TertiaryPaid	, 0)) as PaidAmount,SUM( isnull( c.PrimaryWriteOff, 0) +	isnull( c.SecondaryWriteOff	, 0) + isnull( c.TertiaryWriteOff	, 0)) as AdjustmentAmount,case 		when patientpayment.PaymentMethod = 'Cash' then 'Cash' 	when patientpayment.PaymentMethod = 'Check' then patientpayment.CheckNumber		when patientpayment.PaymentMethod = 'Credit Card' then patientpayment.CCTransactionID				else ''  end 	as CheckNumber , isnull(pPaymentCharge.AllocatedAmount,0)  as AppliedPayments , pra.ID as PracticeID,pra.ClientID as ClientID,(pat.LastName + ', ' + pat.FirstName) as PayerName, pat.AccountNum as PatientAccountNo, visit.ID as VisitNo, pat.LastName as PatientLastName, pat.FirstName as PatientFirstName, convert(varchar, visit.DateOfServiceFrom, 101) as DOS, cpt.CPTCode as CPT, c.Units as CPTUnits,convert(varchar, patientpayment.PaymentDate, 101)  as CheckDate,convert(varchar, patientpayment.AddedDate, 101)as PaymentEntryDate,pro.[Name] as [Provider],lo.[Name] as [Location],RefPr.[Name] as ReferringProvider,pat.ID as PatientId, c.ID as ChargeId   from PatientPayment patientpayment join Visit visit  on   patientpayment.VisitID =visit.ID join Charge c on   visit.ID = c.ID  left join PatientPaymentCharge pPaymentCharge on patientpayment.ID = pPaymentCharge.PatientPaymentID left join Patient pat on visit.PatientID = pat.ID left join Cpt cpt on c.CPTID = cpt.ID left join Practice pra on visit.PracticeID = pra.ID left join [Provider] pro on visit.ProviderID = pro.ID left join [Location] lo on c.LocationID = lo.ID  left join RefProvider RefPr on c.RefProviderID = RefPr.ID     left join ICD icd1t on Visit.ICD1ID = icd1t.ID left join ICD icd2t on Visit.ICD2ID = icd2t.ID left join ICD icd3t on Visit.ICD3ID = icd3t.ID  left join ICD icd4t on Visit.ICD4ID = icd4t.ID  ";

                    oString += "  where c.practiceid = {0} ";
                    oString = string.Format(oString, PracticeId);

                    if (!cRDetailedCollectionReport.ProviderID.IsNull())
                        oString += string.Format(" and pro.ID = '{0}'", cRDetailedCollectionReport.ProviderID);

                    if (!cRDetailedCollectionReport.LocationID.IsNull())
                        oString += string.Format(" and lo.ID = '{0}'", cRDetailedCollectionReport.LocationID);
                    if (!cRDetailedCollectionReport.CheckNo.IsNull() || cRDetailedCollectionReport.CheckNo.Equals("CASH"))
                        oString += string.Format(" and patientpayment.CheckNumber = '{0}'", cRDetailedCollectionReport.CheckNo);
                    if (!cRDetailedCollectionReport.RefProviderID.IsNull())
                        oString += string.Format(" and visit.RefProviderID = '{0}'", cRDetailedCollectionReport.RefProviderID);
                    if (!cRDetailedCollectionReport.UserPosted.IsNull())
                        oString += string.Format(" and patientpayment.AddedBy like '%{0}%'", cRDetailedCollectionReport.UserPosted);



                    if (cRDetailedCollectionReport.DOSDateFrom != null && cRDetailedCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  >= '" + cRDetailedCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "' and visit.DateOfServiceFrom  < '" + cRDetailedCollectionReport.DOSDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.DOSDateFrom != null && cRDetailedCollectionReport.DOSDateTo == null)
                    {
                        oString += (" and ( visit.DateOfServiceFrom  >= '" + cRDetailedCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.DOSDateFrom == null && cRDetailedCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  <= '" + cRDetailedCollectionReport.DOSDateTo.GetValueOrDefault().Date + "')");
                    }


                    if (cRDetailedCollectionReport.CheckDateFrom != null && cRDetailedCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (patientpayment.PaymentDate  >= '" + cRDetailedCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "' and patientpayment.PaymentDate  < '" + cRDetailedCollectionReport.CheckDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.CheckDateFrom != null && cRDetailedCollectionReport.CheckDateTo == null)
                    {
                        oString += (" and ( patientpayment.PaymentDate  >= '" + cRDetailedCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.CheckDateFrom == null && cRDetailedCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (patientpayment.PaymentDate  <= '" + cRDetailedCollectionReport.CheckDateTo.GetValueOrDefault().Date + "')");
                    }



                    if (cRDetailedCollectionReport.PostedDateFrom != null && cRDetailedCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (patientpayment.AddedDate  >= '" + cRDetailedCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "' and patientpayment.AddedDate  < '" + cRDetailedCollectionReport.PostedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.PostedDateFrom != null && cRDetailedCollectionReport.PostedDateTo == null)
                    {
                        oString += (" and ( patientpayment.AddedDate  >= '" + cRDetailedCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.PostedDateFrom == null && cRDetailedCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (patientpayment.AddedDate  <= '" + cRDetailedCollectionReport.PostedDateTo.GetValueOrDefault().Date + "')");
                    }

                    oString += ("Group By icd1t.ICDCode, icd2t.ICDCode,icd3t.ICDCode, icd4t.ICDCode, patientpayment.ID, pra.[Name],patientpayment.AddedBy,isnull(patientpayment.PaymentAmount,0)  ,isnull( c.TotalAmount, 0) , case 		when patientpayment.PaymentMethod = 'Cash' then 'Cash' 	when patientpayment.PaymentMethod = 'Check' then patientpayment.CheckNumber		when patientpayment.PaymentMethod = 'Credit Card' then patientpayment.CCTransactionID				else ''  end  , isnull( pPaymentCharge.AllocatedAmount ,0 ) ,pra.ID ,pra.ClientID , pat.LastName , pat.FirstName , pat.AccountNum , visit.ID , pat.LastName , pat.FirstName ,  convert(varchar, visit.DateOfServiceFrom, 101) , cpt.CPTCode , c.Units , convert(varchar, patientpayment.PaymentDate, 101)  ,convert(varchar, patientpayment.AddedDate, 101)  , pro.[Name] ,lo.[Name] ,RefPr.[Name] ,pat.ID , c.ID    ");


                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {

                            var chaPayment = new GRDetailedCollectionReport();
                            chaPayment.CheckAmount = Convert.ToDecimal(oReader["CheckAmount"].ToString());
                            chaPayment.PayerName = oReader["PayerName"].ToString();
                            chaPayment.PatientName = oReader["PatientLastName"].ToString() + ", " + oReader["PatientFirstName"].ToString();
                            chaPayment.PatientAccountNo = oReader["PatientAccountNo"].ToString();
                            chaPayment.VisitNo = Convert.ToInt64(oReader["VisitNo"].ToString());
                            chaPayment.DOS = oReader["DOS"].ToString();
                            chaPayment.CPT = oReader["CPT"].ToString();
                            chaPayment.CPTUnits = oReader["CPTUnits"].ToString();
                            chaPayment.ChargeAmount = Convert.ToDecimal(oReader["ChargeAmount"].ToString());
                            chaPayment.PaidAmount = Convert.ToDecimal(oReader["PaidAmount"].ToString());
                            chaPayment.AdjustmentAmount = Convert.ToDecimal(oReader["AdjustmentAmount"].ToString());
                            chaPayment.CheckDate = oReader["CheckDate"].ToString();
                            chaPayment.CheckNumber = oReader["CheckNumber"].ToString();
                            chaPayment.PaymentEntryDate = oReader["PaymentEntryDate"].ToString();
                            chaPayment.AppliedPayments = Convert.ToDecimal(oReader["AppliedPayments"].ToString());
                            chaPayment.Provider = oReader["Provider"].ToString();
                            chaPayment.Location = oReader["Location"].ToString();
                            chaPayment.ReferringProvider = oReader["ReferringProvider"].ToString();
                            chaPayment.PatientId = Convert.ToInt64(oReader["PatientId"].ToString());
                            chaPayment.ChargeId = Convert.ToInt64(oReader["ChargeId"].ToString());

                            chaPayment.dx1 = oReader["dx1"].ToString();
                            chaPayment.dx2 = oReader["dx2"].ToString();
                            chaPayment.dx3 = oReader["dx3"].ToString();
                            chaPayment.dx4 = oReader["dx4"].ToString();
                            data.Add(chaPayment);
                        }
                        myConnection.Close();
                    }
                }

                //List<GRDetailedCollectionReport> data2 = (from visit in _context.Visit
                //                                              //join patientpaymen in _context.PatientPayment on visit.ID equals patientpaymen.VisitID into Table1
                //                                              //from patientpayment in Table1.DefaultIfEmpty()
                //                                          join c in _context.Charge on visit.ID equals c.VisitID
                //                                          join pCharge in _context.PaymentCharge on c.ID equals pCharge.ChargeID
                //                                          join pVisit in _context.PaymentVisit on pCharge.PaymentVisitID equals pVisit.ID
                //                                          join paymentcheck in _context.PaymentCheck on pVisit.PaymentCheckID equals paymentcheck.ID
                //                                          join pat in _context.Patient on visit.PatientID equals pat.ID
                //                                          join cpt in _context.Cpt on c.CPTID equals cpt.ID
                //                                          join practice in _context.Practice on paymentcheck.PracticeID equals practice.ID
                //                                          join pro in _context.Provider on visit.ProviderID equals pro.ID

                //                                          //join pPaymentCharge in _context.PatientPaymentCharge on c.ID equals pPaymentCharge.ChargeID

                //                                          join lo in _context.Location on c.LocationID equals lo.ID into Table3
                //                                          from loc in Table3.DefaultIfEmpty()
                //                                          join RefPr in _context.RefProvider on c.RefProviderID equals RefPr.ID into Table2
                //                                          from RefPro in Table2.DefaultIfEmpty()
                //                                          //join mod1 in _context.Modifier on c.Modifier1ID equals mod1.ID into Table4
                //                                          //from modi1 in Table4.DefaultIfEmpty()
                //                                          //join mod2 in _context.Modifier on c.Modifier2ID equals mod2.ID into Table5
                //                                          //from modi2 in Table5.DefaultIfEmpty()
                //                                          where
                //                                                                                                    (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.DOSDateTo, cRDetailedCollectionReport.DOSDateFrom, visit.DateOfServiceTo, visit.DateOfServiceFrom)) &&
                //                                       (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.PostedDateTo, cRDetailedCollectionReport.PostedDateFrom, pCharge.PostedDate, pCharge.PostedDate))
                //                                      && (cRDetailedCollectionReport.CheckNo.IsNull() ? true : paymentcheck.CheckNumber.ToUpper().Equals(cRDetailedCollectionReport.CheckNo))
                //                                      && (ExtensionMethods.IsBetweenDOS(cRDetailedCollectionReport.CheckDateTo, cRDetailedCollectionReport.CheckDateFrom, paymentcheck.CheckDate, paymentcheck.CheckDate))
                //                                      && (cRDetailedCollectionReport.UserPosted.IsNull() ? true : pCharge.PostedBy.IsNull() ? false : pCharge.PostedBy.ToUpper().Equals(cRDetailedCollectionReport.UserPosted))
                //                                      && (practice.ID == UD.PracticeID)
                //                                      && (practice.ClientID == UD.ClientID) && paymentcheck.Status == "P"

                //                                          select new GRDetailedCollectionReport
                //                                          {
                //                                              CheckAmount = paymentcheck.CheckAmount,
                //                                              PracticeID = practice.ID,
                //                                              ClientID = practice.ClientID,
                //                                              PayerName = paymentcheck.PayerName,//v
                //                                              PatientAccountNo = pat.AccountNum,
                //                                              VisitNo = visit.ID,
                //                                              PatientName = pat.LastName + ", " + pat.FirstName,
                //                                              DOS = visit.DateOfServiceFrom.Format("MM/dd/yyyy"),
                //                                              CPT = cpt.CPTCode,
                //                                              CPTUnits = c.Units,
                //                                              //Modifier1 = modi1.Code,
                //                                              //Modifier2 = modi2.Code,
                //                                              ChargeAmount = pVisit.ProcessedAs.IsNull() || pVisit.ProcessedAs == "1" ? c.PrimaryBilledAmount :
                //                                                                pVisit.ProcessedAs == "2" ? c.SecondaryBilledAmount : null,

                //                                              //AllowedAmount = c.PrimaryAllowed.Val() + c.SecondaryAllowed.Val() + c.TertiaryAllowed.Val(), //
                //                                              AllowedAmount = pVisit.ProcessedAs.IsNull() || pVisit.ProcessedAs == "1" ? c.PrimaryAllowed :
                //                                                                pVisit.ProcessedAs == "2" ? c.SecondaryAllowed : null,


                //                                              //PaidAmount = c.PrimaryPaid.Val() + c.SecondaryPaid.Val() + c.TertiaryPaid.Val(),
                //                                              PaidAmount = pVisit.ProcessedAs.IsNull() || pVisit.ProcessedAs == "1" ? c.PrimaryPaid :
                //                                                                pVisit.ProcessedAs == "2" ? c.SecondaryPaid : null,


                //                                              //AdjustmentAmount = c.PrimaryWriteOff.Val() + c.SecondaryWriteOff.Val() + c.TertiaryWriteOff.Val(),
                //                                              AdjustmentAmount = pVisit.ProcessedAs.IsNull() || pVisit.ProcessedAs == "1" ? c.PrimaryWriteOff :
                //                                                                pVisit.ProcessedAs == "2" ? c.SecondaryWriteOff : null,

                //                                              CheckNumber = paymentcheck.CheckNumber, //v
                //                                              CheckDate = paymentcheck.CheckDate.Format("MM/dd/yyyy"),//v
                //                                              PaymentEntryDate = pCharge.PostedDate.Format("MM/dd/yyyy"),
                //                                              AppliedPayments = pCharge.PaidAmount,
                //                                              Provider = pro.Name,
                //                                              Location = loc.Name,
                //                                              ReferringProvider = RefPro.Name,
                //                                              PatientId = pat.ID,
                //                                              ChargeId = c.ID

                //                                          }).Distinct().ToList();
                List<GRDetailedCollectionReport> data2 = new List<GRDetailedCollectionReport>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    string oString = "select isnull(icd1t.ICDCode, '') as dx1, isnull(icd2t.ICDCode, '') as dx2, isnull(icd3t.ICDCode, '') as dx3, isnull(icd4t.ICDCode, '') as dx4, isnull(paymentcheck.CheckAmount,0)  as CheckAmount, ISNULL( pCharge.PaidAmount , 0) as PaidAmount,isnull( pCharge.PaidAmount ,0 )as AppliedPayments,case 	 when pVisit.ProcessedAs = 1 then isnull( c.PrimaryBilledAmount, 0)	when pVisit.ProcessedAs = 2 then isnull(c.SecondaryBilledAmount, 0)	else 0  end 		as ChargeAmount,case	when pVisit.ProcessedAs = 1 then isnull( c.PrimaryAllowed, 0)		when pVisit.ProcessedAs = 2 then isnull( c.SecondaryAllowed	, 0)	else 0  end 		as AllowedAmount, case	when pVisit.ProcessedAs = 1 then isnull( c.PrimaryWriteOff, 0)		when pVisit.ProcessedAs = 2 then isnull( c.SecondaryWriteOff , 0)		else 0  end 		as AdjustmentAmount, practice.ID as PracticeID, practice.ClientID as ClientID,paymentcheck.PayerName as PayerName, pat.AccountNum as PatientAccountNo, visit.ID as VisitNo,pat.LastName as PatientLastName, pat.FirstName as PatientFirstName,convert(varchar, visit.DateOfServiceFrom, 101) as DOS, cpt.CPTCode as CPT, c.Units as CPTUnits,mod1.Code as Modifier1, mod2.Code as Modifier2, paymentcheck.CheckNumber as CheckNumber,convert(varchar, paymentcheck.CheckDate, 101)  as CheckDate,convert(varchar, pCharge.PostedDate, 101)  as PaymentEntryDate,pro.[Name] as [Provider], lo.[Name] as [Location], RefPr.[Name] as ReferringProvider,pat.ID as PatientId, c.ID as ChargeId from PaymentCheck paymentcheck join Practice pra on paymentcheck.PracticeID = pra.ID join PaymentVisit pVisit on paymentcheck.ID = pVisit.PaymentCheckID  join PaymentCharge pCharge on pVisit.ID = pCharge.PaymentVisitID join Charge c on  pCharge.ChargeID = c.ID join Visit visit on  c.VisitID = visit.ID join Patient pat on visit.PatientID = pat.ID join Cpt cpt on c.CPTID = cpt.ID join Practice practice on paymentcheck.PracticeID = practice.ID join [Provider] pro on visit.ProviderID = pro.ID left join [Location] lo on c.LocationID = lo.ID left join RefProvider RefPr on c.RefProviderID = RefPr.ID left join Modifier mod1 on c.Modifier1ID = mod1.ID left join Modifier mod2 on c.Modifier2ID = mod2.ID left join ICD icd1t on Visit.ICD1ID = icd1t.ID left join ICD icd2t on Visit.ICD2ID = icd2t.ID left join ICD icd3t on Visit.ICD3ID = icd3t.ID  left join ICD icd4t on Visit.ICD4ID = icd4t.ID where pCharge.status = 'P' and pCharge.chargeid is not null     ";

                    oString += "  and c.practiceid = {0} ";
                    oString = string.Format(oString, PracticeId);

                    if (!cRDetailedCollectionReport.ProviderID.IsNull())
                        oString += string.Format(" and pro.ID = '{0}'", cRDetailedCollectionReport.ProviderID);
                    if (!cRDetailedCollectionReport.LocationID.IsNull())
                        oString += string.Format(" and lo.ID = '{0}'", cRDetailedCollectionReport.LocationID);
                    if (!cRDetailedCollectionReport.CheckNo.IsNull())
                        oString += string.Format(" and paymentcheck.CheckNumber = '{0}'", cRDetailedCollectionReport.CheckNo);
                    if (!cRDetailedCollectionReport.RefProviderID.IsNull())
                        oString += string.Format(" and visit.RefProviderID = '{0}'", cRDetailedCollectionReport.RefProviderID);
                    if (!cRDetailedCollectionReport.UserPosted.IsNull())
                        oString += string.Format(" and paymentcheck.PostedBy ='{0}'", cRDetailedCollectionReport.UserPosted);


                    if (cRDetailedCollectionReport.DOSDateFrom != null && cRDetailedCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  >= '" + cRDetailedCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "' and visit.DateOfServiceFrom  < '" + cRDetailedCollectionReport.DOSDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.DOSDateFrom != null && cRDetailedCollectionReport.DOSDateTo == null)
                    {
                        oString += (" and ( visit.DateOfServiceFrom  >= '" + cRDetailedCollectionReport.DOSDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.DOSDateFrom == null && cRDetailedCollectionReport.DOSDateTo != null)
                    {
                        oString += (" and (visit.DateOfServiceFrom  <= '" + cRDetailedCollectionReport.DOSDateTo.GetValueOrDefault().Date + "')");
                    }

                    if (cRDetailedCollectionReport.CheckDateFrom != null && cRDetailedCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (paymentcheck.CheckDate  >= '" + cRDetailedCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "' and paymentcheck.CheckDate  < '" + cRDetailedCollectionReport.CheckDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.CheckDateFrom != null && cRDetailedCollectionReport.CheckDateTo == null)
                    {
                        oString += (" and ( paymentcheck.CheckDate  >= '" + cRDetailedCollectionReport.CheckDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.CheckDateFrom == null && cRDetailedCollectionReport.CheckDateTo != null)
                    {
                        oString += (" and (paymentcheck.CheckDate  <= '" + cRDetailedCollectionReport.CheckDateTo.GetValueOrDefault().Date + "')");
                    }







                    if (cRDetailedCollectionReport.PostedDateFrom != null && cRDetailedCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (pCharge.PostedDate  >= '" + cRDetailedCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "' and pCharge.PostedDate  < '" + cRDetailedCollectionReport.PostedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (cRDetailedCollectionReport.PostedDateFrom != null && cRDetailedCollectionReport.PostedDateTo == null)
                    {
                        oString += (" and ( pCharge.PostedDate  >= '" + cRDetailedCollectionReport.PostedDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (cRDetailedCollectionReport.PostedDateFrom == null && cRDetailedCollectionReport.PostedDateTo != null)
                    {
                        oString += (" and (pCharge.PostedDate  <= '" + cRDetailedCollectionReport.PostedDateTo.GetValueOrDefault().Date + "')");
                    }




                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {

                            var chaPayment = new GRDetailedCollectionReport();
                            chaPayment.CheckAmount = Convert.ToDecimal(oReader["CheckAmount"].ToString());
                            chaPayment.PayerName = oReader["PayerName"].ToString();
                            chaPayment.PatientName = oReader["PatientLastName"].ToString() + ", " + oReader["PatientFirstName"].ToString();
                            chaPayment.PatientAccountNo = oReader["PatientAccountNo"].ToString();
                            chaPayment.VisitNo = Convert.ToInt64(oReader["VisitNo"].ToString());
                            chaPayment.DOS = oReader["DOS"].ToString();
                            chaPayment.CPT = oReader["CPT"].ToString();
                            chaPayment.CPTUnits = oReader["CPTUnits"].ToString();
                            chaPayment.Modifier1 = oReader["Modifier1"].ToString();
                            chaPayment.Modifier2 = oReader["Modifier2"].ToString();
                            chaPayment.ChargeAmount = Convert.ToDecimal(oReader["ChargeAmount"].ToString());
                            chaPayment.PaidAmount = Convert.ToDecimal(oReader["PaidAmount"].ToString());
                            chaPayment.AdjustmentAmount = Convert.ToDecimal(oReader["AdjustmentAmount"].ToString());
                            chaPayment.CheckDate = oReader["CheckDate"].ToString();
                            chaPayment.CheckNumber = oReader["CheckNumber"].ToString();
                            chaPayment.PaymentEntryDate = oReader["PaymentEntryDate"].ToString();
                            chaPayment.AppliedPayments = Convert.ToDecimal(oReader["AppliedPayments"].ToString());
                            chaPayment.Provider = oReader["Provider"].ToString();
                            chaPayment.Location = oReader["Location"].ToString();
                            chaPayment.ReferringProvider = oReader["ReferringProvider"].ToString();
                            chaPayment.PatientId = Convert.ToInt64(oReader["PatientId"].ToString());
                            chaPayment.ChargeId = Convert.ToInt64(oReader["ChargeId"].ToString());

                            chaPayment.dx1 = oReader["dx1"].ToString();
                            chaPayment.dx2 = oReader["dx2"].ToString();
                            chaPayment.dx3 = oReader["dx3"].ToString();
                            chaPayment.dx4 = oReader["dx4"].ToString();
                            data2.Add(chaPayment);
                        }
                        myConnection.Close();
                    }
                }



                List<GRDetailedCollectionReport> total = data2.Union(data).ToList();
                return total;
            }

        }


        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CRDetailedCollectionReport cRDetailedCollectionReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRDetailedCollectionReport> data = FindDetailedCollections(cRDetailedCollectionReport, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, cRDetailedCollectionReport, "Detailed Collection Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CRDetailedCollectionReport cRDetailedCollectionReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRDetailedCollectionReport> data = FindDetailedCollections(cRDetailedCollectionReport, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }

        private bool CheckNullLong(long? Value)
        {
            Debug.WriteLine(Value.IsNull());
            return Value.IsNull();
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
        



