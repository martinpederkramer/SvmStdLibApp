using System.Collections.Generic;
//namespace ConsoleTester
namespace SvmStdLib.DataAccess
{
    public interface IDataAccess
    {
        void Write<T>(T dataModel);
        List<T> Read<T>();
    }
}
