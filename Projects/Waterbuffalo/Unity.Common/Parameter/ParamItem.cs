using System;
using System.Data;

namespace Unity.Common.Parameter
{
    public class ParamItem
    {
        public string ParamName { get; set; }
        public SqlDbType ParamType { get; set; }
        public object ParamValue { get; set; }
        public ParameterDirection ParamDirection { get; set; }
        public string ParamTypeName { get; set; }
        public int Size { get; set; }

        public ParamItem()
        {
            this.ParamName = string.Empty;
            this.ParamType = SqlDbType.VarChar;
            this.ParamValue = string.Empty;
            this.ParamDirection = ParameterDirection.Input;
        }

        public ParamItem(string paramName, SqlDbType paramType)
        {
            this.ParamName = paramName;
            this.ParamType = paramType;
            this.ParamValue = string.Empty;
            this.ParamDirection = ParameterDirection.Output;
        }

        public ParamItem(string paramName, SqlDbType paramType, object paramValue)
        {
            this.ParamName = paramName;
            this.ParamType = paramType;
            this.ParamValue = paramValue;
            this.ParamDirection = ParameterDirection.Input;
        }

        public ParamItem(string paramName, SqlDbType paramType, object paramValue, string paramTypeName)
        {
            this.ParamName = paramName;
            this.ParamType = paramType;
            this.ParamValue = paramValue;
            this.ParamDirection = ParameterDirection.Input;
            this.ParamTypeName = paramTypeName;
        }

        public ParamItem(string paramName, SqlDbType paramType, object paramValue, ParameterDirection paramDirection)
        {
            this.ParamName = paramName;
            this.ParamType = paramType;
            this.ParamValue = paramValue;
            this.ParamDirection = paramDirection;
        }

        public ParamItem(string paramName, SqlDbType paramType, object paramValue, int size = 0)
        {
            this.ParamName = paramName;
            this.ParamType = paramType;
            this.ParamValue = paramValue;
            this.Size = size;
            this.ParamDirection = ParameterDirection.Input;
        }
        public ParamItem(string paramName, SqlDbType paramType, object paramValue, int size = 0, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            this.ParamName = paramName;
            this.ParamType = paramType;
            this.ParamValue = paramValue;
            this.Size = size;
            this.ParamDirection = paramDirection;
        }
    }
}
