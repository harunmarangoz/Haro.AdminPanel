using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Haro.AdminPanel.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Internal;

namespace Haro.AdminPanel.Business.Managers
{
    public class SqlProvider
    {
        private SqlCommand SqlCommand(string commandText)
        {
            var sqlConnection = new SqlConnection(App.ConnectionString);
            var sqlCommand = new SqlCommand(commandText, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            return sqlCommand;
        }


        public DataTable List(string query)
        {
            DataTable dataTable = new DataTable();
            var sqlCommand = SqlCommand(query);
            sqlCommand.Connection.Open();
            var sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            dataTable.Load(sqlDataReader);
            return dataTable;
        }

        public DataRow Get(string query)
        {
            DataTable dataTable = new DataTable();
            var sqlCommand = SqlCommand(query);
            sqlCommand.Connection.Open();
            var sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            dataTable.Load(sqlDataReader);
            return dataTable.Rows.Any() ? dataTable.Rows[0] : null;
        }
        public DataRow Get(string tableName, List<ArrayList> conditions = null)
        {
            DataTable dataTable = new DataTable();
            var sql = $"SELECT * FROM {tableName}";
            if (conditions != null || conditions!.Any())
            {
                var conditionStr = "";
                foreach (var item in conditions)
                {
                    conditionStr += " " + item[0] + " = @condition" + item[0] + " ,";
                    item[0] = "condition" + item[0];
                }

                conditionStr = conditionStr.Trim(',');
                conditionStr = conditionStr.Replace(",", "AND");
                sql += $" WHERE {conditionStr}";
            }
            var sqlCommand = SqlCommand(sql);
            foreach (ArrayList parameter in conditions)
            {
                sqlCommand.Parameters.AddWithValue(parameter[0]?.ToString(), parameter[1]?.ToString());
            }
            sqlCommand.Connection.Open();
            var sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            dataTable.Load(sqlDataReader);
            return dataTable.Rows.Any() ? dataTable.Rows[0] : null;
        }

        public long Save(string table, List<ArrayList> parameters)
        {
            if (parameters == null || !parameters.Any()) throw new Exception("Paremetreler boş olamaz");

            var colStr = "";
            var valStr = "";
            foreach (var item in parameters)
            {
                colStr += item[0] + ",";
                valStr += "@" + item[0] + ",";
            }

            colStr = colStr.Trim(',');
            valStr = valStr.Trim(',');
            var sql = $"INSERT INTO {table} ({colStr}) VALUES ({valStr}); SELECT SCOPE_IDENTITY()";
            var sqlCommand = SqlCommand(sql);
            foreach (ArrayList parameter in parameters)
            {
                sqlCommand.Parameters.AddWithValue(parameter[0]?.ToString(), parameter[1]?.ToString());
            }

            sqlCommand.Connection.Open();
            var modified = Convert.ToInt64(sqlCommand.ExecuteScalar());
            sqlCommand.Connection.Close();
            return modified;
        }

        public void Update(string table, List<ArrayList> values, List<ArrayList> conditions)
        {
            if (values == null || !values.Any()) throw new Exception("Paremetreler boş olamaz");

            var valStr = "";
            foreach (var item in values)
            {
                valStr += item[0] + " = @" + item[0] + ",";
            }

            valStr = valStr.Trim(',');
            var conditionStr = "";
            foreach (var item in conditions)
            {
                conditionStr += " " + item[0] + " = @condition" + item[0] + " ,";
                item[0] = "condition" + item[0];
            }

            conditionStr = conditionStr.Trim(',');
            conditionStr = conditionStr.Replace(",", "AND");
            var sql = $"UPDATE {table} SET {valStr} WHERE {conditionStr}";
            var sqlCommand = SqlCommand(sql);
            foreach (ArrayList parameter in values.Concat(conditions))
            {
                sqlCommand.Parameters.AddWithValue(parameter[0]?.ToString(), parameter[1]?.ToString());
            }

            sqlCommand.Connection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Connection.Close();
        }

        public void Delete(string table, List<ArrayList> conditions)
        {
            if (conditions == null || !conditions.Any()) throw new Exception("Paremetreler boş olamaz");
            var conditionStr = "";
            foreach (var item in conditions)
            {
                conditionStr += " " + item[0] + " = @condition" + item[0] + " ,";
                item[0] = "condition" + item[0];
            }

            conditionStr = conditionStr.Trim(',');
            conditionStr = conditionStr.Replace(",", "AND");
            var sql = $"DELETE FROM {table} WHERE {conditionStr}";
            var sqlCommand = SqlCommand(sql);
            foreach (ArrayList parameter in conditions)
            {
                sqlCommand.Parameters.AddWithValue(parameter[0]?.ToString(), parameter[1]?.ToString());
            }
            sqlCommand.Connection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Connection.Close();
        }
    }
}