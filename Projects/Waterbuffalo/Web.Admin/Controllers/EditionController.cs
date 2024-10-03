using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Unity.Common.Configuration;
using Unity.Common.Helper;
using Unity.Core.Interface;
using Unity.Core.Interface.Service;
using Web.Admin.Infrastructure.Filters;
using Web.Admin.Infrastructure.Helper;
using Web.Admin.Models;
using Web.Admin.Models.EditionModel;

namespace Web.Admin.Controllers
{
    public class EditionController : BasicController
    {
        private readonly IEditionService _editionService;
        private readonly IEditionTOCService _editionTOCService;
        private readonly IEditionSummaryService _editionSummaryService;

        public EditionController(IAppUserService userService, IEditionService editionService, IEditionTOCService editionTOCService, IEditionSummaryService editionSummaryService) : base(userService)
        {
            _editionService = editionService;
            _editionTOCService = editionTOCService;
            _editionSummaryService = editionSummaryService;
        }

        // GET: Edition
        public ActionResult List()
        {
            try
            {

                return View();

            }
            catch (Exception ex)
            {
                _editionService.WriteError("Error Admin in EditionController at List() Method", ex.Message);
                return RedirectToRoute("ErrorCode");// Error Page
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/employee/search")]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee }, Roles = "employee-view")]
        public async Task<JsonResult> Ajax_GetEditionList(int pageNo, string keyword, EditionStatus status)
        {
            var model = new List<EditionViewModel>();
            try
            {
                if (!Request.IsAjaxRequest())
                    return Json(new { status = false, message = StrErrorCode });

                var beginRow = Calculator.BeginRow(pageNo);
                var numRows = Calculator.NumRows(pageNo);

                var resultEdition = await _editionService.FindBy(keyword ?? "", status, beginRow, numRows);

                foreach (var item in resultEdition)
                {
                    var itemVM = new EditionViewModel();
                    itemVM.FromModel(item);
                    itemVM.TotalPage = Calculator.TotalPage(item.TotalRows);
                    model.Add(itemVM);
                }

                return Json(new { status = true, totalPage = model.Any() ? model.ElementAt(0).TotalPage : 0, view = RenderHelper.PartialView(this, @"~\Views\Edition\_List.cshtml", model) });
            }
            catch (Exception ex)
            {
                _editionService.WriteError("Error Admin in EditionController at Ajax_GetEditionList() Method", ex.Message);
                return Json(new { status = false, message = StrErrorCode });// Error Page   
            }
        }


    }
}