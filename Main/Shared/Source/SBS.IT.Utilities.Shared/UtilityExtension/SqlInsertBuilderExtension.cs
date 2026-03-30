using System.Text;
using System.Data;
using System.Data.SqlClient;
using System;

namespace SBS.IT.Utilities.Shared.UtilityExtension
{
    public static class SqlInsertBuilderExtension
    {
        public static string BuildAllFieldsSQL(DataTable table)
        {
            string sql = "";
            foreach (DataColumn column in table.Columns)
            {
                if (sql.Length > 0)
                    sql += ", ";
                sql += column.ColumnName;
            }
            return sql;
        }
        public static string BuildInsertSQL(DataTable table, string TableName)
        {
            StringBuilder sql = new StringBuilder("INSERT INTO " + TableName + " (");
            StringBuilder values = new StringBuilder("VALUES (");
            bool bFirst = true;
            bool bIdentity = false;
            string identityType = null;
            int index = 1;
            foreach (DataColumn column in table.Columns)
            {
                if (column.AutoIncrement)
                {
                    bIdentity = true;
                    switch (column.DataType.Name)
                    {
                        case "Int16":
                            identityType = "smallint";
                            break;
                        case "SByte":
                            identityType = "tinyint";
                            break;
                        case "Int64":
                            identityType = "bigint";
                            break;
                        case "Decimal":
                            identityType = "decimal";
                            break;
                        default:
                            identityType = "int";
                            break;
                    }
                }
                else
                {
                    if (bFirst)
                        bFirst = false;
                    else
                    {
                        sql.Append(", ");
                        values.Append(", ");
                    }
                    sql.Append("[" + column.ColumnName + "]");
                    values.Append("@");
                    values.Append(index.ToString());
                }
                index++;
            }
            sql.Append(") ");
            sql.Append(values.ToString());
            sql.Append(")");
            if (bIdentity)
            {
                sql.Append("; SELECT CAST(scope_identity() AS ");
                sql.Append(identityType);
                sql.Append(")");
            }
            return sql.ToString(); ;
        }
        public static void InsertParameter(SqlCommand command, string parameterName, string sourceColumn, object value)
        {
            SqlParameter parameter = new SqlParameter(parameterName, value);
            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = parameterName;
            parameter.SourceColumn = sourceColumn;
            parameter.SourceVersion = DataRowVersion.Current;
            command.Parameters.Add(parameter);
        }
        public static SqlCommand CreateInsertCommand(DataRow row, string TableName)
        {
            DataTable table = row.Table;
            string sql = BuildInsertSQL(table, TableName);
            SqlCommand command = new SqlCommand(sql);
            command.CommandType = CommandType.Text;
            int index = 1;
            foreach (DataColumn column in table.Columns)
            {
                if (!column.AutoIncrement)
                {
                    string parameterName = "@" + index.ToString();
                    InsertParameter(command, parameterName, column.ColumnName, row[column.ColumnName]);
                    index++;
                }
            }
            return command;
        }
    }
}
