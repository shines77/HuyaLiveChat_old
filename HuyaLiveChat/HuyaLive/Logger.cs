
using System;
using System.Diagnostics;
using System.Text;

namespace HuyaLive
{
    public enum LogType
    {
        Debug,
        Console,
        ConsoleOut
    }

    public interface Loggerable
    {
        int level { get; set; }

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
        private int level_ = 0;

        public Debugger() : base()
        {
        }

        int Loggerable.level
        {
            get { return level_; }
            set { level_ = value; }
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
        public int level_ = 0;

        public Consoler() : base()
        {
        }

        int Loggerable.level
        {
            get { return level_; }
            set { level_ = value; }
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
        public int level_ = 0;

        public ConsoleOut() : base()
        {
        }

        int Loggerable.level
        {
            get { return level_; }
            set { level_ = value; }
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

    public class DummyLogger : Loggerable
    {
        public int level_ = 0;

        public DummyLogger() : base()
        {
        }

        int Loggerable.level
        {
            get { return level_; }
            set { level_ = value; }
        }

        public void Close()
        {
            // Do nothing !
        }

        public void Flush()
        {
            // Do nothing !
        }

        public void Print(string message)
        {
            // Do nothing !
        }

        public void Print(string format, params object[] args)
        {
            // Do nothing !
        }

        public void Write(string message)
        {
            // Do nothing !
        }

        public void Write(string format, params object[] args)
        {
            // Do nothing !
        }

        public void Write(Exception ex)
        {
            // Do nothing !
        }

        public void WriteLine(string message)
        {
            // Do nothing !
        }

        public void WriteLine(string format, params object[] args)
        {
            // Do nothing !
        }

        public void WriteLine(Exception ex)
        {
            // Do nothing !
        }
    }

    public class Logger
    {
        private Loggerable logger = null;
        private const int kMinLevel = 6;
        private const int kMaxLevel = 14;

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
            Logger.Print(this.logger, message);
        }

        public void Print(string format, params object[] args)
        {
            Logger.Print(this.logger, format, args);
        }

        public void Write(string message)
        {
            Logger.Write(this.logger, message);
        }

        public void Write(string format, params object[] args)
        {
            Logger.Write(this.logger, format, args);
        }

        public void Write(Exception ex)
        {
            Logger.Write(this.logger, ex);
        }

        public void WriteLine(string message)
        {
            Logger.WriteLine(this.logger, message);
        }

        public void WriteLine(string format, params object[] args)
        {
            Logger.WriteLine(this.logger, format, args);
        }

        public void WriteLine(Exception ex)
        {
            Logger.WriteLine(this.logger, ex);
        }

        public void Enter(string message)
        {
            Logger.Enter(this.logger, message);
        }

        public void Leave(string message)
        {
            Logger.Leave(this.logger, message);
        }

        public void Flush()
        {
            Logger.Flush(this.logger);
        }

        public void Close()
        {
            Logger.Close(this.logger);
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

        static public void Enter(Loggerable logger, string message)
        {
            if (logger != null)
            {
                int level = logger.level;
                int count = (level < kMaxLevel) ? (kMaxLevel - level) : 0;
                count = (count >= kMinLevel) ? count : (kMaxLevel - kMinLevel);
                string separator = "";
                for (int i = 0; i < count; i++)
                {
                    separator += "-----";
                }
                logger.WriteLine(separator);
                logger.WriteLine(message + " enter.");
                logger.level++;
            }
        }

        static public void Leave(Loggerable logger, string message)
        {
            if (logger != null)
            {
                logger.level--;
                logger.WriteLine(message + " leave.");
                int level = logger.level;
                int count = (level < kMaxLevel) ? (kMaxLevel - level) : 0;
                count = (count >= kMinLevel) ? count : (kMaxLevel - kMinLevel);
                string separator = "";
                for (int i = 0; i < count; i++)
                {
                    separator += "-----";
                }
                logger.WriteLine(separator);
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
    }
}
