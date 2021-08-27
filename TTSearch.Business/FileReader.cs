using System;
using System.Linq;
using static System.Console;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

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

        public void FindInFiles(string ext = "*.*") {
            string[] fileSystemEntries = Directory.GetFiles(_FilePath, ext, SearchOption.AllDirectories);
            List<string> res = new List<string>();

            Stopwatch sw = new();
            sw.Start();

            // PARALLEL
            //Parallel.ForEach(fileSystemEntries, i => ReadFile(i, res));
            Parallel.ForEach(fileSystemEntries, i => ReadFileParallel(i, res));

            // NON
            //foreach (string str in fileSystemEntries) { ReadFile(str, res); }
            //foreach (string str in fileSystemEntries) { ReadFileParallel(str, res); }
            sw.Stop();

            WriteLine("Read {0} lines in {1} file(s){2} in {3} ticks ({4} milliseconden).",
                    totalLinesRead, filesRead, ext, sw.ElapsedTicks, sw.ElapsedMilliseconds);
            WriteLine("Press a key to see the results ({0}):", res.Count);
            ReadKey();

            res.ForEach(r => WriteLine(r));
        }

        private void ReadFile(string filePath, List<string> results) {
            string[] fileDataArray = File.ReadAllLines(filePath);
            int linesRead = 0;

            foreach (string line in fileDataArray) {
                if (line.Contains(_KeyWords[0])) { results.Add(
                    $"{filePath} Contains: '{_KeyWords[0]}' At Line {linesRead}:\n\"{line}\"\nThread: {Thread.CurrentThread.ManagedThreadId})\n");
                }

                linesRead++;
            }

            totalLinesRead += linesRead;
            filesRead++;
        }

        private void ReadFileParallel(string filePath, List<string> results) {
            string[] fileDataArray = File.ReadAllLines(filePath);
            int linesRead = 0;

            Parallel.ForEach(fileDataArray, line => {
                if (line.Contains(_KeyWords[0])) { results.Add(
                    $"{filePath} Contains: '{_KeyWords[0]}' At Line {linesRead}:\n\"{line}\"\nThread: {Thread.CurrentThread.ManagedThreadId})\n"
                );}

                linesRead++;
            }); 

            totalLinesRead += linesRead;
            filesRead++;
        }
    }
}

/*
Stopwatch sw = new();
sw.Start();
sw.Stop();

string tr = Thread.CurrentThread.ManagedThreadId.ToString();
WriteLine($"Thread: [{(tr.Length < 2 ? '0' + tr : tr)}] Elapsed: [{sw.ElapsedTicks}], Path: [{filePath}]");
sw.Reset();

ReadKey();
var items = from pair in resultsDict orderby pair.Value ascending select pair;
foreach (KeyValuePair<string,long> pair in items) {
    WriteLine($"{pair.Key} {pair.Value}");
}
*/
