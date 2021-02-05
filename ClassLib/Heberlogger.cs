using System;
using System.IO;
using System.Threading;

namespace ClassLib
{
    internal class Heberlogger
    {
        public const string LogPath = "hlog{0}.txt";
        internal static void Init() { }// => File.Delete(string.Format(LogPath, Thread.CurrentThread.ManagedThreadId));
        internal static void Write(string message, params object[] args)
        {
            File.AppendAllText(string.Format(LogPath, Thread.CurrentThread.ManagedThreadId),
                $"{Thread.CurrentThread.ManagedThreadId}: {string.Format(message, args)}\n");
        }
    }
}