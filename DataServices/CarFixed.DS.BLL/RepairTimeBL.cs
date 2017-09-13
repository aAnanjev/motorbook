using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using AutoGuru.DataProvider.Vehicle;

using System.Text;
using System.Threading.Tasks;

using AutoGuru.Assistant.ThirdParty.AutoData.Models;
using AutoGuru.Assistant.ThirdParty.AutoData;
using CarFixed.DS.DM;

namespace CarFixed.DS.BLL
{
    public interface IRepairTimeBL
    {
        List<AutoDataRepairTime> GetRepairs(List<string> midCodes, int gcodeId, List<BasicSubCategoryRepairRef> repairRefs);

    }

    public class RepairTimeBL : IRepairTimeBL
    {
        public AutoDataRepairLookup _RepairLookup = null;
        public RepairTimeBL()
        {
            _RepairLookup = new AutoDataRepairLookup();


        }

        public List<AutoDataRepairTime> GetRepairs(List<string> midCodes, int gcodeId, List<BasicSubCategoryRepairRef> repairRefs)
        {
            List<AutoDataRepairTimeOptionData> repairOptions = _RepairLookup.LookupRepairOptions(midCodes.ToArray()).ToList();

            DataTable dtRepairs = new DataTable();
            dtRepairs.Columns.Add("MidId");
            dtRepairs.Columns.Add("RepairID");
            dtRepairs.Columns.Add("VariantID", typeof(Int32));
            dtRepairs.Columns.Add("Variant");

            DataSet dsRepairsFiltered = null;

            AutoDataRepairTimeOptionData selectedOption = null;
            List<AutoDataRepairTime> relevantRepairs = new List<AutoDataRepairTime>();


            if (repairOptions.Count > 0)
            {
                foreach (AutoDataRepairTimeOptionData option in repairOptions)
                {
                    DataRow row = dtRepairs.NewRow();

                    row["MidId"] = option.MidCode;
                    row["RepairID"] = -1;
                    row["VariantID"] = option.repair_times_id;
                    row["Variant"] = option.repair_times_description;

                    dtRepairs.Rows.Add(row);
                }

                if (gcodeId > -1)
                {
                    dsRepairsFiltered = VehicleDataProvider.FilterAutoDataRepairs(gcodeId, dtRepairs);

                    if (dsRepairsFiltered.Tables.Count > 0 && dsRepairsFiltered.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dsRepairsFiltered.Tables[0].Rows)
                        {
                            string selectedMid = row["MidId"].ToString();
                            string selectedRepairId = row["RepairId"].ToString();

                            if (repairOptions.Exists(r => r.MidCode == selectedMid && r.repair_times_id == selectedRepairId))
                            {
                                selectedOption = repairOptions.Find(r => r.MidCode == selectedMid && r.repair_times_id == selectedRepairId);
                                break;
                            }

                        }
                    }
                }



                if (selectedOption == null)
                    selectedOption = repairOptions.OrderBy(r => r.sort_order).First();

                List<AutoDataRepairTimeGroup> adAllRepairs = _RepairLookup.LookupRepairs(selectedOption.MidCode, selectedOption.repair_times_id);

                foreach (AutoDataRepairTimeGroup adRepGroup in adAllRepairs)
                {
                    foreach(AutoDataRepairTimeSubGroup adChildGroup in adRepGroup.sub_group)
                    {
                        foreach (AutoDataRepairTime adReptime in adChildGroup.components)
                        {
                            if (repairRefs.Exists(r => r.RepairRef == adReptime.id))
                            {
                                relevantRepairs.Add(adReptime);
                            }
                        }
                        
                    }
                }
               
            }

            return relevantRepairs;
        }
    }
}
