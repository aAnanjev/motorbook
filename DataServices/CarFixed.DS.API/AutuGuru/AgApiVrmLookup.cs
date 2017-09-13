using System;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;

using CarFixed.DS.DM;

namespace CarFixed.DS.API.AutuGuru
{
    public class AgApiVrmLookup : AgApiLookupBase
    {
        public VrmResponse VrmLookup(string vrm)
        {
            VrmResponse vehicle = null;


            if (!String.IsNullOrEmpty(vrm))
            {
                HttpResponseMessage response = this.CallAPI(
                    ConfigurationManager.AppSettings["AutuGuruAPI_BaseURL"],
                    String.Format("api/vrmlookup?vrm={0}", vrm),
                    ConfigurationManager.AppSettings["AutuGuruAPI_ApiKey"],
                    ConfigurationManager.AppSettings["AutuGuruAPI_PrivateKey"]);

                if (response.IsSuccessStatusCode)
                {
                    if (!String.IsNullOrEmpty(response.Content.ReadAsStringAsync().Result))
                    {
                        dynamic data = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                        vehicle = new VrmResponse();
                        vehicle.Vrm = data.identifiers.vrm;
                        vehicle.Vin = data.identifiers.vin;
                        vehicle.GCodeId = data.identifiers.gcode.id;

                        vehicle.Make = data.enhanceddata.gcodedata.manufacturer.Value;
                        vehicle.Model = data.enhanceddata.gcodedata.model.Value;
                        vehicle.Year = data.dvladata.manufactureyear;
                        vehicle.Colour = data.dvladata.colour.current;
                        vehicle.FuelType = data.enhanceddata.gcodedata.fueltype.Value;
                        vehicle.EngineSize = data.enhanceddata.gcodedata.engine.size.litre;
                        vehicle.BHP = data.enhanceddata.gcodedata.engine.power.bhp;
                        vehicle.LastMot = new MotResponse();

                        vehicle.LastMot.RenewalDate = Convert.ToDateTime(data.mothistorydata.renewaldate.Value);

                        if (!String.IsNullOrEmpty(data.mothistorydata.lastmotresult.Value))
                        {
                            vehicle.LastMot.Date = Convert.ToDateTime(data.mothistorydata.lastmotdate.Value);
                            vehicle.LastMot.Result = data.mothistorydata.lastmotresult.Value;
                            vehicle.LastMot.OdometerReading = Convert.ToInt32(data.mothistorydata.lastmot.odometerreading.reading.Value);
                            vehicle.LastMot.OdomoterUnits = data.mothistorydata.lastmot.odometerreading.units.Value;

                            foreach (var advisory in data.mothistorydata.lastmot.advisories)
                                vehicle.LastMot.Advisories.Add(advisory.description.Value);

                            foreach (var failure in data.mothistorydata.lastmot.failures)
                                vehicle.LastMot.Fails.Add(failure.description.Value);
                        }

                        vehicle.TaxRenewalDate = DateTime.Today; /*Convert.ToDateTime(data.dvladata.roadfundlicense.renewaldate);*/
                    }
                }
            }

            return vehicle;
        }

        public VrmResponse VrmLookupLight(string vrm)
        {
            VrmResponse vehicle = null;


            if (!String.IsNullOrEmpty(vrm))
            {
                HttpResponseMessage response = this.CallAPI(
                    ConfigurationManager.AppSettings["AutuGuruAPI_BaseURL"],
                    String.Format("api/vrmlookup?vrm={0}", vrm),
                    ConfigurationManager.AppSettings["AutuGuruAPI_ApiKey_VrmLight"],
                    ConfigurationManager.AppSettings["AutuGuruAPI_PrivateKey_VrmLight"]);

                if (response.IsSuccessStatusCode)
                {
                    if (!String.IsNullOrEmpty(response.Content.ReadAsStringAsync().Result))
                    {
                        dynamic data = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                        vehicle = new VrmResponse();
                        vehicle.Vrm = data.identifiers.vrm;
                        vehicle.Vin = data.identifiers.vin;
                        vehicle.GCodeId = data.identifiers.gcode.id;

                        vehicle.Make = data.enhanceddata.gcodedata.manufacturer.Value;
                        vehicle.Model = data.enhanceddata.gcodedata.model.Value;
                        vehicle.Year = data.dvladata.manufactureyear;
                        vehicle.FuelType = data.enhanceddata.gcodedata.fueltype.Value;

                        vehicle.MidCodes.Add(data.identifiers.thirdparty.autodata.code.Value);

                        if (data.identifiers.thirdparty.autodata.alternatives != null)
                        {
                            foreach (var mid in data.identifiers.thirdparty.autodata.alternatives)
                            {
                                vehicle.MidCodes.Add(mid.Value);
                            }
                        }
                    }
                }
            }

            return vehicle;
        }
    }
}
