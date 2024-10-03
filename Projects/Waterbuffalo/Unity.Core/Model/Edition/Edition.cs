using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Configuration;

namespace Unity.Core.Model.Edition
{
    public class Edition
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
        public int TotalRows { get; set; }
        public int RowNum { get; set; }


    }
}
