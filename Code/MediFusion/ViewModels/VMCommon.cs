using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
using MediFusionPM.Models.Main;
using MediFusionPM.ViewModel;

namespace MediFusionPM.ViewModels
{
    public class VMCommon
    {
        public static UserInfoData GetLoginUserInfo(MainContext contextMain, string Email, string Role = "")
        {
            UserInfoData data = new UserInfoData();
            var CurrentLoginUser = (from u in contextMain.Users
                                    where u.Email == Email
                                    select u
                                ).FirstOrDefault();
            data.UserID = CurrentLoginUser.Id;
            data.PracticeID = CurrentLoginUser.PracticeID.Value;
            data.ClientID = CurrentLoginUser.ClientID.Value;
            data.Role = Role;
            data.Email = Email;
            data.PracticeName = contextMain.MainPractice.Find(CurrentLoginUser.PracticeID.Value)?.OrganizationName;

            data.Rights = GetLoginUserRights(contextMain, data.UserID);
            return data;
        }

        public static MainRights GetLoginUserRights(MainContext contextMain, string Id)
        {
            MainRights data = new MainRights();
            data = (from u in contextMain.MainRights
                    where u.Id == Id
                    select u).FirstOrDefault();
            return data;
        }

        internal static UserInfoData GetLoginUserInfo(object contextMain, string value1, string value2)
        {
            throw new NotImplementedException();
        }

        public static CityStateZipData GetCityStateZipInfo(ClientDbContext pMContext, string zip)
        {
            CityStateZipData cityStateZipData = (from citystatetable in pMContext.CityStateZipData where citystatetable.Zip == zip select citystatetable).ToList().FirstOrDefault();
            return cityStateZipData;
        }

  


        public class UserInfoData
        {
            public string Email { get; set; }
            public string UserID { get; set; }
            public long PracticeID { get; set; }
            public long ClientID { get; set; }
            public string Role { get; set; }
            public MainRights Rights { get; set; }
            public string PracticeName { get; set; }


        }
        public class CGeneralItems
        { 
            public string Name { get; set; }
            public string Value { get; set; }
            public string Description { get; set; }
            public string Type { get; set; } 
            public int? position { get; set; }
             
        }
        //internal static UserInfoData GetLoginUserInfo(PMContext context, string value)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
