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

namespace CatLib.AutoUpdate
{
    /// <summary>
    /// 自动更新事件
    /// </summary>
    public sealed class AutoUpdateEvents
    {
        /// <summary>
        /// 当获取更新url失败时
        /// </summary>
        public static readonly string ON_GET_UPDATE_URL_FAILD = "autoupdate.get.update.url.faild";

        /// <summary>
        /// 当更新启动时
        /// </summary>
        public static readonly string ON_UPDATE_START = "autoupdate.update.start";

        /// <summary>
        /// 当拉取更新列表失败时
        /// </summary>
        public static readonly string ON_UPDATE_LIST_FAILED = "autoupdate.update.list.falid";

        /// <summary>
        /// 当扫描文件hash开始时
        /// </summary>
        public static readonly string ON_SCANNING_DISK_FILE_HASH_START = "autoupdate.disk.file.hash.start";

        /// <summary>
        /// 当扫描文件hash结束时
        /// </summary>
        public static readonly string ON_SCANNING_DISK_FILE_HASH_END = "autoupdate.disk.file.hash.end";

        /// <summary>
        /// 当删除旧的文件开始时
        /// </summary>
        public static readonly string ON_DELETE_DISK_OLD_FILE_START = "autoupdate.disk.delete.old.file.start";

        /// <summary>
        /// 当删除旧的文件时
        /// </summary>
        public static readonly string ON_DELETE_DISK_OLD_FIELD_ACTION = "autoupdate.disk.delete.old.file.action";

        /// <summary>
        /// 当删除旧的文件结束时
        /// </summary>
        public static readonly string ON_DELETE_DISK_OLD_FILE_END = "autoupdate.disk.delete.file.end";

        /// <summary>
        /// 当开始更新时
        /// </summary>
        public static readonly string ON_UPDATE_FILE_START = "autoupdate.update.file.start";

        /// <summary>
        /// 当文件更新时
        /// </summary>
        public static readonly string ON_UPDATE_FILE_ACTION = "autoupdate.update.file.action";

        /// <summary>
        /// 当所有文件更新完毕
        /// </summary>
        public static readonly string ON_UPDATE_FILE_END = "autoupdate.update.file.end";

        /// <summary>
        /// 当文件更新失败时
        /// </summary>
        public static readonly string ON_UPDATE_FILE_FAILD = "autoupdate.update.file.faild";

        /// <summary>
        /// 当文件更新完成
        /// </summary>
        public static readonly string ON_UPDATE_COMPLETE = "autoupdate.uupdate.complete";
    }
}