
namespace CatLib.API.DataTable{

    public interface IDataTableResult{

        string Get(string field);

        string this[string field]{ get; }

    }


}