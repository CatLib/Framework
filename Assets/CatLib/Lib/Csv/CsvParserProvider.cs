/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
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