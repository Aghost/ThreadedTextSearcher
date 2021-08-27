using System;
using System.Text;
using static System.Console;
using System.Threading;
using System.Threading.Tasks;
using TTSearch.Business;

namespace TTSearch.App
{
    class Program
    {
        static void Main(string[] args) {
            if (args.Length > 0) {
                var fr = new FileReader(@"../../0_LIBRARY/", args);
                fr.FindInFiles();
            }
        }
    }
}
