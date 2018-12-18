using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace GildedRose
{
    class Program
    {


        public static void Main()
        {
            GildedRoseManager myManager = new GildedRoseManager();
            myManager.Run();
        }
    }
}
