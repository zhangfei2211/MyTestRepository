using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Utlis
{
    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LogLevel
    {
        Error,
        Debug,
        Warning,
        Info
    }
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        InfoLog,
        ErrorLog,
        DebugLog,
        OtherLog
    }
    /// <summary>
    /// 单例模式初始化
    /// </summary>
    public class Singleton
    {
        private ILog Log;
        private static Singleton instance;
        private Singleton() { }
        public static Singleton getInstance()
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
        /// <summary>
        /// 获取日志初始化器
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        public ILog Init(LogType logType)
        {
            string s = logType.ToString();
            Log = LogManager.GetLogger(s);
            return Log;
        }
    }
    /// <summary>
    /// 日志操作类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 输出Erro日志
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        public static void Error(string message,Exception ex =null)
        {
            WriteLog(LogType.ErrorLog, LogLevel.Error, message, ex);
        }

        /// <summary>
        /// 输出Info日志
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            //记录日志
            WriteLog(LogType.InfoLog, LogLevel.Info, message);
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        private static void WriteLog(LogType logType, LogLevel logLevel, string message, Exception ex = null)
        {
            ILog Log = Singleton.getInstance().Init(logType);
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Log.Debug(message);
                    break;
                case LogLevel.Error:
                    Log.Error(message, ex);
                    break;
                case LogLevel.Info:
                    Log.Info(message);
                    break;
                case LogLevel.Warning:
                    Log.Warn(message);
                    break;
            }

        }
    }
}