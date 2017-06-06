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

namespace CatLib.Routing
{
    /// <summary>
    /// Uri
    /// </summary>
    internal sealed class Uri
    {
        /// <summary>
        /// 原始uri
        /// </summary>
        private readonly System.Uri uri;

        /// <summary>
        /// 原始Uri信息
        /// </summary>
        public System.Uri Original
        {
            get
            {
                return uri;
            }
        }

        /// <summary>
        /// Uri
        /// </summary>
        /// <param name="uri">uri</param>
        public Uri(string uri)
        {
            this.uri = new System.Uri(uri);
        }

        /// <summary>
        /// Uri
        /// </summary>
        /// <param name="uri">uri</param>
        public Uri(System.Uri uri)
        {
            this.uri = uri;
        }

        /// <summary>
        /// 全路径(全路径不包含userinfo) eg: catlib://login/register?id=10
        /// </summary>
        public string FullPath
        {
            get
            {
                return System.Uri.UnescapeDataString(uri.Scheme + "://" + uri.Host + uri.PathAndQuery.TrimEnd('/'));
            }
        }

        /// <summary>
        /// 无参的全路径 eg:catlib://login/register
        /// </summary>
        public string NoParamFullPath
        {
            get
            {
                var index = FullPath.LastIndexOf('?');
                return index >= 0 ? FullPath.Substring(0, index).TrimEnd('/') : FullPath;
            }
        }

        /// <summary>
        /// 方案 eg: catlib
        /// </summary>
        public string Scheme
        {
            get
            {
                return uri.Scheme;
            }
        }

        /// <summary>
        /// host eg: login
        /// </summary>
        public string Host
        {
            get
            {
                return uri.Host;
            }
        }

        /// <summary>
        /// 请求中附带的用户信息
        /// </summary>
        public string UserInfo
        {
            get
            {
                return uri.UserInfo;
            }
        }

        /// <summary>
        /// 片段
        /// </summary>
        public string[] Segments
        {
            get
            {
                return uri.Segments;
            }
        }
    }
}