using System;
using steganography;

namespace CLIVer
{
    class Program
    {
        static void Main(string[] args)
        {
            string strMessage = "Mohamed Saleh";

            CoreTxtImageEx.HideMessage("01.bmp", strMessage);
            CoreTxtImageEx.ExtractMessageToFile("Enc.bmp");

            string hiddenMessage = CoreTxtImageEx.ExtractMessage("Enc.bmp");
            Console.WriteLine("Hidden Message -> " + hiddenMessage);

        }

    }
}
