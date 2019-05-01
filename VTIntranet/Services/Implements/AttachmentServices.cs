using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using VTIntranet.intranetdb;
using VTIntranet.Models;
using VTIntranet.Models.Identity;
using VTIntranet.Repository;
using VTIntranet.Utils;

namespace VTIntranet.Services.Implements
{
    public class AttachmentServices
    {
        private IntranetRepository unit;
        private AccountServices services;

        public AttachmentServices()
        {
            unit = new IntranetRepository();
            services = new AccountServices();
        }

        public tblattachments saveTblAttachment(HttpFileCollectionBase file, int indexFile,  string url)
        {
            try
            {
                FileInfo fi = new FileInfo(file[indexFile].FileName);
                var fileNameLenght = file[indexFile].FileName.Length;
                var fileName = fileNameLenght >= 255 ? file[indexFile].FileName.Substring(0, fileNameLenght - 5) : file[indexFile].FileName;
                var fileExtension = fi.Extension;
                var fileContentType = file[indexFile].ContentType;
                var fileSize = file[indexFile].ContentLength;
                Guid g = Guid.NewGuid();

                
                string shortName = fileName.Contains(fileExtension)? string.Concat(StringUtils.CutLenghtString120Characters(StringUtils.ReplaceSpecialCharacters(string.Concat(DatetimeUtils.ParseDatetoStringSingle(DateTime.Now), "-", g, "-", fileName)))) : string.Concat(StringUtils.CutLenghtString120Characters(StringUtils.ReplaceSpecialCharacters(string.Concat(DatetimeUtils.ParseDatetoStringSingle(DateTime.Now), "-", g, "-", fileName))), fileExtension); 
                string name = fileName.Contains(fileExtension) ? StringUtils.CutLenghtString120Characters(StringUtils.ReplaceSpecialCharacters(fileName)) : string.Concat(StringUtils.CutLenghtString120Characters(StringUtils.ReplaceSpecialCharacters(fileName)), fileExtension); 
                string urlFile = url + "/" + shortName;
                string directory = "~" + url;
                tblattachments tblattachment = new tblattachments();

                tblattachment.attachmentName = name;
                tblattachment.attachmentShortName = shortName;
                tblattachment.attachmentDirectory = directory;
                tblattachment.attachmentUrl = urlFile;
                tblattachment.attachmentUserLastChange = services.getUserId();
                tblattachment.attachmentDateLastChange = DateTime.Now;
                tblattachment.attachmentContentType = fileContentType;
                tblattachment.attachmentActive = Constantes.activeRecord;

                this.Save(tblattachment);

                return tblattachment;
            }
            catch (Exception e)
            {
                _Log.Error("It is not possible to save the record of the file, AttachmentServices.saveTblAttachment, Error ->", e);
                throw new Exception("It is not possible to save the record of the file, AttachmentServices.saveTblAttachment : " + e.Message + "[ ---- Starck Trace] ----" + e.StackTrace);
            }
        }

        public List<tblattachmentstags> getTblAttachmentTags(List<RegisterViewModel> helper, tblattachments attachment)
        {
            try
            {
                
                List<tblattachmentstags> lst = new List<tblattachmentstags>();

                if (helper != null)
                {
                    foreach (var item in helper)
                    {
                        tblattachmentstags attachtags = new tblattachmentstags();
                        attachtags.idAttachment = attachment.idAttachment;
                        attachtags.idTag = item.idTag;
                        attachtags.attachmentTagsActive = item.attachmentTagsActive;

                        if (item.attachmentTagsActive == true)
                        {
                            this.unit.AttachmentsTagsRepository.Insert(attachtags);
                            this.unit.Save();
                        }

                        lst.Add(attachtags);
                    }
                }

                return lst;
            }
            catch (Exception e)
            {
                _Log.Error("Unable to save the record, AttachmentServices.getTblAttachmentTags, Error ->", e);
                throw new Exception("Unable to save the record, AttachmentServices.getTblAttachmentTags: " + e.Message + "[ ---- Starck Trace] ----" + e.StackTrace);
            }
        }

        public tblattachments Save(tblattachments attach)
        {
            try
            {
                unit.AttachmentRepository.Insert(attach);
                unit.Save();
                return unit.AttachmentRepository.Get(x => x.idAttachment == attach.idAttachment, null, "").FirstOrDefault();
            }
            catch (Exception e)
            {
                _Log.Error("Unable to save the record of the file, AttachmentServices.Save, Error ->", e);
                unit.Rollback();
                throw new Exception("Unable to save the record of the file, AttachmentServices.Save: " + e.Message + "[ ---- Starck Trace] ----" + e.StackTrace);
            }
        }


        public List<formSelectModel> getTagByidAttachment(int idAttachment)
        {
            var tagsAttachments = unit.AttachmentsTagsRepository.Get(x => x.idAttachment == idAttachment, null, "").Select(x => new formSelectModel { attachmentTag = x.idAttachmentTag, valueInt = x.idAttachment, value = x.idTag.ToString(), attachmentTagActive = x.attachmentTagsActive }).ToList();

            return tagsAttachments;
        }

        public int addAttachmentTags(int idAttachment,List<tblattachmentstags> helper)
        {
            try
            {
                if(idAttachment != 0)
                {
                    var attach = unit.AttachmentRepository.GetByID(idAttachment);

                    if(helper != null)
                    {
                        var attachtags = attach.tblattachmentstags.ToList();
                        var id = attach.tblattachmentstags.Select(x => x.idTag).ToList();
                        for (int i = 0; i < helper.Count; i++)
                        {
                            var at = helper.ToArray()[i];
                            if (id.Contains(at.idTag))
                            {
                                var e = unit.AttachmentsTagsRepository.Get(x => x.idTag == at.idTag && x.idAttachment == idAttachment).First();
                                e.attachmentTagsActive = at.attachmentTagsActive;
                                this.unit.AttachmentsTagsRepository.Update(e);
                                unit.Save();
                            }
                            else
                            {
                                if(at.attachmentTagsActive == true)
                                {
                                    at.idAttachment = idAttachment;
                                    this.unit.AttachmentsTagsRepository.Insert(at);
                                    unit.Save();
                                }
                            }
                        }
                    }                   

                }
                return idAttachment;
            }
            catch (Exception ex)
            {
                _Log.Error("Unable to add attachment information, AttachmentServices.addAttachmentTags, Error ->", ex);
                this.unit.Rollback();
                throw new Exception("Unable to add attachment information, AttachmentServices.addAttachmentTags, error ->" + ex.Message + "[Stacktrace] ||||>->");
            }
        }


    }
}