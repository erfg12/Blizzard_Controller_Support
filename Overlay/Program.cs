using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;

namespace Overlay
{
    class Program
    {
        private static void Main(string[] args)
        {
            var ow = new GenerateOverlayWindow();
            ow.Initialize();
            //ow.Run();
            Console.ReadLine();
        }
    }
}
