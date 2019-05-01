using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VTIntranet.intranetdb;
using VTIntranet.Models;
using VTIntranet.Models.tree;
using VTIntranet.Repository;
using System.Data.Entity;
using VTIntranet.Utils;
using System.IO;

namespace VTIntranet.Services
{
    public class IntranetService
    {

        readonly IntranetRepository _repository;
        public string serverURL = System.Configuration.ConfigurationManager.AppSettings["urlServer"];
        public IntranetService()
        {
            this._repository = new IntranetRepository();
        }

        //public List<TreeList> GenerarTree(List<tblintranet> modelList)
        //{

        //    var menuList = new List<TreeList>();
        //    foreach (var model in modelList)
        //    {
        //        //null indica que se trata de un nodo padre
        //        if (model.idPadre == 0)
        //        {
        //            menuList.Add(new TreeList(model.idIntranet,model.intranetDescription, model.intranetShortName, GenerarChildTree(modelList, model)));
        //        }
        //    }
        //    return menuList;
        //}

        //protected static List<TreeList> GenerarChildTree(List<tblintranet> modelList, tblintranet modelParent)
        //{
        //    var menuList = new List<TreeList>();
        //    foreach (var objeto in modelList)
        //    {
        //        //la igualdad indica que objeto es hijo de sObjeto
        //        if (objeto.idPadre == modelParent.idIntranet)
        //        {
        //            menuList.Add(new TreeList(objeto.idIntranet,objeto.intranetDescription, objeto.intranetShortName, GenerarChildTree(modelList, objeto)));
        //        }
        //    }
        //    return menuList;
        //}

        public List<tagsModelHelper> getTagsList(List<tbltags> model)
        {
            var menuList = model.Select(x => new tagsModelHelper { idTag = x.idTag, tagName = x.tagName, tagActive = x.tagActive }).ToList();

            return menuList;
        }

        public List<tagsModelHelper> getTagsListById(List<tbluserstags> model, int idUser)
        {
            var menuList = model.Where(x => x.idUser == idUser && x.userTagActive == true).
                Select(x => new tagsModelHelper
            {
                idTag = (int)x.idTag,
                tagName = x.tbltags.tagName,
                tagActive = x.tbltags.tagActive
            }).OrderBy(x => x.tagName).ToList();

            return menuList;
        }

        public tblattachments getAttachmentByIdAttachment(int idAttachment)
        {
            return this._repository.AttachmentRepository.Get(x => x.idAttachment == idAttachment, null, "").First();
        }

        public List<tblprofilestags> getProfileTags(List<tblprofilestags> model, int idProfile)
        {
            var lst = model.Where(x => x.idProfile == idProfile && x.profileTagActive == true).ToList();

            return lst;
        }

        public List<tbluserstags> getUserTags(List<tbluserstags> model, int idUser)
        {
            var lst = model.Where(x => x.idUser == idUser && x.userTagActive == true).ToList();

            return lst;
        }

        public List<attachmentsModelHelper> getAttachmentList(List<tblattachments> model, List<tbluserstags> protags)
        {
            List<attachmentsModelHelper> lst = new List<attachmentsModelHelper>();
            List<attachmentsModelHelper> lst2 = new List<attachmentsModelHelper>();
            List<attachmentsModelHelper> lstResultAll = new List<attachmentsModelHelper>();
            tblattachments attach = new tblattachments();
            var profile = protags.Select(x => x.idTag).ToList();


            foreach (var item in model)
            {
                var query = item.tblattachmentstags.Where(x => x.idAttachment == item.idAttachment).ToList();
                var i = item.tblattachmentstags.Select(x => x.idTag).ToList();

                List<bool> lbool = new List<bool>();
                int val = 0;
                for (int j = 0; j < i.Count; j++)
                {
                    var n = i[j];
                    var a = profile.Contains(n);
                    lbool.Add(a);
                    val = n;
                }
                bool _toAdd = lbool.Contains(false);

                if (_toAdd == false)
                {
                    attach = model.Where(x => x.idAttachment == item.idAttachment).FirstOrDefault();
                    //.Select(x => new attachmentsModelHelper { idAttachment = x.idAttachment, attachmentName = x.attachmentName, attachmentTagsName = getFileTags(x.tblattachmentstags), attachmentUrl = serverURL + x.attachmentUrl }).ToList();
                    var at = convertblAttachment(attach);
                    lst2.Add(at);

                }
            }

            return lst2.OrderBy(x => x.attachmentTagsName).Take(50).ToList();
        }

