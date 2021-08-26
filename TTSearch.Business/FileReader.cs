using System;
using System.Linq;
using static System.Console;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TTSearch.Business
{
    public class FileReader
    {
        private string _FilePath { get; set; }
        private string[] _KeyWords { get; set; }

        private int filesRead;
        private int totalLinesRead;

        public FileReader(string FilePath, string[] KeyWords) {
            this._FilePath = FilePath;
            this._KeyWords = KeyWords;
        }

        public void Testing() {
            WriteLine($"{_FilePath}, {String.Join(" ", _KeyWords)}");
        }

        public void PrintResults() {
        }

        public void FindInFiles() {
            string[] fileSystemEntries = Directory.GetFiles(_FilePath, "*.*", SearchOption.AllDirectories);
            List<string> res = new List<string>();

            Parallel.ForEach(fileSystemEntries, i => { ReadFile(i, res); });

            WriteLine("Read {0} lines in {1} file(s).", totalLinesRead, filesRead);
            WriteLine("Press a key to see the results:");
            ReadKey();

            res.ForEach(r => WriteLine(r));
        }

        private void ReadFile(string filePath, List<string> results) {
            string[] fileDataArray = File.ReadAllLines(filePath);
            int linesRead = 0;

            //results.Add($"file {filePath} threadID: {Thread.CurrentThread.ManagedThreadId}");

            Parallel.ForEach(fileDataArray, line => {
                if (line.Contains(_KeyWords[0])) { results.Add(
                    $"\t>({filePath}) contains '{_KeyWords[0]}' at line {linesRead}:\n\"{line}\" (thread: {Thread.CurrentThread.ManagedThreadId})\n"
                );}

                linesRead++;
            }); 

            totalLinesRead += linesRead;
            filesRead++;
        }
    }
}
