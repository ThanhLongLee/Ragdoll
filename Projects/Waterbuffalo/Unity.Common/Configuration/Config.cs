using System;

namespace Unity.Common.Configuration
{
    #region Base

    public enum LanguageType : byte
    {
        Vietnamese = 0,
        English = 1,
        Korean = 2
    }

    public enum CurrencyType : byte
    {
        VND = 0,
        USD = 1,

        Undefined = 254,
    }

    public enum HostIndex : byte
    {
        CurrentHost = 0,
        HostIndex1 = 1,
        HostIndex2 = 2,

        Undefined = 254
    }

    public enum Status : byte
    {
        Activated = 0,
        Disabled = 250,

        Undefined = 254
    }

    #endregion

    #region Account
    public enum Gender : byte
    {
        Female = 0,
        Male = 1,
        Other = 2,
        Undefined = 254
    }

    public enum AccountType : byte
    {
        Application = 0,

        Undefined = 254
    }

    public enum AccountRole : byte
    {
        Employee = 180,
        Manage = 190,
        Admin = 200,

        Undefined = 254
    }

    public enum AccountStatus : byte
    {
        Active = 0,
        Locked = 1,

        Disabled = 250,

        Undefined = 254
    }
    #endregion

    public enum EditionStatus : byte
    {
        Activated = 0,

        Disabled = 250,

        Undefined = 254
    }
}