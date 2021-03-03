using System;
using NLog;

namespace hobbie.Utilis
{
    public class Log
    {

        private static Log log = new Log();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static Log()
        {

        }

        public static Log getInstance() => log;

        public void info(string message, params object[] objs)
        {
            logger.Info(message.combine(objs));
        }

        public void debug(string message, params object[] objs)
        {
            logger.Debug(message.combine(objs));
        }

        public void error(string message, Exception ex = null, params object[] objs)
        {
            logger.Error(message.combine(objs) + $" . Ex - {(ex == null ? "" : ex.ToString())}");
        }
    }
}
