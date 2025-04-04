﻿using System;
using System.IO;
using System.Reflection;

namespace CommandLine.Utilities
{
    public static class CommandLineUtilities
    {
        private static readonly string Copyright;
        public static readonly string Title;
        public static readonly string InformationalVersion;
        public static readonly string Version;

        static CommandLineUtilities()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            Copyright            = GetCopyright(executingAssembly);
            Version              = GetVersion(executingAssembly);
            InformationalVersion = GetInformationalVersion(executingAssembly);
            
            var entryAssembly = Assembly.GetEntryAssembly();
            
            Title = GetTitle(entryAssembly);
        }

        private static string GetCopyright(Assembly entryAssembly)
        {
            var attr = GetAssemblyAttributes<AssemblyCopyrightAttribute>(entryAssembly);
            return attr?.Copyright.Replace("©", "(c)") ?? $"(c) {DateTime.Now.Year} Illumina, Inc.";
        }

        public static string GetVersion(Assembly entryAssembly)
        {
            var attr = GetAssemblyAttributes<AssemblyFileVersionAttribute>(entryAssembly);
            return attr?.Version;
        }

        private static string GetInformationalVersion(Assembly entryAssembly)
        {
            var attr = GetAssemblyAttributes<AssemblyInformationalVersionAttribute>(entryAssembly);
            return attr?.InformationalVersion;
        }

        private static string GetTitle(Assembly entryAssembly)
        {
            var attr = GetAssemblyAttributes<AssemblyTitleAttribute>(entryAssembly);
            return attr?.Title;
        }

        private static T GetAssemblyAttributes<T>(Assembly entryAssembly)
        {
            var attrs = entryAssembly.GetCustomAttributes(typeof(T)) as T[];
            // ReSharper disable once PossibleNullReferenceException
            return attrs.Length == 0 ? default : attrs[0];
        }

        /// <summary>
        /// Displays the command-line banner for this program
        /// </summary>
        public static void DisplayBanner(string author)
        {
            // create the top and bottom lines
            const int lineLength = 75;
            const int maxVersionLength = 20; // Define a maximum length for the version string
            var line = new string('-', lineLength);

            // truncate the version if it's too long
            var truncatedVersion = InformationalVersion.Length > maxVersionLength
                ? InformationalVersion.Substring(0, maxVersionLength) + "..."
                : InformationalVersion;

            // create the filler string
            int fillerLength  = lineLength - Title.Length - Copyright.Length;
            int fillerLength2 = lineLength - author.Length - truncatedVersion.Length;

            if (fillerLength < 1)
            {
                throw new InvalidOperationException("Unable to display the program banner, the program name is too long.");
            }

            if (fillerLength2 < 1)
            {
                throw new InvalidOperationException("Unable to display the program banner, the author name and version string is too long.");
            }

            var filler  = new string(' ', fillerLength);
            var filler2 = new string(' ', fillerLength2);

            // display the actual banner
            Console.WriteLine(line);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(Title);
            Console.ResetColor();
            Console.WriteLine("{0}{1}", filler, Copyright);
            Console.WriteLine("{0}{1}{2}", author, filler2, truncatedVersion);
            Console.WriteLine("{0}\n", line);
        }

        public static string CommandFileName => Path.GetFileName(Environment.GetCommandLineArgs()[0]);
    }
}
