
using System.IO;

namespace CatLib.Csv
{

    public class RFC4180Parser : IStandard
    {

        private RFC4180Reader render;

        public RFC4180Parser(RFC4180Options options)
        {
            render = new RFC4180Reader(options);
        }

        public string[] Parse(string line)
        {
            using (var sr = new StringReader(line))
            {
                return render.Parse(sr);
            }
        }

    }

}