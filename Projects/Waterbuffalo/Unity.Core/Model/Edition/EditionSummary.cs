﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Configuration;

namespace Unity.Core.Model.Edition
{
    public class EditionSummary
    {
        public int Id { get; set; }
        public int EditionTOCId { get; set; }
        public string ThumbnailImg { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
