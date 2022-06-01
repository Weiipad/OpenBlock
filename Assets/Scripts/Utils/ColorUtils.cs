using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Utils
{
    public static class ColorUtils
    {
        public static Color BROWN = new Color(165 / 255.0f, 42 / 255.0f, 42 / 255.0f);

        public static uint ToColorCode(byte r, byte g, byte b, byte a)
        {
            uint ans = 0;
            ans |= r;
            ans |= (uint)(g << 8);
            ans |= (uint)(b << 16);
            ans |= (uint)(a << 24);
            return ans;
        }

        public static uint ToColorCode(this Color color)
        {
            byte r = (byte)(color.r * 255.0f);
            byte g = (byte)(color.g * 255.0f);
            byte b = (byte)(color.b * 255.0f);

            uint ans = 0;
            ans |= r;
            ans |= (uint)(g << 8);
            ans |= (uint)(b << 16);
            ans |= (0xf << 24);
            return ans;
        }

        public static Color ToColor(this uint colorCode)
        {
            return new Color((byte)(colorCode >> 0) / 255.0f, (byte)(colorCode >> 8) / 255.0f, (byte)(colorCode >> 16) / 255.0f);
        }
    }
}
