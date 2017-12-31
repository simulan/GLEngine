using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core;

namespace UMLProgram {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            SetupLogger();
            Run();
        }
        private static void SetupLogger() {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }
        private static void Run() {
            using (UmlWindow window = new UmlWindow()) {
                window.Run(30.0);
            }
        }
    }
}
