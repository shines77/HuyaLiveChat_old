
using System;
using System.Diagnostics;
using System.Text;

namespace HuyaLive
{
    public enum LoggerType
    {
        Debug,
        Console,
        ConsoleOut
    }

    public interface Loggerable
    {
        void Print(string message);
        void Print(string format, params object[] args);
        void Write(string message);
        void Write(string format, params object[] args);
        void Write(Exception ex);
        void WriteLine(string message);
        void WriteLine(string format, params object[] args);
        void WriteLine(Exception ex);
        void Flush();
        void Close();
    }

    public class Debugger : Loggerable
    {
        public Debugger() : base()
        {
        }

        public void Close()
        {
            Debug.Close();
        }

        public void Flush()
        {
            Debug.Flush();
        }

        public void Print(string message)
        {
            Debug.Print(message);
        }

        public void Print(string format, params object[] args)
        {
            Debug.Print(format, args);
        }

        public void Write(string message)
        {
            Debug.Write(message);
        }

        public void Write(string format, params object[] args)
        {
            Debug.Print(format, args);
        }

        public void Write(Exception ex)
        {
            Debug.Write(ex.ToString());
        }

        public void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            Debug.WriteLine(format, args);
        }

        public void WriteLine(Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    public class Consoler : Loggerable
    {
        public Consoler() : base()
        {
        }

        public void Close()
        {
            // Not implement
        }

        public void Flush()
        {
            // Not implement
        }

        public void Print(string message)
        {
            Console.Write(message);
        }

        public void Print(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public void Write(Exception ex)
        {
            Console.Write(ex.ToString());
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void WriteLine(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public class ConsoleOut : Loggerable
    {
        public ConsoleOut() : base()
        {
        }

        public void Close()
        {
            Console.Out.Close();
        }

        public void Flush()
        {
            Console.Out.Flush();
        }

        public void Print(string message)
        {
            Console.Out.Write(message);
        }

        public void Print(string format, params object[] args)
        {
            Console.Out.Write(format, args);
        }

        public void Write(string message)
        {
            Console.Out.Write(message);
        }

        public void Write(string format, params object[] args)
        {
            Console.Out.Write(format, args);
        }

        public void Write(Exception ex)
        {
            Console.Out.Write(ex.ToString());
        }

        public void WriteLine(string message)
        {
            Console.Out.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.Out.WriteLine(format, args);
        }

        public void WriteLine(Exception ex)
        {
            Console.Out.WriteLine(ex.ToString());
        }
    }

    public class Logger : Loggerable
    {
        private Loggerable logger = null;

        public Logger(Loggerable logger)
        {
            SetInstance(logger);
        }

        public void SetInstance(Loggerable logger)
        {
            this.logger = logger;
        }

        public void Print(string message)
        {
            if (logger != null)
            {
                logger.Print(message);
            }
        }

        public void Print(string format, params object[] args)
        {
            if (logger != null)
            {
                logger.Print(format, args);
            }
        }

        public void Write(string message)
        {
            if (logger != null)
            {
                logger.Write(message);
            }
        }

        public void Write(string format, params object[] args)
        {
            if (logger != null)
            {
                logger.Write(format, args);
            }
        }

        public void Write(Exception ex)
        {
            if (logger != null)
            {
                logger.Write(ex.ToString());
            }
        }

        public void WriteLine(string message)
        {
            if (logger != null)
            {
                logger.WriteLine(message);
            }
        }

        public void WriteLine(string format, params object[] args)
        {
            if (logger != null)
            {
                logger.WriteLine(format, args);
            }
        }

        public void WriteLine(Exception ex)
        {
            if (logger != null)
            {
                logger.WriteLine(ex.ToString());
            }
        }

        public void Enter(string message)
        {
            if (logger != null)
            {
                logger.WriteLine("-------------------------------------------");
                logger.WriteLine(message + " enter.");
            }
        }

        public void Leave(string message)
        {
            if (logger != null)
            {
                logger.WriteLine(message + " leave.");
                logger.WriteLine("-------------------------------------------");
            }
        }

        public void Flush()
        {
            logger.Flush();
        }

        public void Close()
        {
            logger.Close();
        }

        static public void Print(Loggerable logger, string message)
        {
            if (logger != null)
            {
                logger.Print(message);
            }
        }

        static public void Print(Loggerable logger, string format, params object[] args)
        {
            if (logger != null)
            {
                logger.Print(format, args);
            }
        }

        static public void Write(Loggerable logger, string message)
        {
            if (logger != null)
            {
                logger.Write(message);
            }
        }

        static public void Write(Loggerable logger, string format, params object[] args)
        {
            if (logger != null)
            {
                logger.Write(format, args);
            }
        }

        static public void Write(Loggerable logger, Exception ex)
        {
            if (logger != null)
            {
                logger.Write(ex.ToString());
            }
        }

        static public void WriteLine(Loggerable logger, string message)
        {
            if (logger != null)
            {
                logger.WriteLine(message);
            }
        }

        static public void WriteLine(Loggerable logger, string format, params object[] args)
        {
            if (logger != null)
            {
                logger.WriteLine(format, args);
            }
        }

        static public void WriteLine(Loggerable logger, Exception ex)
        {
            if (logger != null)
            {
                logger.WriteLine(ex.ToString());
            }
        }

        static public void Flush(Loggerable logger)
        {
            if (logger != null)
            {
                logger.Flush();
            }
        }

        static public void Close(Loggerable logger)
        {
            if (logger != null)
            {
                logger.Close();
            }
        }

        static public void Enter(Loggerable logger, string message)
        {
            if (logger != null)
            {
                logger.WriteLine("-------------------------------------");
                logger.WriteLine(message + " enter.");
            }
        }

        static public void Leave(Loggerable logger, string message)
        {
            if (logger != null)
            {
                logger.WriteLine(message + " leave.");
                logger.WriteLine("-------------------------------------");
            }
        }
    }
}
