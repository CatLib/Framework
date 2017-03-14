
using CatLib.API.Csv;

namespace CatLib.Csv
{

    /// <summary>
    /// Csv服务提供商
    /// </summary>
    public class CsvParserProvider : ServiceProvider
    {

        public override void Register()
        {

            RegisterParseOptions();
            App.Singleton<CsvParser>().Alias<ICsvParser>().Alias("csv.parser");

        }

        protected void RegisterParseOptions()
        {

            App.Singleton<CsvParserOptions>((app, param) => {

                RFC4180Options rfcOptions = new RFC4180Options();
                var options = new CsvParserOptions(new RFC4180Parser(rfcOptions));
                return options;

            }).Alias("csv.parser.options");

        }

    }

}