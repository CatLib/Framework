
using CatLib.API.IO;

namespace CatLib.API.INI
{

    public interface IINILoader
    {

        IINIResult Load(IFile file);
    }

}