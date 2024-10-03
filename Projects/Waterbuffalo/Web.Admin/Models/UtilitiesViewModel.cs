using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Web.Admin.Models
{
    public class RecaptchaResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }

    public class MultipleImageUpload
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Rotate { get; set; }
        public int Index { get; set; }

    }
}