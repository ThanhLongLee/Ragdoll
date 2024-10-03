using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Configuration;

namespace Unity.Core.Model.Edition
{
    public class EditionTOC
    {
        public int Id { get; set; }
        public int EditionId { get; set; }
        public string Title { get; set; }
        public int Index { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public bool IsShowOnHeader { get; set; }
        public bool IsShowOnSummaryList { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }

    public class SortEditionTOCInexModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
    }
}
