using System;

namespace Unity.Core.Model
{
    public class AppUserLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public Guid UserId { get; set; }

    }
}
