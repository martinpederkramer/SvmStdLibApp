using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
namespace SvmStdLib.DataAccess
{
    public class SqlAccess : IDataAccess
    {
        delegate object SqlColDel(SqlDataReader reader, int index);
        string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=DS;Integrated Security=True;Pooling=False";

        SqlColDel[] GenSqlColDels(PropertyInfo[] props)
        {
            SqlColDel[] sqlColDels = new SqlColDel[props.Length];
            for (int i = 0; i < props.Length; i++)
            {
                TypeCode tc = Type.GetTypeCode(props[i].PropertyType);
                switch (tc)
                {
                    case TypeCode.Boolean:
                        sqlColDels[i] = (reader, index) => reader.GetBoolean(index);
                        break;
                    case TypeCode.Byte:
                        sqlColDels[i] = (reader, index) => reader.GetByte(index);
                        break;
                    case TypeCode.Int32:
                        sqlColDels[i] = (reader, index) => reader.GetInt32(index);
                        break;
                    case TypeCode.Int64:
                        sqlColDels[i] = (reader, index) => reader.GetInt64(index);
                        break;
                    case TypeCode.Single:
                        sqlColDels[i] = (reader, index) => (float)reader.GetDouble(index);
                        break;
                    case TypeCode.Double:
                        sqlColDels[i] = (reader, index) => reader.GetDouble(index);
                        break;
                    case TypeCode.Decimal:
                        sqlColDels[i] = (reader, index) => reader.GetDecimal(index);
                        break;
                    case TypeCode.DateTime:
                        sqlColDels[i] = (reader, index) => reader.GetDateTime(index);
                        break;
                    case TypeCode.String:
                        sqlColDels[i] = (reader, index) => reader.GetString(index);
                        break;
                }
            }
            return sqlColDels;
        }

        public void Write<T>(T dataModel)
        {
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();
            SqlColDel[] sqlColDels = GenSqlColDels(props);
        }
        public List<T> Read<T>()
        {
            List<T> output = new List<T>();

            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();
            SqlColDel[] sqlColDels = GenSqlColDels(props);

            StringBuilder sql = new StringBuilder("SELECT ");
            int i;
            for (i = 0; i < props.Length - 1; i++)
            {
                sql.Append(props[i].Name);
                sql.Append(", ");
            }
            sql.Append(props[i].Name);
            sql.Append(" FROM dbo.");
            sql.Append(type.Name);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql.ToString(), connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    object obj = Activator.CreateInstance(type);
                    for (i = 0; i < sqlColDels.Length; i++)
                    {
                        if (!reader.IsDBNull(i))
                        {
                            props[i].SetValue(obj, sqlColDels[i].Invoke(reader, i));
                        }
                    }
                    output.Add((T)obj);
                }
                reader.Close();
            }
            return output;
        }
    }
}
