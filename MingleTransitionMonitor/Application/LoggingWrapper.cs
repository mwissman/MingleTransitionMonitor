using System;
using System.Diagnostics;
using log4net;

namespace MingleTransitionMonitor.Application
{
    public class LoggingWrapper
    {
        public static void Debug<T>(string message, params object[] arguments)
        {
            Log(() => LogManager.GetLogger(typeof(T)).DebugFormat(message, arguments));
        }


        public static void Error<T>(string message, Exception exception, params object[] arguments)
        {
            string formatedString = GetErrorMessage(message, arguments, 2);
            Log(() => LogManager.GetLogger(typeof(T)).Error(formatedString, exception));
        }

        private static string GetErrorMessage(string message, object[] arguments, int stackIndexOfCallingMethod)
        {
            string typeName = string.Empty;
            string methodName = string.Empty;
            try
            {
                StackTrace stackTrace = new StackTrace();
                typeName = stackTrace.GetFrame(stackIndexOfCallingMethod).GetMethod().Name;
                methodName = stackTrace.GetFrame(stackIndexOfCallingMethod).GetMethod().ReflectedType.FullName;
            }
            catch { }
            return string.Format("Type={0} Method={1} ", methodName, typeName) + string.Format(message, arguments);
        }

        private static void Log(Action logAction)
        {
            try
            {
                string user = string.Empty;

                var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                if (user == string.Empty && identity != null && !string.IsNullOrEmpty(identity.Name))
                {
                    user = identity.Name;
                }

                if (user == string.Empty)
                {
                    user = "(null)";
                }

                ThreadContext.Properties["user"] = user;
                logAction();
            }
            catch { }
        }
    }
}