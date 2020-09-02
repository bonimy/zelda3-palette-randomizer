// <copyright file="SnesColor.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Drawing;
    using Maseya.Helper;
    using static Maseya.Helper.StringHelper;

    /// <summary>
    /// Represents a 15-bit pixel in BGR 5 bit per channel format.
    /// </summary>
    public struct SnesColor : IEquatable<SnesColor>
    {
        /// <summary>
        /// The maximum value that any channel can be.
        /// </summary>
        public const int MaxChannelValue = ChannelMask;

        /// <summary>
        /// The byte index of the <see cref="Low"/> component.
        /// </summary>
        public const int LowIndex = 0;

        /// <summary>
        /// The byte index of the <see cref="High"/> component.
        /// </summary>
        public const int HighIndex = 1;

        /// <summary>
        /// Represent a <see cref="SnesColor"/> that is black.
        /// </summary>
        public static readonly SnesColor Empty = default;

        private const int RedIndex = 0;
        private const int GreenIndex = 1;
        private const int BlueIndex = 2;

        private const int BitsPerChannel = 5;
        private const int RedShift = BitsPerChannel * RedIndex;
        private const int GreenShift = BitsPerChannel * GreenIndex;
        private const int BlueShift = BitsPerChannel * BlueIndex;

        private const int ChannelMask = (1 << BitsPerChannel) - 1;
        private const int RedMask = ChannelMask << RedShift;
        private const int GreenMask = ChannelMask << GreenShift;
        private const int BlueMask = ChannelMask << BlueShift;
        private const int ColorMask = RedMask | GreenMask | BlueMask;

        private ushort _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnesColor "/> struct
        /// with given low and high bytes.
        /// </summary>
        public SnesColor(byte low, byte high)
            : this((ushort)(low | (high << 8)))
        {
        }

        public SnesColor(int red, int green, int blue)
        {
            _value = (ushort)(
                ((red & ChannelMask) << RedShift) |
                ((green & ChannelMask) << GreenShift) |
                ((blue & ChannelMask) << BlueShift));
        }

        private SnesColor(ushort value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets or sets the numerical representation of this <see cref="
        /// SnesColor"/>.
        /// </summary>
        public int Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = (ushort)value;
            }
        }

        /// <summary>
        /// Gets or sets the numerical representation of this <see cref="
        /// SnesColor"/> ignoring the most significant bit, which doesn't
        /// factor into its result.
        /// </summary>
        public int ProperValue
        {
            get
            {
                return Value & ColorMask;
            }

            set
            {
                Value &= ~ColorMask;
                Value |= value & ColorMask;
            }
        }

        /// <summary>
        /// Gets or sets the high byte of <see cref="Value"/>.
        /// </summary>
        public byte High
        {
            get
            {
                return (byte)(Value >> 8);
            }

            set
            {
                Value &= 0x00FF;
                Value |= (ushort)(value << 8);
            }
        }

        /// <summary>
        /// Gets or sets the low byte of <see cref="Value"/>.
        /// </summary>
        public byte Low
        {
            get
            {
                return (byte)Value;
            }

            set
            {
                Value &= 0xFF00;
                Value |= value;
            }
        }

        /// <summary>
        /// Gets or sets the red intensity.
        /// </summary>
        public byte Red
        {
            get
            {
                return (byte)((Value & RedMask) >> RedShift);
            }

            set
            {
                Value &= unchecked((ushort)(~RedMask));
                Value |= (ushort)((value << RedShift) & RedMask);
            }
        }

        /// <summary>
        /// Gets or sets the green intensity.
        /// </summary>
        public byte Green
        {
            get
            {
                return (byte)((Value & GreenMask) >> GreenShift);
            }

            set
            {
                Value &= unchecked((ushort)(~GreenMask));
                Value |= (ushort)((value << GreenShift) & GreenMask);
            }
        }

        /// <summary>
        /// Gets or sets the blue intensity.
        /// </summary>
        public byte Blue
        {
            get
            {
                return (byte)((Value & BlueMask) >> BlueShift);
            }

            set
            {
                Value &= unchecked((ushort)(~BlueMask));
                Value |= (ushort)((value << BlueShift) & BlueMask);
            }
        }

        public SnesColor Inverse
        {
            get
            {
                return (SnesColor)(_value ^ ColorMask);
            }
        }

        public static explicit operator SnesColor(int value)
        {
            return new SnesColor((ushort)value);
        }

        public static implicit operator int(SnesColor color15)
        {
            return color15.Value;
        }

        public static explicit operator SnesColor(Color color)
        {
            return new SnesColor(
                RoundAndClampChannel(color.R) >> 3,
                RoundAndClampChannel(color.G) >> 3,
                RoundAndClampChannel(color.B) >> 3);

            static byte RoundAndClampChannel(byte x)
            {
                return (byte)Math.Clamp(x + 4, Byte.MinValue, Byte.MaxValue);
            }
        }

        public static implicit operator Color(SnesColor color15)
        {
            return Color.FromArgb(
                color15.Red << 3,
                color15.Green << 3,
                color15.Blue << 3);
        }

        public static explicit operator SnesColor(ColorF colorF)
        {
            return (SnesColor)(Color)colorF;
        }

        public static implicit operator ColorF(SnesColor color15)
        {
            return (ColorF)(Color)color15;
        }

        public static bool operator ==(
            SnesColor left,
            SnesColor right)
        {
            return left.ProperValue == right.ProperValue;
        }

        public static bool operator !=(
            SnesColor left,
            SnesColor right)
        {
            return !(left == right);
        }

        public static SnesColor FromInt32(int value)
        {
            return (SnesColor)value;
        }

        public static int ToInt32(SnesColor color15)
        {
            return color15;
        }

        public static SnesColor FromColor(Color color)
        {
            return (SnesColor)color;
        }

        public static Color ToColor(SnesColor color15)
        {
            return color15;
        }

        public static SnesColor FromColorF(ColorF colorF)
        {
            return (SnesColor)colorF;
        }

        public static SnesColor ToColorF(SnesColor color15)
        {
            return color15;
        }

        public bool Equals(SnesColor obj)
        {
            return this == obj.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is SnesColor other && Equals(other);
        }

        public override int GetHashCode()
        {
            return ProperValue;
        }

        public override string ToString()
        {
            return GetString(
                "{{R:{0},G:{1},B:{2}}}",
                Red,
                Green,
                Blue);
        }
    }
}
