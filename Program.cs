using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;
using steganography;

namespace CLIVer
{
    class Program
    {
        static void Main(string[] args)
        {
            CoreTxtImageEx.HideMessage("01.bmp", "Mohamed Saleh");
            CoreTxtImageEx.DiscloseMessage("Enc.bmp");
        }

    }
}
