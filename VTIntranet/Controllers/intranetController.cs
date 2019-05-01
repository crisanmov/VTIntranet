using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VTIntranet.intranetdb;
using VTIntranet.Models;
using VTIntranet.Models.Identity;
using VTIntranet.Services;
using VTIntranet.Services.Implements;
using VTIntranet.Utils;

namespace VTIntranet.Controllers
{
    public class intranetController : ControllerBase
    {
        private readonly IntranetService intranetService;
        private readonly AttachmentServices attachServices;
        private readonly AccountServices accountServices;

        public intranetController()
        {
            intranetService = new IntranetService();
            attachServices = new AttachmentServices();
            accountServices = new AccountServices();
        }

        // GET: intranet
        public ActionResult Index()
        {
            //tags
            TagHelper mt = new TagHelper();
            ViewBag.tags = mt.getTagProfile(1);
            return View();
        }

        public ActionResult SearchFiles()
        {
            return View();
        }

        public ActionResult Access()
        {
            return View();
        }

        public async Task<JsonResult> generateTagList()
        {
            try
            {
                List<tagsModelHelper> data = new List<tagsModelHelper>();
                int idProfile = this.accountServices.getProfile();
                int idUser = this.accountServices.getUserId();
                //if (idProfile.Equals(1)) {
                //    data = intranetService.getTagsList(await this.intranetService.getTags());
                //}
                data = intranetService.getTagsListById(await this.intranetService.getUsersTags(), idUser);
                //var data = intranetService.getTagsList(await this.intranetService.getTags());
                return Json(JsonSerialResponse.ResultSuccess(data, "Consulta realizada correctamente"), JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                _Log.Error("Unable to get the tag list, intranetController.generateTagList, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> getAttachments()
        {
            try
            {
                List<tbluserstags> lst = new List<tbluserstags>();
                int idProfile = this.accountServices.getProfile();
                int idUser = this.accountServices.getUserId();
                lst = intranetService.getUserTags(await this.intranetService.getUsersTags(), idUser);
                var data = intranetService.getAttachmentList(await this.intranetService.getAttachments(), lst);
                return Json(JsonSerialResponse.ResultSuccess(data, "Consulta realizada correctamente"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to get the Attachments list, intranetController.getAttachments, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> searchFiles(string idAttachment,string attachmentName, string attchtags, string uploadDateStart, string uploadDateEnd)
        {
            try
            {
                var resultAttachTags = JsonConvert.DeserializeObject<List<RegisterViewModel>>(attchtags).ToList();
                List<tbluserstags> lst = new List<tbluserstags>();
                DateTime? dateStart = DatetimeUtils.ParseStringToDate(uploadDateStart);
                DateTime? dateEnd = DatetimeUtils.ParseStringToDate(uploadDateEnd);
                int idProfile = this.accountServices.getProfile();
                int idUser = this.accountServices.getUserId();
                lst = intranetService.getUserTags(await this.intranetService.getUsersTags(), idUser);
                var data = intranetService.getAttachmentSearch(await this.intranetService.getAttachmentsTags(), idAttachment, attachmentName, resultAttachTags, lst, dateStart, dateEnd);
                return Json(JsonSerialResponse.ResultSuccess(data, "Consulta realizada correctamente"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to get the file list, intranetController.searchFiles, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AttachFilePropertieAjax(string attachtags)
        {
            //List<tblattachmentstags> tagsattach = new List<tblattachmentstags>();
            //RegisterViewModel model = new RegisterViewModel();

            var resultAttachTags = JsonConvert.DeserializeObject<List<RegisterViewModel>>(attachtags);


            if (!(Request.Files.Count >= 10 || Request.Files.Count == 0))
            {
                var data_ = new List<tblattachmentstags>();
                int idAttachSaved = 0;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i].FileName != "" && Request.Files[i].ContentLength <= 10485760) // Aprox 10mb
                    {

                        try
                        {
                            string directory = Server.MapPath("~/Attachments");
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            string url = getDirectory("Attachments");

                            //Getting pach
                            string extension = Path.GetExtension(Request.Files[i].FileName);

                            var patchVirtual = "~" + url;


                            // Gettig metadata File to sabe attahFile object
                            tblattachments att = this.attachServices.saveTblAttachment(Request.Files, i, url);
                            data_ = this.attachServices.getTblAttachmentTags(resultAttachTags, att);

                            string pathToSave = Path.Combine(Server.MapPath(patchVirtual),
                                                       Path.GetFileName(att.attachmentShortName));
                            Request.Files[i].SaveAs(pathToSave);

                            idAttachSaved = att.idAttachment;

                        }
                        catch (Exception e)
                        {
                            return Json(JsonSerialResponse.ResultError("An error occurred when uploading the file, intranetController.AttachFilePropertieAjax Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(JsonSerialResponse.ResultError("No selected file or exceded the maximum size of 10 mb"), JsonRequestBehavior.AllowGet);
                    }
                }
                var data = 0;//attachservices.getById(idAttachSaved);
                return Json(JsonSerialResponse.ResultSuccess(data, "Query done correctly"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonSerialResponse.ResultError("Only one file can be loaded at a time or No selected file"), JsonRequestBehavior.AllowGet);
            }
        }

        public string getDirectory(string directory)
        {
            string url = string.Empty;
            try
            {
                Guid idGuid = Guid.NewGuid();
                string patchPhisical;
                string patchVirtual;

                DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("~/" + directory));
                DirectoryInfo[] directories = dirInfo.GetDirectories();
                if (directories.ToList().Count > 0)
                {
                    int d = 0;
                    while (d < directories.Length)
                    {
                        int count = directories[d].EnumerateFiles().ToList().Count();
                        if (count <= 100)
                        {
                            url = "/" + directory + "/" + directories[d].Name;
                            break;
                        }
                        else
                        {
                            patchPhisical = Server.MapPath("~/" + directory + "/" + idGuid);
                            patchVirtual = "/" + directory + "/" + idGuid;
                            if (!Directory.Exists(patchPhisical))
                            {
                                Directory.CreateDirectory(patchPhisical);
                                url = patchVirtual;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    patchPhisical = Server.MapPath("~/" + directory + "/" + idGuid);
                    patchVirtual = "/" + directory + "/" + idGuid;
                    if (!Directory.Exists(patchPhisical))
                    {
                        Directory.CreateDirectory(patchPhisical);
                        url = patchVirtual;
                    }

                }

            }
            catch (Exception e)
            {
                _Log.Error("Unable to get folder url, intranetController.getDirectory, Error ->", e);
            }
            return url;
        }

        [AllowAnonymous]
        public JsonResult getTagsByidAttachment(int idAttachment)
        {
            try
            {
                var data = this.attachServices.getTagByidAttachment(idAttachment);
                return Json(JsonSerialResponse.ResultSuccess(data, "Consulta realizada correctamente"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to get the file info , intranetController.getTagsByidAttachment, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public JsonResult attachmenTagsSave(int idAttachment, List<tblattachmentstags> attachtags)
        {
            try
            {
                var data = this.attachServices.addAttachmentTags(idAttachment, attachtags);
                return Json(JsonSerialResponse.ResultSuccess(data, "Consulta realizada correctamente"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to save attachment , intranetController.attachmenTagsSave, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateAttachmentTags(int idAttachment)
        {
            try
            {
                var data = idAttachment;
                return Json(JsonSerialResponse.ResultSuccess(data, "Consulta realizada correctamente"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to update attachment , intranetController.UpdateAttachmentTags, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }
		
		[HttpPost]
        [AllowAnonymous]
        public JsonResult AutoComplete(string nameUser)
        {
            try
            {
                List<tblusers> lst = new List<tblusers>();
                lst = this.intranetService.getUsers();
                var data = this.intranetService.getUserTagsSearch(lst, nameUser);
                return Json(JsonSerialResponse.ResultSuccess(data, "Consulta realizada correctamente"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public JsonResult getInfoUserTagsById(int idUser)
        {
            try
            {
                var data = this.intranetService.getUserTagsById(idUser);
                return Json(JsonSerialResponse.ResultSuccess(data, "Consulta realizada correctamente"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public JsonResult getInfoUserTagsFilesById(int idUser)
        {
            try
            {
                var data = this.intranetService.getUserTagsFilesById(idUser);
                return Json(JsonSerialResponse.ResultSuccess(data, "Consulta realizada correctamente"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewFiles()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult GenerateFile(string idAttachment)
        {
            try
            {
                var attachement = this.intranetService.getAttachmentByIdAttachment(int.Parse(idAttachment));
                var exist = System.IO.File.Exists(Server.MapPath(attachement.attachmentUrl));
                if (!exist)
                {
                    FileStream fs = new FileStream(Server.MapPath(attachement.attachmentUrl), FileMode.Open, FileAccess.Read);
                    return Json(JsonSerialResponse.ResultSuccess(exist, "File not exist."), JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("ViewFiles", "intranet");
                }
                else
                {
                    FileStream fs = new FileStream(Server.MapPath(attachement.attachmentUrl), FileMode.Open, FileAccess.Read);
                    return Json(JsonSerialResponse.ResultSuccess(exist, "Consulta realizada correctamente"), JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(JsonSerialResponse.ResultError("Error ->" + ex.Message + "[Stack-trace]" + ex.StackTrace), JsonRequestBehavior.AllowGet);
            }
            //ViewBag._url = Server.MapPath(attachement.attachmentUrl);
            //return View();
        }

    }
}