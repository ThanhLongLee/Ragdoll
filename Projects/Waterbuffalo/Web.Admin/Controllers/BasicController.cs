using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using Unity.Core.Interface;
using Unity.Common.Configuration;
using Unity.Common.Extensions;
using Unity.Common.Helper;
using Microsoft.AspNet.Identity;
using Web.Admin.Infrastructure;
using Web.Admin.Infrastructure.Filters;
using Web.Admin.Models;

namespace Web.Admin.Controllers
{
    public class BasicController : Controller
    {
        public string TempPath = "Resources/Temp/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
        public string StrAddSuccess = "Đã thêm <b>{0}</b> thành công!";
        public string StrUpdateSuccess = "Đã cập nhật <b>{0}</b> thành công!";
        public string StrErrorCode = "Rất tiếc đã xảy ra lỗi, vui lòng thử lại sau.";
        public string StrErrorExcel = "Rất tiếc đã xảy ra lỗi, vui lòng thử lại sau.";
        public string StrSyncSuccess = "Sync order thành công.";

        private readonly IAppUserService _userService;

        public BasicController(IAppUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// Auto request keep alive session
        /// </summary>
        /// <returns>Json(Datetime.Now())</returns>
        [HttpPost]
        [Route("admin/keepAlive")]
        [ValidateAntiForgeryToken]
        public ActionResult KeepAlive()
        {
            if (Request.IsAjaxRequest())
            {
                if (Request.IsAuthenticated)
                {
                    return Json(new { result = true, isAuthen = true, dateTime = DateTime.Now.ToString("g") });
                }
                return Json(new { result = true, dateTime = DateTime.Now.ToString("g") });
            }
            return Json(new { result = false });
        }


        /// <summary>
        /// Tải file vào folder Temp để tạm lưu hình ảnh upload
        /// </summary>
        /// <param name="files">files: truyền từ plugin upload file</param>
        /// <returns>Danh sách tên hình</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/upload/file-temp")]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee })]
        public ActionResult UploadFileToFolderTemp(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new { status = false, error = true, message = "" });
                }

                if (files == null || !files.Any())
                {
                    return Json(new { status = true, listName = "", pathFolder = "" });
                }

                if (files.Any())
                {
                    List<string> pictureList = new List<string>();

                    // get path folder Temp
                    var strPathTemp = TempPath;

                    // get full path temp on Server
                    var pathTemp = Server.MapPath("~/" + strPathTemp);

                    foreach (var file in files)
                    {
                        if (file.IsImage())
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                if (!string.IsNullOrEmpty(file.FileName.Replace("\"", string.Empty)))
                                {
                                    // create folder with today
                                    Directory.CreateDirectory(pathTemp);

                                    // image name
                                    var imageName = Guid.NewGuid().ToString();

                                    // save image into folder Temp
                                    pictureList.Add(FileHelper.SaveImageByWeb(file, imageName, pathTemp, string.Empty, 1200));
                                }
                            }
                        }
                    }
                    return Json(new { status = true, listName = String.Join("|", pictureList), pathFolder = "/" + strPathTemp });
                }
                return Json(new { status = false, message = "Error: " });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error: " + ex });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/upload-multiple/file-temp")]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee })]
        public ActionResult UploadMultipleFileToFolderTemp(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new { status = false, error = true, message = "" });
                }

                if (files == null || !files.Any())
                {
                    return Json(new { status = true, listName = "", pathFolder = "" });
                }

                if (files.Any())
                {
                    var model = new List<MultipleImageUpload>();

                    // get path folder Temp
                    var strPathTemp = TempPath;

                    // get full path temp on Server
                    var pathTemp = Server.MapPath("~/" + strPathTemp);
                    foreach (var file in files)
                    {
                        if (file.IsImage())
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                if (!string.IsNullOrEmpty(file.FileName.Replace("\"", string.Empty)))
                                {
                                    // create folder with today
                                    Directory.CreateDirectory(pathTemp);

                                    // image name
                                    var imageName = Guid.NewGuid().ToString();

                                    var newFile = FileHelper.SaveImageByWeb(file, imageName, pathTemp, string.Empty, 1200);
                                    // save image into folder Temp
                                    model.Add(new MultipleImageUpload()
                                    {
                                        Id = newFile,
                                        Name = newFile,
                                        Path = "/" + strPathTemp
                                    });
                                }
                            }
                        }
                    }
                    return Json(new { status = true, listImage = model });
                }
                return Json(new { status = false, message = "Error: " });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error: " + ex });
            }
        }


      

        /// <summary>
        /// Tạo Session để phân quyền trên CKFinder
        /// </summary>
        /// <param name="isCreateSession">Tạo Session</param>
        /// <param name="folderSave"></param>
        public void CreateSessionCkFinder(bool isCreateSession, string folderSave)
        {
            if (Request.IsAuthenticated)
            {
                if (isCreateSession)
                {
                    var user = (System.Security.Claims.ClaimsIdentity)User.Identity;
                    Session["AccountRole"] = user.FindFirstValue("AccountRole") + AppSettings.Delimiter + folderSave;
                }
                else
                {
                    Session.Remove("AccountRole");
                }
            }
        }

        #region alert message

        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, "Thành công", "check", message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, "Thông tin", "info", message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, "Cảnh báo", "warning", message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, "Nguy hiểm", "danger", message, dismissable);
        }

        private void AddAlert(string alertStyle, string header, string icon, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Icon = icon,
                Header = header,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }

        #endregion

    }
}