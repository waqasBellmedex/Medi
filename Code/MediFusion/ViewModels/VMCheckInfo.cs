using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
using static MediFusionPM.ViewModels.VMPaymentCheck;

namespace MediFusionPM.ViewModels
{
    public class VMCheckInfo
    {

        // public List<VMPaymentCheck> PaymentCheck { get; set; }

        //public void GetCheckInfo(PMContext pMContext)
        // {

        //        var   PaymentCheck = (from pc in pMContext.PaymentCheck
        //                      join pv in pMContext.PaymentVisit on pc.ID equals pv.PaymentCheckID
        //                      join f in pMContext.Facility on pc.FacilityID equals f.ID
        //                     select new GPaymentCheck()
        //                     {
        //                         Id = pc.ID,
        //                         CheckNumber = pc.CheckNumber,
        //                         PaymentMethod = pc.PaymentMethod,
        //                         CheckDate = pc.CheckDate.ToString("MM/dd/yyyy")
        //                         CheckAmount = pc.CheckAmount,
        //                         Appliedamount = pv.BilledAmount,
        //                         PostedAmount = pv.PaidAmount,
        //                         NumberOfVisits = pc.NumberOfVisits,
        //                         NumberOfPatients = pc.NumberOfPatients,
        //                         Status = pv.StatusCode,
        //                         Payer = pc.PayeeName,
        //                         Facility = f.Name,



        //                     }).ToList();
        //     return PaymentCheck;

        // }


    }
}
