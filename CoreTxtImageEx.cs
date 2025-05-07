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
        public static void HideMessage(string strImagePath, string txtEncMessage)
        {
            Bitmap bmImage = new Bitmap(strImagePath);
            ulong MessageLen = (ulong)txtEncMessage.Length;
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
                        Color nPix = Color.FromArgb(pix.A, pix.R, pix.G, Convert.ToInt32(txtEncMessage[(int)iLetterIndex]));
                        bmImage.SetPixel(iWidth, iHeight, nPix);
                        iLetterIndex++;
                    }
                }
            }

            SaveModifiedImage(ref bmImage, strImagePath);
        }

        private static void SaveModifiedImage(ref Bitmap bmImage, string strImagePath)
        {

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

        private static ulong GetMessageLength(Bitmap bmImage)
        {
            List<Color> colors = new List<Color>();
            colors.Add(bmImage.GetPixel(0, 0));
            colors.Add(bmImage.GetPixel(0, 1));
            colors.Add(bmImage.GetPixel(0, 2));
            ulong messageSize = misc.ColorBytesToULong(colors);
            return messageSize;
        }
        public static void DiscloseMessage(string strImagePath)
        {
            Bitmap bmImage = new Bitmap(strImagePath);
            ulong iMessageLen = GetMessageLength(bmImage);
            StringBuilder strBldAllText = new StringBuilder();
            ulong iLetterIndex = 0;
            bool bFinished = false;
            int iWidth = 0,iHeight = 3;

            for ( ; iWidth < bmImage.Width && !bFinished; iWidth++)
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

            StreamWriter sw = new StreamWriter("Dec.txt", true);
            sw.Write(strBldAllText.ToString());
            sw.Close();


        }
    }
}
