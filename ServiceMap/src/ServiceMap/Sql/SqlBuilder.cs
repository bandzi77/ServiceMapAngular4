using ServiceMap.Models.Service_Tnt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMap.Sql
{
    public static class SqlBuilder
    {
        public static SqlParameter[] GetServicesTnt(ServiceFilter filter, PageInfo page)
        {
            var result = new List<SqlParameter>();

            result.Add(GetSqlParameter(filter.PostCode?.Trim(), "postCode", SqlDbType.NVarChar,6));
            result.Add(GetSqlParameter(filter.CityName?.Trim(), "town", SqlDbType.NVarChar, 50));
            result.Add(GetSqlParameter(page.OrderBy?.Trim(), "order_by", SqlDbType.NVarChar, 128));
            result.Add(GetSqlParameter((page.CurrentPage * page.PageSize ), "start", SqlDbType.Int));
            result.Add(GetSqlParameter(page.PageSize, "limit", SqlDbType.Int));

            return result.ToArray();
        }

        private static SqlParameter GetSqlParameter<T>(T data, string paramName, SqlDbType dbType, int size = 0)
        {
            SqlParameter result;

            if (data == null)
            {
                result = new SqlParameter(paramName, DBNull.Value);
            }
            else
            {
                if (size == 0)
                {
                    result = new SqlParameter(paramName, dbType);
                }
                else
                {
                    result = new SqlParameter(paramName, dbType, size);
                }
                result.Value = data;
            }

            return result;
        }
    }
}
