using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediFusionPM.Models;
using static MediFusionPM.ReportViewModels.RVMPatientVisit;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using MediFusionPM.Controllers;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MediFusionPM.ReportControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RPatientVisitController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public RPatientVisitController(ClientDbContext context, MainContext contextMain)
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


        [Route("GetPatientVisitReport")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRPatientVisit>>> GetPatientVisits()
        {
            try
            {

                // object o = null;
                //string oo = o.ToString();
                return await (from chargetable in _context.Charge
                              join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
                              join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                              join refprovidertable in _context.RefProvider on chargetable.RefProviderID equals refprovidertable.ID into RefProviderTable
                              from reftable in RefProviderTable.DefaultIfEmpty()
                              join claimtable in _context.Visit on chargetable.VisitID equals claimtable.ID
                              join postable in _context.POS on chargetable.POSID equals postable.ID
                              join cpttable in _context.Cpt on chargetable.CPTID equals cpttable.ID

                              join modtable in _context.Modifier on chargetable.Modifier1ID equals modtable.ID into Mod1Table
                              from m1t in Mod1Table.DefaultIfEmpty()
                              join modtable in _context.Modifier on chargetable.Modifier2ID equals modtable.ID into Mod2Table
                              from m2t in Mod2Table.DefaultIfEmpty()
                                  //join modtable in _context.Modifier on chargetable.Modifier3ID equals modtable.ID into Mod3Table
                                  //from m3t in Mod3Table.DefaultIfEmpty()
                                  //join modtable in _context.Modifier on chargetable.Modifier4ID equals modtable.ID into Mod4Table
                                  //from m4t in Mod4Table.DefaultIfEmpty()

                              join icdtable in _context.ICD on claimtable.ICD1ID equals icdtable.ID into ICD1Table
                              from icd1t in ICD1Table.DefaultIfEmpty()
                              join icdtable in _context.ICD on claimtable.ICD2ID equals icdtable.ID into ICD2Table
                              from icd2t in ICD2Table.DefaultIfEmpty()
                              join icdtable in _context.ICD on claimtable.ICD3ID equals icdtable.ID into ICD3Table
                              from icd3t in ICD3Table.DefaultIfEmpty()
                              join icdtable in _context.ICD on claimtable.ICD4ID equals icdtable.ID into ICD4Table
                              from icd4t in ICD4Table.DefaultIfEmpty()
                                  //join icdtable in _context.ICD on claimtable.ICD5ID equals icdtable.ID into ICD5Table
                                  //from icd5t in ICD5Table.DefaultIfEmpty()
                                  //join icdtable in _context.ICD on claimtable.ICD6ID equals icdtable.ID into ICD6Table
                                  //from icd6t in ICD6Table.DefaultIfEmpty()
                                  //join icdtable in _context.ICD on claimtable.ICD7ID equals icdtable.ID into ICD7Table
                                  //from icd7t in ICD7Table.DefaultIfEmpty()
                                  //join icdtable in _context.ICD on claimtable.ICD8ID equals icdtable.ID into ICD8Table
                                  //from icd8t in ICD8Table.DefaultIfEmpty()
                                  //join icdtable in _context.ICD on claimtable.ICD9ID equals icdtable.ID into ICD9Table
                                  //from icd9t in ICD9Table.DefaultIfEmpty()
                                  //join icdtable in _context.ICD on claimtable.ICD10ID equals icdtable.ID into ICD10Table
                                  //from icd10t in ICD10Table.DefaultIfEmpty()

                              join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID into PrimaryPatientPlanTable
                              from primarypatientplantable in PrimaryPatientPlanTable.DefaultIfEmpty()
                              join patientplantable in _context.PatientPlan on chargetable.SecondaryPatientPlanID equals patientplantable.ID into SecondaryPatientPlanTable
                              from secondarypatientplantable in SecondaryPatientPlanTable.DefaultIfEmpty()
                              join patientplantable in _context.PatientPlan on chargetable.TertiaryPatientPlanID equals patientplantable.ID into OtherPatientPlanTable
                              from otherpatientplantable in OtherPatientPlanTable.DefaultIfEmpty()


                              join insuranceplantable in _context.InsurancePlan on primarypatientplantable.InsurancePlanID equals insuranceplantable.ID into PrimaryInsuranceTable
                              from primaryinsurancetable in PrimaryInsuranceTable.DefaultIfEmpty()
                              join insuranceplantable in _context.InsurancePlan on secondarypatientplantable.InsurancePlanID equals insuranceplantable.ID into SecondaryInsuranceTable
                              from secondaryinsurancetable in SecondaryInsuranceTable.DefaultIfEmpty()
                              join insuranceplantable in _context.InsurancePlan on otherpatientplantable.InsurancePlanID equals insuranceplantable.ID into OtherInsuranceTable
                              from otherinsurancetable in OtherInsuranceTable.DefaultIfEmpty()


                              select new GRPatientVisit
                              {
                                  PatientID = chargetable.PatientID,
                                  VisitNo = chargetable.VisitID,
                                  SubmissionDate = chargetable.SubmittetdDate.Format(@"MM\/dd\/yyyy"),
                                  PatientFirstName = patienttable.FirstName,
                                  PatientLastName = patienttable.LastName,
                                  MiddleInitial = patienttable.MiddleInitial,
                                  PatientGender = patienttable.Gender,
                                  SSN = patienttable.SSN.IsNull() ? "" : patienttable.SSN,
                                  Cpt = cpttable.CPTCode,
                                  CptDescription = cpttable.Description,
                                  DateOfBirth = patienttable.DOB.HasValue ? patienttable.DOB.Value.ToString(@"MM\/dd\/yyyy") : "",
                                  DateOfService = chargetable.DateOfServiceFrom.ToString(@"MM\/dd\/yyyy"),
                                  dx1 = icd1t.ICDCode.IsNull() ? "" : icd1t.ICDCode,
                                  dx2 = icd2t.ICDCode.IsNull() ? "" : icd2t.ICDCode,
                                  dx3 = icd3t.ICDCode.IsNull() ? "" : icd3t.ICDCode,
                                  dx4 = icd4t.ICDCode.IsNull() ? "" : icd4t.ICDCode,
                                  //dx5 = icd5t.ICDCode.IsNull() ? "" : icd5t.ICDCode,
                                  //dx6 = icd6t.ICDCode.IsNull() ? "" : icd6t.ICDCode,
                                  //dx7 = icd7t.ICDCode.IsNull() ? "" : icd7t.ICDCode,
                                  //dx8 = icd8t.ICDCode.IsNull() ? "" : icd8t.ICDCode,
                                  //dx9 = icd9t.ICDCode.IsNull() ? "" : icd9t.ICDCode,
                                  //dx10 = icd10t.ICDCode.IsNull() ? "" : icd10t.ICDCode,
                                  MOD1 = m1t.Code.IsNull() ? "" : m1t.Code,
                                  MOD2 = m2t.Code.IsNull() ? "" : m2t.Code,
                                  //MOD3 = m3t.Code.IsNull() ? "" : m3t.Code,
                                  //MOD4 = m4t.Code.IsNull() ? "" : m4t.Code,
                                  POS = postable.PosCode,
                                  ProviderName = providertable.Name,
                                  FacilityName = postable.Name,
                                  IndividualNPI = providertable.NPI,
                                  PrimaryInsurance = primaryinsurancetable.PlanName,
                                  PrimarySubscriberId = primarypatientplantable.SubscriberId,
                                  SecondaryInsurance = secondaryinsurancetable.PlanName.IsNull() ? "" : secondaryinsurancetable.PlanName,
                                  SecondarySubscriberId = secondarypatientplantable.SubscriberId.IsNull() ? "" : secondarypatientplantable.SubscriberId,
                                  OtherInsurance = otherinsurancetable.PlanName.IsNull() ? "" : otherinsurancetable.PlanName,
                                  OtherSubscriberId = otherpatientplantable.SubscriberId.IsNull() ? "" : otherpatientplantable.SubscriberId,
                                  ReferringPhysicianName = reftable.Name.IsNull() ? "" : reftable.Name,
                                  AdjustmentAmount = chargetable.PrimaryWriteOff.Val() + chargetable.SecondaryWriteOff.Val() + chargetable.TertiaryWriteOff.Val(),
                                  AllowedAmount = chargetable.PrimaryAllowed.Val() + chargetable.SecondaryAllowed.Val() + chargetable.TertiaryAllowed.Val(),
                                  Charges = chargetable.TotalAmount,
                                  PaidAmount = chargetable.PrimaryPaid.Val() + chargetable.SecondaryPaid.Val() + chargetable.TertiaryPaid.Val(),
                                  PatientBalance = chargetable.PrimaryPatientBal.Val() + chargetable.SecondaryPatientBal.Val() + chargetable.TertiaryPatientBal.Val(),
                                  PlanBalance = chargetable.PrimaryBal.Val() + chargetable.SecondaryPatientBal.Val() + chargetable.TertiaryPatientBal.Val(),
                                  Balance = chargetable.PrimaryPatientBal.Val() + chargetable.SecondaryPatientBal.Val() + chargetable.TertiaryPatientBal.Val(),
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

        //_____________________________________________________________________________
        [Route("FindPatientVisitReports")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRPatientVisit>>> FindPatientVisitReports(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            // if (UD == null || UD.Rights == null || UD.Rights.SchedulerCreate == false)
            // return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return FindPatientVisitReports(CRPatientVisit, UD);
        }
        private List<GRPatientVisit> FindPatientVisitReports(CRPatientVisit CRPatientVisit, UserInfoData UD)
        {

            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Client = (from w in _contextMain.MainPractice
                          where w.ID == PracticeId
                          select w
                        ).FirstOrDefault();
            string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
            string newConnection = GetConnectionStringManager(contextName);
            List<GRPatientVisit> data = new List<GRPatientVisit>();
            using (SqlConnection myConnection = new SqlConnection(newConnection))
            {

                string oString = "SELECT 	chargetable.PrimaryStatus,chargetable.SecondaryStatus, chargetable.PatientID as PatientID,chargetable.VisitID as  ClaimID, convert(varchar, chargetable.SubmittetdDate, 101) as  SubmissionDate, patienttable.FirstName  as PatientFirstName,patienttable.LastName  as PatientLastName, patienttable.MiddleInitial  as MiddleInitial, convert(varchar, patienttable.DOB, 101)  as DateOfBirth, patienttable.Gender  as PatientGender, patienttable.SSN  as SSN, insurangeplantable.PlanName  as PrimaryInsurance, patientplantable.SubscriberId  as PrimaryPolicyNumber,  secinsuranceplantable.PlanName as  SecondaryInsurance,  secpatientplantable.SubscriberId as  SecondaryPolicyNumber,  otherinsurancetable.PlanName  as OtherInsurance,otherpatientplantable.SubscriberId  as OtherPolicyNumber, convert(varchar, chargetable.DateOfServiceFrom, 101)  as DateOfService, providertable.[Name]  as ProviderName, providertable.NPI as  IndividualNPI,  refprovidertable.[Name] as  ReferringPhysicianName, postable.PosCode  as POS, postable.[Name]  as FacilityName, Cpt.CPTCode  as Cpt,Cpt.[Description]  as CptDescription, cpt.ShortDescription as ShortCptDescription, m1t.Code  as MOD1,m2t.Code  as MOD2, icd1t.ICDCode  as dx1,icd2t.ICDCode  as dx2,icd3t.ICDCode  as dx3,icd4t.ICDCode  as dx4, SUM (ISNULL(chargetable.PrimaryAllowed,0) + ISNULL(chargetable.SecondaryAllowed,0) + ISNULL(chargetable.TertiaryAllowed,0)) as AllowedAmount,chargetable.TotalAmount as Charges,  SUM (ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) as PaidAmount, SUM (ISNULL(chargetable.PrimaryWriteOff,0) + ISNULL( chargetable.SecondaryWriteOff, 0) + ISNULL(chargetable.TertiaryWriteOff,0) ) as AdjustmentAmount,  SUM (ISNULL(chargetable.PrimaryBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as PlanBalance,  SUM (ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as PatientBalance,  SUM (ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as Balance,   convert(varchar, chargetable.AddedDate, 101) as EntryDate,   convert(varchar, chargetable.PrimaryPaymentDate, 101) as PrimaryCheckDate, convert(varchar, chargetable.SecondaryPaymentDate, 101) as SecondaryCheckDate    fROM Charge chargetable  join  Patient patienttable on chargetable.PatientID = patienttable.ID  join Visit v on chargetable.VisitID = v.ID  join Practice practicetable on chargetable.PracticeID = practicetable.ID  join [Provider] providertable on chargetable.ProviderID =  providertable.ID  join [Location] locationtable on chargetable.LocationID = locationtable.ID  join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID  join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID  join Cpt cpt on chargetable.CPTID = cpt.ID  left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID  left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID  left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID  left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID  left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID   left join PatientPlan otherpatientplantable on chargetable.TertiaryPatientPlanID =otherpatientplantable.ID left join InsurancePlan otherinsurancetable on otherpatientplantable.InsurancePlanID =otherinsurancetable.ID  left join POS postable on chargetable.POSID = postable.ID left join Modifier m1t on chargetable.Modifier1ID = m1t.ID left join Modifier m2t on chargetable.Modifier2ID = m2t.ID left join ICD icd1t on v.ICD1ID = icd1t.ID left join ICD icd2t on v.ICD2ID = icd2t.ID left join ICD icd3t on v.ICD3ID = icd3t.ID left join ICD icd4t on v.ICD4ID = icd4t.ID  where ";

                oString += "  chargetable.practiceid = {0} ";
                oString = string.Format(oString, PracticeId);



                if (!CRPatientVisit.PatientName.IsNull())
                    oString += string.Format(" and (patienttable.LastName + ', ' + patienttable.FirstName) like '%{0}%'", CRPatientVisit.PatientName);

                if (!CRPatientVisit.ProviderID.IsNull())
                    oString += string.Format(" and chargetable.ProviderID ='{0}'", CRPatientVisit.ProviderID);
                if (!CRPatientVisit.CPTCode.IsNull())
                    oString += string.Format(" and cpt.CPTCode ='{0}'", CRPatientVisit.CPTCode);
                if (!CRPatientVisit.PrescribingMD.IsNull())
                    oString += string.Format(" and v.PrescribingMD ='{0}'", CRPatientVisit.PrescribingMD);

                if (!CRPatientVisit.PaymentCriteria.IsNull())
                {
                    if (CRPatientVisit.PaymentCriteria == "Paid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "PartialPaid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "UnPaid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) = 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "PatientBal")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) > 0 ");
                    }
                }

                if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo != null)
                {
                    oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo == null)
                {
                    oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.DateOfServiceFrom == null && CRPatientVisit.DateOfServiceTo != null)
                {
                    oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date + "')");
                }


                if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo != null)
                {
                    oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo == null)
                {
                    oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.EntryDateFrom == null && CRPatientVisit.EntryDateTo != null)
                {
                    oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date + "')");
                }


                if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo != null)
                {
                    oString += (" and (chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo == null)
                {
                    oString += (" and ( chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.SubmittedDateFrom == null && CRPatientVisit.SubmittedDateTo != null)
                {
                    oString += (" and (chargetable.SubmittetdDate  <= '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date + "')");
                }
                oString += " GROUP BY chargetable.PrimaryStatus,chargetable.SecondaryStatus, chargetable.PatientID , chargetable.VisitID , chargetable.SubmittetdDate , patienttable.FirstName , patienttable.LastName , patienttable.MiddleInitial , patienttable.DOB , patienttable.Gender , patienttable.SSN , insurangeplantable.PlanName ,  patientplantable.SubscriberId , secinsuranceplantable.PlanName , secpatientplantable.SubscriberId , otherinsurancetable.PlanName ,otherpatientplantable.SubscriberId ,  chargetable.DateOfServiceFrom ,  providertable.[Name] , providertable.NPI ,   refprovidertable.[Name] , postable.[Name] , Cpt.CPTCode,  postable.PosCode ,Cpt.[Description] , m1t.Code , m2t.Code , icd1t.ICDCode , icd2t.ICDCode ,icd3t.ICDCode , icd4t.ICDCode ,  chargetable.TotalAmount, chargetable.AddedDate, chargetable.PrimaryPaymentDate, chargetable.SecondaryPaymentDate, cpt.ShortDescription ";

                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        var pvReport = new GRPatientVisit();
                        pvReport.PatientID = Convert.ToInt64(oReader["PatientID"].ToString());
                        pvReport.VisitNo = Convert.ToInt64(oReader["ClaimID"].ToString());
                        pvReport.SubmissionDate = oReader["SubmissionDate"].ToString();
                        pvReport.PatientFirstName = oReader["PatientFirstName"].ToString();
                        pvReport.PatientLastName = oReader["PatientLastName"].ToString();
                        pvReport.MiddleInitial = oReader["MiddleInitial"].ToString();
                        pvReport.DateOfBirth = oReader["DateOfBirth"].ToString();
                        pvReport.PatientGender = oReader["PatientGender"].ToString();
                        pvReport.SSN = oReader["SSN"].ToString();
                        pvReport.PrimaryInsurance = oReader["PrimaryInsurance"].ToString();
                        pvReport.PrimarySubscriberId = oReader["PrimaryPolicyNumber"].ToString();
                        pvReport.SecondaryInsurance = oReader["SecondaryInsurance"].ToString();
                        pvReport.SecondarySubscriberId = oReader["SecondaryPolicyNumber"].ToString();
                        pvReport.OtherInsurance = oReader["OtherInsurance"].ToString();
                        pvReport.OtherSubscriberId = oReader["OtherPolicyNumber"].ToString();
                        pvReport.DateOfService = oReader["DateOfService"].ToString();
                        pvReport.ProviderName = oReader["ProviderName"].ToString();
                        pvReport.IndividualNPI = oReader["IndividualNPI"].ToString();
                        pvReport.ReferringPhysicianName = oReader["ReferringPhysicianName"].ToString();
                        pvReport.FacilityName = oReader["FacilityName"].ToString();
                        pvReport.Cpt = oReader["Cpt"].ToString();
                        pvReport.POS = oReader["POS"].ToString();
                        pvReport.CptDescription = oReader["CptDescription"].ToString();
                        pvReport.ShortCptDescription = oReader["ShortCptDescription"].ToString();
                        pvReport.MOD1 = oReader["MOD1"].ToString();
                        pvReport.MOD2 = oReader["MOD2"].ToString();
                        pvReport.dx1 = oReader["dx1"].ToString();
                        pvReport.dx2 = oReader["dx2"].ToString();
                        pvReport.dx3 = oReader["dx3"].ToString();
                        pvReport.dx4 = oReader["dx4"].ToString();
                        pvReport.Charges = Convert.ToDecimal(oReader["Charges"].ToString());
                        pvReport.AllowedAmount = Convert.ToDecimal(oReader["AllowedAmount"].ToString());
                        pvReport.PaidAmount = Convert.ToDecimal(oReader["PaidAmount"].ToString());
                        pvReport.AdjustmentAmount = Convert.ToDecimal(oReader["AdjustmentAmount"].ToString());
                        pvReport.PlanBalance = Convert.ToDecimal(oReader["PlanBalance"].ToString());
                        pvReport.PatientBalance = Convert.ToDecimal(oReader["PatientBalance"].ToString());
                        pvReport.Balance = Convert.ToDecimal(oReader["Balance"].ToString());
                        pvReport.EntryDate = oReader["EntryDate"].ToString();
                        pvReport.PrimaryCheckDate = oReader["PrimaryCheckDate"].ToString();
                        pvReport.SecondaryCheckDate = oReader["SecondaryCheckDate"].ToString();
                        pvReport.PrimaryStatus = oReader["PrimaryStatus"].ToString();
                        pvReport.SecondaryStatus = oReader["SecondaryStatus"].ToString();
                        data.Add(pvReport);
                    }
                    myConnection.Close();
                }
            }

            return data;
        }


        [HttpPost]
        [Route("FindPatientVisitReportsExport")]
        public async Task<IActionResult> FindPatientVisitReportsExport(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientVisit> data = FindPatientVisitReports(CRPatientVisit, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CRPatientVisit, "Patient Visit Report");
        }


        [HttpPost]
        [Route("FindPatientVisitReportsExportPdf")]
        public async Task<IActionResult> FindPatientVisitReportsExportPdf(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientVisit> data = FindPatientVisitReports(CRPatientVisit, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }




        //_______________________________________________________________________________
        [Route("FindSimplePatientVisitReports")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRPatientVisitSimple>>> FindSimplePatientVisitReports(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            return FindSimplePatientVisitReports(CRPatientVisit, UD);
        }
        private List<GRPatientVisitSimple> FindSimplePatientVisitReports(CRPatientVisit CRPatientVisit, UserInfoData UD)
        {
            //List<GRPatientVisitSimple> data = (from chargetable in _context.Charge
            //                                   join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
            //                                   join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
            //                                   join postable in _context.POS on chargetable.POSID equals postable.ID
            //                                   join cpttable in _context.Cpt on chargetable.CPTID equals cpttable.ID
            //                                   join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
            //                                   join insuranceplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insuranceplantable.ID
            //                                   where
            //                                     (CheckNullLong(CRPatientVisit.ProviderID) ? true : chargetable.ProviderID.Equals(CRPatientVisit.ProviderID)) &&
            //                                     (ExtensionMethods.IsBetweenDOS(CRPatientVisit.DateOfServiceTo, CRPatientVisit.DateOfServiceFrom, chargetable.DateOfServiceTo, chargetable.DateOfServiceFrom)) &&
            //                                     (ExtensionMethods.IsBetweenDOS(CRPatientVisit.EntryDateTo, CRPatientVisit.EntryDateFrom, chargetable.AddedDate, chargetable.AddedDate)) &&
            //                                     (ExtensionMethods.IsBetweenDOS(CRPatientVisit.SubmittedDateTo, CRPatientVisit.SubmittedDateFrom, chargetable.SubmittetdDate, chargetable.SubmittetdDate)) &&
            //                                     (CRPatientVisit.PatientName.IsNull() ? true : (patienttable.FirstName.Trim() + " " + patienttable.LastName.Trim()).Contains(CRPatientVisit.PatientName)) &&
            //                                (CRPatientVisit.CPTCode.IsNull() ? true : cpttable.CPTCode.Equals(CRPatientVisit.CPTCode))
            //                                   select new GRPatientVisitSimple
            //                                   {
            //                                       PatientName = patienttable.LastName + ", " + patienttable.FirstName,
            //                                       Cpt = cpttable.CPTCode,
            //                                       DateOfService = chargetable.DateOfServiceFrom.ToString(@"MM\/dd\/yyyy"),
            //                                       POS = postable.PosCode,
            //                                       InsuranceName = insuranceplantable.PlanName,
            //                                       Charges = chargetable.TotalAmount.Val(),
            //                                       SubmissionDate = chargetable.SubmittetdDate.Format("MM/dd/yyyy"),

            //                                   }).ToList();
            //return data;



            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Client = (from w in _contextMain.MainPractice
                          where w.ID == PracticeId
                          select w
                        ).FirstOrDefault();
            string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
            string newConnection = GetConnectionStringManager(contextName);
            List<GRPatientVisitSimple> data = new List<GRPatientVisitSimple>();
            using (SqlConnection myConnection = new SqlConnection(newConnection))
            {
                string oString = "SELECT 	convert(varchar, chargetable.SubmittetdDate, 101) as  SubmissionDate, convert(varchar, chargetable.DateOfServiceFrom, 101)  as DateOfService, Cpt.CPTCode  as Cpt, postable.PosCode  as POS ,SUM (ISNULL(chargetable.PrimaryAllowed,0) + ISNULL(chargetable.SecondaryAllowed,0) + ISNULL(chargetable.TertiaryAllowed,0)) as AllowedAmount,chargetable.TotalAmount as Charges , insurangeplantable.PlanName as InsuranceName, (patienttable.LastName + ', ' + patienttable.FirstName) as PatientName   FROM Charge chargetable  join  Patient patienttable on chargetable.PatientID = patienttable.ID  join Visit v on chargetable.VisitID = v.ID  join Practice practicetable on chargetable.PracticeID = practicetable.ID  join [Provider] providertable on chargetable.ProviderID =  providertable.ID  join [Location] locationtable on chargetable.LocationID = locationtable.ID  join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID  join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID  join Cpt cpt on chargetable.CPTID = cpt.ID  left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID  left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID  left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID  left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID  left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID   left join PatientPlan otherpatientplantable on chargetable.TertiaryPatientPlanID =otherpatientplantable.ID left join InsurancePlan otherinsurancetable on otherpatientplantable.InsurancePlanID =otherinsurancetable.ID  left join POS postable on chargetable.POSID = postable.ID left join Modifier m1t on chargetable.Modifier1ID = m1t.ID left join Modifier m2t on chargetable.Modifier2ID = m2t.ID left join ICD icd1t on v.ICD1ID = icd1t.ID left join ICD icd2t on v.ICD2ID = icd2t.ID left join ICD icd3t on v.ICD3ID = icd3t.ID left join ICD icd4t on v.ICD4ID = icd4t.ID  where ";

                oString += "  chargetable.practiceid = {0} ";
                oString = string.Format(oString, PracticeId);



                if (!CRPatientVisit.PatientName.IsNull())
                    oString += string.Format(" and (patienttable.LastName + ', ' + patienttable.FirstName) like '%{0}%'", CRPatientVisit.PatientName);

                if (!CRPatientVisit.ProviderID.IsNull())
                    oString += string.Format(" and chargetable.ProviderID ='{0}'", CRPatientVisit.ProviderID);
                if (!CRPatientVisit.CPTCode.IsNull())
                    oString += string.Format(" and cpt.CPTCode ='{0}'", CRPatientVisit.CPTCode);
                if (!CRPatientVisit.PrescribingMD.IsNull())
                    oString += string.Format(" and v.PrescribingMD ='{0}'", CRPatientVisit.PrescribingMD);

                if (!CRPatientVisit.PaymentCriteria.IsNull())
                {
                    if (CRPatientVisit.PaymentCriteria == "Paid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "PartialPaid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "UnPaid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) = 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "PatientBal")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) > 0 ");
                    }
                }

                if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo != null)
                {
                    oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo == null)
                {
                    oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.DateOfServiceFrom == null && CRPatientVisit.DateOfServiceTo != null)
                {
                    oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date + "')");
                }


                if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo != null)
                {
                    oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo == null)
                {
                    oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.EntryDateFrom == null && CRPatientVisit.EntryDateTo != null)
                {
                    oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date + "')");
                }


                if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo != null)
                {
                    oString += (" and (chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo == null)
                {
                    oString += (" and ( chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.SubmittedDateFrom == null && CRPatientVisit.SubmittedDateTo != null)
                {
                    oString += (" and (chargetable.SubmittetdDate  <= '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date + "')");
                }

                oString += " GROUP BY chargetable.SubmittetdDate , chargetable.DateOfServiceFrom , Cpt.CPTCode , postable.PosCode , insurangeplantable.PlanName , (patienttable.LastName + ', ' + patienttable.FirstName), chargetable.TotalAmount";

                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        var pvReport = new GRPatientVisitSimple();
                        pvReport.SubmissionDate = oReader["SubmissionDate"].ToString();
                        pvReport.DateOfService = oReader["DateOfService"].ToString();
                        pvReport.Cpt = oReader["Cpt"].ToString();
                        pvReport.POS = oReader["POS"].ToString();
                        pvReport.Charges = Convert.ToDecimal(oReader["Charges"].ToString());
                        pvReport.InsuranceName = oReader["InsuranceName"].ToString();
                        pvReport.PatientName = oReader["PatientName"].ToString();
                        data.Add(pvReport);
                    }
                    myConnection.Close();
                }
            }

            return data;

        }
        [HttpPost]
        [Route("ExportSimple")]
        public async Task<IActionResult> ExportSimple(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientVisitSimple> data = FindSimplePatientVisitReports(CRPatientVisit, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CRPatientVisit, "Total Count Billed By CPT");
        }
        [HttpPost]
        [Route("ExportSimplePdf")]
        public async Task<IActionResult> ExportSimplePdf(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientVisitSimple> data = FindSimplePatientVisitReports(CRPatientVisit, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }
        //_________________________________________________________________________________

        [Route("FindPatientVisitTotalCountPaid")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRPatientVisitCountPaid>>> FindPatientVisitTotalCountPaid(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindPatientVisitTotalCountPaid(CRPatientVisit, UD);
        }
        private List<GRPatientVisitCountPaid> FindPatientVisitTotalCountPaid(CRPatientVisit CRPatientVisit, UserInfoData UD)
        {
            //List<GRPatientVisitCountPaid> data = (from chargetable in _context.Charge
            //                                      join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
            //                                      join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
            //                                      join Paymentvisittable in _context.PaymentVisit on chargetable.VisitID equals Paymentvisittable.VisitID
            //                                      join paymentchecktable in _context.PaymentCheck on Paymentvisittable.PaymentCheckID equals paymentchecktable.ID
            //                                      join postable in _context.POS on chargetable.POSID equals postable.ID
            //                                      join cpttable in _context.Cpt on chargetable.CPTID equals cpttable.ID
            //                                      join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
            //                                      join insuranceplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insuranceplantable.ID
            //                                      where
            //                                      ((chargetable.PrimaryPaid.Val() + chargetable.SecondaryPaid.Val() + chargetable.TertiaryPaid.Val()) > 0) &&
            //                                        (CheckNullLong(CRPatientVisit.ProviderID) ? true : chargetable.ProviderID.Equals(CRPatientVisit.ProviderID)) &&
            //                                        (ExtensionMethods.IsBetweenDOS(CRPatientVisit.DateOfServiceTo, CRPatientVisit.DateOfServiceFrom, chargetable.DateOfServiceTo, chargetable.DateOfServiceFrom)) &&
            //                                        (ExtensionMethods.IsBetweenDOS(CRPatientVisit.EntryDateTo, CRPatientVisit.EntryDateFrom, chargetable.AddedDate, chargetable.AddedDate)) &&
            //                                        (ExtensionMethods.IsBetweenDOS(CRPatientVisit.SubmittedDateTo, CRPatientVisit.SubmittedDateFrom, chargetable.SubmittetdDate, chargetable.SubmittetdDate)) &&
            //                                        (CRPatientVisit.PatientName.IsNull() ? true : (patienttable.FirstName.Trim() + " " + patienttable.LastName.Trim()).Contains(CRPatientVisit.PatientName)) &&
            //                                (CRPatientVisit.CPTCode.IsNull() ? true : cpttable.CPTCode.Equals(CRPatientVisit.CPTCode))
            //                                      select new GRPatientVisitCountPaid
            //                                      {
            //                                          PatientName = patienttable.LastName + ", " + patienttable.FirstName,
            //                                          Cpt = cpttable.CPTCode,
            //                                          DateOfService = chargetable.DateOfServiceFrom.ToString(@"MM\/dd\/yyyy"),
            //                                          POS = postable.PosCode,
            //                                          PayerName = insuranceplantable.Description,
            //                                          SubmissionDate = chargetable.SubmittetdDate.Format("MM/dd/yyyy"),
            //                                          PaymentDate = chargetable.PrimaryPaymentDate.Format("MM/dd/yyyy"),
            //                                          SecondaryPaymentDate = chargetable.SecondaryPaymentDate.Format("MM/dd/yyyy"),
            //                                          BilledAmount = chargetable.PrimaryBilledAmount.Val() + chargetable.SecondaryBilledAmount.Val() + chargetable.TertiaryBilledAmount.Val(),
            //                                          PaidAmount = chargetable.PrimaryPaid.Val() + chargetable.SecondaryPaid.Val() + chargetable.TertiaryPaid.Val(),
            //                                          PlanBalance = chargetable.PrimaryBal.Val() + chargetable.SecondaryBal.Val() + chargetable.TertiaryBal.Val(),
            //                                          PatientBalance = chargetable.PrimaryPatientBal.Val() + chargetable.SecondaryPatientBal.Val() + chargetable.TertiaryPatientBal.Val()



            //                                      }).ToList();
            //return data;


            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Client = (from w in _contextMain.MainPractice
                          where w.ID == PracticeId
                          select w
                        ).FirstOrDefault();
            string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
            string newConnection = GetConnectionStringManager(contextName);
            List<GRPatientVisitCountPaid> data = new List<GRPatientVisitCountPaid>();
            using (SqlConnection myConnection = new SqlConnection(newConnection))
            {
                string oString = "SELECT 	convert(varchar, chargetable.SubmittetdDate, 101) as  SubmissionDate ,convert(varchar, chargetable.DateOfServiceFrom, 101)  as DateOfService, Cpt.CPTCode  as Cpt, postable.PosCode  as POS ,SUM (ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) as PaidAmount , SUM (ISNULL(chargetable.PrimaryBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as PlanBalance, SUM (ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as PatientBalance, (patienttable.LastName + ', ' + patienttable.FirstName) as PatientName, insurangeplantable.[Description] as PayerName,  convert(varchar, chargetable.PrimaryPaymentDate, 101) as PaymentDate, convert(varchar, chargetable.SecondaryPaymentDate, 101) as SecondaryPaymentDate, SUM (ISNULL(chargetable.PrimaryBilledAmount,0) + ISNULL(chargetable.SecondaryBilledAmount,0) + ISNULL(chargetable.TertiaryBilledAmount,0)) as BilledAmount  FROM Charge chargetable  join  Patient patienttable on chargetable.PatientID = patienttable.ID  join Visit v on chargetable.VisitID = v.ID  join Practice practicetable on chargetable.PracticeID = practicetable.ID  join [Provider] providertable on chargetable.ProviderID =  providertable.ID  join [Location] locationtable on chargetable.LocationID = locationtable.ID  join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID  join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID  join Cpt cpt on chargetable.CPTID = cpt.ID  left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID  left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID  left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID  left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID  left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID   left join PatientPlan otherpatientplantable on chargetable.TertiaryPatientPlanID =otherpatientplantable.ID left join InsurancePlan otherinsurancetable on otherpatientplantable.InsurancePlanID =otherinsurancetable.ID  left join POS postable on chargetable.POSID = postable.ID left join Modifier m1t on chargetable.Modifier1ID = m1t.ID left join Modifier m2t on chargetable.Modifier2ID = m2t.ID left join ICD icd1t on v.ICD1ID = icd1t.ID left join ICD icd2t on v.ICD2ID = icd2t.ID left join ICD icd3t on v.ICD3ID = icd3t.ID left join ICD icd4t on v.ICD4ID = icd4t.ID  where ";

                oString += "  chargetable.practiceid = {0} ";
                oString = string.Format(oString, PracticeId);

                if (!CRPatientVisit.PatientName.IsNull())
                    oString += string.Format(" and (patienttable.LastName + ' ' + patienttable.FirstName) like '%{0}%'", CRPatientVisit.PatientName);

                if (!CRPatientVisit.ProviderID.IsNull())
                    oString += string.Format(" and chargetable.ProviderID ='{0}'", CRPatientVisit.ProviderID);
                if (!CRPatientVisit.CPTCode.IsNull())
                    oString += string.Format(" and cpt.CPTCode ='{0}'", CRPatientVisit.CPTCode);
                if (!CRPatientVisit.PrescribingMD.IsNull())
                    oString += string.Format(" and v.PrescribingMD ='{0}'", CRPatientVisit.PrescribingMD);

                if (!CRPatientVisit.PaymentCriteria.IsNull())
                {
                    if (CRPatientVisit.PaymentCriteria == "Paid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "PartialPaid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "UnPaid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) = 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "PatientBal")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) > 0 ");
                    }
                }

                if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo != null)
                {
                    oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo == null)
                {
                    oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.DateOfServiceFrom == null && CRPatientVisit.DateOfServiceTo != null)
                {
                    oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date + "')");
                }


                if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo != null)
                {
                    oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo == null)
                {
                    oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.EntryDateFrom == null && CRPatientVisit.EntryDateTo != null)
                {
                    oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date + "')");
                }


                if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo != null)
                {
                    oString += (" and (chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo == null)
                {
                    oString += (" and ( chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.SubmittedDateFrom == null && CRPatientVisit.SubmittedDateTo != null)
                {
                    oString += (" and (chargetable.SubmittetdDate  <= '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date + "')");
                }

                oString += " GROUP BY  chargetable.SubmittetdDate ,  chargetable.DateOfServiceFrom, Cpt.CPTCode , postable.PosCode , (patienttable.LastName + ', ' + patienttable.FirstName) ,  insurangeplantable.[Description], chargetable.PrimaryPaymentDate,   chargetable.SecondaryPaymentDate  having SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ";

                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        var pvReport = new GRPatientVisitCountPaid();
                        pvReport.SubmissionDate = oReader["SubmissionDate"].ToString();
                        pvReport.DateOfService = oReader["DateOfService"].ToString();
                        pvReport.Cpt = oReader["Cpt"].ToString();
                        pvReport.POS = oReader["POS"].ToString();
                        pvReport.PaidAmount = Convert.ToDecimal(oReader["PaidAmount"].ToString());
                        pvReport.PlanBalance = Convert.ToDecimal(oReader["PlanBalance"].ToString());
                        pvReport.PatientBalance = Convert.ToDecimal(oReader["PatientBalance"].ToString());
                        pvReport.PatientName = oReader["PatientName"].ToString();
                        pvReport.InsuranceName = oReader["PayerName"].ToString();
                        pvReport.PaymentDate = oReader["PaymentDate"].ToString();
                        pvReport.SecondaryPaymentDate = oReader["SecondaryPaymentDate"].ToString();
                        pvReport.BilledAmount = Convert.ToDecimal(oReader["BilledAmount"].ToString());

                        data.Add(pvReport);
                    }
                    myConnection.Close();
                }
            }

            return data;



        }
        [HttpPost]
        [Route("ExportTotalCountPaid")]
        public async Task<IActionResult> ExportTotalCountPaid(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientVisitCountPaid> data = FindPatientVisitTotalCountPaid(CRPatientVisit, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CRPatientVisit, "Total Count Paid Report By CPT");
        }

        [HttpPost]
        [Route("ExportTotalCountPaidPdf")]
        public async Task<IActionResult> ExportTotalCountPaidPdf(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientVisitCountPaid> data = FindPatientVisitTotalCountPaid(CRPatientVisit, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }

        //_____________________________________________________________________


        [Route("FindPatientVisitTotalCountBilledRev")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRPatientVisitTotalRevenue>>> FindPatientVisitTotalCountBilledRev(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindPatientVisitTotalCountBilledRev(CRPatientVisit, UD);
        }

        private List<GRPatientVisitTotalRevenue> FindPatientVisitTotalCountBilledRev(CRPatientVisit CRPatientVisit, UserInfoData UD)
        {
            //List<GRPatientVisitTotalRevenue> data = (from chargetable in _context.Charge
            //                                         join v in _context.Visit on chargetable.VisitID equals v.ID
            //                                         join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
            //                                         join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
            //                                         //    join Paymentvisittable in _context.PaymentVisit on chargetable.VisitID equals Paymentvisittable.VisitID
            //                                         //    join paymentchecktable in _context.PaymentCheck on Paymentvisittable.PaymentCheckID equals paymentchecktable.ID
            //                                         join postable in _context.POS on chargetable.POSID equals postable.ID
            //                                         join cpttable in _context.Cpt on chargetable.CPTID equals cpttable.ID
            //                                         join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
            //                                         join insuranceplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insuranceplantable.ID
            //                                         where
            //                                           (CheckNullLong(CRPatientVisit.ProviderID) ? true : chargetable.ProviderID.Equals(CRPatientVisit.ProviderID)) &&
            //                                           (ExtensionMethods.IsBetweenDOS(CRPatientVisit.DateOfServiceTo, CRPatientVisit.DateOfServiceFrom, chargetable.DateOfServiceTo, chargetable.DateOfServiceFrom)) &&
            //                                           (ExtensionMethods.IsBetweenDOS(CRPatientVisit.EntryDateTo, CRPatientVisit.EntryDateFrom, chargetable.AddedDate, chargetable.AddedDate)) &&
            //                                           (ExtensionMethods.IsBetweenDOS(CRPatientVisit.SubmittedDateTo, CRPatientVisit.SubmittedDateFrom, chargetable.SubmittetdDate, chargetable.SubmittetdDate)) &&
            //                                           (CRPatientVisit.PatientName.IsNull() ? true : (patienttable.FirstName.Trim() + " " + patienttable.LastName.Trim()).Contains(CRPatientVisit.PatientName)) &&
            //                                            (CRPatientVisit.CPTCode.IsNull() ? true : cpttable.CPTCode.Equals(CRPatientVisit.CPTCode))
            //                                         select new GRPatientVisitTotalRevenue
            //                                         {
            //                                             PatientName = patienttable.LastName + ", " + patienttable.FirstName,
            //                                             PayerName = insuranceplantable.Description,
            //                                             POS = postable.PosCode,
            //                                             Cpt = cpttable.CPTCode,
            //                                             DateOfService = chargetable.DateOfServiceFrom.ToString(@"MM\/dd\/yyyy"),
            //                                             Charges = chargetable.TotalAmount,
            //                                             Payment = chargetable.PatientPaid.Val() + chargetable.PrimaryPaid.Val() + chargetable.SecondaryPaid.Val() + chargetable.TertiaryPaid.Val(),
            //                                             CollectedRevenue = chargetable.PatientPaid.Val() + chargetable.PrimaryPaid.Val() + chargetable.SecondaryPaid.Val() + chargetable.TertiaryPaid.Val(),
            //                                             Balance = chargetable.PrimaryPatientBal.Val() + chargetable.SecondaryPatientBal.Val() + chargetable.TertiaryPatientBal.Val() + chargetable.PrimaryBal.Val() + chargetable.SecondaryBal.Val() + chargetable.TertiaryBal.Val(),
            //                                             AverageRevenue = chargetable.PatientPaid.Val() + chargetable.PrimaryPaid.Val() + chargetable.SecondaryPaid.Val() + chargetable.TertiaryPaid.Val() / 4,
            //                                             SubmissionDate = chargetable.SubmittetdDate.Format("MM/dd/yyyy"),
            //                                             PaymentDate = chargetable.PrimaryPaymentDate.Format("MM/dd/yyyy"),
            //                                             SecondaryPaymentDate = chargetable.SecondaryPaymentDate.Format("MM/dd/yyyy"),
            //                                             PrescribingMD = v.PrescribingMD,
            //                                             Provider = providertable.Name
            //                                         }).ToList();
            //return data;



            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Client = (from w in _contextMain.MainPractice
                          where w.ID == PracticeId
                          select w
                        ).FirstOrDefault();
            string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
            string newConnection = GetConnectionStringManager(contextName);
            List<GRPatientVisitTotalRevenue> data = new List<GRPatientVisitTotalRevenue>();
            using (SqlConnection myConnection = new SqlConnection(newConnection))
            {
                string oString = "SELECT convert(varchar, chargetable.SubmittetdDate, 101) as  SubmissionDate,convert(varchar, chargetable.DateOfServiceFrom, 101)  as DateOfService, Cpt.CPTCode  as Cpt, postable.PosCode  as POS,chargetable.TotalAmount as Charges,  SUM (ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as Balance , (patienttable.LastName + ', ' + patienttable.FirstName) as PatientName , insurangeplantable.[Description] as PayerName, SUM (ISNULL(chargetable.PatientPaid,0) + ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0) ) as Payment ,SUM (ISNULL(chargetable.PatientPaid,0) + ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0) ) as CollectedRevenue, (SUM (ISNULL(chargetable.PatientPaid,0) + ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0) ) / 4) as AverageRevenue , convert(varchar, chargetable.PrimaryPaymentDate, 101) as PaymentDate ,   convert(varchar, chargetable.SecondaryPaymentDate, 101) as SecondaryPaymentDate, v.PrescribingMD as PrescribingMD, providertable.[Name] as [Provider]   FROM Charge chargetable  join  Patient patienttable on chargetable.PatientID = patienttable.ID  join Visit v on chargetable.VisitID = v.ID  join Practice practicetable on chargetable.PracticeID = practicetable.ID  join [Provider] providertable on chargetable.ProviderID =  providertable.ID  join [Location] locationtable on chargetable.LocationID = locationtable.ID  join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID  join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID  join Cpt cpt on chargetable.CPTID = cpt.ID  left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID  left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID  left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID  left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID  left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID   left join PatientPlan otherpatientplantable on chargetable.TertiaryPatientPlanID =otherpatientplantable.ID left join InsurancePlan otherinsurancetable on otherpatientplantable.InsurancePlanID =otherinsurancetable.ID  left join POS postable on chargetable.POSID = postable.ID left join Modifier m1t on chargetable.Modifier1ID = m1t.ID left join Modifier m2t on chargetable.Modifier2ID = m2t.ID left join ICD icd1t on v.ICD1ID = icd1t.ID left join ICD icd2t on v.ICD2ID = icd2t.ID left join ICD icd3t on v.ICD3ID = icd3t.ID left join ICD icd4t on v.ICD4ID = icd4t.ID  where ";

                oString += "  chargetable.practiceid = {0} ";
                oString = string.Format(oString, PracticeId);



                if (!CRPatientVisit.PatientName.IsNull())
                    oString += string.Format(" and (patienttable.LastName + ', ' + patienttable.FirstName) like '%{0}%'", CRPatientVisit.PatientName);

                if (!CRPatientVisit.ProviderID.IsNull())
                    oString += string.Format(" and chargetable.ProviderID ='{0}'", CRPatientVisit.ProviderID);
                if (!CRPatientVisit.CPTCode.IsNull())
                    oString += string.Format(" and cpt.CPTCode ='{0}'", CRPatientVisit.CPTCode);
                if (!CRPatientVisit.PrescribingMD.IsNull())
                    oString += string.Format(" and v.PrescribingMD ='{0}'", CRPatientVisit.PrescribingMD);

                if (!CRPatientVisit.PaymentCriteria.IsNull())
                {
                    if (CRPatientVisit.PaymentCriteria == "Paid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "PartialPaid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "UnPaid")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) = 0 ");
                    }
                    else if (CRPatientVisit.PaymentCriteria == "PatientBal")
                    {
                        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) > 0 ");
                    }
                }

                if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo != null)
                {
                    oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo == null)
                {
                    oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.DateOfServiceFrom == null && CRPatientVisit.DateOfServiceTo != null)
                {
                    oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date + "')");
                }


                if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo != null)
                {
                    oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo == null)
                {
                    oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.EntryDateFrom == null && CRPatientVisit.EntryDateTo != null)
                {
                    oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date + "')");
                }


                if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo != null)
                {
                    oString += (" and (chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo == null)
                {
                    oString += (" and ( chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (CRPatientVisit.SubmittedDateFrom == null && CRPatientVisit.SubmittedDateTo != null)
                {
                    oString += (" and (chargetable.SubmittetdDate  <= '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date + "')");
                }

                oString += " GROUP BY chargetable.SubmittetdDate , patienttable.FirstName , patienttable.LastName , patienttable.MiddleInitial , Cpt.CPTCode ,Cpt.[Description] ,  chargetable.DateOfServiceFrom , postable.PosCode , providertable.[Name] , chargetable.TotalAmount,  chargetable.PrimaryPaymentDate, chargetable.SecondaryPaymentDate,  insurangeplantable.[Description], v.PrescribingMD ,providertable.[Name] ";

                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        var pvReport = new GRPatientVisitTotalRevenue();
                        pvReport.SubmissionDate = oReader["SubmissionDate"].ToString();
                        pvReport.DateOfService = oReader["DateOfService"].ToString();
                        pvReport.Cpt = oReader["Cpt"].ToString();
                        pvReport.POS = oReader["POS"].ToString();
                        pvReport.Charges = Convert.ToDecimal(oReader["Charges"].ToString());
                        pvReport.Balance = Convert.ToDecimal(oReader["Balance"].ToString());
                        pvReport.PatientName = oReader["PatientName"].ToString();
                        pvReport.PayerName = oReader["PayerName"].ToString();
                        pvReport.Payment = Convert.ToDecimal(oReader["Payment"].ToString());
                        pvReport.CollectedRevenue = Convert.ToDecimal(oReader["CollectedRevenue"].ToString());
                        pvReport.AverageRevenue = Convert.ToDecimal(oReader["AverageRevenue"].ToString());
                        pvReport.PaymentDate = oReader["PaymentDate"].ToString();
                        pvReport.SecondaryPaymentDate = oReader["SecondaryPaymentDate"].ToString();
                        pvReport.PrescribingMD = oReader["PrescribingMD"].ToString();
                        pvReport.Provider = oReader["Provider"].ToString();

                        data.Add(pvReport);
                    }
                    myConnection.Close();
                }
            }

            return data;
        }

        [HttpPost]
        [Route("ExportTotalCountBilledRev")]
        public async Task<IActionResult> ExportTotalCountBilledRev(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientVisitTotalRevenue> data = FindPatientVisitTotalCountBilledRev(CRPatientVisit, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CRPatientVisit, "Total Revenue Collect By CPT Report");
        }
        [HttpPost]
        [Route("ExportTotalCountBilledRevPdf")]
        public async Task<IActionResult> ExportTotalCountBilledRevPdf(CRPatientVisit CRPatientVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientVisitTotalRevenue> data = FindPatientVisitTotalCountBilledRev(CRPatientVisit, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }
        //________________________________________________________________
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




        [Route("FindPatientVisitReports2")]
        [HttpPost]
        public async Task<ActionResult> FindPatientVisitReports2(CRPatientVisit CRPatientVisit)
        {
            int skipPage = (CRPatientVisit.pageNo - 1) * 10;
            int totalCount = 0, totalPages = 0, perPage = 0, currentPage = 0;
            List<GRPatientVisit> objPatient = new List<GRPatientVisit>();
            try
            {
                if (skipPage >= 0)
                {
                    // here Query



                    long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
                    var Client = (from w in _contextMain.MainPractice
                                  where w.ID == PracticeId
                                  select w
                                ).FirstOrDefault();
                    string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
                    string newConnection = GetConnectionStringManager(contextName);
                    List<GRPatientVisit> data3 = new List<GRPatientVisit>();
                    List<GRPatientVisit> dataCount = new List<GRPatientVisit>();
                    using (SqlConnection myConnection = new SqlConnection(newConnection))
                    {
                        string oString = "SELECT 	chargetable.PatientID as PatientID,chargetable.VisitID as  ClaimID, convert(varchar, chargetable.SubmittetdDate, 101) as  SubmissionDate, patienttable.FirstName  as PatientFirstName,patienttable.LastName  as PatientLastName, patienttable.MiddleInitial  as MiddleInitial, convert(varchar, patienttable.DOB, 101)  as DateOfBirth, patienttable.Gender  as PatientGender, patienttable.SSN  as SSN, insurangeplantable.PlanName  as PrimaryInsurance, patientplantable.SubscriberId  as PrimaryPolicyNumber,  secinsuranceplantable.PlanName as  SecondaryInsurance,  secpatientplantable.SubscriberId as  SecondaryPolicyNumber,  otherinsurancetable.PlanName  as OtherInsurance,otherpatientplantable.SubscriberId  as OtherPolicyNumber, convert(varchar, chargetable.DateOfServiceFrom, 101)  as DateOfService, providertable.[Name]  as ProviderName, providertable.NPI as  IndividualNPI,  refprovidertable.[Name] as  ReferringPhysicianName, postable.PosCode  as POS, postable.[Name]  as FacilityName, Cpt.CPTCode  as Cpt,Cpt.[Description]  as CptDescription, cpt.ShortDescription as ShortCptDescription, m1t.Code  as MOD1,m2t.Code  as MOD2, icd1t.ICDCode  as dx1,icd2t.ICDCode  as dx2,icd3t.ICDCode  as dx3,icd4t.ICDCode  as dx4, SUM (ISNULL(chargetable.PrimaryAllowed,0) + ISNULL(chargetable.SecondaryAllowed,0) + ISNULL(chargetable.TertiaryAllowed,0)) as AllowedAmount,chargetable.TotalAmount as Charges,  SUM (ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) as PaidAmount, SUM (ISNULL(chargetable.PrimaryWriteOff,0) + ISNULL( chargetable.SecondaryWriteOff, 0) + ISNULL(chargetable.TertiaryWriteOff,0) ) as AdjustmentAmount,  SUM (ISNULL(chargetable.PrimaryBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as PlanBalance,  SUM (ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as PatientBalance,  SUM (ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as Balance,   convert(varchar, chargetable.AddedDate, 101) as EntryDate,   convert(varchar, chargetable.PrimaryPaymentDate, 101) as PrimaryCheckDate, convert(varchar, chargetable.SecondaryPaymentDate, 101) as SecondaryCheckDate    fROM Charge chargetable  join  Patient patienttable on chargetable.PatientID = patienttable.ID  join Visit v on chargetable.VisitID = v.ID  join Practice practicetable on chargetable.PracticeID = practicetable.ID  join [Provider] providertable on chargetable.ProviderID =  providertable.ID  join [Location] locationtable on chargetable.LocationID = locationtable.ID  join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID  join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID  join Cpt cpt on chargetable.CPTID = cpt.ID  left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID  left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID  left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID  left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID  left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID   left join PatientPlan otherpatientplantable on chargetable.TertiaryPatientPlanID =otherpatientplantable.ID left join InsurancePlan otherinsurancetable on otherpatientplantable.InsurancePlanID =otherinsurancetable.ID  left join POS postable on chargetable.POSID = postable.ID left join Modifier m1t on chargetable.Modifier1ID = m1t.ID left join Modifier m2t on chargetable.Modifier2ID = m2t.ID left join ICD icd1t on v.ICD1ID = icd1t.ID left join ICD icd2t on v.ICD2ID = icd2t.ID left join ICD icd3t on v.ICD3ID = icd3t.ID left join ICD icd4t on v.ICD4ID = icd4t.ID  where ";

                        oString += "  chargetable.practiceid = {0} ";
                        oString = string.Format(oString, PracticeId);



                        if (!CRPatientVisit.PatientName.IsNull())
                            oString += string.Format(" and (patienttable.LastName + ', ' + patienttable.FirstName) like '%{0}%'", CRPatientVisit.PatientName);

                        if (!CRPatientVisit.ProviderID.IsNull())
                            oString += string.Format(" and chargetable.ProviderID ='{0}'", CRPatientVisit.ProviderID);
                        if (!CRPatientVisit.CPTCode.IsNull())
                            oString += string.Format(" and cpt.CPTCode ='{0}'", CRPatientVisit.CPTCode);
                        if (!CRPatientVisit.PrescribingMD.IsNull())
                            oString += string.Format(" and v.PrescribingMD ='{0}'", CRPatientVisit.PrescribingMD);

                        if (!CRPatientVisit.PaymentCriteria.IsNull())
                        {
                            if (CRPatientVisit.PaymentCriteria == "Paid")
                            {
                                oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                            }
                            else if (CRPatientVisit.PaymentCriteria == "PartialPaid")
                            {
                                oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                            }
                            else if (CRPatientVisit.PaymentCriteria == "UnPaid")
                            {
                                oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) = 0 ");
                            }
                            else if (CRPatientVisit.PaymentCriteria == "PatientBal")
                            {
                                oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) > 0 ");
                            }
                        }

                        if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo == null)
                        {
                            oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRPatientVisit.DateOfServiceFrom == null && CRPatientVisit.DateOfServiceTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date + "')");
                        }


                        if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo == null)
                        {
                            oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRPatientVisit.EntryDateFrom == null && CRPatientVisit.EntryDateTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date + "')");
                        }


                        if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo != null)
                        {
                            oString += (" and (chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo == null)
                        {
                            oString += (" and ( chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRPatientVisit.SubmittedDateFrom == null && CRPatientVisit.SubmittedDateTo != null)
                        {
                            oString += (" and (chargetable.SubmittetdDate  <= '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date + "')");
                        }
                        oString += " GROUP BY chargetable.PatientID , chargetable.VisitID , chargetable.SubmittetdDate , patienttable.FirstName , patienttable.LastName , patienttable.MiddleInitial , patienttable.DOB , patienttable.Gender , patienttable.SSN , insurangeplantable.PlanName ,  patientplantable.SubscriberId , secinsuranceplantable.PlanName , secpatientplantable.SubscriberId , otherinsurancetable.PlanName ,otherpatientplantable.SubscriberId ,  chargetable.DateOfServiceFrom ,  providertable.[Name] , providertable.NPI ,   refprovidertable.[Name] , postable.[Name] , Cpt.CPTCode,  postable.PosCode ,Cpt.[Description] , m1t.Code , m2t.Code , icd1t.ICDCode , icd2t.ICDCode ,icd3t.ICDCode , icd4t.ICDCode ,  chargetable.TotalAmount, chargetable.AddedDate, chargetable.PrimaryPaymentDate, chargetable.SecondaryPaymentDate, cpt.ShortDescription ORDER BY chargetable.PatientID OFFSET " + skipPage + " ROWS FETCH NEXT 10 ROWS ONLY";

                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                var pvReport = new GRPatientVisit();
                                pvReport.PatientID = Convert.ToInt64(oReader["PatientID"].ToString());
                                pvReport.VisitNo = Convert.ToInt64(oReader["ClaimID"].ToString());
                                pvReport.SubmissionDate = oReader["SubmissionDate"].ToString();
                                pvReport.PatientFirstName = oReader["PatientFirstName"].ToString();
                                pvReport.PatientLastName = oReader["PatientLastName"].ToString();
                                pvReport.MiddleInitial = oReader["MiddleInitial"].ToString();
                                pvReport.DateOfBirth = oReader["DateOfBirth"].ToString();
                                pvReport.PatientGender = oReader["PatientGender"].ToString();
                                pvReport.SSN = oReader["SSN"].ToString();
                                pvReport.PrimaryInsurance = oReader["PrimaryInsurance"].ToString();
                                pvReport.PrimarySubscriberId = oReader["PrimaryPolicyNumber"].ToString();
                                pvReport.SecondaryInsurance = oReader["SecondaryInsurance"].ToString();
                                pvReport.SecondarySubscriberId = oReader["SecondaryPolicyNumber"].ToString();
                                pvReport.OtherInsurance = oReader["OtherInsurance"].ToString();
                                pvReport.OtherSubscriberId = oReader["OtherPolicyNumber"].ToString();
                                pvReport.DateOfService = oReader["DateOfService"].ToString();
                                pvReport.ProviderName = oReader["ProviderName"].ToString();
                                pvReport.IndividualNPI = oReader["IndividualNPI"].ToString();
                                pvReport.ReferringPhysicianName = oReader["ReferringPhysicianName"].ToString();
                                pvReport.FacilityName = oReader["FacilityName"].ToString();
                                pvReport.Cpt = oReader["Cpt"].ToString();
                                pvReport.POS = oReader["POS"].ToString();
                                pvReport.CptDescription = oReader["CptDescription"].ToString();
                                pvReport.ShortCptDescription = oReader["ShortCptDescription"].ToString();
                                pvReport.MOD1 = oReader["MOD1"].ToString();
                                pvReport.MOD2 = oReader["MOD2"].ToString();
                                pvReport.dx1 = oReader["dx1"].ToString();
                                pvReport.dx2 = oReader["dx2"].ToString();
                                pvReport.dx3 = oReader["dx3"].ToString();
                                pvReport.dx4 = oReader["dx4"].ToString();
                                pvReport.Charges = Convert.ToDecimal(oReader["Charges"].ToString());
                                pvReport.AllowedAmount = Convert.ToDecimal(oReader["AllowedAmount"].ToString());
                                pvReport.PaidAmount = Convert.ToDecimal(oReader["PaidAmount"].ToString());
                                pvReport.AdjustmentAmount = Convert.ToDecimal(oReader["AdjustmentAmount"].ToString());
                                pvReport.PlanBalance = Convert.ToDecimal(oReader["PlanBalance"].ToString());
                                pvReport.PatientBalance = Convert.ToDecimal(oReader["PatientBalance"].ToString());
                                pvReport.Balance = Convert.ToDecimal(oReader["Balance"].ToString());
                                pvReport.EntryDate = oReader["EntryDate"].ToString();
                                pvReport.PrimaryCheckDate = oReader["PrimaryCheckDate"].ToString();
                                pvReport.SecondaryCheckDate = oReader["SecondaryCheckDate"].ToString();

                                data3.Add(pvReport);
                            }
                            myConnection.Close();
                        }
                    }


                    List<GRPatientVisit> data2 = new List<GRPatientVisit>();
                    using (SqlConnection myConnection = new SqlConnection(newConnection))
                    {
                        string oString = "SELECT 	chargetable.PatientID as PatientID,chargetable.VisitID as  ClaimID, convert(varchar, chargetable.SubmittetdDate, 101) as  SubmissionDate, patienttable.FirstName  as PatientFirstName,patienttable.LastName  as PatientLastName, patienttable.MiddleInitial  as MiddleInitial, convert(varchar, patienttable.DOB, 101)  as DateOfBirth, patienttable.Gender  as PatientGender, patienttable.SSN  as SSN, insurangeplantable.PlanName  as PrimaryInsurance, patientplantable.SubscriberId  as PrimaryPolicyNumber,  secinsuranceplantable.PlanName as  SecondaryInsurance,  secpatientplantable.SubscriberId as  SecondaryPolicyNumber,  otherinsurancetable.PlanName  as OtherInsurance,otherpatientplantable.SubscriberId  as OtherPolicyNumber, convert(varchar, chargetable.DateOfServiceFrom, 101)  as DateOfService, providertable.[Name]  as ProviderName, providertable.NPI as  IndividualNPI,  refprovidertable.[Name] as  ReferringPhysicianName, postable.PosCode  as POS, postable.[Name]  as FacilityName, Cpt.CPTCode  as Cpt,Cpt.[Description]  as CptDescription, cpt.ShortDescription as ShortCptDescription, m1t.Code  as MOD1,m2t.Code  as MOD2, icd1t.ICDCode  as dx1,icd2t.ICDCode  as dx2,icd3t.ICDCode  as dx3,icd4t.ICDCode  as dx4, SUM (ISNULL(chargetable.PrimaryAllowed,0) + ISNULL(chargetable.SecondaryAllowed,0) + ISNULL(chargetable.TertiaryAllowed,0)) as AllowedAmount,chargetable.TotalAmount as Charges,  SUM (ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) as PaidAmount, SUM (ISNULL(chargetable.PrimaryWriteOff,0) + ISNULL( chargetable.SecondaryWriteOff, 0) + ISNULL(chargetable.TertiaryWriteOff,0) ) as AdjustmentAmount,  SUM (ISNULL(chargetable.PrimaryBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as PlanBalance,  SUM (ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as PatientBalance,  SUM (ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as Balance,   convert(varchar, chargetable.AddedDate, 101) as EntryDate,   convert(varchar, chargetable.PrimaryPaymentDate, 101) as PrimaryCheckDate, convert(varchar, chargetable.SecondaryPaymentDate, 101) as SecondaryCheckDate    fROM Charge chargetable  join  Patient patienttable on chargetable.PatientID = patienttable.ID  join Visit v on chargetable.VisitID = v.ID  join Practice practicetable on chargetable.PracticeID = practicetable.ID  join [Provider] providertable on chargetable.ProviderID =  providertable.ID  join [Location] locationtable on chargetable.LocationID = locationtable.ID  join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID  join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID  join Cpt cpt on chargetable.CPTID = cpt.ID  left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID  left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID  left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID  left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID  left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID   left join PatientPlan otherpatientplantable on chargetable.TertiaryPatientPlanID =otherpatientplantable.ID left join InsurancePlan otherinsurancetable on otherpatientplantable.InsurancePlanID =otherinsurancetable.ID  left join POS postable on chargetable.POSID = postable.ID left join Modifier m1t on chargetable.Modifier1ID = m1t.ID left join Modifier m2t on chargetable.Modifier2ID = m2t.ID left join ICD icd1t on v.ICD1ID = icd1t.ID left join ICD icd2t on v.ICD2ID = icd2t.ID left join ICD icd3t on v.ICD3ID = icd3t.ID left join ICD icd4t on v.ICD4ID = icd4t.ID  where ";

                        oString += "  chargetable.practiceid = {0} ";
                        oString = string.Format(oString, PracticeId);



                        if (!CRPatientVisit.PatientName.IsNull())
                            oString += string.Format(" and (patienttable.LastName + ', ' + patienttable.FirstName) like '%{0}%'", CRPatientVisit.PatientName);

                        if (!CRPatientVisit.ProviderID.IsNull())
                            oString += string.Format(" and chargetable.ProviderID ='{0}'", CRPatientVisit.ProviderID);
                        if (!CRPatientVisit.CPTCode.IsNull())
                            oString += string.Format(" and cpt.CPTCode ='{0}'", CRPatientVisit.CPTCode);
                        if (!CRPatientVisit.PrescribingMD.IsNull())
                            oString += string.Format(" and v.PrescribingMD ='{0}'", CRPatientVisit.PrescribingMD);

                        if (!CRPatientVisit.PaymentCriteria.IsNull())
                        {
                            if (CRPatientVisit.PaymentCriteria == "Paid")
                            {
                                oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                            }
                            else if (CRPatientVisit.PaymentCriteria == "PartialPaid")
                            {
                                oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                            }
                            else if (CRPatientVisit.PaymentCriteria == "UnPaid")
                            {
                                oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) = 0 ");
                            }
                            else if (CRPatientVisit.PaymentCriteria == "PatientBal")
                            {
                                oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) > 0 ");
                            }
                        }

                        if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRPatientVisit.DateOfServiceFrom != null && CRPatientVisit.DateOfServiceTo == null)
                        {
                            oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.DateOfServiceFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRPatientVisit.DateOfServiceFrom == null && CRPatientVisit.DateOfServiceTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.DateOfServiceTo.GetValueOrDefault().Date + "')");
                        }


                        if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRPatientVisit.EntryDateFrom != null && CRPatientVisit.EntryDateTo == null)
                        {
                            oString += (" and ( chargetable.AddedDate  >= '" + CRPatientVisit.EntryDateFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRPatientVisit.EntryDateFrom == null && CRPatientVisit.EntryDateTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  <= '" + CRPatientVisit.EntryDateTo.GetValueOrDefault().Date + "')");
                        }


                        if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo != null)
                        {
                            oString += (" and (chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRPatientVisit.SubmittedDateFrom != null && CRPatientVisit.SubmittedDateTo == null)
                        {
                            oString += (" and ( chargetable.SubmittetdDate  >= '" + CRPatientVisit.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRPatientVisit.SubmittedDateFrom == null && CRPatientVisit.SubmittedDateTo != null)
                        {
                            oString += (" and (chargetable.SubmittetdDate  <= '" + CRPatientVisit.SubmittedDateTo.GetValueOrDefault().Date + "')");
                        }
                        oString += " GROUP BY chargetable.PatientID , chargetable.VisitID , chargetable.SubmittetdDate , patienttable.FirstName , patienttable.LastName , patienttable.MiddleInitial , patienttable.DOB , patienttable.Gender , patienttable.SSN , insurangeplantable.PlanName ,  patientplantable.SubscriberId , secinsuranceplantable.PlanName , secpatientplantable.SubscriberId , otherinsurancetable.PlanName ,otherpatientplantable.SubscriberId ,  chargetable.DateOfServiceFrom ,  providertable.[Name] , providertable.NPI ,   refprovidertable.[Name] , postable.[Name] , Cpt.CPTCode,  postable.PosCode ,Cpt.[Description] , m1t.Code , m2t.Code , icd1t.ICDCode , icd2t.ICDCode ,icd3t.ICDCode , icd4t.ICDCode ,  chargetable.TotalAmount, chargetable.AddedDate, chargetable.PrimaryPaymentDate, chargetable.SecondaryPaymentDate, cpt.ShortDescription ORDER BY chargetable.PatientID OFFSET " + skipPage + " ROWS FETCH NEXT " + CRPatientVisit.PerPage + " ROWS ONLY";

                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                var pvReport = new GRPatientVisit();
                                pvReport.PatientID = Convert.ToInt64(oReader["PatientID"].ToString());
                                dataCount.Add(pvReport);
                            }
                            myConnection.Close();
                        }
                    }


                    totalCount = dataCount.Count();
                    totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalCount) / CRPatientVisit.PerPage));
                    perPage = CRPatientVisit.PerPage;
                    currentPage = CRPatientVisit.pageNo;
                    objPatient = data2;


                }

                var z = new { data = objPatient, TotalCount = totalCount, totalPages = totalPages, PerPage = perPage, CurrentPage = currentPage };
                return Ok(z);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("FindPatientVisitReports3")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRPatientVisit>>> FindPatientVisitReports3(CRPatientVisit CRPatientVisit)
        {

            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Client = (from w in _contextMain.MainPractice
                          where w.ID == PracticeId
                          select w
                        ).FirstOrDefault();
            string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
            string newConnection = GetConnectionStringManager(contextName);
            List<GRPatientVisit> data3 = new List<GRPatientVisit>();
            List<GRPatientVisit> dataCount = new List<GRPatientVisit>();


            // List<AgingCOThird> agingCOThird = new List<AgingCOThird>();
            List<GRPatientVisit> data = new List<GRPatientVisit>();
            using (SqlConnection myConnection = new SqlConnection(newConnection))
            {

                //set stored procedure name
                string Sql = @"dbo.[FindPatientVisitReports]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(Sql, myConnection);

                ////Set SqlParameter - the employee id parameter value will be set from the command line

                cmd.Parameters.Add("@ProviderID", SqlDbType.NVarChar).Value = CRPatientVisit.ProviderID;
                cmd.Parameters.Add("@DateOfServiceTo", SqlDbType.DateTime).Value = CRPatientVisit.DateOfServiceTo;
                cmd.Parameters.Add("@DateOfServiceFrom", SqlDbType.DateTime).Value = CRPatientVisit.DateOfServiceFrom;

                cmd.Parameters.Add("@EntryDateFrom", SqlDbType.DateTime).Value = CRPatientVisit.EntryDateFrom;
                cmd.Parameters.Add("@EntryDateTo", SqlDbType.DateTime).Value = CRPatientVisit.EntryDateTo;

                cmd.Parameters.Add("@SubmittedDateFrom", SqlDbType.DateTime).Value = CRPatientVisit.SubmittedDateFrom;
                cmd.Parameters.Add("@SubmittedDateTo", SqlDbType.DateTime).Value = CRPatientVisit.SubmittedDateTo;


                cmd.Parameters.Add("@PatientName", SqlDbType.NVarChar).Value = CRPatientVisit.PatientName;
                cmd.Parameters.Add("@CPTCode", SqlDbType.NVarChar).Value = CRPatientVisit.CPTCode;
                cmd.Parameters.Add("@VisitType", SqlDbType.NVarChar).Value = CRPatientVisit.VisitType;
                cmd.Parameters.Add("@PaymentCriteria", SqlDbType.NVarChar).Value = CRPatientVisit.PaymentCriteria;
                cmd.Parameters.Add("@PrescribingMD", SqlDbType.NVarChar).Value = CRPatientVisit.PrescribingMD;
                cmd.Parameters.Add("@pageNo", SqlDbType.Int).Value = CRPatientVisit.pageNo;
                cmd.Parameters.Add("@PerPage", SqlDbType.Int).Value = CRPatientVisit.PerPage;


                //open connection
                myConnection.Open();

                //set the SqlCommand type to stored procedure and execute
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader oReader = cmd.ExecuteReader();

                //check if there are records
                if (oReader.HasRows)
                {
                    while (oReader.Read())
                    {
                        var rPvisit = new GRPatientVisit();

                        rPvisit.PatientID = Convert.ToInt64(oReader["PatientID"].ToString());
                        //rPvisit.PlanAmount = Convert.ToDecimal(oReader["planAmount"].ToString());
                        //rPvisit.PaidAmount = Convert.ToDecimal(oReader["patientAmount"].ToString());
                        data.Add(rPvisit);
                    }
                }

                //close data reader
                oReader.Close();

                //close connection
                myConnection.Close();
            }


            return data;
        }
        private bool CheckNullLong(long? Value)
        {
            Debug.WriteLine(Value.IsNull());
            return Value.IsNull();
        }

    }
}