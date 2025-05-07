using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace steganography
{
    public class CoreTxtImageEx
    {
        /// <summary>
        /// Hide text message into a Bitmap
        /// </summary>
        /// <param name="strImagePath">Image File Path</param>
        /// <param name="txtMessage">Message to hide</param>
        public static void HideMessage(string strImagePath, string txtMessage)
        {
            Bitmap bmImage = new Bitmap(strImagePath);
            ulong MessageLen = (ulong)txtMessage.Length;
            ulong iTotal = (ulong)(bmImage.Width * bmImage.Height);
            bool doable = MessageLen + 3 <= iTotal;
            ulong iLetterIndex = 0;
            bool bFinished = false;
            int iWidth = 0, iHeight = 0;

            if (!doable)
            {
                Console.WriteLine("Please select bigger picture.");
                bmImage.Dispose();
                return;
            }

            // Store Size
            List<Color> colors = misc.ConvertNumberToBytes(MessageLen);

            for (; iHeight < 3; iHeight++)
            {
                Color pix = colors[iHeight];
                bmImage.SetPixel(iWidth, iHeight, pix);
            }

            // Store Message
            iHeight = 3;
            for (; iWidth < bmImage.Width && !bFinished; iWidth++)
            {
                for (; iHeight < bmImage.Height ; iHeight++)
                {
                    Color pix = bmImage.GetPixel(iWidth, iHeight);
                    if (iLetterIndex >= MessageLen)
                    {
                        bFinished = true;
                        break;
                    }
                    else
                    {
                        Color nPix = Color.FromArgb(pix.A, pix.R, pix.G, Convert.ToInt32(txtMessage[(int)iLetterIndex]));
                        bmImage.SetPixel(iWidth, iHeight, nPix);
                        iLetterIndex++;
                    }
                }
            }

            FileInfo file_info = new FileInfo(strImagePath);
            string strExtensionLowerCase = file_info.Extension.ToLower();

            switch (strExtensionLowerCase)
            {
                case ".png":
                    bmImage.Save("Enc.png", System.Drawing.Imaging.ImageFormat.Png);
                    break;
                case ".bmp":
                    bmImage.Save("Enc.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case ".gif":
                    bmImage.Save("Enc.gif", System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case ".tiff":
                case ".tif":
                    bmImage.Save("Enc.tif", System.Drawing.Imaging.ImageFormat.Tiff);
                    break;
                case ".jpg":
                case ".jpeg":
                    bmImage.Save("Enc.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
            }
            bmImage.Dispose();
        }

        /// <summary>
        /// Extract Message from an Image 
        /// </summary>
        /// <param name="strImagePath">Image file path</param>
        private static StringBuilder CoreExtractMessageToFile(string strImagePath)
        {
            Bitmap bmImage = new Bitmap(strImagePath);
            ulong iMessageLen = misc.GetMessageLength(bmImage);
            StringBuilder strBldAllText = new StringBuilder();
            ulong iLetterIndex = 0;
            bool bFinished = false;
            int iWidth = 0, iHeight = 3;

            for (; iWidth < bmImage.Width && !bFinished; iWidth++)
            {
                for (; iHeight < bmImage.Height; iHeight++)
                {
                    Color pix = bmImage.GetPixel(iWidth, iHeight);

                    int intMessageLetterValue = pix.B;
                    char chMessageLetter = Convert.ToChar(intMessageLetterValue);
                    strBldAllText.Append(chMessageLetter);

                    iLetterIndex++;

                    if (iLetterIndex >= iMessageLen)
                    {
                        bFinished = true;
                        break;
                    }
                }
            }

            return strBldAllText;
        }

        /// <summary>
        /// Extract Message from an Image to String
        /// </summary>
        /// <param name="strImagePath">Image file path</param>
        public static void ExtractMessageToFile(string strImagePath)
        {
            StringBuilder strBldAllText = new StringBuilder();
            strBldAllText = CoreExtractMessageToFile(strImagePath);

            StreamWriter sw = new StreamWriter("message.txt", true);
            sw.Write(strBldAllText.ToString());
            sw.Close();
        }

        /// <summary>
        /// Extract Message from an Image to File message.txt
        /// </summary>
        /// <param name="strImagePath">Image file path</param>
        public static string ExtractMessage(string strImagePath)
        {
            StringBuilder strBldAllText = new StringBuilder();
            strBldAllText = CoreExtractMessageToFile(strImagePath);

            return strBldAllText.ToString();
        }
    }
}
