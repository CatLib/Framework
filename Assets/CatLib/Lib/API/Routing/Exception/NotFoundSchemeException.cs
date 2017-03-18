
namespace CatLib.API.Routing
{

    /// <summary>
    /// can not find scheme
    /// </summary>
    public class NotFoundSchemeException : CatLibException
    {
        public NotFoundSchemeException(string message) : base(message) { }
    }

}