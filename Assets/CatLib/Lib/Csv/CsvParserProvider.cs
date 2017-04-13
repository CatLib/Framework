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
    public sealed class CsvParserProvider : ServiceProvider
    {
        /// <summary>
        /// 注册Csv服务
        /// </summary>
        public override void Register()
        {
            App.Singleton<CsvParser>((app, param) =>
            {
                var rfcOptions = new Rfc4180Options();
                var options = new CsvParserOptions(new RFC4180Parser(rfcOptions));
                return new CsvParser(options);
            }).Alias<ICsvParser>().Alias("csv.parser");
        }
    }
}