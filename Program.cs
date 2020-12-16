using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace PassHash
{
    static class Program
    {
        public class Options
    {
        [Option('i',"in", Required = true, HelpText = "Input file containing list of passwords")]
        public string inputFile { get; set; }
        
        [Option('o',"out", Required = true, HelpText = "Output file to write")]
        public string outputFile { get; set; }
        
        [Usage(ApplicationAlias = "NTLMhash")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>() {
                    new Example("Generate list of NTLM password hash from a file", new Options
                    {
                        inputFile = "password-list.txt",
                        outputFile = "password-hash.txt"
                    })
                };
            }
        }
    }
        
        static void Main(string[] args)
        {
            Environment.Exit(Parser.Default.ParseArguments<Options>(args).MapResult(Run, HandleParseError));
        }

        static int Run(Options o)
        {
            // var x = Assembly.GetExecutingAssembly().Location;
            // string t = System.AppContext.BaseDirectory + Assembly.GetExecutingAssembly().GetName().Name;
            // FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(t);
            Console.WriteLine($"\x1b[1mNTLMhash\x1b[0m");
            // Console.WriteLine($"© 2020 {versionInfo.LegalCopyright}");
            Console.WriteLine();
            
            if (File.Exists(o.inputFile))
            {
                Console.WriteLine("Checking input file...");
                int lineCount = File.ReadLines(o.inputFile).Count();
                Console.WriteLine($"Total lines: {lineCount}");

                int index = 0;
                int currentCount = 0;
                using (FileStream fs = File.Open(o.inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream bs = new BufferedStream(fs))
                using (StreamReader sr = new StreamReader(bs))
                {
                    Console.WriteLine("Processing...");
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        ++currentCount;
                        ++index;
                        
                        File.AppendAllText(o.outputFile, $"{line}:{BCMD4.NTHash(line)}\n");
                        if (currentCount == 50)
                        {
                            currentCount = CalcPercent(index, lineCount);
                        }
                    }
                }
                
                CalcPercent(index, lineCount);
                
                return 0;
            }
            else
            {
                Console.WriteLine($"Input file not exists [{o.inputFile}]");
                return -3;
            }
        }

        private static int CalcPercent(int index, int lineCount)
        {
            int currentCount;
            Decimal percent = (Convert.ToDecimal(index) / Convert.ToDecimal(lineCount)) * 100;
            Console.WriteLine($"Progress {percent.ToString("00.00")} %");
            currentCount = 0;
            return currentCount;
        }

        static int HandleParseError(IEnumerable<Error> errs)
        {
            var result = -2;
            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
                result = -1;
            return result;
        }
    }
}