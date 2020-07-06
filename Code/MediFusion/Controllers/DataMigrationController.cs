using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MediFusionPM.Models;
using MediFusionPM.Models.Main;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.TestModel;
using MediFusionPM.Uitilities;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static MediFusionPM.Controllers.ElectronicSubmissionController;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMExternalPatient;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class DataMigrationController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        public IConfiguration _config;

        public DataMigrationController(ClientDbContext context, MainContext contextMain, IConfiguration _config)
        {
            _context = context;
            _contextMain = contextMain;
            this._config = _config;
        }

        [HttpPost]
        [Route("AddPatientData2")]
        public async Task<ActionResult> AddPatientData2(VMDataMigration InputData)
        {
            #region large string
            string s = "Florida Pace Centers Inc|UCS: Benefit Plan Administrators of Eau Claire, Inc.|OMNI Administrators|Kentucky Health Administrators, Inc.|Longevity NY|Mutual of Omaha Medicare Advantage|OSF Healthcare Central|OSF Healthcare East|Provider Partners Health Plan of Illinois|TriWest Healthcare Alliance PC3 (VAPCCC Regions 1-6)|University of Maryland Health Advantage|VieCare Life Butler|1199 National Benefit Fund|21st Century Health and Benefits|3P Admin|4 YourChoice EPO/PPO Network|6 Degrees Health, Inc.|8th District Elec|A & I Benefit Plan Administrators|AAG - American Administrative Group|AAG Benefit Plan Administrators, Inc.|AARP Hospital Indemnity Plans insured by UnitedHealthcare Insurance Company|AARP Medicare Supplement Plans insured by UnitedHealthcare Insurance Company|AARP MedicareComplete insured through UnitedHealthcare (formerly AARP MedicareComplete from SecureHorizons)|AARP MedicareComplete insured through UnitedHealthcare / Oxford Medicare Network -  AARP MedicareComplete|AARP MedicareComplete insured through UnitedHealthcare / Oxford Mosaic Network - AARP MedicareComplete  Mosaic|ABRAZO Advantage Health Plan|Absolute Total Care - Medical|Access Administrators|Access Behavioral Care|Access Community Health Network|Access Integra|Access IPA|Access Medical Group|Access Primary Care Medical Group (APCMG)|Access Santa Monica|Acclaim|Acclaim IPA|Acclaim IPA (MHCAC)|Accountable Healthcare IPA (AHCIPA)|ACS Benefit Services, Inc.|ACTIN Care Groups / Clifton Health Systems|Active Care (UCS)|ActivHealthCare|Activia Benefit Services|Administration Systems Research Corp|Administration Systems Research Health Benefits|Administrative Concepts Inc.|Administrative Services Inc|AdminOne|Advanced Health CCO - Mental Health Claims Only (previously WOAH-Western Oregon Advanced Health CCO)|Advanced Medical Doctors of CA|Advanced Physicians IPA|Advanstaff, Inc.|Advantage Care IPA|Advantage Health Solutions|Advantage Health Solutions (CMCS) - Medicare Advantage|Advantage Medical Group|Advantage Preferred Plus|Advantek Benefit Administrators (SANTE)|Advantica Benefits|Adventist Health Plan|Adventist Health System West|Adventist White Memorial - Crown City Medical Group|Adventist White Memorial - Southland San Gabriel Valley|Advocate Health Centers|Advocate Health Partners|Aetna Advantage|Aetna Affordable Health Choices - SRC/Boone|Aetna Better Health - Parkland (TX)|Aetna Better Health of California|Aetna Better Health of Florida|Aetna Better Health of Illinois (Medicaid)|Aetna Better Health of Kansas|Aetna Better Health of Kentucky|Aetna Better Health of Louisiana|Aetna Better Health of Maryland|Aetna Better Health of Michigan|Aetna Better Health of New Jersey|Aetna Better Health of New York|Aetna Better Health of Ohio|Aetna Better Health of Pennsylvania (Medicaid)|Aetna Better Health of Texas|Aetna Better Health of Virginia|Aetna Better Health of Virginia - CCC Plus|Aetna Better Health of West Virginia|Aetna Choice Plans|Aetna Health, Inc.|Aetna Healthcare|Aetna HealthFund|Aetna Illinois Medicaid|Aetna Life & Casualty Company|Aetna Life Insurance Company|Aetna Medicaid Plans|Aetna Medicare|Aetna Senior Plans|Aetna Signature Administrators (ASA)|Aetna Student Health|Aetna TX Medicaid and CHIP|Aetna US Healthcare - AUSHC|Affiliated Doctor's of Orange County (ADOC) (Regal)|Affiliated Partners IPA|Affiliated Physicians Group (APG)|Affiliated Physicians IPA|Affinity Health (Medicare Advantage)|Affinity Health Plan|Affinity Health Plan Essential|Affinity Medical Group|Affordable Benefit Administrators, Inc.|AFTRA Health & Retirement Funds|Agency Services Inc.|AgeRight Advantage|AgeWell New York|Agua Caliente|Agua Dulce ISD|AHP Provider Network|AKM Medical Group|Alameda Alliance for Health|Alamitos IPA (AIPA)|Alaska Children''s Services, Inc. Group #P68|Alaska Electrical Health & Welfare Fund|Alaska Laborers Construction Industry Trust Group # F23|Alaska Pipe Trades Local 375 Group # F24|Alaska UFCW Health & Welfare Trust Group # F45|Albuquerque Public Schools|ALICARE|Aliera Healthcare|Aligned Community Physicians|Alignis|Alignment Health Care (Fee For Service)|Alignment Health Plan (Fee For Service)|All Care Medical Group|AllCare IPA|AllCare PEBB|Allegeant|Allegiance Benefit Plan Management Inc.|Allegiance Health Management|Alliance Behavioral Health|Alliance Coal Health Plan|Alliance Health Systems|Alliance Healthcare - Stones River Regional IPA|Alliance IPA|Alliance Physicians Medical Group|Alliant Health Plans of Georgia|Allied Benefit Systems, Inc.|Allied Healthcare|Allied Pacific of California|Allied Physicians Medical Group|Allina Health Aetna|Allways Health Partners|Aloha Care|Alpha Care Medical Group NMM|Alta Bates Medical Group|ALTA Health & Life Insurance|AltaMed|Alternative Opportunities|Altius Health Plan|Alvarado IPA|Amalgamated Insurance Company|Amalgamated Life|Amalgamated Life IPA Alicare|Ambetter of Arkansas - Medical|Ambetter of Missouri - Medical|Ambetter of New Hampshire - Medical|Ameriben Solutions, Inc.|Americaid Community Care|American Administrative Group|American Behavioral|American Family Health Providers|American Family Insurance|American Family PPO Policies|American General Life & Accident|American Healthcare Alliance|American Insurance Company of Texas|American Life Care|American National Insurance Co.|American Plan Administrators|American Postal Workers Union (APWU)|American Postal Workers Union (APWU)|American Primary Care Alliance IPA |American Public Life (APL)|American Republic Insurance Company (ARIC)|American Specialty Health Plan|American Therapy Association (ATA) - Florida|American Worker Health Plan|Americas 1st Choice Health Plans of South Carolina Inc.|America's Choice Healthplan|America's Health Choice|Americas Health Plan|Americas PPO|Amerigroup Community Care|Amerigroup Corporation|AmeriHealth Administrator|AmeriHealth Caritas / VIP Care|AmeriHealth Caritas / VIP Care Plus - Michigan|AmeriHealth Caritas Delaware|AmeriHealth Caritas District of Columbia|AmeriHealth Caritas Healthplan New Hampshire|AmeriHealth Caritas Louisiana|AmeriHealth Caritas NorthEast|AmeriHealth Delaware - HMO/POS|AmeriHealth Delaware (Non-HMO)|AmeriHealth Mercy Health Plan|AmeriHealth New Jersey - HMO/POS|AmeriHealth New Jersey (Non-HMO)|AmeriHealth Pennsylvania - HMO/POS/ERISA|Ameri-West Health Associates|Amfirst Insurance Company|Amida Care|AMPS|AMPS - CX|AMPS America|AMVI - Prospect Health Network|AMVI Medical Group|Anaheim Memorial IPA (MHCAN)|Anchor Benefit Consulting Inc.|Anchor Medical Group|Ancillary Benefit Systems - Arizona Foundation for Medical Care|Angel Medical Group|Angeles IPA|Antares Management Solutions|Antelope Valley Medical Associates (Regal)|Anthem Blue Cross CA Encounters|Anthem Blue Cross of California (BCCA)|Anthem Healthy Indiana Plan (HIP)|Anthem Hoosier HealthWise|Anthem Medicaid - Indiana|Anthem Medicaid - South Carolina|Anthem Medicaid - Wisconsin|Anthem NV Medicaid|Anthem OMHS|APAC (Pinnacle Health Resources)|Apex Benefit Services|Apostrophe|AppleCare Medical Management|ARAZ|Arbor Health Plan|ARC Administrators|Arcadia Methodist IPA|Arcadian Management Services|Arch Health Partners/Centre for Healthcare|Argus Vision|Arise Health Plan (WPS)|Arizona Complete Health (formerly Health Net Access)|Arizona Foundation for Medical Care|Arizona Priority Care Plus|Arkansas Superior Select|ARM, Ltd|Arnett Health Plan|Arnot Ogden Medical Center (UCS)|Arroyo Vista Family Health Center|Arta Health Network|Arta Western|Asian American Medical Group|Asian Community Medical Group, Inc.|Aspen Medical Associates|Aspire Health Plan|ASRM LLC|Associated Dignity Medical Group Professional Corp|Associated Hispanic Physicians of Southern California|Associated Hyspanic Physicians|Associates for Health Care, Inc. (AHC)|Assurant Health Self-Funded (Allied Benefit)|Assured Benefits Administrators|Asuris Northwest Health|ATENDATPA|Atlantic Medical|Atlantis Eyecare|Atrio Health Plans|AudioNet - National Elevator|Audionet - UAW Ford Retirees/GM Employees (AUDIOSVS)|Audionet - UAW GM retirees/Chrysler Emps & Retirees (AUDIOABS)|AudioNet America (AUDIOCS)|Aultra Administrative Group|Automated Benefit Services|Automated Group Administration Inc. (AGA)|Avalon Administrative Services - BCBSNC|Avalon Healthcare Solutions|Avalon IPA|Avante Health|Avera Health Plans|Avera Health Plans for John Morrell|Avesis Third Party Administrator|AvMed, Inc.|AXA Assistance_USA|Axminster Medical Group|BadgerCare Plus (CCHP)|Bakersfield Family Medical Center (BFMC) (Regal)|Bakery and Confectionery Union and Industry International Health|Bankers Life (ERA Only)|Banner Health and Aetna Health Insurance Company|Banner Health Company|Banner UFC ALTCS|Baptist Health South Florida|Basic Plus (TCC)|Bass Administrators, Inc.|Bay Bridge Administrators, LLC|Baycare Life Management|BayCare Select Health Plans Inc|BCBS Empire New York|BCBS Mountain States|BCBS of West Virginia|BCBS South Carolina (FEP - Federal Employee Plan)|BCBS South Carolina (Preferred Blue)|BCBS Western New York Medicaid/CHP|BCBSMN Blue Plus Medicaid|Beacon Health Options (Empire Plan Mental Health)|Beacon Health Options (formerly Value Options)|Beacon Health Options (General Motors)|Beacon Health Strategies|Behavioral Health Systems|Bella Vista Medical Group|BeneBay|Benefit & Risk Management Services (BRMS)|Benefit Administration Services LTD (BAS LTD)|Benefit Administrative Systems (BAS Health)|Benefit Coordinators Corporation|Benefit Management Admin (BMA)|Benefit Management Group-NV|Benefit Management Inc - Joplin Claims|Benefit Management Services|Benefit Management Services of Louisiana|Benefit Plan Administrators|Benefit Plan Administrators, Inc.|Benefit Plan Services|Benefit Systems & Services, Inc. (BSSI)|Benefits Administrative Solutions Inc|BeneSys Inc.|Benveo - First Health Network|Berkshire / Lehigh Partners (BVP)|Best Life & Health Insurance Company|Better Health|Better Health of Florida|Beverly Alianza IPA|Beverly Hospital - Imperial Health Holdings|Beverly Hospital BEVAHISP|BIENVIVIR|Bind (PreferredOne)|Block Vision - Eye Specialists|Block Vision - VIPA|Block Vision Inc.|Block Vision of Texas|Blue Benefit Administrators of MA|Blue Card Program|Blue Care Network Advantage of Michigan|Blue Care Network of Michigan|Blue Choice of South Carolina|Blue Choice of South Carolina (Medicaid)|Blue Cross (Cal-Optima - Medi-Cal)|Blue Cross Blue Shield Arkansas|Blue Cross Blue Shield of Alabama|Blue Cross Blue Shield of Arizona|Blue Cross Blue Shield of Arizona Advantage (formerly Banner Medisun)|Blue Cross Blue Shield of Colorado|Blue Cross Blue Shield of Connecticut|Blue Cross Blue Shield of DC (Carefirst)|Blue Cross Blue Shield of Delaware|Blue Cross Blue Shield of Florida|Blue Cross Blue Shield of Georgia (Anthem)|Blue Cross Blue Shield of Hawaii (HMSA)|Blue Cross Blue Shield of Illinois|Blue Cross Blue Shield of Indiana (Anthem)|Blue Cross Blue Shield of Iowa|Blue Cross Blue Shield of Kansas|Blue Cross Blue Shield of Kansas City|Blue Cross Blue Shield of Kentucky (Anthem)|Blue Cross Blue Shield of Louisiana|Blue Cross Blue Shield of Maine (Anthem)|Blue Cross Blue Shield of Maryland (CareFirst )|Blue Cross Blue Shield of Massachusetts|Blue Cross Blue Shield of Michigan|Blue Cross Blue Shield of Minnesota|Blue Cross Blue Shield of Mississippi|Blue Cross Blue Shield of Missouri (Anthem)|Blue Cross Blue Shield of Montana|Blue Cross Blue Shield of Nebraska|Blue Cross Blue Shield of Nevada (Anthem)|Blue Cross Blue Shield of New Hampshire (Anthem)|Blue Cross Blue Shield of New Mexico|Blue Cross Blue Shield of New York - Excellus (Central Region)|Blue Cross Blue Shield of New York - Excellus (Rochester Region)|Blue Cross Blue Shield of New York - Excellus (Utica Region)|Blue Cross Blue Shield of New York - Excellus (Utica Watertown Region)|Blue Cross Blue Shield of North Carolina|Blue Cross Blue Shield of North Dakota|Blue Cross Blue Shield of Ohio (Anthem)|Blue Cross Blue Shield of Oklahoma|Blue Cross Blue Shield of Oregon (Regence)|Blue Cross Blue Shield of Pennsylvania (Highmark)|Blue Cross Blue Shield of Rhode Island|Blue Cross Blue Shield of South Carolina|Blue Cross Blue Shield of South Carolina (State Health Plan)|Blue Cross Blue Shield of South Dakota|Blue Cross Blue Shield of Tennessee|Blue Cross Blue Shield of Texas|Blue Cross Blue Shield of Texas - Medicaid|Blue Cross Blue Shield of Utah (Regence)|Blue Cross Blue Shield of Vermont|Blue Cross Blue Shield of Virginia (Anthem)|Blue Cross Blue Shield of Western New York|Blue Cross Blue Shield of Western New York - Federal Employee Program (FEP)|Blue Cross Blue Shield of Wisconsin (Anthem)|Blue Cross Blue Shield of Wyoming|Blue Cross Community Centennial (BCBS NM)|Blue Cross Community ICP / FHP|Blue Cross Community MMAI|Blue Cross Community Options of Illinois|Blue Cross Complete of Michigan|Blue Cross Medicare Advantage PPO/HMO|Blue Cross of California|Blue Cross of Idaho|Blue Cross of Pennsylvania (Capital Blue / CAIC)|Blue Medicare of North Carolina|Blue Shield - California|Blue Shield - California / Blue Shield (FEP)|Blue Shield of California Promise Health Plan|Bluegrass Family Health|BlueShield of Northeastern New York|BMC HealthNet Plan|Boilermakers National Health & Welfare Fund|Boncura Health Solutions|BookMD Inc.|Boon Chapman Administrators, Inc.|Boston Medical Center Health Plan (BMCHP)|Boulder Administration Services|Brand New Day (FFS)|Brand New Day Encounters|BridgeSpan|Bridgeview Company|Bridgeway Health Solutions - Medical|Bright Health Medicare Advantage|Bright Health Plan|Brighten Health Plan Solutions|Bristol Park Medical Group|Brokerage Concepts|Brookshire IPA (BIPA)|Brown & Toland Health Services|Brown & Toland Medical Group|Brown & Toland Sutter Select|Buckeye Community Health (Ambetter) - Medical|Bureau For Children With Medical Handicaps|Butler Benefit|C & O Employees Hospital Association|C. L. Frates and Company|Caduceus Medical Group (Regal)|Cal Care IPA|Cal Care IPA Encounters|Cal Viva Health|Cal Water (California Water Service)|California Care (Humboldt Del Norte)|California Eye Care|California Health and Wellness|California Health and Wellness - Medical|California IPA (Capital MSO)|California Kids Care (CKC)|California Pacific Medical Center (CPMC)|California Pacific Physicians Medical Group, Inc.|California Rural Indian Health Board (CRIHB) Cares|California Rural Indian Health Board (CRIHB) Options|CalOptima Direct|Cameron Manufacturing & Design, Inc. (UCS)|Camp Lejeune Family Member Program|Cannon Cochran Management Services|Cap Management|Capital Blue Cross|Capital District Physicians Health Plan (CDPHP)|Capital Health Plan|Capitol Administrators|CapRock Health Plan - Verity|CapRock HealthPlans|Cardinal Innovations (Formerly Piedmont Behavioral Health)|Cardiovascular Care Providers|Care Advantage|Care Improvement Plus (CIP) / XL Health|Care Improvement Plus (CIP) / XL Health|Care Management Resources (CMR)|Care N' Care|Care Wisconsin Health Plan|Care1st Health Plan of Arizona|CareATC|CareCentrix|CareCore National|CareCore National LLC (Aetna Radiology)|CareCore/WCNY RAD|CareFirst BCBS NCA Region|CareFirst BCBS of Maryland|Carelink Health Plan|Caremark WPAS, Inc., Grp# P62|CareMore Encounters|CareMore Health Plan|CareMore Value Plus (CVP)|CareOregon Inc.|CarePartners|Careplus (Encounters)|Careplus Health Plan|Caresource Health Plan of Oregon|CareSource of Georgia|Caresource of Indiana|Caresource of Kentucky|CareSource of Ohio|Caresource West Virginia|Careworks|Cariten Healthcare|Cariten Senior Health|Carolina Behavioral Health Alliance|Carolina Care Plan Inc.|Carolina Complete|Carpenters Health & Welfare (FCHN)|Carpenters Trust of Western Washington|CASD/Sanford  (Chiropractic Associates of SD)|CBA BLUE|CBHNP - HealthChoices|CCEA Welfare Benefit Trust|CDO Technologies|Cedar Valley Community HealthCare (CVCH)|Cedars Towers Surgical Medical Group  (Encounters Only, Payer ID Required)|Cedars-Sinai Medical Group (CSHS) - Capitated/Encounters|Cedars-Sinai Medical Group (CSHS) - Fee for Service|Celtic Insurance|CeltiCare Health Plan (Ambetter) - Medical|Cement Masons & Plasterers Health & Welfare Trust Group #F16|CenCal Health|Cenpatico Behavioral Health – All States|Center Care PPO (Meritain)|Center for Elders Independence (CEI)|Center For Sight|Center IPA|CenterLight Healthcare|Centers Plan for Healthy Living|Centinela Valley IPA|Centinela Valley IPA Encounters|Central Benefits Life|Central Benefits Mutual|Central Benefits National|Central California Alliance for Health (CCAH)|Central Health Medicare Plan|Central Health MSO|Central Reserve Life|Central Reserve Life Insurance Company (Medicare Supplement)|Central States Health & Welfare Funds|Central Valley Health Plan|Central Valley Medical Group|Centurion|Centurypho|CHA Health|Champion Chevrolet|CHAMPVA - HAC|CHC Cares of South Carolina|CHEC - A Subsidiary of Sprint|Chesterfield Technologies|Childhealth Plus by Healthfirst (CHP)|Children First Medical Group|Children of Women Vietnam Veterans - VA HAC|Children’s Community Health Plan (CCHP)|Children’s Hospital Orange County (CHOC) Health Alliance|Childrens Medical Center Health Plan (CMCHP)|Children's Physicians Medical Group (CPMG)|Children's Specialists of San Diego II|Chinese Community Health Plan|ChiroCare|Chiropractic Associates of SD (CASD)|Chiropractic Care of Minnesota|Chiropractic Health Plans (CHP)|Chiropractic Health Plans (CHP) - Deseret Mutual Benefits Administration|Chiropractic Health Plans (CHP) - Educators Mutual Insurance|Chiropractic Health Plans (CHP) - Medicaid|Chiropractic Health Plans (CHP) - University of Utah Health Plans|CHOC Health Alliance|Choice Medical Group|Choice Physicians Net First Choice|Choice Physicians Network|ChoiceOne IPA|CHP 3|Christian Brothers Services|Christian Care Ministries (MediShare)|Christiana Care VBR|Christus Health Medicare Advantage|Christus Health New Mexico HIX|Christus Health Plan|CHRISTUS Health Plan Texas HIX|Christus Spohn Health Plan|CIGNA|CIGNA - PPO|CIGNA Behavioral Health|CIGNA For Seniors|CIGNA Health Plan|CIGNA HealthCare|CIGNA Healthcare for Seniors (Medicare)|Cigna HealthSpring|CIGNA PPA|CIGNA Premier Plus|CIGNA Private|Citizens Choice Health Plan (Fee For Service)|Citrus Health Plan|Citrus Valley IPA|Citrus Valley Physicians Group (CVPG)|City of Amarillo Group Health Plan|City of Austin|Clackamas County Mental Health|Clackamas MHO General Fund|Claimchoice Administrators (formerly AmeraPlan)|Claims Development Corporation|ClaimsBridge Custom Provider Network|Claimsware, Inc. DBA ManageMed|Clear One Health Plans|Client First|Clinica Medica San Miguel|Clinical Specialties, Inc|Clinicas del Camino Real|Clover Health (formerly CarePoint Health Plan)|CMS MMA Specialty Plan|Coachella Valley Physicians|Coastal Administrative Services|Coastal Communities Physician Network (CCPN) (Regal)|Coastal TPA|College Health IPA (CHIPA)|Colonial Medical|Colorado Access|Colorado Behavioral Healthcare Inc|Colorado Community Health Alliance|Colorado Health Insurance Cooperative Inc.|Colorado Kaiser Permanente|Columbine Health Plan|CoMed - CIGNA|Commerce Benefits Group|Common Ground Healthcare|Commonwealth Care Alliance|Commonwealth Health Alliance (CHA)|Community Care Alliance of IL|Community Care Associates|Community Care BHO|Community Care Inc. - Family Care (Wisconsin)|Community Care Inc. (Wisconsin)|Community Care IPA|Community Care Managed Health Care Plans of Oklahoma|Community Care Plan (Commercial)|Community Care Plan (Medicaid)|Community Family Care|Community Family Care Medical Group|Community First|Community Health Alliance|Community Health Alliance of Tennessee|Community Health Center Network - CHCN|Community Health Choice|Community Health Choice - Health Insurance Marketplace|Community Health Group of SD (CHGSD) - Capitated Claims|Community Health Group of SD (CHGSD) - FFS Claims|Community Health Plan of Washington (CHP of WA)|Community Life|Community Medical Group of the West Valley (Regal)|CommunityCare Oklahoma (CCOK)|Complementary Healthcare Plan (CHP)|Complete Care IPA|CompManagement Health|Comprehensive Care Management|Comprehensive Care Services|ComPsych|CompuSys Inc|CompuSys Inc|CompuSys Inc|Concordia Care, Inc|Confederation Administration Services & Life Insurance|Conifer Health Solutions|Connected Senior Care Advantage|Connecticare - Medicare Advantage (Emblem)|Connecticare (Commercial)|Connecticut Carpenters Health Fund|Connecticut General|Consociate Group|Consolidated Health Plans|Consumers Choice Health SC|Container Graphics Corporation|Contessa Health|Contessa Health - Security Health Plan|Continental Benefits|Continental General Insurance Company|Contractors, Laborers, Teamsters & Engineers (Local 14B)|Cook Childrens Health Plan|Cook Childrens Star Plan|Cook County Health - Behavioral|Cook County Health - Medical|Cook Group Health Plan|Cook Group Solutions|Cooperative Benefit Administrators (CBA)|CoOportunity Health|Co-ordinated Benefit Plans LLC|Coordinated Care - Medical|Coordinated Medical Specialists|COPC - Senior Care Advantage|Core Administrative Services|Core Benefits / Multimatic Tennesse, LLC|Core Management Resources Group|CoreSource - Internal|CoreSource - Ohio|CoreSource (AZ, MN)|CoreSource (MD, PA, IL, IA)|CoreSource (NC, IN)|CoreSource KC (FMH)|CoreSource Little Rock|Corizon Inc (Formerly Correctional Medical Services)|Cornerstone Benefit Administrators|Cornerstone Preferred Resources|Corporate Benefit Service (CBSA)|Corporate Benefits Service, Inc. (NC)|CorrectCare Integrated Health (CA Prison Health Care Services)|Correction Health Partners|Correctional Medical Services, Inc. (Now known as Corizon Inc)|County Care Health Plan|County Medical Services Program (CMSP)|County of Fresno|County of Kern POS|County of Sacramento - EMSF|County of Sacramento - Healthy Partners|Covenant Administrators Inc.|Covenant Management System Employee Benefit Plan|Coventry Health & Life|Coventry Health Care|Coventry Health Care of the Carolinas, Inc. (formerly WellPath)|Coventry Health Care of Virginia, Inc. (formerly Southern Health Services)|Coventry Healthcare of Florida (Aetna Medicaid)|Coventry One|CoventryCares (formerly OmniCare/CareNet/Carelink Medicaid)|Cox Health Plan|Create Healthcare|Creative Medical Systems|Crescent Health Solutions|Crown City Medical Group (CCMG)|Crown Life - Great West|Croy - Hall Management, Inc.|Crystal Run Health Plans|Crystal Run Health Plans|CSHP / Evan|CSI NETWORK SERVICES|CSI OF MICHIGAN|Custody Medical Services Program (CMSP)|Custom Benefit Administrators|Custom Design Benefits, Inc.|CWI Benefits, Inc.|CYPRESS BENEFIT ADMINISTRATORS|DakotaCare|DakotaCare Chiropractic|Dameron Hospital Association|Dart Management Corporation|Dart Member Care|Dean Health Plan|Delano Regional Medical Group (Managed Care Systems)|Dell Children's Health Plan|Delta Health Systems|Denver Health and Hospital Authority|Denver Health Medical Plan|Denver Health Medical Plan, In. - Medicare Choice|Department of Corrections - Oklahoma|Department of Social and Health Services (DSHS)|Department Rehabilitative Services - Oklahoma|Deseret Mutual Benefit Administrators|Desert Medical Group (Regal)|Desert Oasis Healthcare (DOHC) (Regal)|Desert Valley Medical Group|Design Benefit Administrators (Agua Caliente)|Detroit Medical Center|Devoted Health|DHR Provider Management|Diamond Bar Medical Group|Diamond Plan|Dignity Global|Dignity HCLA|Dignity Health - Mercy Medical Group / Woodland Clinic|Dignity Health - Sequoia Physicians Network|Dignity Health Medical Group - Inland Empire|Dignity Health MSO|Dimension Benefit Management|Direct Care Administrators|Director's Guild of America - Producer Health Plans|Diversified Administration Corporation|Diversified Group Administrators|Diversified Group Brokerage|DMERC Region A Medicare|DMERC Region B Medicare|DMERC Region C Medicare|DMERC Region D Medicare|Doctors Healthcare Plans|Downey Select IPA|Dreyer Health|Dreyer Medical Clinic|Driscoll Childrens Health Plan|Dunn & Associates Benefits Administrators, Inc|Dupage Medical Group|Early Intervention Central Billing|East Bay Drainage Drivers Security Fund|East Carolina Behavioral Health|Eastern Iowa Community Healthcare (EICH)|Eastland Medical Group IPA (Regal)|EastPointe Behavioral Health|Easy Care MSO LLC|Easy Choice Health Plan of California (Encounters) - ECHP|Easy Choice Health Plan of California (Fee for Service) - ECHP|EasyAccess Care IPA (ProCare MSO)|EBA &amp; M  Employee Benefits Administration and Management Corp|EBC (Buffalo Rock) Birmingham|Eberle Vivian|Ebix Health Administration Exchange (EHAE)|Edinger Medical Group|EHS Medical Group - Fresno|El Paso First - CHIP|El Proyecto De Barrio|ElderPlan, Inc.|Elderwood Health|Electrical Workers Health and Welfare Plan for Northern Nevada|Electrical Workers Insurance Fund Local 5800|Electronic Practice Solutions, Inc.|Element Care, Inc.|ElmCare LLC|Elmira Savings Bank (UCS)|Emanate Health Med Center AltaMed|Emanate Health Med Center NMM|Emanate Health Med Center PDT MSO|Emergency Medical Service Fund (EMSF)|EMHS Employee Health Plan|EMI Health|EMI-KP Ambulance Claims|Empire Healthcare IPA|Empire Physicians Medical Group (EPMG)|Empire Physicians Medical Group (EPMG)|Employee Benefit Concepts|Employee Benefit Corporation|Employee Benefit Logistics (EBL)|Employee Benefit Management Corp (EBMC)|Employee Benefit Management Services (EBMS)|Employee Benefit Management, Inc. (EBSI)|Employee Benefit Services  - Ft. Mill, South Carolina|Employee Benefit Services (EBS of San Antonio)|Employee Benefits administrators (EBA)|Employee Benefits Consultants|Employee Benefits Plan Administration (EBPA)|Employee Health Coalition|Employee Health Systems|Employee Plans, LLC|Employee Resource Management TPA - Kempton Group (UCS)|Employer Direct Healthcare|Employers Direct Health|Employer's Direct Health - Self|Employers Mutual Inc.|Employer's Mutual, Inc. (Jacksonville, FL)|Empower Arkansas|Encircle PPO|Encore Health Network|Enstar Natural Gas, Group # P61|Enterprise Group Planning|ENTRUST|Eon Health|EPN (Seton Health Plan)|Equicor|Equicor - PPO|Equitable Plan Services|Erin Group Administrators|Erisa Administrative Services|ES Beveridge & Associates|Esperanza IPA|Essence Healthcare|Essential Health Partners|Everence|Evergreen Health|Evergreen Health Cooperative|Evolutions Healthcare Systems|Exceedent LLC|Excel MSO|Exceptional Care Medical Group (ECMG)|Exclusicare|Exclusive Care - County of Riverside|Extended MLTC|Eyecare 2020|Eyenez Eye Institute|EyeQuest|F40 Alaska Carpenters Trust|FABOH (CHP RPU)|Facey Medical Foundation|Falling Colors (BHSD STAR)|Fallon Community Health|Fallon Health (Transplant Claims ONLY)|Fallon Total Care|Fallon Transplant|Family Care Medicaid|Family Care Specialists (FCS)|Family Choice Medical Group|Family Health Alliance|Family Health Network|Family Medical Network|Family Seniors Medical Group|FCE Benefit Administrators|Federal Employee Plan of South Carolina|Federal Employees Plan WA (Regence Blue Shield)|Federated Mutual Health Insurance Company|Fidelis Care New York|First Carolina Care|First Choice Health Administrators (FCHA)|First Choice Health Network (FCHN)|First Choice Medical Group|First Choice of Midwest PPO|First Great West Life & Annuity Ins. Co.|First Health - The Lewer Agency|First Health Network (f.k.a. CCN Managed Care Inc. & PPO Oklahoma)|First Priority Health (FPH) - Highmark|First Priority Life Insurance Company (FPLIC) - Highmark|First Source (Endeavor)|Firstcare (Medicaid)|Firstcare and Firstcare Medicaid|FirstNation Health|FirstSolutions|Fitzharris & Company, Inc.|Florida Blue HMO|Florida Community Care|Florida Health Solution HMO Company (Commercial ACA Metal Plans)|Florida Healthcare Plan|Florida Hospital Orlando VBR|Florida Hospital Waterman|Florida True Health|Flume Health, Inc|FMH Benefit Services, Inc.|Focus Plan NC|Foothill Eye Care Services|Foreign Service Benefit Plan|Forest County Potawatomi Insurance Department|Foundation for Medical Care of Tulare & Kings County|Foundation Health HMO|Fountain Valley IPA (FVIPA)|Fox Valley Medicine Site 199|Freedom Blue|Freedom Health Plan|Freedom Life Insurance Company|Fresenius Medical Care|Fringe Benefit Group Inc (Employer Plan Services, Inc. - Austin)|Fringe Benefit Group Inc (Employer Plan Services, Inc. - Houston)|Fringe Benefit Management (FBMC)|Fringe Benefits Coordinators|FrontPath Health Coalition|Galveston County Indigent Health Care|Garretson Resolution Group|Gateway Health Plan|Gateway Health Plan - Medicare Assured|Gateway Health Plan (Medicaid PA)|Gateway Health Plan Ohio - Medicare Assured|Gateway IPA (Pinnacle Health Resources)|GEHA – Aetna Signature Administrators|GEHA – ASA|GEHA Group Health (Nevada)|Geisinger Health Plans|GemCare Health Plan, Kern County EPO|GEMCare Medical Group (Managed Care Systems)|Genelco / Gencare Health|General Hospital (Humboldt Del Norte)|Generations Healthcare|Genesis Healthcare (EMG)|GI Innovative Management|GIC Indemnity Plan|Gilsbar|Glendale Adventist Medical Center|Glendale Memorial Group (Regal)|Glendale Physicians Alliance (Regal)|Global Care Inc.|Global Care Medical Group IPA|Global Excel Management|Global Health|Global One Ventures|GMR Healthcare|Gold Coast|Golden Physicians Medical Group|Golden State Medicare Health Plan|Golden Triangle Physician Alliance (GTPA)|Good Samaritan Hospital (GSH)|Good Samaritan Medical Practice Association (GSMPA)|Government Employees Hospital Association (GEHA)|Granite State - Medical|GRAVIE|Great Lakes PACE|Great Plains Medicare Advantage of Nebraska|Great Plains Medicare Advantage of North Dakota|Great Plains Medicare Advantage of South Dakota |Great States Health IICT|Great West Health Care|Greater Covina Medical Group (GCMG) (Regal)|Greater Dayton Senior Care Advantage|Greater L.A. Healthcare|Greater Newport Physicians|Greater Orange Medical Group|Greater Oregon Behavioral Health Inc|Greater San Gabriel Medical Group|Greater San Gabriel Valley Physicians|Greater Tri-Cities IPA|Greater Valley Medical Group|Great-West Life & Annuity Insurance Company|Greentree Administrators|Group & Pension Administrators|Group Administrators Ltd.|Group Benefit Services|Group Benefits - Louisiana|Group Health Cooperative (GHC)|Group Health Cooperative of Eau Claire|Group Health Cooperative of South Central Wisconsin (Claims)|Group Health Cooperative of South Central Wisconsin (Encounters)|Group Health Options (Pacific Northwest)|Group Health, Inc. (GHI HMO) (Emblem)|Group Health, Inc. (GHI PPO) (Emblem)|Group Insurance Service Center|Group Management Services, Inc. (GMS)|Group Practice Affiliates|Group Resources|GroupLink|Guardian Life (The Guardian)|Guildnet|Gulf Coast Physician Network (MedView)|Gulf Guaranty or MedPlus|Gulf Quest IPA HMO Blue|Gulf South Risk Services - CLHG Group (UCS)|Gulf Stream (CoreSource)|Gundersen Health Plan|H.E.R.E Health Trust|HAA Preferred Partners|Halcyon Behavioral Health|Hamaspik Choice|HAP Midwest Health Plans|HAP/AHL/Curanet|Harbor Health Plan (ProCare)|Harmony Health Plan|Harriman Jones Medical Group|Harrington Health - Kansas (Formerly Fiserv/Willis)|Harrington Health Non-EPO|Harvard Community Health Plan (AKA Harvard Pilgrim Stride)|Harvard Pilgrim Health Care|Harvard Pilgrim Passport Connect|Hawaii Management Alliance Association (HMAA)|Hawaii Medical Alliance Association (HMAA)|Hawaii Medical Assurance Association (HMAA)|Hawaii Western Management Group (HWMG)|HCC Life Insurance|HCH Administration - Illinois|HCH Administration, Inc.|HCS - Health Claims Service|HDPC - Premier Care (High Desert Primary Care Premier)|Health Alliance Medical Plan|Health Alliance Plan of Michigan|Health Assurance - Health America, Inc.|Health Care District Palm Beach|Health Care LA IPA (HCLA)|Health Choice Generations|Health Choice Integrated Care (HCIC)|Health Choice of Utah|Health Cost Solutions (HCS)|Health Design Plus - Hudson, Ohio|Health Economic Livelihood Partnership (HELP)|Health EOS|Health Excel IPA|Health EZ|Health First Colorado (Medicaid)|Health First Health Plans|Health Insurance Plan (HIP) of Greater New York (Emblem)|Health Net - California|Health Net - Medicare Advantage (MA) / Individual Family Plan (IFP)|Health Net - Missouri|Health Net - Oregon|Health Net - Washington|Health Net of Arizona|Health Net of the Northeast|Health Network One|Health Network Solutions (HNS) - Absolute Total Care SC|Health Network Solutions (HNS) - BCBS NC|Health Network Solutions (HNS) - CIGNA NC|Health Network Solutions (HNS) - CIGNA SC|Health Network Solutions (HNS) - HealthSpring NC|Health Network Solutions (HNS) - HealthSpring SC|Health Network Solutions (HNS) - Liberty Advantage|Health Network Solutions (HNS) - MedCost NC|Health Network Solutions (HNS) - MedCost SC|Health Network Solutions (HNS) - Select Health SC|Health Network Solutions (HNS) HealthTeam Advantage NC|Health New England|Health Now New York|Health Options (Florida Blue)|Health Partners of Minnesota|Health Partners of Tennessee|Health Partners Plans (Philadelphia, PA)|Health Payment Systems, Inc.|Health Plan of Michigan|Health Plan of Nevada|Health Plan of Nevada - Encounters|Health Plan of San Joaquin|Health Plan of San Mateo|Health Plan Services|Health Plans, Inc.|Health Plus PHSP|Health Plus Physician Organization (HPPO)|Health Services for Children with Special Needs (HSCSN)|Health Services Management – HSM|Health Share CCO Clackamas County|Health Source MSO|Health Special Risk, Inc.|Health Systems International - ECOH|Health Texas Medical Group|Health Tradition Health Plan|Health York Network|Healthcare Group, The|Healthcare Highways|Healthcare Management Administrators (HMA)|Healthcare Options|HealthCare Partners (HCP) - A Davita Medical Group|HealthCare Partners IPA - New York|HealthCare Partners Medical Group - California|HealthCare Partners of Nevada|HealthCare Resources NW|HealthCare Solutions Group|HealthCare USA|HealthCare's Finest Network (HFN)|HealthChoice of Arizona|HealthChoice Oklahoma|HealthComp|HealthEdge Administrators|HealthEQ Westlake Medical Center|Healthfirst - New York|HealthFirst (HMO)|Healthfirst 65 Plus|Healthfirst Family Health Plus (FHP)|Healthfirst Health Plan of New Jersey|Healthfirst Inc|Healthfirst PHSP|HealthFirst TPA|Healthfirst Tyler, Texas|Healthgram|HealthGroup Limited|Healthlink HMO|Healthlink PPO|HealthPlan Services|HealthPlus of Louisiana|HealthPlus of Michigan|HealthSCOPE Benefits, Inc.|HealthShare of Oregon CCO - Multnomah (Formerly known as Verity)|HealthSmart Accel|HealthSmart Benefit Solutions (formerly American Administrative/Gallagher Benefit)|HealthSmart Benefit Solutions (formerly JSL Administrators)|HealthSmart Benefit Solutions (Formerly Pittman and Associates)|HealthSmart Benefit Solutions (formerly Wells Fargo TPA/Acordia National)|HealthSmart MSO|HealthSmart Preferred Care|Healthsource (CIGNA)|Healthsource CMHC|HealthSpan Inc|HealthSpring (formerly Bravo Health/Elder Health)|HealthSpring HMO|HealthSpring Medicare + Choice|HealthSpring of Alabama|Healthsun Health Plan|HealthTeam Advantage|Healthways WholeHealth Network|Healthy Blue LA|Healthy CT|Healthy Equation|Healthy Montana Kids (HMK)|Healthy Palm Beaches, Inc.|Healthy San Francisco|Hemet Community Medical Group (HCMG)|Hennepin Health (formerly Metropolitan Health Plan)|Heritage Consultants|Heritage IPA|Heritage Physicians Network (HPN)|Heritage Provider Network (Regal)|Heritage Victor Valley Medical Group (Regal)|Hewitt Coleman|High Desert Medical Group (Regal)|Highline MSO - Molina|Highmark Health Options|Highmark Senior - Freedom Blue PPO (Pennsylvania)|Highmark Senior Health Company (Pennsylvania)|Highmark Senior Solutions Company - West Virginia|Hill Physicians Blue Cross PPO|Hill Physicians Blue Shield PPO|Hill Physicians Health Net PPO|Hill Physicians Medical Group (HPMG)|Hill Physicians United Healthcare PPO|Hill UHCSR PPO|Hilliard Corporation (UCS)|Hinsdale Medicare Advantage|Hinsdale Physicians Healthcare|Hispanic Physician IPA (Encounters Only)|Hispanic Physicians IPA (Fee For Service Only)|HMA Hawaii|HMC HealthWorks|HMO Louisiana Blue Advantage|HMSA Quest|HN1 Therapy Network|Hoag Affiliated Physicians|Hoag Clinic|Hockenberg Equipment|Holista, LLC|Hollywood Pres GLOBAL|Hollywood Presbyterian Medical Center - Alta Med Clinics (HPMC - AMC)|Hollywood Presbyterian Medical Center - Preferred (HPMC - PMG)|Hollywood Presbyterian Medical Center - San Judas IPA (HPMC - SJM)|Hollywood Presbyterian Medical Center - St. Vincent's IPA (HPMC - SVI)|Hollywood Presbyterian Medical Group|Hollywood Presbyterian San Judas|Home State - Medical|Homeland Security (AKA ICE Health)|HomeLink|Homelink HealthPartners|Hometown Health Plan Nevada|Homewood Resorts|Horizon BCBS of New Jersey|Horizon NJ Health|Horizon Valley Medical Group|Hotel Employees & Restaurant Employees Health Trust, Group # F19|HSA Health Insurance Company|HSM - Health Services Management|Hudson Health Plan (now MVP)|Human Behavior Institute|Humana|Humana Behavioral Health (formerly LifeSynch)|Humana Encounters|Humana Gold Choice|Humana Gold Plus|Humana Health Plans of Ohio|Humana Long Term Care (LTC)|Humana LTSS Claims|Humana Ohio 2|Humana Puerto Rico|Humboldt Del Norte Foundation IPA|Huntington Park Mission Medical Group|Hylton Payroll (Benefit Plan Administrators)|I. E. Shaffer|IAC Life|IBC Personal Choice|IBEW Local 6  - San Francisco Electrical Workers|IBEW Local 640 & AZ Chapter NECA Health & Welfare Trust|IBEW: Local No. 1 Health and Welfare Fund (Mental Health)|IBG Administrators, LLC|iCARE (Independent Care Health Plan)|ICare Health Options|iCircle of New York|IHC Health Solutions|IHG Direct|IlliniCare Health Plan - Medical|Illinois Health Plans (IHP)|ILWU Local 21 (FCHN)|IMA (Cotiva)|IMA, Inc|IMCare|Imperial Health Holdings Medical Group|Imperial Health Plan of California Encounters|Imperial Health Plan of California, Inc.|Imperial Insurance Company of Texas|IMS - Trial Card (TC-IMS)|IMS TPA|IMS TPA: Parton Lumber (UCS)|IMS TPA: The Spencer Group|IMS TPA: Walker White, Inc. (UCS)|INDECS Corporation|Independence Administrators|Independence Blue Cross|Independence Care Systems / FIDA-MMP|Independence Medical Group - Kern County|Independence Medical Group - Kern County|Independence Medical Group - Tulare County|Independence Medical Group - Tulare County |Independent Health|Independent Living System|Indian Health Services / Veteran Affairs|Indiana ProHealth Network|Indiana Teamsters Health Benefits Fund|Indiana University (IU) Health Plan – Commercial/Marketplace|Indiana University (IU) Health Plan – Medicare Advantage|Individual Health Insurance Companies|iNetico Inc|Infinity IPA|Inland Empire Electrical Trust (FCHN)|Inland Empire Health Plan|Inland Faculty Medical Group (MV Medical)|Inland Faculty Medical Group Encounters (MV Medical)|Inland Health Organization (IHO)|Inland Valley - (Redlands IPA)|InnovAge|Innovante Benefit Administrator|Innovation Health (Aetna)|INS Health Services (AKA ICE Health)|Insurance Administrator of America, Inc. (IAA)|Insurance Design Administrators|Insurance Management Services|Insurance Program Managers Group (IPMG)|Insurance Systems|Insurance TPA|Insurers Administrative Corp.|Integra Administrative Group|Integra BMS|Integra Group|Integra Managed Long Term Care|Integral Quality Care|Integrated Eye Care Network|Integrated Health Partners (IHP)|Integrated Medical Solutions|Inter Valley Health Plan (IVHP)|Interactive Medical Systems|Inter-Americas Insurance|InterCommunity Health CCO (IHN)|Intercommunity Health Net|Intergroup Services Corporation|Intermountain Ironworkers Trust Fund|International Benefit Administrator|International Brotherhood of Boilermakers|International Educational Exchange Service (IEES)|International Medica Group Inc|International Union of Operating Engineers ~ Local 15, 15A, 15C & 15D|Intervalley|InterWest Health PPO|INTotal Health, LLC|Iowa Total Care|Itasca Medical Center|Jai Medical Systems|Jencare Medical|JLS Family Enterprises (dba League Medical Concepts)|John Muir Physician Network|John Muir Trauma Physicians|John P. Pearl & Associates, Ltd.|Johns Hopkins Health Advantage|Johns Hopkins Healthcare|Johns Hopkins Healthcare (USFHP)|Johns Hopkins Home Care Group|JP Farley Corporation|Kaiser CSI - California Select for Individuals|Kaiser Foundation Health Plan of Colorado|Kaiser Foundation Health Plan of Georgia|Kaiser Foundation Health Plan of Northern CA Region|Kaiser Foundation Health Plan of Southern CA Region|Kaiser Foundation Health Plan of the Mid-Atlantic States|Kaiser Foundation Health Plan of the Northwest|Kaiser Foundation Health Plan of Washington|Kaiser Permanente Health Plan of Hawaii|Kaiser Permanente Ins. Co. (KPIC)|Kaiser Self-Funded|Kalos Health|Kane County IPA|Kansas Health Advantage (aka Kansas Superior Select)|Katy Medical Group|Kaweah Delta HC District Emp Plan|Kaweah Delta Medicare Advantage|Keenan & Associates|Kelsey Seybold|Kempton Company|Kempton Group Administrators|Kempton Group TPA: Kempton Group Administrators (UCS)|Kentucky Health Cooperative|Kentucky Spirit Health - Medical|Kern Health Care Network|Kern Health Systems|Kern Legacy Health Plan|Key Benefit Administrators|Key Health Medical Solutions, Inc.|Key Medical Group|Key Medical Group - Medicare Advantage|Key Select|Keystone Connect (AmeriHealth)|Keystone First Community Health Choices|Keystone First VIP Choice|Keystone Health (Lakeside - Glendale, CA) (Regal)|Keystone Health Plan Central|Keystone Health Plan East (KHP)|Keystone Mercy Health Plan|Keystone West (Highmark)|KG Administrative Services|Klais & Company|Klais & Company (Repricing for HealthSpan Network Only)|KM Strategic Management (KMSM)|Korean American Medical Group|Kova Healthcare, Inc.|KPIC Self-Funded Claims Administrator|KPS Healthplans|Kuspuk School District|LA Care Health Plan|LACH HealthNet by MedPOINT|LADOC CorrectCare|Lafayette Consolidated Government|Lake Region Community Health Plan|Lakes Area Community Health Plan (LACH)|Lakeside Community Healthcare (Regal)|Lakeside Medical Group (Regal Lakeside)|Lakewood Health Plan (LHP)|Lakewood IPA (LIPA)|Lancaster General Health (LGH)|Land of Lincoln Health|Landmark|Larson Group|LaSalle Medical Associates|Lassen Municipal Utility District|Lasso Healthcare Msa|Laundry Workers|Lawndale Christian Health Center|LBA Health Plans|League Medical Concepts (aka JLS Family Enterprises)|Leon Medical Center Health Plan|LHP Claims Unit|LHS/ MedCost Solutions LLC|Liberty Advantage|Liberty Health Advantage|Liberty Union|Life Assurance Company|Life St. Mary (Trinity Health Pace)|Life Trac|LifeCare Assurance Co|Lifetime Benefit Solutions|LifeWell Health Plans|LifeWise Healthplan of Oregon (Premera)|LifeWise Healthplan of Washington (Premera)|LifeWorks Advantage (ISNP plan)|Lighthouse Health Plan|Linden Oaks Behavioral Health|Little Company of Mary|Local 135 Health Benefits|Local 137 Operating Engineers Welfare Fund|Local 670 Engineers|Local 670 VIP|Lockard & Williams|Loma Linda University|Loma Linda University Adventist Health Sciences Center|Loma Linda University Adventist Health Sciences Centers|Loma Linda University Behavorial Medicine Center Employee Health Plan|Loma Linda University Dept of Risk Management|Loma Linda University Healthcare - ManagedCare Claims|Loma Linda University Healthcare Employee Health Plan|Lone Star TPA|Long Beach Memorial IPA (MHCLB)|Longevity IL|Loomis - SeeChange Health Insurance|Loren Cook|Los Angeles Medical Center (LAMC)|Los Angeles Medical Center (LAMC) - Encounters|Louisiana Healthcare Connections - Medical|Lovelace Salud  (Medicaid)|Lower Kuskokwim School District|Loyal American Life (Medicare Supplement)|Mabuhay Medical Group|MacNeal Health Providers - CHS|Magan Medical Clinic|Magellan Behavioral Health - Case Rate|Magellan Behavioral Health Services|Magellan Behavioral Health Services (Medicaid NE)|Magellan Behavioral Health Services (Medicaid VA)|Magellan Complete Care of Virginia (MCC of VA)|Magellan Complete Health of Virginia|Magellan HIA-CA|MagnaCare|MagnaCare - Oscar Network|Magnolia Health Plan (Ambetter) - Medical|Mail Handlers Benefit Plan (CAC)|Maine Community Health Options|Managed Care Services|Managed Care Systems|Managed Care Systems CDCR|Managed Health Network (MHN)|Managed Health Services Indiana (Ambetter) - Medical|Managed Health Services Wisconsin (Ambetter) - Medical|Managed Healthcare Associates|March Vision Care|Maricopa Care Advantage|Maricopa Health Plan (UAHP)|Marion Health Services - CHW|Marrick Medical Finance, LLC|Martins Point Health Care|Maryland Physicians Care|Maryland’s Medicaid Public Behavioral Health System|Mashantucket Pequot Tribal Nation|Masonry Industry Trust|Masonry Welfare Trust Fund|Masters Mates and Pilots Plan|Maverick Medical Group|Maxor Administrative Services|Mayo Clinic Health Solutions|Mayo Management Services, Inc. (MMSI)|MBA Benefit Administrators|MBA of Wyoming|MBS (Formerly MedCost Benefit Services)|MCA Administrators, Inc.|McCreary Corporation|McGregor PACE|McLaren Advantage SNP|McLaren Health Advantage|McLaren Health Plan (MHP) Commercial|McLaren Health Plan (MHP) Medicaid|MD Anderson Physician Network|MDwise Cooperative Managed Care Services (CMCS)|MDwise Franciscan - Hoosier Healthwise|MDwise Health Indiana Plan (DOS on or After 1/1/2019)|MDwise Hoosier Care Connect|MDwise Hoosier Healthwise (For 2019 claim submissions)|MDwise Hoosier Healthwise (HHW)|MDwise Select Health Network - Hoosier Healthwise|MDwise St. Anthony - Hoosier Healthwise|MDwise St. Catherine - Hoosier Healthwise|MDwise St. Margaret - Hoosier Healthwise|MDwise St. Vincent - Hoosier Healthwise|MDX Hawaii|MED3000 CMS Safety Net|MED3000 CMS Title 19 Reform|MED3000 CMS Title21|MED3000 Pedicare Title 19|MED3000 Pedicare Title 21|MedAdmin Solutions|MedAdmin Solutions (Self funded plans)|MedBen|MedCom|MedCore|MedCore HP - Central Health Plan |Medcore HP - Omni IPA|MedCore HP - Vitality Health Plan of California|MedCost, Inc.|MedDirect|Medequip Inc|Medex Health Network IPA (ProCare MSO)|Medfocus|Medica|Medica Health Plan Solutions|Medica HealthCare Plan of Florida|Medica2|Medicaid Alabama|Medicaid Alaska|Medicaid Arizona|Medicaid Arkansas|Medicaid Colorado|Medicaid Colorado (Behavioral Health)|Medicaid Connecticut|Medicaid Delaware|Medicaid District of Columbia|Medicaid Florida|Medicaid Georgia|Medicaid Hawaii|Medicaid Idaho|Medicaid Illinois|Medicaid Indiana|Medicaid Iowa|Medicaid Kansas|Medicaid Kentucky|Medicaid Louisiana|Medicaid Louisiana Ambulance Only (PAYER ID REQD)|Medicaid Louisiana DME Only (PAYER ID REQD)|Medicaid Maine|Medicaid Maryland|Medicaid Maryland MHS (PMHS)|Medicaid Massachusetts|Medicaid Massachusetts (Behavioral Health)|Medicaid Michigan|Medicaid Minnesota|Medicaid Mississippi|Medicaid Missouri|Medicaid Montana|Medicaid Nebraska|Medicaid Nevada|Medicaid New Hampshire|Medicaid New Jersey|Medicaid New Mexico|Medicaid New Mexico - Presbyterian Salud|Medicaid New York|Medicaid North Carolina|Medicaid North Dakota|Medicaid Ohio|Medicaid Oklahoma|Medicaid Oregon|Medicaid Pennsylvania|Medicaid Pennsylvania Behavioral Health (Beacon Health Options)|Medicaid Rhode Island|Medicaid South Carolina|Medicaid South Carolina (Blue Choice)|Medicaid South Dakota|Medicaid Texas|Medicaid Texas (Premier Plan)|Medicaid Utah|Medicaid Utah Crossovers|Medicaid Vermont|Medicaid Virginia|Medicaid Washington|Medicaid Washington DC|Medicaid West Virginia|Medicaid Wisconsin|Medicaid Wyoming|Medi-Cal (California Medicaid)|Medical Associates Health Plan|Medical Benefits Administrators, Inc.|Medical Benefits Companies|Medical Benefits Mutual|Medical Benefits Mutual Life Insurance Co.|Medical Mutual of Ohio|Medical Resource Network (MRN)|Medical Safety Net|Medical Service Company (MCS)|Medical Specialties Managers Inc|Medical Value Plan-MVP-OH|Medicare Alabama|Medicare Alaska|Medicare and Much More Florida|Medicare Arizona|Medicare Arkansas|Medicare California (North)|Medicare California (South)|Medicare Colorado|Medicare Connecticut|Medicare Delaware|Medicare District of Columbia|Medicare Florida|Medicare Georgia|Medicare Hawaii|Medicare Idaho|Medicare Illinois|Medicare Indiana|Medicare Iowa|Medicare Kansas|Medicare Kansas City|Medicare Kentucky|Medicare Louisiana|Medicare Maine|Medicare Maryland|Medicare Massachusetts|Medicare Michigan|Medicare Minnesota|Medicare Mississippi|Medicare Missouri (East & West)|Medicare Montana|Medicare Nebraska|Medicare Nevada|Medicare New Hampshire|Medicare New Jersey|Medicare New Mexico|Medicare New York (Downstate)|Medicare New York (Empire)|Medicare New York (Queens)|Medicare New York (Upstate)|Medicare North Carolina|Medicare North Dakota|Medicare Northern Virginia|Medicare Ohio|Medicare Oklahoma|Medicare Oregon|Medicare Pennsylvania|Medicare Railroad|Medicare Rhode Island|Medicare South Carolina|Medicare South Dakota|Medicare Tennessee|Medicare Texas|Medicare Utah|Medicare Vermont|Medicare Virgin Islands|Medicare Virginia (Palmetto)|Medicare Washington|Medicare Washington DC|Medicare West Virginia|Medicare Wisconsin|Medicare Wyoming|MediChoice IPA|Medicina Familiar Medical Group|Medico Insurance|Medigold|Medigold PPO|MediShare (Christian Care Ministries)|Mediview Sendero CHIP & STAR|Mediview Vista 360 Health|MedLogix MSO - All United Medical Group, Inc.|MedLogix MSO - Allied Health Solutions|MedLogix MSO - Americo IPA|Medova Healthhcare|MedPartners Administrative Services|MedSave USA Third Party Administration|MedSolutions, Inc|MedStar Family Choice of DC|MedStar Family Choice of Maryland|Medstar Select|MedView (via SmartData)|MedXoom|Mega Life & Health Insurance - OKC|Memorial Clinical Associates (MCA)|Memorial Healthcare IPA|Memorial Hermann Health Network (MHHN)|Memorial Integrated Healthcare (Commercial)|Memorial Integrated Healthcare (Medicaid)|Memorial Medical Center - Non Sutter|Memorial Medical Group|MemorialCare Medical Foundation|MemorialCare Medical Foundation Cap Services|MemorialCare Medical Foundation UCI|Menifee Valley Community Medical Group|Mental Health Consultants Inc.|Mental Health Network (MHNet)|Mercy Accountable Care Plan|Mercy Benefit Administration|Mercy Care Health Plans – Wisconsin & Illinois |Mercy Care Plan (AHCCCS)|Mercy Care RBHA|Mercy Health Plan|Mercy Managed Behavioral Health|Mercy Physicians Medical Group (MPMG)|Mercy Provider Network|Meridian Health Plan|Meridian Health Plan of Iowa|Meridian Health Plan of Michigan|Merit IPA|Meritage Medical Network|Meritain Health|Meritain Health|Meritain Health|Meritain Health (formerly Weyco, Inc)|Metal Culverts|Metcare Health Plans|Methodist First Choice|Metro Plus Health Plan|Metropolitan Health Plan (now Hennepin Health)|Miami Children's Health Plan|Michigan Fidelis Secure Care - Medical|Michigan Medicaid BCCCNP|Mid Cities IPA|Mid Rogue IPA|Mid Rogue Oregon Health Plan|Mid-America Associates Inc.|Mid-American Benefits|Mid-American TPA: AAI, Inc. (UCS)|Mid-County Physicians Medical Group|Midlands Choice, Inc.|Mid-Valley Behavioral Health Network|Midwest Group Benefits|Midwest Health Plans, Inc.|Midwest Operating Engineers Welfare Fund|MidWest Physician Administrative Services (MPAS)|Mills Peninsula Health Services (SPS - Sutter)|Mills Peninsula Medical Group (SPS - Sutter)|Minnesota Department of Health|Minnesota Health Care Program|Mission Community IPA|Mission Heritage Medical Group|Mission Hospital Affiliated Physicians|Mississippi Public Entity  Employee Benefit Trust (MPEEBT)|Mississippi Select Health Care|Missoula County Medical Benefits Plan|Missouri Care (MC)|Missouri Medicare Select|MMM of Florida|Moda Health (Formerly ODS Health Plan)|Molina Healthcare of CA Encounters|Molina Healthcare of California|Molina Healthcare of Florida|Molina Healthcare of Idaho|Molina Healthcare of Illinois|Molina Healthcare of Michigan|Molina Healthcare of Mississippi|Molina Healthcare of New Mexico|Molina Healthcare of New Mexico - Salud|Molina Healthcare of New York (formerly Total Care)|Molina Healthcare of Ohio|Molina Healthcare of Puerto Rico|Molina Healthcare of South Carolina|Molina Healthcare of Texas|Molina Healthcare of Utah (aka American Family Care)|Molina Healthcare of Virginia|Molina Healthcare of WA Encounters|Molina Healthcare of Washington|Molina Healthcare of Wisconsin|Molina Medicaid Solutions - Idaho|Molina Medicaid Solutions - Louisiana|Monarch Healthcare IPA|Mondial Assistance|Montana Health Co-op|Montefiore Contract Management Organization (CMO)|Monumental Life Insurance Company (MLIC)|Morris Associates|Motion Picture Industry Health Plan|Mountain States Administrators|MPLAN, Inc. - HealthCare Group|MPM Prospect|MRIPA - AllCare Health Plan|MSA Care Guard|Mt. Carmel Health Plan|Multicultural Primary Care Medical Group|MultiPlan GEHA|Multiplan Wisconsin Preferred Network|Multnomah County Other|Multnomah Treatment Fund (Formerly known as Verity Plus)|Municipal Health Benefit Fund|Mutual Assurance|Mutual Health Services|Mutual of Omaha|Mutually Preferred|MVP - Ohio|MVP Health Care (Mohawk Valley)|My Family Medical Group|MyChoice IPA (ProCare MSO)|myNEXUS Anthem|MyNexus, Inc.|N.W. Ironworkers Health & Security Trust Fund, Group # F15|N.W. Roofers & Employers Health & Security Trust Fund, Group# F26|N.W. Textile Processors Group # F14|Naphcare Inc|National Accident & Health General Agency Inc (NAHGA)|National Association of Letter Carriers (NALC)|National Capital Preferred Provider Organization (NCPPO)|National Financial Insurance Company|National Foundation Life Insurance Company|National Rural Electric Cooperative Assoc.|National Rural Letter Carrier Assoc.|National Telecommunications Cooperative Association (NTCA - Staff)|National Telecommunications Cooperative Association (NTCA)|Native Care Health|NCA Medical Group|NCAS|NCAS - Fairfax, Virginia|Nebraska Plastics Group|Nebraska Total Care - Medical|Neighborborhood Health Partnership (NHP)|Neighborhood Health Partnership|Neighborhood Health Plan|Neighborhood Health Plan (NHPRI)|Neighborhood Health Plan RI - Exchange Unity Integrity|Neighborhood Health Providers &  Suffolk Health Plan (NHP - SHP)|Netcare Life & Health Insurance - Hagatna, Guam|Network Health Insurance (NHIC) Medicare|Network Health of WI - Commercial|Network Health of WI - Medicare|Network Health Plan of Wisconsin - Commercial|Network Medical Management|Network Solutions IPA|New Avenues|New Century Health - Arizona Integrated Physicians|New Century Health - Arizona Integrated Physicians|New Century Health – CAC Centers|New Century Health - CarePlus Cardiology|New Century Health - CarePlus Urology|New Century Health - CarePlus Urology Lab|New Century Health – Cigna Medical Group AZ|New Century Health - Devoted Health Cardiology|New Century Health - Devoted Health Oncology|New Century Health - Humana Oncology|New Century Health - Humana Radiation Oncology|New Century Health - Simply Health Care Cardiology|New Century Health - Simply Health Care Oncology|New Century Health - Simply Health Care Radiation Oncology|New Century Health - Vista Cardiology - Summit|New Century Health Solutions - CarePlus Oncology|New Century Health Solutions – CarePlus Radiation Oncology|New Century Health Solutions - Vista Oncology|New Century Health Solutions – Vista Radiation Oncology|New Directions Behavioral Health (NDBH)|New England, The|New Era Employee Welfare Benefit|New Era Life - Employee Benefit Plans|New Era Life Insurance Company|New Hampshire Healthy Families - Medical|New Life Medical Group, Inc.|New Mexico District Council of Carpenters|New Mexico Health Connections|New Mexico Painters & Allied Trades Local #823|New Mexico West Texas Multi-Craft|New York Network Management|Next Level Health Partners|Nexus Health Medical Group|Nexus IPA (ProCare MSO)|NGS American|NHC Advantage|Nippon Life Insurance Company of America|Nivano Physicians IPA|NJ Carpenters Health Fund|Noble AMA Select IPA|Noble Community Medical Associates LA|Noble Mid OC Orange County  (Health Smart)|North America Administrators (NAA) - Nashville, Tennessee|North American Administrators|North American Medical Management (NAMM)|North Broward Hospital District|North Iowa Community Health Plan (NICH)|North Shore - LIJ (Healthfirst)|North Shore - LIJ CareConnect Insurance Company|North Texas Healthcare Network|North West Administrators (FCHN)|North West Orange County Medical Group|Northeast Carpenters Funds|Northeast Georgia Health Services|Northeast Iowa Area Agency|Northern California Advantage Medical Group  (NCAMG)|Northern Illinois Health Plan|Northern Nevada Operating Engineers Health and Welfare|Northern Nevada Trust Fund (Benefit Plan Admin)|Northridge Medical Group|Northshore Physician Associates|NorthShore University Health System|Northwest Community Health Partners|Northwest Diagnostic Clinic (NWDC)|Northwest Physicians Network|Northwood Healthcare|Nova Healthcare Administrators, Inc.|NovaNet|NovaSys Health - Medical|NP PHP Commercial|NP PHP Medicare|NP PHP OHP|NP Yamhill County CCO|NTCA - National Telecommunications Cooperative Association|Nuestra Familia Medical Group|NX Health Network (UCS)|NXT IPA|Nyhart|Oak West Physician Association|Oasis IPA (Regal)|Occupational Eyewear Network, Inc.|Ogden Benefits Administration (EBC)|Ohana Health Plan|Ohio Health Choice PPO|Ohio PPO Connect|Ojai Valley Community Medical Group|Oklahoma DRS DOC|OLOLRMC Uninsured Patient Services Program|Olympus Managed Health Care (OMHC)|Omni Healthcare - Humboldt Del Norte, California|Omnicare Medical Group (OMNI)|Oncology Network of Orange County|Oncology Physicians Network CA PC|One Call Medical|One Care Connect (OCC)|One Health Plan|One Health Plan of California|One Health Plan of Colorado|OneNet PPO (formerly Alliance PPO & MAPSI)|OnLok Senior Health Services, Inc.|OPEIU Locals 30& 537|Operating Engineers Local #53|Operating Engineers Local 501 of California|Operating Engineers Locals 302 & 612 Health & Security Fund,  Group # F12|OptiCare Managed Vision (Formerly known as Prime Vision Health)|Opticare of Utah|Optima Insurance Company|Optimum Choice of the Carolina's (OCCI)|Optimum Healthcare|Options Health Plan|Optum – Complex Medical Conditions (CMC) (formerly OptumHealth Care Solutions and United Resource Networks)|Optum / Salt Lake County (Medicaid)|OptumCare / AZ, UT (formerly Optum Medical Network & Lifeprint Network)|OptumCare Network of Connecticut|OptumHealth / OptumHealth Behavioral Solutions (formerly United Behavioral Health [UBH] and PacifiCare Behavioral Health )|OptumHealth / OptumHealth Behavioral Solutions of NM|OptumHealth / OptumHealth Physical Health (includes Oxford) (Formerly ACN)|Orange Coast Memorial IPA|Orange County Advantage Medical Group|Orthonet - Aetna|Orthonet - Uniformed Services Family Health Plan|Oscar Health|OSF Health Plans|OSMA Health (formerly PLICO)|Outsourcing Program|P.S.E.W. Trust|P3 Health Partners Arizona|P3 Health Partners Nevada|PA Health and Wellness - Medical|PA Preferred Health Network (PPHN)|PACE Center|PACEpartner Solutions|Pacific Alliance Medical Center|Pacific Associates IPA|Pacific Health MSO|Pacific Healthcare IPA|Pacific IPA|Pacific Southwest Administrators (PSWA)|PacificSource Community Solutions|PacificSource Health Plans|PacificSource Medicare|Palomar Pomerado Hospital|Pan American Life Insurance Group|Paradigm Senior Care Advantage|Paramount Health Care|Parity Healthcare|Parkland Community Health Plan|Partners Behavioral Health|Partners Health Plan|Partners in Health|Partners National Health Plan of NC|Partnership HealthPlan of California|Pasadena Primary Care Physicians Group|Passport Advantage|Passport Health Plan|Passport Health Plan|PATH Administrators (Formerly DH Evans)|Patient Advocates LLC  |Patient Services, Inc|PatientPay|Paul Mueller|Payer Compass – ACS Benefits|Payer Fusion Holdings|PBS - Oregon Alaska Industrial Hardware|PBS - Oregon Alaska Rubber & Supply|PBS - Oregon Cascade Rubber Products|PBS - Oregon Construction Machinery Industrial|PBS - Oregon Peninsula Airways|PBS - Oregon TDX Corporation|PCA Health Plan of Texas (Humana)|PCMG of San Luis Obispo (formerly San Luis Obispo Select IPA)|PCMG of Santa Maria (formerly Midcoast Care Inc.)|PDT - Hollywood Presbyterian-St Vincent|Peach State Health Plan (Ambetter) - Medical|Peak Pace Solutions|Pediatric Associates|PEF Clinic|Pegasus Medical Group (Regal)|Pekin Insurance|Penn Behavioral Health|Peoples Health Network|Personal Insurance Administrators, Inc. (PIA)|PHCS GEHA|Philadelphia American Life Insurance Company|Phoenix Health Plan (Medicare)|PHP of the Carolinas|Physician Associates of Greater San Gabriel Valley|Physician Associates of Louisiana|Physician Partners IPA - Alameda (ProCare MSO)|Physician Partners IPA - Santa Clara (ProCare MSO)|Physician Partners IPA - South (ProCare MSO)|Physicians Care|Physicians Care Network|Physicians Care Network / The Polyclinic|Physicians Choice Medical Group of San Luis Obispo|Physicians Choice Medical Group of Santa Maria|Physicians Data Trust (PDT)|Physicians Health Choice- HCFA Claims|Physicians Health Network (PHN)|Physicians Health Plan|Physicians Health Plan (PHP) - Global Care|Physicians Health Plan (PHP) - Mid Michigan|Physicians Health Plan (PHP) - Northern Indiana|Physicians Health Plan (PHP) - South Michigan|Physicians Health Plan (PHP) - Sparrow|Physicians Healthcare Plans|Physicians Healthways IPA|Physicians Management Group|Physicians Medical Group of San Jose|Physicians Medical Group of Santa Cruz County|Physicians Mutual Insurance Company|Physicians of Southwest Washington|Physicians Plus Insurance Corporation|Physicians United Plan|Physicians’ Healthways IPA|PhysMetrics (DOS prior to 7/1/17)|PhysMetrics (formerly ChiroMetrics)|PIH Health (formerly Bright Health)|Pinnacle Claims Management, Inc.|Pinnacle Health Resources|Pinnacol Assurance|Pioneer Provider Network|Pittman and Associates (Now known as HealthSmart Benefit Solutions)|Planned Admin Inc P and C Division|Planned Administrators, Inc. (PAI)|Plexis Healthcare Systems (UCS)|PMG of San Jose|Podi Care Managed Care|Podiatry First|Podiatry Network Florida|Podiatry Plan Inc	|Poe & Brown|Point Comfort Underwriters|Point Comfort Underwriters|Pomona Valley Medical Group (PVMG)|Populytics (formerly Spectrum Administrators)|Porter Scott|Positive Health Care (PHC) / FL MCO PHC|PPO Plus LLC|Prairie States Enterprises|Prefered Administrator|Preferred Admin Childrens Hosp|Preferred Benefit Administrators|Preferred Blue of South Carolina|Preferred Care Partners - Florida|Preferred Community Choice|Preferred Community Choice - PCCSelect - CompMed|Preferred Health Plan of the Carolinas|Preferred Health Professionals|Preferred Healthcare System - PPO|Preferred IPA|Preferred Network Access, Inc.|PreferredOne|Premera Blue Cross Blue Shield of Alaska|Premera Blue Cross of Washington|Premier Administrative Solutions|Premier Benefits Inc.|Premier Care IPA (ProCare MSO)|Premier Care of Northern California|Premier Eye Care|Premier Health Plan (Premier Health Group)|Premier Health Plans|Premier Health Systems|Premier HealthCare Exchange (PHX)  - A-G Admin (SOMA group)|Premier HealthCare Exchange (PHX)  - Auxiant|Premier HealthCare Exchange (PHX)  - BPA – Benefit Plan Administrators|Premier HealthCare Exchange (PHX)  - Commercial Travelers|Premier HealthCare Exchange (PHX)  - First Agency (FIA)|Premier HealthCare Exchange (PHX)  - Mayo Clinic FL/GA|Premier HealthCare Exchange (PHX)  - Med-Pay|Premier HealthCare Exchange (PHX)  - Mississippi Physician Care Network|Premier HealthCare Exchange (PHX) - BMI Kansas|Premier HealthCare Exchange (PHX) - CDS of Nevada|Premier HealthCare Exchange (PHX) - Cypress Benefit Admin|Premier HealthCare Exchange (PHX) - Elmco|Premier HealthCare Exchange (PHX) - Fox Everett/HUB International|Premier HealthCare Exchange (PHX) – Preferred Admin|Premier HealthCare Exchange (PHX) – The Benefit Group|Premier Patient Care IPA|Premier Physician Alliance|Premier Physician Network|Premier Physician Network (PPN)|Presbyterian Health Plan|Presbyterian Health Plan, Inc. (Magellan)|Presbyterian Intercommunity Hospital|Presbyterian Salud (NM)|Presence ERC|Prestige Health Choice (AmeriHealth Caritas)|Prezzo|Primary Care & Pediatric IPA (PCP IPA)|Primary Care Associates Medical Group (PCAMG)|Primary Care Associates of California (PCAC)|Primary Care of Joliet|Primary Care Practices Of Sacramento - EHS|Primary Health CCO Medical|Primary Health Network|Primary Health of Josephine County|Primary Physician Care|Primary Provider Management|Primary Provider Management Encounters|PrimaryHealth CCO Mental Health|Prime Community Care Central Valley  Encounters (MV Medical)|Prime Community Care Central Valley (MV Medical)|Prime West Health Systems|PrimeCare Medical Group of Chino Valley (PCMG)|PrimeCare Medical Network|PrimeCare Medical Network - Chino|PrimeCare Medical Network - Citrus Valley|PrimeCare Medical Network - Corona|PrimeCare Medical Network - Hemet|PrimeCare Medical Network - Inland Valley|PrimeCare Medical Network - Moreno Valley|PrimeCare Medical Network - Redlands|PrimeCare Medical Network - Riverside|Primecare Medical Network - San Bernardino|PrimeCare Medical Network - Sun City|PrimeCare Medical Network - Temecula|Princeton Premier IPA|Priority Health|Prism - Univera|Prism Health Networks|prnLink-MLK|ProCare (Prospect)|Procare Health Plan - Medicaid|Productive Programming, Inc. (PPI)|Professional Benefit Administrators, Inc.|Professional Claims Management - Canton, Ohio|Progressive Medical Associates, Inc|Progyny|Prominence Health Plan of Nevada|Prospect Health Services of Texas|Prospect Medical Group|Prospect Provider Group of Rhode Island|Prospect Sherman Oaks Medical Group|Protective Life Insurance|Providence Facility Claims|Providence Health Plan|Providence PPO|Providence Preferred PPO|Providence Risk & Insurance|Provident American Life & Health Insurance Company (Medicare Supplement)|Provident Healthsource (CIGNA)|Provident Life & Accident|Provident Life & Casualty|Provident Preferred Network|Provider Direct Network (HealthSmart Preferred Care)|Provider Partners Health Plan of Maryland|ProviDRs Care Network|Prudent Medical Group|Pruitt Health Premier|PSKW|Psychcare, LLC|Psychiatric Centers at San Diego (PCSD)|Public Employees Health Plan (PEHP)|Public Health Medical Services (PHMS)|Puget Sound Benefits Trust, Group# F25|Puget Sound Electrical Workers Trust, Group# F33|QCA Health Plan|Quad Cities Community Health Plan (QCCH)|QualCare (Managed Care Systems)|QualCare Alliance Networks, Inc. (QANI)|QualCare, Inc.|QualChoice Advantage|QualChoice Health Insurance|QualChoice of Arkansas|Quality Care IPA|Quality Care Partners|Quality Health Plans of New York|Quartz ASO|Quest Behavioral Health|Quest EAP|Questcare Medical Services|Quick Trip Corporation|QVI Risk Solutions Inc|Rady Children’s Health Network (RCHN)|Rady’s Children’s Specialists of San Diego|Ravenswood Physician Associates|Redwood Community Health Coalition|Regal Medical Group|Regence Blue Cross Blue Shield - Oregon|Regence Blue Shield - Idaho|Regence Blue Shield of Washington State|Regence Group Administrators (RGA)|Regency Employee Benefits|Regent Medical Group, Inc. (Regent Family Practice of Glendale)|Regional Care, Inc.|Rehn and Associates|Religious Order of Jehovah's Witnesses|Renaissance Gulf Quest|Renaissance Physicians Organization|Renassaince - River Oaks|Renassaince Riveroaks Blue|Reserve National Insurance|Resolve Health Plan Administrators, LLC.|ResourceOne Administrators|Resurrection Health Care Preferred|Resurrection Physicians Group|Right Care from Scott & White|Right Choice Benefit Administrators|Rios Southwest Medical Group|River City Medical Group|Riverside Health, Inc.|Riverside Medical Clinic|Riverside Physicians Network|RMSCO, Inc.|Robert F Kennedy Medical Plan|Rocky Mountain HMO|Royal Health Care|Royal Healthcare on behalf of Extended MLTC|RTG Medical Group|Rural Carrier Benefit Plan|Rush Prudential Health Plans - HMO Only|Ryan White Network (RWN)|S & S Healthcare Strategies|Sagamore Health Network|Sage Program|Sage Technologies|Saint Agnes Medical Group|Saint Johns Health Clinic|Samaritan Advantage (SA01)|Samaritan Choice (SCP)|Samaritan Health Plans|Samaritan Healthy Kids (KID)|Samaritan Ministries International|SAMBA|San Bernardino Medical Group (SBMED)|San Diego County Medical Services (CMS)|San Diego County Physician Emergency Services (PES)|San Diego County Primary Care Services|San Diego PACE|San Diego Physicians Medical Group|San Francisco Electrical Workers - IBEW Local 6|San Francisco Health Plan|San Joaquin Health Administrators|Sana Benefits|SandHills Center|Sanford Health Plan|Sansum Clinic|Santa Barbara Select IPA|Santa Clara County IPA ACO|Santa Clara County IPA HMO|Santa Clara County IPA PPO|Santa Clara Family Health Plan (SCFHP)|Sante Community Medical Centers|Sante Community Physicians|Sante Medi-Cal|Saudi Health Mission|Scan Health Plan - Arizona|Scan Health Plan - California|SCCIPA Anthem ACO|SCCIPA Anthem PPO|SCCIPA HMO|SCHS Alta Pod by MedPOINT|Scott & White Health Care Plan|Screen Actors Guild (SAG AFTRA)|Scripps Clinic|Scripps Coastal Medical Center|Scripps Health Plan MSO|Scripps Health Plan Services|Scripps Medical Plan (Aetna)|Scripps Physicians Medical Group|Seaside Health Plan|Seaside Health Plan Encounters|Seattle Area Plumbing|Seaview IPA|Secure Health Plans of Georgia, LLC|SecureOne|Security Administrative Services, LLC (Cornerstone)|Security Health Plan|SEIU Local 1 Cleveland Welfare Fund|Select Administrative Services (SAS)|Select Benefits Administrators|Select Benefits Administrators of America|Select Health of South Carolina|Select Health of Utah|Select Healthcare System, Inc.|Select Senior Clinic|SelectCare of Texas (GTPA - Golden Triangle)|SelectCare of Texas (Houston)|SelectCare of Texas (HPN - Heritage Physicians)|SelectCare of Texas (Kelsey-Seybold)|SelectCare of Texas (MCA – Memorial Clinical)|SelectCare of Texas (NWDC)|Self Insured Plans|Self Insured Services Company (SISCO)|Self-Funded Plans, Inc.|Selman and Company|Selman Tricare Supplement Plans|Sendero IdealCare|Sendero IdealCare SHP|Senior Network Health|Senior Whole Health|Sentara Family Plan / Sentara Health Management|Sentara Mental Health Professional|Sentinel Management Services|Sentinel Security Life Insurance Company|Sentry Life Insurance Company|Seoul Medical Group|Sequoia Health IPA|Seton Health Plan - CHIP|Seton Health Plan (Employee Plans ONLY)|Seton Map Program (Mediview)|Seton Star|Seven Corners|SGIC|Sharp Community Medical Group|Sharp Health Plan|Sharp Rees-Stealy Medical Group|Shasta Administrative Services|SIEBA, LTD|Sierra Health and Life - Encounters|Sierra Health and Life (Claims)|Sierra Medical Group (Regal)|Signature Advantage|SIHRTE|Silicon Valley Medical Development|SilverBack TPA|SilverSummit Healthplan - Medical|Simply Health Care Plan|SIMPRA Advantage|Sinclair Health Plan|SISCO Benefits|Sloans Lake Managed Care|Smith Administrators|Smoky Mountain Center (now Vaya Health)|Solidarity Healthshare|Solis Health Plans|Sound Health & Wellness Trust (Aetna)|Sound Health (now known as First Choice Health Network)|Sound Health (Sisters of Providence)|Sound Health (Sisters of Providence)|Sound Health (Sisters of Providence)|Sound Health Wellness Trust|Soundpath Health (formerly known as Puget Sound Health Partners)|South Atlantic Medical Group|South Central Preferred|South Country Health Alliance|South Dakota Behavioral Health - STARS / State Funding|South Dakota DSS - STARS / State Funding|South Florida Community Care Network - SFCCN (Commercial)|South Florida Community Care Network - SFCCN (Medicaid)|South Florida Musculo Skeletal Care (SFMSC)|South Tahoe Refuse|SouthCare/Healthcare Preferred|Southeastern Indiana Health (SIHO)|Southern Benefit Services|Southern California Children's Healthcare Network|Southern California Dairy|Southern California Healthcare System (SCHS) Alta Pod by MedPOINT|Southern California Lumber|Southern California Oncology Associates (SCOA)|Southern California Oncology Associates (SCOA) - Encounters|Southern California Physicians Managed Care Services (SCPMCS)|Southern California UFCW Unions & Food Employers|Southern Illinois Healthcare Association|Southland Advantage Medical Group, Inc.|Southland IPA|Southland San Gabriel Valley Medical Group, Inc.|Southwest Oregon IPA (DOCS)|Southwest Service Administrators|Southwest Service Life|Special Agents Mutual Benefit Association|Spencer Stuart (ARM, LTD)|Spina Bifida - VA HAC|Spohn Health|St James PHO|St Vincent Medical Center (STVMC)|St. Barnabas System Health Plan|St. Francis - AllCare by MedPOINT|St. Francis - HCLA by MedPOINT|St. Francis IPA|St. John's Claim Administration|St. Johns Health Clinic|St. Joseph Heritage Healthcare|St. Joseph Heritage Medical Group|St. Joseph Hospital Affiliated|St. Jude Affiliated Physicians|St. Jude Heritage Medical Group|St. Mary High Desert|St. Mary’s IPA (SMIPA)|St. Mary's Health Plan|St. Peter Medical Group, Inc.|St. Therese Physician Associates|St. Thomas Med Ntwk Gulfquest|St. Vincent IPA|Standard Life & Accident Insurance Company|Starmark|State Farm Insurance Company|State Farm Property & Casualty|State Health Plan of North Carolina|State Health Plan of SC|State of Texas Dental Plan (GEHA)|Staywell Health Plan|Sterling Medicare Advantage|Sterling Option One|Steward Health Choice Generations Utah|Stowe Associates|Student Resources (UnitedHealthcare)|Summacare Health Plan (HMO)|Summit Administration Services, Inc.|Summit America Insurance Services, Inc.|Summit Community Care|Sunflower State - Medical|Sunrise Advantage Plan of Illinois|Sunrise Advantage Plan of New York |Sunrise Advantage Plan of Pennsylvania|Sunrise Advantage Plan of Virginia|Sunshine State Health Plan (Ambetter) - Medical|Superior Choice Medical Group|Superior Choice Medical Group |Superior Health Plan (Ambetter) - Medical|Superior Insurance Services|Sutter Connect - East Bay Region Hospitals - B&T|Sutter Connect – East Bay Region Hospitals – Non Sutter Groups|Sutter Connect - Palo Alto Medical Foundation|Sutter Connect - Solano Regional Medical Foundation (SRMF)|Sutter Connect - Sutter Delta Medical Group|Sutter Connect - Sutter Gould Medical Foundation (SGMF)|Sutter Connect - Sutter Independent Physicians, Sutter Medical Group, Sutter West Medical Group  (SIP/SMG/SWMG)|Sutter Connect - Sutter Medical Group of the Redwoods, Santa Rosa Network (SMGR/SRN)|Sutter East Bay Medical Foundation|Sutter Senior Care|Swedish Covenant Hospital|Swift Glass Corporation (UCS)|SynerMed|T2017 Tricare West (Previously known as UnitedHealthcare Military & Veterans)|TakeCare Insurance Company|Talbert Medical Group|Tall Tree Administrators|Tarrant Health Services|Taylor Benefit Resource|TCC (Self-Funded)|TCC Benefits Administrator - Pre-Med Defender|TCU-LA MTA|Teachers Health Trust|Team Choice PNS|Teamcare|Teamsters Miscellaneous Security Trust Fund - Northwest Administrators|Teamsters Welfare Trust|Tech-Steel, Inc|Telamon|Temecula Valley Medical Group|TennCare Select/ BlueCare|TexanPlus of Texas (Houston)|TexanPlus of Texas (Kelsey-Seybold)|Texas Agricultural Cooperative Trust (TACT)|Texas Children's Health Plan|Texas Childrens Health Plan - Medicaid|Texas First Health Plans|Texas HealthSpring|Texas Medicaid & Healthcare Partnership (TMHP)|Texas Tech University Medical|The Alliance|The Benefit Group, Inc.|The Boon Group|The Care Network/The Savannah Business Group|The Chesterfield Companies|The CHP Group|The City of Odessa|The Health Exchange (Cerner Corporation)|The Health Plan|The Health Plan of Upper Ohio Valley|The Health Plan of West Virginia (Medicaid)|The Health Plan of Western Illinois (HOWI)|The Integrity Benefit Network, Inc. (Marietta, GA)|The Loomis Company|The Macaluso Group|The Mutual Group|The Physicians Alliance Corp.|The Preferred Healthcare System - PPO|THIPA|Thomas H. Cooper|Thrivent Financial|Thrivent Financial Aid Association for Lutheran Medicare|Thrivent Financial Lutheran Brotherhood Medicare|Tift Regional|TKFMC|TLC Advantage of Sioux Falls|TLC Benefit Solutions|Todays Health|Together with CCHP|Tongass Timber Trust|Tooling and Manufacturing Assocation|Torrance Hospital IPA (THIPA)|Torrance Memorial Medical Center|Total Community Care|Total Health Care|Total Plan Concepts|Total Plan Services (via SmartData)|Touchpoint Solutions / Touchpoint CRM)|Touchstone Health PSO|TR Paul, Inc.|Transamerica Life Insurance Company (TLIC)|TransChoice - Key Benefit Administrators|Transplant Associates Baylor Billing|Transwestern General Agency|Transwestern Insurance Administrators, Inc|Trellis Health Partners|TRIAD HEALTHCARE INC|Tribute Health Plan of Arkansas|Tribute Health Plan of Oklahoma|TriCare East|TriCare for Life|Tricare Overseas|Tricare West|TriCities IPA|TriHealth Physician Solutions|Trillium Community - Medicaid|Trillium Community - Medicare|Trillium Community Health Plans (Agate Resources/LIPA)|Trilogy Network|TriStar (formerly Select Benefit Administrators)|TriState Benefit Solutions|TRLHN/AS|Tru Blue TPA|True Health New Mexico|Trusted Health Plan|Trusted Senior Care Advantage|Trustee Plans (FCHN)|Trusteed Insurance (FCHN)|Trusteed Plans Service Corporation|Trustmark Insurance Company|Tufts Health Plan|Tufts Health Public Plan (Network Health)|UC Davis Health System|UC Health Plan Administrators|UC Irvine|UC Irvine Health (MemorialCare)|UCare Individual & Family Plans (DOS on or After 1/1/2019)|UCare Minnesota|UCLA Medical Group|UCS - BASI: Lynco|UCS - Coeur: Connectivity Source|UCS - Coeur: Genesis Park Management|UCS - Coeur: Hermann Sons Life|UCS - Coeur: Leonard Holding Company|UCS - Coeur: Master Mobile Link|UCS - Coeur: Northwest Petroleum|UCS - Coeur: Sercel|UCS - Coeur: The Woodlands Christian Academy|UCS - Insight Benefit Administrators|UCS (The City of East Chicago)|UCS BASI Hotstart|UCS BASI: Ellsworth Constructions|UCS BASI: Meter Group USA|UCS Benovation|UCS Cement Masons|UCS Coeur Humphrey Associates|UCS Core: Nemco|UCS Core: TJR Equipment|UCS ES Beveridge & Associates Inc: Grace Schools|UCSD Physician Network (UCSDPN)|UFCW Northern California &Drug Employers H & W Trust Fund|UFT Welfare Fund|UHA Health Insurance|UHP Management|UICI Administrators|Ultra Benefits Inc.|UM Health Partners (UMHP)|UMC Health Plan|Umpqua Health Alliance|UMR (formerly UMR Onalaska/Midwest Security/Harrington)|UMR (formerly UMR Wausau)|Umwa Health & Retirement Funds|Underwriters Services Corporation|Unicare|Unified Group Services|Unified Health Services (Workman's Comp Only)|Unified IPA|Unified IPA|Unified Physicians Network|Uniform Medical Plan|Uniformed Services Family Health Plan|UNIGARD (FCHN)|Union Pacific Railroad Employees Health Systems (UPREHS)|United Agriculture Benefit Trust|United Benefits (formerly Brown &amp; Brown Benefits)|United Care Medical Group (UCMG) (Regal)|United Employee Benefit Trust (FCHN)|United Food & Commercial Workers Unions and Food Employers Benefit Fund|United Group Programs|United Medical Alliance (UMA)|United of Omaha|United Physicians International (UPI)|United States Automobile Association (USAA)|UnitedHealthcare (UHC)|UnitedHealthcare / All Savers Alternate Funding|UnitedHealthcare / All Savers Insurance|UnitedHealthcare / Definity Health Plan|UnitedHealthcare / Empire Plan|UnitedHealthcare / MAHP - MD IPA, Optimum Choice and MLH (formerly MAMSI)|UnitedHealthcare / Oxford|UnitedHealthcare / Spectera Eyecare Networks|UnitedHealthcare / UHIS - UnitedHealth Integrated Services|UnitedHealthcare / UnitedHealthcare Plan of the River Valley (formerly John Deere Healthcare)|UnitedHealthcare / UnitedHealthcare West (formerly PacifiCare)|UnitedHealthcare Community Plan / AZ (formerly Arizona Physicians IPA and APIPA)|UnitedHealthcare Community Plan / CA, DE, FL, HI, IA, LA, MA, MD, MS CAN, NC, NE, NM, NY, OH, OK, PA, RI, TX, VA, WA, WI (formerly AmeriChoice or Unison)|UnitedHealthcare Community Plan / IA, hawk-i (formerly AmeriChoice of Iowa)|UnitedHealthcare Community Plan / KS (KanCare)|UnitedHealthcare Community Plan / MI (formerly Great Lakes Health Plan)|UnitedHealthcare Community Plan / MS (formerly AmeriChoice MS - CHIP)|UnitedHealthcare Community Plan / NE (formerly Americhoice NE, ShareAdvantage, and UnitedHealthcare of the Midlands)|UnitedHealthcare Community Plan / NJ (formerly AmeriChoice NJ Medicaid, NJ Family Care, NJ Personal Care Plus)|UnitedHealthcare Community Plan / NY (formerly AmeriChoice by UnitedHealthcare, AmeriChoice NY Medicaid & Child Health Plus, AmeriChoice NY Personal Care Plus)|UnitedHealthcare Community Plan / SC (formerly Unison)|UnitedHealthcare Community Plan / TN (formerly AmeriChoice TN: TennCare, Secure Plus Complete)|UnitedHealthcare Community Plan / UnitedHealthcare Dual Complete - Oxford Medicare Network|UnitedHealthcare Community Plan / UnitedHealthcare Dual Complete (formerly Evercare)|UnitedHealthcare Community Plan / UnitedHealthcare Long Term Care (formerly Evercare)|UnitedHealthcare Community Plan / UnitedHealthcare Plan of the River Valley (formerly John Deere Healthcare)|UnitedHealthcare Community Plan of Missouri|UnitedHealthcare Medicare Solutions / UnitedHealthcare Chronic Complete  (formerly Evercare)|UnitedHealthcare Medicare Solutions / UnitedHealthcare Group Medicare Advantage|UnitedHealthcare Medicare Solutions / UnitedHealthcare MedicareComplete (formerly SecureHorizons)|UnitedHealthcare Medicare Solutions / UnitedHealthcare MedicareDirect  (formerly SecureHorizons)|UnitedHealthcare Medicare Solutions / UnitedHealthcare Nursing Home Plan  (formerly Evercare)|UnitedHealthcare Shared Services (UHSS)|UnitedHealthOne / Golden Rule|UnitedHealthOne / PacifiCare Life and Health Insurance Company|UnitedHealthOne / UnitedHealthcare Life Insurance Company (formerly American Medical Security)|Unity Health Insurance|Univera Healthcare (Excellus)|Univera PPO (Excellus)|Universal Care (FFS)|Universal Care Encounters|Universal Fidelity Administrators Company - Aggregate|University Care Advantage|University Family Care|University Health Alliance|University Healthcare Group|University Healthcare Marketplace|University of Illinois at Chicago Div of Specialized Care for Children|University of Maryland Health Partners (UMHP)|University of Miami Behavioral Health|University of Missouri|University of Utah Health Plans|University Physicians Care Advantage|University Physicians Healthcare Group|University Trust|Upland Medical Group|UPMC Health Plan|UPMC Vision Advantage|Upper Peninsula Health Plan|US Benefits|US Department of Labor|US Department of Labor - Black Lung|US Department of Labor - Energy|US Family Health Plan of Texas and Louisiana (USFHP)|US Imaging Network|US Imaging Network II|USFHP - St. Vincent Catholic Medical Centers of New York|Utah Carpenters|Utah Laborers|Utah Pipe Trades|UTMB 3 Share Program|VA Community Care Network Region 1|VA Community Care Network Region 2|VA Community Care Network Region 3|VA Fee Basis Programs|Valir Pace|Valley Care IPA|Valley Care Select IPA|Valley Health Plan (Commercial)|Valley Health Plan (Medi-Cal)|Valley Physicians Network|Value Options (Beacon Health Options)|Vantage Health Plan, Inc.|Vantage Medical Group|Vantage Medical Group Encounters|Variable Protection Administrators (VPA)|Varipro|Vaya Health (formerly Smoky Mountain)|Ventura County Health Care Plan (VCHCP)|VentureNet Healthcare|Verdugo Hills Medical Group (Regal)|Verity National Group|VestaCare|Vestica Healthcare|VGM Homelink|Vida Care|VieCare Life Beaver and Life Lawrence Counties|Village Health Plan|VillageCareMAX|VillageMD of Northern Indiana|Virginia Health Network, Inc.|Virginia Mason Group Health|Virginia Premier CompleteCare|Virginia Premier Elite|Virginia Premier Elite Plus|Virginia Premier Gold|Virginia Premier Health Plan|Virginia Premier Medallion 4.0|Vista Health Plan|Vitality Health Plan of CA FFS|Viva Health Plan|Vivida Health|VNA Homecare Options|VNS Choice SelectCare|Volunteer States Health Plan|Volusia Health Network|Vytra Health Plans (Emblem)|W.C. Beeler & Company|WA Teamster|Wagner Meinert|Washington County Health and Human Services|Washington State Health Insurance Pool  (WSHIP)|Waterstone Benefit Administrators|Watts Health Care|WEA Insurance Group|Web TPA / Community Health Electronic claims / CHEC|Weiss Health Providers|Well Sense Health Plan|WellCare Health Plan|Wellcare Health Plans - Encounters|WellCare HMO|WellCare Private FFS|WellMed Medical (Claims)|WellMed Medical (Encounters)|WellSpan EAP|WellSpan Employee Assistance Program|WellSystems, LLC|Wenatchee Valley Medical Center|West Covina Medical Group (Regal)|West Suburban Health Providers|West Virginia Senior Choice|Western Growers Assurance Trust|Western Growers Insurance Company|Western Health Advantage|Western Mutual Insurance (WMI)|Western Oregon Advanced Health|Western Sky Community Care|Western Southern Financial Group|Western Utilities or Local 57|Westlake Financial|White Memorial Medical Center|Willamette Valley Community Health CCO|William C. Earhart Company|William J Sutton & Co. Ltd (Toronto)|Willow Health|Wilson-McShane|Wilson-McShane: International Brotherhood of Boilermakers|WINhealth Partners|Winston Hospitality|Wisconsin Assigned Risk Plan (HIRSP)|Wisconsin Physicians Service (WPS) Commercial|Women's Integrated Network (WIN Health)|Worksite Benefit Services, LLC|WPAS (FCHN)|WPPN- HPO (HealthEOS)|WPS Arise (Prevea)|WPS Health Insurance|Writer's Guild Industry Health Plan|WSHIP - Washington State Health Insurance Pool|Wyoming Health Solutions|Ximed Medical Group|Yamhill CCO|Yamhill COO Physical Health|Yerington Paiute Tribe|YourCare Health Plan|Zenith (FCHN)|Zenith American Solutions - ILWU-PMA";
            #endregion
            string[] sa = s.Split('|');
            List<string> InsuranceNames = sa.ToList();
            List<Insurance> insurances = _context.Insurance.ToList();
            List<PlanType> plantypes = _context.PlanType.ToList();
            List<Edi837Payer> payers = _context.Edi837Payer.Where(x => x.ID < 2783).ToList();

            int hits = 0, misses = 0;
            int count = 0;
            foreach (string Name in InsuranceNames)
            {
                string TempName = Name.Replace("'", "\"").Trim();
                InsurancePlan insurancePlan = new InsurancePlan();
                insurancePlan.PlanName = TempName.ToUpper();
                insurancePlan.Description = TempName.ToUpper();

                if (Name.Contains("medicare", StringComparison.InvariantCultureIgnoreCase))
                {
                    insurancePlan.InsuranceID = insurances.Where(lamb => lamb.Name.Contains("medicare", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().ID;
                    insurancePlan.PlanTypeID = plantypes.Where(lamb => lamb.Code.Contains("ma", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().ID;
                }
                else if (Name.Contains("medicaid", StringComparison.InvariantCultureIgnoreCase))
                {
                    insurancePlan.InsuranceID = insurances.Where(lamb => lamb.Name.Contains("medicaid", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().ID;
                    insurancePlan.PlanTypeID = plantypes.Where(lamb => lamb.Code.Contains("mc", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().ID;
                }
                else if (Name.Contains("bcbs", StringComparison.InvariantCultureIgnoreCase))
                {
                    insurancePlan.InsuranceID = insurances.Where(lamb => lamb.Name.Contains("bcbs", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().ID;
                    insurancePlan.PlanTypeID = plantypes.Where(lamb => lamb.Code.Contains("bl", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().ID;
                }
                else
                {
                    insurancePlan.InsuranceID = insurances.Where(lamb => lamb.Name.Contains("commercial", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().ID;
                    insurancePlan.PlanTypeID = plantypes.Where(lamb => lamb.Code.Contains("ci", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().ID;
                }

                insurancePlan.IsCapitated = false;
                insurancePlan.SubmissionType = "E";
                //Debug.WriteLine(Name.Length);
                var temp = payers.Where(x => x.PayerName.IsNull() ? false : x.PayerName.Contains(TempName));

                if (temp.Count() != 0)
                {
                    hits++;
                    insurancePlan.Edi837PayerID = payers.Where(x => x.PayerName.IsNull() ? false : CompareStrings(x.PayerName, TempName.Replace("'", "\""))).FirstOrDefault().ID;
                }
                else
                {
                    Debug.WriteLine(count);
                    misses++;
                    insurancePlan.Edi837PayerID = null;
                }

                insurancePlan.IsActive = true;
                insurancePlan.IsDeleted = false;
                insurancePlan.AddedDate = DateTime.Now;

                string Query = "IF NOT EXISTS(SELECT * FROM InsurancePlan WHERE PlanName = '" + insurancePlan.PlanName + "') BEGIN insert into InsurancePlan (PlanName, Description, InsuranceID, PlanTypeID, IsCapitated, SubmissionType, Edi837PayerID, IsActive, IsDeleted, AddedDate) values('" + insurancePlan.PlanName.Replace("'", "\"") + "','" + insurancePlan.Description.Replace("'", "\"") + "'," + insurancePlan.InsuranceID + "," + insurancePlan.PlanTypeID + "," + insurancePlan.IsCapitated + ",'E'," + (insurancePlan.Edi837PayerID.IsNull() ? "null" : insurancePlan.Edi837PayerID + "") + "," + insurancePlan.IsActive + "," + insurancePlan.IsDeleted + ",'" + DateTime.Now.Format("") + "') END";
                Debug.WriteLine(Query);
                count++;
            }

            Debug.WriteLine(hits + "  " + misses);
            return Ok();
        }

        [HttpPost]
        [Route("RevalidateVisits")]
        public async Task<ActionResult> RevalidateVisits(VMDataMigration InputData)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<Cpt> CptData = _context.Cpt.ToList();
            List<InsurancePlan> InsurancePlanData = _context.InsurancePlan.ToList();
            List<ExternalCharge> InValidExternalCharges = _context.ExternalCharge.Where(lamb => lamb.PracticeID == UD.PracticeID).Where(lamb => lamb.VisitID == null && lamb.ChargeID == null).OrderBy(lamb => lamb.GroupID).ThenBy(lamb => lamb.AddedDate).ToList();
            List<PatientPlan> PatientPlanData = _context.PatientPlan.ToList();
            Debug.WriteLine(InValidExternalCharges.Count());
            long LinkedCharges = 0;
            List<List<ExternalCharge>> InvalidExternalChargesByGroupId = new List<List<ExternalCharge>>();
            List<ExternalCharge> IntermediateList = new List<ExternalCharge>();
            foreach (ExternalCharge Temp in InValidExternalCharges)
            {
                if (IntermediateList.Count() != 0)
                {
                    if (Temp.GroupID != IntermediateList.ElementAt(IntermediateList.Count() - 1).GroupID)
                    {
                        InvalidExternalChargesByGroupId.Add(IntermediateList);
                        IntermediateList = new List<ExternalCharge>();
                        IntermediateList.Add(Temp);
                    }
                    else
                    {
                        IntermediateList.Add(Temp);
                    }
                }
                else
                {
                    IntermediateList.Add(Temp);
                }
            }
            if (IntermediateList.Count() != 0)
            {
                InvalidExternalChargesByGroupId.Add(IntermediateList);
            }
            foreach (List<ExternalCharge> GroupedCharges in InvalidExternalChargesByGroupId)
            {
                foreach (ExternalCharge TempEC in GroupedCharges)
                {
                    string CptCode = TempEC.CptCode;
                    string InsuranceName = TempEC.InsuranceName;
                    bool CptMatched = false, PrimaryPlanIDFound = false;
                    Cpt MatchedCpt = CptData.Where(lamb => lamb.CPTCode.Equals(TempEC.CptCode)).FirstOrDefault();
                    if (MatchedCpt != null)
                    {
                        TempEC.CPTID = MatchedCpt.ID;
                        if (TempEC.ErrorMessage != null)
                        {
                            TempEC.ErrorMessage = TempEC.ErrorMessage.Replace("(invalid cpt)", "");
                        }
                        CptMatched = true;
                    }
                    long PatientID = TempEC.PatientID.GetValueOrDefault();
                    if (PatientID != 0)
                    {
                        PatientPlan TempPrimaryPatientPlan = PatientPlanData.Where(lamb => lamb.PatientID.Equals(PatientID)).FirstOrDefault();
                        if (TempPrimaryPatientPlan != null)
                        {
                            InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                            if (TempInsurancePlan != null)
                            {
                                TempEC.PrimaryInsuredID = TempPrimaryPatientPlan.ID;
                                if (TempEC.ErrorMessage != null)
                                {
                                    TempEC.ErrorMessage = TempEC.ErrorMessage.Replace("(Patient plan id not found)", "");
                                }
                                PrimaryPlanIDFound = true;
                            }
                            else
                            {
                                ExInsuranceMapping TempExInsuranceMapping = _contextMain.ExInsuranceMapping.Where(lamb1 => lamb1.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                if (TempExInsuranceMapping != null)
                                {
                                    InsurancePlan TempInsurancePlan2 = InsurancePlanData.Where(lamb => lamb.ID == TempExInsuranceMapping.InsurancePlanID).FirstOrDefault();
                                    if (TempInsurancePlan2 != null)
                                    {
                                        TempEC.PrimaryInsuredID = TempPrimaryPatientPlan.ID;
                                        if (TempEC.ErrorMessage != null)
                                        {
                                            TempEC.ErrorMessage = TempEC.ErrorMessage.Replace("(Patient plan id not found)", "");
                                        }
                                        PrimaryPlanIDFound = true;
                                    }
                                }
                            }
                        }
                    }
                    if (TempEC.PrimaryInsuredID != null)
                    {
                        PrimaryPlanIDFound = true;
                    }
                    if (TempEC.CPTID != null)
                    {
                        CptMatched = true;
                    }
                    if (PrimaryPlanIDFound && CptMatched)
                    {
                        LinkedCharges++;
                    }
                }
                long NullErrorMessageCount = GroupedCharges.Where(lamb => lamb.ErrorMessage == null).Count();
                long EmptyErrorMessageCount = GroupedCharges.Where(lamb => lamb.ErrorMessage != null).Where(lamb => lamb.ErrorMessage.Equals("")).Count();
            }
            foreach (ExternalCharge TempEC in InValidExternalCharges)
            {
            }
            Debug.WriteLine(LinkedCharges + " charges got fixed");
            return Ok();
        }
        public bool CompareStrings(string str1, string str2)
        {
            if (string.IsNullOrWhiteSpace(str1) || string.IsNullOrWhiteSpace(str2)) return false;

            if (str1.Replace("\"", "").Replace("'", "").Replace(" ", "").Contains(str2.Replace("\"", "").Replace("'", "").Replace(" ", "")))
                return true;
            else
            {
                // Debug.WriteLine(str1 + "   " + str2);
                return false;
            }

        }

        [HttpPost]
        [Route("FindExternalCharges")]
        public async Task<ActionResult<IEnumerable<GExteralCharges>>> FindExternalCharges(CExteralCharges CExteralCharges)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);

            return FindExternalCharges(CExteralCharges, PracticeId);
        }

        private List<GExteralCharges> FindExternalCharges(CExteralCharges CExteralCharges, long PracticeId)
        {
            List<GExteralCharges> data = (from extChrg in _context.ExternalCharge
                                          join pat in _context.Patient on extChrg.PatientID equals pat.ID into TempPatTable
                                          from patleftjoin in TempPatTable.DefaultIfEmpty()
                                          join providertable in _context.Provider on extChrg.ProviderID equals providertable.ID into TempProviderTable
                                          from provjoinedleft in TempProviderTable.DefaultIfEmpty()
                                          join visit in _context.Visit on extChrg.VisitID equals visit.ID into TempVisitTable
                                          from visitleftjoin in TempVisitTable.DefaultIfEmpty()
                                              //join icd in _context.ICD on visitleftjoin.ICD1ID equals icd.ID into TempICDTable
                                              //from icdleftjoin in TempICDTable.DefaultIfEmpty()
                                          where extChrg.PracticeID == PracticeId &&

                                          (CExteralCharges.ResolvedErrorMessage.IsNull() ? true : CExteralCharges.ResolvedErrorMessage == "Y" ? extChrg.resolve == true : CExteralCharges.ResolvedErrorMessage == "N" ? extChrg.resolve == false : true) &&

                                          (CExteralCharges.ExternalPatientID.IsNull() ? true : extChrg.ExternalPatientID.Equals(CExteralCharges.ExternalPatientID)) &&
                                          (CExteralCharges.AccountNum.IsNull() ? true : patleftjoin.AccountNum.Equals(CExteralCharges.AccountNum)) &&
                                          (CExteralCharges.PaymentProcessed.IsNull() ? true : extChrg.PaymentProcessed.Equals(CExteralCharges.PaymentProcessed)) &&
                                          //(ExtensionMethods.IsBetweenDOS(CExteralCharges.DOSTO, CExteralCharges.DOSFROM, extChrg.DateOfService, extChrg.DateOfService)) &&
                                          (CExteralCharges.FileName.IsNull() ? true : extChrg.FileName.Contains(CExteralCharges.FileName, StringComparison.InvariantCultureIgnoreCase)) &&
                                          (CExteralCharges.ErrorMessage.IsNull() ? true :
                                          (extChrg.ErrorMessage.IsNull() ? false : extChrg.ErrorMessage.Contains(CExteralCharges.ErrorMessage, StringComparison.InvariantCultureIgnoreCase))) &&
                                          //(ExtensionMethods.IsBetweenDOS(CExteralCharges.EntryDateTo, CExteralCharges.EntryDateFrom, extChrg.AddedDate, extChrg.AddedDate)) &&
                                          (CExteralCharges.Status != null ? CExteralCharges.Status == "E" ? (!extChrg.ErrorMessage.IsNull() || extChrg.VisitID.IsNull()) : CExteralCharges.Status == "D" ? extChrg.MergeStatus == CExteralCharges.Status : true : true) &&
                                          ((CExteralCharges.EntryDateFrom != null && CExteralCharges.EntryDateTo != null) ? (extChrg.AddedDate != null ? extChrg.AddedDate.Date <= CExteralCharges.EntryDateTo.GetValueOrDefault().Date && extChrg.AddedDate.Date >= CExteralCharges.EntryDateFrom.GetValueOrDefault().Date : extChrg.AddedDate != null ? extChrg.AddedDate.Date >= CExteralCharges.EntryDateFrom.GetValueOrDefault() : false) : (CExteralCharges.EntryDateFrom != null ? (extChrg.AddedDate != null && CExteralCharges.EntryDateFrom.HasValue ? extChrg.AddedDate.Date >= CExteralCharges.EntryDateFrom.GetValueOrDefault() : true) : true)) &&
                                          ((CExteralCharges.DOSFROM != null && CExteralCharges.DOSTO != null) ? (extChrg.DateOfService != null ? extChrg.DateOfService.Date <= CExteralCharges.DOSTO.GetValueOrDefault().Date && extChrg.DateOfService.Date >= CExteralCharges.DOSFROM.GetValueOrDefault().Date : extChrg.DateOfService != null ? extChrg.DateOfService.Date >= CExteralCharges.DOSFROM.GetValueOrDefault() : false) : (CExteralCharges.DOSFROM != null ? (extChrg.DateOfService != null && CExteralCharges.DOSFROM.HasValue ? extChrg.DateOfService.Date >= CExteralCharges.DOSFROM.GetValueOrDefault() : true) : true)) &&
                                          (CExteralCharges.RecordType.IsNull() ? true : CExteralCharges.RecordType == "Regular" ? extChrg.IsRegularRecord == true : extChrg.IsRegularRecord != true)

                                          select new GExteralCharges()
                                          {
                                              ID = extChrg.ID,
                                              ExternalPatientID = extChrg.ExternalPatientID,
                                              ExternalPatientName = extChrg.LastName + ", " + extChrg.FirstName,
                                              AccountNum = patleftjoin.AccountNum,
                                              Provider = provjoinedleft.Name,
                                              InsuranceName = extChrg.InsuranceName,
                                              DOS = extChrg.DateOfService.Format("MM/dd/yyyy"),
                                              CPT = extChrg.CptCode,
                                              Modifiers = extChrg.Modifier1ID,
                                              POS = extChrg.POSCode,
                                              BilledAmount = extChrg.Balance,
                                              InsurancePayment = extChrg.InsurancePayment,
                                              PatientPayment = extChrg.PatientPayment,
                                              PatientID = patleftjoin.ID,
                                              FileName = extChrg.FileName,
                                              EntryDate = extChrg.AddedDate.Format("MM/dd/yyyy"),
                                              ProviderID = provjoinedleft.ID,
                                              Adjustments = extChrg.Adj,
                                              Charges = extChrg.Charges,
                                              PaymentProcessed = ConvertProcessCode(extChrg.PaymentProcessed),
                                              VisitID = extChrg.VisitID,
                                              ErrorMessage = extChrg.ErrorMessage,
                                              //ICD = icdleftjoin.ICDCode.IsNull() ? "" : icdleftjoin.ICDCode,
                                              ICD = extChrg.DiagnosisCode,
                                              PrescribingMD = extChrg.PrescribingMD,
                                              RecordType = extChrg.IsRegularRecord ? "Regular" : "Office Ally",
                                              Status = extChrg.MergeStatus != null ? extChrg.MergeStatus == "A" ? "Added" : extChrg.MergeStatus == "E" ? "External" : extChrg.MergeStatus == "D" ? "Duplicate" : "" : ""
                                          }).ToList();


            return data;
        }

        [Route("ResolvePatient/{id}/{value}")]
        [HttpGet("{id}/{value}")]
        public async Task<ActionResult<ExternalPatient>> ResolvePatient(long ID, bool value)
        {
            var external = await _context.ExternalPatient.FindAsync(ID);

            if (external != null)
            {
                external.resolve = value;
                external.ResolvedErrorMessage = external.MissingInfo;
                external.MissingInfo = string.Empty;
            }
            else
            {
                external.resolve = value;

            }
            _context.SaveChanges();

            return external;
        }
        [HttpPost]
        [Route("ExportCharges")]
        public async Task<IActionResult> ExportCharges(CExteralCharges CExteralCharges)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
           );

            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;
            List<GExteralCharges> data = FindExternalCharges(CExteralCharges, PracticeId);
            ExportController controller = new ExportController(_context);

            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CExteralCharges, "External Charge Report");

        }
        [HttpPost]
        [Route("ExportPdfCharges")]
        public async Task<IActionResult> ExportPdfCharges(CExteralCharges CExteralCharges)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GExteralCharges> data = FindExternalCharges(CExteralCharges, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }
        private string ConvertProcessCode(string Code)
        {
            if (Code.Equals("P"))
            {
                return "Yes";
            }
            else if (Code.Equals("NP"))
            {
                return "No";
            }
            else if (Code.Equals("F"))
            {
                return "Payment Not Received";
            }
            else
            {
                return "";
            }
        }

        [Route("ResolveExternalChargeRecrod/{id}/{value}")]
        [HttpGet("{id}/{value}")]
        public async Task<ActionResult<ExternalCharge>> ResolveExternalChargeRecrod(long ID, bool value)
        {
            var externalcharge = await _context.ExternalCharge.FindAsync(ID);

            if (externalcharge != null)
            {
                externalcharge.resolve = value;
                externalcharge.ResolvedErrorMessage = externalcharge.ErrorMessage;
                externalcharge.ErrorMessage = string.Empty;
            }
            else
            {
                externalcharge.resolve = value;

            }
            _context.SaveChanges();

            return externalcharge;
        }


        [HttpPost]
        [Route("AddPaymentChecks")]
        public async Task<ActionResult> AddPaymentChecks()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
                User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
                User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<ExternalCharge> SourceCharges = _context.ExternalCharge.Where(lamb => lamb.PracticeID == UD.PracticeID && lamb.VisitID != null && lamb.PaymentProcessed.Equals("NP") && lamb.MergeStatus.Equals("A")).OrderBy(lamb => lamb.GroupID).ToList();
            List<Visit> Visits = _context.Visit.Where(lamb => lamb.PracticeID == UD.PracticeID).ToList();
            Practice Practice = _context.Practice.Where(lamb => lamb.ID == UD.PracticeID).FirstOrDefault();
            List<List<ExternalCharge>> ExternalChargesByGroupId = new List<List<ExternalCharge>>();
            List<ExternalCharge> IntermediateList = new List<ExternalCharge>();
            List<VisitWithPatientPayment> PatientPayments = new List<VisitWithPatientPayment>();
            long Count = 0;
            foreach (ExternalCharge Temp in SourceCharges)
            {
                if (IntermediateList.Count() != 0)
                {
                    if (Temp.GroupID != IntermediateList.ElementAt(IntermediateList.Count() - 1).GroupID)
                    {
                        ExternalChargesByGroupId.Add(IntermediateList);
                        IntermediateList = new List<ExternalCharge>();
                        IntermediateList.Add(Temp);
                    }
                    else
                    {
                        IntermediateList.Add(Temp);
                    }
                }
                else
                {
                    if (Count == SourceCharges.Count - 1)
                    {
                        IntermediateList.Add(Temp);
                        ExternalChargesByGroupId.Add(IntermediateList);
                    }
                    else
                    {
                        IntermediateList.Add(Temp);
                    }
                }
                Count++;
            }
            List<PaymentCheckMapping> CompleteIdMapping = new List<PaymentCheckMapping>();
            List<PaymentCheckWithGroupNumber> PaymentChecksWithGroup = new List<PaymentCheckWithGroupNumber>();

            using (var transaction = _context.Database.BeginTransaction())
            {

                foreach (List<ExternalCharge> VisitCharges in ExternalChargesByGroupId)
                {
                    PaymentCheckMapping PaymentCheckMapping = new PaymentCheckMapping();
                    bool IsVisitValid = true;
                    decimal TotalBalance = 0, TotalPatientPayment = 0, TotalInsurancePayment = 0, TotalAdj = 0, TotalCharges = 0;
                    List<ExternalCharge> ChargesToAdd = new List<ExternalCharge>();
                    foreach (ExternalCharge TempExternalCharge in VisitCharges)
                    {
                        if (TempExternalCharge.Charges != TempExternalCharge.Balance)
                        {
                            ChargesToAdd.Add(TempExternalCharge);
                            TotalBalance = TotalBalance + TempExternalCharge.Balance;
                            TotalPatientPayment = TotalPatientPayment + TempExternalCharge.PatientPayment;
                            TotalInsurancePayment = TotalInsurancePayment + TempExternalCharge.InsurancePayment;
                            TotalAdj = TotalAdj + TempExternalCharge.Adj;
                            TotalCharges = TotalCharges + TempExternalCharge.Charges;
                        }
                        else
                        {
                            TempExternalCharge.PaymentProcessed = "F";
                            _context.ExternalCharge.Update(TempExternalCharge);
                        }
                    }
                    if (ChargesToAdd.Count > 0)
                    {
                        Visit SourceVisit = Visits.Where(lamb => lamb.ID == ChargesToAdd.FirstOrDefault().VisitID).FirstOrDefault();
                        PaymentCheck TempPaymentCheck = new PaymentCheck();
                        TempPaymentCheck.CheckNumber = "CHECK_" + SourceVisit.ID;
                        TempPaymentCheck.CheckDate = SourceVisit.DateOfServiceFrom;
                        TempPaymentCheck.CheckAmount = Math.Abs(TotalInsurancePayment);
                        TempPaymentCheck.Status = "NP";
                        TempPaymentCheck.PayeeName = Practice.Name;
                        TempPaymentCheck.PayerName = ChargesToAdd.FirstOrDefault().InsuranceName;
                        TempPaymentCheck.NumberOfPatients = 1;
                        TempPaymentCheck.NumberOfVisits = 1;
                        TempPaymentCheck.ReceiverID = 1;
                        TempPaymentCheck.AddedBy = UD.Email;
                        TempPaymentCheck.AddedDate = ChargesToAdd.FirstOrDefault().DateOfService;
                        TempPaymentCheck.PracticeID = UD.PracticeID;
                        long GroupId = ChargesToAdd.FirstOrDefault().GroupID;
                        _context.PaymentCheck.Add(TempPaymentCheck);
                        PaymentVisit TempPaymentVisit = new PaymentVisit();
                        TempPaymentVisit.PatientID = SourceVisit.PatientID;
                        TempPaymentVisit.VisitID = SourceVisit.ID;
                        if (SourceVisit.DateOfServiceFrom.HasValue)
                            TempPaymentVisit.AddedDate = SourceVisit.DateOfServiceFrom.Value;
                        TempPaymentVisit.WriteOffAmount = Math.Abs(TotalAdj);
                        TempPaymentVisit.BilledAmount = Math.Abs(TotalCharges);
                        TempPaymentVisit.PaidAmount = Math.Abs(TotalInsurancePayment);
                        TempPaymentVisit.ProcessedAs = "1";
                        TempPaymentVisit.ClaimStatementFromDate = SourceVisit.DateOfServiceFrom;
                        TempPaymentVisit.ClaimStatementToDate = SourceVisit.DateOfServiceTo;


                        VisitWithPatientPayment VPP = new VisitWithPatientPayment();
                        VPP.Visit = SourceVisit;
                        VPP.Check = TempPaymentCheck;

                        //if (TotalInsurancePayment > 0)
                        //{
                        TempPaymentVisit.PatientAmount = Math.Abs(TotalBalance) + Math.Abs(TotalPatientPayment);
                        //}
                        TempPaymentVisit.AllowedAmount = TotalInsurancePayment + TempPaymentVisit.PatientAmount.Val();
                        _context.PaymentVisit.Add(TempPaymentVisit);
                        List<PaymentCharge> TempPaymentCharges = new List<PaymentCharge>();
                        AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == "45").FirstOrDefault();

                        List<decimal> PatientPaymentValues = new List<decimal>();
                        foreach (ExternalCharge TempExternalCharge in ChargesToAdd)
                        {
                            PaymentCharge TempPaymentCharge = new PaymentCharge();
                            TempPaymentCharge.ChargeID = TempExternalCharge.ChargeID;
                            TempPaymentCharge.BilledAmount = Math.Abs(TempExternalCharge.Charges);
                            //if (TempExternalCharge.InsurancePayment > 0)
                            //{
                            TempPaymentCharge.PatientAmount = Math.Abs(TempExternalCharge.Balance) + Math.Abs(TempExternalCharge.PatientPayment);
                            //}
                            TempPaymentCharge.DOSFrom = TempExternalCharge.DateOfService;
                            TempPaymentCharge.DOSTo = TempExternalCharge.DateOfService;
                            TempPaymentCharge.Modifier1 = TempExternalCharge.Modifier1Code;
                            TempPaymentCharge.Modifier2 = TempExternalCharge.Modifier2Code;
                            TempPaymentCharge.Modifier3 = TempExternalCharge.Modifier3Code;
                            TempPaymentCharge.Modifier4 = TempExternalCharge.Modifier4Code;
                            TempPaymentCharge.CPTCode = TempExternalCharge.CptCode;
                            if (Math.Abs(TempPaymentCharge.PatientAmount.Val()) != 0)
                                TempPaymentCharge.DeductableAmount = Math.Abs(TempPaymentCharge.PatientAmount.Val());
                            TempPaymentCharge.AllowedAmount = Math.Abs(TempExternalCharge.InsurancePayment + TempPaymentCharge.PatientAmount.Val());
                            TempPaymentCharge.WriteoffAmount = Math.Abs(TempExternalCharge.Adj);
                            TempPaymentCharge.Units = TempExternalCharge.DaysOrUnits;
                            if (TempExternalCharge.PatientPayment > 0 && TempPaymentCharge.PatientAmount == TempExternalCharge.PatientPayment)
                                TempPaymentCharge.AppliedToSec = false;
                            else
                                TempPaymentCharge.AppliedToSec = true;
                            if (TempExternalCharge.PatientPayment > 0)
                            {
                                PatientPaymentValues.Add(TempExternalCharge.PatientPayment);
                            }
                            if (TempExternalCharge.Adj > 0)
                            {
                                TempPaymentCharge.AdjustmentCodeID1 = adjustmentCode?.ID;
                                TempPaymentCharge.AdjustmentAmount1 = Math.Abs(TempExternalCharge.Adj);
                                TempPaymentCharge.GroupCode1 = "CO";
                            }
                            TempPaymentCharge.PaidAmount = Math.Abs(TempExternalCharge.InsurancePayment);
                            _context.PaymentCharge.Add(TempPaymentCharge);
                            TempPaymentCharges.Add(TempPaymentCharge);
                        }

                        VPP.PatientPayments = PatientPaymentValues;

                        PatientPayments.Add(VPP);

                        PaymentCheckMapping.GroupID = GroupId;
                        PaymentCheckMapping.PaymentCheck = TempPaymentCheck;
                        PaymentCheckMapping.PaymentVisit = TempPaymentVisit;
                        PaymentCheckMapping.PaymentCharges = TempPaymentCharges;
                        PaymentCheckMapping.ExternalCharges = ChargesToAdd;
                        CompleteIdMapping.Add(PaymentCheckMapping);
                        foreach (ExternalCharge TempExternalCharge in ChargesToAdd)
                        {
                            TempExternalCharge.PaymentProcessed = "P";
                            _context.ExternalCharge.Update(TempExternalCharge);
                        }
                    }
                }
                _context.SaveChanges();
                transaction.Commit();
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                foreach (PaymentCheckMapping MappingItem in CompleteIdMapping)
                {
                    try
                    {
                        MappingItem.PaymentVisit.PaymentCheckID = MappingItem.PaymentCheck.ID;
                        _context.PaymentVisit.Update(MappingItem.PaymentVisit);
                        foreach (PaymentCharge AddedPaymentCharge in MappingItem.PaymentCharges)
                        {
                            AddedPaymentCharge.PaymentVisitID = MappingItem.PaymentVisit.ID;
                            _context.PaymentCharge.Update(AddedPaymentCharge);
                        }
                        foreach (ExternalCharge EChargeToLink in MappingItem.ExternalCharges)
                        {
                            EChargeToLink.PaymentCheckID = MappingItem.PaymentCheck.ID;
                            _context.ExternalCharge.Update(EChargeToLink);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.StackTrace);
                        return BadRequest("Something went wrong. Please contact BellMedEx");
                    }
                }
                _context.SaveChanges();
                transaction.Commit();
            }
            DateTime TimeBeforePosting = DateTime.Now;
            // long PostThreshold = long.Parse(_config["ClientSettings:ExternalPaymentPostingLimit"]);
            // List<long> ExternalChargesWithCheckIds = (from extChr in _context.ExternalCharge
            //                                           join check in _context.PaymentCheck on extChr.PaymentCheckID equals check.ID
            //                                           where extChr.PracticeID == UD.PracticeID && check.Status != "P"
            //                                           select extChr.PaymentCheckID.Value
            //                                                     ).OrderBy(lamb => lamb).Distinct().Take((int)PostThreshold).ToList();

            // //List<ExternalCharge> ExternalChargesWithCheckIds1 = _context.ExternalCharge.Where(lamb => !lamb.PaymentCheckID.IsNull() && lamb.PracticeID == UD.PracticeID).Distinct().Take((int)PostThreshold).ToList();
            // PaymentCheckController PaymentCheckController = new PaymentCheckController(_context, _contextMain);
            // PaymentCheckController.ControllerContext = this.ControllerContext;
            // foreach (long MappingItem in ExternalChargesWithCheckIds)
            // {
            //     try
            //     {
            //         ListModel model = new ListModel();
            //         model.Ids = new long[1];
            //         model.Ids[0] = MappingItem;
            //         Debug.WriteLine(await PaymentCheckController.PostEra(MappingItem));
            //     }
            //     catch (Exception ex)
            //     {
            //         Debug.WriteLine(ex.StackTrace);
            //         return BadRequest("Something went wrong. Please contact BellMedEx");
            //     }
            // }
            // DateTime TimeAfterPosting = DateTime.Now;
            //Debug.WriteLine("Total time for posting all checks :" + (TimeAfterPosting - TimeBeforePosting).TotalMinutes + ":" + (TimeAfterPosting - TimeBeforePosting).TotalSeconds);

            //foreach (VisitWithPatientPayment Temp in PatientPayments)
            //{
            //    foreach (decimal PaymentValue in Temp.PatientPayments)
            //    {
            //        Temp.Visit.PatientPayments.Add(new PatientPayment()
            //        {
            //            AddedBy = UD.Email,
            //            AddedDate = DateTime.Now,
            //            AllocatedAmount = null,
            //            CCTransactionID = null,
            //            CheckNumber = null,
            //            Description = null,
            //            PatientID = Temp.Visit.PatientID,
            //            PaymentAmount = PaymentValue,
            //            PaymentDate = Temp.Check.CheckDate,
            //            Status = "O",
            //            Type = "Other"
            //        });
            //    }
            //    VisitController VisitController = new VisitController(_context, _contextMain);
            //    VisitController.ControllerContext = this.ControllerContext;
            //    await VisitController.SaveVisit(Temp.Visit);
            //}

            return Ok();
        }
        [HttpPost]
        [Route("AddChargeData")]
        public async Task<ActionResult> AddChargeData(VMDataMigration InputData)
        {
            ReturnObject returnObject = new ReturnObject();
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
                User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
                User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long ClientID = UD.ClientID;
            int ChargesAdded = 0, GroupsAdded = 0, VisitsAdded = 0, ExternalChargesAdded = 0;

            List<Practice> PracticeData = _context.Practice.ToList();
            List<Provider> ProviderData = _context.Provider.ToList();
            List<Models.Location> LocationData = _context.Location.ToList();
            List<Patient> PatientData = _context.Patient.ToList();
            List<InsurancePlan> InsurancePlanData = _context.InsurancePlan.ToList();
            List<ExternalPatient> ExternalPatientData = _context.ExternalPatient.ToList();
            List<ExInsuranceMapping> ExInsuranceMappingData = _contextMain.ExInsuranceMapping.ToList();
            List<PatientPlan> PatientPlanData = _context.PatientPlan.ToList();
            List<ExternalCharge> ExternalChargeData = _context.ExternalCharge.ToList();
            List<POS> POSTable = _context.POS.ToList();
            List<Cpt> CptTable = _context.Cpt.ToList();
            List<Modifier> ModifierTable = _context.Modifier.ToList();
            ICD DummyIcd = _context.ICD.Where(lamb => lamb.ICDCode.Equals("99999999")).FirstOrDefault();
            if (DummyIcd == null)
            {
                DummyIcd = new ICD();
                DummyIcd.ICDCode = "99999999";
                DummyIcd.Description = "Dummy ICD";
                DummyIcd.AddedDate = DateTime.Now;
                DummyIcd.AddedBy = UD.Email;
                DummyIcd.IsActive = true;
                DummyIcd.IsDeleted = false;
                _context.ICD.Add(DummyIcd);
                _context.SaveChanges();
            }

            long ExternalChargeInitialID = 0, ExternalPatientIDAfterInsertion = 0;

            if (ExternalChargeData.Count() != 0)
            {
                ExternalChargeInitialID = ExternalChargeData.OrderBy(lamb => lamb.ID).ElementAt(ExternalChargeData.Count() - 1).ID;
            }

            byte[] bytes = Convert.FromBase64String(InputData.UploadModel.Content.Substring(InputData.UploadModel.Content.IndexOf("base64,") + 7));
            string FileName = InputData.UploadModel.Name;

            Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            string OutputPath = Path.Combine("\\\\", settings.DocumentServerURL,
                    settings.DocumentServerDirectory, "DataMigration", UD.ClientID + "",
                    DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));

            if (System.IO.Directory.Exists(OutputPath))
            {
                System.IO.File.WriteAllBytes(Path.Combine(OutputPath, FileName), bytes);
            }
            else
            {
                System.IO.Directory.CreateDirectory(OutputPath);
                System.IO.File.WriteAllBytes(Path.Combine(OutputPath, FileName), bytes);
            }

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                long practiceID;
                long providerID;
                long locationID;
                long InterPracticeID = 0;
                long InterProviderID = 0;
                long InterLocationID = 0;
                //Stream Stream = contents;

                practiceID = InputData.PracticeID;
                providerID = InputData.ProviderID;
                locationID = InputData.LocationID;

                if (InputData.PracticeID.IsNull())
                    return BadRequest("Practice id not present");
                if (InputData.ProviderID.IsNull())
                    return BadRequest("Provider id not present");
                if (InputData.LocationID.IsNull())
                    return BadRequest("Location id not present");



                //SaveStreamAsFile("D:\\", Stream, "streamedfile.xlsx");

                SpreadsheetDocument Doc = SpreadsheetDocument.Open(stream, false);

                WorkbookPart WorkbookPart = Doc.WorkbookPart;
                WorksheetPart WorksheetPart = WorkbookPart.WorksheetParts.First();
                Worksheet Sheet = WorksheetPart.Worksheet;

                SharedStringTablePart SstPart = WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();

                SharedStringTable Sst = SstPart.SharedStringTable;


                var AllRows = Sheet.Descendants<Row>();
                int SubPatientCount = 0;
                bool ChargesEnded = true;

                List<List<ExternalCharge>> FinalChargesData = new List<List<ExternalCharge>>();
                List<string> VisitAmounts = new List<string>();

                List<ExternalCharge> CurrentPatientCharges = new List<ExternalCharge>();
                ExternalCharge TempChargeWithPatientInfo = new ExternalCharge();
                List<POSWithExtCh> POSLinks = new List<POSWithExtCh>();

                decimal VisitCharge = 0, VisitBalance = 0, VisitAdj = 0, VisitPatientpayment = 0, VisitInsurancepayment = 0;

                long GroupID = 0;

                if (ExternalChargeData.OrderByDescending(lamb => lamb.GroupID).Count() != 0)
                {
                    GroupID = ExternalChargeData.OrderByDescending(lamb => lamb.GroupID).FirstOrDefault() != null ? ExternalChargeData.OrderByDescending(lamb => lamb.GroupID).FirstOrDefault().GroupID + 1 : 0;
                }

                List<PatientPlanWithPatient> PatientPlanMapping = new List<PatientPlanWithPatient>();
                for (int RowIndex = 6; RowIndex < AllRows.Count(); RowIndex++)
                {
                    try
                    {
                        bool IsChargeDataPresent = false;
                        List<Cell> RowCells = AllRows.ElementAt(RowIndex).Elements<Cell>().ToList();
                        bool SummationIdentifier = true;

                        ExternalCharge TempCharge = new ExternalCharge();

                        //Debug.Write("start of row");

                        for (int ColIndex = 0; ColIndex < RowCells.Count(); ColIndex++)
                        {
                            try
                            {
                                CellReferenceValuePair pair = GetValueFromCell(RowCells.ElementAt(ColIndex), Sst);

                                if (pair != null)
                                {
                                    //Debug.Write(pair.Reference+ "       "+pair.Value);
                                    string ReferenceAlphabets = new String(pair.Reference.Where(Char.IsLetter).ToArray());
                                    string FinalIdentifier = "";
                                    pair.Reference = new String(pair.Reference.Where(Char.IsLetter).ToArray());

                                    for (int StrIndex = 0; StrIndex < pair.Reference.Length; StrIndex++)
                                    {
                                        FinalIdentifier += GetColumnQualifier(pair.Reference[StrIndex]);
                                    }
                                    if (FinalIdentifier.Equals("5"))
                                    {
                                        if (pair.Value.Contains("Sub"))
                                        {
                                            SummationIdentifier = false;
                                        }
                                    }
                                    if (FinalIdentifier.Equals("0"))
                                    {
                                        if (pair.Value.Contains("Patient:", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            if (!pair.Value.Contains("Patient Count"))
                                            {
                                                if (CurrentPatientCharges.Count() != 0)
                                                {
                                                    FinalChargesData.Add(CurrentPatientCharges);
                                                    CurrentPatientCharges = new List<ExternalCharge>();
                                                }
                                                string TempAmount = VisitAdj + " " + VisitBalance + " " + VisitCharge + " " + VisitInsurancepayment + " " + VisitPatientpayment;
                                                VisitAmounts.Add(GroupID + " " + TempAmount);
                                                VisitAdj = 0;
                                                VisitBalance = 0;
                                                VisitCharge = 0;
                                                VisitInsurancepayment = 0;
                                                VisitPatientpayment = 0;
                                                SubPatientCount = 0;
                                                GroupID++;
                                                GroupsAdded++;
                                            }

                                        }
                                    }

                                    if (SubPatientCount == 0)
                                    {
                                        if (FinalIdentifier.Equals("0"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempChargeWithPatientInfo.FirstName = pair.Value.Split(',')[1].Trim().ToUpper();
                                            TempChargeWithPatientInfo.LastName = pair.Value.Split(',')[0].Trim().Split(":")[1].Trim().ToUpper();

                                        }
                                        if (FinalIdentifier.Equals("11"))
                                        {
                                            TempChargeWithPatientInfo.ExternalPatientID = pair.Value.Split(':')[1].Trim().ToUpper();
                                            Patient TempPatient = PatientData.Where(lamb => !lamb.ExternalPatientID.IsNull()).Where(lamb => lamb.ExternalPatientID.Trim().Equals(TempChargeWithPatientInfo.ExternalPatientID.Trim())).FirstOrDefault();
                                            if (TempPatient == null)
                                            {
                                                Debug.WriteLine("");
                                            }
                                            if (TempPatient != null)
                                                TempChargeWithPatientInfo.PatientID = TempPatient.ID;
                                            else
                                            {
                                                TempChargeWithPatientInfo.PatientID = PatientData.FirstOrDefault().ID;
                                                PatientPlan TempPrimaryPatientPlan = PatientPlanData.Where(lamb => lamb.PatientID.Equals(TempChargeWithPatientInfo.PatientID)).FirstOrDefault();
                                                if (TempPrimaryPatientPlan != null)
                                                {
                                                    TempChargeWithPatientInfo.PrimaryInsuredID = TempPrimaryPatientPlan.ID;
                                                }
                                                else
                                                {
                                                    if (TempCharge.ErrorMessage != null)
                                                    {
                                                        if (!TempCharge.ErrorMessage.Contains("Patient plan id not found"))
                                                            TempCharge.ErrorMessage = TempCharge.ErrorMessage + "(Patient plan id not found)";
                                                    }
                                                    else
                                                    {
                                                        TempCharge.ErrorMessage = "(Patient plan id not found)";
                                                    }
                                                }
                                            }
                                        }
                                        if (FinalIdentifier.Equals("15"))
                                        {
                                            TempChargeWithPatientInfo.Gender = pair.Value.Split(':')[1].Trim().ToUpper();
                                        }
                                        if (FinalIdentifier.Equals("19"))
                                        {
                                            TempChargeWithPatientInfo.DOB = DateTime.Parse(pair.Value, System.Globalization.CultureInfo.InvariantCulture).ToString(@"MM\/dd\/yyyy");
                                        }
                                    }
                                    if (SubPatientCount == 1)
                                    {
                                        if (FinalIdentifier.Equals("0"))
                                        {
                                            TempChargeWithPatientInfo.InsuranceName = pair.Value.Split(':')[1].Trim().ToUpper();
                                            string InsuranceNameFromSheet = pair.Value.Split(':')[1].Trim().ToUpper();
                                            InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(InsuranceNameFromSheet.Replace(" ", ""))).FirstOrDefault();
                                            if (TempInsurancePlan != null)
                                            {
                                                TempChargeWithPatientInfo.Insurance = TempInsurancePlan.ID;
                                            }
                                            else
                                            {
                                                ExInsuranceMapping MappingRecord = ExInsuranceMappingData.Where(lamb => lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(InsuranceNameFromSheet.Trim().ToUpper().Replace(" ", ""))).FirstOrDefault();
                                                if (MappingRecord != null)
                                                {
                                                    TempChargeWithPatientInfo.Insurance = MappingRecord.InsurancePlanID;
                                                }
                                                else
                                                {
                                                    TempChargeWithPatientInfo.Insurance = null;
                                                }
                                            }
                                        }
                                        if (FinalIdentifier.Equals("14"))
                                        {
                                            TempChargeWithPatientInfo.ProviderName = pair.Value;
                                            string ProviderNameFromSheet = pair.Value.Trim().ToUpper();
                                            Provider TempProvider = ProviderData.Where(lamb => lamb.Name.Trim().ToUpper().Replace(" ", "").Equals(ProviderNameFromSheet.Replace(" ", ""))).FirstOrDefault();
                                            if (TempProvider != null)
                                            {
                                                TempChargeWithPatientInfo.ProviderID = TempProvider.ID;
                                            }
                                        }
                                        if (FinalIdentifier.Equals("19"))
                                        {
                                            TempChargeWithPatientInfo.OfficeName = pair.Value.Trim().ToUpper();
                                            TempChargeWithPatientInfo.LocationID = locationID;
                                        }
                                    }
                                    if (SubPatientCount > 1)
                                    {
                                        #region resource assignment
                                        if (FinalIdentifier.Equals("2"))
                                        {
                                            IsChargeDataPresent = true;
                                            if (pair.Value.Contains("/") || pair.Value.Contains("-"))
                                            {
                                                TempCharge.DateOfService = DateTime.Parse(pair.Value, System.Globalization.CultureInfo.InvariantCulture);
                                            }
                                            else
                                            {
                                                TempCharge.DateOfService = DateTime.FromOADate(double.Parse(pair.Value));
                                            }

                                        }
                                        if (FinalIdentifier.Equals("4"))
                                        {
                                            string InterPosCode = pair.Value;
                                            if (long.Parse(pair.Value) < 10)
                                            {
                                                InterPosCode = "0" + InterPosCode;
                                            }
                                            POS MatchedPOS = POSTable.Where(lamb => lamb.PosCode.Equals(InterPosCode.Trim())).FirstOrDefault();

                                            if (MatchedPOS != null)
                                                TempCharge.POSID = MatchedPOS.ID;
                                            else
                                            {
                                                //POS POSTBA = new POS();
                                                //POSTBA.PosCode = InterPosCode;
                                                //POSTBA.AddedBy = UD.Email;
                                                //POSTBA.AddedDate = DateTime.Now;
                                                //POSTBA.Description = "pos with code " + InterPosCode;
                                                //POSTBA.Name = "";

                                                //POSWithExtCh Temp = new POSWithExtCh();
                                                //Temp.ExCh = TempCharge;
                                                //Temp.POS = POSTBA;
                                                //_context.POS.Add(Temp.POS);

                                                //POSLinks.Add(Temp);
                                            }
                                            TempCharge.POSCode = InterPosCode;
                                        }
                                        if (FinalIdentifier.Equals("5"))
                                        {
                                            if (pair.Value.Equals("Sub Total:"))
                                            {
                                                SummationIdentifier = false;
                                            }
                                            Cpt MatchedCpt = CptTable.Where(lamb => lamb.CPTCode.Equals(pair.Value.Trim())).FirstOrDefault();
                                            if (MatchedCpt != null)
                                                TempCharge.CPTID = MatchedCpt.ID;
                                            TempCharge.CptCode = pair.Value;
                                        }
                                        if (FinalIdentifier.Equals("8"))
                                        {
                                            Modifier MatchedModifier = ModifierTable.Where(lamb => lamb.Code.Equals(pair.Value)).FirstOrDefault();
                                            if (MatchedModifier != null)
                                                TempCharge.Modifier1ID = MatchedModifier.ID;
                                            TempCharge.Modifier1Code = pair.Value.Trim().ToUpper();
                                        }
                                        if (FinalIdentifier.Equals("9"))
                                        {
                                            Modifier MatchedModifier = ModifierTable.Where(lamb => lamb.Code.Equals(pair.Value)).FirstOrDefault();
                                            if (MatchedModifier != null)
                                                TempCharge.Modifier2ID = MatchedModifier.ID;
                                            TempCharge.Modifier2Code = pair.Value.Trim().ToUpper();
                                        }
                                        if (FinalIdentifier.Equals("10"))
                                        {
                                            Modifier MatchedModifier = ModifierTable.Where(lamb => lamb.Code.Equals(pair.Value)).FirstOrDefault();
                                            if (MatchedModifier != null)
                                                TempCharge.Modifier3ID = MatchedModifier.ID;
                                            TempCharge.Modifier3Code = pair.Value.Trim().ToUpper();
                                        }
                                        if (FinalIdentifier.Equals("11"))
                                        {
                                            Modifier MatchedModifier = ModifierTable.Where(lamb => lamb.Code.Equals(pair.Value)).FirstOrDefault();
                                            if (MatchedModifier != null)
                                                TempCharge.Modifier4ID = MatchedModifier.ID;
                                            TempCharge.Modifier4Code = pair.Value.Trim().ToUpper();
                                            //Debug.WriteLine(pair.Value);
                                        }
                                        if (FinalIdentifier.Equals("12"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.DaysOrUnits = pair.Value;
                                        }
                                        if (FinalIdentifier.Equals("13"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.Charges = Math.Abs(decimal.Parse(pair.Value));

                                        }
                                        if (FinalIdentifier.Equals("14"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.InsurancePayment = Math.Abs(decimal.Parse(pair.Value));

                                        }
                                        if (FinalIdentifier.Equals("15"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.PatientPayment = Math.Abs(decimal.Parse(pair.Value));

                                        }
                                        if (FinalIdentifier.Equals("17"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.Adj = Math.Abs(decimal.Parse(pair.Value));

                                        }
                                        if (FinalIdentifier.Equals("19"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.Balance = Math.Abs(decimal.Parse(pair.Value));

                                        }
                                        #endregion
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                return BadRequest("Unexpected Error Occurred. Please Contect BellMedEx\n" + ex.Message + "\n" + ex.StackTrace);
                            }
                        }

                        // Debug.WriteLine("end of row");

                        if (SummationIdentifier)
                        {
                            VisitCharge += TempCharge.Charges;
                            VisitInsurancepayment += TempCharge.InsurancePayment;
                            VisitPatientpayment += TempCharge.PatientPayment;
                            VisitAdj += TempCharge.Adj;
                            VisitBalance += TempCharge.Balance;
                        }

                        if (!TempCharge.DateOfService.IsNull())
                        {
                            if (TempChargeWithPatientInfo.LocationID != 0)
                            {
                                TempCharge.LocationID = TempChargeWithPatientInfo.LocationID;
                            }
                            else
                            {
                                TempCharge.LocationID = locationID;
                            }
                            TempCharge.PracticeID = practiceID;
                            if (TempChargeWithPatientInfo.ProviderID != 0)
                            {
                                TempCharge.ProviderID = TempChargeWithPatientInfo.ProviderID;
                            }
                            else
                            {
                                TempCharge.ProviderID = providerID;
                            }

                            if (TempCharge.CPTID == 0)
                            {
                                TempCharge.CPTID = null;
                                if (TempCharge.ErrorMessage != null)
                                {
                                    if (!TempCharge.ErrorMessage.Contains("invalid cpt"))
                                        TempCharge.ErrorMessage = TempCharge.ErrorMessage + "(invalid cpt)";
                                }
                                else
                                {
                                    TempCharge.ErrorMessage = "(invalid cpt)";
                                }
                            }
                            else if (TempCharge.CPTID.IsNull())
                            {
                                if (TempCharge.ErrorMessage != null)
                                {
                                    if (!TempCharge.ErrorMessage.Contains("invalid cpt"))
                                        TempCharge.ErrorMessage = TempCharge.ErrorMessage + "(invalid cpt)";
                                }
                                else
                                {
                                    TempCharge.ErrorMessage = "(invalid cpt)";
                                }
                            }
                            TempCharge.FirstName = TempChargeWithPatientInfo.FirstName;
                            TempCharge.LastName = TempChargeWithPatientInfo.LastName;
                            TempCharge.Provider = TempChargeWithPatientInfo.Provider;
                            TempCharge.OfficeName = TempChargeWithPatientInfo.OfficeName;
                            TempCharge.InsuranceName = TempChargeWithPatientInfo.InsuranceName;
                            TempCharge.Gender = TempChargeWithPatientInfo.Gender;
                            TempCharge.PatientID = TempChargeWithPatientInfo.PatientID;
                            TempCharge.ExternalPatientID = TempChargeWithPatientInfo.ExternalPatientID;
                            TempCharge.DOB = TempChargeWithPatientInfo.DOB;
                            TempCharge.MergeStatus = "E";
                            TempCharge.AddedDate = TempCharge.DateOfService;
                            TempCharge.AddedBy = UD.Email;
                            TempCharge.GroupID = GroupID;
                            TempCharge.FileName = FileName;
                            TempCharge.PaymentProcessed = "NP";
                            TempCharge.SubmittetdDate = TempCharge.DateOfService;

                            //Debug.WriteLine(TempCharge.CptCode+" with id "+TempCharge.CPTID);
                            CurrentPatientCharges.Add(TempCharge);
                            _context.ExternalCharge.Add(TempCharge);
                            ExternalChargesAdded++;
                        }
                        if (SubPatientCount == 0)
                        {
                            SubPatientCount++;
                        }
                        else if (SubPatientCount == 1)
                        {
                            SubPatientCount++;
                        }
                        else
                            SubPatientCount++;
                        if (RowIndex == AllRows.Count() - 2)
                        {
                            FinalChargesData.Add(CurrentPatientCharges);
                            CurrentPatientCharges = new List<ExternalCharge>();
                            string TempAmount = VisitAdj + " " + VisitBalance + " " + VisitCharge + " " + VisitInsurancepayment + " " + VisitPatientpayment;
                            VisitAmounts.Add(GroupID + " " + TempAmount);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Unexpected Error Occurred. Please Contect BellMedEx\n" + ex.Message + "\n" + ex.StackTrace);
                    }
                }
                _context.SaveChanges();
                POSTable = _context.POS.ToList();
                foreach (POSWithExtCh POSWithExtCh in POSLinks)
                {
                    POSWithExtCh.ExCh.POSID = POSWithExtCh.POS.ID;
                    _context.ExternalCharge.Update(POSWithExtCh.ExCh);
                }

                _context.SaveChanges();

                List<PatientPlan> AddedPatientPlans = new List<PatientPlan>();
                List<PatientPlanWithID> PatientPlanIdsToUpdate = new List<PatientPlanWithID>();
                foreach (List<ExternalCharge> Temp in FinalChargesData)
                {
                    if (Temp.ElementAt(0).PatientID.GetValueOrDefault() != 0)
                    {
                        long patientID = Temp.ElementAt(0).PatientID.GetValueOrDefault();
                        Patient TempPatient = PatientData.Where(p => p.ID == patientID).FirstOrDefault();
                        ExternalPatient TempExternalPatient = ExternalPatientData.Where(ep => ep.ExternalPatientID == TempPatient.ExternalPatientID).FirstOrDefault();
                        PatientPlan TempPrimaryPatientPlan = PatientPlanData.Where(lamb => lamb.PatientID.Equals(patientID)).FirstOrDefault();
                        if (TempPrimaryPatientPlan == null)
                        {
                            #region patient plan code
                            PatientPlan TempPatientPlan = new PatientPlan();
                            TempPatientPlan.FirstName = TempPatient.FirstName;
                            TempPatientPlan.LastName = TempPatient.LastName;
                            TempPatientPlan.PatientID = TempPatient.ID;
                            TempPatientPlan.DOB = TempPatient.DOB;
                            TempPatientPlan.Gender = TempPatient.Gender;
                            TempPatientPlan.Email = TempPatient.Email;
                            TempPatientPlan.Address1 = TempPatient.Address1;
                            TempPatientPlan.City = TempPatient.City;
                            TempPatientPlan.State = TempPatient.State;
                            TempPatientPlan.ZipCode = TempPatient.ZipCode;
                            TempPatientPlan.PhoneNumber = TempPatient.PhoneNumber;
                            TempPatientPlan.SubscriberId = TempExternalPatient.PrimaryInsuredID;
                            TempPatientPlan.Coverage = "P";
                            TempPatientPlan.IsActive = true;
                            TempPatientPlan.IsDeleted = false;
                            TempPatientPlan.AddedDate = DateTime.Today.Date;
                            TempPatientPlan.AddedBy = UD == null ? "" : UD.Email.ToUpper();
                            TempPatientPlan.RelationShip = 18 + "";
                            string TempString = Temp.ElementAt(0).InsuranceName;
                            InsurancePlan PossibleCandidate = _context.InsurancePlan.Where(lamb => lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(TempString.Trim().ToUpper().Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                            if (PossibleCandidate != null)
                            {
                                TempPatientPlan.InsurancePlanID = PossibleCandidate.ID;
                            }
                            else
                            {
                                ExInsuranceMapping MappingRecord = _contextMain.ExInsuranceMapping.Where(lamb => lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(TempString.Trim().ToUpper().Replace(" ", ""))).FirstOrDefault();
                                if (MappingRecord != null)
                                {
                                    InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == MappingRecord.InsurancePlanID).FirstOrDefault();
                                    if (TempInsurancePlan != null)
                                    {
                                        TempPatientPlan.InsurancePlanID = MappingRecord.InsurancePlanID;
                                    }
                                }
                                else
                                {
                                    TempPatientPlan.InsurancePlanID = null;
                                }
                            }
                            if (TempPatientPlan.InsurancePlanID != null)
                            {
                                bool PatientPlanPresent = AddedPatientPlans.Any(pp => pp.PatientID == TempPatientPlan.PatientID && pp.InsurancePlanID == TempPatientPlan.InsurancePlanID && pp.Coverage == "P");
                                if (!PatientPlanPresent)
                                {
                                    AddedPatientPlans.Add(TempPatientPlan);
                                    _context.PatientPlan.Add(TempPatientPlan);
                                    PatientPlanWithID TempObj = new PatientPlanWithID();
                                    TempObj.PatientPlan = TempPatientPlan;
                                    TempObj.AccountNumber = TempExternalPatient.ID + "//" + Temp.ElementAt(0).InsuranceName;
                                    PatientPlanIdsToUpdate.Add(TempObj);
                                    foreach (ExternalCharge T in Temp)
                                    {
                                        if (T.ErrorMessage != null)
                                        {
                                            T.ErrorMessage = T.ErrorMessage.Replace("(Patient plan id not found)", "");
                                        }
                                        _context.ExternalCharge.Update(T);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                //_context.SaveChanges();
                //foreach (PatientPlanWithID s in PatientPlanIdsToUpdate)
                //{
                //    long ExternalPatientID = long.Parse(s.AccountNumber.Split(new string[] { "//" }, StringSplitOptions.None)[0]);
                //    long PatientPlanID = s.PatientPlan.ID;
                //    string InsuranceName = s.AccountNumber.Split(new string[] { "//" }, StringSplitOptions.None)[1];
                //    ExternalPatient Temp = ExternalPatientData.Where(ep => ep.ID == ExternalPatientID).FirstOrDefault();
                //    if (Temp != null)
                //    {
                //        Temp.PrimaryPatientPlanID = PatientPlanID;
                //        Temp.PrimaryInsurance = InsuranceName;
                //        _context.ExternalPatient.Update(Temp);
                //    }
                //}
                _context.SaveChanges();
                PatientPlanData = _context.PatientPlan.ToList();
                ExternalChargeData = _context.ExternalCharge.Where(x => x.PracticeID == UD.PracticeID).ToList();
                if (ExternalChargeData.Count() != 0)
                    ExternalPatientIDAfterInsertion = ExternalChargeData.OrderBy(lamb => lamb.ID).ElementAt(ExternalChargeData.Count() - 1).ID;
                if (ExternalChargeData.Count() != 0)
                    ExternalChargeData = _context.ExternalCharge.Where(x => x.PracticeID == UD.PracticeID && x.ID > ExternalChargeInitialID && x.ID <= ExternalPatientIDAfterInsertion).ToList();
                else
                    ExternalChargeData = _context.ExternalCharge.Where(x => x.PracticeID == UD.PracticeID).ToList();


                List<VisitWithGroupNumber> VisitChargeLinks = new List<VisitWithGroupNumber>();
                List<ChargeWithGroupNumber> ChargeExternalChargeLinks = new List<ChargeWithGroupNumber>();
                foreach (List<ExternalCharge> Temp in FinalChargesData)
                {
                    bool VisitIsValid = true;
                    foreach (ExternalCharge TempCharge in Temp)
                    {
                        if (!TempCharge.ErrorMessage.IsNull())
                        {
                            if (!TempCharge.ErrorMessage.Equals(""))
                            {
                                VisitIsValid = false;
                            }
                        }
                        else
                        {

                        }

                    }

                    if (VisitIsValid)
                    {
                        ExternalCharge FirstCharge = Temp.ElementAt(0);
                        Visit TempVisit = new Visit();

                        TempVisit.ClientID = ClientID;
                        TempVisit.PracticeID = UD.PracticeID;
                        //TempVisit.LocationID = FirstCharge.LocationID;
                        //TempVisit.ProviderID = FirstCharge.ProviderID;
                        TempVisit.DateOfServiceFrom = FirstCharge.DateOfService;
                        TempVisit.DateOfServiceTo = FirstCharge.DateOfService;
                        if (FirstCharge.PatientID.GetValueOrDefault() != 0)
                        {
                            TempVisit.PatientID = FirstCharge.PatientID.GetValueOrDefault();
                        }

                        if (FirstCharge.POSID.GetValueOrDefault() != 0)
                        {
                            TempVisit.POSID = FirstCharge.POSID.GetValueOrDefault();
                        }

                        if (FirstCharge.ProviderID.IsNull())
                        {
                            TempVisit.ProviderID = providerID;
                        }
                        else
                        {
                            TempVisit.ProviderID = FirstCharge.ProviderID;
                        }

                        if (FirstCharge.LocationID.IsNull())
                        {
                            TempVisit.LocationID = locationID;
                        }
                        else
                        {
                            TempVisit.LocationID = FirstCharge.LocationID;
                        }

                        if (FirstCharge.PatientID.GetValueOrDefault() != 0)
                        {
                            TempVisit.PatientID = FirstCharge.PatientID.GetValueOrDefault();
                            Patient TempPatient = PatientData.Where(p => p.ID == TempVisit.PatientID).FirstOrDefault();
                            ExternalPatient TempExternalPatient = ExternalPatientData.Where(ep => ep.ExternalPatientID == TempPatient.ExternalPatientID).FirstOrDefault();
                            PatientPlan TempPrimaryPatientPlan = PatientPlanData.Where(lamb => lamb.PatientID.Equals(TempVisit.PatientID)).FirstOrDefault();
                            if (TempPrimaryPatientPlan != null)
                            {
                                InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                if (TempInsurancePlan != null)
                                {
                                    if (FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                    {
                                        TempVisit.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                    }
                                }
                                else
                                {
                                    ExInsuranceMapping TempExInsuranceMapping = _contextMain.ExInsuranceMapping.Where(lamb1 => lamb1.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                    if (TempExInsuranceMapping != null)
                                    {
                                        InsurancePlan TempInsurancePlan2 = InsurancePlanData.Where(lamb => lamb.ID == TempExInsuranceMapping.InsurancePlanID).FirstOrDefault();
                                        if (TempInsurancePlan2 != null)
                                        {
                                            if (FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                            {
                                                TempVisit.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        TempVisit.ICD1ID = DummyIcd.ID;
                        TempVisit.OutsideReferral = false;
                        TempVisit.AddedDate = FirstCharge.DateOfService;
                        TempVisit.AddedBy = UD.Email;

                        string VisitAmount = VisitAmounts.Where(lamb => lamb.Split(' ')[0].Equals(FirstCharge.GroupID.ToString())).FirstOrDefault();
                        TempVisit.PrimaryBal = decimal.Parse(VisitAmount.Split(' ')[3]);
                        TempVisit.PrimaryBilledAmount = decimal.Parse(VisitAmount.Split(' ')[3]);
                        TempVisit.TotalAmount = decimal.Parse(VisitAmount.Split(' ')[3]);
                        TempVisit.PrimaryStatus = "S";
                        TempVisit.IsDontPrint = false;
                        TempVisit.IsForcePaper = false;
                        TempVisit.IsSubmitted = true;
                        TempVisit.SubmittedDate = FirstCharge.SubmittetdDate;
                        TempVisit.IsReversalApplied = false;
                        if (TempVisit.PrimaryPatientPlanID != null)
                        {
                            _context.Visit.Add(TempVisit);
                            VisitsAdded++;

                            VisitWithGroupNumber temp = new VisitWithGroupNumber();
                            temp.Visit = TempVisit;
                            temp.GroupNumber = FirstCharge.GroupID;
                            VisitChargeLinks.Add(temp);
                        }
                        else
                        {
                            foreach (ExternalCharge TempCharge in Temp)
                            {
                                if (TempCharge.ErrorMessage != null)
                                {
                                    if (!TempCharge.ErrorMessage.Contains("Patient plan id not found"))
                                        TempCharge.ErrorMessage = TempCharge.ErrorMessage + "(Patient plan id not found)";
                                }
                                else
                                {
                                    TempCharge.ErrorMessage = "(Patient plan id not found)";
                                }
                                _context.ExternalCharge.Update(TempCharge);
                            }
                        }
                    }
                }

                _context.SaveChanges();

                List<List<Charge>> AddedValidCharges = new List<List<Charge>>();

                foreach (List<ExternalCharge> Temp in FinalChargesData)
                {
                    bool ValidCharges = true;

                    List<Charge> ListToBeAdded = new List<Charge>();
                    long CurrentGroupID = Temp.FirstOrDefault().GroupID;

                    foreach (ExternalCharge TempCharge in Temp)
                    {
                        if (!TempCharge.ErrorMessage.IsNull())
                        {
                            if (!TempCharge.ErrorMessage.Equals(""))
                            {
                                ValidCharges = false;
                            }
                        }
                        else
                        {
                            Charge ActualCharge = new Charge();
                            ActualCharge.CPTID = TempCharge.CPTID.GetValueOrDefault();
                            ActualCharge.POSID = TempCharge.POSID.GetValueOrDefault();
                            ActualCharge.ClientID = UD.ClientID;
                            ActualCharge.PracticeID = UD.PracticeID;
                            ActualCharge.LocationID = locationID;
                            ActualCharge.ProviderID = providerID;
                            if (TempCharge.PatientID.GetValueOrDefault() != 0)
                            {
                                ActualCharge.PatientID = TempCharge.PatientID.GetValueOrDefault();
                                PatientPlan TempPrimaryPatientPlan = PatientPlanData.Where(lamb => lamb.PatientID.Equals(ActualCharge.PatientID)).FirstOrDefault();
                                if (TempPrimaryPatientPlan != null)
                                {
                                    InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                    if (TempInsurancePlan != null)
                                    {
                                        if (TempCharge.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(TempCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                        {
                                            ActualCharge.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                        }
                                    }
                                    else
                                    {
                                        ExInsuranceMapping TempExInsuranceMapping = _contextMain.ExInsuranceMapping.Where(lamb1 => lamb1.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                        if (TempExInsuranceMapping != null)
                                        {
                                            InsurancePlan TempInsurancePlan2 = InsurancePlanData.Where(lamb => lamb.ID == TempExInsuranceMapping.InsurancePlanID).FirstOrDefault();
                                            if (TempInsurancePlan2 != null)
                                            {
                                                if (TempCharge.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(TempCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                                {
                                                    ActualCharge.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            ActualCharge.Modifier1ID = TempCharge.Modifier1ID;
                            ActualCharge.Modifier2ID = TempCharge.Modifier2ID;
                            ActualCharge.Modifier3ID = TempCharge.Modifier3ID;
                            ActualCharge.Modifier4ID = TempCharge.Modifier4ID;


                            ActualCharge.Units = TempCharge.DaysOrUnits;
                            ActualCharge.DateOfServiceFrom = TempCharge.DateOfService;
                            ActualCharge.DateOfServiceTo = TempCharge.DateOfService;
                            ActualCharge.CPTID = TempCharge.CPTID.GetValueOrDefault();
                            ActualCharge.CPTID = TempCharge.CPTID.GetValueOrDefault();
                            ActualCharge.TotalAmount = TempCharge.Charges;
                            ActualCharge.PrimaryBilledAmount = TempCharge.Charges;
                            ActualCharge.PrimaryBal = TempCharge.Charges;
                            ActualCharge.IsSubmitted = true;
                            ActualCharge.PrimaryStatus = "S";
                            ActualCharge.SubmittetdDate = ActualCharge.DateOfServiceFrom;
                            ActualCharge.Pointer1 = "1";
                            ActualCharge.Pointer2 = "";
                            ActualCharge.Pointer3 = "";
                            ActualCharge.Pointer4 = "";
                            ActualCharge.AddedDate = TempCharge.DateOfService;
                            ActualCharge.AddedBy = UD.Email;
                            ActualCharge.IsReversalApplied = false;
                            if (ActualCharge.PrimaryPatientPlanID != null)
                            {
                                ListToBeAdded.Add(ActualCharge);
                            }
                            else
                            {
                                if (TempCharge.ErrorMessage != null)
                                {
                                    if (!TempCharge.ErrorMessage.Contains("Patient plan id not found"))
                                        TempCharge.ErrorMessage = TempCharge.ErrorMessage + "(Patient plan id not found)";
                                }
                                else
                                {
                                    TempCharge.ErrorMessage = "(Patient plan id not found)";
                                }
                                _context.ExternalCharge.Update(TempCharge);
                            }

                        }
                    }

                    AddedValidCharges.Add(ListToBeAdded);
                    if (ValidCharges)
                    {
                        Visit TempVisit = VisitChargeLinks.Where(lamb => lamb.GroupNumber.Equals(CurrentGroupID)).FirstOrDefault().Visit;
                        foreach (Charge ChargeToBeAdded in ListToBeAdded)
                        {



                            if (TempVisit != null)
                            {
                                ChargeToBeAdded.VisitID = TempVisit.ID;
                                ExternalCharge TempExternalCharge = ExternalChargeData.Where(lamb => lamb.GroupID.Equals(CurrentGroupID)).Where(
                                                                                                                                                        lamb => lamb.CPTID.Equals(ChargeToBeAdded.CPTID) &&
                                                                                                                                                              lamb.POSID.Equals(ChargeToBeAdded.POSID) &&
                                                                                                                                                              lamb.Charges.Equals(ChargeToBeAdded.PrimaryBal.GetValueOrDefault()) &&
                                                                                                                                                              lamb.Charges.Equals(ChargeToBeAdded.PrimaryBilledAmount.GetValueOrDefault()) &&
                                                                                                                                                              lamb.Charges.Equals(ChargeToBeAdded.TotalAmount)
                                                                                                                                                    ).FirstOrDefault();
                                TempExternalCharge.VisitID = TempVisit.ID;

                                ChargeWithGroupNumber TempLinkingClass = new ChargeWithGroupNumber();
                                TempLinkingClass.Charge = ChargeToBeAdded;
                                TempLinkingClass.IdentifierID = TempExternalCharge.ID;
                                ChargeExternalChargeLinks.Add(TempLinkingClass);
                                ChargeToBeAdded.IsDontPrint = false;
                                _context.ExternalCharge.Update(TempExternalCharge);
                                _context.Charge.Add(ChargeToBeAdded);
                                ChargesAdded++;
                            }
                            else
                            {
                            }
                        }
                        Notes note = new Notes();
                        note.PracticeID = UD.PracticeID;
                        note.Note = "Data Migrated from OA, filename: " + FileName;
                        note.AddedBy = UD.Email;
                        note.AddedDate = DateTime.Now;
                        note.NotesDate = DateTime.Now;
                        note.VisitID = TempVisit.ID;
                        _context.Notes.Add(note);
                    }

                }
                _context.SaveChanges();
                foreach (ChargeWithGroupNumber Temp in ChargeExternalChargeLinks)
                {
                    ExternalCharge ExternalChargeToBeUpdated = ExternalChargeData.Where(lamb => lamb.ID == Temp.IdentifierID).FirstOrDefault();
                    ExternalChargeToBeUpdated.ChargeID = Temp.Charge.ID;
                    ExternalChargeToBeUpdated.VisitID = Temp.Charge.VisitID;
                    if (Temp.Charge.PrimaryPatientPlanID.HasValue)
                    {
                        ExternalChargeToBeUpdated.PrimaryInsuredID = Temp.Charge.PrimaryPatientPlanID.Value;
                    }
                    ExternalChargeToBeUpdated.MergeStatus = "A";
                    _context.ExternalCharge.Update(ExternalChargeToBeUpdated);
                }
                _context.SaveChanges();
                Debug.WriteLine("external charges added: " + ExternalChargesAdded + ", visits added: " + VisitsAdded + ", charges added: " + ChargesAdded + ", groups added: " + GroupsAdded);
                return Ok(returnObject);
            }

        }
        [HttpPost]
        [Route("ProcessCharges")]
        public async Task<ActionResult> ProcessCharges(ListModel model)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
                User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
                User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            List<Patient> PatientData = _context.Patient.ToList();
            List<InsurancePlan> InsurancePlanData = _context.InsurancePlan.ToList();
            List<ICD> ICDData = _context.ICD.ToList();
            List<ExternalPatient> ExternalPatientData = _context.ExternalPatient.ToList();
            List<ExInsuranceMapping> ExInsuranceMappingData = _contextMain.ExInsuranceMapping.ToList();
            List<PlanType> PlanTypeData = _context.PlanType.ToList();
            List<RefProvider> RefProviderData = _context.RefProvider.ToList();
            List<PatientPlan> PatientPlanData = _context.PatientPlan.ToList();
            //List<ExternalCharge> ExternalChargeData = _context.ExternalCharge.ToList();

            List<ExternalCharge> UnprocessedCharges = null;

            if (model != null && model.Ids != null && model.Ids.Length > 0)
            {
                UnprocessedCharges = _context.ExternalCharge.
                Where(exCh => exCh.VisitID.IsNull() && exCh.resolve == false && exCh.MergeStatus != "D"
                && exCh.IsRegularRecord == true && model.Ids.Contains(exCh.ID, new VisitComparer())
                ).OrderBy(exCh => exCh.GroupID).ToList();
            }
            else
            {
                UnprocessedCharges = _context.ExternalCharge.
                Where(exCh => exCh.VisitID.IsNull() && exCh.resolve == false && exCh.MergeStatus != "D"
                && exCh.IsRegularRecord == true
                ).OrderBy(exCh => exCh.GroupID).ToList();
            }
           
            List<List<ExternalCharge>> UnprocessedChargesByVisit = new List<List<ExternalCharge>>();

            ICD DummyIcd = _context.ICD.Where(lamb => lamb.ICDCode.Equals("99999999")).FirstOrDefault();
            if (DummyIcd == null)
            {
                DummyIcd = new ICD();
                DummyIcd.ICDCode = "99999999";
                DummyIcd.Description = "Dummy ICD";
                DummyIcd.AddedDate = DateTime.Now;
                DummyIcd.AddedBy = UD.Email;
                DummyIcd.IsActive = true;
                DummyIcd.IsDeleted = false;
                _context.ICD.Add(DummyIcd);
                _context.SaveChanges();
            }
            if (UnprocessedCharges.Count > 0)
            {
                long previousGroupID = UnprocessedCharges.FirstOrDefault().GroupID;
                ExternalCharge PreviousCharge = UnprocessedCharges.FirstOrDefault();

                List<ExternalCharge> TempCharges = new List<ExternalCharge>();
                foreach (ExternalCharge Temp in UnprocessedCharges)
                {
                    if (Temp.GroupID == 0)
                    {
                        if (Temp.InsuranceName != null && Temp.InsuranceName != "" && Temp.PatientID != null && Temp.DateOfService != null)
                        {
                            if (Temp.InsuranceName != PreviousCharge.InsuranceName && Temp.PatientID != PreviousCharge.PatientID && Temp.DateOfService != PreviousCharge.DateOfService)
                            {
                                UnprocessedChargesByVisit.Add(TempCharges);
                                TempCharges = new List<ExternalCharge>();
                                TempCharges.Add(Temp);
                            }
                            else
                            {
                                TempCharges.Add(Temp);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (previousGroupID != Temp.GroupID)
                        {
                            UnprocessedChargesByVisit.Add(TempCharges);
                            TempCharges = new List<ExternalCharge>();
                            TempCharges.Add(Temp);
                        }
                        else
                        {
                            TempCharges.Add(Temp);
                        }
                    }
                    previousGroupID = Temp.GroupID;
                    PreviousCharge = Temp;
                }
                if (TempCharges.Count() > 0)
                {
                    UnprocessedChargesByVisit.Add(TempCharges);
                }
                List<VisitData> VisitsToBeLinked = new List<VisitData>();
                List<string> AddedCpts = new List<string>(), AddedPos = new List<string>(), AddedPlans = new List<string>();

                int ccc = 0;
                foreach (List<ExternalCharge> VisitChargesForAddition in UnprocessedChargesByVisit)
                {
                    try
                    {
                        ccc += 1;
                        if (VisitChargesForAddition.Count > 0)
                        {
                            ExternalCharge FirstCharge = VisitChargesForAddition.FirstOrDefault();
                            if (FirstCharge.PatientID.IsNull())
                            {
                                var pat = (from p in _context.Patient
                                          join ep in _context.ExternalPatient
                                          on p.AccountNum equals ep.AccountNum
                                          where ep.ExternalPatientID == FirstCharge.ExternalPatientID
                                          select p
                                          ).FirstOrDefault();

                                if (pat != null)
                                {
                                    FirstCharge.PatientID = pat.ID;
                                    _context.ExternalCharge.Update(FirstCharge);
                                }
                            }

                            if (FirstCharge.PatientID.IsNull())
                                continue;


                            bool PlanLinked = false;
                            Patient Patient = PatientData.Where(p => p.ID.Equals(FirstCharge.PatientID.GetValueOrDefault())).FirstOrDefault();
                            //ExternalPatient EPatient = ExternalPatientData.Where(ep => ep.AccountNum != null ? ep.AccountNum.Equals(Patient.AccountNum) : false).FirstOrDefault();

                            if (FirstCharge.InsuranceName != null && FirstCharge.InsuranceName != "" && FirstCharge.PrimaryInsuredID == null)
                            {
                                long InsurancePlanID = 0;
                                InsurancePlan PossibleCandidate = InsurancePlanData.Where(lamb => lamb.PlanName != null ? lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase) : false).FirstOrDefault();

                                if (PossibleCandidate != null)
                                {
                                    InsurancePlanID = PossibleCandidate.ID;
                                }
                                else
                                {
                                    ExInsuranceMapping MappingRecord = ExInsuranceMappingData.Where(lamb => lamb.ExternalInsuranceName != null ? lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")) : false).FirstOrDefault();
                                    if (MappingRecord != null)
                                    {
                                        InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == MappingRecord.InsurancePlanID).FirstOrDefault();

                                        if (TempInsurancePlan != null)
                                        {
                                            InsurancePlanID = MappingRecord.InsurancePlanID.GetValueOrDefault();
                                        }
                                    }
                                }
                                if (InsurancePlanID != 0 && Patient != null)
                                {
                                    PatientPlan Plan = PatientPlanData.Where(pp => pp.PatientID == Patient.ID && pp.InsurancePlanID == InsurancePlanID && pp.Coverage == "P").FirstOrDefault();
                                    if (Plan != null)
                                    {
                                        foreach (ExternalCharge EXCharge in VisitChargesForAddition)
                                        {
                                            if (!EXCharge.ErrorMessage.IsNull())
                                            {
                                                EXCharge.ErrorMessage = EXCharge.ErrorMessage.Replace("(Patient plan id not found)", "").Replace("Patient plan not found. ", "");

                                            }
                                            EXCharge.PrimaryInsuredID = Plan.ID;
                                            _context.ExternalCharge.Update(EXCharge);
                                        }
                                        FirstCharge.PrimaryInsuredID = Plan.ID;
                                        PlanLinked = true;
                                    }
                                }
                            }

                            if (Patient != null && PlanLinked == false)
                            {
                                if (FirstCharge.InsuranceName != null && FirstCharge.InsuranceName != "" && FirstCharge.PrimaryInsuredID.IsNull())
                                {
                                    PatientPlan TempPatientPlan = new PatientPlan();

                                    TempPatientPlan.FirstName = Patient.FirstName;
                                    TempPatientPlan.LastName = Patient.LastName;
                                    if (FirstCharge.PatientID.GetValueOrDefault() != 0)
                                        TempPatientPlan.PatientID = FirstCharge.PatientID.GetValueOrDefault();
                                    if (FirstCharge.DOB != null)
                                        TempPatientPlan.DOB = DateTime.Parse(FirstCharge.DOB, System.Globalization.CultureInfo.InvariantCulture);
                                    TempPatientPlan.Gender = Patient.Gender;
                                    TempPatientPlan.Email = Patient.Email;
                                    TempPatientPlan.Address1 = Patient.Address1;
                                    TempPatientPlan.City = Patient.City;
                                    TempPatientPlan.State = Patient.State;
                                    TempPatientPlan.ZipCode = Patient.ZipCode;
                                    TempPatientPlan.PhoneNumber = Patient.PhoneNumber;
                                    //TempPatientPlan.SubscriberId = EPatient.PrimaryInsuredID;
                                    TempPatientPlan.Coverage = "P";
                                    TempPatientPlan.IsActive = true;
                                    TempPatientPlan.IsDeleted = false;
                                    TempPatientPlan.AddedDate = DateTime.Today.Date;
                                    TempPatientPlan.AddedBy = UD == null ? "" : UD.Email.ToUpper();
                                    TempPatientPlan.RelationShip = "18";
                                    //string TempString = EPatient.PrimaryInsurance;

                                    InsurancePlan PossibleCandidate = InsurancePlanData.Where(lamb => lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                                    if (PossibleCandidate != null)
                                    {
                                        TempPatientPlan.InsurancePlanID = PossibleCandidate.ID;
                                    }
                                    else
                                    {
                                        ExInsuranceMapping MappingRecord = ExInsuranceMappingData.Where(lamb => lamb.ExternalInsuranceName != null ? lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")) : false).FirstOrDefault();
                                        if (MappingRecord != null)
                                        {
                                            InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == MappingRecord.InsurancePlanID).FirstOrDefault();

                                            if (TempInsurancePlan != null)
                                            {
                                                TempPatientPlan.InsurancePlanID = MappingRecord.InsurancePlanID;
                                            }
                                        }
                                        else
                                        {
                                            TempPatientPlan.InsurancePlanID = null;
                                        }
                                    }

                                    if (TempPatientPlan.InsurancePlanID != null)
                                    {
                                        if (!AddedPlans.Contains(FirstCharge.InsuranceName + Patient.ID))
                                        {
                                            AddedPlans.Add(FirstCharge.InsuranceName + Patient.ID);
                                            _context.PatientPlan.Add(TempPatientPlan);
                                            //EPatient.PrimaryPatientPlanID = TempPatientPlan.ID;
                                            //_context.ExternalPatient.Update(EPatient);
                                            foreach (ExternalCharge EXCharge in VisitChargesForAddition)
                                            {
                                                if (!EXCharge.ErrorMessage.IsNull())
                                                {
                                                    EXCharge.ErrorMessage = EXCharge.ErrorMessage.Replace("(Patient plan id not found)", "").Replace("Patient plan not found. ", "");


                                                }
                                                EXCharge.PrimaryInsuredID = TempPatientPlan.ID;
                                                _context.ExternalCharge.Update(EXCharge);
                                            }
                                            FirstCharge.PrimaryInsuredID = TempPatientPlan.ID;
                                        }
                                    }
                                }
                            }


                            if (!FirstCharge.PrescribingMD.IsNull() && FirstCharge.RefProviderID.IsNull())
                            {
                                string PrescribingMD = FirstCharge.PrescribingMD.Split(new string[] { ", MD" }, StringSplitOptions.None)[0];
                                if (PrescribingMD.Split(' ').Length > 0)
                                {
                                    if (PrescribingMD.Contains(" "))
                                    {
                                        string FirstName = PrescribingMD.Split(' ')[0];
                                        string LastName = PrescribingMD.Split(' ')[1];

                                        RefProvider TempRefProvider = RefProviderData.Where(refp => refp.FirstName.Equals(FirstName, StringComparison.InvariantCultureIgnoreCase) && refp.LastName.Equals(LastName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                        if (TempRefProvider != null)
                                        {
                                            //ex.RefProviderID = TempRefProvider.ID;

                                            foreach (ExternalCharge EXCharge in VisitChargesForAddition)
                                            {
                                                if (!EXCharge.ErrorMessage.IsNull())
                                                {
                                                    EXCharge.ErrorMessage = EXCharge.ErrorMessage.Replace("Prescribing MD\\Referring Provider not found in system. ", "");
                                                }
                                                EXCharge.RefProviderID = TempRefProvider.ID;
                                                _context.ExternalCharge.Update(EXCharge);
                                            }
                                        }
                                    }
                                }
                            }


                            foreach (ExternalCharge T in VisitChargesForAddition)
                            {
                                if (!T.DiagnosisCode.IsNull())
                                {
                                    if (!T.ErrorMessage.IsNull() && T.ErrorMessage.Contains("ICD"))
                                    {
                                        T.ErrorMessage = T.ErrorMessage.Replace("ICD not found.", "").Replace("ICD not found", "");
                                        _context.ExternalCharge.Update(T);
                                    }
                                }

                                if ((T.POSID == 0 || T.POSID == null) && long.Parse(T.POSCode) < 10)
                                {
                                    POS pos = _context.POS.Where(p => p.PosCode.Equals("0" + T.POSCode)).FirstOrDefault();
                                    if (pos != null)
                                    {
                                        T.POSID = pos.ID;
                                    }
                                    _context.ExternalCharge.Update(T);
                                }
                                if (T.CPTID == 0 || T.CPTID == null)
                                {
                                    Cpt TempCpt = new Cpt();
                                    TempCpt.CPTCode = T.CptCode;
                                    TempCpt.Description = T.CptCode + " procedure code";
                                    TempCpt.Amount = 0;
                                    if (!AddedCpts.Contains(TempCpt.CPTCode))
                                    {
                                        AddedCpts.Add(TempCpt.CPTCode);
                                        _context.Cpt.Add(TempCpt);
                                        T.CPTID = TempCpt.ID;
                                        if (!T.ErrorMessage.IsNull())
                                            T.ErrorMessage = T.ErrorMessage.Replace("invalid cpt. ", "").Replace("(invalid cpt)", "").Replace("Cpt not found. ", "").Replace("invalid cpt", "");

                                        _context.ExternalCharge.Update(T);
                                    }
                                }
                                if (T.POSID == 0 || T.POSID == null)
                                {
                                    POS TempPOS = new POS();
                                    TempPOS.PosCode = T.POSCode;
                                    TempPOS.Description = T.POSCode + " procedure code";
                                    TempPOS.AddedDate = DateTime.Now;
                                    TempPOS.AddedBy = UD.Email;
                                    if (!AddedPos.Contains(TempPOS.PosCode))
                                    {
                                        AddedPos.Add(TempPOS.PosCode);
                                        _context.POS.Add(TempPOS);
                                        T.POSID = TempPOS.ID;
                                        //T.ErrorMessage = T.ErrorMessage.Replace();
                                        _context.ExternalCharge.Update(T);
                                    }
                                }

                            }

                            if (ccc % 100 == 0)
                            {
                                _context.SaveChanges();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Write(e.Message);
                    }
                }
                _context.SaveChanges();

                foreach (List<ExternalCharge> VisitCharges in UnprocessedChargesByVisit)
                {
                    try
                    {
                        if (VisitCharges == null || VisitCharges.Count == 0) continue;

                        ICD ICDTemp = null;

                        if (!VisitCharges.Any(ch => ch.ErrorMessage.IsNull() ? false : true))
                        {
                            VisitData VisitData = new VisitData();
                            VisitData.Charges = new List<Charge>();
                            VisitData.ExternalCharges = new List<ExternalCharge>();
                            VisitData.Cpts = new List<Cpt>();

                            ExternalCharge FirstCharge = VisitCharges.FirstOrDefault();
                            Visit VisitToBeAdded = new Visit();
                            if (FirstCharge.PatientID.HasValue)
                            {
                                VisitToBeAdded.PatientID = FirstCharge.PatientID.Value;
                            }
                            VisitToBeAdded.ClientID = UD.ClientID;
                            VisitToBeAdded.PracticeID = UD.PracticeID;
                            VisitToBeAdded.DateOfServiceFrom = FirstCharge.DateOfService;
                            VisitToBeAdded.DateOfServiceTo = FirstCharge.DateOfService;
                            if (VisitCharges.FirstOrDefault().IsRegularRecord)
                            {
                                ICDTemp = ICDData.Where(i => i.ICDCode.Equals(VisitCharges.FirstOrDefault().DiagnosisCode)).FirstOrDefault();
                                if (ICDTemp != null)
                                {
                                    VisitToBeAdded.ICD1ID = ICDTemp.ID;
                                    foreach (ExternalCharge TempEC in VisitCharges)
                                    {
                                        if (!TempEC.ErrorMessage.IsNull())
                                            TempEC.ErrorMessage = TempEC.ErrorMessage.Replace("ICD not found. ", "");
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                VisitToBeAdded.ICD1ID = DummyIcd.ID;
                            }


                            if (FirstCharge.PatientID.GetValueOrDefault() != 0)
                            {
                                VisitToBeAdded.PatientID = FirstCharge.PatientID.GetValueOrDefault();
                            }

                            if (FirstCharge.POSID.GetValueOrDefault() != 0)
                            {
                                VisitToBeAdded.POSID = FirstCharge.POSID.GetValueOrDefault();
                            }

                            if (FirstCharge.ProviderID.IsNull())
                            {
                                VisitToBeAdded.ProviderID = FirstCharge.ProviderID; ;
                            }
                            else
                            {
                                VisitToBeAdded.ProviderID = FirstCharge.ProviderID;
                            }

                            if (FirstCharge.LocationID.IsNull())
                            {
                                VisitToBeAdded.LocationID = FirstCharge.LocationID;
                            }
                            else
                            {
                                VisitToBeAdded.LocationID = FirstCharge.LocationID;
                            }

                            if (FirstCharge.PatientID.GetValueOrDefault() != 0)
                            {
                                VisitToBeAdded.PatientID = FirstCharge.PatientID.GetValueOrDefault();
                                Patient TempPatient = PatientData.Where(p => p.ID == VisitToBeAdded.PatientID).FirstOrDefault();
                                ExternalPatient TempExternalPatient = ExternalPatientData.Where(ep => ep.ExternalPatientID == TempPatient.ExternalPatientID).FirstOrDefault();
                                PatientPlan TempPrimaryPatientPlan = PatientPlanData.Where(lamb => lamb.PatientID.Equals(VisitToBeAdded.PatientID) && lamb.Coverage == "P").FirstOrDefault();
                                if (TempPrimaryPatientPlan != null)
                                {
                                    InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                    if (TempInsurancePlan != null)
                                    {
                                        if (FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                        {
                                            VisitToBeAdded.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                        }
                                    }
                                    else
                                    {
                                        ExInsuranceMapping TempExInsuranceMapping = _contextMain.ExInsuranceMapping.Where(lamb1 => lamb1.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                        if (TempExInsuranceMapping != null)
                                        {
                                            InsurancePlan TempInsurancePlan2 = InsurancePlanData.Where(lamb => lamb.ID == TempExInsuranceMapping.InsurancePlanID).FirstOrDefault();
                                            if (TempInsurancePlan2 != null)
                                            {
                                                if (FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                                {
                                                    VisitToBeAdded.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                                }
                                            }
                                        }
                                    }
                                }
                            }



                            VisitToBeAdded.OutsideReferral = false;
                            if (FirstCharge.IsRegularRecord)
                            {
                                VisitToBeAdded.AddedDate = DateTime.Now;
                                VisitToBeAdded.PrimaryStatus = "N";
                                VisitToBeAdded.IsSubmitted = false;

                            }
                            else
                            {
                                VisitToBeAdded.PrimaryStatus = "S";
                                VisitToBeAdded.AddedDate = FirstCharge.DateOfService;
                                VisitToBeAdded.IsSubmitted = true;
                                VisitToBeAdded.SubmittedDate = FirstCharge.SubmittetdDate;
                            }
                            VisitToBeAdded.AddedBy = UD.Email;

                            VisitToBeAdded.PrimaryBal = VisitCharges.Sum(vc => vc.Charges);
                            VisitToBeAdded.PrimaryBilledAmount = VisitCharges.Sum(vc => vc.Charges);
                            VisitToBeAdded.TotalAmount = VisitCharges.Sum(vc => vc.Charges);

                            VisitToBeAdded.IsDontPrint = false;
                            VisitToBeAdded.IsForcePaper = false;
                            if (!FirstCharge.PrescribingMD.IsNull())
                            {
                                string PrescribingMD = FirstCharge.PrescribingMD.Split(new string[] { ", MD" }, StringSplitOptions.None)[0];
                                if (PrescribingMD.Split(' ').Length > 0)
                                {
                                    if (PrescribingMD.Contains(" "))
                                    {
                                        string FirstName = PrescribingMD.Split(' ')[0];
                                        string LastName = PrescribingMD.Split(' ')[1];

                                        RefProvider TempRefProvider = RefProviderData.Where(refp => refp.FirstName.Equals(FirstName, StringComparison.InvariantCultureIgnoreCase) && refp.LastName.Equals(LastName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                        if (TempRefProvider != null)
                                        {
                                            VisitToBeAdded.RefProviderID = TempRefProvider.ID;
                                        }
                                    }
                                }
                            }

                            VisitToBeAdded.IsReversalApplied = false;
                            Cpt TempCpt = null;
                            InsurancePlan MatchedInsurancePlan = null;
                            foreach (ExternalCharge TempEC in VisitCharges)
                            {
                                Charge ActualCharge = new Charge();
                                ActualCharge.CPTID = TempEC.CPTID.GetValueOrDefault();

                                ActualCharge.POSID = TempEC.POSID.GetValueOrDefault();
                                ActualCharge.ClientID = UD.ClientID;
                                ActualCharge.PracticeID = UD.PracticeID;
                                ActualCharge.LocationID = TempEC.LocationID;
                                ActualCharge.ProviderID = TempEC.ProviderID;
                                if (TempEC.PatientID.GetValueOrDefault() != 0)
                                {
                                    ActualCharge.PatientID = TempEC.PatientID.GetValueOrDefault();
                                    ActualCharge.PrimaryPatientPlanID = VisitToBeAdded.PrimaryPatientPlanID;

                                    //PatientPlan TempPrimaryPatientPlan = PatientPlanData.Where(lamb => lamb.PatientID.Equals(ActualCharge.PatientID) && lamb.Coverage == "P").FirstOrDefault();
                                    //if (TempPrimaryPatientPlan != null)
                                    //{
                                    //    InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                    //    if (TempInsurancePlan != null)
                                    //    {
                                    //        MatchedInsurancePlan = TempInsurancePlan;
                                    //        if (TempEC.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(TempEC.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                    //        {
                                    //            ActualCharge.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        ExInsuranceMapping TempExInsuranceMapping = _contextMain.ExInsuranceMapping.Where(lamb1 => lamb1.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                    //        if (TempExInsuranceMapping != null)
                                    //        {
                                    //            InsurancePlan TempInsurancePlan2 = InsurancePlanData.Where(lamb => lamb.ID == TempExInsuranceMapping.InsurancePlanID).FirstOrDefault();
                                    //            if (TempInsurancePlan2 != null)
                                    //            {
                                    //                MatchedInsurancePlan = TempInsurancePlan2;
                                    //                if (TempEC.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(TempEC.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                    //                {
                                    //                    ActualCharge.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                                ActualCharge.Modifier1ID = TempEC.Modifier1ID;
                                ActualCharge.Modifier2ID = TempEC.Modifier2ID;
                                ActualCharge.Modifier3ID = TempEC.Modifier3ID;
                                ActualCharge.Modifier4ID = TempEC.Modifier4ID;
                                //if (PrescribingMD.Split(' ').Length > 0)
                                //{
                                //    string FirstName = PrescribingMD.Split(' ')[0];
                                //    string LastName = PrescribingMD.Split(' ')[1];
                                //    RefProvider TempRefProvider = RefProviderData.Where(refp => refp.FirstName.Equals(FirstName, StringComparison.InvariantCultureIgnoreCase) && refp.LastName.Equals(LastName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                //    if (TempRefProvider != null)
                                //    {
                                //        ActualCharge.RefProviderID = TempRefProvider.ID;
                                //    }
                                //}

                                ActualCharge.RefProviderID = VisitToBeAdded.RefProviderID;


                                ActualCharge.Units = TempEC.DaysOrUnits;
                                ActualCharge.DateOfServiceFrom = TempEC.DateOfService;
                                ActualCharge.DateOfServiceTo = TempEC.DateOfService;
                                ActualCharge.TotalAmount = TempEC.Charges;
                                ActualCharge.PrimaryBilledAmount = TempEC.Charges;
                                ActualCharge.PrimaryBal = TempEC.Charges;
                                ActualCharge.Pointer1 = "1";
                                if (TempEC.IsRegularRecord)
                                {
                                    ActualCharge.AddedDate = DateTime.Now;
                                    ActualCharge.PrimaryStatus = "N";
                                    ActualCharge.IsSubmitted = false;
                                }
                                else
                                {
                                    ActualCharge.PrimaryStatus = "S";
                                    ActualCharge.AddedDate = TempEC.DateOfService;
                                    ActualCharge.IsSubmitted = true;
                                    ActualCharge.SubmittetdDate = TempEC.SubmittetdDate;
                                }
                                ActualCharge.AddedBy = UD.Email;
                                ActualCharge.IsReversalApplied = false;
                                ActualCharge.IsDontPrint = false;

                                if (ActualCharge.PrimaryPatientPlanID.GetValueOrDefault() != 0)
                                {
                                    if (ActualCharge.CPTID != 0)
                                    {

                                        VisitData.ExternalCharges.Add(TempEC);
                                        _context.Charge.Add(ActualCharge);
                                        VisitData.Charges.Add(ActualCharge);
                                    }
                                }
                            }
                            VisitData.Visit = VisitToBeAdded;
                            if (VisitData.Charges.Count() != 0)
                            {
                                _context.Visit.Add(VisitToBeAdded);
                                VisitsToBeLinked.Add(VisitData);
                            }

                        }
                    }
                    catch (Exception e)
                    {
                    }

                }
                await _context.SaveChangesAsync();

                foreach (VisitData VisitData in VisitsToBeLinked)
                {
                    if (VisitData.Charges.Count() != 0)
                    {
                        for (int i = 0; i < VisitData.Charges.Count(); i++)
                        {
                            VisitData.Charges.ElementAt(i).VisitID = VisitData.Visit.ID;
                            //VisitData.Charges.ElementAt(i).CPTID = VisitData.Cpts.ElementAt(i).ID;
                            VisitData.ExternalCharges.ElementAt(i).ChargeID = VisitData.Charges.ElementAt(i).ID;
                            VisitData.ExternalCharges.ElementAt(i).VisitID = VisitData.Visit.ID;
                            VisitData.ExternalCharges.ElementAt(i).ErrorMessage = null;
                            VisitData.ExternalCharges.ElementAt(i).MergeStatus = "A";
                            _context.ExternalCharge.Update(VisitData.ExternalCharges.ElementAt(i));
                            _context.Charge.Update(VisitData.Charges.ElementAt(i));
                        }
                    }
                }

                _context.SaveChanges();
            }
            return Ok();
        }


        [HttpPost]
        [Route("UpdateChargeData")]
        public async Task<ActionResult> UpdateChargeData(VMDataMigration InputData)
        {
            ReturnObject returnObject = new ReturnObject();
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
                User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
                User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long ClientID = UD.ClientID;

            List<Practice> PracticeData = _context.Practice.ToList();
            List<Provider> ProviderData = _context.Provider.ToList();
            List<Models.Location> LocationData = _context.Location.ToList();
            List<Patient> PatientData = _context.Patient.ToList();
            List<InsurancePlan> InsurancePlanData = _context.InsurancePlan.ToList();
            List<ExternalPatient> ExternalPatientData = _context.ExternalPatient.ToList();
            List<PatientPlan> PatientPlanData = _context.PatientPlan.ToList();
            List<ExternalCharge> ExternalChargeData = _context.ExternalCharge.ToList();
            List<POS> POSTable = _context.POS.ToList();
            List<Cpt> CptTable = _context.Cpt.ToList();
            List<Modifier> ModifierTable = _context.Modifier.ToList();

            List<ExternalPayment> ExternalPaymentList = _context.ExternalPayment.Where(x => x.PracticeID == UD.PracticeID).ToList();
            long ExternalPaymentInitialID = 0, ExternalPaymentIDAfterInsertion = 0;

            if (ExternalPaymentList.Count() != 0)
            {
                ExternalPaymentInitialID = ExternalPaymentList.OrderBy(lamb => lamb.ID).ElementAt(ExternalPaymentList.Count() - 1).ID;
            }

            byte[] bytes = Convert.FromBase64String(InputData.UploadModel.Content.Substring(InputData.UploadModel.Content.IndexOf("base64,") + 7));
            string FileName = InputData.UploadModel.Name;
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                long practiceID;
                long providerID;
                long locationID;
                long InterPracticeID = 0;
                long InterProviderID = 0;
                long InterLocationID = 0;
                //Stream Stream = contents;

                practiceID = InputData.PracticeID;
                providerID = InputData.ProviderID;
                locationID = InputData.LocationID;

                if (InputData.PracticeID.IsNull())
                    return BadRequest("Practice id not present");
                if (InputData.ProviderID.IsNull())
                    return BadRequest("Provider id not present");
                if (InputData.LocationID.IsNull())
                    return BadRequest("Location id not present");



                //SaveStreamAsFile("D:\\", Stream, "streamedfile.xlsx");

                SpreadsheetDocument Doc = SpreadsheetDocument.Open(stream, false);

                WorkbookPart WorkbookPart = Doc.WorkbookPart;
                WorksheetPart WorksheetPart = WorkbookPart.WorksheetParts.First();
                Worksheet Sheet = WorksheetPart.Worksheet;

                SharedStringTablePart SstPart = WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();

                SharedStringTable Sst = SstPart.SharedStringTable;


                var AllRows = Sheet.Descendants<Row>();

                List<List<ExternalPayment>> FinalChargesData = new List<List<ExternalPayment>>();
                List<string> VisitAmounts = new List<string>();

                List<Dictionary<string, string>> SheetData = new List<Dictionary<string, string>>();

                for (int RowIndex = 0; RowIndex < AllRows.Count(); RowIndex++)
                {
                    try
                    {
                        bool IsChargeDataPresent = false;
                        List<Cell> RowCells = AllRows.ElementAt(RowIndex).Elements<Cell>().ToList();
                        bool SummationIdentifier = true;

                        ExternalPayment TempPayment = new ExternalPayment();

                        //Debug.Write("start of row");
                        Dictionary<string, string> IntermediateData = new Dictionary<string, string>();

                        for (int ColIndex = 0; ColIndex < RowCells.Count(); ColIndex++)
                        {
                            try
                            {
                                CellReferenceValuePair pair = GetValueFromCell(RowCells.ElementAt(ColIndex), Sst);

                                if (pair != null)
                                {
                                    //Debug.Write(pair.Reference+ "       "+pair.Value);
                                    string ReferenceAlphabets = new String(pair.Reference.Where(Char.IsLetter).ToArray());
                                    string FinalIdentifier = "";
                                    pair.Reference = new String(pair.Reference.Where(Char.IsLetter).ToArray());

                                    for (int StrIndex = 0; StrIndex < pair.Reference.Length; StrIndex++)
                                    {
                                        FinalIdentifier += GetColumnQualifier(pair.Reference[StrIndex]);
                                    }


                                    if (FinalIdentifier.Equals("0"))
                                    {
                                        TempPayment.PatientName = pair.Value.Trim();
                                    }

                                    if (FinalIdentifier.Equals("1"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("insurance_name", pair.Value);
                                        TempPayment.InsuranceName = pair.Value;

                                    }

                                    if (FinalIdentifier.Equals("2"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("unique_value", pair.Value);

                                    }

                                    if (FinalIdentifier.Equals("3"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("dos", pair.Value);
                                        if (pair.Value.Contains("-") || pair.Value.Contains("/"))
                                            TempPayment.DateOfService = DateTime.Parse(pair.Value);
                                        else
                                            TempPayment.DateOfService = DateTime.FromOADate(double.Parse(pair.Value));
                                    }

                                    if (FinalIdentifier.Equals("4"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("pos", pair.Value);
                                        TempPayment.POSCode = pair.Value;
                                        POS MatchedPOS = POSTable.Where(lamb => lamb.PosCode.Equals(pair.Value.Trim())).FirstOrDefault();
                                        if (MatchedPOS != null)
                                            TempPayment.POSID = MatchedPOS.ID;

                                    }
                                    if (FinalIdentifier.Equals("5"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("cpt", pair.Value);
                                        Cpt MatchedCpt = CptTable.Where(lamb => lamb.CPTCode.Equals(pair.Value.Trim())).FirstOrDefault();
                                        if (MatchedCpt != null)
                                            TempPayment.CPTID = MatchedCpt.ID;
                                        TempPayment.CptCode = pair.Value;
                                    }
                                    if (FinalIdentifier.Equals("6"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("days_or_units", pair.Value);
                                        TempPayment.DaysOrUnits = pair.Value;
                                    }
                                    if (FinalIdentifier.Equals("7"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("charges", pair.Value);
                                        TempPayment.Charges = decimal.Parse(pair.Value);
                                    }
                                    if (FinalIdentifier.Equals("8"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("insurance_payment", pair.Value);
                                        TempPayment.InsurancePayment = decimal.Parse(pair.Value);
                                    }
                                    if (FinalIdentifier.Equals("9"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("provider_name", pair.Value);
                                        TempPayment.ProviderName = pair.Value;
                                    }
                                    if (FinalIdentifier.Equals("10"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("patient_payment", pair.Value);
                                        TempPayment.PatientPayment = decimal.Parse(pair.Value);
                                    }
                                    if (FinalIdentifier.Equals("11"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("adj", pair.Value);
                                        TempPayment.Adj = decimal.Parse(pair.Value);
                                    }

                                    if (FinalIdentifier.Equals("12"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("balance", pair.Value);
                                        TempPayment.Balance = decimal.Parse(pair.Value);

                                    }
                                    if (FinalIdentifier.Equals("13"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        IntermediateData.Add("check_date", pair.Value);
                                        if (!pair.Value.Equals("N/A"))
                                        {
                                            if (pair.Value.Contains("-") || pair.Value.Contains("/"))
                                                TempPayment.CheckDate = DateTime.Parse(pair.Value);
                                            else
                                                TempPayment.CheckDate = DateTime.FromOADate(double.Parse(pair.Value));
                                        }

                                    }
                                    if (FinalIdentifier.Equals("14"))
                                    {
                                        if (!pair.Value.Equals("N/A"))
                                        {
                                            IntermediateData.Add("check_number", pair.Value);
                                            TempPayment.CheckNumber = pair.Value;
                                        }
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                return BadRequest("Unexpected Error Occurred. Please Contect BellMedEx\n" + ex.Message + "\n" + ex.StackTrace);
                            }
                        }

                        TempPayment.LocationID = locationID;
                        TempPayment.ProviderID = providerID;
                        TempPayment.PracticeID = UD.PracticeID;
                        TempPayment.AddedDate = DateTime.Now;
                        TempPayment.AddedBy = UD.Email;
                        TempPayment.MergeStatus = "E";
                        SheetData.Add(IntermediateData);

                        _context.ExternalPayment.Add(TempPayment);

                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Unexpected Error Occurred. Please Contect BellMedEx\n" + ex.Message + "\n" + ex.StackTrace);

                    }
                }

                _context.SaveChanges();

                ExternalPaymentList = _context.ExternalPayment.Where(x => x.PracticeID == UD.PracticeID).ToList();

                if (ExternalPaymentList.Count() != 0)
                    ExternalPaymentIDAfterInsertion = ExternalPaymentList.OrderBy(lamb => lamb.ID).ElementAt(ExternalPaymentList.Count() - 1).ID;

                if (ExternalPaymentList.Count() != 0)
                    ExternalPaymentList = _context.ExternalPayment.Where(x => x.PracticeID == UD.PracticeID && x.ID > ExternalPaymentInitialID && x.ID <= ExternalPaymentIDAfterInsertion).ToList();
                else
                    ExternalPaymentList = _context.ExternalPayment.Where(x => x.PracticeID == UD.PracticeID).ToList();

                foreach (ExternalPayment TempExternalPayment in ExternalPaymentList)
                {
                    string FirstName = TempExternalPayment.PatientName.Split(',')[1].Trim();
                    string LastName = TempExternalPayment.PatientName.Split(',')[0].Trim();
                    ExternalCharge MatchingExternalCharge = ExternalChargeData.Where(
                            lamb => lamb.FirstName.Trim().ToUpper().Replace(" ", "").Equals(FirstName.Trim().ToUpper()) &&
                            lamb.LastName.Trim().ToUpper().Replace(" ", "").Equals(LastName.Trim().ToUpper()) &&
                            lamb.POSCode.Trim().ToUpper().Replace(" ", "").Equals(TempExternalPayment.POSCode.Trim().ToUpper()) &&
                            lamb.CptCode.Trim().ToUpper().Replace(" ", "").Equals(TempExternalPayment.CptCode.Trim().ToUpper()) &&
                            lamb.DateOfService.Equals(TempExternalPayment.DateOfService) &&
                            lamb.Charges == TempExternalPayment.Charges
                        ).FirstOrDefault();

                    if (MatchingExternalCharge != null)
                    {
                        MatchingExternalCharge.Adj = TempExternalPayment.Adj;
                        MatchingExternalCharge.Balance = TempExternalPayment.Balance;
                        MatchingExternalCharge.DaysOrUnits = TempExternalPayment.DaysOrUnits;
                        MatchingExternalCharge.InsuranceName = TempExternalPayment.InsuranceName;
                        MatchingExternalCharge.ProviderName = TempExternalPayment.ProviderName;
                        MatchingExternalCharge.PatientPayment = TempExternalPayment.PatientPayment;
                        MatchingExternalCharge.InsurancePayment = TempExternalPayment.InsurancePayment;
                        MatchingExternalCharge.UpdatedBy = UD.Email;
                        MatchingExternalCharge.UpdatedDate = DateTime.Now;
                        _context.ExternalCharge.Update(MatchingExternalCharge);
                        TempExternalPayment.ExternalChargeID = MatchingExternalCharge.ID.ToString();
                    }

                }
                _context.SaveChanges();

                return Ok(returnObject);
            }

        }

        public class VisitWithGroupNumber
        {
            public Visit Visit { get; set; }
            public long GroupNumber { get; set; }
        }
        public class PaymentCheckWithGroupNumber
        {
            public PaymentCheck PaymentCheck { get; set; }
            public long GroupNumber { get; set; }
        }
        public class PaymentCheckMapping
        {

            public PaymentCheck PaymentCheck { get; set; }
            public PaymentVisit PaymentVisit { get; set; }
            public List<PaymentCharge> PaymentCharges { get; set; }
            public List<ExternalCharge> ExternalCharges { get; set; }
            public long GroupID { get; set; }
        }
        public class ChargeWithGroupNumber
        {
            public Charge Charge { get; set; }
            public long IdentifierID { get; set; }
        }
        [HttpPost]
        [Route("FindExternalPatients")]
        public async Task<ActionResult<IEnumerable<GExternalPatient>>> FindExternalPatients(CExternalPatient CExternalPatient)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindExternalPatients(CExternalPatient, PracticeId);
        }
        private List<GExternalPatient> FindExternalPatients(CExternalPatient CExternalPatient, long PracticeId)
        {
            List<GExternalPatient> data = (from externalpatienttable in _context.ExternalPatient
                                           join locationtable in _context.Location on externalpatienttable.LocationId equals locationtable.ID
                                           join patienttable in _context.Patient on externalpatienttable.AccountNum equals patienttable.AccountNum
                                           into temppat
                                           from finalpatienttable in temppat.DefaultIfEmpty()
                                           join providertable in _context.Provider on externalpatienttable.ProviderID equals providertable.ID

                                           where
                                           (
                                           (CExternalPatient.AccountNumber.IsNull() ? true : CExternalPatient.AccountNumber.Equals(externalpatienttable.AccountNum)) &&
                                           (CExternalPatient.ExternalPatientID.IsNull() ? true : CExternalPatient.ExternalPatientID.Equals(externalpatienttable.ExternalPatientID)) &&
                                           (CExternalPatient.FirstName.IsNull() ? true : CExternalPatient.FirstName.Equals(externalpatienttable.FirstName)) &&
                                           (CExternalPatient.LastName.IsNull() ? true : CExternalPatient.LastName.Equals(externalpatienttable.LastName)) &&
                                           (CExternalPatient.ResolvedErrorMessage.IsNull() ? true : CExternalPatient.ResolvedErrorMessage == "Y" ? externalpatienttable.resolve == true : CExternalPatient.ResolvedErrorMessage == "N" ? externalpatienttable.resolve == false : true) &&
                                           (CExternalPatient.Status.IsNull() ? true : CExternalPatient.Status == "F" ? externalpatienttable.MissingInfo != null : CExternalPatient.Status.Equals(externalpatienttable.MergeStatus)) &&
                                           ((CExternalPatient.EntryDateFrom != null && CExternalPatient.EntryDateTo != null) ? (externalpatienttable.AddedDate != null ? externalpatienttable.AddedDate.Date <= CExternalPatient.EntryDateTo.GetValueOrDefault().Date && externalpatienttable.AddedDate.Date >= CExternalPatient.EntryDateFrom.GetValueOrDefault().Date : externalpatienttable.AddedDate != null ? externalpatienttable.AddedDate.Date >= CExternalPatient.EntryDateFrom.GetValueOrDefault() : false) : (CExternalPatient.EntryDateFrom != null ? (externalpatienttable.AddedDate != null && CExternalPatient.EntryDateFrom.HasValue ? externalpatienttable.AddedDate.Date >= CExternalPatient.EntryDateFrom.GetValueOrDefault() : true) : true)) &&

                                           //(ExtensionMethods.IsBetweenDOS(CExternalPatient.EntryFromDate, CExternalPatient.EntryToDate, externalpatienttable.AddedDate, externalpatienttable.AddedDate)) &&
                                           //(CExternalPatient.FileName.IsNull() ? true : CExternalPatient.FileName.Replace(".","").Contains(externalpatienttable.FileName.Replace(".", ""))) &&
                                           (CExternalPatient.FileName.IsNull() ? true : externalpatienttable.FileName.Contains(CExternalPatient.FileName, StringComparison.InvariantCultureIgnoreCase)) &&
                                           (externalpatienttable.PracticeID == PracticeId))



                                           select new GExternalPatient
                                           {
                                               ID = externalpatienttable.ID,
                                               AccountNumber = externalpatienttable.AccountNum.IsNull() ? "" : externalpatienttable.AccountNum,
                                               Dob = externalpatienttable.DOB.HasValue ? externalpatienttable.DOB.Value.ToString(@"MM\/dd\/yyyy") : DateTime.Now.ToString(@"MM\/dd\/yyyy"),
                                               ExternalPatientID = externalpatienttable.ExternalPatientID.IsNull() ? "" : externalpatienttable.ExternalPatientID,
                                               FileName = externalpatienttable.FileName.IsNull() ? "" : externalpatienttable.FileName,
                                               FirstName = externalpatienttable.FirstName.IsNull() ? "" : externalpatienttable.FirstName,
                                               LastName = externalpatienttable.LastName.IsNull() ? "" : externalpatienttable.LastName,
                                               LocationName = locationtable.Name.IsNull() ? "" : locationtable.Name,
                                               ProviderName = providertable.Name.IsNull() ? "" : providertable.Name,
                                               Status = externalpatienttable.MergeStatus.IsNull() ? "" : externalpatienttable.MergeStatus,
                                               LocationID = locationtable.ID,
                                               ProviderID = providertable.ID,
                                               EntryDate = externalpatienttable.AddedDate.Format("MM/dd/yyyy"),
                                               PrimaryPatientPlanID = externalpatienttable.PrimaryPatientPlanID == null ? -1 : externalpatienttable.PrimaryPatientPlanID,
                                               PrimaryPayer = externalpatienttable.PrimaryInsurance.IsNull() ? "" : externalpatienttable.PrimaryInsurance,
                                               PrimarySubscriberID = externalpatienttable.PrimaryInsuredID.IsNull() ? "" : externalpatienttable.PrimaryInsuredID,
                                               SecondaryPatientPlanID = externalpatienttable.SecondaryPatientPlanID == null ? -1 : externalpatienttable.SecondaryPatientPlanID,
                                               SecondaryPayer = externalpatienttable.SecondaryInsurance.IsNull() ? "" : externalpatienttable.SecondaryInsurance,
                                               SecondarySubscriberID = externalpatienttable.SecondaryInsuredID.IsNull() ? "" : externalpatienttable.SecondaryInsuredID,
                                               PatientID = finalpatienttable.ID == null ? -1 : finalpatienttable.ID,
                                               ErrorMessage = externalpatienttable.MissingInfo
                                           }).ToList();

            return data;
        }


        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CExternalPatient CExteralPatient)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
           );

            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;
            List<GExternalPatient> data = FindExternalPatients(CExteralPatient, PracticeId);
            ExportController controller = new ExportController(_context);

            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CExteralPatient, "External Patient Report");

        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CExternalPatient CExternalPatient)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GExternalPatient> data = FindExternalPatients(CExternalPatient, PracticeId);
            ExportController controller = new ExportController(_context);

            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }
        [HttpPost]
        [Route("AddPatientData")]
        [RequestSizeLimit(209715200)]
        [RequestFormLimits(ValueLengthLimit = 209715200)]
        public async Task<ActionResult> AddPatientData(VMDataMigration InputData)
        {
            if (System.IO.File.Exists(System.IO.Path.Combine(_context.env.ContentRootPath, "Officeally patient entry logs.txt")))
            {
                System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Officeally patient entry logs.txt"), "Start of add patient data method officelly\n");
            }
            else
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Officeally patient entry logs.txt"), "Start of add patient data method officelly\n");
            }
            bool ErrorsCaught = false;
            //var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            //using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            //{

            try
            {
                UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
                User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
                User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

                long ClientID = UD.ClientID;

                //List<Edi837Payer> Edi837PayerData = _context.Edi837Payer.ToList();
                //List<InsurancePlan> InsurancePlanData = _context.InsurancePlan.ToList();
                //List<ExInsuranceMapping> ExInsuranceMappingData = _contextMain.ExInsuranceMapping.ToList();
                //List<Practice> PracticeData = _context.Practice.ToList();
                //List<Provider> ProviderData = _context.Provider.ToList();
                //List<Models.Location> LocationData = _context.Location.ToList();
                //List<PlanType> PlanTypeData = _context.PlanType.ToList();
                //List<Patient> PatientData = _context.Patient.Where(x => x.PracticeID == UD.PracticeID).ToList();
                //List<ExternalPatient> ExternalPatientList = _context.ExternalPatient.Where(x => x.PracticeID == UD.PracticeID).ToList();
                //List<long> ExInsuranceMappingIds = _contextMain.ExInsuranceMapping.Select(lamb => lamb.ID).ToList();
                //List<long> InsurancePlanIds = _context.InsurancePlan.Select(lamb => lamb.ID).ToList();

                List<Edi837Payer> Edi837PayerData = new List<Edi837Payer>();
                List<InsurancePlan> InsurancePlanData = new List<InsurancePlan>();
                List<ExInsuranceMapping> ExInsuranceMappingData = new List<ExInsuranceMapping>();
                List<Practice> PracticeData = new List<Practice>();
                List<Provider> ProviderData = new List<Provider>();
                List<Models.Location> LocationData = new List<Models.Location>();
                List<PlanType> PlanTypeData = new List<PlanType>();
                List<Patient> PatientData = new List<Patient>();
                List<ExternalPatient> ExternalPatientList = new List<ExternalPatient>();
                List<long> ExInsuranceMappingIds = new List<long>();
                List<long> InsurancePlanIds = new List<long>();
                long ExternalPatientInitialID = 0, ExternalPatientIDAfterInsertion = 0;

                if (ExternalPatientList.Count() != 0)
                {
                    ExternalPatientInitialID = ExternalPatientList.OrderBy(lamb => lamb.ID).ElementAt(ExternalPatientList.Count() - 1).ID;
                }


                // string FilePath = ;
                ReturnObject returnObject = new ReturnObject();
                byte[] bytes = Convert.FromBase64String(InputData.UploadModel.Content.Substring(InputData.UploadModel.Content.IndexOf("base64,") + 7));
                string FileName = InputData.UploadModel.Name;
                Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
                string OutputPath = Path.Combine("\\\\", settings.DocumentServerURL,
                        settings.DocumentServerDirectory, "DataMigration", UD.ClientID + "",
                        DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));

                if (System.IO.Directory.Exists(OutputPath))
                {
                    System.IO.File.WriteAllBytes(Path.Combine(OutputPath,FileName), bytes);
                }
                else
                {
                    System.IO.Directory.CreateDirectory(OutputPath);
                    System.IO.File.WriteAllBytes(Path.Combine(OutputPath, FileName), bytes);
                }
                
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    long practiceID;
                    long providerID;
                    long locationID;
                    long InterPracticeID = 0;
                    long InterProviderID = 0;
                    long InterLocationID = 0;
                    //Stream Stream = contents;

                    practiceID = InputData.PracticeID;
                    providerID = InputData.ProviderID;
                    locationID = InputData.LocationID;

                    if (InputData.PracticeID == null)
                        return BadRequest("Practice id not present");
                    if (InputData.ProviderID == null)
                        return BadRequest("Provider id not present");
                    if (InputData.LocationID == null)
                        return BadRequest("Location id not present");



                    //SaveStreamAsFile("D:\\", Stream, "streamedfile.xlsx");

                    SpreadsheetDocument Doc = SpreadsheetDocument.Open(stream, false);

                    WorkbookPart WorkbookPart = Doc.WorkbookPart;
                    WorksheetPart WorksheetPart = WorkbookPart.WorksheetParts.First();
                    Worksheet Sheet = WorksheetPart.Worksheet;

                    SharedStringTablePart SstPart = WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();

                    SharedStringTable Sst = SstPart.SharedStringTable;

                    var AllCells = Sheet.Descendants<Cell>();
                    var AllRows = Sheet.Descendants<Row>();

                    bool TakeTrioFromSheet = false, TrioFirstPresent = false, TrioSecondPresent = false, TrioThirdPresent = false;

                    Debug.WriteLine(AllRows.Count() + "  " + AllCells.Count());

                    //Adding Patients

                    bool[] PrimaryInsuranceIdPresent = new bool[AllRows.Count()];
                    bool[] SecondaryInsuranceIdPresent = new bool[AllRows.Count()];

                    //string[] PrimaryInsurancePlanIds = new string[AllRows.Count()];
                    //string[] SecondaryInsurancePlanIds = new string[AllRows.Count()];

                    //PatientPlan[] PrimaryPatientPlans = new PatientPlan[AllRows.Count()];
                    //PatientPlan[] SecondaryPatientPlans = new PatientPlan[AllRows.Count()];

                    List<PatientPlanWithID> PrimaryPatientPlanWithIDs = new List<PatientPlanWithID>();
                    List<PatientPlanWithID> SecondaryPatientPlanWithIDs = new List<PatientPlanWithID>();

                    int RowNum = 0;
                    //string[] PatientIds = new string[AllRows.Count()]; 
                    List<string> PatientIds = new List<string>();
                    Patient[] PatientsAdded = new Patient[AllRows.Count()];
                    foreach (Row row in AllRows)
                    {
                        bool TrioFirstValuePresent = false, TrioSecondValuePresent = false, TrioThirdValuePresent = false;
                        try
                        {
                            InterPracticeID = 0;
                            InterProviderID = 0;
                            InterLocationID = 0;
                            if (RowNum != 0)
                            {
                                //int ColNum = 1;
                                //Debug.WriteLine(row.Elements<Cell>().Count());
                                List<Cell> RowCells = row.Elements<Cell>().ToList();
                                ExternalPatient externalPatient = new ExternalPatient();

                                for (int i = 0; i < RowCells.Count; i++)
                                {
                                    try
                                    {
                                        CellReferenceValuePair pair = GetValueFromCell(RowCells.ElementAt(i), Sst);
                                        if (pair != null && pair.Value != null && !pair.Reference.IsNull())
                                        {
                                            //Debug.WriteLine(pair.Reference+"  with value  "+pair.Value);
                                            //char ColumnNumber = pair.Reference[0];
                                            //Debug.WriteLine(RowNum + " " + i);

                                            //Debug.WriteLine(pair.Value.IsNull() + "  pair value");
                                            //Debug.WriteLine(pair.Reference.IsNull() + " pair reference");

                                            string ReferenceAlphabets = new String(pair.Reference.Where(Char.IsLetter).ToArray());
                                            string FinalIdentifier = "";
                                            pair.Reference = new String(pair.Reference.Where(Char.IsLetter).ToArray());

                                            for (int StrIndex = 0; StrIndex < pair.Reference.Length; StrIndex++)
                                            {
                                                FinalIdentifier += GetColumnQualifier(pair.Reference[StrIndex]);
                                            }
                                            //Debug.WriteLine(pair.Reference + "    " + FinalIdentifier + "   " + ReferenceAlphabets);
                                            if (FinalIdentifier.Equals("0"))

                                                externalPatient.ExternalPatientID = pair.Value;
                                            if (FinalIdentifier.Equals("1"))
                                                externalPatient.Status = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("2"))
                                            {
                                                string CompleteName = pair.Value.Trim();
                                                if (CompleteName.Contains("BHUVANAM", StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    Debug.WriteLine("");
                                                }
                                                string[] FirstSplit = CompleteName.Split(',');
                                                string[] SecondSplit = FirstSplit[1].Trim().Split(' ');
                                                string FirstName = "", MI = "";
                                                if (SecondSplit.Length > 1)
                                                {
                                                    FirstName = SecondSplit[0];
                                                    string temp = SecondSplit[1];
                                                    for (int candidateNumber = 1; candidateNumber < SecondSplit.Length; candidateNumber++)
                                                    {
                                                        if (SecondSplit[candidateNumber].Length > 3)
                                                        {
                                                            FirstName = FirstName + " " + SecondSplit[candidateNumber].Trim().ToUpper();
                                                        }
                                                        else
                                                        {
                                                            if (SecondSplit[candidateNumber].Any(char.IsLower))
                                                            {
                                                                FirstName = FirstName + " " + SecondSplit[candidateNumber].Trim().ToUpper();
                                                            }
                                                            else
                                                            {
                                                                MI = SecondSplit[candidateNumber].ToUpper();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    FirstName = SecondSplit[0].Trim().ToUpper();
                                                }
                                                externalPatient.LastName = FirstSplit[0].Trim().ToUpper();
                                                externalPatient.FirstName = FirstName;
                                                externalPatient.MiddleInitial = MI;
                                                Debug.WriteLine(FirstName + "   " + externalPatient.LastName);
                                            }
                                            else if (FinalIdentifier.Equals("3"))
                                                externalPatient.Address1 = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("4"))
                                                externalPatient.City = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("5"))
                                            {
                                                if (pair.Value.Length < 3)
                                                {
                                                    externalPatient.State = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                                }
                                            }
                                            else if (FinalIdentifier.Equals("6"))
                                                externalPatient.ZipCode = pair.Value.IsNull() ? "" : pair.Value.Replace("-", "");
                                            else if (FinalIdentifier.Equals("7"))
                                                externalPatient.PhoneNumber = pair.Value.IsNull() ? "" : pair.Value.Replace("-", "");
                                            else if (FinalIdentifier.Equals("8"))
                                                externalPatient.PreferredLanguage = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("9"))
                                                externalPatient.Email = pair.Value.IsNull() ? "" : pair.Value.Replace(" ", "").ToUpper();
                                            else if (FinalIdentifier.Equals("10"))
                                                externalPatient.AccountType = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("11"))
                                            {
                                                //Debug.WriteLine(pair.Value);
                                                if (pair.Value.Contains("-") || pair.Value.Contains("/"))
                                                {

                                                    externalPatient.DOB = pair.Value.IsNull() ? new DateTime() : DateTime.Parse(pair.Value, System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                else
                                                {
                                                    externalPatient.DOB = pair.Value.IsNull() ? new DateTime() : DateTime.FromOADate(double.Parse(pair.Value));
                                                }
                                            }
                                            else if (FinalIdentifier.Equals("12"))
                                                externalPatient.Gender = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("13"))
                                                externalPatient.GuarantarID = pair.Value.IsNull() ? "" : pair.Value;
                                            else if (FinalIdentifier.Equals("14"))
                                                externalPatient.GuarantarName = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("15"))
                                            {
                                                if (pair.Value != null && pair.Value.Contains(".") && pair.Value.Contains("E"))
                                                {
                                                    decimal LeftPart = decimal.Parse(pair.Value.Split("E")[0]);
                                                    long Multiplier = long.Parse(pair.Value.Split("E")[1]);
                                                    decimal MultiplierTemp = 1;
                                                    for (int l = 0; l < Multiplier; l++)
                                                    {
                                                        MultiplierTemp *= 10;
                                                    }
                                                    long finalSubscriberID = Convert.ToInt64(LeftPart * MultiplierTemp);
                                                    externalPatient.PrimaryInsuredID = "" + finalSubscriberID;
                                                    PrimaryInsuranceIdPresent[RowNum] = true;
                                                }
                                                else
                                                {
                                                    if (pair.Value.Contains("."))
                                                    {
                                                        externalPatient.PrimaryInsuredID = pair.Value.Replace(".0", "");
                                                        PrimaryInsuranceIdPresent[RowNum] = true;
                                                    }
                                                    else
                                                    {
                                                        externalPatient.PrimaryInsuredID = pair.Value;
                                                        PrimaryInsuranceIdPresent[RowNum] = true;
                                                    }
                                                }
                                            }
                                            else if (FinalIdentifier.Equals("16"))
                                                externalPatient.PrimaryInsuredName = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("17"))
                                                externalPatient.PrimaryInsurance = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("18"))
                                            {
                                                if (pair.Value != null && pair.Value.Contains(".") && pair.Value.Contains("E"))
                                                {
                                                    decimal LeftPart = decimal.Parse(pair.Value.Split("E")[0]);
                                                    long Multiplier = long.Parse(pair.Value.Split("E")[1]);
                                                    decimal MultiplierTemp = 1;
                                                    for (int l = 0; l < Multiplier; l++)
                                                    {
                                                        MultiplierTemp *= 10;
                                                    }
                                                    long finalSubscriberID = Convert.ToInt64(LeftPart * MultiplierTemp);
                                                    externalPatient.SecondaryInsuredID = "" + finalSubscriberID;
                                                    SecondaryInsuranceIdPresent[RowNum] = true;
                                                }
                                                else
                                                {
                                                    if (pair.Value.Contains("."))
                                                    {
                                                        externalPatient.SecondaryInsuredID = pair.Value.Replace(".0", "");
                                                        SecondaryInsuranceIdPresent[RowNum] = true;
                                                    }
                                                    else
                                                    {
                                                        externalPatient.SecondaryInsuredID = pair.Value;
                                                        SecondaryInsuranceIdPresent[RowNum] = true;
                                                    }
                                                }
                                            }
                                            else if (FinalIdentifier.Equals("19"))
                                                externalPatient.SecondaryInsuredName = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("20"))
                                            {
                                                externalPatient.SecondaryInsurance = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            }
                                            else if (FinalIdentifier.Equals("21"))
                                                externalPatient.PrimaryProvider = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("22"))
                                                externalPatient.EmployerName = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("23"))
                                                externalPatient.EmployerAddress = pair.Value.IsNull() ? "" : pair.Value.Replace(" ", "").ToUpper();
                                            else if (FinalIdentifier.Equals("24"))
                                                externalPatient.EmployerCity = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("25"))
                                                externalPatient.EmployerState = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("00"))
                                                externalPatient.EmployerZip = pair.Value.IsNull() ? "" : pair.Value;
                                            else if (FinalIdentifier.Equals("01"))
                                                externalPatient.EmployerPhone = pair.Value.IsNull() ? "" : pair.Value;
                                            else if (FinalIdentifier.Equals("02"))
                                                externalPatient.EmergencyContact = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("03"))
                                                externalPatient.EmergencyAddress = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("04"))
                                                externalPatient.EmergencyCity = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("05"))
                                                externalPatient.EmergencyState = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            else if (FinalIdentifier.Equals("06"))
                                                externalPatient.EmergencyZip = pair.Value.IsNull() ? "" : pair.Value;
                                            else if (FinalIdentifier.Equals("07"))
                                                externalPatient.EmergencyPhone = pair.Value.IsNull() ? "" : pair.Value;
                                            else if (FinalIdentifier.Equals("08"))
                                                externalPatient.PracticeNPI = pair.Value.IsNull() ? "" : pair.Value;
                                            else if (FinalIdentifier.Equals("09"))
                                                externalPatient.LocationNPI = pair.Value.IsNull() ? "" : pair.Value;
                                            else if (FinalIdentifier.Equals("010"))
                                                externalPatient.ProviderNPI = pair.Value.IsNull() ? "" : pair.Value;


                                            if (RowNum == 1)
                                            {
                                                if (ReferenceAlphabets.Equals("AI"))
                                                    TrioFirstPresent = true;
                                                if (ReferenceAlphabets.Equals("AJ"))
                                                    TrioSecondPresent = true;
                                                if (ReferenceAlphabets.Equals("AK"))
                                                    TrioThirdPresent = true;

                                                if (TrioFirstPresent && TrioSecondPresent && TrioThirdPresent)
                                                {
                                                    TakeTrioFromSheet = true;
                                                }
                                                //Debug.WriteLine("Row 2 data :::: " + TrioFirstPresent + "  " + TrioSecondPresent + "  " + TrioThirdPresent + "  " + TakeTrioFromSheet);
                                            }
                                            if (TrioFirstPresent)
                                            {
                                                if (ReferenceAlphabets.Equals("AI"))
                                                {
                                                    try
                                                    {
                                                        Practice TempPractice = PracticeData.Where(lamb => lamb.NPI.Equals(pair.Value)).FirstOrDefault();
                                                        InterPracticeID = TempPractice.ID;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        return BadRequest("Problem retrieving Practice id");
                                                    }
                                                }
                                            }
                                            if (TrioSecondPresent)
                                            {
                                                if (ReferenceAlphabets.Equals("AJ"))
                                                {
                                                    try
                                                    {
                                                        Models.Location TempLocation = LocationData.Where(lamb => lamb.NPI.Equals(pair.Value)).FirstOrDefault();
                                                        InterLocationID = TempLocation.ID;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        return BadRequest("Problem retrieving Location id");
                                                    }
                                                }
                                            }
                                            if (TrioThirdPresent)
                                            {
                                                if (ReferenceAlphabets.Equals("AK"))
                                                {
                                                    try
                                                    {
                                                        Provider TempProvider = ProviderData.Where(lamb => lamb.NPI.Equals(pair.Value)).FirstOrDefault();
                                                        InterProviderID = TempProvider.ID;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        return BadRequest("Problem retrieving Provider id");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorsCaught = true;
                                        return BadRequest("UnExpected error occured. Please contact Bellmedex\n" + ex.Message + "\n" + ex.StackTrace);
                                    }
                                }



                                //TempPatient.AccountNum = _context.GetNextSequenceValue("S_PATIENTACCOUNTNUMBER");
                                externalPatient.AddedDate = DateTime.Now;
                                externalPatient.AddedBy = UD.Email.ToUpper();
                                //TempPatient.AddedBy = "maaz@belmedex.com".ToUpper();
                                externalPatient.MergeStatus = "E";
                                externalPatient.IsActive = true;
                                externalPatient.FileName = FileName.ToUpper();

                                if (!TakeTrioFromSheet)
                                {
                                    externalPatient.PracticeID = UD.PracticeID;
                                    externalPatient.LocationId = locationID;
                                    externalPatient.ProviderID = providerID;
                                }
                                else
                                {
                                    externalPatient.PracticeID = UD.PracticeID;
                                    externalPatient.LocationId = InterLocationID;
                                    externalPatient.ProviderID = InterProviderID;
                                }

                                if (externalPatient.ExternalPatientID.IsNull())
                                {
                                    externalPatient.ExternalPatientID = DateTime.Now.Format("MMddyyyyhhmmssff");
                                }

                                if (!externalPatient.FirstName.IsNull() && !externalPatient.LastName.IsNull() && !externalPatient.Address1.IsNull())
                                {


                                    if (_context.ExternalPatient.Where(lamb => lamb.MergeStatus.Equals("A")).Where(lamb => lamb.FirstName.Trim().Replace(" ", "").Equals(externalPatient.FirstName.Trim().Replace(" ", "")) &&
                                                                                                                           lamb.LastName.Trim().Replace(" ", "").Equals(externalPatient.LastName.Trim().Replace(" ", ""))
                                                                                                                         ).Count() != 0)
                                    {
                                        externalPatient.MergeStatus = "D";
                                        _context.ExternalPatient.Add(externalPatient);
                                        //returnObject.Result = "Data not added. Duplicate Sheet";
                                        //ErrorsCaught = true;
                                        //return BadRequest("Duplicate Sheet");
                                    }
                                    //else if (!externalPatient.LastName.IsNull() && !externalPatient.FirstName.IsNull())
                                    //    _context.ExternalPatient.Add(externalPatient);
                                    else
                                    {
                                        _context.ExternalPatient.Add(externalPatient);
                                    }
                                }
                                else
                                {
                                    _context.ExternalPatient.Add(externalPatient);
                                }
                            }
                            RowNum++;
                        }
                        catch (Exception ex)
                        {
                            ErrorsCaught = true;
                            return BadRequest("UnExpected error occured. Please contact Bellmedex\n" + ex.Message + "\n" + ex.StackTrace);
                        }
                    }
                    _context.SaveChanges();
                    System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Officeally patient entry logs.txt"), "Externalpatients added . patient data method officelly\n");

                    ExternalPatientList = _context.ExternalPatient.Where(x => x.PracticeID == UD.PracticeID).ToList();
                    if (ExternalPatientList.Count() != 0)
                        ExternalPatientIDAfterInsertion = ExternalPatientList.OrderBy(lamb => lamb.ID).ElementAt(ExternalPatientList.Count() - 1).ID;
                    if (ExternalPatientList.Count() != 0)
                        ExternalPatientList = _context.ExternalPatient.Where(x => x.PracticeID == UD.PracticeID && x.ID > ExternalPatientInitialID && x.ID <= ExternalPatientIDAfterInsertion).ToList();
                    else
                        ExternalPatientList = _context.ExternalPatient.Where(x => x.PracticeID == UD.PracticeID).ToList();

                    int Idex = 0;
                    RowNum = 0;
                    foreach (ExternalPatient externalPatient in ExternalPatientList)
                    {
                        try
                        {
                            if (Idex == 177)
                            {
                                Debug.Write("11");
                            }
                            Debug.WriteLine(Idex + "   row number");
                            Patient patient = new Patient();
                            if (externalPatient.MergeStatus.Equals("E"))
                            {
                                patient.ExternalPatientID = externalPatient.ExternalPatientID;
                                patient.LastName = externalPatient.LastName.IsNull() ? "" : externalPatient.LastName.ToUpper();
                                patient.FirstName = externalPatient.FirstName.IsNull() ? "" : externalPatient.FirstName.ToUpper();
                                patient.MiddleInitial = externalPatient.MiddleInitial.IsNull() ? "" : externalPatient.MiddleInitial.ToUpper();
                                patient.Address1 = externalPatient.Address1.IsNull() ? "" : externalPatient.Address1.ToUpper();
                                patient.City = externalPatient.City.IsNull() ? "" : externalPatient.City.ToUpper();
                                patient.State = externalPatient.State.IsNull() ? "" : externalPatient.State.ToUpper();
                                patient.ZipCode = externalPatient.ZipCode;
                                patient.PhoneNumber = externalPatient.PhoneNumber;
                                patient.Email = externalPatient.Email.IsNull() ? "" : externalPatient.Email.ToUpper();
                                patient.DOB = externalPatient.DOB.HasValue ? externalPatient.DOB : new DateTime();
                                patient.Gender = externalPatient.Gender.IsNull() ? "" : externalPatient.Gender.ToUpper();
                                patient.AccountNum = _context.GetNextSequenceValue("S_PATIENTACCOUNTNUMBER");
                                patient.AddedDate = DateTime.Now;
                                patient.AddedBy = externalPatient.AddedBy.IsNull() ? "" : externalPatient.AddedBy.ToUpper();
                                patient.IsActive = externalPatient.IsActive;
                                patient.IsDeleted = externalPatient.IsDeleted;
                                patient.PracticeID = externalPatient.PracticeID;
                                patient.ProviderID = externalPatient.ProviderID;
                                patient.LocationId = externalPatient.LocationId;


                                externalPatient.MergeStatus = "A";

                                List<Patient> MatchedPatients = _context.Patient.Where(x => x.PracticeID == UD.PracticeID).Where(lamb => lamb.ExternalPatientID.Equals(patient.ExternalPatientID)).ToList();
                                if (MatchedPatients.Count() == 0)
                                {
                                    _context.Patient.Add(patient);
                                    PatientIds.Add(patient.ExternalPatientID + "|" + patient.AccountNum);

                                    //PatientsAdded[RowNum] = patient;
                                }
                                else
                                {
                                    PatientIds.Add(MatchedPatients.FirstOrDefault().ExternalPatientID + "|" + MatchedPatients.FirstOrDefault().AccountNum);
                                    //PatientsAdded[RowNum] = MatchedPatients.FirstOrDefault();
                                }
                                externalPatient.AccountNum = patient.AccountNum;
                                _context.ExternalPatient.Update(externalPatient);
                            }


                            Idex++;
                            RowNum++;
                        }
                        catch (Exception ex)
                        {
                            ErrorsCaught = true;
                            return BadRequest("UnExpected error occured. Please contact Bellmedex\n" + ex.Message + "\n" + ex.StackTrace);
                        }
                    }
                    _context.SaveChanges();
                    System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Officeally patient entry logs.txt"), "Patients Added . patient data method officelly\n");

                    //foreach (string tempstr in PatientIds)
                    //{
                    //    try
                    //    {
                    //        string ExteralPatientID = tempstr.Split('|')[0];
                    //        string AccountNumber = tempstr.Split('|')[1];
                    //        //Patient TempPatient = PatientsAdded[TempIndex];
                    //        ExternalPatient TempEP = _context.ExternalPatient.Where(x => x.PracticeID == UD.PracticeID).Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(ExteralPatientID)).FirstOrDefault();

                    //        TempEP.AccountNum = AccountNumber;
                    //        _context.ExternalPatient.Update(TempEP);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        return BadRequest("UnExpected error occured. Please contact Bellmedex\n" + ex.Message + "\n" + ex.StackTrace);
                    //    }
                    //}
                    //for(int TempIndex = 0; TempIndex < PatientIds.Count(); TempIndex++)
                    //{
                    //    if (PatientIds[TempIndex]!=null)
                    //    {
                    //        string ExteralPatientID = PatientIds[TempIndex].Split('|')[0];
                    //        string AccountNumber = PatientIds[TempIndex].Split('|')[1];
                    //        //Patient TempPatient = PatientsAdded[TempIndex];
                    //        ExternalPatient TempEP = _context.ExternalPatient.Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(ExteralPatientID)).FirstOrDefault();

                    //        TempEP.AccountNum = AccountNumber;
                    //        _context.ExternalPatient.Update(TempEP);
                    //    }
                    //}
                    //_context.SaveChanges();
                    //Adding patient plans
                    int primaryInsurances = 0, secondaryInsurances = 0;
                    PatientData = _context.Patient.Where(x => x.PracticeID == UD.PracticeID).ToList();
                    RowNum = 0;
                    string TempExternalPatientID = "";
                    List<long> insuranceplanids = new List<long>();
                    List<long> InsurancePlanIdOutliers = new List<long>();
                    List<PatientPlan> PatientPlanOutliers = new List<PatientPlan>();
                    foreach (ExternalPatient externalPatient in ExternalPatientList)
                    {
                        try
                        {
                            Debug.WriteLine(RowNum + "    ROW NUM      ");
                            if (externalPatient.MergeStatus.Equals("A"))
                            {
                                Patient TempPatient = PatientData.Where(lamb => lamb.AccountNum == externalPatient.AccountNum).FirstOrDefault();
                                Notes note = new Notes();
                                note.PracticeID = UD.PracticeID;
                                note.Note = "Data Migrated from OA, filename: " + FileName;
                                note.AddedBy = UD.Email;
                                note.AddedDate = DateTime.Now;
                                note.NotesDate = DateTime.Now;
                                note.PatientID = TempPatient.ID;

                                _context.Notes.Add(note);

                                if (!externalPatient.PrimaryInsurance.IsNull())
                                {
                                    PatientPlan TempPatientPlan = new PatientPlan();

                                    TempPatientPlan.FirstName = externalPatient.FirstName;
                                    TempPatientPlan.LastName = externalPatient.LastName;
                                    TempPatientPlan.PatientID = TempPatient.ID;
                                    TempPatientPlan.DOB = externalPatient.DOB;
                                    TempPatientPlan.Gender = externalPatient.Gender;
                                    TempPatientPlan.Email = externalPatient.Email;
                                    TempPatientPlan.Address1 = externalPatient.Address1;
                                    TempPatientPlan.City = externalPatient.City;
                                    TempPatientPlan.State = externalPatient.State;
                                    TempPatientPlan.ZipCode = externalPatient.ZipCode;
                                    TempPatientPlan.PhoneNumber = externalPatient.PhoneNumber;
                                    TempPatientPlan.SubscriberId = externalPatient.PrimaryInsuredID;
                                    TempPatientPlan.Coverage = "P";
                                    TempPatientPlan.IsActive = true;
                                    TempPatientPlan.IsDeleted = false;
                                    TempPatientPlan.AddedDate = DateTime.Today.Date;
                                    TempPatientPlan.AddedBy = UD == null ? "" : UD.Email.ToUpper();
                                    TempPatientPlan.RelationShip = 18 + "";
                                    string TempString = externalPatient.PrimaryInsurance;
                                    InsurancePlan PossibleCandidate = InsurancePlanData.Where(lamb => lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(externalPatient.PrimaryInsurance.Trim().ToUpper().Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                    if (TempPatientPlan.SubscriberId == "JZH124857136001")
                                    {
                                        Debug.WriteLine(TempString);
                                    }
                                    if (PossibleCandidate != null)
                                    {
                                        TempPatientPlan.InsurancePlanID = PossibleCandidate.ID;
                                    }
                                    else
                                    {
                                        ExInsuranceMapping MappingRecord = ExInsuranceMappingData.Where(lamb => lamb.ExternalInsuranceName != null ? lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(externalPatient.PrimaryInsurance.Trim().ToUpper().Replace(" ", "")) : false).FirstOrDefault();
                                        if (MappingRecord != null)
                                        {
                                            InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == MappingRecord.InsurancePlanID).FirstOrDefault();

                                            if (TempInsurancePlan != null)
                                            {
                                                TempPatientPlan.InsurancePlanID = MappingRecord.InsurancePlanID;
                                            }
                                        }
                                        else
                                        {
                                            TempPatientPlan.InsurancePlanID = null;
                                        }
                                    }

                                    if (TempPatientPlan.InsurancePlanID != null)
                                    {
                                        _context.PatientPlan.Add(TempPatientPlan);

                                        //ExternalPatient TempEP = _context.ExternalPatient.Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(TempExternalPatientID)).FirstOrDefault();
                                        //TempEP.PrimaryPatientPlanID = TempPatientPlan.ID;

                                        //_context.ExternalPatient.Update(TempEP);
                                        PatientPlanWithID TempPPWID = new PatientPlanWithID();
                                        TempPPWID.PatientPlan = TempPatientPlan;
                                        TempPPWID.AccountNumber = externalPatient.ExternalPatientID;

                                        PrimaryPatientPlanWithIDs.Add(TempPPWID);
                                    }
                                }
                                if (!externalPatient.SecondaryInsurance.IsNull())
                                {
                                    PatientPlan TempPatientPlan = new PatientPlan();

                                    TempPatientPlan.FirstName = externalPatient.FirstName;
                                    TempPatientPlan.LastName = externalPatient.LastName;
                                    TempPatientPlan.PatientID = TempPatient.ID;
                                    TempPatientPlan.DOB = externalPatient.DOB;
                                    TempPatientPlan.Gender = externalPatient.Gender;
                                    TempPatientPlan.Email = externalPatient.Email;
                                    TempPatientPlan.Address1 = externalPatient.Address1;
                                    TempPatientPlan.City = externalPatient.City;
                                    TempPatientPlan.State = externalPatient.State;
                                    TempPatientPlan.ZipCode = externalPatient.ZipCode;
                                    TempPatientPlan.PhoneNumber = externalPatient.PhoneNumber;
                                    TempPatientPlan.SubscriberId = externalPatient.SecondaryInsuredID;
                                    TempPatientPlan.Coverage = "S";
                                    TempPatientPlan.IsActive = true;
                                    TempPatientPlan.IsDeleted = false;
                                    TempPatientPlan.AddedDate = DateTime.Today.Date;
                                    TempPatientPlan.AddedBy = UD == null ? "" : UD.Email.ToUpper();
                                    TempPatientPlan.RelationShip = 18 + "";
                                    string chec = externalPatient.SecondaryInsurance;
                                    InsurancePlan PossibleCandidate = InsurancePlanData.Where(lamb => lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(externalPatient.SecondaryInsurance.Trim().ToUpper().Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                    if (PossibleCandidate != null)
                                    {
                                        TempPatientPlan.InsurancePlanID = PossibleCandidate.ID;
                                    }
                                    else
                                    {
                                        ExInsuranceMapping MappingRecord = ExInsuranceMappingData.Where(lamb => lamb.ExternalInsuranceName != null ? lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(externalPatient.SecondaryInsurance.Trim().ToUpper().Replace(" ", "")) : false).FirstOrDefault();
                                        if (MappingRecord != null)
                                        {
                                            InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == MappingRecord.InsurancePlanID).FirstOrDefault();

                                            if (TempInsurancePlan != null)
                                            {
                                                TempPatientPlan.InsurancePlanID = MappingRecord.InsurancePlanID;
                                            }
                                        }
                                        else
                                        {
                                            TempPatientPlan.InsurancePlanID = null;
                                        }
                                    }

                                    if (TempPatientPlan.InsurancePlanID != null)
                                    {
                                        _context.PatientPlan.Add(TempPatientPlan);

                                        //ExternalPatient TempEP = _context.ExternalPatient.Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(TempExternalPatientID)).FirstOrDefault();
                                        //TempEP.PrimaryPatientPlanID = TempPatientPlan.ID;

                                        //_context.ExternalPatient.Update(TempEP);
                                        PatientPlanWithID TempPPWID = new PatientPlanWithID();
                                        TempPPWID.PatientPlan = TempPatientPlan;
                                        TempPPWID.AccountNumber = externalPatient.ExternalPatientID;

                                        SecondaryPatientPlanWithIDs.Add(TempPPWID);
                                    }
                                }
                            }
                            RowNum++;
                        }
                        catch (Exception ex)
                        {
                            ErrorsCaught = true;
                            return BadRequest("UnExpected error occured. Please contact Bellmedex" + ex.Message + "  " + ex.StackTrace);
                        }
                    }
                    
                    _context.SaveChanges();
                    System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Officeally patient entry logs.txt"), "Plans added . patient data method officelly\n");


                    foreach (PatientPlanWithID PrimaryPatientPlanWithID in PrimaryPatientPlanWithIDs)
                    {
                        if (PrimaryPatientPlanWithID.AccountNumber != null)
                        {
                            ExternalPatient TempEP = ExternalPatientList.Where(x => x.PracticeID == UD.PracticeID).Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(PrimaryPatientPlanWithID.AccountNumber)).FirstOrDefault();
                            TempEP.PrimaryPatientPlanID = PrimaryPatientPlanWithID.PatientPlan.ID;
                            _context.ExternalPatient.Update(TempEP);
                        }
                    }
                    foreach (PatientPlanWithID SecondaryPatientPlanWithID in SecondaryPatientPlanWithIDs)
                    {
                        if (SecondaryPatientPlanWithID.AccountNumber != null)
                        {
                            ExternalPatient TempEP = ExternalPatientList.Where(x => x.PracticeID == UD.PracticeID).Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(SecondaryPatientPlanWithID.AccountNumber)).FirstOrDefault();
                            TempEP.SecondaryPatientPlanID = SecondaryPatientPlanWithID.PatientPlan.ID;
                            _context.ExternalPatient.Update(TempEP);
                        }
                    }
                    System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Officeally patient entry logs.txt"), "Plans linked  patient data method officelly\n");

                    _context.SaveChanges();
                    returnObject.Result = "Data Added Successfully";
                }

                //await _context.SaveChangesAsync();
                //objTrnScope.Complete();
                //objTrnScope.Dispose();

                return Ok(returnObject);
            }
            catch (Exception ex)
            {
                ErrorsCaught = true;
                return BadRequest("UnExpected error occured. Please contact Bellmedex" + ex.Message + "  " + ex.StackTrace);
            }
            //}
        }

        public static bool CompareInsurancePlans(InsurancePlan plan1, InsurancePlan plan2)
        {
            bool ToBeAdded = true;

            if (plan1.PlanName != plan2.PlanName)
                ToBeAdded = false;
            else if (plan1.Edi837PayerID != plan2.Edi837PayerID)
                ToBeAdded = false;
            else if (plan1.Description != plan2.Description)
                ToBeAdded = false;
            else if (plan1.SubmissionType != plan2.SubmissionType)
                ToBeAdded = false;
            else if (plan1.IsActive != plan2.IsActive)
                ToBeAdded = false;
            else if (plan1.IsDeleted != plan2.IsDeleted)
                ToBeAdded = false;
            return ToBeAdded;
        }
        public int GetColumnQualifier(char Reference)
        {
            int[] values = new int[] { 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90 };
            List<int> intValues = values.ToList();
            return intValues.IndexOf((int)Reference);
        }
        public static CellReferenceValuePair GetValueFromCell(Cell InputCell, SharedStringTable Sst)
        {
            if ((InputCell.DataType != null) && (InputCell.DataType == CellValues.SharedString))
            {

                int ssid = int.Parse(InputCell.CellValue.Text);
                string str = Sst.ChildElements[ssid].InnerText;
                //Debug.WriteLine("Shared string {0}: {1}", ssid, str);
                CellReferenceValuePair pair = new CellReferenceValuePair();
                pair.Reference = InputCell.CellReference;
                pair.Value = str;
                return pair;
            }
            else if (InputCell.CellValue != null)
            {
                //Debug.WriteLine("Cell contents: {0}", InputCell.CellValue.Text);
                CellReferenceValuePair pair = new CellReferenceValuePair();
                pair.Reference = InputCell.CellReference;
                pair.Value = InputCell.CellValue.Text;
                return pair;
            }
            return null;
        }
        public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        {
            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(filePath, fileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                inputStream.CopyTo(outputFileStream);
            }
        }
        [HttpPost]
        [Route("FixInsuredIDs")]
        public void FixInsuredIDs()
        {
            List<ExternalPatient> ExternalPatientData = _context.ExternalPatient.Where(pp=>pp.PrimaryInsuredID.Contains(".") && pp.PrimaryInsuredID.Contains("E")).ToList();

            foreach(ExternalPatient temp in ExternalPatientData)
            {
                decimal LeftPart = decimal.Parse(temp.PrimaryInsuredID.Split("E")[0]);
                long Multiplier = long.Parse(temp.PrimaryInsuredID.Split("E")[1]);
                decimal MultiplierTemp = 1;
                for(int i = 0; i < Multiplier; i++)
                {
                    MultiplierTemp *= 10;
                }
                long finalSubscriberID = Convert.ToInt64(LeftPart * MultiplierTemp);

                temp.SecondaryGroup = ""+finalSubscriberID;
                _context.ExternalPatient.Update(temp);
            }
            _context.SaveChanges();
        }


        [HttpPost]
        [Route("ProcessRemainingPatientPlan")]
        public async Task<ActionResult> ProcessRemainingPatientPlan()
        {
            try
            {
                List<ExternalPatient> TargetExternalPatients = _context.ExternalPatient.
                    Where(m => !m.AccountNum.IsNull() && m.PrimaryPatientPlanID.IsNull()).ToList();
                UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
                    User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
                    User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
                List<PatientPlanWithID> PrimaryPatientPlanWithIDs = new List<PatientPlanWithID>();
                List<PatientPlanWithID> SecondaryPatientPlanWithIDs = new List<PatientPlanWithID>();
                List<PatientPlan> PatientPlanData = _context.PatientPlan.ToList();
                List<Patient> PatientData = _context.Patient.Where(x => x.PracticeID == UD.PracticeID).ToList();
                List<InsurancePlan> InsurancePlanData = _context.InsurancePlan.ToList();
                List<ExInsuranceMapping> ExInsuranceMappingData = _contextMain.ExInsuranceMapping.ToList();

                List<string> AddedExMappingEntries = new List<string>();

                foreach (ExternalPatient ExP in TargetExternalPatients)
                {
                    try
                    {
                        Patient TempPatient = PatientData.Where(lamb => lamb.AccountNum == ExP.AccountNum).FirstOrDefault();
                        List<PatientPlan> TempPatientPlans = PatientPlanData.Where(ppd => ppd.PatientID == TempPatient.ID).ToList();

                        long PrimaryPlanID = 0, SecondaryPlanID = 0;

                        InsurancePlan PrimaryCandidate = null;
                        InsurancePlan SecondaryCandidate = null;

                        if (!ExP.PrimaryInsurance.IsNull())
                        {
                            PrimaryCandidate = InsurancePlanData.Where(lamb => lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(ExP.PrimaryInsurance.Trim().ToUpper().Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                            if (PrimaryCandidate != null)
                            {
                                PrimaryPlanID = PrimaryCandidate.ID;
                            }
                            else
                            {
                                ExInsuranceMapping MappingRecord = ExInsuranceMappingData.Where(lamb => lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(ExP.PrimaryInsurance.Trim().ToUpper().Replace(" ", ""))).FirstOrDefault();
                                if (MappingRecord != null)
                                {
                                    if (MappingRecord.InsurancePlanID.GetValueOrDefault() != 0)
                                    {
                                        PrimaryPlanID = MappingRecord.InsurancePlanID.GetValueOrDefault();
                                    }

                                }
                            }
                        }
                        if (!ExP.SecondaryInsurance.IsNull())
                        {
                            SecondaryCandidate = InsurancePlanData.Where(lamb => lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(ExP.SecondaryInsurance.Trim().ToUpper().Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                            if (SecondaryCandidate != null)
                            {
                                SecondaryPlanID = SecondaryCandidate.ID;
                            }
                            else
                            {
                                ExInsuranceMapping MappingRecord = ExInsuranceMappingData.Where(lamb => lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(ExP.SecondaryInsurance.Trim().ToUpper().Replace(" ", ""))).FirstOrDefault();
                                if (MappingRecord != null)
                                {
                                    if (MappingRecord.InsurancePlanID.GetValueOrDefault() != 0)
                                    {
                                        SecondaryPlanID = MappingRecord.InsurancePlanID.GetValueOrDefault();
                                    }

                                }
                            }
                        }

                        bool PrimaryPlanPresent = TempPatientPlans.Where(tpp => tpp.InsurancePlanID == PrimaryPlanID && tpp.Coverage == "P").Count() != 0 ? true : false;
                        bool SecondaryPlanPresent = TempPatientPlans.Where(tpp => tpp.InsurancePlanID == SecondaryPlanID && tpp.Coverage == "S").Count() != 0 ? true : false;

                        if (!ExP.PrimaryInsurance.IsNull() && !PrimaryPlanPresent)
                        {
                            PatientPlan TempPatientPlan = new PatientPlan();

                            TempPatientPlan.FirstName = ExP.FirstName;
                            TempPatientPlan.LastName = ExP.LastName;
                            TempPatientPlan.PatientID = TempPatient.ID;
                            TempPatientPlan.DOB = ExP.DOB;
                            TempPatientPlan.Gender = ExP.Gender;
                            TempPatientPlan.Email = ExP.Email;
                            TempPatientPlan.Address1 = ExP.Address1;
                            TempPatientPlan.City = ExP.City;
                            TempPatientPlan.State = ExP.State;
                            TempPatientPlan.ZipCode = ExP.ZipCode;
                            TempPatientPlan.PhoneNumber = ExP.PhoneNumber;
                            TempPatientPlan.SubscriberId = ExP.PrimaryInsuredID;
                            TempPatientPlan.Coverage = "P";
                            TempPatientPlan.IsActive = true;
                            TempPatientPlan.IsDeleted = false;
                            TempPatientPlan.AddedDate = DateTime.Today.Date;
                            TempPatientPlan.AddedBy = UD == null ? "" : UD.Email.ToUpper();
                            TempPatientPlan.RelationShip = 18 + "";
                            string TempString = ExP.PrimaryInsurance;
                            if (PrimaryPlanID != 0)
                            {
                                TempPatientPlan.InsurancePlanID = PrimaryPlanID;
                            }


                            if (TempPatientPlan.InsurancePlanID != null)
                            {
                                if (InsurancePlanData.Select(ip => ip.ID).Contains(TempPatientPlan.InsurancePlanID.Value))
                                {
                                    _context.PatientPlan.Add(TempPatientPlan);

                                    //ExternalPatient TempEP = _context.ExternalPatient.Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(TempExternalPatientID)).FirstOrDefault();
                                    //TempEP.PrimaryPatientPlanID = TempPatientPlan.ID;

                                    //_context.ExternalPatient.Update(TempEP);
                                    PatientPlanWithID TempPPWID = new PatientPlanWithID();
                                    TempPPWID.PatientPlan = TempPatientPlan;
                                    TempPPWID.AccountNumber = ExP.ExternalPatientID;

                                    PrimaryPatientPlanWithIDs.Add(TempPPWID);
                                }
                                else
                                {
                                    ExP.MissingInfo = ExP.MissingInfo + ". Primary patient plan was not created";
                                    _context.Update(ExP);
                                }

                            }
                            else
                            {
                                ExP.MissingInfo = ExP.MissingInfo + ". Primary patient plan was not created";
                                _context.Update(ExP);
                                if (_contextMain.ExInsuranceMapping.Where(exim => exim.ExternalInsuranceName.Equals(ExP.PrimaryInsurance)).Count() == 0 && !AddedExMappingEntries.Contains(ExP.PrimaryInsurance))
                                {
                                    //ExInsuranceMapping mapping = new ExInsuranceMapping();
                                    //mapping.Status = "F";
                                    //mapping.ExternalInsuranceName = ExP.PrimaryInsurance;
                                    //mapping.AddedBy = UD.Email;
                                    //mapping.AddedDate = DateTime.Now;
                                    //AddedExMappingEntries.Add(ExP.PrimaryInsurance);
                                    //_contextMain.ExInsuranceMapping.Add(mapping);
                                }
                            }
                        }
                        else if (!ExP.PrimaryInsurance.IsNull() && PrimaryPlanPresent)
                        {
                            PatientPlanWithID TempPPWID = new PatientPlanWithID();
                            TempPPWID.PatientPlan = TempPatientPlans.Where(tpp => tpp.InsurancePlanID == PrimaryPlanID && tpp.Coverage == "P").FirstOrDefault();
                            TempPPWID.AccountNumber = ExP.ExternalPatientID;

                            PrimaryPatientPlanWithIDs.Add(TempPPWID);
                        }

                        if (!ExP.SecondaryInsurance.IsNull() && !SecondaryPlanPresent)
                        {
                            PatientPlan TempPatientPlan = new PatientPlan();

                            TempPatientPlan.FirstName = ExP.FirstName;
                            TempPatientPlan.LastName = ExP.LastName;
                            TempPatientPlan.PatientID = TempPatient.ID;
                            TempPatientPlan.DOB = ExP.DOB;
                            TempPatientPlan.Gender = ExP.Gender;
                            TempPatientPlan.Email = ExP.Email;
                            TempPatientPlan.Address1 = ExP.Address1;
                            TempPatientPlan.City = ExP.City;
                            TempPatientPlan.State = ExP.State;
                            TempPatientPlan.ZipCode = ExP.ZipCode;
                            TempPatientPlan.PhoneNumber = ExP.PhoneNumber;
                            TempPatientPlan.SubscriberId = ExP.SecondaryInsuredID;
                            TempPatientPlan.Coverage = "S";
                            TempPatientPlan.IsActive = true;
                            TempPatientPlan.IsDeleted = false;
                            TempPatientPlan.AddedDate = DateTime.Today.Date;
                            TempPatientPlan.AddedBy = UD == null ? "" : UD.Email.ToUpper();
                            TempPatientPlan.RelationShip = 18 + "";
                            string chec = ExP.SecondaryInsurance;
                            if (SecondaryPlanID != 0)
                            {
                                TempPatientPlan.InsurancePlanID = SecondaryPlanID;
                            }


                            if (TempPatientPlan.InsurancePlanID != null)
                            {
                                if (InsurancePlanData.Select(ip => ip.ID).Contains(TempPatientPlan.InsurancePlanID.Value))
                                {
                                    _context.PatientPlan.Add(TempPatientPlan);

                                    //ExternalPatient TempEP = _context.ExternalPatient.Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(TempExternalPatientID)).FirstOrDefault();
                                    //TempEP.PrimaryPatientPlanID = TempPatientPlan.ID;

                                    //_context.ExternalPatient.Update(TempEP);
                                    PatientPlanWithID TempPPWID = new PatientPlanWithID();
                                    TempPPWID.PatientPlan = TempPatientPlan;
                                    TempPPWID.AccountNumber = ExP.ExternalPatientID;

                                    SecondaryPatientPlanWithIDs.Add(TempPPWID);
                                }
                                else
                                {
                                    ExP.MissingInfo = ExP.MissingInfo + ". Secondary patient plan was not created";
                                    _context.Update(ExP);
                                }

                            }
                            else
                            {
                                ExP.MissingInfo = ExP.MissingInfo + ". Secondary patient plan was not created";
                                _context.Update(ExP);
                                if (_contextMain.ExInsuranceMapping.Where(exim => exim.ExternalInsuranceName.Equals(ExP.PrimaryInsurance)).Count() == 0 && !AddedExMappingEntries.Contains(ExP.PrimaryInsurance))
                                {
                                    ExInsuranceMapping mapping = new ExInsuranceMapping();
                                    mapping.Status = "F";
                                    mapping.ExternalInsuranceName = ExP.PrimaryInsurance;
                                    mapping.AddedDate = DateTime.Now;
                                    mapping.AddedBy = UD.Email;

                                    AddedExMappingEntries.Add(ExP.PrimaryInsurance);
                                    _contextMain.ExInsuranceMapping.Add(mapping);
                                }
                            }
                        }
                        else if (!ExP.SecondaryInsurance.IsNull() && SecondaryPlanPresent)
                        {
                            PatientPlanWithID TempPPWID = new PatientPlanWithID();
                            TempPPWID.PatientPlan = TempPatientPlans.Where(tpp => tpp.InsurancePlanID == SecondaryPlanID && tpp.Coverage == "S").FirstOrDefault();
                            TempPPWID.AccountNumber = ExP.ExternalPatientID;

                            SecondaryPatientPlanWithIDs.Add(TempPPWID);
                        }
                    }
                    catch (Exception ex)
                    {
                        //return BadRequest("Something went wrong. Please contact BellMedEx");
                    }
                }
                _context.SaveChanges();

                foreach (PatientPlanWithID PrimaryPatientPlanWithID in PrimaryPatientPlanWithIDs)
                {
                    try
                    {
                        if (PrimaryPatientPlanWithID.AccountNumber != null)
                        {
                            ExternalPatient TempEP = TargetExternalPatients.Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(PrimaryPatientPlanWithID.AccountNumber)).FirstOrDefault();
                            TempEP.PrimaryPatientPlanID = PrimaryPatientPlanWithID.PatientPlan.ID;
                            _context.ExternalPatient.Update(TempEP);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Something went wrong. Please contact BellMedEx");
                    }
                }
                foreach (PatientPlanWithID SecondaryPatientPlanWithID in SecondaryPatientPlanWithIDs)
                {
                    try
                    {
                        if (SecondaryPatientPlanWithID.AccountNumber != null)
                        {
                            ExternalPatient TempEP = TargetExternalPatients.Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(SecondaryPatientPlanWithID.AccountNumber)).FirstOrDefault();
                            TempEP.SecondaryPatientPlanID = SecondaryPatientPlanWithID.PatientPlan.ID;
                            _context.ExternalPatient.Update(TempEP);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Something went wrong. Please contact BellMedEx");
                    }
                }
                _contextMain.SaveChanges();
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong. Please contact BellMedEx");
            }
        }

        // Saad Method is Added on Dated 03/27/2020  04.40 PM
        [HttpPost]
        [Route("AddChargesDataFromSmartSheetExcel")]
        public async Task<ActionResult> AddChargesDataFromSmartSheetExcel(VMDataMigration InputData)
        {
            try
            {
                ReturnObject returnObject = new ReturnObject();
                UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
                    User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
                    User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
                long ClientID = UD.ClientID;
                int ChargesAdded = 0, GroupsAdded = 0, VisitsAdded = 0, ExternalChargesAdded = 0;

                List<Patient> PatientData = _context.Patient.ToList();
                List<InsurancePlan> InsurancePlanData = _context.InsurancePlan.ToList();
                List<ExternalPatient> ExternalPatientData = _context.ExternalPatient.ToList();
                List<ExInsuranceMapping> ExInsuranceMappingData = _contextMain.ExInsuranceMapping.ToList();
                List<PatientPlan> PatientPlanData = _context.PatientPlan.ToList();
                List<ExternalCharge> ExternalChargeData = _context.ExternalCharge.ToList();
                List<RefProvider> RefProviderData = _context.RefProvider.ToList();
                List<PlanType> PlanTypeData = _context.PlanType.ToList();
                List<POS> POSTable = _context.POS.ToList();
                List<Cpt> CptTable = _context.Cpt.ToList();
                List<ICD> ICDTable = _context.ICD.ToList();
                List<Visit> VisitData = _context.Visit.Where(v => v.PracticeID == UD.PracticeID).ToList();
                List<Charge> ChargeData = _context.Charge.Where(v => v.PracticeID == UD.PracticeID).ToList();
                List<Modifier> ModifierTable = _context.Modifier.ToList();
                ICD DummyIcd = _context.ICD.Where(lamb => lamb.ICDCode.Equals("99999999")).FirstOrDefault();
                if (DummyIcd == null)
                {
                    DummyIcd = new ICD();
                    DummyIcd.ICDCode = "99999999";
                    DummyIcd.Description = "Dummy ICD";
                    DummyIcd.AddedDate = DateTime.Now;
                    DummyIcd.AddedBy = UD.Email;
                    DummyIcd.IsActive = true;
                    DummyIcd.IsDeleted = false;
                    _context.ICD.Add(DummyIcd);
                    _context.SaveChanges();
                }
                long ExternalChargeInitialID = 0, ExternalPatientIDAfterInsertion = 0;

                if (ExternalChargeData.Count() != 0)
                {
                    ExternalChargeInitialID = ExternalChargeData.OrderBy(lamb => lamb.ID).ElementAt(ExternalChargeData.Count() - 1).ID;
                }

                byte[] bytes = Convert.FromBase64String(InputData.UploadModel.Content.Substring(InputData.UploadModel.Content.IndexOf("base64,") + 7));
                string FileName = InputData.UploadModel.Name;

            Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            string OutputPath = Path.Combine("\\\\", settings.DocumentServerURL,
                    settings.DocumentServerDirectory, "DataMigration", UD.ClientID + "",
                    DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));

            if (System.IO.Directory.Exists(OutputPath))
            {
                System.IO.File.WriteAllBytes(Path.Combine(OutputPath, FileName), bytes);
            }
            else
            {
                System.IO.Directory.CreateDirectory(OutputPath);
                System.IO.File.WriteAllBytes(Path.Combine(OutputPath, FileName), bytes);
            }

                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    long practiceID;
                    long providerID;
                    long locationID;
                    long InterPracticeID = 0;
                    long InterProviderID = 0;
                    long InterLocationID = 0;
                    int RowNum = 0;
                    //Stream Stream = contents;

                    practiceID = InputData.PracticeID;
                    providerID = InputData.ProviderID;
                    locationID = InputData.LocationID;

                    if (InputData.PracticeID.IsNull())
                        return BadRequest("Practice id not present");
                    if (InputData.ProviderID.IsNull())
                        return BadRequest("Provider id not present");
                    if (InputData.LocationID.IsNull())
                        return BadRequest("Location id not present");



                    //SaveStreamAsFile("D:\\", Stream, "streamedfile.xlsx");

                    SpreadsheetDocument Doc = SpreadsheetDocument.Open(stream, false);

                    WorkbookPart WorkbookPart = Doc.WorkbookPart;
                    WorksheetPart WorksheetPart = WorkbookPart.WorksheetParts.First();
                    Worksheet Sheet = WorksheetPart.Worksheet;

                SharedStringTablePart SstPart = WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();

                SharedStringTable Sst = SstPart.SharedStringTable;

                var AllCells = Sheet.Descendants<Cell>();
                var AllRows = Sheet.Descendants<Row>();
                bool TakeTrioFromSheet = false, TrioFirstPresent = false, TrioSecondPresent = false, TrioThirdPresent = false;

                Debug.WriteLine(AllRows.Count() + "  " + AllCells.Count());

                //Adding Patients

                bool[] PrimaryInsuranceIdPresent = new bool[AllRows.Count()];
                bool[] SecondaryInsuranceIdPresent = new bool[AllRows.Count()];

                List<List<ExternalCharge>> FinalChargesData = new List<List<ExternalCharge>>();
                List<string> VisitAmounts = new List<string>();

                List<ExternalCharge> CurrentPatientCharges = new List<ExternalCharge>();
                ExternalCharge TempChargeWithPatientInfo = new ExternalCharge();



                decimal VisitCharge = 0, VisitBalance = 0, VisitAdj = 0, VisitPatientpayment = 0, VisitInsurancepayment = 0;

                long GroupID = 1, InvalidGroupID = 0;


                    if (ExternalChargeData.OrderByDescending(lamb => lamb.GroupID).Count() != 0)
                    {
                        GroupID = ExternalChargeData.OrderByDescending(lamb => lamb.GroupID).FirstOrDefault() != null ? ExternalChargeData.OrderByDescending(lamb => lamb.GroupID).FirstOrDefault().GroupID + 1 : 0;
                    }
                    ExternalCharge PreviousRow = null;
                    List<PatientPlanWithPatient> PatientPlanMapping = new List<PatientPlanWithPatient>();
                    for (int RowIndex = 1; RowIndex < AllRows.Count(); RowIndex++)
                    {
                        try
                        {
                            ExternalCharge TempCharge = new ExternalCharge();
                            bool IsChargeDataPresent = false;
                            List<Cell> RowCells = AllRows.ElementAt(RowIndex).Elements<Cell>().ToList();
                            bool SummationIdentifier = true;
                            ICD ICDTemp = null;

                            //Debug.Write("start of row");
                            Cpt tempcpt = null;
                            for (int ColIndex = 0; ColIndex < RowCells.Count(); ColIndex++)
                            {
                                try
                                {
                                    CellReferenceValuePair pair = GetValueFromCell(RowCells.ElementAt(ColIndex), Sst);

                                if (pair != null && pair.Value != null && !pair.Reference.IsNull() && pair.Value != "")
                                {
                                    //Debug.Write(pair.Reference+ "       "+pair.Value);
                                    string ReferenceAlphabets = new String(pair.Reference.Where(Char.IsLetter).ToArray());
                                    string FinalIdentifier = "";
                                    pair.Reference = new String(pair.Reference.Where(Char.IsLetter).ToArray());

                                    for (int StrIndex = 0; StrIndex < pair.Reference.Length; StrIndex++)
                                    {
                                        FinalIdentifier += GetColumnQualifier(pair.Reference[StrIndex]);
                                    }


                                    if (FinalIdentifier.Equals("1"))
                                    {

                                            string CompleteName = pair.Value.Trim().Replace(",,",",");

                                            if (CompleteName.Contains(","))
                                            {
                                                string[] FirstSplit = CompleteName.Split(',');
                                                string[] SecondSplit = FirstSplit[1].Trim().Split(' ');
                                                string FirstName = "", MI = "";
                                                if (SecondSplit.Length > 1)
                                                {
                                                    string temp = SecondSplit[1];
                                                    if (temp.Length > 3)
                                                    {
                                                        FirstName = SecondSplit[0].Trim().ToUpper() + " " + SecondSplit[1].Trim().ToUpper();
                                                        if (SecondSplit.Length > 2)
                                                        {
                                                            MI = SecondSplit[2];
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FirstName = SecondSplit[0].Trim().ToUpper();
                                                        TempCharge.FirstName = SecondSplit[0].Trim().ToUpper();
                                                        //if (SecondSplit.Length > 1)
                                                        //    TempCharge.MiddleInitial = SecondSplit[1].Trim().ToUpper();
                                                        //else
                                                        //    TempCharge.MiddleInitial = "";
                                                        //MI = TempCharge.MiddleInitial;
                                                    }
                                                }
                                                else
                                                {
                                                    FirstName = SecondSplit[0].Trim().ToUpper();
                                                }
                                                TempCharge.LastName = FirstSplit[0].Trim().ToUpper();
                                                TempCharge.FirstName = FirstName;
                                                //TempCharge.MiddleInitial = MI;
                                            }
                                            else 
                                            {
                                                string[] SpaceSplit = CompleteName.Split(' ');
                                                if (SpaceSplit.Length > 2)
                                                {
                                                    TempCharge.FirstName = SpaceSplit[1];
                                                    TempCharge.LastName = SpaceSplit[0];
                                                    TempCharge.MiddleInitial = SpaceSplit[2];
                                                }
                                                else if(SpaceSplit.Length>1 && SpaceSplit.Length < 3)
                                                {
                                                    TempCharge.FirstName = SpaceSplit[1];
                                                    TempCharge.LastName = SpaceSplit[0];
                                                }
                                                else
                                                {
                                                    TempCharge.LastName = CompleteName;
                                                    if(TempCharge.ErrorMessage != null)
                                                        TempCharge.ErrorMessage = TempCharge.ErrorMessage + "Patient Not found. ";
                                                    else 
                                                        TempCharge.ErrorMessage = "Patient Not found. ";
                                                }
                                            }
                                        }

                                    if (FinalIdentifier.Equals("4"))
                                    {
                                        //TempChargeWithPatientInfo.DOB = DateTime.FromOADate(pair.Value).ToString(@"dd\/MM\/yyyy");
                                        //TempCharge.DOB = DateTime.FromOADate(double.Parse(pair.Value, System.Globalization.CultureInfo.InvariantCulture)).ToString(@"MM\/dd\/yyyy");
                                        if (pair.Value.Contains("/") || pair.Value.Contains("-"))
                                        {
                                            TempCharge.DOB = DateTime.Parse(pair.Value, System.Globalization.CultureInfo.InvariantCulture).ToString(@"MM\/dd\/yyyy");
                                        }
                                        else
                                        {
                                            TempCharge.DOB = DateTime.FromOADate(double.Parse(pair.Value)).ToString(@"MM\/dd\/yyyy");

                                        }
                                    }
                                    #region resource assignment
                                    if (FinalIdentifier.Equals("0"))
                                    {
                                        // Debug.Write(pair.Value + " " + RowIndex);
                                        IsChargeDataPresent = true;
                                        if (pair.Value.Contains("/") || pair.Value.Contains("-"))
                                        {
                                            TempCharge.DateOfService = DateTime.Parse(pair.Value, System.Globalization.CultureInfo.InvariantCulture);
                                        }
                                        else
                                        {
                                            TempCharge.DateOfService = DateTime.FromOADate(double.Parse(pair.Value));

                                        }

                                    }

                                    if (FinalIdentifier.Equals("2"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        TempCharge.SheetName = pair.Value;

                                    }
                                    if (FinalIdentifier.Equals("3"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        TempCharge.PrescribingMD = pair.Value;

                                    }
                                    if (FinalIdentifier.Equals("5"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        TempCharge.ReportType = pair.Value;

                                    }
                                    if (FinalIdentifier.Equals("6"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        TempCharge.DiagnosisCode = pair.Value;
                                    }
                                    if (FinalIdentifier.Equals("7"))
                                    {
                                        TempCharge.CptCode = pair.Value.Replace(".0", "");

                                        tempcpt = CptTable.Where(c => c.CPTCode.Equals(TempCharge.CptCode)).FirstOrDefault();
                                        if (tempcpt != null)
                                        {
                                            TempCharge.Balance = tempcpt.Amount;
                                            TempCharge.Charges = tempcpt.Amount;
                                            TempCharge.CPTID = tempcpt.ID;
                                        }
                                    }
                                    if (FinalIdentifier.Equals("8"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        TempCharge.NotBilled = pair.Value;

                                    }
                                    if (FinalIdentifier.Equals("10"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        TempCharge.NeedDemos = pair.Value;

                                    }
                                    if (FinalIdentifier.Equals("11"))
                                    {
                                        //Debug.WriteLine(pair.Value);
                                        TempCharge.Remarks = pair.Value;

                                    }
                                    #endregion

                                }

                            }
                            catch (Exception ex)
                            {
                                return BadRequest("Unexpected Error Occurred. Please Contect BellMedEx\n" + ex.Message + "\n" + ex.StackTrace);
                            }
                        }

                            POS Posid = POSTable.Where(c => c.PosCode.Equals("11")).FirstOrDefault();
                            TempCharge.POSID = Posid.ID;
                            Patient PatientDat = null;
                            if (TempCharge.DOB != null)
                            {
                                if(TempCharge.FirstName != null && TempCharge.LastName != null)
                                    PatientDat = PatientData.Where(c => (c.DOB.HasValue ? c.DOB.Value.Date.Equals(DateTime.Parse(TempCharge.DOB, System.Globalization.CultureInfo.InvariantCulture).Date) : true) &&
                                                                c.FirstName.ToUpper().Equals(TempCharge.FirstName.ToUpper()) &&
                                                                c.LastName.ToUpper().Equals(TempCharge.LastName.ToUpper())
                                                               ).FirstOrDefault();

                                if (PatientDat != null)
                                {
                                    if (PatientDat.LastName != TempCharge.LastName || PatientDat.FirstName != TempCharge.FirstName)
                                    {
                                        TempCharge.ErrorMessage = TempCharge.ErrorMessage + "Patient Not found. ";
                                        PatientDat = null;
                                    }
                                }
                                else
                                {
                                    if (TempCharge.FirstName != null && TempCharge.LastName != null)
                                    {
                                        List<Patient> temp = PatientData.Where(c =>
                                                                c.FirstName.ToUpper().Equals(TempCharge.FirstName.ToUpper()) &&
                                                                c.LastName.ToUpper().Equals(TempCharge.LastName.ToUpper())
                                                               ).ToList();
                                        if (temp != null && temp.Count == 1)
                                        {
                                            TempCharge.ErrorMessage = TempCharge.ErrorMessage + "Patient DOB not matched";
                                            PatientDat = temp[0];
                                        }
                                        else
                                        {
                                            TempCharge.ErrorMessage = TempCharge.ErrorMessage + "Patient Not found. ";
                                            PatientDat = null;
                                        }
                                    }
                                }
                            }
                            InsurancePlan MatchedInsurancePlan = null;
                            if (PatientDat != null)
                            {
                                //if (PatientDat.FirstName == "CRISTINA" && PatientDat.LastName == "CALINGO")
                                //{
                                //    Debug.WriteLine("");
                                //}
                                TempCharge.PatientID = PatientDat.ID;
                                TempCharge.ExternalPatientID = PatientDat.ExternalPatientID;
                                PatientPlan PrimaryPlan = PatientPlanData.Where(c => c.IsActive.Equals(true) && c.PatientID.Equals(PatientDat.ID) && c.Coverage == "P").FirstOrDefault();

                            if (PrimaryPlan != null)
                            {
                                MatchedInsurancePlan = InsurancePlanData.Where(ip => ip.ID.Equals(PrimaryPlan.InsurancePlanID)).FirstOrDefault();
                                if (MatchedInsurancePlan != null)
                                {
                                    TempCharge.InsuranceName = MatchedInsurancePlan.PlanName;
                                }
                            }

                        }

                        if (PreviousRow == null)
                        {
                            PreviousRow = TempCharge;
                        }
                        if (TempCharge.DateOfService != PreviousRow.DateOfService || TempCharge.InsuranceName != PreviousRow.InsuranceName || TempCharge.PatientID != PreviousRow.PatientID)
                        {
                            GroupID++;
                        }



                        if (!TempCharge.DateOfService.IsNull())
                        {
                            if (TempChargeWithPatientInfo.LocationID != 0)
                            {
                                TempCharge.LocationID = TempChargeWithPatientInfo.LocationID;
                            }
                            else
                            {
                                TempCharge.LocationID = locationID;
                            }
                            TempCharge.PracticeID = practiceID;
                            if (TempChargeWithPatientInfo.ProviderID != 0)
                            {
                                TempCharge.ProviderID = TempChargeWithPatientInfo.ProviderID;
                            }
                            else
                            {
                                TempCharge.ProviderID = providerID;
                            }

                                if (TempCharge.DiagnosisCode.IsNull())
                                {
                                    TempCharge.ErrorMessage = TempCharge.ErrorMessage + "ICD not found. ";
                                }
                                else
                                {
                                    ICDTemp = ICDTable.Where(i => i.ICDCode.Equals(TempCharge.DiagnosisCode)).FirstOrDefault();
                                    if (ICDTemp == null)
                                    {
                                        TempCharge.ErrorMessage = TempCharge.ErrorMessage + "ICD not found. ";
                                    }
                                }

                            if (TempCharge.CPTID == 0)
                            {
                                TempCharge.CPTID = null;
                                if (TempCharge.ErrorMessage != null)
                                {
                                    if (!TempCharge.ErrorMessage.Contains("invalid cpt"))
                                        TempCharge.ErrorMessage = TempCharge.ErrorMessage + "invalid cpt. ";
                                }
                                else
                                {
                                    TempCharge.ErrorMessage = "invalid cpt";
                                }
                            }
                            else if (TempCharge.CPTID.IsNull())
                            {
                                if (TempCharge.ErrorMessage != null)
                                {
                                    if (!TempCharge.ErrorMessage.Contains("invalid cpt"))
                                        TempCharge.ErrorMessage = TempCharge.ErrorMessage + "invalid cpt. ";
                                }
                                else
                                {
                                    TempCharge.ErrorMessage = "invalid cpt";
                                }
                            }
                            if (MatchedInsurancePlan != null && PlanTypeData != null && !MatchedInsurancePlan.PlanTypeID.IsNull())
                            {
                                PlanType TempPlanType = PlanTypeData.Where(p => p.ID.Equals(MatchedInsurancePlan.PlanTypeID)).FirstOrDefault();
                                if (TempPlanType != null)
                                {
                                    if (TempPlanType.Code.Equals("MB"))
                                    {
                                        TempCharge.Balance = tempcpt.MedicareAmount.Val();
                                        TempCharge.Charges = tempcpt.MedicareAmount.Val();
                                    }
                                }
                            }
                            if (TempCharge.InsuranceName == null)
                            {
                                TempCharge.GroupID = InvalidGroupID;
                            }
                            else
                            {
                                TempCharge.GroupID = GroupID;
                            }
                            if (TempCharge.GroupID == 0 || TempCharge.GroupID == null)
                            {
                                TempCharge.ErrorMessage = TempCharge.ErrorMessage + "Invalid Group. ";
                            }

                                if (tempcpt != null)
                                {
                                    POS PosFromCpt = POSTable.Where(pt => pt.ID.Equals(tempcpt.POSID)).FirstOrDefault();
                                    if (PosFromCpt != null)
                                    {
                                        TempCharge.POSID = PosFromCpt.ID;
                                    }
                                }
                                //TempCharge.PrescribingMD = TempChargeWithPatientInfo.PrescribingMD;
                                //TempCharge.FirstName = TempChargeWithPatientInfo.FirstName;
                                // TempCharge.LastName = TempChargeWithPatientInfo.LastName;
                                //TempCharge.Provider = TempChargeWithPatientInfo.Provider;
                                //TempCharge.OfficeName = TempChargeWithPatientInfo.OfficeName;
                                //TempCharge.InsuranceName = TempChargeWithPatientInfo.InsuranceName;
                                if (PatientDat != null)
                                    TempCharge.Gender = PatientDat.Gender;
                                //TempCharge.PatientID = TempChargeWithPatientInfo.PatientID;
                                //TempCharge.ExternalPatientID = TempChargeWithPatientInfo.ExternalPatientID;
                                //TempCharge.DOB = TempChargeWithPatientInfo.DOB;
                                TempCharge.MergeStatus = "E";
                                TempCharge.AddedDate = DateTime.Now;
                                TempCharge.AddedBy = UD.Email;
                                //TempCharge.GroupID = GroupID;
                                TempCharge.FileName = FileName;
                                TempCharge.PaymentProcessed = "NP";
                                TempCharge.IsRegularRecord = true;
                                if (TempCharge.PrescribingMD != null)
                                {
                                    string PrescribingMD = TempCharge.PrescribingMD.Split(new string[] { ", MD" }, StringSplitOptions.None)[0];
                                    if (PrescribingMD.Split(' ').Length > 1)
                                    {
                                        string FirstName = PrescribingMD.Split(' ')[0];
                                        string LastName = PrescribingMD.Split(' ')[1];
                                        RefProvider TempRefProvider = RefProviderData.Where(refp => refp.FirstName.Equals(FirstName, StringComparison.InvariantCultureIgnoreCase) && refp.LastName.Equals(LastName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                        if (TempRefProvider != null)
                                        {
                                            TempCharge.RefProviderID = TempRefProvider.ID;
                                        }
                                        else
                                        {
                                            TempCharge.ErrorMessage = TempCharge.ErrorMessage + "Prescribing MD\\Referring Provider not found in system. ";
                                        }
                                    }
                                    else
                                    {
                                        TempCharge.ErrorMessage = TempCharge.ErrorMessage + "Prescribing MD\\Referring Provider not found in system. ";
                                    }
                                }
                                else
                                {
                                    TempCharge.ErrorMessage = TempCharge.ErrorMessage + "Prescribing MD\\Referring Provider not found in system. ";
                                }
                                if (TempCharge.ErrorMessage == null)
                                {
                                    if (ICDTemp != null)
                                    {
                                        if (TempCharge.PatientID.HasValue)
                                        {
                                            Visit MatchedVisit = VisitData.Where(v => v.PatientID == TempCharge.PatientID && v.DateOfServiceFrom == TempCharge.DateOfService && v.ICD1ID == ICDTemp.ID).FirstOrDefault();
                                            if (MatchedVisit != null)
                                            {
                                                List<Charge> MatchingVisitCharges = ChargeData.Where(c => c.VisitID == MatchedVisit.ID).ToList();
                                                if (MatchingVisitCharges != null)
                                                {
                                                    if (MatchingVisitCharges.Any(mvc => mvc.CPTID == tempcpt.ID))
                                                    {
                                                        TempCharge.MergeStatus = "D";
                                                        CurrentPatientCharges.Add(TempCharge);
                                                        _context.ExternalCharge.Add(TempCharge);
                                                        ExternalChargesAdded++;
                                                    }
                                                    else
                                                    {
                                                        CurrentPatientCharges.Add(TempCharge);
                                                        _context.ExternalCharge.Add(TempCharge);
                                                        ExternalChargesAdded++;
                                                    }
                                                }
                                                else
                                                {
                                                    CurrentPatientCharges.Add(TempCharge);
                                                    _context.ExternalCharge.Add(TempCharge);
                                                    ExternalChargesAdded++;
                                                }
                                            }
                                            else
                                            {
                                                CurrentPatientCharges.Add(TempCharge);
                                                _context.ExternalCharge.Add(TempCharge);
                                                ExternalChargesAdded++;
                                            }

                                        }
                                        else
                                        {
                                            CurrentPatientCharges.Add(TempCharge);
                                            _context.ExternalCharge.Add(TempCharge);
                                            ExternalChargesAdded++;
                                        }
                                    }
                                }
                                else
                                {

                                    ExternalCharge MatchingExternalCharge = ExternalChargeData.Where(ec => ec.DateOfService == TempCharge.DateOfService && ec.CptCode == TempCharge.CptCode && ec.DiagnosisCode == TempCharge.DiagnosisCode && ec.FirstName == TempCharge.FirstName && ec.LastName == TempCharge.LastName && ec.PrescribingMD == TempCharge.PrescribingMD && ec.InsuranceName == TempCharge.InsuranceName && ec.DOB == TempCharge.DOB && ec.ExternalPatientID == TempCharge.ExternalPatientID && ec.Charges == TempCharge.Charges && ec.Adj == TempCharge.Adj && ec.Balance  == TempCharge.Balance && ec.PatientPayment == TempCharge.PatientPayment && ec.InsurancePayment ==TempCharge.InsurancePayment).FirstOrDefault();
                                    if (MatchingExternalCharge != null)
                                    {
                                        TempCharge.MergeStatus = "D";
                                        CurrentPatientCharges.Add(TempCharge);
                                        _context.ExternalCharge.Add(TempCharge);
                                        ExternalChargesAdded++;
                                    }
                                    else
                                    {
                                        ExternalCharge MatchingExternalChargeInCurrentIteration = CurrentPatientCharges.Where(cps => cps.DateOfService == TempCharge.DateOfService && cps.CptCode == TempCharge.CptCode && cps.DiagnosisCode == TempCharge.DiagnosisCode && cps.FirstName == TempCharge.FirstName && cps.LastName == TempCharge.LastName && cps.PrescribingMD == TempCharge.PrescribingMD && cps.InsuranceName == TempCharge.InsuranceName && cps.DOB == TempCharge.DOB && cps.ExternalPatientID == TempCharge.ExternalPatientID && cps.Charges == TempCharge.Charges && cps.Adj == TempCharge.Adj && cps.Balance == TempCharge.Balance && cps.PatientPayment == TempCharge.PatientPayment && cps.InsurancePayment == TempCharge.InsurancePayment).FirstOrDefault();
                                        if (MatchingExternalChargeInCurrentIteration != null)
                                        {
                                            TempCharge.MergeStatus = "D";
                                            CurrentPatientCharges.Add(TempCharge);
                                            _context.ExternalCharge.Add(TempCharge);
                                            ExternalChargesAdded++;
                                        }
                                        else
                                        {
                                            CurrentPatientCharges.Add(TempCharge);
                                            _context.ExternalCharge.Add(TempCharge);
                                            ExternalChargesAdded++;
                                        }
                                    }


                                }
                            }
                            PreviousRow = TempCharge;
                        }

                        catch (Exception ex)
                        {
                            return BadRequest("Unexpected Error Occurred. Please Contect BellMedEx\n" + ex.Message + "\n" + ex.StackTrace);
                        }
                    }

                _context.SaveChanges();
                ExternalChargeData = _context.ExternalCharge.Where(x => x.PracticeID == UD.PracticeID).ToList();
                if (ExternalChargeData.Count() != 0)
                    ExternalPatientIDAfterInsertion = ExternalChargeData.OrderBy(lamb => lamb.ID).ElementAt(ExternalChargeData.Count() - 1).ID;
                if (ExternalChargeData.Count() != 0)
                    ExternalChargeData = _context.ExternalCharge.Where(x => x.PracticeID == UD.PracticeID && x.ID > ExternalChargeInitialID && x.ID <= ExternalPatientIDAfterInsertion).ToList();
                else
                    ExternalChargeData = _context.ExternalCharge.Where(x => x.PracticeID == UD.PracticeID).ToList();

                ExternalChargeData = ExternalChargeData.Where(c => !c.GroupID.Equals(InvalidGroupID)).ToList();

                List<VisitData> VisitObjects = new List<VisitData>();
                List<ChargeWithGroupNumber> ChargeExternalChargeLinks = new List<ChargeWithGroupNumber>();

                List<List<ExternalCharge>> CompleteData = new List<List<ExternalCharge>>();
                List<ExternalCharge> Templist = new List<ExternalCharge>();

                //Visit AddVisit = new Visit();
                ExternalCharge PreviousCharge = ExternalChargeData.FirstOrDefault();
                foreach (ExternalCharge CurrentCharge in ExternalChargeData)
                {

                    if (PreviousCharge.GroupID == CurrentCharge.GroupID)
                    {
                        Templist.Add(CurrentCharge);
                    }
                    else
                    {
                        CompleteData.Add(Templist);
                        Templist = new List<ExternalCharge>();
                        Templist.Add(CurrentCharge);
                    }

                    PreviousCharge = CurrentCharge;

                }

                    if (Templist.Count != 0)
                    {
                        CompleteData.Add(Templist);
                    }
                    List<ICDWithVisit> ICDsToLink = new List<ICDWithVisit>();
                    foreach (List<ExternalCharge> ClaimData in CompleteData)
                    {
                        if (!ClaimData.Any(cd => cd.MergeStatus == "D"))
                        {
                            VisitData Temp = new VisitData();
                            Temp.Charges = new List<Charge>();
                            Temp.ExternalCharges = new List<ExternalCharge>();

                    Visit VisitTBA = new Visit();

                    ExternalCharge FirstCharge = ClaimData.FirstOrDefault();
                    bool IsClaimValid = true;
                    foreach (ExternalCharge ChargeFromSheet in ClaimData)
                    {

                        if (ChargeFromSheet.ErrorMessage != null)
                        {
                            if (ChargeFromSheet.ErrorMessage != "")
                            {
                                IsClaimValid = false;
                                break;
                            }
                        }
                        if (ChargeFromSheet.PatientID.IsNull())
                        {
                            IsClaimValid = false;
                            break;
                        }
                        if (ChargeFromSheet.InsuranceName.IsNull())
                        {
                            IsClaimValid = false;
                            break;
                        }
                        Charge ChargeTBA = new Charge();

                        #region Charge settings

                        if (ChargeFromSheet.PatientID.GetValueOrDefault() != 0)
                            ChargeTBA.PatientID = ChargeFromSheet.PatientID.GetValueOrDefault();

                        if (ChargeFromSheet.CPTID.GetValueOrDefault() != 0)
                            ChargeTBA.CPTID = ChargeFromSheet.CPTID.GetValueOrDefault();

                        if (ChargeFromSheet.POSID.GetValueOrDefault() != 0)
                            ChargeTBA.POSID = ChargeFromSheet.POSID.GetValueOrDefault();

                        ChargeTBA.ClientID = UD.ClientID;
                        ChargeTBA.PracticeID = UD.PracticeID;
                        ChargeTBA.LocationID = locationID;
                        ChargeTBA.ProviderID = providerID;

                        if (ChargeFromSheet.PatientID.GetValueOrDefault() != 0)
                        {
                            ChargeTBA.PatientID = ChargeFromSheet.PatientID.GetValueOrDefault();
                            PatientPlan TempPrimaryPatientPlan = PatientPlanData.Where(lamb => lamb.Coverage =="P" && lamb.PatientID.Equals(ChargeTBA.PatientID)).FirstOrDefault();
                            if (TempPrimaryPatientPlan != null)
                            {
                                InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                if (TempInsurancePlan != null)
                                {
                                    if (ChargeFromSheet.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(ChargeFromSheet.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                    {
                                        ChargeTBA.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                    }
                                }
                                else
                                {
                                    ExInsuranceMapping TempExInsuranceMapping = _contextMain.ExInsuranceMapping.Where(lamb1 => lamb1.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                    if (TempExInsuranceMapping != null)
                                    {
                                        InsurancePlan TempInsurancePlan2 = InsurancePlanData.Where(lamb => lamb.ID == TempExInsuranceMapping.InsurancePlanID).FirstOrDefault();
                                        if (TempInsurancePlan2 != null)
                                        {
                                            if (ChargeFromSheet.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(ChargeFromSheet.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                            {
                                                ChargeTBA.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        ChargeTBA.Modifier1ID = ChargeFromSheet.Modifier1ID;
                        ChargeTBA.Modifier2ID = ChargeFromSheet.Modifier2ID;
                        ChargeTBA.Modifier3ID = ChargeFromSheet.Modifier3ID;
                        ChargeTBA.Modifier4ID = ChargeFromSheet.Modifier4ID;


                        ChargeTBA.Units = ChargeFromSheet.DaysOrUnits;
                        ChargeTBA.DateOfServiceFrom = ChargeFromSheet.DateOfService;
                        ChargeTBA.DateOfServiceTo = ChargeFromSheet.DateOfService;
                        ChargeTBA.CPTID = ChargeFromSheet.CPTID.GetValueOrDefault();
                        ChargeTBA.CPTID = ChargeFromSheet.CPTID.GetValueOrDefault();
                        ChargeTBA.TotalAmount = ChargeFromSheet.Charges;
                        ChargeTBA.PrimaryBilledAmount = ChargeFromSheet.Charges;
                        ChargeTBA.PrimaryBal = ChargeFromSheet.Charges;
                        //ChargeTBA.IsSubmitted = true;
                        //ChargeTBA.PrimaryStatus = "S";
                        //ChargeTBA.SubmittetdDate = ChargeTBA.DateOfServiceFrom;

                        ChargeTBA.IsSubmitted = false;
                        ChargeTBA.PrimaryStatus = "Pending";
                        ChargeTBA.SubmittetdDate = null;

                        ChargeTBA.Pointer1 = "1";
                        ChargeTBA.Pointer2 = "";
                        ChargeTBA.Pointer3 = "";
                        ChargeTBA.Pointer4 = "";
                        ChargeTBA.AddedDate = DateTime.Now;
                        ChargeTBA.Units = "1";
                        ChargeTBA.AddedBy = UD.Email;
                        ChargeTBA.RefProviderID = ChargeFromSheet.RefProviderID;

                        #endregion

                        Temp.Charges.Add(ChargeTBA);
                        Temp.ExternalCharges.Add(ChargeFromSheet);
                    }

                    if (IsClaimValid)
                    {
                        #region Visit settings

                        VisitTBA.ClientID = ClientID;
                        VisitTBA.PracticeID = UD.PracticeID;
                        //TempVisit.LocationID = FirstCharge.LocationID;
                        //TempVisit.ProviderID = FirstCharge.ProviderID;
                        VisitTBA.DateOfServiceFrom = FirstCharge.DateOfService;
                        VisitTBA.DateOfServiceTo = FirstCharge.DateOfService;
                        if (FirstCharge.PatientID.GetValueOrDefault() != 0)
                        {
                            VisitTBA.PatientID = FirstCharge.PatientID.GetValueOrDefault();
                        }

                        if (FirstCharge.POSID.GetValueOrDefault() != 0)
                        {
                            VisitTBA.POSID = FirstCharge.POSID.GetValueOrDefault();
                        }

                        if (FirstCharge.ProviderID.IsNull())
                        {
                            VisitTBA.ProviderID = providerID;
                        }
                        else
                        {
                            VisitTBA.ProviderID = FirstCharge.ProviderID;
                        }

                        if (FirstCharge.LocationID.IsNull())
                        {
                            VisitTBA.LocationID = locationID;
                        }
                        else
                        {
                            VisitTBA.LocationID = FirstCharge.LocationID;
                        }

                        if (FirstCharge.PatientID.GetValueOrDefault() != 0)
                        {
                            VisitTBA.PatientID = FirstCharge.PatientID.GetValueOrDefault();
                            Patient TempPatient = PatientData.Where(p => p.ID == VisitTBA.PatientID).FirstOrDefault();
                            ExternalPatient TempExternalPatient = ExternalPatientData.Where(ep => ep.ExternalPatientID == TempPatient.ExternalPatientID).FirstOrDefault();
                            PatientPlan TempPrimaryPatientPlan = PatientPlanData.Where(lamb => lamb.Coverage =="P" &&  lamb.PatientID.Equals(VisitTBA.PatientID)).FirstOrDefault();
                            if (TempPrimaryPatientPlan != null)
                            {
                                InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                if (TempInsurancePlan != null)
                                {
                                    if (FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                    {
                                        VisitTBA.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                    }
                                }
                                else
                                {
                                    ExInsuranceMapping TempExInsuranceMapping = _contextMain.ExInsuranceMapping.Where(lamb1 => lamb1.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                    if (TempExInsuranceMapping != null)
                                    {
                                        InsurancePlan TempInsurancePlan2 = InsurancePlanData.Where(lamb => lamb.ID == TempExInsuranceMapping.InsurancePlanID).FirstOrDefault();
                                        if (TempInsurancePlan2 != null)
                                        {
                                            if (FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                            {
                                                VisitTBA.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        ICD MatchedIcd = ICDTable.Where(icd => icd.ICDCode.Equals(FirstCharge.DiagnosisCode)).FirstOrDefault();
                        if (MatchedIcd != null)
                        {
                            VisitTBA.ICD1ID = MatchedIcd.ID;
                        }
                        //else
                        //{
                        //    VisitTBA.ICD1ID = DummyIcd.ID;
                        //ICD NewICD = new ICD();
                        //NewICD.ICDCode = FirstCharge.DiagnosisCode.Replace(".", "");
                        //NewICD.IsActive = true;
                        //NewICD.IsDeleted = false;
                        //NewICD.Description = "Icd code "+ FirstCharge.DiagnosisCode.Replace(".", "");
                        //NewICD.AddedBy = UD.Email;
                        //NewICD.AddedDate = DateTime.Now;
                        //_context.ICD.Add(NewICD);
                        ////VisitTBA.ICD1ID = NewICD.ID;
                        //ICDWithVisit TempObj = new ICDWithVisit();
                        //TempObj.Visit = VisitTBA;
                        //TempObj.ICD = NewICD;
                        //ICDsToLink.Add(TempObj);
                        //}
                        VisitTBA.OutsideReferral = false;
                        VisitTBA.AddedDate = DateTime.Now;
                        VisitTBA.AddedBy = UD.Email;

                        VisitTBA.PrimaryBal = ClaimData.Sum(bal => bal.Charges);
                        VisitTBA.PrimaryBilledAmount = ClaimData.Sum(bal => bal.Charges);
                        VisitTBA.TotalAmount = ClaimData.Sum(bal => bal.Charges);

                        VisitTBA.IsDontPrint = false;
                        VisitTBA.IsForcePaper = false;
                        //VisitTBA.PrimaryStatus = "S";
                        //VisitTBA.IsSubmitted = true;
                        //VisitTBA.SubmittedDate = DateTime.Now;

                        VisitTBA.PrimaryStatus = "Pending";
                        VisitTBA.IsSubmitted = false;
                        VisitTBA.SubmittedDate = null;

                        VisitTBA.IsReversalApplied = false;
                        VisitTBA.RefProviderID = FirstCharge.RefProviderID;
                        VisitTBA.PrescribingMD = FirstCharge.PrescribingMD;

                        #endregion

                                if (VisitTBA.DateOfServiceFrom != null && !VisitTBA.ICD1ID.IsNull() && VisitTBA.ICD1ID != 0)
                                {
                                    foreach (Charge CH in Temp.Charges)
                                    {
                                        _context.Charge.Add(CH);
                                    }
                                    _context.Visit.Add(VisitTBA);
                                    Temp.Visit = VisitTBA;
                                    Temp.GroupID = FirstCharge.GroupID;
                                    VisitObjects.Add(Temp);
                                }
                            }
                        }
                    }

                _context.SaveChanges();

                    foreach (VisitData TempClaimData in VisitObjects)
                    {
                        for (int i = 0; i < TempClaimData.ExternalCharges.Count(); i++)
                        {
                            TempClaimData.ExternalCharges.ElementAt(i).ChargeID = TempClaimData.Charges.ElementAt(i).ID;
                            TempClaimData.ExternalCharges.ElementAt(i).VisitID = TempClaimData.Visit.ID;
                            TempClaimData.ExternalCharges.ElementAt(i).MergeStatus = "A";
                            TempClaimData.ExternalCharges.ElementAt(i).PrimaryInsuredID = TempClaimData.Charges.ElementAt(i).PrimaryPatientPlanID.Value;
                            TempClaimData.Charges.ElementAt(i).VisitID = TempClaimData.Visit.ID;

                            _context.Charge.Update(TempClaimData.Charges.ElementAt(i));
                            _context.ExternalCharge.Update(TempClaimData.ExternalCharges.ElementAt(i));
                        }
                        TempClaimData.Visit.Charges = TempClaimData.Charges;
                        _context.Visit.Update(TempClaimData.Visit);


                    }
                    _context.SaveChanges();
                    Debug.WriteLine("external charges added: " + ExternalChargesAdded + ", visits added: " + VisitsAdded + ", charges added: " + ChargesAdded + ", groups added: " + GroupsAdded);
                    return Ok(returnObject);
                }
            }
            catch(Exception ex)
            {
                return BadRequest("Something went wrong. Please contact BellMedEx");
            }
        }

        //multiple charges in a row 
        //each row represents a visit
        //primary and secondary insurance details available
        //billing physician = practice (same physician h i guess)
        //REFERRAL_PHYSICIAN  = referring physician
        //ATTENDING_PHYSICIAN = providerid

        [HttpPost]
        [Route("AddMTBCCharges")]
        public async Task<ActionResult> AddMTBCCharges(VMDataMigration InputData)
        {
            ReturnObject returnObject = new ReturnObject();
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
                User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
                User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long ClientID = UD.ClientID;
            int ChargesAdded = 0, GroupsAdded = 0, VisitsAdded = 0, ExternalChargesAdded = 0;

            List<Practice> PracticeData = _context.Practice.ToList();
            List<Provider> ProviderData = _context.Provider.ToList();
            List<Models.Location> LocationData = _context.Location.ToList();
            List<Patient> PatientData = _context.Patient.ToList();
            List<InsurancePlan> InsurancePlanData = _context.InsurancePlan.ToList();
            List<ExternalPatient> ExternalPatientData = _context.ExternalPatient.ToList();
            List<ExInsuranceMapping> ExInsuranceMappingData = _contextMain.ExInsuranceMapping.ToList();
            List<PatientPlan> PatientPlanData = _context.PatientPlan.ToList();
            List<ExternalCharge> ExternalChargeData = _context.ExternalCharge.ToList();
            List<POS> POSTable = _context.POS.ToList();
            List<Cpt> CptTable = _context.Cpt.ToList();
            List<ICD> ICDTable = _context.ICD.ToList();
            List<Modifier> ModifierTable = _context.Modifier.ToList();
            ICD DummyIcd = ICDTable.Where(lamb => lamb.ICDCode.Equals("99999999")).FirstOrDefault();
            if (DummyIcd == null)
            {
                DummyIcd = new ICD();
                DummyIcd.ICDCode = "99999999";
                DummyIcd.Description = "Dummy ICD";
                DummyIcd.AddedDate = DateTime.Now;
                DummyIcd.AddedBy = UD.Email;
                DummyIcd.IsActive = true;
                DummyIcd.IsDeleted = false;
                _context.ICD.Add(DummyIcd);
                _context.SaveChanges();
            }
            long ExternalChargeInitialID = 0, ExternalPatientIDAfterInsertion = 0;

            if (ExternalChargeData.Count() != 0)
            {
                ExternalChargeInitialID = ExternalChargeData.OrderBy(lamb => lamb.ID).ElementAt(ExternalChargeData.Count() - 1).ID;
            }

            byte[] bytes = Convert.FromBase64String(InputData.UploadModel.Content.Substring(InputData.UploadModel.Content.IndexOf("base64,") + 7));
            string FileName = InputData.UploadModel.Name;

            Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            string OutputPath = Path.Combine("\\\\", settings.DocumentServerURL,
                    settings.DocumentServerDirectory, "DataMigration", UD.ClientID + "",
                    DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));

            if (System.IO.Directory.Exists(OutputPath))
            {
                System.IO.File.WriteAllBytes(Path.Combine(OutputPath, FileName), bytes);
            }
            else
            {
                System.IO.Directory.CreateDirectory(OutputPath);
                System.IO.File.WriteAllBytes(Path.Combine(OutputPath, FileName), bytes);
            }

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                try
                {
                    long practiceID;
                    long providerID;
                    long locationID;
                    long InterPracticeID = 0;
                    long InterProviderID = 0;
                    long InterLocationID = 0;
                    int RowNum = 0;
                    //Stream Stream = contents;

                    practiceID = InputData.PracticeID;
                    providerID = InputData.ProviderID;
                    locationID = InputData.LocationID;

                    if (InputData.PracticeID.IsNull())
                        return BadRequest("Practice id not present");
                    if (InputData.ProviderID.IsNull())
                        return BadRequest("Provider id not present");
                    if (InputData.LocationID.IsNull())
                        return BadRequest("Location id not present");



                    //SaveStreamAsFile("D:\\", Stream, "streamedfile.xlsx");

                    SpreadsheetDocument Doc = SpreadsheetDocument.Open(stream, false);

                    WorkbookPart WorkbookPart = Doc.WorkbookPart;
                    WorksheetPart WorksheetPart = WorkbookPart.WorksheetParts.First();
                    Worksheet Sheet = WorksheetPart.Worksheet;

                    SharedStringTablePart SstPart = WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();

                    SharedStringTable Sst = SstPart.SharedStringTable;

                    var AllCells = Sheet.Descendants<Cell>();
                    var AllRows = Sheet.Descendants<Row>();
                    bool TakeTrioFromSheet = false, TrioFirstPresent = false, TrioSecondPresent = false, TrioThirdPresent = false;

                    Debug.WriteLine(AllRows.Count() + "  " + AllCells.Count());


                    List<List<ExternalCharge>> FinalChargesData = new List<List<ExternalCharge>>();
                    List<string> VisitAmounts = new List<string>();



                    decimal VisitCharge = 0, VisitBalance = 0, VisitAdj = 0, VisitPatientpayment = 0, VisitInsurancepayment = 0;
                    List<ExternalCharge> AddedExternalCharges = new List<ExternalCharge>();
                    ExternalCharge PreviousRow = null;
                    List<PatientPlanWithPatient> PatientPlanMapping = new List<PatientPlanWithPatient>();
                    for (int RowIndex = 1; RowIndex < AllRows.Count(); RowIndex++)
                    {
                        try
                        {
                            ExternalCharge TempCharge = new ExternalCharge();
                            bool IsChargeDataPresent = false;
                            List<Cell> RowCells = AllRows.ElementAt(RowIndex).Elements<Cell>().ToList();
                            bool SummationIdentifier = true;

                            List<ExternalCharge> SubCharges = new List<ExternalCharge>();
                            SubCharges.Add(new ExternalCharge());
                            SubCharges.Add(new ExternalCharge());
                            SubCharges.Add(new ExternalCharge());
                            SubCharges.Add(new ExternalCharge());
                            SubCharges.Add(new ExternalCharge());

                            //Debug.Write("start of row");
                            for (int ColIndex = 0; ColIndex < RowCells.Count(); ColIndex++)
                            {
                                try
                                {
                                    CellReferenceValuePair pair = GetValueFromCell(RowCells.ElementAt(ColIndex), Sst);

                                    if (pair != null && pair.Value != null && !pair.Reference.IsNull())
                                    {
                                        //Debug.Write(pair.Reference+ "       "+pair.Value);
                                        string ReferenceAlphabets = new String(pair.Reference.Where(Char.IsLetter).ToArray());
                                        string FinalIdentifier = "";
                                        pair.Reference = new String(pair.Reference.Where(Char.IsLetter).ToArray());

                                        //for (int StrIndex = 0; StrIndex < pair.Reference.Length; StrIndex++)
                                        //{
                                        //    FinalIdentifier += GetColumnQualifier(pair.Reference[StrIndex]);
                                        //}
                                        #region resource assignment

                                        if (ReferenceAlphabets.Equals("F"))
                                        {
                                            SubCharges[0].LastName = pair.Value.Trim().ToUpper();
                                            SubCharges[1].LastName = pair.Value.Trim().ToUpper();
                                            SubCharges[2].LastName = pair.Value.Trim().ToUpper();
                                            SubCharges[3].LastName = pair.Value.Trim().ToUpper();
                                            SubCharges[4].LastName = pair.Value.Trim().ToUpper();
                                        }
                                        if (ReferenceAlphabets.Equals("G"))
                                        {
                                            SubCharges[0].FirstName = pair.Value.Trim().ToUpper();
                                            SubCharges[1].FirstName = pair.Value.Trim().ToUpper();
                                            SubCharges[2].FirstName = pair.Value.Trim().ToUpper();
                                            SubCharges[3].FirstName = pair.Value.Trim().ToUpper();
                                            SubCharges[4].FirstName = pair.Value.Trim().ToUpper();
                                        }
                                        if (ReferenceAlphabets.Equals("H"))
                                        {
                                            SubCharges[0].MiddleInitial = pair.Value.Trim().ToUpper();
                                            SubCharges[1].MiddleInitial = pair.Value.Trim().ToUpper();
                                            SubCharges[2].MiddleInitial = pair.Value.Trim().ToUpper();
                                            SubCharges[3].MiddleInitial = pair.Value.Trim().ToUpper();
                                            SubCharges[4].MiddleInitial = pair.Value.Trim().ToUpper();
                                        }
                                        if (ReferenceAlphabets.Equals("A"))
                                        {
                                            SubCharges[0].ExternalPatientID = pair.Value;
                                            SubCharges[1].ExternalPatientID = pair.Value;
                                            SubCharges[2].ExternalPatientID = pair.Value;
                                            SubCharges[3].ExternalPatientID = pair.Value;
                                            SubCharges[4].ExternalPatientID = pair.Value;
                                            Patient patient = PatientData.Where(p => p.ExternalPatientID == pair.Value).FirstOrDefault();
                                            if (patient != null)
                                            {
                                                SubCharges[0].PatientID = patient.ID;
                                                SubCharges[1].PatientID = SubCharges[0].PatientID;
                                                SubCharges[2].PatientID = SubCharges[0].PatientID;
                                                SubCharges[3].PatientID = SubCharges[0].PatientID;
                                                SubCharges[4].PatientID = SubCharges[0].PatientID;
                                            }
                                            else
                                            {
                                                SubCharges[0].ErrorMessage = SubCharges[0].ErrorMessage + "Patient not found. ";
                                                SubCharges[1].ErrorMessage = SubCharges[0].ErrorMessage + "Patient not found. ";
                                                SubCharges[2].ErrorMessage = SubCharges[0].ErrorMessage + "Patient not found. ";
                                                SubCharges[3].ErrorMessage = SubCharges[0].ErrorMessage + "Patient not found. ";
                                                SubCharges[4].ErrorMessage = SubCharges[0].ErrorMessage + "Patient not found. ";
                                            }

                                        }
                                        if (ReferenceAlphabets.Equals("B"))
                                        {
                                            SubCharges[0].InsuranceName = pair.Value;
                                            SubCharges[1].InsuranceName = pair.Value;
                                            SubCharges[2].InsuranceName = pair.Value;
                                            SubCharges[3].InsuranceName = pair.Value;
                                            SubCharges[4].InsuranceName = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("D"))
                                        {
                                            SubCharges[0].SecondaryInsuranceName = pair.Value;
                                            SubCharges[1].SecondaryInsuranceName = pair.Value;
                                            SubCharges[2].SecondaryInsuranceName = pair.Value;
                                            SubCharges[3].SecondaryInsuranceName = pair.Value;
                                            SubCharges[4].SecondaryInsuranceName = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("J"))
                                        {
                                            if (pair.Value.Contains("/") || pair.Value.Contains("-"))
                                            {
                                                CultureInfo invC = CultureInfo.InvariantCulture;
                                                SubCharges[0].DateOfService = DateTime.Parse(pair.Value, invC, DateTimeStyles.None);
                                            }
                                            else
                                            {
                                                SubCharges[0].DateOfService = DateTime.FromOADate(double.Parse(pair.Value));
                                            }
                                            SubCharges[1].DateOfService = SubCharges[0].DateOfService;
                                            SubCharges[2].DateOfService = SubCharges[0].DateOfService;
                                            SubCharges[3].DateOfService = SubCharges[0].DateOfService;
                                            SubCharges[4].DateOfService = SubCharges[0].DateOfService;
                                        }
                                        if (ReferenceAlphabets.Equals("K"))
                                        {
                                            if (pair.Value.Trim() != "" && pair.Value != null)
                                            {
                                                SubCharges[0].VisitPOSCode = pair.Value;
                                                SubCharges[1].VisitPOSCode = pair.Value;
                                                SubCharges[2].VisitPOSCode = pair.Value;
                                                SubCharges[3].VisitPOSCode = pair.Value;
                                                SubCharges[4].VisitPOSCode = pair.Value;
                                            }
                                            else
                                            {
                                                SubCharges[0].VisitPOSCode = "11";
                                                SubCharges[1].VisitPOSCode = "11";
                                                SubCharges[2].VisitPOSCode = "11";
                                                SubCharges[3].VisitPOSCode = "11";
                                                SubCharges[4].VisitPOSCode = "11";
                                            }
                                        }
                                        if (ReferenceAlphabets.Equals("L"))
                                        {
                                            if (pair.Value.Contains("/") || pair.Value.Contains("-"))
                                            {
                                                CultureInfo invC = CultureInfo.InvariantCulture;
                                                SubCharges[0].SubmittetdDate = DateTime.Parse(pair.Value, invC, DateTimeStyles.None);
                                            }
                                            else
                                            {
                                                SubCharges[0].SubmittetdDate = DateTime.FromOADate(double.Parse(pair.Value));

                                            }
                                            SubCharges[1].SubmittetdDate = SubCharges[0].SubmittetdDate;
                                            SubCharges[2].SubmittetdDate = SubCharges[0].SubmittetdDate;
                                            SubCharges[3].SubmittetdDate = SubCharges[0].SubmittetdDate;
                                            SubCharges[4].SubmittetdDate = SubCharges[0].SubmittetdDate;
                                        }
                                        if (ReferenceAlphabets.Equals("M"))
                                        {
                                            SubCharges[0].DXCode1 = pair.Value;
                                            SubCharges[1].DXCode1 = pair.Value;
                                            SubCharges[2].DXCode1 = pair.Value;
                                            SubCharges[3].DXCode1 = pair.Value;
                                            SubCharges[4].DXCode1 = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("N"))
                                        {
                                            SubCharges[0].DXCode2 = pair.Value;
                                            SubCharges[1].DXCode2 = pair.Value;
                                            SubCharges[2].DXCode2 = pair.Value;
                                            SubCharges[3].DXCode2 = pair.Value;
                                            SubCharges[4].DXCode2 = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("O"))
                                        {
                                            SubCharges[0].DXCode3 = pair.Value;
                                            SubCharges[1].DXCode3 = pair.Value;
                                            SubCharges[2].DXCode3 = pair.Value;
                                            SubCharges[3].DXCode3 = pair.Value;
                                            SubCharges[4].DXCode3 = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("P"))
                                        {
                                            SubCharges[0].DXCode4 = pair.Value;
                                            SubCharges[1].DXCode4 = pair.Value;
                                            SubCharges[2].DXCode4 = pair.Value;
                                            SubCharges[3].DXCode4 = pair.Value;
                                            SubCharges[4].DXCode4 = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("Q"))
                                        {
                                            SubCharges[0].DXCode5 = pair.Value;
                                            SubCharges[1].DXCode5 = pair.Value;
                                            SubCharges[2].DXCode5 = pair.Value;
                                            SubCharges[3].DXCode5 = pair.Value;
                                            SubCharges[4].DXCode5 = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("R"))
                                        {
                                            SubCharges[0].DXCode6 = pair.Value;
                                            SubCharges[1].DXCode6 = pair.Value;
                                            SubCharges[2].DXCode6 = pair.Value;
                                            SubCharges[3].DXCode6 = pair.Value;
                                            SubCharges[4].DXCode6 = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("S"))
                                        {
                                            SubCharges[0].CptCode = pair.Value.Replace(".0", "").Trim();
                                            Cpt tempcpt = CptTable.Where(c => c.CPTCode.Equals(SubCharges[0].CptCode)).FirstOrDefault();
                                            if (tempcpt != null)
                                            {
                                                SubCharges[0].CPTID = tempcpt.ID;
                                            }
                                            else
                                            {
                                                SubCharges[0].ErrorMessage = SubCharges[0].ErrorMessage + "Cpt not found. ";
                                            }
                                        }

                                        if (ReferenceAlphabets.Equals("T"))
                                        {
                                            SubCharges[0].Charges = decimal.Parse(pair.Value);
                                        }
                                        if (ReferenceAlphabets.Equals("W"))
                                        {
                                            SubCharges[0].DaysOrUnits = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("Y"))
                                        {
                                            SubCharges[1].CptCode = pair.Value.Replace(".0", "").Trim();
                                            Cpt tempcpt = CptTable.Where(c => c.CPTCode.Equals(SubCharges[1].CptCode)).FirstOrDefault();
                                            if (tempcpt != null)
                                            {
                                                SubCharges[1].CPTID = tempcpt.ID;
                                            }
                                            else
                                            {
                                                SubCharges[1].ErrorMessage = SubCharges[1].ErrorMessage + "Cpt not found. ";
                                            }
                                        }
                                        if (ReferenceAlphabets.Equals("Z"))
                                        {
                                            SubCharges[1].Charges = decimal.Parse(pair.Value);
                                        }
                                        if (ReferenceAlphabets.Equals("AC"))
                                        {
                                            SubCharges[1].DaysOrUnits = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("AE"))
                                        {
                                            SubCharges[2].CptCode = pair.Value.Replace(".0", "").Trim();
                                            Cpt tempcpt = CptTable.Where(c => c.CPTCode.Equals(SubCharges[2].CptCode)).FirstOrDefault();
                                            if (tempcpt != null)
                                            {
                                                SubCharges[2].CPTID = tempcpt.ID;
                                            }
                                            else
                                            {
                                                SubCharges[2].ErrorMessage = SubCharges[2].ErrorMessage + "Cpt not found. ";
                                            }
                                        }
                                        if (ReferenceAlphabets.Equals("AF"))
                                        {
                                            SubCharges[2].Charges = decimal.Parse(pair.Value);
                                        }
                                        if (ReferenceAlphabets.Equals("AI"))
                                        {
                                            SubCharges[2].DaysOrUnits = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("AK"))
                                        {
                                            SubCharges[3].CptCode = pair.Value.Replace(".0", "").Trim();
                                            Cpt tempcpt = CptTable.Where(c => c.CPTCode.Equals(SubCharges[3].CptCode)).FirstOrDefault();
                                            if (tempcpt != null)
                                            {
                                                SubCharges[3].CPTID = tempcpt.ID;
                                            }
                                            else
                                            {
                                                SubCharges[3].ErrorMessage = SubCharges[3].ErrorMessage + "Cpt not found. ";
                                            }
                                        }

                                        if (ReferenceAlphabets.Equals("AL"))
                                        {
                                            SubCharges[3].Charges = decimal.Parse(pair.Value);
                                        }
                                        if (ReferenceAlphabets.Equals("AN"))
                                        {
                                            SubCharges[3].DaysOrUnits = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("AP"))
                                        {
                                            SubCharges[4].CptCode = pair.Value.Replace(".0", "").Trim();
                                            Cpt tempcpt = CptTable.Where(c => c.CPTCode.Equals(SubCharges[4].CptCode)).FirstOrDefault();
                                            if (tempcpt != null)
                                            {
                                                SubCharges[4].CPTID = tempcpt.ID;
                                            }
                                            else
                                            {
                                                SubCharges[4].ErrorMessage = SubCharges[4].ErrorMessage + "Cpt not found. ";
                                            }
                                        }
                                        if (ReferenceAlphabets.Equals("AQ"))
                                        {
                                            SubCharges[4].Charges = decimal.Parse(pair.Value);
                                        }
                                        if (ReferenceAlphabets.Equals("AR"))
                                        {
                                            SubCharges[4].DaysOrUnits = pair.Value;
                                        }
                                        if (ReferenceAlphabets.Equals("AW"))
                                        {
                                            SubCharges[0].PatientPayment = decimal.Parse(pair.Value);
                                            SubCharges[1].PatientPayment = decimal.Parse(pair.Value);
                                            SubCharges[2].PatientPayment = decimal.Parse(pair.Value);
                                            SubCharges[3].PatientPayment = decimal.Parse(pair.Value);
                                            SubCharges[4].PatientPayment = decimal.Parse(pair.Value);
                                        }
                                        if (ReferenceAlphabets.Equals("AX"))
                                        {
                                            SubCharges[0].Adj = decimal.Parse(pair.Value);
                                            SubCharges[1].Adj = decimal.Parse(pair.Value);
                                            SubCharges[2].Adj = decimal.Parse(pair.Value);
                                            SubCharges[3].Adj = decimal.Parse(pair.Value);
                                            SubCharges[4].Adj = decimal.Parse(pair.Value);
                                        }
                                        if (ReferenceAlphabets.Equals("AZ"))
                                        {
                                            SubCharges[0].PrimaryBal = decimal.Parse(pair.Value);
                                            SubCharges[1].PrimaryBal = decimal.Parse(pair.Value);
                                            SubCharges[2].PrimaryBal = decimal.Parse(pair.Value);
                                            SubCharges[3].PrimaryBal = decimal.Parse(pair.Value);
                                            SubCharges[4].PrimaryBal = decimal.Parse(pair.Value);
                                        }
                                        #endregion

                                    }

                                }
                                catch (Exception ex)
                                {
                                    return BadRequest("Unexpected Error Occurred. Please Contect BellMedEx\n" + ex.Message + "\n" + ex.StackTrace);
                                }
                            }
                            if (SubCharges[0].VisitPOSCode == null)
                            {
                                SubCharges[0].VisitPOSCode = "11";
                            }
                            SubCharges[0].POSCode = SubCharges[0].VisitPOSCode;
                            SubCharges[1].POSCode = SubCharges[0].VisitPOSCode;
                            SubCharges[2].POSCode = SubCharges[0].VisitPOSCode;
                            SubCharges[3].POSCode = SubCharges[0].VisitPOSCode;
                            SubCharges[4].POSCode = SubCharges[0].VisitPOSCode;
                            POS Posid = POSTable.Where(c => c.PosCode.Equals(SubCharges[0].POSCode)).FirstOrDefault();
                            SubCharges[0].POSID = Posid.ID;
                            SubCharges[1].POSID = Posid.ID;
                            SubCharges[2].POSID = Posid.ID;
                            SubCharges[3].POSID = Posid.ID;
                            SubCharges[4].POSID = Posid.ID;


                            List<ExternalCharge> ChargesToAdd = new List<ExternalCharge>();
                            foreach (ExternalCharge ExCh in SubCharges)
                            {

                                if (ExCh.CptCode != null)
                                {
                                    if (ExCh.CptCode.Trim() != "")
                                    {
                                        if (ExCh.DXCode1 == null)
                                        {
                                            if (ExCh.ErrorMessage != null)
                                            {
                                                ExCh.ErrorMessage = ExCh.ErrorMessage + "ICD not found. ";
                                            }
                                            else
                                            {
                                                ExCh.ErrorMessage = "ICD not found. ";
                                            }
                                        }
                                        ExCh.LocationID = locationID;
                                        ExCh.PracticeID = practiceID;
                                        ExCh.ProviderID = providerID;

                                        ExCh.MergeStatus = "E";
                                        ExCh.AddedDate = TempCharge.DateOfService;
                                        ExCh.AddedBy = UD.Email;
                                        //TempCharge.GroupID = GroupID;
                                        ExCh.FileName = FileName;
                                        ExCh.PaymentProcessed = "NP";

                                        _context.ExternalCharge.Add(ExCh);
                                        ChargesToAdd.Add(ExCh);
                                        ExternalChargesAdded++;
                                    }

                                }
                            }
                            List<ExternalCharge> excludedCharges = ChargesToAdd.Where(ec => ec.LocationID != 1).ToList();
                            if (excludedCharges.Count > 0)
                            {
                                Debug.WriteLine("");
                            }
                            if (ChargesToAdd.Count > 0)
                                FinalChargesData.Add(ChargesToAdd);
                            PreviousRow = TempCharge;
                        }

                        catch (Exception ex)
                        {
                            return BadRequest("Unexpected Error Occurred. Please Contect BellMedEx\n" + ex.Message + "\n" + ex.StackTrace);
                        }
                    }

                    _context.SaveChanges();

                    List<VisitData> VisitAndChargesToLink = new List<VisitData>();

                    foreach (List<ExternalCharge> VisitGroup in FinalChargesData)
                    {
                        VisitData TempData = new VisitData();
                        TempData.Charges = new List<Charge>();
                        TempData.ExternalCharges = new List<ExternalCharge>();
                        try
                        {

                            ExternalCharge FirstCharge = VisitGroup.FirstOrDefault();

                            Visit VisitTBA = new Visit();

                            VisitTBA.DateOfServiceFrom = FirstCharge.DateOfService;
                            VisitTBA.DateOfServiceTo = FirstCharge.DateOfService;
                            VisitTBA.OutsideReferral = false;
                            VisitTBA.AddedDate = FirstCharge.DateOfService;
                            VisitTBA.IsSubmitted = true;
                            VisitTBA.SubmittedDate = FirstCharge.DateOfService;
                            VisitTBA.AddedBy = UD.Email;

                            VisitTBA.PrimaryBal = FirstCharge.PrimaryBal;
                            VisitTBA.PrimaryBilledAmount = VisitGroup.Sum(bal => bal.Charges);
                            VisitTBA.TotalAmount = VisitGroup.Sum(bal => bal.Charges);
                            VisitTBA.PrimaryStatus = "S";
                            VisitTBA.IsDontPrint = false;
                            VisitTBA.IsForcePaper = false;
                            VisitTBA.IsReversalApplied = false;
                            VisitTBA.LocationID = FirstCharge.LocationID;
                            VisitTBA.ProviderID = FirstCharge.ProviderID;
                            VisitTBA.PracticeID = FirstCharge.PracticeID;
                            VisitTBA.ClientID = UD.ClientID;

                            if (VisitTBA.ClientID == 0 || VisitTBA.ClientID == null)
                            {
                                Debug.WriteLine("");
                            }
                            PatientPlan TempPrimaryPatientPlan = null, TempSecondaryPatientPlan = null;
                            if (FirstCharge.PatientID.GetValueOrDefault() != 0)
                            {
                                VisitTBA.PatientID = FirstCharge.PatientID.GetValueOrDefault();
                            }

                            if (FirstCharge.POSID.GetValueOrDefault() != 0)
                            {
                                VisitTBA.POSID = FirstCharge.POSID.GetValueOrDefault();
                            }

                            if (FirstCharge.PatientID.GetValueOrDefault() != 0)
                            {
                                VisitTBA.PatientID = FirstCharge.PatientID.GetValueOrDefault();
                                Patient TempPatient = PatientData.Where(p => p.ID == VisitTBA.PatientID).FirstOrDefault();
                                ///ExternalPatient TempExternalPatient = ExternalPatientData.Where(ep => ep.ExternalPatientID == TempPatient.ExternalPatientID).FirstOrDefault();
                                TempPrimaryPatientPlan = PatientPlanData.Where(lamb => lamb.PatientID.Equals(VisitTBA.PatientID) && lamb.Coverage.Equals("P")).FirstOrDefault();
                                if (TempPrimaryPatientPlan != null)
                                {
                                    InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                    if (TempInsurancePlan != null)
                                    {
                                        if (FirstCharge.InsuranceName != null)
                                        {
                                            if (FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                            {
                                                VisitTBA.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ExInsuranceMapping TempExInsuranceMapping = _contextMain.ExInsuranceMapping.Where(lamb1 => lamb1.ID == TempPrimaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                        if (TempExInsuranceMapping != null)
                                        {
                                            InsurancePlan TempInsurancePlan2 = InsurancePlanData.Where(lamb => lamb.ID == TempExInsuranceMapping.InsurancePlanID).FirstOrDefault();
                                            if (TempInsurancePlan2 != null)
                                            {
                                                if (FirstCharge.InsuranceName != null)
                                                {
                                                    if (TempInsurancePlan2.PlanName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.InsuranceName.Trim().ToUpper().Replace(" ", "")))
                                                    {
                                                        VisitTBA.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                TempSecondaryPatientPlan = PatientPlanData.Where(lamb => lamb.PatientID.Equals(VisitTBA.PatientID) && lamb.Coverage.Equals("S")).FirstOrDefault();
                                if (TempSecondaryPatientPlan != null)
                                {
                                    InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.ID == TempSecondaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                    if (TempInsurancePlan != null)
                                    {
                                        if (FirstCharge.SecondaryInsuranceName != null)
                                        {
                                            if (TempInsurancePlan.PlanName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.SecondaryInsuranceName.Trim().ToUpper().Replace(" ", "")))
                                            {
                                                VisitTBA.SecondaryPatientPlanID = TempSecondaryPatientPlan.ID;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ExInsuranceMapping TempExInsuranceMapping = ExInsuranceMappingData.Where(lamb1 => lamb1.ID == TempSecondaryPatientPlan.InsurancePlanID).FirstOrDefault();
                                        if (TempExInsuranceMapping != null)
                                        {
                                            InsurancePlan TempInsurancePlan2 = InsurancePlanData.Where(lamb => lamb.ID == TempExInsuranceMapping.InsurancePlanID).FirstOrDefault();
                                            if (TempInsurancePlan2 != null)
                                            {
                                                if (FirstCharge.SecondaryInsuranceName != null)
                                                {
                                                    if (TempInsurancePlan2.PlanName.Trim().ToUpper().Replace(" ", "").Equals(FirstCharge.SecondaryInsuranceName.Trim().ToUpper().Replace(" ", "")))
                                                    {
                                                        VisitTBA.SecondaryPatientPlanID = TempSecondaryPatientPlan.ID;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (!FirstCharge.DXCode1.IsNull() && FirstCharge.DXCode1 != "")
                            {
                                ICD DX1 = ICDTable.Where(i => i.ICDCode.Equals(FirstCharge.DXCode1)).FirstOrDefault();
                                if (DX1 != null)
                                {
                                    VisitTBA.ICD1ID = DX1.ID;
                                }
                            }
                            if (!FirstCharge.DXCode2.IsNull() && FirstCharge.DXCode2 != "")
                            {
                                ICD DX2 = ICDTable.Where(i => i.ICDCode.Equals(FirstCharge.DXCode2)).FirstOrDefault();
                                if (DX2 != null)
                                {
                                    VisitTBA.ICD2ID = DX2.ID;
                                }
                            }
                            if (!FirstCharge.DXCode3.IsNull() && FirstCharge.DXCode3 != "")
                            {
                                ICD DX3 = ICDTable.Where(i => i.ICDCode.Equals(FirstCharge.DXCode3)).FirstOrDefault();
                                if (DX3 != null)
                                {
                                    VisitTBA.ICD3ID = DX3.ID;
                                }
                            }
                            if (!FirstCharge.DXCode4.IsNull() && FirstCharge.DXCode4 != "")
                            {
                                ICD DX4 = ICDTable.Where(i => i.ICDCode.Equals(FirstCharge.DXCode4)).FirstOrDefault();
                                if (DX4 != null)
                                {
                                    VisitTBA.ICD4ID = DX4.ID;
                                }
                            }
                            if (!FirstCharge.DXCode5.IsNull() && FirstCharge.DXCode5 != "")
                            {
                                ICD DX5 = ICDTable.Where(i => i.ICDCode.Equals(FirstCharge.DXCode5)).FirstOrDefault();
                                if (DX5 != null)
                                {
                                    VisitTBA.ICD5ID = DX5.ID;
                                }
                            }
                            if (!FirstCharge.DXCode6.IsNull() && FirstCharge.DXCode6 != "")
                            {
                                ICD DX6 = ICDTable.Where(i => i.ICDCode.Equals(FirstCharge.DXCode6)).FirstOrDefault();
                                if (DX6 != null)
                                {
                                    VisitTBA.ICD6ID = DX6.ID;
                                }
                            }

                            bool IsVisitValid = true;

                            if (VisitTBA.PrimaryPatientPlanID == null || VisitTBA.PrimaryPatientPlanID == 0)
                            {
                                foreach (ExternalCharge ExCh in VisitGroup)
                                {
                                    if (ExCh.ErrorMessage != null)
                                    {
                                        ExCh.ErrorMessage += "Primary patient plan was not created. ";
                                    }
                                    else
                                    {
                                        ExCh.ErrorMessage = "Primary patient plan was not created. ";
                                    }
                                    _context.ExternalCharge.Update(ExCh);
                                }
                                IsVisitValid = false;
                            }
                            //if (VisitTBA.SecondaryPatientPlanID == null)
                            //{
                            //    foreach (ExternalCharge ExCh in VisitGroup)
                            //    {
                            //        if (ExCh.ErrorMessage != null)
                            //        {
                            //            ExCh.ErrorMessage += "Secondary patient plan was not created. ";
                            //        }
                            //        else
                            //        {
                            //            ExCh.ErrorMessage = "Secondary patient plan was not created. ";
                            //        }
                            //        _context.ExternalCharge.Update(ExCh);
                            //    }
                            //}
                            if (VisitTBA.DateOfServiceFrom == null)
                            {
                                foreach (ExternalCharge ExCh in VisitGroup)
                                {
                                    if (ExCh.ErrorMessage != null)
                                    {
                                        ExCh.ErrorMessage += "Date of service not valid. ";
                                    }
                                    else
                                    {
                                        ExCh.ErrorMessage = "Date of service not valid. ";
                                    }
                                    _context.ExternalCharge.Update(ExCh);
                                }
                                IsVisitValid = false;
                            }

                            if (VisitTBA.PatientID == 0 || VisitTBA.PatientID == null)
                            {
                                foreach (ExternalCharge ExCh in VisitGroup)
                                {
                                    if (ExCh.ErrorMessage != null)
                                    {
                                        ExCh.ErrorMessage += "Patient not found. ";
                                    }
                                    else
                                    {
                                        ExCh.ErrorMessage = "Patient not found. ";
                                    }
                                    _context.ExternalCharge.Update(ExCh);
                                }
                                IsVisitValid = false;
                            }

                            if (VisitTBA.ICD1ID == null || VisitTBA.ICD1ID == 0)
                            {
                                foreach (ExternalCharge ExCh in VisitGroup)
                                {
                                    if (ExCh.ErrorMessage != null)
                                    {
                                        ExCh.ErrorMessage += "ICD not found. ";
                                    }
                                    else
                                    {
                                        ExCh.ErrorMessage = "ICD not found. ";
                                    }
                                    _context.ExternalCharge.Update(ExCh);
                                }
                                IsVisitValid = false;
                            }
                            if (VisitGroup.Any(vg => vg.CPTID == 0 || vg.CPTID == null))
                            {
                                foreach (ExternalCharge ExCh in VisitGroup)
                                {
                                    if (ExCh.ErrorMessage != null)
                                    {
                                        ExCh.ErrorMessage += "Invalid CPT in charges. ";
                                    }
                                    else
                                    {
                                        ExCh.ErrorMessage = "Invalid CPT in charges. ";
                                    }
                                    _context.ExternalCharge.Update(ExCh);
                                }
                                IsVisitValid = false;
                            }

                            if (!IsVisitValid)
                                continue;



                            _context.Visit.Add(VisitTBA);
                            TempData.Visit = VisitTBA;
                            List<Charge> visitCharges = new List<Charge>();
                            foreach (ExternalCharge ExCh in VisitGroup)
                            {

                                if (ExCh.CptCode != null && ExCh.CptCode != "")
                                {
                                    if (ExCh.CptCode.Trim() != "")
                                    {
                                        Charge ChargeTBA = new Charge();

                                        #region Charge settings

                                        if (ExCh.PatientID.GetValueOrDefault() != 0)
                                            ChargeTBA.PatientID = ExCh.PatientID.GetValueOrDefault();

                                        if (ExCh.CPTID.GetValueOrDefault() != 0)
                                            ChargeTBA.CPTID = ExCh.CPTID.GetValueOrDefault();

                                        if (ExCh.POSID.GetValueOrDefault() != 0)
                                            ChargeTBA.POSID = ExCh.POSID.GetValueOrDefault();

                                        ChargeTBA.ClientID = UD.ClientID;
                                        ChargeTBA.PracticeID = UD.PracticeID;
                                        ChargeTBA.LocationID = locationID;
                                        ChargeTBA.ProviderID = providerID;

                                        if (VisitTBA.PrimaryPatientPlanID.GetValueOrDefault() != 0)
                                        {
                                            ChargeTBA.PrimaryPatientPlanID = VisitTBA.PrimaryPatientPlanID.GetValueOrDefault();
                                        }

                                        if (VisitTBA.SecondaryPatientPlanID.GetValueOrDefault() != 0)
                                        {
                                            ChargeTBA.SecondaryPatientPlanID = VisitTBA.SecondaryPatientPlanID.GetValueOrDefault();
                                        }

                                        ChargeTBA.Modifier1ID = ExCh.Modifier1ID;
                                        ChargeTBA.Modifier2ID = ExCh.Modifier2ID;
                                        ChargeTBA.Modifier3ID = ExCh.Modifier3ID;
                                        ChargeTBA.Modifier4ID = ExCh.Modifier4ID;


                                        ChargeTBA.Units = ExCh.DaysOrUnits;
                                        ChargeTBA.DateOfServiceFrom = ExCh.DateOfService;
                                        ChargeTBA.DateOfServiceTo = ExCh.DateOfService;
                                        ChargeTBA.CPTID = ExCh.CPTID.GetValueOrDefault();
                                        ChargeTBA.TotalAmount = ExCh.Charges;
                                        ChargeTBA.PrimaryBilledAmount = ExCh.Charges;
                                        ChargeTBA.PrimaryBal = ExCh.Charges;
                                        ChargeTBA.IsSubmitted = true;
                                        ChargeTBA.PrimaryStatus = "S";
                                        ChargeTBA.SubmittetdDate = ChargeTBA.DateOfServiceFrom;
                                        if (VisitTBA.ICD1ID != null)
                                        {
                                            ChargeTBA.Pointer1 = "1";
                                        }
                                        if (VisitTBA.ICD2ID != null)
                                        {
                                            ChargeTBA.Pointer2 = "1";
                                        }
                                        if (VisitTBA.ICD3ID != null)
                                        {
                                            ChargeTBA.Pointer3 = "1";
                                        }
                                        if (VisitTBA.ICD4ID != null)
                                        {
                                            ChargeTBA.Pointer4 = "1";
                                        }
                                        ChargeTBA.AddedDate = ChargeTBA.DateOfServiceFrom;
                                        ChargeTBA.AddedBy = UD.Email;
                                        ChargeTBA.VisitID = VisitTBA.ID;
                                        if (TempPrimaryPatientPlan != null)
                                            ChargeTBA.PrimaryPatientPlanID = TempPrimaryPatientPlan.ID;
                                        if (TempSecondaryPatientPlan != null)
                                            ChargeTBA.SecondaryPatientPlanID = TempSecondaryPatientPlan.ID;
                                        #endregion

                                        _context.Charge.Add(ChargeTBA);

                                        visitCharges.Add(ChargeTBA);
                                        TempData.Charges.Add(ChargeTBA);
                                        TempData.ExternalCharges.Add(ExCh);
                                    }
                                }

                            }
                            VisitAndChargesToLink.Add(TempData);


                        }
                        catch (Exception ex)
                        {
                            return BadRequest("Something went wrong. Please contact BellMedEx");
                        }
                    }
                    _context.SaveChanges();

                    foreach (VisitData Temp in VisitAndChargesToLink)
                    {
                        for (int i = 0; i < Temp.ExternalCharges.Count; i++)
                        {
                            Temp.ExternalCharges.ElementAt(i).VisitID = Temp.Visit.ID;
                            Temp.ExternalCharges.ElementAt(i).ChargeID = Temp.Charges.ElementAt(i).ID;
                            Temp.ExternalCharges.ElementAt(i).PrimaryInsuredID = Temp.Visit.PrimaryPatientPlanID;
                            Temp.ExternalCharges.ElementAt(i).MergeStatus = "A";
                            _context.ExternalCharge.Update(Temp.ExternalCharges.ElementAt(i));
                        }
                    }
                    _context.SaveChanges();
                    return Ok(returnObject);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return BadRequest("Something went wrong. Please contact BellMedEx");
                }
            }


        }

        [HttpPost]
        [Route("AddChargesDataVectorFormat")]
        public async Task<ActionResult> AddChargesDataVectorFormat(VMDataMigration InputData)
        {
            try
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt")))
                {
                    System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "Start of add charges vector format - Log Time: "+DateTime.Now+"\n");
                }
                else
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "Start of add charges vector format - Log Time: " + DateTime.Now + "\n");
                }

                int FileStartCount = int.Parse(_config["DataMigrationSettings:VectorFileReadStart"]);

                ReturnObject returnObject = new ReturnObject();
                UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
                    User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
                    User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
                long ClientID = UD.ClientID;
                int ChargesAdded = 0, GroupsAdded = 0, VisitsAdded = 0, ExternalChargesAdded = 0;

                System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "UD retrieved, starting preloadin of db tables - Log Time: " + DateTime.Now + "\n");

                //List<Practice> PracticeData = _context.Practice.ToList();
                //List<Provider> ProviderData = _context.Provider.ToList();
                //List<Models.Location> LocationData = _context.Location.ToList();
                //List<Patient> PatientData = _context.Patient.ToList();
                //List<InsurancePlan> InsurancePlanData = _context.InsurancePlan.ToList();
                //List<ExternalPatient> ExternalPatientData = _context.ExternalPatient.ToList();
                //List<ExInsuranceMapping> ExInsuranceMappingData = _contextMain.ExInsuranceMapping.ToList();
                //List<PatientPlan> PatientPlanData = _context.PatientPlan.ToList();
                List<ExternalCharge> ExternalChargeData = _context.ExternalCharge.ToList();
                //List<POS> POSTable = _context.POS.ToList();
                //List<Cpt> CptTable = _context.Cpt.ToList();
                //List<Modifier> ModifierTable = _context.Modifier.ToList();

                System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "All required preloading of tables done - Log Time: " + DateTime.Now + "\n");


                ICD DummyIcd = _context.ICD.Where(lamb => lamb.ICDCode.Equals("99999999")).FirstOrDefault();
                if (DummyIcd == null)
                {
                    DummyIcd = new ICD();
                    DummyIcd.ICDCode = "99999999";
                    DummyIcd.Description = "Dummy ICD";
                    DummyIcd.AddedDate = DateTime.Now;
                    DummyIcd.AddedBy = UD.Email;
                    DummyIcd.IsActive = true;
                    DummyIcd.IsDeleted = false;
                    _context.ICD.Add(DummyIcd);
                    _context.SaveChanges();
                }
                long ExternalChargeInitialID = 0, ExternalPatientIDAfterInsertion = 0;

                if (ExternalChargeData.Count() != 0)
                {
                    ExternalChargeInitialID = ExternalChargeData.OrderBy(lamb => lamb.ID).ElementAt(ExternalChargeData.Count() - 1).ID;
                }

                byte[] bytes = Convert.FromBase64String(InputData.UploadModel.Content.Substring(InputData.UploadModel.Content.IndexOf("base64,") + 7));
                string FileName = InputData.UploadModel.Name;

                //Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
                //string OutputPath = Path.Combine("\\\\", settings.DocumentServerURL,
                //        settings.DocumentServerDirectory, "DataMigration", UD.ClientID + "",
                //        DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));

                //if (System.IO.Directory.Exists(OutputPath))
                //{
                //    System.IO.File.WriteAllBytes(Path.Combine(OutputPath, FileName), bytes);
                //}
                //else
                //{
                //    System.IO.Directory.CreateDirectory(OutputPath);
                //    System.IO.File.WriteAllBytes(Path.Combine(OutputPath, FileName), bytes);
                //}
                System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "Initiating file read - Log Time: " + DateTime.Now + "\n");

                using (MemoryStream stream = new MemoryStream(bytes))
                {

                    System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "File stream read - Log Time: " + DateTime.Now + "\n");

                    long practiceID;
                    long providerID;
                    long locationID;
                    long InterPracticeID = 0;
                    long InterProviderID = 0;
                    long InterLocationID = 0;
                    int RowNum = 0;
                    //Stream Stream = contents;

                    practiceID = InputData.PracticeID;
                    providerID = InputData.ProviderID;
                    locationID = InputData.LocationID;

                    if (InputData.PracticeID.IsNull())
                        return BadRequest("Practice id not present");
                    if (InputData.ProviderID.IsNull())
                        return BadRequest("Provider id not present");
                    if (InputData.LocationID.IsNull())
                        return BadRequest("Location id not present");



                    //SaveStreamAsFile("D:\\", Stream, "streamedfile.xlsx");

                    SpreadsheetDocument Doc = SpreadsheetDocument.Open(stream, false);

                    WorkbookPart WorkbookPart = Doc.WorkbookPart;
                    WorksheetPart WorksheetPart = WorkbookPart.WorksheetParts.First();
                    Worksheet Sheet = WorksheetPart.Worksheet;

                    SharedStringTablePart SstPart = WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();

                    SharedStringTable Sst = SstPart.SharedStringTable;

                    var AllCells = Sheet.Descendants<Cell>();
                    var AllRows = Sheet.Descendants<Row>();
                    bool TakeTrioFromSheet = false, TrioFirstPresent = false, TrioSecondPresent = false, TrioThirdPresent = false;

                    Debug.WriteLine(AllRows.Count() + "  " + AllCells.Count());

                    //Adding Patients

                    bool[] PrimaryInsuranceIdPresent = new bool[AllRows.Count()];
                    bool[] SecondaryInsuranceIdPresent = new bool[AllRows.Count()];

                    List<List<ExternalCharge>> FinalChargesData = new List<List<ExternalCharge>>();
                    List<string> VisitAmounts = new List<string>();

                    List<ExternalCharge> CurrentPatientCharges = new List<ExternalCharge>();
                    ExternalCharge TempChargeWithPatientInfo = new ExternalCharge();



                    decimal VisitCharge = 0, VisitBalance = 0, VisitAdj = 0, VisitPatientpayment = 0, VisitInsurancepayment = 0;

                    long GroupID = 1, ErrorGroupID = 0;

                    if (ExternalChargeData.OrderByDescending(lamb => lamb.GroupID).Count() != 0)
                    {
                        GroupID = ExternalChargeData.OrderByDescending(lamb => lamb.GroupID).FirstOrDefault() != null ? ExternalChargeData.OrderByDescending(lamb => lamb.GroupID).FirstOrDefault().GroupID + 1 : 0;
                    }
                    ExternalCharge PreviousRow = null;
                    List<PatientPlanWithPatient> PatientPlanMapping = new List<PatientPlanWithPatient>();
                    System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "Reading data from rows - Log Time: " + DateTime.Now + "\n");

                    for (int RowIndex = FileStartCount-1; RowIndex < AllRows.Count(); RowIndex++)
                    {
                        try
                        {
                            ExternalCharge TempCharge = new ExternalCharge();
                            bool IsChargeDataPresent = false;
                            List<Cell> RowCells = AllRows.ElementAt(RowIndex).Elements<Cell>().ToList();
                            bool SummationIdentifier = true;


                            //Debug.Write("start of row");
                            System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "Reading data from columns of row " + RowIndex + " - Log Time: " + DateTime.Now + "\n");

                            for (int ColIndex = 0; ColIndex < RowCells.Count(); ColIndex++)
                            {
                                try
                                {
                                    CellReferenceValuePair pair = GetValueFromCell(RowCells.ElementAt(ColIndex), Sst);

                                    if (pair != null && pair.Value != null && !pair.Reference.IsNull())
                                    {
                                        //Debug.Write(pair.Reference+ "       "+pair.Value);
                                        string ReferenceAlphabets = new String(pair.Reference.Where(Char.IsLetter).ToArray());
                                        string FinalIdentifier = "";
                                        pair.Reference = new String(pair.Reference.Where(Char.IsLetter).ToArray());

                                        for (int StrIndex = 0; StrIndex < pair.Reference.Length; StrIndex++)
                                        {
                                            FinalIdentifier += GetColumnQualifier(pair.Reference[StrIndex]);
                                        }


                                        if (FinalIdentifier.Equals("1"))
                                        {

                                            string CompleteName = pair.Value.Trim();

                                            string[] FirstSplit = CompleteName.Split(',');
                                            string[] SecondSplit = FirstSplit[1].Trim().Split(' ');
                                            string FirstName = "", MI = "";
                                            if (SecondSplit.Length > 1)
                                            {
                                                string temp = SecondSplit[1];
                                                if (temp.Length > 3)
                                                {
                                                    FirstName = SecondSplit[0].Trim().ToUpper() + " " + SecondSplit[1].Trim().ToUpper();
                                                    if (SecondSplit.Length > 2)
                                                    {
                                                        MI = SecondSplit[2];
                                                    }
                                                }
                                                else
                                                {
                                                    FirstName = SecondSplit[0].Trim().ToUpper();
                                                    TempCharge.FirstName = SecondSplit[0].Trim().ToUpper();
                                                    //if (SecondSplit.Length > 1)
                                                    //    TempCharge.MiddleInitial = SecondSplit[1].Trim().ToUpper();
                                                    //else
                                                    //    TempCharge.MiddleInitial = "";
                                                    //MI = TempCharge.MiddleInitial;
                                                }
                                            }
                                            else
                                            {
                                                FirstName = SecondSplit[0].Trim().ToUpper();
                                            }
                                            TempCharge.LastName = FirstSplit[0].Trim().ToUpper();
                                            TempCharge.FirstName = FirstName;
                                            //TempCharge.MiddleInitial = MI;

                                        }

                                        //if (FinalIdentifier.Equals("4"))
                                        //{
                                        //    //TempChargeWithPatientInfo.DOB = DateTime.FromOADate(pair.Value).ToString(@"dd\/MM\/yyyy");
                                        //    TempCharge.DOB = DateTime.FromOADate(double.Parse(pair.Value)).ToString(@"MM\/dd\/yyyy");
                                        //}
                                        #region resource assignment
                                        else if (FinalIdentifier.Equals("3"))
                                        {
                                            // Debug.Write(pair.Value + " " + RowIndex);
                                            IsChargeDataPresent = true;
                                            if (pair.Value.Contains("/") || pair.Value.Contains("-"))
                                            {
                                                CultureInfo invC = CultureInfo.InvariantCulture;
                                                TempCharge.DateOfService = DateTime.Parse(pair.Value, invC, DateTimeStyles.None);
                                            }
                                            else
                                            {
                                                TempCharge.DateOfService = DateTime.FromOADate(double.Parse(pair.Value));

                                            }

                                        }
                                        else if (FinalIdentifier.Equals("6"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.DaysOrUnits = pair.Value;

                                        }
                                        else if (FinalIdentifier.Equals("7"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.Charges = Math.Abs(decimal.Parse(pair.Value));

                                        }
                                        else if (FinalIdentifier.Equals("8"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.InsurancePayment = Math.Abs(decimal.Parse(pair.Value));

                                        }
                                        else if (FinalIdentifier.Equals("9"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.PatientPayment = Math.Abs(decimal.Parse(pair.Value));

                                        }
                                        else if (FinalIdentifier.Equals("10"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.Adj = Math.Abs(decimal.Parse(pair.Value));

                                        }
                                        else if (FinalIdentifier.Equals("11"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.Balance = Math.Abs(decimal.Parse(pair.Value));

                                        }
                                        else if (FinalIdentifier.Equals("12"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.PrescribingMD = pair.Value;

                                        }

                                        else if (FinalIdentifier.Equals("2"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.InsuranceName = pair.Value.ToUpper();
                                        }
                                        else if (FinalIdentifier.Equals("0"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.ExternalPatientID = pair.Value.Replace(".0", "");
                                            //Patient patient = PatientData.Where(p => p.ExternalPatientID == TempCharge.ExternalPatientID).FirstOrDefault();
                                            //if (patient != null)
                                            //{
                                            //    TempCharge.PatientID = patient.ID;
                                            //}
                                            //else
                                            //{
                                            //    TempCharge.ErrorMessage = TempCharge.ErrorMessage + "Patient not found. ";
                                            //}

                                        }
                                        else if (FinalIdentifier.Equals("5"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.CptCode = pair.Value.Replace(".0", "");

                                            //Cpt tempcpt = CptTable.Where(c => c.CPTCode.Equals(TempCharge.CptCode)).FirstOrDefault();
                                            //if (tempcpt != null)
                                            //{
                                            //    TempCharge.CPTID = tempcpt.ID;
                                            //}
                                            //else
                                            //{
                                            //    TempCharge.ErrorMessage = TempCharge.ErrorMessage + "Cpt not found. ";
                                            //}

                                        }
                                        else if (FinalIdentifier.Equals("4"))
                                        {
                                            //Debug.WriteLine(pair.Value);
                                            TempCharge.POSCode = pair.Value;
                                            //POS Posid = POSTable.Where(c => c.PosCode.Equals(pair.Value.Replace(".0", ""))).FirstOrDefault();
                                            //if (Posid != null)
                                            //{
                                            //    TempCharge.POSID = Posid.ID;
                                            //}
                                        }
                                        #endregion

                                    }

                                }
                                catch (Exception ex)
                                {
                                    System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "Exception Generated with message : " + ex.Message + "\nStack Trace : " + ex.StackTrace+ " - Log Time: " + DateTime.Now + "\n");
                                    return BadRequest("Unexpected Error Occurred. Please Contect BellMedEx\n" + ex.Message + "\n" + ex.StackTrace);
                                }
                            }

                            System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "Done reading data from columns of row " + RowIndex + " - Log Time: " + DateTime.Now + "\n");



                            //if (TempCharge.PatientID != null)
                            //{

                            //    PatientPlan TempPrimaryPatientPlan = PatientPlanData.Where(lamb => lamb.Coverage == "P" && lamb.PatientID.Equals(TempCharge.PatientID)).FirstOrDefault();
                            //    if (TempPrimaryPatientPlan != null)
                            //    {
                            //        InsurancePlan TempInsurancePlan = InsurancePlanData.Where(lamb => lamb.PlanName.ToUpper().Replace(" ","") == TempCharge.InsuranceName.ToUpper().Replace(" ", "")).FirstOrDefault();
                            //        if (TempInsurancePlan != null)
                            //        {
                            //            if (TempPrimaryPatientPlan.InsurancePlanID == TempInsurancePlan.ID)
                            //            {
                            //                TempCharge.PrimaryInsuredID = TempPrimaryPatientPlan.ID;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            ExInsuranceMapping TempExInsuranceMapping = _contextMain.ExInsuranceMapping.Where(lamb1 => lamb1.ExternalInsuranceName.ToUpper().Replace(" ", "") == TempCharge.InsuranceName.ToUpper().Replace(" ","")).FirstOrDefault();
                            //            if (TempExInsuranceMapping != null)
                            //            {
                            //                InsurancePlan TempInsurancePlan2 = InsurancePlanData.Where(lamb => lamb.ID == TempExInsuranceMapping.InsurancePlanID).FirstOrDefault();
                            //                if (TempInsurancePlan2 != null)
                            //                {
                            //                    if (TempExInsuranceMapping.InsurancePlanID == TempPrimaryPatientPlan.ID)
                            //                    {
                            //                        TempCharge.PrimaryInsuredID = TempPrimaryPatientPlan.ID;
                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        TempCharge.ErrorMessage = TempCharge.ErrorMessage + "Patient plan not found. ";
                            //    }

                            //}

                            if (PreviousRow == null)
                            {
                                PreviousRow = TempCharge;
                            }

                            if (TempCharge.DateOfService != PreviousRow.DateOfService || TempCharge.InsuranceName != PreviousRow.InsuranceName || TempCharge.PatientID != PreviousRow.PatientID)
                            {
                                GroupID++;
                            }

                            if (TempCharge.InsuranceName == null)
                            {
                                GroupID = ErrorGroupID;
                            }

                            if (!TempCharge.DateOfService.IsNull())
                            {

                                TempCharge.LocationID = locationID;
                                TempCharge.PracticeID = practiceID;
                                TempCharge.ProviderID = providerID;


                                //if (TempCharge.CPTID == 0)
                                //{
                                //    TempCharge.CPTID = null;
                                //    if (TempCharge.ErrorMessage != null)
                                //    {
                                //        if (!TempCharge.ErrorMessage.Contains("invalid cpt"))
                                //            TempCharge.ErrorMessage = TempCharge.ErrorMessage + "invalid cpt";
                                //    }
                                //    else
                                //    {
                                //        TempCharge.ErrorMessage = "invalid cpt";
                                //    }
                                //}
                                //else if (TempCharge.CPTID.IsNull())
                                //{
                                //    if (TempCharge.ErrorMessage != null)
                                //    {
                                //        if (!TempCharge.ErrorMessage.Contains("invalid cpt"))
                                //            TempCharge.ErrorMessage = TempCharge.ErrorMessage + "invalid cpt";
                                //    }
                                //    else
                                //    {
                                //        TempCharge.ErrorMessage = "invalid cpt";
                                //    }
                                //}
                                TempCharge.DXCode1 = DummyIcd.ID + "";
                                TempCharge.Gender = TempChargeWithPatientInfo.Gender;
                                TempCharge.MergeStatus = "N";
                                TempCharge.AddedDate = TempCharge.DateOfService;
                                TempCharge.AddedBy = UD.Email;
                                //TempCharge.GroupID = GroupID;
                                TempCharge.FileName = FileName;
                                TempCharge.PaymentProcessed = "NP";
                                TempCharge.SubmittetdDate = TempCharge.DateOfService;
                                TempCharge.GroupID = GroupID;
                                CurrentPatientCharges.Add(TempCharge);
                                _context.ExternalCharge.Add(TempCharge);
                                ExternalChargesAdded++;
                            }

                            PreviousRow = TempCharge;

                            if (RowIndex % 5000 == 0)
                            {
                                _context.SaveChanges();
                                System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), RowIndex + " rows added to db - Log Time: " + DateTime.Now + "\n");

                            }
                        }

                        catch (Exception ex)
                        {
                            System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "Exception Generated with message : " + ex.Message + "\nStack Trace : " + ex.StackTrace+ " - Log Time: " + DateTime.Now + "\n");

                            return BadRequest("Unexpected Error Occurred. Please Contect BellMedEx\n" + ex.Message + "\n" + ex.StackTrace);
                        }
                    }
                    System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), " All rows read and added to db - Log Time: " + DateTime.Now + "\n");

                    _context.SaveChanges();

                    Debug.WriteLine("external charges added: " + ExternalChargesAdded + ", visits added: " + VisitsAdded + ", charges added: " + ChargesAdded + ", groups added: " + GroupsAdded);
                    return Ok(returnObject);
                }

            }catch(Exception ex)
            {
                System.IO.File.AppendAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Add Charges Vector Logs.txt"), "Exception Generated with message : " + ex.Message + "\nStack Trace : " + ex.StackTrace+ " - Log Time: " + DateTime.Now + "\n");
                return BadRequest("Unexpected Error Occurred. Please Contect BellMedEx\n" + ex.Message + "\n" + ex.StackTrace);

                
            }
        }



        [HttpPost]
        [Route("AddPatiensFromSmartSheetExcel")]
        public async Task<ActionResult> AddPatiensFromSmartSheetExcel(VMDataMigration InputData)
        {
            bool ErrorsCaught = false;
            //var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            //using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            //{

            try
            {
                UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
                User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
                User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

                long ClientID = UD.ClientID;

                List<Edi837Payer> Edi837PayerData = _context.Edi837Payer.ToList();
                List<InsurancePlan> InsurancePlanData = _context.InsurancePlan.ToList();
                List<Practice> PracticeData = _context.Practice.ToList();
                List<Provider> ProviderData = _context.Provider.ToList();
                List<Models.Location> LocationData = _context.Location.ToList();
                List<PlanType> PlanTypeData = _context.PlanType.ToList();
                List<Patient> PatientData = _context.Patient.Where(x => x.PracticeID == UD.PracticeID).ToList();
                List<ExternalPatient> ExternalPatientList = _context.ExternalPatient.Where(x => x.PracticeID == UD.PracticeID).ToList();
                List<long> ExInsuranceMappingIds = _contextMain.ExInsuranceMapping.Select(lamb => lamb.ID).ToList();
                List<long> InsurancePlanIds = _context.InsurancePlan.Select(lamb => lamb.ID).ToList();
                long ExternalPatientInitialID = 0, ExternalPatientIDAfterInsertion = 0;

                if (ExternalPatientList.Count() != 0)
                {
                    ExternalPatientInitialID = ExternalPatientList.OrderBy(lamb => lamb.ID).ElementAt(ExternalPatientList.Count() - 1).ID;
                }
                //if (InputData.Type.Equals("AddPatiensFromSmartSheetExcel"))
                //{
                // string FilePath = ;
                ReturnObject returnObject = new ReturnObject();
                byte[] bytes = Convert.FromBase64String(InputData.UploadModel.Content.Substring(InputData.UploadModel.Content.IndexOf("base64,") + 7));
                string FileName = InputData.UploadModel.Name;

                Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
                string OutputPath = Path.Combine("\\\\", settings.DocumentServerURL,
                        settings.DocumentServerDirectory, "DataMigration", UD.ClientID + "",
                        DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));

                if (System.IO.Directory.Exists(OutputPath))
                {
                    System.IO.File.WriteAllBytes(Path.Combine(OutputPath, FileName), bytes);
                }
                else
                {
                    System.IO.Directory.CreateDirectory(OutputPath);
                    System.IO.File.WriteAllBytes(Path.Combine(OutputPath, FileName), bytes);
                }

                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    long practiceID;
                    long providerID;
                    long locationID;
                    long InterPracticeID = 0;
                    long InterProviderID = 0;
                    long InterLocationID = 0;
                    //Stream Stream = contents;

                    practiceID = InputData.PracticeID;
                    providerID = InputData.ProviderID;
                    locationID = InputData.LocationID;

                    if (InputData.PracticeID == null)
                        return BadRequest("Practice id not present");
                    if (InputData.ProviderID == null)
                        return BadRequest("Provider id not present");
                    if (InputData.LocationID == null)
                        return BadRequest("Location id not present");



                    //SaveStreamAsFile("D:\\", Stream, "streamedfile.xlsx");

                    SpreadsheetDocument Doc = SpreadsheetDocument.Open(stream, false);

                    WorkbookPart WorkbookPart = Doc.WorkbookPart;
                    WorksheetPart WorksheetPart = WorkbookPart.WorksheetParts.First();
                    Worksheet Sheet = WorksheetPart.Worksheet;

                    SharedStringTablePart SstPart = WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();

                    SharedStringTable Sst = SstPart.SharedStringTable;

                    var AllCells = Sheet.Descendants<Cell>();
                    var AllRows = Sheet.Descendants<Row>();

                    bool TakeTrioFromSheet = false, TrioFirstPresent = false, TrioSecondPresent = false, TrioThirdPresent = false;

                    Debug.WriteLine(AllRows.Count() + "  " + AllCells.Count());

                    //Adding Patients

                    bool[] PrimaryInsuranceIdPresent = new bool[AllRows.Count()];
                    bool[] SecondaryInsuranceIdPresent = new bool[AllRows.Count()];

                    //string[] PrimaryInsurancePlanIds = new string[AllRows.Count()];
                    //string[] SecondaryInsurancePlanIds = new string[AllRows.Count()];

                    //PatientPlan[] PrimaryPatientPlans = new PatientPlan[AllRows.Count()];
                    //PatientPlan[] SecondaryPatientPlans = new PatientPlan[AllRows.Count()];

                    List<PatientPlanWithID> PrimaryPatientPlanWithIDs = new List<PatientPlanWithID>();
                    List<PatientPlanWithID> SecondaryPatientPlanWithIDs = new List<PatientPlanWithID>();

                    int RowNum = 0;
                    //string[] PatientIds = new string[AllRows.Count()]; 
                    List<string> PatientIds = new List<string>();
                    Patient[] PatientsAdded = new Patient[AllRows.Count()];
                    foreach (Row row in AllRows)
                    {
                        bool TrioFirstValuePresent = false, TrioSecondValuePresent = false, TrioThirdValuePresent = false;
                        try
                        {
                            InterPracticeID = 0;
                            InterProviderID = 0;
                            InterLocationID = 0;
                            if (RowNum != 0)
                            {
                                //int ColNum = 1;
                                //Debug.WriteLine(row.Elements<Cell>().Count());
                                List<Cell> RowCells = row.Elements<Cell>().ToList();
                                ExternalPatient externalPatient = new ExternalPatient();

                                for (int i = 0; i < RowCells.Count; i++)
                                {
                                    try
                                    {
                                        CellReferenceValuePair pair = GetValueFromCell(RowCells.ElementAt(i), Sst);
                                        if (pair != null && pair.Value != null && !pair.Reference.IsNull())
                                        {

                                            string ReferenceAlphabets = new String(pair.Reference.Where(Char.IsLetter).ToArray());
                                            string FinalIdentifier = "";
                                            pair.Reference = new String(pair.Reference.Where(Char.IsLetter).ToArray());

                                            for (int StrIndex = 0; StrIndex < pair.Reference.Length; StrIndex++)
                                            {
                                                FinalIdentifier += GetColumnQualifier(pair.Reference[StrIndex]);
                                            }
                                            if (FinalIdentifier.Equals("14"))
                                                externalPatient.Status = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            if (FinalIdentifier.Equals("1"))
                                            {
                                                string CompleteName = pair.Value.Trim();
                                                if (CompleteName.Equals("Zander, Theodore A"))
                                                {
                                                    Debug.WriteLine("");
                                                }
                                                string[] FirstSplit = CompleteName.Split(',');
                                                string[] SecondSplit = FirstSplit[1].Trim().Split(' ');
                                                string FirstName = "", MI = "";
                                                if (SecondSplit.Length > 1)
                                                {
                                                    string temp = SecondSplit[1];
                                                    if (temp.Length > 3)
                                                    {
                                                        FirstName = SecondSplit[0].Trim().ToUpper() + " " + SecondSplit[1].Trim().ToUpper();
                                                        if (SecondSplit.Length > 2)
                                                        {
                                                            MI = SecondSplit[2];
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FirstName = SecondSplit[0].Trim().ToUpper();
                                                        externalPatient.FirstName = SecondSplit[0].Trim().ToUpper();
                                                        if (SecondSplit.Length > 1)
                                                            externalPatient.MiddleInitial = SecondSplit[1].Trim().ToUpper();
                                                        else
                                                            externalPatient.MiddleInitial = "";
                                                        MI = externalPatient.MiddleInitial;
                                                    }
                                                }
                                                else
                                                {
                                                    FirstName = SecondSplit[0].Trim().ToUpper();
                                                }
                                                externalPatient.LastName = FirstSplit[0].Trim().ToUpper();
                                                externalPatient.FirstName = FirstName;
                                                externalPatient.MiddleInitial = MI;
                                                Debug.WriteLine(FirstName + "   " + externalPatient.LastName);
                                            }
                                            if (FinalIdentifier.Equals("3"))
                                                externalPatient.Address1 = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            if (FinalIdentifier.Equals("4"))
                                                externalPatient.City = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            if (FinalIdentifier.Equals("5"))
                                                if (pair.Value.Length < 3)
                                                {
                                                    externalPatient.State = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                                }
                                            if (FinalIdentifier.Equals("6"))
                                                externalPatient.ZipCode = pair.Value.IsNull() ? "" : pair.Value.Replace("-", "").Replace(".0", "");
                                            if (FinalIdentifier.Equals("7"))
                                                externalPatient.PhoneNumber = pair.Value.IsNull() ? "" : pair.Value.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

                                            if (FinalIdentifier.Equals("2"))
                                            {
                                                if (pair.Value.Contains("-") || pair.Value.Contains("/"))
                                                {

                                                    externalPatient.DOB = pair.Value.IsNull() ? new DateTime() : DateTime.Parse(pair.Value);
                                                }
                                                else
                                                {
                                                    externalPatient.DOB = pair.Value.IsNull() ? new DateTime() : DateTime.FromOADate(double.Parse(pair.Value));
                                                }
                                            }
                                            if (FinalIdentifier.Equals("9"))
                                            {
                                                if (pair.Value != null && pair.Value.Contains(".") && pair.Value.Contains("E"))
                                                {
                                                    decimal LeftPart = decimal.Parse(pair.Value.Split("E")[0]);
                                                    long Multiplier = long.Parse(pair.Value.Split("E")[1]);
                                                    decimal MultiplierTemp = 1;
                                                    for (int l = 0; l < Multiplier; l++)
                                                    {
                                                        MultiplierTemp *= 10;
                                                    }
                                                    long finalSubscriberID = Convert.ToInt64(LeftPart * MultiplierTemp);
                                                    externalPatient.PrimaryInsuredID = "" + finalSubscriberID;
                                                    PrimaryInsuranceIdPresent[RowNum] = true;
                                                }
                                                else
                                                {
                                                    if (pair.Value.Contains("."))
                                                    {
                                                        externalPatient.PrimaryInsuredID = pair.Value.Replace(".0", "");
                                                        PrimaryInsuranceIdPresent[RowNum] = true;
                                                    }
                                                    else
                                                    {
                                                        externalPatient.PrimaryInsuredID = pair.Value;
                                                        PrimaryInsuranceIdPresent[RowNum] = true;
                                                    }
                                                }
                                            }
                                            if (FinalIdentifier.Equals("10"))
                                            {
                                                externalPatient.PrimaryInsuredName = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                                externalPatient.SecondaryInsuredName = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            }
                                            if (FinalIdentifier.Equals("11"))
                                                externalPatient.PrimaryInsurance = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            if (FinalIdentifier.Equals("12"))
                                            {
                                                if (pair.Value != null && pair.Value.Contains(".") && pair.Value.Contains("E"))
                                                {
                                                    decimal LeftPart = decimal.Parse(pair.Value.Split("E")[0]);
                                                    long Multiplier = long.Parse(pair.Value.Split("E")[1]);
                                                    decimal MultiplierTemp = 1;
                                                    for (int l = 0; l < Multiplier; l++)
                                                    {
                                                        MultiplierTemp *= 10;
                                                    }
                                                    long finalSubscriberID = Convert.ToInt64(LeftPart * MultiplierTemp);
                                                    externalPatient.SecondaryInsuredID = "" + finalSubscriberID;
                                                    PrimaryInsuranceIdPresent[RowNum] = true;
                                                }
                                                else
                                                {
                                                    if (pair.Value.Contains("."))
                                                    {
                                                        externalPatient.SecondaryInsuredID = pair.Value.Replace(".0", "");
                                                        SecondaryInsuranceIdPresent[RowNum] = true;
                                                    }
                                                    else
                                                    {
                                                        externalPatient.SecondaryInsuredID = pair.Value;
                                                        SecondaryInsuranceIdPresent[RowNum] = true;
                                                    }
                                                }
                                            }
                                            if (FinalIdentifier.Equals("13"))
                                            {
                                                externalPatient.SecondaryInsurance = pair.Value.IsNull() ? "" : pair.Value.ToUpper();
                                            }
                                            if (FinalIdentifier.Equals("14"))
                                                externalPatient.PracticeNPI = pair.Value.IsNull() ? "" : pair.Value;
                                            if (FinalIdentifier.Equals("15"))
                                                externalPatient.LocationNPI = pair.Value.IsNull() ? "" : pair.Value;
                                            if (FinalIdentifier.Equals("16"))
                                                externalPatient.ProviderNPI = pair.Value.IsNull() ? "" : pair.Value;


                                            if (RowNum == 1)
                                            {
                                                if (ReferenceAlphabets.Equals("O"))
                                                    TrioFirstPresent = true;
                                                if (ReferenceAlphabets.Equals("P"))
                                                    TrioSecondPresent = true;
                                                if (ReferenceAlphabets.Equals("Q"))
                                                    TrioThirdPresent = true;

                                                if (TrioFirstPresent && TrioSecondPresent && TrioThirdPresent)
                                                {
                                                    TakeTrioFromSheet = true;
                                                }
                                                //Debug.WriteLine("Row 2 data :::: " + TrioFirstPresent + "  " + TrioSecondPresent + "  " + TrioThirdPresent + "  " + TakeTrioFromSheet);
                                            }
                                            if (TrioFirstPresent)
                                            {
                                                if (ReferenceAlphabets.Equals("O"))
                                                {
                                                    try
                                                    {
                                                        Practice TempPractice = PracticeData.Where(lamb => lamb.NPI.Equals(pair.Value)).FirstOrDefault();
                                                        InterPracticeID = TempPractice.ID;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        return BadRequest("Problem retrieving Practice id");
                                                    }
                                                }
                                            }
                                            if (TrioSecondPresent)
                                            {
                                                if (ReferenceAlphabets.Equals("P"))
                                                {
                                                    try
                                                    {
                                                        Models.Location TempLocation = LocationData.Where(lamb => lamb.NPI.Equals(pair.Value)).FirstOrDefault();
                                                        InterLocationID = TempLocation.ID;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        return BadRequest("Problem retrieving Location id");
                                                    }
                                                }
                                            }
                                            if (TrioThirdPresent)
                                            {
                                                if (ReferenceAlphabets.Equals("Q"))
                                                {
                                                    try
                                                    {
                                                        Provider TempProvider = ProviderData.Where(lamb => lamb.NPI.Equals(pair.Value)).FirstOrDefault();
                                                        InterProviderID = TempProvider.ID;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        return BadRequest("Problem retrieving Provider id");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorsCaught = true;
                                        return BadRequest("UnExpected error occured. Please contact Bellmedex\n" + ex.Message + "\n" + ex.StackTrace);
                                    }
                                }



                                //TempPatient.AccountNum = _context.GetNextSequenceValue("S_PATIENTACCOUNTNUMBER");
                                externalPatient.AddedDate = DateTime.Now;
                                externalPatient.AddedBy = UD.Email.ToUpper();
                                //TempPatient.AddedBy = "maaz@belmedex.com".ToUpper();
                                externalPatient.MergeStatus = "E";
                                externalPatient.IsActive = true;
                                externalPatient.IsRegularRecord = true;
                                externalPatient.FileName = FileName.ToUpper();

                                if (!TakeTrioFromSheet)
                                {
                                    externalPatient.PracticeID = UD.PracticeID;
                                    externalPatient.LocationId = locationID;
                                    externalPatient.ProviderID = providerID;
                                }
                                else
                                {
                                    externalPatient.PracticeID = UD.PracticeID;
                                    externalPatient.LocationId = InterLocationID;
                                    externalPatient.ProviderID = InterProviderID;
                                }

                                if (externalPatient.ExternalPatientID.IsNull())
                                {
                                    externalPatient.ExternalPatientID = DateTime.Now.Format("MMddyyyyhhmmssff");
                                }

                                if (!externalPatient.FirstName.IsNull() && !externalPatient.LastName.IsNull())
                                {


                                    if (_context.ExternalPatient.Where(lamb => lamb.MergeStatus.Equals("A")).Where(lamb => lamb.FirstName.Trim().Replace(" ", "").Equals(externalPatient.FirstName.Trim().Replace(" ", "")) &&
                                                                                                                           lamb.LastName.Trim().Replace(" ", "").Equals(externalPatient.LastName.Trim().Replace(" ", ""))
                                                                                                                         ).Count() != 0)
                                    {
                                        externalPatient.MergeStatus = "D";
                                        _context.ExternalPatient.Add(externalPatient);
                                    }
                                    else
                                    {
                                        string str = null;
                                        if (externalPatient.FirstName == null)
                                        {

                                            str = "FirstName,";
                                            externalPatient.MissingInfo = externalPatient.MissingInfo + str;
                                        }
                                        if (externalPatient.LastName == null)
                                        {
                                            str = "LastName,";
                                            externalPatient.MissingInfo = externalPatient.MissingInfo + str;
                                        }
                                        if (externalPatient.Gender == null)
                                        {
                                            str = "Gender,";
                                            externalPatient.MissingInfo = externalPatient.MissingInfo + str;
                                        }
                                        if (externalPatient.State == null)
                                        {
                                            str = "State,";
                                            externalPatient.MissingInfo = externalPatient.MissingInfo + str;
                                        }
                                        if (externalPatient.DOB == null)
                                        {
                                            str = "DOB,";
                                            externalPatient.MissingInfo = externalPatient.MissingInfo + str;
                                        }
                                        if (externalPatient.ZipCode == null)
                                        {
                                            str = "ZipCode,";
                                            externalPatient.MissingInfo = externalPatient.MissingInfo + str;
                                        }
                                        if (externalPatient.Address1 == null)
                                        {
                                            str = "Address1,";
                                            externalPatient.MissingInfo = externalPatient.MissingInfo + str;
                                        }
                                        if (externalPatient.City == null)
                                        {
                                            str = "City,";
                                            externalPatient.MissingInfo = externalPatient.MissingInfo + str;
                                        }

                                        if (externalPatient.MissingInfo != null)
                                        {
                                            externalPatient.MissingInfo = externalPatient.MissingInfo.TrimEnd(',');
                                            if (externalPatient.MissingInfo.Contains(","))
                                            {
                                                externalPatient.MissingInfo = externalPatient.MissingInfo + " are missing";
                                            }
                                            else
                                            {
                                                externalPatient.MissingInfo = externalPatient.MissingInfo + " is missing";
                                            }
                                        }

                                        //}
                                        _context.ExternalPatient.Add(externalPatient);
                                    }
                                }
                            }
                            RowNum++;
                        }
                        catch (Exception ex)
                        {
                            ErrorsCaught = true;
                            return BadRequest("UnExpected error occured. Please contact Bellmedex\n" + ex.Message + "\n" + ex.StackTrace);
                        }
                    }
                    _context.SaveChanges();
                    ExternalPatientList = _context.ExternalPatient.Where(x => x.PracticeID == UD.PracticeID).ToList();
                    if (ExternalPatientList.Count() != 0)
                        ExternalPatientIDAfterInsertion = ExternalPatientList.OrderBy(lamb => lamb.ID).ElementAt(ExternalPatientList.Count() - 1).ID;
                    if (ExternalPatientList.Count() != 0)
                        ExternalPatientList = _context.ExternalPatient.Where(x => x.PracticeID == UD.PracticeID && x.ID > ExternalPatientInitialID && x.ID <= ExternalPatientIDAfterInsertion).ToList();
                    else
                        ExternalPatientList = _context.ExternalPatient.Where(x => x.PracticeID == UD.PracticeID).ToList();

                    int Idex = 0;
                    RowNum = 0;
                    foreach (ExternalPatient externalPatient in ExternalPatientList)
                    {
                        try
                        {
                            if (Idex == 177)
                            {
                                Debug.Write("11");
                            }
                            Debug.WriteLine(Idex + "   row number");
                            Patient patient = new Patient();
                            if (externalPatient.MergeStatus.Equals("E"))
                            {
                                patient.ExternalPatientID = externalPatient.ExternalPatientID;
                                if (externalPatient.LastName != null)
                                {
                                    patient.LastName = externalPatient.LastName.ToUpper();
                                }
                                if (externalPatient.FirstName != null)
                                {
                                    patient.FirstName = externalPatient.FirstName.ToUpper();
                                }
                                patient.MiddleInitial = externalPatient.MiddleInitial.IsNull() ? "" : externalPatient.MiddleInitial.ToUpper();
                                if (!externalPatient.Address1.IsNull())
                                {
                                    patient.Address1 = externalPatient.Address1.ToUpper();
                                }
                                if (!externalPatient.City.IsNull())
                                {
                                    patient.City = externalPatient.City.ToUpper();
                                }
                                if (!externalPatient.State.IsNull())
                                {
                                    patient.State = externalPatient.State.ToUpper();

                                }
                                patient.PrescribingMD = externalPatient.PrescribingMD;
                                if (externalPatient.ZipCode != null)
                                {
                                    patient.ZipCode = externalPatient.ZipCode;
                                }
                                patient.PhoneNumber = externalPatient.PhoneNumber;
                                patient.Email = externalPatient.Email.IsNull() ? "" : externalPatient.Email.ToUpper();
                                if (externalPatient.DOB != null)
                                {
                                    patient.DOB = externalPatient.DOB.HasValue ? externalPatient.DOB : new DateTime();
                                }
                                if (externalPatient.Gender != null)
                                {
                                    patient.Gender = externalPatient.Gender.IsNull() ? "" : externalPatient.Gender.ToUpper();
                                }
                                patient.AccountNum = _context.GetNextSequenceValue("S_PATIENTACCOUNTNUMBER");
                                patient.AddedDate = DateTime.Now;
                                patient.AddedBy = externalPatient.AddedBy.ToUpper();
                                patient.IsActive = externalPatient.IsActive;
                                patient.IsDeleted = externalPatient.IsDeleted;
                                patient.PracticeID = externalPatient.PracticeID;
                                patient.ProviderID = externalPatient.ProviderID;
                                patient.LocationId = externalPatient.LocationId;


                                externalPatient.MergeStatus = "A";

                                List<Patient> MatchedPatients = _context.Patient.Where(x => x.PracticeID == UD.PracticeID).Where(lamb => lamb.ExternalPatientID.Equals(patient.ExternalPatientID)).ToList();
                                if (MatchedPatients.Count() == 0)
                                {
                                    _context.Patient.Add(patient);
                                    PatientIds.Add(patient.ExternalPatientID + "|" + patient.AccountNum);

                                    //PatientsAdded[RowNum] = patient;
                                }
                                else
                                {
                                    PatientIds.Add(MatchedPatients.FirstOrDefault().ExternalPatientID + "|" + MatchedPatients.FirstOrDefault().AccountNum);
                                    //PatientsAdded[RowNum] = MatchedPatients.FirstOrDefault();
                                }
                                externalPatient.AccountNum = patient.AccountNum;
                                _context.ExternalPatient.Update(externalPatient);
                            }


                            Idex++;
                            RowNum++;
                        }
                        catch (Exception ex)
                        {
                            ErrorsCaught = true;
                            return BadRequest("UnExpected error occured. Please contact Bellmedex\n" + ex.Message + "\n" + ex.StackTrace);
                        }
                    }
                    _context.SaveChanges();

                    _context.SaveChanges();
                    //Adding patient plans
                    int primaryInsurances = 0, secondaryInsurances = 0;
                    PatientData = _context.Patient.Where(x => x.PracticeID == UD.PracticeID).ToList();
                    RowNum = 0;
                    string TempExternalPatientID = "";
                    List<long> insuranceplanids = new List<long>();
                    List<long> InsurancePlanIdOutliers = new List<long>();
                    List<PatientPlan> PatientPlanOutliers = new List<PatientPlan>();

                    List<string> AddedExMappingEntries = new List<string>();

                    // Patient Plan

                    foreach (ExternalPatient externalPatient in ExternalPatientList)
                    {
                        try
                        {
                            Debug.WriteLine(RowNum + "    ROW NUM      ");
                            if (externalPatient.MergeStatus.Equals("A"))
                            {
                                Patient TempPatient = PatientData.Where(lamb => lamb.AccountNum == externalPatient.AccountNum).FirstOrDefault();
                                //Notes note = new Notes();
                                //note.PracticeID = UD.PracticeID;
                                //note.Note = "Data Migrated from OA, filename: " + FileName;
                                //note.AddedBy = UD.Email;
                                //note.AddedDate = DateTime.Now;
                                //note.NotesDate = DateTime.Now;
                                //note.PatientID = TempPatient.ID;

                                //_context.Notes.Add(note);

                                if (!externalPatient.PrimaryInsurance.IsNull())
                                {
                                    PatientPlan TempPatientPlan = new PatientPlan();

                                    TempPatientPlan.FirstName = externalPatient.FirstName;
                                    TempPatientPlan.LastName = externalPatient.LastName;
                                    TempPatientPlan.PatientID = TempPatient.ID;
                                    TempPatientPlan.DOB = externalPatient.DOB;
                                    TempPatientPlan.Gender = externalPatient.Gender;
                                    TempPatientPlan.Email = externalPatient.Email;
                                    TempPatientPlan.Address1 = externalPatient.Address1;
                                    TempPatientPlan.City = externalPatient.City;
                                    TempPatientPlan.State = externalPatient.State;
                                    TempPatientPlan.ZipCode = externalPatient.ZipCode;
                                    TempPatientPlan.PhoneNumber = externalPatient.PhoneNumber;
                                    TempPatientPlan.SubscriberId = externalPatient.PrimaryInsuredID;
                                    TempPatientPlan.Coverage = "P";
                                    TempPatientPlan.IsActive = true;
                                    TempPatientPlan.IsDeleted = false;
                                    TempPatientPlan.AddedDate = DateTime.Today.Date;
                                    TempPatientPlan.AddedBy = UD == null ? "" : UD.Email.ToUpper();
                                    TempPatientPlan.RelationShip = 18 + "";
                                    string TempString = externalPatient.PrimaryInsurance;
                                    InsurancePlan PossibleCandidate = _context.InsurancePlan.Where(lamb => lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(externalPatient.PrimaryInsurance.Trim().ToUpper().Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                    if (TempPatientPlan.SubscriberId == "JZH124857136001")
                                    {
                                        Debug.WriteLine(TempString);
                                    }
                                    if (PossibleCandidate != null)
                                    {
                                        TempPatientPlan.InsurancePlanID = PossibleCandidate.ID;
                                    }
                                    else
                                    {
                                        ExInsuranceMapping MappingRecord = _contextMain.ExInsuranceMapping.Where(lamb => lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(externalPatient.PrimaryInsurance.Trim().ToUpper().Replace(" ", ""))).FirstOrDefault();
                                        if (MappingRecord != null)
                                        {
                                            TempPatientPlan.InsurancePlanID = MappingRecord.InsurancePlanID;
                                        }
                                        else
                                        {
                                            TempPatientPlan.InsurancePlanID = null;
                                        }
                                    }

                                    if (TempPatientPlan.InsurancePlanID != null)
                                    {
                                        if (InsurancePlanData.Select(ip => ip.ID).Contains(TempPatientPlan.InsurancePlanID.Value))
                                        {
                                            _context.PatientPlan.Add(TempPatientPlan);

                                            //ExternalPatient TempEP = _context.ExternalPatient.Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(TempExternalPatientID)).FirstOrDefault();
                                            //TempEP.PrimaryPatientPlanID = TempPatientPlan.ID;

                                            //_context.ExternalPatient.Update(TempEP);
                                            PatientPlanWithID TempPPWID = new PatientPlanWithID();
                                            TempPPWID.PatientPlan = TempPatientPlan;
                                            TempPPWID.AccountNumber = externalPatient.ExternalPatientID;

                                            PrimaryPatientPlanWithIDs.Add(TempPPWID);
                                        }
                                        else
                                        {
                                            externalPatient.MissingInfo = externalPatient.MissingInfo + ". Primary patient plan was not created";
                                            _context.Update(externalPatient);
                                        }

                                    }
                                    else
                                    {
                                        externalPatient.MissingInfo = externalPatient.MissingInfo + ". Primary patient plan was not created";
                                        _context.Update(externalPatient);
                                        if (_contextMain.ExInsuranceMapping.Where(exim => exim.ExternalInsuranceName.Equals(externalPatient.PrimaryInsurance)).Count() == 0 && !AddedExMappingEntries.Contains(externalPatient.PrimaryInsurance))
                                        {
                                            ExInsuranceMapping mapping = new ExInsuranceMapping();
                                            mapping.Status = "F";
                                            mapping.ExternalInsuranceName = externalPatient.PrimaryInsurance;
                                            mapping.AddedBy = UD.Email;
                                            mapping.AddedDate = DateTime.Now;
                                            AddedExMappingEntries.Add(externalPatient.PrimaryInsurance);
                                            _contextMain.ExInsuranceMapping.Add(mapping);
                                        }
                                    }
                                }
                                if (!externalPatient.SecondaryInsurance.IsNull())
                                {
                                    PatientPlan TempPatientPlan = new PatientPlan();

                                    TempPatientPlan.FirstName = externalPatient.FirstName;
                                    TempPatientPlan.LastName = externalPatient.LastName;
                                    TempPatientPlan.PatientID = TempPatient.ID;
                                    TempPatientPlan.DOB = externalPatient.DOB;
                                    TempPatientPlan.Gender = externalPatient.Gender;
                                    TempPatientPlan.Email = externalPatient.Email;
                                    TempPatientPlan.Address1 = externalPatient.Address1;
                                    TempPatientPlan.City = externalPatient.City;
                                    TempPatientPlan.State = externalPatient.State;
                                    TempPatientPlan.ZipCode = externalPatient.ZipCode;
                                    TempPatientPlan.PhoneNumber = externalPatient.PhoneNumber;
                                    TempPatientPlan.SubscriberId = externalPatient.SecondaryInsuredID;
                                    TempPatientPlan.Coverage = "S";
                                    TempPatientPlan.IsActive = true;
                                    TempPatientPlan.IsDeleted = false;
                                    TempPatientPlan.AddedDate = DateTime.Today.Date;
                                    TempPatientPlan.AddedBy = UD == null ? "" : UD.Email.ToUpper();
                                    TempPatientPlan.RelationShip = 18 + "";
                                    string chec = externalPatient.SecondaryInsurance;
                                    InsurancePlan PossibleCandidate = _context.InsurancePlan.Where(lamb => lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(externalPatient.SecondaryInsurance.Trim().ToUpper().Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                    if (PossibleCandidate != null)
                                    {
                                        TempPatientPlan.InsurancePlanID = PossibleCandidate.ID;
                                    }
                                    else
                                    {
                                        ExInsuranceMapping MappingRecord = _contextMain.ExInsuranceMapping.Where(lamb => lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(externalPatient.SecondaryInsurance.Trim().ToUpper().Replace(" ", ""))).FirstOrDefault();
                                        if (MappingRecord != null)
                                        {
                                            TempPatientPlan.InsurancePlanID = MappingRecord.InsurancePlanID;
                                        }
                                        else
                                        {
                                            TempPatientPlan.InsurancePlanID = null;
                                        }
                                    }

                                    if (TempPatientPlan.InsurancePlanID != null)
                                    {
                                        if (InsurancePlanData.Select(ip => ip.ID).Contains(TempPatientPlan.InsurancePlanID.Value))
                                        {
                                            _context.PatientPlan.Add(TempPatientPlan);

                                            //ExternalPatient TempEP = _context.ExternalPatient.Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(TempExternalPatientID)).FirstOrDefault();
                                            //TempEP.PrimaryPatientPlanID = TempPatientPlan.ID;

                                            //_context.ExternalPatient.Update(TempEP);
                                            PatientPlanWithID TempPPWID = new PatientPlanWithID();
                                            TempPPWID.PatientPlan = TempPatientPlan;
                                            TempPPWID.AccountNumber = externalPatient.ExternalPatientID;

                                            SecondaryPatientPlanWithIDs.Add(TempPPWID);
                                        }
                                        else
                                        {
                                            externalPatient.MissingInfo = externalPatient.MissingInfo + ". Secondary patient plan was not created";
                                            _context.Update(externalPatient);
                                        }

                                    }
                                    else
                                    {
                                        externalPatient.MissingInfo = externalPatient.MissingInfo + ". Secondary patient plan was not created";
                                        _context.Update(externalPatient);
                                        if (_contextMain.ExInsuranceMapping.Where(exim => exim.ExternalInsuranceName.Equals(externalPatient.PrimaryInsurance)).Count() == 0 && !AddedExMappingEntries.Contains(externalPatient.PrimaryInsurance))
                                        {
                                            ExInsuranceMapping mapping = new ExInsuranceMapping();
                                            mapping.Status = "F";
                                            mapping.ExternalInsuranceName = externalPatient.PrimaryInsurance;
                                            mapping.AddedDate = DateTime.Now;
                                            mapping.AddedBy = UD.Email;
                                            AddedExMappingEntries.Add(externalPatient.PrimaryInsurance);
                                            _contextMain.ExInsuranceMapping.Add(mapping);
                                        }
                                    }
                                }
                            }
                            RowNum++;
                        }
                        catch (Exception ex)
                        {
                            ErrorsCaught = true;
                            return BadRequest("UnExpected error occured. Please contact Bellmedex" + ex.Message + "  " + ex.StackTrace);
                        }
                    }
                    foreach (long atemplong in insuranceplanids)
                    {
                        Debug.WriteLine("this is a temp long " + atemplong);
                    }
                    _context.SaveChanges();

                    foreach (PatientPlanWithID PrimaryPatientPlanWithID in PrimaryPatientPlanWithIDs)
                    {
                        if (PrimaryPatientPlanWithID.AccountNumber != null)
                        {
                            ExternalPatient TempEP = ExternalPatientList.Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(PrimaryPatientPlanWithID.AccountNumber)).FirstOrDefault();
                            TempEP.PrimaryPatientPlanID = PrimaryPatientPlanWithID.PatientPlan.ID;
                            _context.ExternalPatient.Update(TempEP);
                        }
                    }
                    foreach (PatientPlanWithID SecondaryPatientPlanWithID in SecondaryPatientPlanWithIDs)
                    {
                        if (SecondaryPatientPlanWithID.AccountNumber != null)
                        {
                            ExternalPatient TempEP = ExternalPatientList.Where(lamb => lamb.ExternalPatientID.IsNull() ? false : lamb.ExternalPatientID.Equals(SecondaryPatientPlanWithID.AccountNumber)).FirstOrDefault();
                            TempEP.SecondaryPatientPlanID = SecondaryPatientPlanWithID.PatientPlan.ID;
                            _context.ExternalPatient.Update(TempEP);
                        }
                    }
                    _contextMain.SaveChanges();
                    _context.SaveChanges();
                    returnObject.Result = "Data Added Successfully";
                }
                return Ok(returnObject);
                //}             
            }

            catch (Exception ex)
            {
                ErrorsCaught = true;
                return BadRequest("UnExpected error occured. Please contact Bellmedex" + ex.Message + "  " + ex.StackTrace);
            }

        }


        [Route("SaveExChargeMissingInfo")]
        [HttpPost]
        public async Task<ActionResult<ExChargeMissingInfo>> SaveExChargeMissingInfo(IEnumerable<ExChargeMissingInfo> ExchargeM)
        {
            foreach (ExChargeMissingInfo item in ExchargeM)
            {
                ICD icd = _context.ICD.Find(item.ICDID);

                ExternalCharge exc = _context.ExternalCharge.Find(item.ExternalChargeID);
                if (icd != null)
                {
                    exc.DiagnosisCode = icd.ICDCode;
                    if (!exc.ErrorMessage.IsNull())
                    {
                        exc.ErrorMessage = exc.ErrorMessage.Replace("ICD not found.", "").Replace("ICD not found", "");
                    }
                }

                if (exc != null && !item.InsuranceName.IsNull() && !item.PrimaryInsuredID.IsNull())
                {
                    exc.InsuranceName = item.InsuranceName;
                    exc.PrimaryInsuredID = item.PrimaryInsuredID;
                }

                _context.ExternalCharge.Update(exc);
            }

            _context.SaveChanges();


            DataMigrationController dataMigration = new DataMigrationController(_context, _contextMain, null);
            dataMigration.ControllerContext = this.ControllerContext;

            ListModel model = new ListModel();
            model.Ids = new long[ExchargeM.ToList().Count];
            int counter = 0;
            foreach (var item in ExchargeM)
            {
                model.Ids[counter] = item.ExternalChargeID;
                counter++;
            }
            await dataMigration.ProcessCharges(model);

            return Json(ExchargeM);
        }

        [HttpPost]
        [Route("AddPatientCollabrateMD")]
        public async Task<ActionResult<IEnumerable<ExternalPatient>>> AddPatientCollabrateMD()
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;

            return AddPatientCollabrateMD(PracticeId, temp);
        }
        private List<ExternalPatient> AddPatientCollabrateMD(long PracticeId, string temp)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            string connectionstring = CommonUtil.GetConnectionString(UD.PracticeID, temp);
            List<InsurancePlan> InsurancePlanData = _context.InsurancePlan.ToList();
            List<Patient> PatientData = _context.Patient.Where(x => x.PracticeID == UD.PracticeID).ToList();
            List<ExternalPatient> data = new List<ExternalPatient>();

            try
            {
                using (SqlConnection myconnection = new SqlConnection(connectionstring))
                {
                    string ostring = "select PLAST, PFIRST, PSEX, PBDATE, PCITY, PSTATE, PZIPCODE, PHOMEPH, PEMAIL, PADDR1, ACCTTYPE, [B_PATIENT].DELETED, PCELLPH, PACCTNO, pripayer.Payor as PrimaryInsurance, secpayer.Payor as SecondaryInsurance from [B_PATIENT]" +
                    " left join [B_PAYOR] as pripayer on [B_PATIENT].INSSEQ1 = pripayer.SEQNO " +
                    "left join [B_PAYOR] as secpayer on [B_PATIENT].INSSEQ2 = secpayer.SEQNO";

                    SqlCommand ocmd = new SqlCommand(ostring, myconnection);
                    ocmd.CommandTimeout = 12000;
                    myconnection.Open();

                    using (SqlDataReader oreader = ocmd.ExecuteReader())
                    {
                        while (oreader.Read())
                        {
                            try
                            {
                                string PLAST = oreader["PLAST"].ToString();
                                string PFIRST = oreader["PFIRST"].ToString();
                                bool isDOBNull = false;
                                DateTime? PBDATE = null;
                                if (oreader["PBDATE"] is DBNull)
                                {
                                    isDOBNull = true;

                                }
                                else
                                {
                                    PBDATE = Convert.ToDateTime(oreader["PBDATE"]);
                                }
                                string PSEX = oreader["PSEX"].ToString().Equals("0") ? "M" : "F";
                                string PCITY = oreader["PCITY"].ToString();
                                string PSTATE = oreader["PSTATE"].ToString();
                                string PZIPCODE = oreader["PZIPCODE"].ToString().Replace("-", "").Replace(" ", "");
                                string PHOMEPH = oreader["PHOMEPH"].ToString().Replace("-", "").Replace("(", "").Replace("-", ")").Replace(" ", "");
                                string PEMAIL = oreader["PEMAIL"].ToString();
                                string PADDR1 = oreader["PADDR1"].ToString();
                                //bool DELETED = oreader["DELETED"].ToString().Equals("0") ? true : false;
                                string ACCTTYPE = oreader["ACCTTYPE"].ToString();
                                string PCELLPH = oreader["PCELLPH"].ToString();
                                string PrimaryInsurance = oreader["PrimaryInsurance"].ToString();
                                string SecondaryInsurance = oreader["SecondaryInsurance"].ToString();
                                string ExternalPatient = oreader["PACCTNO"].ToString();
                                if (PLAST == null)
                                    PLAST = "";

                                if (PFIRST == null)
                                    PFIRST = "";
                                if (PSEX == null)
                                    PSEX = "";
                                if (PCITY == null)
                                    PCITY = "";
                                if (PSTATE == null)
                                    PSTATE = "";
                                if (PZIPCODE == null)
                                    PZIPCODE = "";
                                if (PHOMEPH == null)
                                    PHOMEPH = "";
                                if (PEMAIL == null)
                                    PEMAIL = "";
                                if (PADDR1 == null)
                                    PADDR1 = "";
                                if (ACCTTYPE == null)
                                    ACCTTYPE = "";
                                if (PCELLPH == null)
                                    PCELLPH = "";
                                if (PrimaryInsurance == null)
                                    PrimaryInsurance = "";
                                if (SecondaryInsurance == null)
                                    SecondaryInsurance = "";
                                if (PSTATE.Length > 2)
                                {
                                    PSTATE = "";
                                }
                                if (PCITY.Length > 20)
                                {
                                    PCITY = "";
                                }
                                ExternalPatient EPatient = null;
                                if (isDOBNull)
                                {
                                    EPatient = new ExternalPatient()
                                    {
                                        ExternalPatientID = ExternalPatient,
                                        LastName = PLAST,
                                        FirstName = PFIRST,
                                        Gender = PSEX,
                                        City = PCITY,
                                        State = PSTATE,
                                        ZipCode = PZIPCODE,
                                        PhoneNumber = PHOMEPH,
                                        Email = PEMAIL,
                                        Address1 = PADDR1,
                                        IsDeleted = false,
                                        AccountType = ACCTTYPE,
                                        MobileNumber = PCELLPH,
                                        // AccountNum = oreader["PACCTNO"].ToString(),
                                        PrimaryInsurance = PrimaryInsurance,
                                        PrimaryInsuredName = PLAST.Trim() + ", " + PFIRST.Trim(),
                                        // PrimaryInsuredID = oreader["SEQNO"].ToString(),
                                        SecondaryInsurance = SecondaryInsurance,
                                        SecondaryInsuredName = PLAST.Trim() + ", " + PFIRST.Trim(),
                                        PracticeID = UD.PracticeID,
                                        ProviderID = 1,
                                        LocationId = 1,
                                        AddedBy = UD.Email.ToUpper(),
                                        AddedDate = DateTime.Now,
                                        MergeStatus = "E",
                                        FileName = "DAT File",
                                        IsActive = true,
                                        //  AccountNum = _context.GetNextSequenceValue("S_PATIENTACCOUNTNUMBER")
                                    };
                                }
                                else
                                {
                                    EPatient = new ExternalPatient()
                                    {
                                        ExternalPatientID = ExternalPatient,
                                        LastName = PLAST,
                                        FirstName = PFIRST,
                                        Gender = PSEX,
                                        DOB = PBDATE,
                                        City = PCITY,
                                        State = PSTATE,
                                        ZipCode = PZIPCODE,
                                        PhoneNumber = PHOMEPH,
                                        Email = PEMAIL,
                                        Address1 = PADDR1,
                                        IsDeleted = false,
                                        AccountType = ACCTTYPE,
                                        MobileNumber = PCELLPH,
                                        // AccountNum = oreader["PACCTNO"].ToString(),
                                        PrimaryInsurance = PrimaryInsurance,
                                        PrimaryInsuredName = PLAST.Trim() + ", " + PFIRST.Trim(),
                                        // PrimaryInsuredID = oreader["SEQNO"].ToString(),
                                        SecondaryInsurance = SecondaryInsurance,
                                        SecondaryInsuredName = PLAST.Trim() + ", " + PFIRST.Trim(),
                                        PracticeID = UD.PracticeID,
                                        ProviderID = 1,
                                        LocationId = 1,
                                        AddedBy = UD.Email.ToUpper(),
                                        AddedDate = DateTime.Now,
                                        MergeStatus = "E",
                                        FileName = "DAT File",
                                        IsActive = true,
                                        // AccountNum = _context.GetNextSequenceValue("S_PATIENTACCOUNTNUMBER")
                                    };
                                }

                                if (!EPatient.FirstName.IsNull() && !EPatient.LastName.IsNull() && !EPatient.Address1.IsNull())
                                {


                                    if (data.Where(lamb => lamb.FirstName.Trim().Replace(" ", "").Equals(EPatient.FirstName.Trim().Replace(" ", "")) &&
                                    lamb.LastName.Trim().Replace(" ", "").Equals(EPatient.LastName.Trim().Replace(" ", "")) &&
                                    lamb.Address1.Trim().Replace(" ", "").Equals(EPatient.Address1.Trim().Replace(" ", ""))
                                    ).Count() != 0)
                                    {
                                        EPatient.MergeStatus = "D";
                                        data.Add(EPatient);
                                        _context.ExternalPatient.Add(EPatient);
                                    }
                                    else
                                    {
                                        data.Add(EPatient);
                                        _context.ExternalPatient.Add(EPatient);
                                    }
                                }
                                else
                                {
                                    //string str = null;
                                    //if (EPatient.FirstName == null)
                                    //{

                                    //    str = "FirstName,";
                                    //    EPatient.MissingInfo = EPatient.MissingInfo + str;
                                    //}
                                    //if (EPatient.LastName == null)
                                    //{
                                    //    str = "LastName,";
                                    //    EPatient.MissingInfo = EPatient.MissingInfo + str;
                                    //}
                                    //if (EPatient.Gender == null)
                                    //{
                                    //    str = "Gender,";
                                    //    EPatient.MissingInfo = EPatient.MissingInfo + str;
                                    //}
                                    //if (EPatient.State == null)
                                    //{
                                    //    str = "State,";
                                    //    EPatient.MissingInfo = EPatient.MissingInfo + str;
                                    //}
                                    //if (EPatient.DOB == null)
                                    //{
                                    //    str = "DOB,";
                                    //    EPatient.MissingInfo = EPatient.MissingInfo + str;
                                    //}
                                    //if (EPatient.ZipCode == null)
                                    //{
                                    //    str = "ZipCode,";
                                    //    EPatient.MissingInfo = EPatient.MissingInfo + str;
                                    //}
                                    //if (EPatient.Address1 == null)
                                    //{
                                    //    str = "Address1,";
                                    //    EPatient.MissingInfo = EPatient.MissingInfo + str;
                                    //}
                                    //if (EPatient.City == null)
                                    //{
                                    //    str = "City,";
                                    //    EPatient.MissingInfo = EPatient.MissingInfo + str;
                                    //}

                                    //if (EPatient.MissingInfo != null)
                                    //{
                                    //    EPatient.MissingInfo = EPatient.MissingInfo.TrimEnd(',');
                                    //    if (EPatient.MissingInfo.Contains(","))
                                    //    {
                                    //        EPatient.MissingInfo = EPatient.MissingInfo + " are missing";
                                    //    }
                                    //    else
                                    //    {
                                    //        EPatient.MissingInfo = EPatient.MissingInfo + " is missing";
                                    //    }
                                    //}
                                    data.Add(EPatient);
                                    _context.ExternalPatient.Add(EPatient);
                                }

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                        }
                        _context.SaveChanges();
                        myconnection.Close();
                    }
                }

                List<string> PatientIds = new List<string>();
                int Idex = 0;
                List<string> AddedExMappingEntries = new List<string>();
                foreach (ExternalPatient TempEP in data)
                {
                    Patient patient = new Patient();
                    try
                    {

                        if (TempEP.MergeStatus.Equals("E"))
                        {
                            patient.ExternalPatientID = TempEP.ExternalPatientID;
                            patient.LastName = TempEP.LastName.IsNull() ? "" : TempEP.LastName.ToUpper();
                            patient.FirstName = TempEP.FirstName.IsNull() ? "" : TempEP.FirstName.ToUpper();
                            patient.MiddleInitial = TempEP.MiddleInitial.IsNull() ? "" : TempEP.MiddleInitial.ToUpper();
                            patient.Address1 = TempEP.Address1.IsNull() ? "" : TempEP.Address1.ToUpper();
                            patient.City = TempEP.City.IsNull() ? "" : TempEP.City.ToUpper();
                            patient.State = TempEP.State.IsNull() ? "" : TempEP.State.ToUpper();
                            patient.ZipCode = TempEP.ZipCode;
                            patient.PhoneNumber = TempEP.PhoneNumber;
                            patient.Email = TempEP.Email.IsNull() ? "" : TempEP.Email.ToUpper();
                            patient.DOB = TempEP.DOB.HasValue ? TempEP.DOB : new DateTime();
                            patient.Gender = TempEP.Gender.IsNull() ? "" : TempEP.Gender.ToUpper();
                            patient.AccountNum = _context.GetNextSequenceValue("S_PATIENTACCOUNTNUMBER");
                            patient.AddedDate = DateTime.Now;
                            patient.AddedBy = TempEP.AddedBy.IsNull() ? "" : TempEP.AddedBy.ToUpper();
                            patient.IsActive = TempEP.IsActive;
                            patient.IsDeleted = TempEP.IsDeleted;
                            patient.PracticeID = TempEP.PracticeID;
                            patient.ProviderID = TempEP.ProviderID;
                            patient.LocationId = TempEP.LocationId;

                            TempEP.MergeStatus = "A";
                            _context.Patient.Add(patient);


                            PatientIds.Add(patient.ExternalPatientID + "|" + patient.AccountNum);
                            TempEP.AccountNum = patient.AccountNum;
                            _context.ExternalPatient.Update(TempEP);
                            List<PatientPlanWithID> PrimaryPatientPlanWithIDs = new List<PatientPlanWithID>();
                            List<PatientPlanWithID> SecondaryPatientPlanWithIDs = new List<PatientPlanWithID>();
                            Notes note = new Notes();
                            note.PracticeID = UD.PracticeID;
                            note.AddedBy = UD.Email;
                            note.AddedDate = DateTime.Now;
                            note.NotesDate = DateTime.Now;
                            //  note.PatientID = TempPatient.ID;
                            _context.Notes.Add(note);
                            if (!TempEP.PrimaryInsurance.IsNull())
                            {
                                PatientPlan TempPatientPlan = new PatientPlan();

                                TempPatientPlan.FirstName = patient.FirstName;
                                TempPatientPlan.LastName = patient.LastName;
                                TempPatientPlan.PatientID = patient.ID;
                                TempPatientPlan.DOB = patient.DOB;
                                TempPatientPlan.Gender = patient.Gender;
                                TempPatientPlan.Email = patient.Email;
                                TempPatientPlan.Address1 = patient.Address1;
                                TempPatientPlan.City = patient.City;
                                TempPatientPlan.State = patient.State;
                                TempPatientPlan.ZipCode = patient.ZipCode;
                                TempPatientPlan.PhoneNumber = patient.PhoneNumber;
                                //TempPatientPlan.SubscriberId = TempEP.PrimaryInsuredID;
                                TempPatientPlan.Coverage = "P";
                                TempPatientPlan.IsActive = true;
                                TempPatientPlan.IsDeleted = false;
                                TempPatientPlan.AddedDate = DateTime.Today.Date;
                                TempPatientPlan.AddedBy = UD == null ? "" : UD.Email.ToUpper();
                                string TempString = TempEP.PrimaryInsurance;
                                InsurancePlan PossibleCandidate = InsurancePlanData.Where(lamb => lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(TempEP.PrimaryInsurance.Trim().ToUpper().Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                if (PossibleCandidate != null)
                                {
                                    TempPatientPlan.InsurancePlanID = PossibleCandidate.ID;
                                }
                                else
                                {
                                    ExInsuranceMapping MappingRecord = _contextMain.ExInsuranceMapping.Where(lamb => lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(TempEP.PrimaryInsurance.Trim().ToUpper().Replace(" ", ""))).FirstOrDefault();
                                    if (MappingRecord != null)
                                    {
                                        TempPatientPlan.InsurancePlanID = MappingRecord.InsurancePlanID;
                                    }
                                    else
                                    {
                                        TempPatientPlan.InsurancePlanID = null;
                                    }
                                }

                                if (TempPatientPlan.InsurancePlanID != null)
                                {
                                    if (!InsurancePlanData.Select(ip => ip.ID).Contains(TempPatientPlan.InsurancePlanID.Value))
                                    {
                                        Debug.WriteLine("");
                                    }
                                    _context.PatientPlan.Add(TempPatientPlan);




                                    PatientPlanWithID TempPPWID = new PatientPlanWithID();
                                    TempPPWID.PatientPlan = TempPatientPlan;
                                    TempPPWID.AccountNumber = TempEP.ExternalPatientID;

                                    PrimaryPatientPlanWithIDs.Add(TempPPWID);




                                }
                                else
                                {
                                    TempEP.MissingInfo = TempEP.MissingInfo + ". Primary patient plan was not created";
                                    _context.Update(TempEP);
                                    if (_contextMain.ExInsuranceMapping.Where(exim => exim.ExternalInsuranceName.Equals(TempEP.PrimaryInsurance) && exim.InsurancePlanID == null).Count() == 0 && !AddedExMappingEntries.Contains(TempEP.PrimaryInsurance))
                                    {
                                        ExInsuranceMapping mapping = new ExInsuranceMapping();
                                        mapping.Status = "F";
                                        mapping.ExternalInsuranceName = TempEP.PrimaryInsurance;
                                        mapping.AddedBy = UD.Email;
                                        mapping.AddedDate = DateTime.Now;

                                        AddedExMappingEntries.Add(TempEP.PrimaryInsurance);
                                        _contextMain.ExInsuranceMapping.Add(mapping);

                                    }
                                }
                            }
                            if (!TempEP.SecondaryInsurance.IsNull())
                            {
                                PatientPlan TempPatientPlan = new PatientPlan();

                                TempPatientPlan.FirstName = patient.FirstName;
                                TempPatientPlan.LastName = patient.LastName;
                                TempPatientPlan.PatientID = patient.ID;
                                TempPatientPlan.DOB = patient.DOB;
                                TempPatientPlan.Gender = patient.Gender;
                                TempPatientPlan.Email = patient.Email;
                                TempPatientPlan.Address1 = patient.Address1;
                                TempPatientPlan.City = patient.City;
                                TempPatientPlan.State = patient.State;
                                TempPatientPlan.ZipCode = patient.ZipCode;
                                TempPatientPlan.PhoneNumber = patient.PhoneNumber;
                                //TempPatientPlan.SubscriberId = patient.SecondaryInsuredID;
                                TempPatientPlan.Coverage = "S";
                                TempPatientPlan.IsActive = true;
                                TempPatientPlan.IsDeleted = false;
                                TempPatientPlan.AddedDate = DateTime.Today.Date;
                                TempPatientPlan.AddedBy = UD == null ? "" : UD.Email.ToUpper();
                                //TempPatientPlan.RelationShip = 18 + "";
                                string chec = TempEP.SecondaryInsurance;
                                InsurancePlan PossibleCandidate = InsurancePlanData.Where(lamb => lamb.PlanName.Trim().ToUpper().Replace(" ", "").Equals(TempEP.SecondaryInsurance.Trim().ToUpper().Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                if (PossibleCandidate != null)
                                {
                                    TempPatientPlan.InsurancePlanID = PossibleCandidate.ID;
                                }
                                else
                                {
                                    ExInsuranceMapping MappingRecord = _contextMain.ExInsuranceMapping.Where(lamb => lamb.ExternalInsuranceName.Trim().ToUpper().Replace(" ", "").Equals(TempEP.SecondaryInsurance.Trim().ToUpper().Replace(" ", ""))).FirstOrDefault();
                                    if (MappingRecord != null)
                                    {
                                        TempPatientPlan.InsurancePlanID = MappingRecord.InsurancePlanID;
                                    }
                                    else
                                    {
                                        TempPatientPlan.InsurancePlanID = null;
                                    }
                                }

                                if (TempPatientPlan.InsurancePlanID != null)
                                {
                                    if (!InsurancePlanData.Select(ip => ip.ID).Contains(TempPatientPlan.InsurancePlanID.Value))
                                    {
                                        Debug.WriteLine("");
                                    }
                                    _context.PatientPlan.Add(TempPatientPlan);

                                    PatientPlanWithID TempPPWID = new PatientPlanWithID();
                                    TempPPWID.PatientPlan = TempPatientPlan;
                                    TempPPWID.AccountNumber = TempEP.ExternalPatientID;
                                    SecondaryPatientPlanWithIDs.Add(TempPPWID);

                                }
                                else
                                {
                                    TempEP.MissingInfo = TempEP.MissingInfo + ". Secondary patient plan was not created";
                                    _context.Update(TempEP);
                                    if (_contextMain.ExInsuranceMapping.Where(exim => exim.ExternalInsuranceName.Equals(TempEP.PrimaryInsurance)).Count() == 0 && !AddedExMappingEntries.Contains(TempEP.PrimaryInsurance))
                                    {
                                        ExInsuranceMapping mapping = new ExInsuranceMapping();
                                        mapping.Status = "F";
                                        mapping.ExternalInsuranceName = TempEP.PrimaryInsurance;
                                        mapping.AddedDate = DateTime.Now;
                                        mapping.AddedBy = UD.Email;
                                        AddedExMappingEntries.Add(TempEP.PrimaryInsurance);
                                        _contextMain.ExInsuranceMapping.Add(mapping);
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    finally
                    {
                        Idex++;
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return data;



        }




        public class ReturnObject
        {
            public string Result;
        }
        public class PatientPlanWithID
        {
            public string AccountNumber { get; set; }
            public PatientPlan PatientPlan { get; set; }
        }
        public class PatientPlanWithPatient
        {
            public ExternalPatient ExternalPatient { get; set; }
            public PatientPlan PatientPlan { get; set; }
            public ExternalCharge ExternalCharge { get; set; }
        }
        public class VisitWithPatientPayment
        {
            public Visit Visit { get; set; }
            public List<decimal> PatientPayments { get; set; }
            public PaymentCheck Check { get; set; }
        }

        public class VisitData
        {
            public Visit Visit { get; set; }
            public List<Charge> Charges { get; set; }
            public List<Cpt> Cpts { get; set; }

            public List<ExternalCharge> ExternalCharges { get; set; }
            public long GroupID { get; set; }
        }

        public class POSWithExtCh
        {
            public ExternalCharge ExCh { get; set; }
            public POS POS { get; set; }
        }
        public class ICDWithVisit
        {
            public Visit Visit { get; set; }
            public ICD ICD { get; set; }
        }
    }
}