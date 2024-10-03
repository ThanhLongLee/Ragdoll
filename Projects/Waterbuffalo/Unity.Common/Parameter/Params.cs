using System;
using System.Data;
using System.Data.SqlClient;

namespace Unity.Common.Parameter
{
    public class Params
    {
        public static SqlParameter[] Create(ParamItem[] arrParams)
        {
            SqlParameter[] sqlParams = null;

            if (arrParams.Length > 0)
            {
                Array.Resize(ref sqlParams, arrParams.Length);

                for (int i = 0; i < arrParams.Length; i++)
                {
                    SqlParameter param = new SqlParameter();

                    param.ParameterName = arrParams[i].ParamName;
                    param.SqlDbType = arrParams[i].ParamType;

                    if (arrParams[i].ParamDirection != ParameterDirection.Input)
                        param.Direction = arrParams[i].ParamDirection;
                    if (arrParams[i].ParamDirection == ParameterDirection.Input || arrParams[i].ParamDirection == ParameterDirection.InputOutput)
                        param.Value = arrParams[i].ParamValue;

                    if (!String.IsNullOrEmpty(arrParams[i].ParamTypeName))
                        param.TypeName = arrParams[i].ParamTypeName;

                    sqlParams[i] = param;
                }
            }

            return sqlParams;
        }
    }
}
