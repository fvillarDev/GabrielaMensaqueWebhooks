using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Transactions;

namespace GabrielaMensaqueWebhooks.Models
{
    public class DataHelper
    {
        private const string ConnectionString = "Server=048cec32-7382-48c5-b195-a6c90016ac5c.sqlserver.sequelizer.com;Database=db048cec32738248c5b195a6c90016ac5c;User ID=yckohfksrmgmrdpo;Password=nRYXzsyKzcpduHRdxmuP2JYZwVcsqBZvjAepejMNPVio6vwhTmgCV5PKZ8ALqgu7;";

        private static SqlCommand DataCommand(string cmd, SqlConnection connection)
        {
            return new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 60,
                Connection = connection,
                CommandText = cmd
            };
        }

        public static void ExecuteNonQuery(string cmd, params object[] parameters)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    using (var connection = new SqlConnection(ConnectionString))
                    {
                        var sqlCommand = DataCommand(cmd, connection);
                        connection.Open();
                        SqlCommandBuilder.DeriveParameters(sqlCommand);
                        if (parameters != null)
                            if (parameters.Length > 0)
                            {
                                var index = 0;
                                foreach (SqlParameter parameter in sqlCommand.Parameters)
                                {
                                    if (parameter.Direction == ParameterDirection.Input ||
                                        parameter.Direction == ParameterDirection.
                                            InputOutput)
                                    {
                                        parameter.Value = parameters[index];
                                        index++;
                                    }
                                }
                            }
                        sqlCommand.ExecuteNonQuery();
                        connection.Close();
                    }
                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                //LogHelper.Log(ex);
            }
            catch (Exception ex)
            {

            }
        }
    }
}