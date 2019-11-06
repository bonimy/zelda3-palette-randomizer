// <copyright file="Color15BppBgr.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.PixelFormat
{
    using System;
    using System.Drawing;
    using static StringHelper;

    /// <summary>
    /// Represents a 15-bit pixel in RGB 5 bit per channel format.
    /// </summary>
    /// <remarks>
    /// The purpose of this data type is to ease the process of bitmap drawing.
    /// For 15-bit BGR bitmaps, its pixel buffer pointer can be cast to type
    /// <see cref="Color15BppBgr"/>* for simplified color operations.
    /// </remarks>
    public struct Color15BppBgr : IEquatable<Color15BppBgr>
    {
        /// <summary>
        /// The size, in bytes, of the <see cref="Color15BppBgr"/> data type.
        /// </summary>
        public const int SizeOf = sizeof(short);

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
        /// Represent a <see cref="Color15BppBgr"/> that is black.
        /// </summary>
        public static readonly Color15BppBgr Empty = default;

        /// <summary>
        /// The number of bits in a <see cref="Byte"/>.
        /// </summary>
        private const int BitsPerByte = 8;

        /// <summary>
        /// The 5-bit index of the <see cref="Red"/> component.
        /// </summary>
        private const int RedIndex = 0;

        /// <summary>
        /// The 5-bit index of the <see cref="Green"/> component.
        /// </summary>
        private const int GreenIndex = 1;

        /// <summary>
        /// The 5-bit index of the <see cref="Blue"/> component.
        /// </summary>
        private const int BlueIndex = 2;

        /// <summary>
        /// The number of bits that represent the intensity of the any channel.
        /// </summary>
        private const int BitsPerChannel = 5;

        /// <summary>
        /// The number of bits to shift left to reach the <see cref=" Red"/>
        /// component.
        /// </summary>
        private const int RedShift = BitsPerChannel * RedIndex;

        /// <summary>
        /// The number of bits to shift left to reach the <see cref=" Green"/>
        /// component.
        /// </summary>
        private const int GreenShift = BitsPerChannel * GreenIndex;

        /// <summary>
        /// The number of bits to shift left to reach the <see cref=" Blue"/>
        /// component.
        /// </summary>
        private const int BlueShift = BitsPerChannel * BlueIndex;

        /// <summary>
        /// The number of bits difference between bits in a <see cref=" Byte"/>
        /// and bits in a <see cref="Color15BppBgr"/> color channel.
        /// </summary>
        private const int BitShift = BitsPerByte - BitsPerChannel;

        /// <summary>
        /// The bit mask of a channel mask (5 consecutive bits set).
        /// </summary>
        private const int ChannelMask = (1 << BitsPerChannel) - 1;

        /// <summary>
        /// The bit mask of the <see cref="Red"/> channel in its binary
        /// representation of <see cref="Color15BppBgr"/>.
        /// </summary>
        private const int RedMask = ChannelMask << RedShift;

        /// <summary>
        /// The bit mask of the <see cref="Green"/> channel in its binary
        /// representation of <see cref="Color15BppBgr"/>.
        /// </summary>
        private const int GreenMask = ChannelMask << GreenShift;

        /// <summary>
        /// The bit mask of the <see cref="Blue"/> channel in its binary
        /// representation of <see cref="Color15BppBgr"/>.
        /// </summary>
        private const int BlueMask = ChannelMask << BlueShift;

        /// <summary>
        /// The bit mask of all color channel in its binary representation of
        /// <see cref="Color15BppBgr"/>.
        /// </summary>
        private const int ColorMask = RedMask | GreenMask | BlueMask;

        /// <summary>
        /// The bit mask of the <see cref="High"/> byte in its binary
        /// representation of <see cref="Color15BppBgr"/>.
        /// </summary>
        private const int HighMask =
            Byte.MaxValue << (BitsPerByte * HighIndex);

        /// <summary>
        /// The bit mask of the <see cref="Low"/> byte in its binary
        /// representation of <see cref="Color15BppBgr"/>.
        /// </summary>
        private const int LowMask =
            Byte.MaxValue << (BitsPerByte * LowIndex);

        /// <summary>
        /// The binary representation of this <see cref=" Color15BppBgr"/>.
        /// </summary>
        private ushort _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Color15BppBgr "/>
        /// struct with given low and high bytes.
        /// </summary>
        /// <param name="low">
        /// The low byte.
        /// </param>
        /// <param name="high">
        /// The high byte.
        /// </param>
        public Color15BppBgr(byte low, byte high)
            : this((ushort)(low | (high << BitsPerByte)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color15BppBgr "/>
        /// struct with given red, green, and blue intensities.
        /// </summary>
        /// <param name="red">
        /// The red intensity. Valid values are from 0 to <see cref="
        /// ChannelMask"/>.
        /// </param>
        /// <param name="green">
        /// The green intensity. Valid values are from 0 to <see cref="
        /// ChannelMask"/>.
        /// </param>
        /// <param name="blue">
        /// The blue intensity. Valid values are from 0 to <see cref="
        /// ChannelMask"/>.
        /// </param>
        public Color15BppBgr(int red, int green, int blue)
        {
            _value = (ushort)(
                ((red & ChannelMask) << RedShift) |
                ((green & ChannelMask) << GreenShift) |
                ((blue & ChannelMask) << BlueShift));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color15BppBgr "/>
        /// struct with a given value.
        /// </summary>
        /// <param name="value">
        /// The binary value to assign.
        /// </param>
        private Color15BppBgr(ushort value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets or sets the binary representation of this <see cref="
        /// Color15BppBgr"/>.
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
        /// Gets or sets the binary representation of this <see cref="
        /// Color15BppBgr"/> ignoring the most significant bit, which doesn't
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
                return (byte)(Value >> BitsPerByte);
            }

            set
            {
                Value &= HighMask;
                Value |= (ushort)(value << BitsPerByte);
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
                Value &= LowMask;
                Value |= value;
            }
        }

        /// <summary>
        /// Gets or sets the red intensity.
        /// </summary>
        /// <remarks>
        /// Valid values range from 0 to <see cref=" ChannelMask"/>.
        /// </remarks>
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
        /// <remarks>
        /// Valid values range from 0 to <see cref=" ChannelMask"/>.
        /// </remarks>
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
        /// <remarks>
        /// Valid values range from 0 to <see cref=" ChannelMask"/>.
        /// </remarks>
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

        /// <summary>
        /// Creates a <see cref="Color15BppBgr"/> with a specified <see
        /// cref="Value"/> parameter.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Value"/> parameter to use for this <see
        /// cref="Color15BppBgr"/> instance.
        /// </param>
        /// <returns>
        /// A <see cref="Color15BppBgr"/> structure with a <see cref=" Value"/>
        /// equal to <paramref name="value"/>.
        /// </returns>
        public static explicit operator Color15BppBgr(int value)
        {
            return new Color15BppBgr((ushort)value);
        }

        /// <summary>
        /// Creates a 32-bit signed integer equal to a <see cref=" Value"/>
        /// parameter.
        /// </summary>
        /// <param name="color15">
        /// The <see cref="Color15BppBgr"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Int32"/> whose value is the <see cref="Value"/> of
        /// <paramref name="color15"/>.
        /// </returns>
        public static implicit operator int(Color15BppBgr color15)
        {
            return color15.Value;
        }

        /// <summary>
        /// Creates a <see cref="Color15BppBgr"/> structure whose <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties are equal to a <see cref=" Color24BppRgb"/>'s <see
        /// cref="Color24BppRgb.Red"/>, <see cref="Color24BppRgb.Green"/>, and
        /// <see cref=" Color24BppRgb.Blue"/> properties, respectively.
        /// </summary>
        /// <param name="color24">
        /// The <see cref="Color24BppRgb"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color15BppBgr"/> whose <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties are equal to a
        /// <see cref=" Color24BppRgb"/>'s <see cref="Color24BppRgb.Red"/>,
        /// <see cref="Color24BppRgb.Green"/>, and <see cref="
        /// Color24BppRgb.Blue"/> properties, respectively.
        /// </returns>
        public static explicit operator Color15BppBgr(
            Color24BppRgb color24)
        {
            return new Color15BppBgr(
                color24.Red >> BitShift,
                color24.Green >> BitShift,
                color24.Blue >> BitShift);
        }

        /// <summary>
        /// Creates a <see cref="Color24BppRgb"/> structure whose <see
        /// cref="Color24BppRgb.Red"/>, <see cref="Color24BppRgb.Green "/>, and
        /// <see cref="Color24BppRgb.Blue"/> properties are equal to a <see
        /// cref="Color15BppBgr"/>'s <see cref="Red"/>, <see cref="Green"/>,
        /// and <see cref=" Blue"/> properties, respectively.
        /// </summary>
        /// <param name="color15">
        /// The <see cref="Color15BppBgr"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color24BppRgb"/> whose <see
        /// cref="Color24BppRgb.Red"/>, <see cref="Color24BppRgb.Green "/>, and
        /// <see cref="Color24BppRgb.Blue"/> properties are equal to a <see
        /// cref="Color15BppBgr"/>'s <see cref="Red"/>, <see cref="Green"/>,
        /// and <see cref=" Blue"/> properties, respectively of <paramref
        /// name="color15"/>.
        /// </returns>
        public static implicit operator Color24BppRgb(
            Color15BppBgr color15)
        {
            return new Color24BppRgb(
                color15.Red << BitShift,
                color15.Green << BitShift,
                color15.Blue << BitShift);
        }

        /// <summary>
        /// Creates a <see cref="Color15BppBgr"/> structure whose <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties are equal to a <see cref=" Color32BppArgb"/>'s <see
        /// cref="Color32BppArgb.Red"/>, <see cref="Color32BppArgb.Green"/>,
        /// and <see cref=" Color32BppArgb.Blue"/> properties, respectively.
        /// </summary>
        /// <param name="color32">
        /// The <see cref="Color32BppArgb"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color15BppBgr"/> whose <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties are equal to a
        /// <see cref=" Color32BppArgb"/>'s <see cref="Color32BppArgb.Red"/>,
        /// <see cref="Color32BppArgb.Green"/>, and <see cref="
        /// Color32BppArgb.Blue"/> properties, respectively.
        /// </returns>
        public static explicit operator Color15BppBgr(
            Color32BppArgb color32)
        {
            return new Color15BppBgr(
                color32.Red >> BitShift,
                color32.Green >> BitShift,
                color32.Blue >> BitShift);
        }

        /// <summary>
        /// Creates a <see cref="Color32BppArgb"/> structure whose <see
        /// cref="Color32BppArgb.Red"/>, <see cref="Color32BppArgb.Green "/>,
        /// and <see cref="Color32BppArgb.Blue"/> properties are equal to a
        /// <see cref="Color15BppBgr"/>'s <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref=" Blue"/> properties, respectively.
        /// The <see cref="Color32BppArgb.Alpha"/> is set to its default value.
        /// </summary>
        /// <param name="color15">
        /// The <see cref="Color15BppBgr"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color32BppArgb"/> whose <see
        /// cref="Color32BppArgb.Red"/>, <see cref="Color32BppArgb.Green "/>,
        /// and <see cref="Color32BppArgb.Blue"/> properties are equal to a
        /// <see cref="Color15BppBgr"/>'s <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref=" Blue"/> properties, respectively of
        /// <paramref name="color15"/>. The <see cref=" Color32BppArgb.Alpha"/>
        /// is set to its default value.
        /// </returns>
        public static implicit operator Color32BppArgb(
            Color15BppBgr color15)
        {
            return new Color32BppArgb(
                color15.Red << BitShift,
                color15.Green << BitShift,
                color15.Blue << BitShift);
        }

        /// <summary>
        /// Creates a <see cref="Color15BppBgr"/> structure whose <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties are equal to a <see cref=" Color"/>'s <see
        /// cref="Color.R"/>, <see cref="Color.G"/>, and <see cref="Color.B"/>
        /// properties, respectively.
        /// </summary>
        /// <param name="color">
        /// The <see cref="Color"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color15BppBgr"/> whose <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties are equal to a
        /// <see cref=" Color"/>'s <see cref="Color.R"/>, <see
        /// cref="Color.G"/>, and <see cref="Color.B"/> properties,
        /// respectively of <paramref name="color"/>.
        /// </returns>
        public static explicit operator Color15BppBgr(Color color)
        {
            return new Color15BppBgr(
                color.R >> BitShift,
                color.G >> BitShift,
                color.B >> BitShift);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> structure whose <see cref="
        /// Color.R"/>, <see cref="Color.G"/>, and <see cref="Color.B"/>
        /// properties are equal to a <see cref="Color15BppBgr"/>'s <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref=" Blue"/>
        /// properties, respectively. The <see cref=" Color.A"/> is set to its
        /// default value.
        /// </summary>
        /// <param name="color15">
        /// The <see cref="Color15BppBgr"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color"/> whose <see cref="Color.R"/>, <see
        /// cref="Color.G"/>, and <see cref="Color.B"/> properties are equal to
        /// a <see cref="Color15BppBgr"/>'s <see cref=" Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties, respectively of
        /// <paramref name="color15"/>. The <see cref="Color.A"/> is set to its
        /// default value.
        /// </returns>
        public static implicit operator Color(Color15BppBgr color15)
        {
            return Color.FromArgb(
                color15.Red << BitShift,
                color15.Green << BitShift,
                color15.Blue << BitShift);
        }

        /// <summary>
        /// Creates a <see cref="Color15BppBgr"/> structure whose <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties are equal to a <see cref=" ColorF"/>'s <see
        /// cref="ColorF.Red"/>, <see cref="ColorF. Green"/>, and <see
        /// cref="ColorF.Blue"/> properties, respectively.
        /// </summary>
        /// <param name="colorF">
        /// The <see cref="ColorF"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color15BppBgr"/> whose <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties are equal to a
        /// <see cref=" ColorF"/>'s <see cref="ColorF.Red"/>, <see
        /// cref="ColorF. Green"/>, and <see cref="ColorF.Blue"/> properties,
        /// respectively of <paramref name="colorF"/>.
        /// </returns>
        public static explicit operator Color15BppBgr(ColorF colorF)
        {
            return (Color15BppBgr)(Color24BppRgb)colorF;
        }

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure whose <see cref="
        /// ColorF.Red"/>, <see cref="ColorF.Green"/>, and <see cref="
        /// ColorF.Blue"/> properties are equal to a <see cref="
        /// Color15BppBgr"/>'s <see cref="Red"/>, <see cref="Green"/>, and <see
        /// cref="Blue"/> properties, respectively. The <see
        /// cref="ColorF.Alpha"/> is set to its default value.
        /// </summary>
        /// <param name="color15">
        /// The <see cref="Color15BppBgr"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="ColorF"/> whose <see cref=" ColorF.Red"/>, <see
        /// cref="ColorF.Green"/>, and <see cref=" ColorF.Blue"/> properties
        /// are equal to a <see cref=" Color15BppBgr"/>'s <see cref="Red"/>,
        /// <see cref="Green"/>, and <see cref="Blue"/> properties,
        /// respectively of <paramref name="color15"/>. The <see cref="
        /// ColorF.Alpha"/> is set to its default value.
        /// </returns>
        public static implicit operator ColorF(Color15BppBgr color15)
        {
            return (Color24BppRgb)color15;
        }

        /// <summary>
        /// Compares to <see cref="Color15BppBgr"/> values. The result
        /// specifies whether the values of the <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref=" Blue"/> properties of the two <see
        /// cref="Color15BppBgr"/> are equal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color15BppBgr"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color15BppBgr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> values of <paramref
        /// name="left"/> and <paramref name=" right"/> are equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator ==(
            Color15BppBgr left,
            Color15BppBgr right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares to <see cref="Color15BppBgr"/> values. The result
        /// specifies whether the values of the <see cref="Red"/>, <see
        /// cref="Green"/>, or <see cref=" Blue"/> properties of the two <see
        /// cref="Color15BppBgr"/> are unequal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color15BppBgr"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color15BppBgr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> values of <paramref
        /// name="left"/> or <paramref name=" right"/> are unequal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator !=(
            Color15BppBgr left,
            Color15BppBgr right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a
        /// specified <see cref="Color15BppBgr"/> value.
        /// </summary>
        /// <param name="obj">
        /// A <see cref="Color15BppBgr"/> value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> has the same value
        /// as this instance; otherwise, <see langword=" false"/>.
        /// </returns>
        public bool Equals(Color15BppBgr obj)
        {
            return ProperValue.Equals(obj.ProperValue);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a
        /// specified object.
        /// </summary>
        /// <param name="obj">
        /// An object to compare with this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of
        /// <see cref="Color15BppBgr"/> and equals the value of this instance;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Color15BppBgr value ? Equals(value) : false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
        {
            // We base equality on the proper value, not the base value.
            return ProperValue;
        }

        /// <summary>
        /// Converts the value of the current <see cref=" Color15BppBgr"/>
        /// object to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the value of this object.
        /// </returns>
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
