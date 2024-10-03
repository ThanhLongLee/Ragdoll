using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity.Common.Configuration;
using Unity.Common.Helper;
using Unity.Core.Model;
using Unity.Core.Model.Edition;

namespace Web.Admin.Models.EditionModel
{
    public class EditionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ThumbnailImg { get; set; }
        public string BannerImg { get; set; }
        public string TitleOnBanner { get; set; }
        public string SubTitleOnBanner { get; set; }
        public string IntroduceContent { get; set; }
        public string IntroduceThumbnail { get; set; }
        public EditionStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public double TotalPage { get; set; }

    }

    public static partial class MapperHelper
    {
        public static void FromModel(this EditionViewModel obj, Edition model)
        {
            obj.Id = model.Id;
            obj.Title = model.Title;
            obj.ThumbnailImg = model.ThumbnailImg.ToImageUrl(AppSettings.ThumbnailPath);
            obj.BannerImg = model.BannerImg.ToImageUrl(AppSettings.ThumbnailPath);
            obj.TitleOnBanner = model.TitleOnBanner;
            obj.IntroduceContent = model.IntroduceContent;
            obj.IntroduceThumbnail = model.IntroduceThumbnail.ToImageUrl(AppSettings.ThumbnailPath);
            obj.Status = model.Status;
            obj.ModifiedDate = model.ModifiedDate;
            obj.CreatedDate = model.CreatedDate;
        }
    }
}