using Valve.VR;
using System.Windows.Forms;
using System.IO;
using System;

namespace ClassLibrary2
{
    public class Class1
    {
        public static void Test()
        {
            using StreamWriter sW = new StreamWriter("~1.txt");
            
            sW.WriteLine(DateTime.Now);
            
        }
    }
}