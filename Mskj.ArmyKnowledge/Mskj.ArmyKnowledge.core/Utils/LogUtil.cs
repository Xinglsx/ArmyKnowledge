using System;
using NLog;

namespace Mskj.ArmyKnowledge.Core
{
    /// <summary>
    /// 日志功能类，NLog实现
    /// </summary>
    public static class LogUtil
    {
        #region Private Fields
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region WebLog
        /// <summary>
        /// 根据日志级别，将信息写入日志中
        /// </summary>
        /// <param name="obj">日志信息</param>
        /// <param name="isDebug">是否是Debug日志</param>
        public static void WebLog(object obj)
        {
            if (null == obj)
                return;
            Logger.Info(obj);
        }

        /// <summary>
        /// 将异常信息写入日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        public static void WebError(Exception ex)
        {
            if (null == ex)
                return;

            Logger.Info(ex, ex.Message);
            //Logger.Error(ex, ex.Message);
        }

        /// <summary>
        /// 将异常信息写入日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="flag">日志标签</param>
        public static void WebError(Exception ex,string logFlag)
        {
            if (null == ex)
                return;

            Logger.Info(ex, logFlag + ex.Message);
            //Logger.Error(ex, logFlag + ex.Message);
        }
        #endregion

    }
}
