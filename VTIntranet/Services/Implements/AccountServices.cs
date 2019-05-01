using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using VTIntranet.intranetdb;
using VTIntranet.Models;
using VTIntranet.Models.Identity;
using VTIntranet.Repository;
using VTIntranet.Services.Security;
using VTIntranet.Utils;

namespace VTIntranet.Services.Implements
{
    public class AccountServices
    {
        IntranetRepository unit;
        private ManagerPasswordHash manager;
        private FormControlServices formservice;
        public AccountServices()
        {
            manager = new ManagerPasswordHash();
            formservice = new FormControlServices();
            unit = new IntranetRepository();
        }

        public ClaimsIdentity AddIdentityBasic(LoginViewModel model)
        {
            tblusers user = this.unit.UsersRepository.Get(x => x.userName == model.UserName, null, "tbluserspermissions,tblusersprofiles").FirstOrDefault();

            var identityClaims = new ClaimsIdentity(new[]
                                {
                                    new Claim(ClaimTypes.Name,model.UserName == null ? "-" : model.UserName),
                                    new Claim(ClaimTypes.NameIdentifier,user.idUser.ToString()),
                                    new Claim(ClaimTypes.Email,user.userEmail == null ? "-": user.userEmail),
                                    new Claim(ClaimTypes.Role, user.tblusersprofiles.FirstOrDefault().idProfile.ToString()),

                                    new Claim("Fullname", user.userNameReal == null ? "" : string.Concat(user.userNameReal , " ",user.userFirstLastName == null ? "-" : user.userFirstLastName)),
                                    new Claim("idProfile",user.tblusersprofiles.FirstOrDefault().idProfile.ToString()),
                                    new Claim("IdRelationship",user.idRelationship == null ? "0" : user.idRelationship.ToString())
                                }, DefaultAuthenticationTypes.ApplicationCookie);

            identityClaims.AddClaims(this.addSessionPermissions(user));


            return identityClaims;
        }

        public bool verifyUser(LoginViewModel model)
        {
            // List of permission VTH
            //List<long> permisisonsvth = new List<long>();

            // HashingPass
            string passwordHash = this.EncodePassword(model.Password);

            // verifig Hashes
            tblusers user = this.unit.UsersRepository.Get(x => x.userName == model.UserName, null, "tbluserspermissions").FirstOrDefault();
            if (user == null) { throw new Exception("User do not exists."); }
            if (user.userActive == false) { throw new Exception("Inactive user."); }



            return this.VerifyPassword(model.Password, user.userPasswordHash);

        }

        public string EncodePassword(string password)
        {
            return manager.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordhash)
        {
            return manager.VerifyHashedPassword(passwordhash, password);
        }

        private List<Claim> addSessionPermissions(tblusers model)
        {
            List<Claim> lc = new List<Claim>();
            var permissions = model.tbluserspermissions.Where(x => x.userPermissionActive == Constantes.activeRecord).OrderBy(y => y.tblpermissions.permissionController);
            foreach (tbluserspermissions table in permissions)
            {
                if (string.IsNullOrEmpty(table.tblpermissions.permissionController))
                {
                    lc.Add(new Claim("userpermission", table.tblpermissions.permissionAction));
                }
                else if (string.IsNullOrEmpty(table.tblpermissions.permissionAction))
                {
                    lc.Add(new Claim("userpermission", table.tblpermissions.permissionController));
                }
                else
                {
                    lc.Add(new Claim("userpermission", string.Concat(table.tblpermissions.permissionController, "-", table.tblpermissions.permissionAction)));
                }

            }

            return lc;

        }

        public int getUserId()
        {
            int id = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserId());
            return id;
        }

        public int getProfile()
        {
            IEnumerable<Claim> claims = ((ClaimsIdentity)HttpContext.Current.User.Identity).Claims;
            var profile = claims.Where(t => t.Type == "idProfile").FirstOrDefault();
            return profile == null ? 0 : Convert.ToInt32(profile.Value);
        }

