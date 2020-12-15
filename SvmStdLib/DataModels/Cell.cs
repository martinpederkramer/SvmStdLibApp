using System;
namespace SvmStdLib.DataModels
{
    public class Cell
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public void Print()
        {
            Console.WriteLine($"{Id}: {Name}, {Description}");
        }
    }
}
