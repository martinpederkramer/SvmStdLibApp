using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;
using SvmStdLib;
namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string output = $"Machine: {Environment.MachineName}";

            SqlAccess sqlAccess = new SqlAccess();
            //Cell cell = new Cell { Id = 3, Name = "Cell03" };
            //sqlAccess.Update<Cell>(cell);
            List<Cell> cells = sqlAccess.GetCells();

            foreach (var item in sqlAccess.GetCells())
            {
                item.Print();
            }

            Console.WriteLine(output);
        }
    }

    public class SqlAccess : IDataAccess
    {
        string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=DS;Integrated Security=True;Pooling=False";
        public void Update<T>(T dataModel)
        {
            Type type = typeof(T);
            var props = type.GetProperties();
            foreach (var item in props)
            {
                TypeCode tc = Type.GetTypeCode(item.PropertyType);
                string s = item.Name;
                Console.WriteLine(tc + " " + s);
            }
        }
        public List<Cell> GetCells()
        {
            List<Cell> output = new List<Cell>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM dbo.Cell";

                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Cell cell = new Cell();
                    cell.Id = reader.GetInt32(0);
                    cell.Name = reader.GetString(1);
                    output.Add(cell);
                }
                reader.Close();
            }
            return output;
        }
    }
    public interface IDataAccess
    {
        void Update<T>(T dataModel);
    }
    public class Cell
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public void Print()
        {
            Console.WriteLine($"{Id}: {Name}");
        }
    }
}