        public List<usersModelHelper> getAllUser()
        {
            var result = unit.getUsers();

            return result.Select(x => new usersModelHelper
            {
                idUser = x.idUser,
                userName = x.userName,
                userCompledName = x.userNameReal == null ? "" : x.userNameReal + " " + x.userFirstLastName == null ? "" :x.userFirstLastName,
                userEmail = x.userEmail,
                userProfileType = x.tblusersprofiles.FirstOrDefault().tblprofiles.profileName,
                idProfile = x.tblusersprofiles.FirstOrDefault().tblprofiles.idProfile
            }).ToList();

        }

        public RegisterUserViewModel getInfoUser(int idUser)
        {
            RegisterUserViewModel helper = new RegisterUserViewModel();

            var user = unit.UsersRepository.Get(x => x.idUser == idUser, null, "").FirstOrDefault();

            helper.UserName = user.userName;
            helper.userEmail = user.userEmail;
            helper.UserNameReal = user.userNameReal;
            helper.userFirstLastName = user.userFirstLastName;
            helper.userSecondLastName = user.userSecondLastName;
            helper.idProfile = user.tblusersprofiles.FirstOrDefault().idProfile;
            helper.IdRelationship = user.idRelationship;
            helper.userTypeRelationship = (int)user.idRelationship;
            helper.userActive = user.userActive;

            return helper;
        }

