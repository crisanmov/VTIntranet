using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTIntranet.intranetdb;
using VTIntranet.Models;
using VTIntranet.Repository;
using VTIntranet.Utils;

namespace VTIntranet.Services.Implements
{
    public class FormControlServices
    {
        private IntranetRepository unit = new IntranetRepository();

        public List<formSelectModel> SelectProfile()
        {
            try
            {
                return convertToSelectModel(unit.ProfilesRepository.Get(null, null, "").ToList());
            }
            catch (Exception e)
            {
                _Log.Error("Can not be processed the creation of the Select, FormControlServices.SelectProfile, Error ->", e);
                throw new Exception("Can not be processed the creation of the Select, #FormControlServices-SelectServicesProfiles, causes: " + e.Message);
            }
        }

        public List<formSelectModel> convertToSelectModel(List<tblprofiles> list)
        {

            List<formSelectModel> listToSend = new List<formSelectModel>();
            listToSend.Add(new formSelectModel().initialize_en());

            foreach (tblprofiles _tbl in list)
            {
                formSelectModel select = new formSelectModel();
                select.value = _tbl.idProfile.ToString();
                select.text = _tbl.profileName.ToString();
                listToSend.Add(select);
            }

            return listToSend;
        }

    }
}