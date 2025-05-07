using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace steganography
{
    public class misc
    {
        /// <summary>
        /// Convert ulong to List of Color
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static List<Color> ConvertNumberToBytes(ulong number)
        {
            byte[] sizeInBytes = BitConverter.GetBytes(number).ToArray();
            List<Color> colors = new List<Color>
            {
                BytesToColor(sizeInBytes[0], sizeInBytes[1], sizeInBytes[2]),
                BytesToColor(sizeInBytes[3], sizeInBytes[4], sizeInBytes[5]),
                BytesToColor(sizeInBytes[6], sizeInBytes[7], 0)
            };

            return colors;
        }
        /// <summary>
        /// Convert Colors to uLong
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static ulong ColorBytesToULong(List<Color> colors)
        {

            List<byte> sizeInBytes = new List<byte>();
            foreach (Color color in colors)
            {
                sizeInBytes.Add(color.R);
                sizeInBytes.Add(color.G);
                sizeInBytes.Add(color.B);
            }
            ulong stored_value = BitConverter.ToUInt64(sizeInBytes.ToArray(), 0);
            return stored_value;
        }

        /// <summary>
        /// Bytes to Color
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static Color BytesToColor(byte r, byte g, byte b)
        {
            int red = Convert.ToInt32(r);
            int green = Convert.ToInt32(g);
            int blue = Convert.ToInt32(b);
            Color c = Color.FromArgb(red, green, blue);
            return c;
        }

        /// <summary>
        /// Extract Message Size from a bitmap
        /// </summary>
        /// <param name="bmImage"></param>
        /// <returns></returns>
        public static ulong GetMessageLength(Bitmap bmImage)
        {
            List<Color> colors = new List<Color>()
            {
                bmImage.GetPixel(0, 0),
                bmImage.GetPixel(0, 1),
                bmImage.GetPixel(0, 2)
            };
            ulong messageSize = misc.ColorBytesToULong(colors);
            return messageSize;
        }
    }
}
