using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorApp.Util
{
    public class EventTraceListener : TraceListener
    {
        private readonly SemaphoreSlim mutex = new SemaphoreSlim(1);

        public string FileName { get; }

        public EventTraceListener(string fileName)
        {
            this.FileName = fileName;
            if (File.Exists(fileName))
            {
                try
                {
                    // Create a new file each time the program is started
                    using (var writer = new StreamWriter(fileName, append: false))
                    {
                        writer.Write("");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public override void Write(string message)
        {
            try
            {
                mutex.Wait();

                using (var writer = new StreamWriter(this.FileName, true, Encoding.ASCII))
                {
                    writer.Write(message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                mutex.Release();
            }
        }

        public override void WriteLine(string message)
        {
            mutex.Wait();
            try
            {
                using (var writer = new StreamWriter(this.FileName, true, Encoding.ASCII))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                mutex.Release();
            }
        }
    }
}