        public List<attachmentsModelHelper> getAttachmentSearch(List<tblattachmentstags> allData,string AttachmentID ,string name, List<VTIntranet.Models.Identity.RegisterViewModel> tags, List<tbluserstags> protags, DateTime? startDate, DateTime? endDate)
        {
            var q = allData;
            List<attachmentsModelHelper> lst = new List<attachmentsModelHelper>();
            List<tblattachmentstags> lstResult1 = new List<tblattachmentstags>();
            List<tblattachmentstags> lstResult2 = new List<tblattachmentstags>();
            List<tblattachmentstags> lstResult3 = new List<tblattachmentstags>();
            List<tblattachmentstags> lstResult1_2 = new List<tblattachmentstags>();
            List<tblattachmentstags> lstResult2_2 = new List<tblattachmentstags>();
            List<tblattachmentstags> lstResult3_2 = new List<tblattachmentstags>();
            List<tblattachmentstags> lstResult4 = new List<tblattachmentstags>();
            List<tblattachmentstags> lstResult4_2 = new List<tblattachmentstags>();
            List<tblattachmentstags> lstResult5 = new List<tblattachmentstags>();
            List<tblattachmentstags> lstResult5_2 = new List<tblattachmentstags>();
            tblattachments attach = new tblattachments();
            var profile = protags.Select(x => x.idTag).ToList();

            try
            {
                #region search
                #region search ID
                if (AttachmentID != "")
                {
                    lstResult5 = q.Where(x => x.tblattachments.idAttachment == int.Parse(AttachmentID)).ToList();
                    foreach (var item in lstResult5)
                    {
                        var query = item.tblattachments.tblattachmentstags.Where(x => x.idAttachment == item.idAttachment && x.attachmentTagsActive == true).ToList();
                        var i = query.Select(x => x.idTag).ToList();
                        List<bool> lbool = new List<bool>();
                        int val = 0;
                        for (int j = 0; j < i.Count; j++)
                        {
                            var n = i[j];
                            var a = profile.Contains(n);
                            lbool.Add(a);
                            val = n;
                        }
                        bool _toAdd;
                        _toAdd = lbool.Contains(false);
                        if (_toAdd == false)
                        {
                            lstResult5_2 = lstResult5.Where(x => x.tblattachments.idAttachment == item.idAttachment && x.attachmentTagsActive == true).ToList();
                            var l = lstResult5_2.Select(x => x.tblattachments).ToList();
                            attach = l.Where(x => x.idAttachment == item.idAttachment).FirstOrDefault();
                            var at = convertblAttachment(attach);
                            bool ls = lst.Exists(x => x.idAttachment == item.idAttachment);
                            if (ls == false)
                            {
                                lst.Add(at);
                            }
                        }
                        else
                        {
                        }
                    }
                }
                #endregion
                #region search text
                if (name != "")
                {
                    lstResult1 = q.Where(x => x.tblattachments.attachmentName.Contains(name)).ToList();

                    foreach (var item in lstResult1)
                    {
                        var query = item.tblattachments.tblattachmentstags.Where(x => x.idAttachment == item.idAttachment && x.attachmentTagsActive == true).ToList();
                        var i = query.Select(x => x.idTag).ToList();
                        List<bool> lbool = new List<bool>();
                        int val = 0;
                        for (int j = 0; j < i.Count; j++)
                        {
                            var n = i[j];
                            var a = profile.Contains(n);
                            lbool.Add(a);
                            val = n;
                        }
                        bool _toAdd;
                        _toAdd = lbool.Contains(false);
                        if (_toAdd == false)
                        {
                            lstResult1_2 = lstResult1.Where(x => x.tblattachments.idAttachment == item.idAttachment && x.attachmentTagsActive == true).ToList();
                            var l = lstResult1_2.Select(x => x.tblattachments).ToList();
                            attach = l.Where(x => x.idAttachment == item.idAttachment).FirstOrDefault();
                            var at = convertblAttachment(attach);
                            bool ls = lst.Exists(x => x.idAttachment == item.idAttachment);
                            if (ls == false)
                            {
                                lst.Add(at);
                            }
                        }
                        else
                        {
                        }
                    }
                }
                #endregion
                #region search tags
                if (tags.Count != 0)
                {
                    int[] _temp = tags.Select(y => y.idTag).ToArray();
                    lstResult2 = q.Where(x => _temp.Contains(x.idTag)).ToList();

                    foreach (var item in lstResult2)
                    {
                        var query = item.tblattachments.tblattachmentstags.Where(x => x.idAttachment == item.idAttachment && x.attachmentTagsActive == true).ToList();
                        var i = query.Select(x => x.idTag).ToList();
                        var appTags = tags.Select(x => x.idTag).ToList();
                        List<bool> lbool = new List<bool>();
                        int val = 0;
                        for (int j = 0; j < i.Count; j++)
                        {
                            var n = i[j];
                            var t = appTags.Contains(n);
                            var a = profile.Contains(n);
                            lbool.Add(a);
                            val = n;
                        }
                        bool _toAdd;
                        _toAdd = lbool.Contains(false);

                        if (_toAdd == false)
                        {
                            lstResult2_2 = lstResult2.Where(x => x.tblattachments.idAttachment == item.idAttachment && x.attachmentTagsActive == true).ToList();
                            var l = lstResult2_2.Select(x => x.tblattachments).ToList();
                            attach = l.Where(x => x.idAttachment == item.idAttachment).FirstOrDefault();
                            var at = convertblAttachment(attach);
                            bool ls = lst.Exists(x => x.idAttachment == item.idAttachment);
                            if (ls == false)
                            {
                                lst.Add(at);
                            }
                        }
                        else
                        {

                        }
                    }
                }
                #endregion
                #region search sin filtro
                if (AttachmentID == "" && name == "" && tags.Count == 0 && startDate == null)
                {
                    lstResult3 = q.ToList();

                    foreach (var item in lstResult3)
                    {
                        var query = item.tblattachments.tblattachmentstags.Where(x => x.idAttachment == item.idAttachment && x.attachmentTagsActive == true).ToList();
                        var i = query.Select(x => x.idTag).ToList();
                        List<bool> lbool = new List<bool>();
                        int val = 0;
                        for (int j = 0; j < i.Count; j++)
                        {
                            var n = i[j];
                            var a = profile.Contains(n);
                            lbool.Add(a);
                            val = n;
                        }
                        bool _toAdd;
                        _toAdd = lbool.Contains(false);
                        if (_toAdd == false)
                        {
                            lstResult3_2 = lstResult3.Where(x => x.tblattachments.idAttachment == item.idAttachment && x.attachmentTagsActive == true).ToList();
                            var l = lstResult3_2.Select(x => x.tblattachments).ToList();
                            attach = l.Where(x => x.idAttachment == item.idAttachment).FirstOrDefault();
                            var at = convertblAttachment(attach);

                            bool ls = lst.Exists(x => x.idAttachment == item.idAttachment);
                            if (ls == false)
                            {
                                lst.Add(at);
                            }
                        }
                        else
                        {
                        }
                    }
                }
                #endregion
                #region search by date
                if (startDate != null && endDate != null)
                {
                    //DateTime date = (DateTime)startDate;

                    //var endDate = date.AddDays(1);
                    lstResult4 = q.Where(x => x.tblattachments.attachmentDateLastChange >= startDate && x.tblattachments.attachmentDateLastChange <= endDate).ToList();

                    foreach (var item in lstResult4)
                    {
                        var query = item.tblattachments.tblattachmentstags.Where(x => x.idAttachment == item.idAttachment && x.attachmentTagsActive == true).ToList();
                        var i = query.Select(x => x.idTag).ToList();
                        List<bool> lbool = new List<bool>();
                        int val = 0;
                        for (int j = 0; j < i.Count; j++)
                        {
                            var n = i[j];
                            var a = profile.Contains(n);
                            lbool.Add(a);
                            val = n;
                        }
                        bool _toAdd;
                        _toAdd = lbool.Contains(false);
                        if (_toAdd == false)
                        {
                            lstResult4_2 = lstResult4.Where(x => x.tblattachments.idAttachment == item.idAttachment && x.attachmentTagsActive == true).ToList();
                            var l = lstResult4_2.Select(x => x.tblattachments).ToList();
                            attach = l.Where(x => x.idAttachment == item.idAttachment).FirstOrDefault();
                            var at = convertblAttachment(attach);
                            bool ls = lst.Exists(x => x.idAttachment == item.idAttachment);
                            if (ls == false)
                            {
                                lst.Add(at);
                            }
                        }
                        else
                        {

                        }
                    }
                }
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to query information, error ->" + ex.Message + "[Stacktrace] ||||>->");
            }

            return lst.OrderBy(x => x.attachmentTagsName).Take(50).ToList(); //ResultAll.Select(x => new attachmentsModelHelper { attachmentName = x.tblattachments.attachmentName, attachmentTagsName = getFileTags(x.tblattachments.tblattachmentstags), attachmentUrl = serverURL + x.tblattachments.attachmentUrl }).ToList();

        }
        public async Task<List<tbltags>> getTags()
        {
            return await _repository.getTags();

        }

