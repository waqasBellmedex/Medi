using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediFusionPM.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using MediFusionPM.ViewModels;
using MediFusionPM.ViewModel;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RightsController : ControllerBase
    {
        private readonly ClientDbContext _context;

        public RightsController(ClientDbContext context)
        {
            _context = context;
        }


        // GET: api/Rights/5
        [Route("GetRights/{email}")]
        [HttpGet("{email}")]
        public async Task<ActionResult<Rights>> GetRights(string Email)
        {




            var getUser = (from u in _context.Users
                           where u.Email == Email
                           select new { u.Id }
                           ).FirstOrDefault();
            if (getUser != null)
            {
                Rights CheckRights = (from rig in _context.Rights
                                      where rig.Id == getUser.Id
                                      select rig
                                   ).FirstOrDefault();
                if (CheckRights != null)
                {
                    return CheckRights;
                }
                else
                {
                    return BadRequest("Right Not Found.");
                }
            }
            else
            {
                return BadRequest("User Not Found.");
            }
        }

        // POST: api/Rights
        [HttpPost]
        [Route("SaveRights")]
        public async Task<ActionResult<Rights>> SaveRights(VMRights vmRights)
        {

            Rights rights = new Rights();
            var CurrentLoginUserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti));
            var CurrentLoginEmail = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            //PropertyCopy.Copy(rights, vmRights);

            //Check User
            var user = (from u in _context.Users
                        where u.Email == vmRights.Email
                        select u
                                    ).FirstOrDefault();

            if (user != null)
            {
                //check Rights already assigned or not
                var chkRights = (from u in _context.Rights
                                 where u.Id == user.Id
                                 select u
                    ).FirstOrDefault();
                //if not abefore not assigned
                if (chkRights == null)
                {
                    vmRights.Rights.Id = user.Id;
                    vmRights.Rights.AddedBy = user.Email;
                    _context.Rights.Add(vmRights.Rights);
                    _context.SaveChanges();

                    return Ok(vmRights);
                }
                else
                {

                    //Scheduler Rights
                    chkRights.SchedulerCreate = vmRights.Rights.SchedulerCreate;
                    chkRights.SchedulerEdit = vmRights.Rights.SchedulerEdit;
                    chkRights.SchedulerDelete = vmRights.Rights.SchedulerDelete;
                    chkRights.SchedulerSearch = vmRights.Rights.SchedulerSearch;
                    chkRights.SchedulerImport = vmRights.Rights.SchedulerImport;
                    chkRights.SchedulerExport = vmRights.Rights.SchedulerExport;



                    //Patient Rights
                    chkRights.PatientCreate = vmRights.Rights.PatientCreate;
                    chkRights.PatientEdit = vmRights.Rights.PatientEdit;
                    chkRights.PatientDelete = vmRights.Rights.PatientDelete;
                    chkRights.PatientSearch = vmRights.Rights.PatientSearch;
                    chkRights.PatientImport = vmRights.Rights.PatientImport;
                    chkRights.PatientExport = vmRights.Rights.PatientExport;

                    //Charges Rights
                    chkRights.ChargesCreate = vmRights.Rights.ChargesCreate;
                    chkRights.ChargesEdit = vmRights.Rights.ChargesEdit;
                    chkRights.ChargesDelete = vmRights.Rights.ChargesDelete;
                    chkRights.ChargesSearch = vmRights.Rights.ChargesSearch;
                    chkRights.ChargesImport = vmRights.Rights.ChargesImport;
                    chkRights.ChargesExport = vmRights.Rights.ChargesExport;

                    //Documents Rights
                    chkRights.DocumentsCreate = vmRights.Rights.DocumentsCreate;
                    chkRights.DocumentsEdit = vmRights.Rights.DocumentsEdit;
                    chkRights.DocumentsDelete = vmRights.Rights.DocumentsDelete;
                    chkRights.DocumentsSearch = vmRights.Rights.DocumentsSearch;
                    chkRights.DocumentsImport = vmRights.Rights.DocumentsImport;
                    chkRights.DocumentsExport = vmRights.Rights.DocumentsExport;

                    //Submissions Rights
                    chkRights.SubmissionsCreate = vmRights.Rights.SubmissionsCreate;
                    chkRights.SubmissionsEdit = vmRights.Rights.SubmissionsEdit;
                    chkRights.SubmissionsDelete = vmRights.Rights.SubmissionsDelete;
                    chkRights.SubmissionsSearch = vmRights.Rights.SubmissionsSearch;
                    chkRights.SubmissionsImport = vmRights.Rights.SubmissionsImport;
                    chkRights.SubmissionsExport = vmRights.Rights.SubmissionsExport;

                    //Payments Rights
                    chkRights.PaymentsCreate = vmRights.Rights.PaymentsCreate;
                    chkRights.PaymentsEdit = vmRights.Rights.PaymentsEdit;
                    chkRights.PaymentsDelete = vmRights.Rights.PaymentsDelete;
                    chkRights.PaymentsSearch = vmRights.Rights.PaymentsSearch;
                    chkRights.PaymentsImport = vmRights.Rights.PaymentsImport;
                    chkRights.PaymentsExport = vmRights.Rights.PaymentsExport;

                    //Followup Rights
                    chkRights.FollowupCreate = vmRights.Rights.FollowupCreate;
                    chkRights.FollowupEdit = vmRights.Rights.FollowupEdit;
                    chkRights.FollowupDelete = vmRights.Rights.FollowupDelete;
                    chkRights.FollowupSearch = vmRights.Rights.FollowupSearch;
                    chkRights.FollowupImport = vmRights.Rights.FollowupImport;
                    chkRights.FollowupExport = vmRights.Rights.FollowupExport;

                    //Reports Rights
                    chkRights.ReportsCreate = vmRights.Rights.ReportsCreate;
                    chkRights.ReportsEdit = vmRights.Rights.ReportsEdit;
                    chkRights.ReportsDelete = vmRights.Rights.ReportsDelete;
                    chkRights.ReportsSearch = vmRights.Rights.ReportsSearch;
                    chkRights.ReportsImport = vmRights.Rights.ReportsImport;
                    chkRights.ReportsExport = vmRights.Rights.ReportsExport;


                    ////SetUp Client 
                    //Client Rights
                    chkRights.ClientCreate = vmRights.Rights.ClientCreate;
                    chkRights.ClientEdit = vmRights.Rights.ClientEdit;
                    chkRights.ClientDelete = vmRights.Rights.ClientDelete;
                    chkRights.ClientSearch = vmRights.Rights.ClientSearch;
                    chkRights.ClientImport = vmRights.Rights.ClientImport;
                    chkRights.ClientExport = vmRights.Rights.ClientExport;

                    chkRights.UserCreate = vmRights.Rights.UserCreate;
                    chkRights.UserEdit = vmRights.Rights.UserEdit;
                    chkRights.UserDelete = vmRights.Rights.UserDelete;
                    chkRights.UserSearch = vmRights.Rights.UserSearch;
                    chkRights.UserImport = vmRights.Rights.UserImport;
                    chkRights.UserExport = vmRights.Rights.UserExport;

                    ////SetUp Admin 
                    //Practice
                    chkRights.PracticeCreate = vmRights.Rights.PracticeCreate;
                    chkRights.PracticeEdit = vmRights.Rights.PracticeEdit;
                    chkRights.PracticeDelete = vmRights.Rights.PracticeDelete;
                    chkRights.PracticeSearch = vmRights.Rights.PracticeSearch;
                    chkRights.PracticeImport = vmRights.Rights.PracticeImport;
                    chkRights.PracticeExport = vmRights.Rights.PracticeExport;

                    //Location
                    chkRights.LocationCreate = vmRights.Rights.LocationCreate;
                    chkRights.LocationEdit = vmRights.Rights.LocationEdit;
                    chkRights.LocationDelete = vmRights.Rights.LocationDelete;
                    chkRights.LocationSearch = vmRights.Rights.LocationSearch;
                    chkRights.LocationImport = vmRights.Rights.LocationImport;
                    chkRights.LocationExport = vmRights.Rights.LocationExport;

                    //Provider
                    chkRights.ProviderCreate = vmRights.Rights.ProviderCreate;
                    chkRights.ProviderEdit = vmRights.Rights.ProviderEdit;
                    chkRights.ProviderDelete = vmRights.Rights.ProviderDelete;
                    chkRights.ProviderSearch = vmRights.Rights.ProviderSearch;
                    chkRights.ProviderImport = vmRights.Rights.ProviderImport;
                    chkRights.ProviderExport = vmRights.Rights.ProviderExport;


                    //Referring Provider
                    chkRights.ReferringProviderCreate = vmRights.Rights.ReferringProviderCreate;
                    chkRights.ReferringProviderEdit = vmRights.Rights.ReferringProviderEdit;
                    chkRights.ReferringProviderDelete = vmRights.Rights.ReferringProviderDelete;
                    chkRights.ReferringProviderSearch = vmRights.Rights.ReferringProviderSearch;
                    chkRights.ReferringProviderImport = vmRights.Rights.ReferringProviderImport;
                    chkRights.ReferringProviderExport = vmRights.Rights.ReferringProviderExport;

                    //Setup Insurance
                    //Insurance

                    chkRights.InsuranceCreate = vmRights.Rights.InsuranceCreate;
                    chkRights.InsuranceEdit = vmRights.Rights.InsuranceEdit;
                    chkRights.InsuranceDelete = vmRights.Rights.InsuranceDelete;
                    chkRights.InsuranceSearch = vmRights.Rights.InsuranceSearch;
                    chkRights.InsuranceImport = vmRights.Rights.InsuranceImport;
                    chkRights.InsuranceExport = vmRights.Rights.InsuranceExport;

                    //Insurance Plan 
                    chkRights.InsurancePlanCreate = vmRights.Rights.InsurancePlanCreate;
                    chkRights.InsurancePlanEdit = vmRights.Rights.InsurancePlanEdit;
                    chkRights.InsurancePlanDelete = vmRights.Rights.InsurancePlanDelete;
                    chkRights.InsurancePlanSearch = vmRights.Rights.InsurancePlanSearch;
                    chkRights.InsurancePlanImport = vmRights.Rights.InsurancePlanImport;
                    chkRights.InsurancePlanExport = vmRights.Rights.InsurancePlanExport;

                    //Insurance Plan Address 
                    chkRights.InsurancePlanAddressCreate = vmRights.Rights.InsurancePlanAddressCreate;
                    chkRights.InsurancePlanAddressEdit = vmRights.Rights.InsurancePlanAddressEdit;
                    chkRights.InsurancePlanAddressDelete = vmRights.Rights.InsurancePlanAddressDelete;
                    chkRights.InsurancePlanAddressSearch = vmRights.Rights.InsurancePlanAddressSearch;
                    chkRights.InsurancePlanAddressImport = vmRights.Rights.InsurancePlanAddressImport;
                    chkRights.InsurancePlanAddressExport = vmRights.Rights.InsurancePlanAddressExport;

                    //EDI Submit
                    chkRights.EDISubmitCreate = vmRights.Rights.EDISubmitCreate;
                    chkRights.EDISubmitEdit = vmRights.Rights.EDISubmitEdit;
                    chkRights.EDISubmitDelete = vmRights.Rights.EDISubmitDelete;
                    chkRights.EDISubmitSearch = vmRights.Rights.EDISubmitSearch;
                    chkRights.EDISubmitImport = vmRights.Rights.EDISubmitImport;
                    chkRights.EDISubmitExport = vmRights.Rights.EDISubmitExport;

                    //EDI EligiBility
                    chkRights.EDIEligiBilityCreate = vmRights.Rights.EDIEligiBilityCreate;
                    chkRights.EDIEligiBilityEdit = vmRights.Rights.EDIEligiBilityEdit;
                    chkRights.EDIEligiBilityDelete = vmRights.Rights.EDIEligiBilityDelete;
                    chkRights.EDIEligiBilitySearch = vmRights.Rights.EDIEligiBilitySearch;
                    chkRights.EDIEligiBilityImport = vmRights.Rights.EDIEligiBilityImport;
                    chkRights.EDIEligiBilityExport = vmRights.Rights.EDIEligiBilityExport;

                    //EDI Status
                    chkRights.EDIStatusCreate = vmRights.Rights.EDIStatusCreate;
                    chkRights.EDIStatusEdit = vmRights.Rights.EDIStatusEdit;
                    chkRights.EDIStatusDelete = vmRights.Rights.EDIStatusDelete;
                    chkRights.EDIStatusSearch = vmRights.Rights.EDIStatusSearch;
                    chkRights.EDIStatusImport = vmRights.Rights.EDIStatusImport;
                    chkRights.EDIStatusExport = vmRights.Rights.EDIStatusExport;

                    //Coding
                    //ICD
                    chkRights.ICDCreate = vmRights.Rights.ICDCreate;
                    chkRights.ICDEdit = vmRights.Rights.ICDEdit;
                    chkRights.ICDDelete = vmRights.Rights.ICDDelete;
                    chkRights.ICDSearch = vmRights.Rights.ICDSearch;
                    chkRights.ICDImport = vmRights.Rights.ICDImport;
                    chkRights.ICDExport = vmRights.Rights.ICDExport;

                    //CPT
                    chkRights.CPTCreate = vmRights.Rights.CPTCreate;
                    chkRights.CPTEdit = vmRights.Rights.CPTEdit;
                    chkRights.CPTDelete = vmRights.Rights.CPTDelete;
                    chkRights.CPTSearch = vmRights.Rights.CPTSearch;
                    chkRights.CPTImport = vmRights.Rights.CPTImport;
                    chkRights.CPTExport = vmRights.Rights.CPTExport;

                    //Modifiers
                    chkRights.ModifiersCreate = vmRights.Rights.ModifiersCreate;
                    chkRights.ModifiersEdit = vmRights.Rights.ModifiersEdit;
                    chkRights.ModifiersDelete = vmRights.Rights.ModifiersDelete;
                    chkRights.ModifiersSearch = vmRights.Rights.ModifiersSearch;
                    chkRights.ModifiersImport = vmRights.Rights.ModifiersImport;
                    chkRights.ModifiersExport = vmRights.Rights.ModifiersExport;

                    //POS
                    chkRights.POSCreate = vmRights.Rights.POSCreate;
                    chkRights.POSEdit = vmRights.Rights.POSEdit;
                    chkRights.POSDelete = vmRights.Rights.POSDelete;
                    chkRights.POSSearch = vmRights.Rights.POSSearch;
                    chkRights.POSImport = vmRights.Rights.POSImport;
                    chkRights.POSExport = vmRights.Rights.POSExport;

                    //EDI Codes
                    //Claim Status Category Codes
                    chkRights.ClaimStatusCategoryCodesCreate = vmRights.Rights.ClaimStatusCategoryCodesCreate;
                    chkRights.ClaimStatusCategoryCodesEdit = vmRights.Rights.ClaimStatusCategoryCodesEdit;
                    chkRights.ClaimStatusCategoryCodesDelete = vmRights.Rights.ClaimStatusCategoryCodesDelete;
                    chkRights.ClaimStatusCategoryCodesSearch = vmRights.Rights.ClaimStatusCategoryCodesSearch;
                    chkRights.ClaimStatusCategoryCodesImport = vmRights.Rights.ClaimStatusCategoryCodesImport;
                    chkRights.ClaimStatusCategoryCodesExport = vmRights.Rights.ClaimStatusCategoryCodesExport;

                    //Claim Status Codes
                    chkRights.ClaimStatusCodesCreate = vmRights.Rights.ClaimStatusCodesCreate;
                    chkRights.ClaimStatusCodesEdit = vmRights.Rights.ClaimStatusCodesEdit;
                    chkRights.ClaimStatusCodesDelete = vmRights.Rights.ClaimStatusCodesDelete;
                    chkRights.ClaimStatusCodesSearch = vmRights.Rights.ClaimStatusCodesSearch;
                    chkRights.ClaimStatusCodesImport = vmRights.Rights.ClaimStatusCodesImport;
                    chkRights.ClaimStatusCodesExport = vmRights.Rights.ClaimStatusCodesExport;

                    //Adjustment Codes
                    chkRights.AdjustmentCodesCreate = vmRights.Rights.AdjustmentCodesCreate;
                    chkRights.AdjustmentCodesEdit = vmRights.Rights.AdjustmentCodesEdit;
                    chkRights.AdjustmentCodesDelete = vmRights.Rights.AdjustmentCodesDelete;
                    chkRights.AdjustmentCodesSearch = vmRights.Rights.AdjustmentCodesSearch;
                    chkRights.AdjustmentCodesImport = vmRights.Rights.AdjustmentCodesImport;
                    chkRights.AdjustmentCodesExport = vmRights.Rights.AdjustmentCodesExport;

                    //Remark Codes
                    chkRights.RemarkCodesCreate = vmRights.Rights.RemarkCodesCreate;
                    chkRights.RemarkCodesEdit = vmRights.Rights.RemarkCodesEdit;
                    chkRights.RemarkCodesDelete = vmRights.Rights.RemarkCodesDelete;
                    chkRights.RemarkCodesSearch = vmRights.Rights.RemarkCodesSearch;
                    chkRights.RemarkCodesImport = vmRights.Rights.RemarkCodesImport;
                    chkRights.RemarkCodesExport = vmRights.Rights.RemarkCodesExport;



                      //Team Rights
        chkRights.teamCreate = vmRights.Rights.teamCreate ;
        chkRights.teamupdate = vmRights.Rights.teamupdate ;
        chkRights.teamDelete = vmRights.Rights.teamDelete ;
        chkRights.teamSearch = vmRights.Rights.teamSearch ;
        chkRights.teamExport = vmRights.Rights.teamExport ;
        chkRights.teamImport = vmRights.Rights.teamImport ;

        //Receiver Rights
        chkRights.receiverCreate = vmRights.Rights.receiverCreate ;
        chkRights.receiverupdate = vmRights.Rights.receiverupdate ;
        chkRights.receiverDelete = vmRights.Rights.receiverDelete ;
        chkRights.receiverSearch = vmRights.Rights.receiverSearch ;
        chkRights.receiverExport = vmRights.Rights.receiverExport ;
        chkRights.receiverImport = vmRights.Rights.receiverImport ;

        //Submitter Rights
        chkRights.submitterCreate = vmRights.Rights.submitterCreate ;
        chkRights.submitterUpdate = vmRights.Rights.submitterUpdate ;
        chkRights.submitterDelete = vmRights.Rights.submitterDelete ;
        chkRights.submitterSearch = vmRights.Rights.submitterSearch ;
        chkRights.submitterExport = vmRights.Rights.submitterExport ;
        chkRights.submitterImport = vmRights.Rights.submitterImport ;




        //PatientPlan Rights
        chkRights.patientPlanCreate = vmRights.Rights.patientPlanCreate ;
        chkRights.patientPlanUpdate = vmRights.Rights.patientPlanUpdate ;
        chkRights.patientPlanDelete = vmRights.Rights.patientPlanDelete ;
        chkRights.patientPlanSearch = vmRights.Rights.patientPlanSearch ;
        chkRights.patientPlanExport = vmRights.Rights.patientPlanExport ;
        chkRights.patientPlanImport = vmRights.Rights.patientPlanImport ;
        chkRights.performEligibility = vmRights.Rights.performEligibility ;


        //PatientPayment Rights
        chkRights.patientPaymentCreate = vmRights.Rights.patientPaymentCreate ;
        chkRights.patientPaymentUpdate = vmRights.Rights.patientPaymentUpdate ;
        chkRights.patientPaymentDelete = vmRights.Rights.patientPaymentDelete ;
        chkRights.patientPaymentSearch = vmRights.Rights.patientPaymentSearch ;
        chkRights.patientPaymentExport = vmRights.Rights.patientPaymentExport ;
        chkRights.patientPaymentImport = vmRights.Rights.patientPaymentImport ;

        // Charges Rights
        chkRights.resubmitCharges = vmRights.Rights.resubmitCharges ;
        // BatchDocument Rights
        chkRights.batchdocumentCreate = vmRights.Rights.batchdocumentCreate ;
        chkRights.batchdocumentUpdate = vmRights.Rights.batchdocumentUpdate ;
        chkRights.batchdocumentDelete = vmRights.Rights.batchdocumentDelete ;
        chkRights.batchdocumentSearch = vmRights.Rights.batchdocumentSearch ;
        chkRights.batchdocumentExport = vmRights.Rights.batchdocumentExport ;
        chkRights.batchdocumentImport = vmRights.Rights.batchdocumentImport ;





        // ElectronicSubmission Rights
        chkRights.electronicsSubmissionSearch = vmRights.Rights.electronicsSubmissionSearch ;
        chkRights.electronicsSubmissionSubmit = vmRights.Rights.electronicsSubmissionSubmit ;
        chkRights.electronicsSubmissionResubmit = vmRights.Rights.electronicsSubmissionResubmit ;

        // PaperSubmission Rights
        chkRights.paperSubmissionSearch = vmRights.Rights.paperSubmissionSearch ;
        chkRights.paperSubmissionSubmit = vmRights.Rights.paperSubmissionSubmit ;
        chkRights.paperSubmissionResubmit = vmRights.Rights.paperSubmissionResubmit ;
        // SubmissionLog Rights	
        chkRights.submissionLogSearch = vmRights.Rights.submissionLogSearch ;
        // PlanFollowup Rights	
        chkRights.planFollowupSearch = vmRights.Rights.planFollowupSearch ;
        chkRights.planFollowupCreate = vmRights.Rights.planFollowupCreate ;
        chkRights.planFollowupDelete = vmRights.Rights.planFollowupDelete ;
        chkRights.planFollowupUpdate = vmRights.Rights.planFollowupUpdate ;
        chkRights.planFollowupImport = vmRights.Rights.planFollowupImport ;
        chkRights.planFollowupExport = vmRights.Rights.planFollowupExport ;

        // PatientFollowup Rights	
        chkRights.patientFollowupSearch = vmRights.Rights.patientFollowupSearch ;
        chkRights.patientFollowupCreate = vmRights.Rights.patientFollowupCreate ;
        chkRights.patientFollowupDelete = vmRights.Rights.patientFollowupDelete ;
        chkRights.patientFollowupUpdate = vmRights.Rights.patientFollowupUpdate ;
        chkRights.patientFollowupImport = vmRights.Rights.patientFollowupImport ;
        chkRights.patientFollowupExport = vmRights.Rights.patientFollowupExport ;

        // Group Rights	
        chkRights.groupSearch = vmRights.Rights.groupSearch ;
        chkRights.groupCreate = vmRights.Rights.groupCreate ;
        chkRights.groupUpdate = vmRights.Rights.groupUpdate ;
        chkRights.groupDelete = vmRights.Rights.groupDelete ;
        chkRights.groupExport = vmRights.Rights.groupExport ;
        chkRights.groupImport = vmRights.Rights.groupImport ;
        // Reason Rights	
        chkRights.reasonSearch = vmRights.Rights.reasonSearch ;
        chkRights.reasonCreate = vmRights.Rights.reasonCreate ;
        chkRights.reasonUpdate = vmRights.Rights.reasonUpdate ;
        chkRights.reasonDelete = vmRights.Rights.reasonDelete ;
        chkRights.reasonExport = vmRights.Rights.reasonExport ;
        chkRights.reasonImport = vmRights.Rights.reasonImport ;

         chkRights.addPaymentVisit = vmRights.Rights.addPaymentVisit;
         chkRights.DeleteCheck = vmRights.Rights.DeleteCheck;
         chkRights.ManualPosting = vmRights.Rights.ManualPosting;
         chkRights.Postcheck = vmRights.Rights.Postcheck;
         chkRights.PostExport = vmRights.Rights.PostExport;
         chkRights.PostImport = vmRights.Rights.PostImport;
         chkRights.ManualPostingAdd = vmRights.Rights.ManualPostingAdd;
         chkRights.ManualPostingUpdate = vmRights.Rights.ManualPostingUpdate;
         chkRights.PostCheckSearch = vmRights.Rights.PostCheckSearch;
         chkRights.DeletePaymentVisit = vmRights.Rights.DeletePaymentVisit;



                    chkRights.UpdatedBy = CurrentLoginEmail;

                    _context.Entry(chkRights).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok(vmRights);
                }
            }
            return BadRequest(false);

        }


        // DELETE: api/Rights/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Rights>> DeleteRights(string id)
        {
            var rights = await _context.Rights.FindAsync(id);
            if (rights == null)
            {
                return NotFound();
            }

            _context.Rights.Remove(rights);
            await _context.SaveChangesAsync();

            return rights;
        }

        private bool RightsExists(string id)
        {
            return _context.Rights.Any(e => e.Id == id);
        }

        // GET: api/Rights/5
          [Route("GetDefault/{Role}")]
          [HttpGet("{Role}")]
        public async Task<ActionResult<Rights>> GetDefault(string Role)
        {
            var checkrole = (from rol in _context.Roles
                             where rol.Name == Role
                             select rol
                             ).FirstOrDefault();
            if (checkrole == null)
                return BadRequest("No Role Found");

            Rights chkRights = new Rights();
            if (Role == "SuperAdmin")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;

            }
            else if (Role == "SuperUser")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "Manager")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "TeamLead")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "Biller")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;


                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "ClientManager")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "ClientUser")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "SupportAdmin")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;


            }
            else if (Role == "SupportEditor")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "SupportUser")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }

            return chkRights;
        }
        //[Route("FindAudit/{RightsID}")]
        //[HttpGet("{RightsID}")]
        //public List<RightsAudit> FindAudit(long RightsID)
        //{
        //    List<RightsAudit> data = (from pAudit in _context.RightsAudit
        //                              where pAudit.RightsID == RightsID
        //                              select new RightsAudit()
        //                              {
        //                                  ID = pAudit.ID,
        //                                  RightsID = pAudit.RightsID,
        //                                  TransactionID = pAudit.TransactionID,
        //                                  ColumnName = pAudit.ColumnName,
        //                                  CurrentValue = pAudit.CurrentValue,
        //                                  NewValue = pAudit.NewValue,
        //                                  CurrentValueID = pAudit.CurrentValueID,
        //                                  NewValueID = pAudit.NewValueID,
        //                                  HostName = pAudit.HostName,
        //                                  AddedBy = pAudit.AddedBy,
        //                                  AddedDate = pAudit.AddedDate,
        //                              }).ToList<RightsAudit>();
        //    return data;
        //}


    }
}