        public int AddUser(RegisterUserViewModel helper, List<tbluserspermissions> permissions, List<tblprofilestags> profilestag)
        {
            try
            {
                #region update user
                if (helper.idUser != 0)
                {
                    var user = unit.UsersRepository.GetByID(helper.idUser);
                    var profile = unit.UsersProfilesRepository.GetByID(helper.idUser);
                    //var usersTags = unit.UsersTagsRepository.GetByID((int)helper.idUser);
                    var usersTags = unit.UsersTagsRepository.Get(x => x.idUser == helper.idUser, null, "").ToList();

                    user.userName = helper.UserName;
                    user.userFirstLastName = helper.userFirstLastName;
                    user.userSecondLastName = helper.userSecondLastName;
                    user.userEmail = helper.userEmail;
                    user.userActive = helper.userActive;
                    user.userNameReal = helper.UserNameReal;
                    user.idRelationship = helper.idProfile;//helper.IdRelationship;

                    this.unit.UsersRepository.Update(user);
                    this.unit.Save();

                    #region tbluserprofile
                    if (helper.idProfile != 0)
                    {
                        profile.idProfile = helper.idProfile;
                        this.unit.UsersProfilesRepository.Update(profile);
                        this.unit.Save();
                    }
                    #endregion
                    #region  tblpermissions
                    if (permissions != null)
                    {
                        var userper = user.tbluserspermissions.ToList();
                        var id = user.tbluserspermissions.Select(x => x.idPermission).ToList();

                        for (int i = 0; i < permissions.Count; i++)
                        {
                            var up = permissions.ToArray()[i];
                            if (id.Contains(up.idPermission))
                            {
                                var e = unit.UsersPermissionsRepository.Get(x => x.idPermission == up.idPermission && x.idUser == helper.idUser).First();
                                e.userPermissionActive = up.userPermissionActive;
                                this.unit.UsersPermissionsRepository.Update(e);
                                unit.Save();
                            }
                            else
                            {
                                if (up.userPermissionActive == true)
                                {
                                    up.idUser = helper.idUser;
                                    this.unit.UsersPermissionsRepository.Insert(up);
                                    unit.Save();
                                }
                            }
                        }
                    }
                    #endregion
                    #region tblprofiletags
                    //if (profilestag != null)
                    //{
                    //    var protag = profile.tblprofiles.tblprofilestags.ToList();
                    //    var id = profile.tblprofiles.tblprofilestags.Select(x => x.idTag).ToList();

                    //    for (int i = 0; i < profilestag.Count; i++)
                    //    {
                    //        var pt = profilestag.ToArray()[i];
                    //        if (id.Contains(pt.idTag))
                    //        {
                    //            var p = unit.ProfilesTagsRepository.Get(x => x.idTag == pt.idTag && x.idProfile == helper.idProfile).First();
                    //            p.profileTagActive = pt.profileTagActive;
                    //            this.unit.ProfilesTagsRepository.Update(p);
                    //            this.unit.Save();
                    //        }
                    //        else
                    //        {
                    //            if (pt.profileTagActive == true)
                    //            {
                    //                pt.idProfile = helper.idProfile;
                    //                this.unit.ProfilesTagsRepository.Insert(pt);
                    //                this.unit.Save();
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                    #region tblusertags
                    if (helper.idUser != 0 && profilestag != null)
                    {
                        //var usertag = usersTags.tblusers.tbluserstags.Where(x => x.idUser == helper.idUser).ToList();
                            var id = usersTags.Select(x => x.idTag).ToList();
                            for (int i = 0; i < profilestag.Count; i++)
                            {
                                var pt = profilestag.ToArray()[i];
                                if (id.Contains(pt.idTag))
                                {
                                    var ut = unit.UsersTagsRepository.Get(x => x.idTag == pt.idTag && x.idUser == helper.idUser).First();
                                    ut.userTagActive = pt.profileTagActive;
                                    this.unit.UsersTagsRepository.Update(ut);
                                    this.unit.Save();

                                }
                                else
                                {
                                    if (pt.profileTagActive == true)
                                    {
                                        tbluserstags ustag = new tbluserstags();
                                        ustag.idUser = helper.idUser;
                                        ustag.idTag = pt.idTag;
                                        ustag.userTagActive = true;
                                        this.unit.UsersTagsRepository.Insert(ustag);
                                        this.unit.Save();
                                    }
                                }
                            }
                        
                    }
                    #endregion

                    return helper.idUser;
                }
                #endregion
                #region insert user
                else
                {
                    tblusers model = new tblusers();
                    tblusersprofiles profile = new tblusersprofiles();
                    tbluserstags usertag = new tbluserstags();
                    if (helper.UserName != null && helper.Password != null)
                    {
                        model.userName = helper.UserName;
                        model.userFirstLastName = helper.userFirstLastName;
                        model.userSecondLastName = helper.userSecondLastName;
                        model.userEmail = helper.userEmail;
                        model.userPasswordHash = this.EncodePassword(helper.Password);
                        model.userNameReal = helper.UserNameReal;
                        model.idRelationship = helper.idProfile;//helper.userTypeRelationship;
                        model.userActive = helper.userActive;

                        this.unit.UsersRepository.Insert(model);

                        this.unit.Save();

                        #region tblusersprofiles
                        if (model.idUser != 0)
                        {
                            profile.idUser = model.idUser;
                            profile.idProfile = helper.idProfile;

                            this.unit.UsersProfilesRepository.Insert(profile);
                            this.unit.Save();
                        }
                        #endregion
                        #region tblpermissions
                        if (permissions != null)
                        {
                            foreach (var up in permissions)
                            {
                                if (up.userPermissionActive == true)
                                {
                                    up.idUser = model.idUser;
                                    this.unit.UsersPermissionsRepository.Insert(up);
                                    this.unit.Save();
                                }
                            }
                        }
                        #endregion
                        #region tblprofilestags
                        //if (profilestag != null)
                        //{
                        //    foreach (var protag in profilestag)
                        //    {
                        //        if (protag.profileTagActive == true)
                        //        {
                        //            protag.idProfile = helper.idProfile;
                        //            this.unit.ProfilesTagsRepository.Insert(protag);
                        //            this.unit.Save();


                        //        }
                        //    }

                        //}
                        #endregion
                        #region tblusersTags
                        if (model.idUser != 0 && profilestag != null)
                        {
                            foreach (var protag in profilestag)
                            {
                                if (protag.profileTagActive == true)
                                {
                                    usertag.idUser = model.idUser;
                                    usertag.idTag = protag.idTag;
                                    usertag.userTagActive = true;
                                    this.unit.UsersTagsRepository.Insert(usertag);
                                    this.unit.Save();
                                }
                            }
                        }
                        #endregion

                        return model.idUser;
                    }
                    else
                    {
                        return model.idUser = 0;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _Log.Error("Unable to add user information, AccountServices.AddUser, Error ->", ex);
                this.unit.Rollback();
                throw new Exception("Unable to add user information, error ->" + ex.Message + "[Stacktrace] ||||>->");
            }
        }

        public bool UpdatePassword(RegisterUserViewModel helper)
        {
            try
            {
                bool retorno = false;
                if (helper.idUser != 0)
                {
                    if (helper.idProfile.Equals(1))
                    {
                        if (helper.Password.Equals(helper.ConfirmPassword))
                        {
                            var user = unit.UsersRepository.GetByID(helper.idUser);

                            user.userPasswordHash = this.EncodePassword(helper.Password);

                            this.unit.UsersRepository.Update(user);

                            this.unit.Save();

                            return retorno = true;
                        }
                    }
                    else
                    {
                        if (helper.OldPassword != null && helper.Password != null && helper.ConfirmPassword != null)
                        {
                            var user = unit.UsersRepository.GetByID(helper.idUser);

                            retorno = this.VerifyPassword(helper.OldPassword, user.userPasswordHash);

                            if (retorno == true)
                            {
                                user.userPasswordHash = this.EncodePassword(helper.Password);

                                this.unit.UsersRepository.Update(user);

                                this.unit.Save();

                                return retorno = true;
                            }
                        }
                        else
                        {
                            return retorno;
                        }
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                _Log.Error("Unable to update password, AccountServices.UpdatePassword, Error -> ", ex);
                this.unit.Rollback();
                throw new Exception("Unable to update password, Error -> " + ex.Message);
            }
        }

        public List<formSelectModel> getPermissions()
        {
            var permission = unit.PermissionsRepository.Get(null, null, "").Select(x => new formSelectModel { value = x.idPermission.ToString(), text = x.permissionTitle, shortText = x.permissionDescription }).ToList();

            return permission;
        }

        public List<formSelectModel> getTags()
        {
            var permission = unit.TagsRepository.Get(null, null, "").Select(x => new formSelectModel { value = x.idTag.ToString(), text = x.tagName, shortText = x.tagDescription }).ToList();

            return permission;
        }

        public List<formSelectModel> getPermissionsByidUser(int idUser)
        {
            var permission = unit.UsersPermissionsRepository.Get(x => x.idUser == idUser, null, "").Select(x => new formSelectModel { userPermission = x.idUserPermission, valueInt = (int)x.idUser, value = x.idPermission.ToString(), userPermissionActive = (bool)x.userPermissionActive }).ToList();

            return permission;
        }

        public List<formSelectModel> getTagsByidProfile(int idProfile)
        {
            var permission = unit.ProfilesTagsRepository.Get(x => x.idProfile == idProfile, null, "").Select(x => new formSelectModel { profileTag = x.idProfileTag, valueInt = (int)x.idProfile, value = x.idTag.ToString(), profileTagActive = (bool)x.profileTagActive }).ToList();

            return permission;
        }

        public List<formSelectModel> getTagsByidUser(int idUser)
        {
            var permission = unit.UsersTagsRepository.Get(x => x.idUser == idUser, null, "").Select(x => new formSelectModel { userTag = x.idUserTag, valueInt = (int)x.idUser, value = x.idTag.ToString(), userTagActive = (bool)x.userTagActive }).ToList();

            return permission;
        }

    }
}