        public async Task<List<tblprofilestags>> getProfileTags()
        {
            return await _repository.getProfileTags();

        }

        public async Task<List<tbluserstags>> getUsersTags()
        {
            return await _repository.getUsersTags();

        }

        public string getFileTags(ICollection<tblattachmentstags> model)
        {
            string tags = "";
            var count = model.Count;
            int i = 0;
            foreach (var item in model)
            {
                if (item.attachmentTagsActive == true)
                {
                    tags += i == count - 1 ? item.tbltags.tagName : item.tbltags.tagName + " , ";
                }
                i = i + 1;

            }
            return tags;
        }

        public async Task<List<tblattachments>> getAttachments()
        {
            return await _repository.getAttachments();

        }

        public async Task<List<tblattachmentstags>> getAttachmentsTags()
        {
            return await _repository.getAttachmentsTags();

        }

        public attachmentsModelHelper getAttachmentById(int id)
        {
            return convertblAttachment(this._repository.AttachmentRepository.Get(x => x.idAttachment == id, null, "").FirstOrDefault());
        }

        public attachmentsModelHelper convertblAttachment(tblattachments model)
        {
            HttpRequest oRequest = HttpContext.Current.Request;
            string host = oRequest.Url.Authority;
            string path = oRequest.ApplicationPath;

            attachmentsModelHelper helper = new attachmentsModelHelper();
            helper.idAttachment = model.idAttachment;
            helper.attachmentName = model.attachmentName;
            helper.attachmentTagsName = getFileTags(model.tblattachmentstags);
            helper.attachmentShortName = model.attachmentShortName;
            helper.attachmentContentType = model.attachmentContentType;
            helper.attachmentDirectory = model.attachmentDirectory;
            helper.attachmentUrl = model.attachmentUrl;
            helper.attachmentDateLastChange = DatetimeUtils.ParseDatetoString(model.attachmentDateLastChange);

            helper.Url = string.Concat("/utilsapp/downloadbinaryDocument/", model.idAttachment);

            return helper;
        }
        public List<tblusers> getUsers()
        {
            List<tblusers> lst = new List<tblusers>();
            lst = this._repository.UsersRepository.Get(null, null, "").ToList();

            return lst;
        }
        public List<usersTagsModelHelper> getUserTagsSearch(List<tblusers> user, string userName)
        {
            List<usersTagsModelHelper> result = new List<usersTagsModelHelper>();
            List<tblusers> userResult = new List<tblusers>();
            var query = user;

            if (userName != "")
            {
                userResult = query.Where(x => x.userNameReal.ToLower().Contains(userName)).ToList();
                if(userResult.Count == 0)
                    userResult = query.Where(x => x.userNameReal.ToUpper().Contains(userName)).ToList();
            }
            if (userResult.Count == 0)
            {
                userResult = query.Where(x => x.userFirstLastName.ToLower().Contains(userName)).ToList();
                if (userResult.Count == 0)
                    userResult = query.Where(x => x.userFirstLastName.ToUpper().Contains(userName)).ToList();
            }


            result = userResult.Select(x => new usersTagsModelHelper { idUser = x.idUser, userName = x.userNameReal + " " + x.userFirstLastName }).ToList();

            return result;
        }

