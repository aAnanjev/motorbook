using System;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;

using CarFixed.DS.DM;

namespace CarFixed.DS.API.AutuGuru
{
    public class AgApiRepairLookup : AgApiLookupBase
    {
        public RepairResponse RepairLookup(string vrm, int componentId)
        {
            RepairResponse repair = null;

            if (!String.IsNullOrEmpty(vrm))
            {
                HttpResponseMessage response = this.CallAPI(
                    ConfigurationManager.AppSettings["AutuGuruAPI_BaseURL"],
                    String.Format("api/vehicle/{0}/component/{1}/part", vrm, componentId),
                    ConfigurationManager.AppSettings["AutuGuruAPI_ApiKey"],
                    ConfigurationManager.AppSettings["AutuGuruAPI_PrivateKey"]);

                if (response.IsSuccessStatusCode)
                {

                }
            }

           return repair;
        }
    }
}