        public usersTagsModelHelper getUserTagsById(int idUser)
        {
            usersTagsModelHelper result = new usersTagsModelHelper();
            var query = this._repository.UsersRepository.Get(x => x.idUser == idUser, null, "").ToList();

            result = query.Select(x => new usersTagsModelHelper
            {
                idUser = x.idUser,
                idProfile = x.tblusersprofiles.FirstOrDefault().idProfile,
                userName = x.userNameReal + " " + x.userFirstLastName
            }).First();

            var tag = query.Select(x => x.tblusersprofiles.FirstOrDefault().tblusers.tbluserstags.Where(y => y.userTagActive == true).Select(y => y.tbltags).ToList()).ToList();

            foreach (List<tbltags> t in tag)
            {
                foreach (var item in t)
                {
                    tbltags tas = new tbltags();
                    tas.idTag = item.idTag;
                    tas.tagName = item.tagName;
                    tas.tagActive = item.tagActive;
                    result.tags.Add(tas);
                }
            }

            return result;
        }

        public usersTagsModelHelper getUserTagsFilesById(int idUser)
        {
            try
            {
                usersTagsModelHelper result = new usersTagsModelHelper();
                var query = this._repository.UsersRepository.Get(x => x.idUser == idUser, null, "").ToList();
                var attachmentstags = this._repository.AttachmentsTagsRepository.Get(x => x.attachmentTagsActive == true, null, "").ToList();
                result = query.Select(x => new usersTagsModelHelper
                {
                    idUser = x.idUser,
                    userName = x.userNameReal + " " + x.userFirstLastName
                }).First();

                var profTag = query.Select(x => x.tblusersprofiles.FirstOrDefault().tblusers.tbluserstags.Where(y => y.userTagActive == true).Select(y => y.idTag).ToList()).ToList();

                List<int> profileTag = new List<int>();
                foreach (var item in profTag)
                {
                    foreach (var i in item)
                    { profileTag.Add((int)i); }
                }

                foreach (var element in attachmentstags.OrderBy(x => x.tblattachments.attachmentName))
                {
                    var lstTag = element.tblattachments.tblattachmentstags.Select(x => x.idTag).ToList();
                    List<bool> lbool = new List<bool>();
                    for (int i = 0; i < lstTag.Count; i++)
                    {
                        var n = lstTag[i];
                        var a = profileTag.Contains(n);
                        lbool.Add(a);
                    }
                    bool _toAdd = lbool.Contains(false);
                    if (_toAdd == false)
                    {
                        var attTag = attachmentstags.Where(x => x.tblattachments.idAttachment == element.idAttachment && x.attachmentTagsActive == true).ToList();
                        var lstattach = attTag.Select(x => x.tblattachments).OrderBy(x=> x.attachmentName).ToList();
                        var attachment = lstattach.Where(x => x.idAttachment == element.idAttachment).FirstOrDefault();

                        foreach (var latta in lstattach)
                        {
                            bool ls = result.attachModel.Exists(x => x.idAttachment == latta.idAttachment);
                            if (ls == false)
                            {
                                attachmentsModelHelper atta = new attachmentsModelHelper();
                                atta.idAttachment = latta.idAttachment;
                                string name = Path.GetFileName(latta.attachmentName);
                                atta.attachmentName = getName(name);
                                atta.attachmentUrl = latta.attachmentUrl;
                                atta.attachmentDateLastChange = DatetimeUtils.ParseDatetoString(latta.attachmentDateLastChange);
                                string extension = Path.GetExtension(latta.attachmentShortName);
                                atta.attachmentFileType = getTypeFile(extension);
                                foreach (var latttag in latta.tblattachmentstags)
                                {
                                    tagsModelHelper tas = new tagsModelHelper();
                                    tas.idAttachment = latttag.idAttachment;
                                    tas.idTag = latttag.tbltags.idTag;
                                    tas.tagName = latttag.tbltags.tagName;
                                    tas.tagActive = latttag.tbltags.tagActive;

                                    result.tagsModel.Add(tas);
                                }
                                result.attachModel.Add(atta);
                            }
                        }

                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception("Unable to query information, error ->" + ex.Message + "[Stacktrace] ||||>->");
            }

            
        }

        public string getTypeFile(string extension)
        {
            string fileType = "";

            switch (extension)
            {
                case ".pdf":
                    fileType = "PDF";
                    break;
                case ".xlsx":
                    fileType = "EXCEL";
                    break;
                case ".docx":
                    fileType = "WORD";
                    break;
                case ".pptx":
                    fileType = "POWER POINT";
                    break;
                case ".txt":
                    fileType = "BLOC DE NOTAS";
                    break;
                case ".xls":
                    fileType = "EXCEL";
                    break;
                case ".doc":
                    fileType = "WORD";
                    break;
                case ".png":
                    fileType = "IMAGEN PNG";
                    break;
                case ".jpg":
                    fileType = "IMAGEN JPG";
                    break;
                case ".jpeg":
                    fileType = "IMAGEN JPEG";
                    break;
                default:
                    fileType = "OTHERS";
                    break;
            }

            return fileType;
        }

        public string getName(string name)
        {
            return name.Replace(".xlsx", "").Replace(".docx","").Replace(".pptx", "").Replace(".pdf", "").Replace(".txt", "").Replace(".xls", "").Replace(".doc", "").Replace(".ppt", "").Replace(".jpg", "").Replace(".png", "").Replace(".jpeg", "");
        }

    }
}