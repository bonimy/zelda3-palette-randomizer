// <copyright file="Color24BppRgb.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.PixelFormat
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using static System.Math;
    using static StringHelper;

    /// <summary>
    /// Represents a 24-bit pixel in RGB 8 bit per channel format.
    /// </summary>
    /// <remarks>
    /// The purpose of this data type is to ease the process of bitmap drawing.
    /// For 24-bit RGB bitmaps, its pixel buffer pointer can be cast to type
    /// <see cref="Color24BppRgb"/>* for simplified color operations.
    /// </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 3)]
    public struct Color24BppRgb : IEquatable<Color24BppRgb>
    {
        /// <summary>
        /// The size, in bytes, of the <see cref="Color24BppRgb"/> data type.
        /// </summary>
        public const int SizeOf = 3 * sizeof(byte);

        /// <summary>
        /// The byte index of the <see cref="Red"/> component.
        /// </summary>
        public const int RedIndex = 2;

        /// <summary>
        /// The byte index of the <see cref="Green"/> component.
        /// </summary>
        public const int GreenIndex = 1;

        /// <summary>
        /// The byte index of the <see cref="Blue"/> component.
        /// </summary>
        public const int BlueIndex = 0;

        /// <summary>
        /// The number of bits that represent the intensity of the <see
        /// cref="Red"/> component.
        /// </summary>
        internal const int BitsPerRed = BitsPerChannel;

        /// <summary>
        /// The number of bits that represent the intensity of the <see
        /// cref="Green"/> component.
        /// </summary>
        internal const int BitsPerGreen = BitsPerChannel;

        /// <summary>
        /// The number of bits that represent the intensity of the <see
        /// cref="Blue"/> component.
        /// </summary>
        internal const int BitsPerBlue = BitsPerChannel;

        /// <summary>
        /// The number of bits that represent the intensity of the any channel.
        /// </summary>
        private const int BitsPerChannel = 8;

        /// <summary>
        /// The number of bits to shift left to reach the <see cref=" Red"/>
        /// component.
        /// </summary>
        private const int RedShift = BitsPerRed * RedIndex;

        /// <summary>
        /// The number of bits to shift left to reach the <see cref=" Green"/>
        /// component.
        /// </summary>
        private const int GreenShift = BitsPerGreen * GreenIndex;

        /// <summary>
        /// The number of bits to shift left to reach the <see cref=" Blue"/>
        /// component.
        /// </summary>
        private const int BlueShift = BitsPerBlue * BlueIndex;

        /// <summary>
        /// Represents a <see cref="Color24BppRgb"/> that is black.
        /// </summary>
        private static readonly Color24BppRgb _empty = default;

        /// <summary>
        /// The red intensity.
        /// </summary>
        [FieldOffset(RedIndex)]
        private byte _red;

        /// <summary>
        /// The green intensity.
        /// </summary>
        [FieldOffset(GreenIndex)]
        private byte _green;

        /// <summary>
        /// The blue intensity.
        /// </summary>
        [FieldOffset(BlueIndex)]
        private byte _blue;

        /// <summary>
        /// Initializes a new instance of the <see cref=" Color24BppRgb"/>
        /// struct with given red, green, and blue intensities.
        /// </summary>
        /// <param name="red">
        /// The red intensity.
        /// </param>
        /// <param name="green">
        /// The green intensity.
        /// </param>
        /// <param name="blue">
        /// The blue intensity.
        /// </param>
        public Color24BppRgb(int red, int green, int blue)
        {
            _red = (byte)red;
            _green = (byte)green;
            _blue = (byte)blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=" Color24BppRgb"/>
        /// struct from a 32-bit integer value.
        /// </summary>
        /// <param name="value">
        /// The 32-bit RGB value. The highest 8 bits are ignored.
        /// </param>
        private Color24BppRgb(int value)
            : this(
            value >> RedShift,
            value >> GreenShift,
            value >> BlueShift)
        {
        }

        /// <summary>
        /// Gets a <see cref="Color24BppRgb"/> that is black.
        /// </summary>
        public static ref readonly Color24BppRgb Empty
        {
            get
            {
                return ref _empty;
            }
        }

        /// <summary>
        /// Gets or sets the red intensity.
        /// </summary>
        public byte Red
        {
            get
            {
                return _red;
            }

            set
            {
                _red = value;
            }
        }

        /// <summary>
        /// Gets or sets the green intensity.
        /// </summary>
        public byte Green
        {
            get
            {
                return _green;
            }

            set
            {
                _green = value;
            }
        }

        /// <summary>
        /// Gets or sets the blue intensity.
        /// </summary>
        public byte Blue
        {
            get
            {
                return _blue;
            }

            set
            {
                _blue = value;
            }
        }

        /// <summary>
        /// Gets or sets the full 32-bit integer value representing this <see
        /// cref="Color24BppRgb"/>'s RGB components.
        /// </summary>
        public int Value
        {
            get
            {
                return
                    (Red << (BitsPerChannel * RedIndex)) |
                    (Green << (BitsPerChannel * GreenIndex)) |
                    (Blue << (BitsPerChannel * BlueIndex));
            }

            set
            {
                this = new Color24BppRgb(value);
            }
        }

        /// <summary>
        /// Creates a <see cref="Color24BppRgb"/> with a specified <see
        /// cref="Value"/> parameter.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Value"/> parameter to use for this <see
        /// cref="Color24BppRgb"/> instance.
        /// </param>
        /// <returns>
        /// A <see cref="Color24BppRgb"/> structure with a <see cref=" Value"/>
        /// equal to <paramref name="value"/>.
        /// </returns>
        public static explicit operator Color24BppRgb(int value)
        {
            return new Color24BppRgb(value);
        }

        /// <summary>
        /// Creates a 32-bit signed integer equal to a <see cref=" Value"/>
        /// parameter.
        /// </summary>
        /// <param name="color24">
        /// The <see cref="Color24BppRgb"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Int32"/> whose value is the <see cref="Value"/> of
        /// <paramref name="color24"/>.
        /// </returns>
        public static implicit operator int(Color24BppRgb color24)
        {
            return color24.Value;
        }

        /// <summary>
        /// Creates a <see cref="Color24BppRgb"/> structure whose <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties are equal to a <see cref=" Color32BppArgb"/>'s <see
        /// cref="Color32BppArgb.Red"/>, <see cref="Color32BppArgb.Green"/>,
        /// and <see cref=" Color32BppArgb.Blue"/> properties, respectively.
        /// </summary>
        /// <param name="color32">
        /// The <see cref="Color32BppArgb"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color24BppRgb"/> whose <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties are equal to a
        /// <see cref=" Color32BppArgb"/>'s <see cref="Color32BppArgb.Red"/>,
        /// <see cref="Color32BppArgb.Green"/>, and <see cref="
        /// Color32BppArgb.Blue"/> properties, respectively.
        /// </returns>
        public static explicit operator Color24BppRgb(
            Color32BppArgb color32)
        {
            return new Color24BppRgb(color32.Value);
        }

        /// <summary>
        /// Creates a <see cref="Color32BppArgb"/> structure whose <see
        /// cref="Color32BppArgb.Red"/>, <see cref="Color32BppArgb.Green "/>,
        /// and <see cref="Color32BppArgb.Blue"/> properties are equal to a
        /// <see cref="Color24BppRgb"/>'s <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref=" Blue"/> properties, respectively.
        /// The <see cref="Color32BppArgb.Alpha"/> is set to its default value.
        /// </summary>
        /// <param name="color24">
        /// The <see cref="Color24BppRgb"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color32BppArgb"/> whose <see
        /// cref="Color32BppArgb.Red"/>, <see cref="Color32BppArgb.Green "/>,
        /// and <see cref="Color32BppArgb.Blue"/> properties are equal to a
        /// <see cref="Color24BppRgb"/>'s <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref=" Blue"/> properties, respectively of
        /// <paramref name="color24"/>. The <see cref=" Color32BppArgb.Alpha"/>
        /// is set to its default value.
        /// </returns>
        public static implicit operator Color32BppArgb(
            Color24BppRgb color24)
        {
            return color24.Value;
        }

        /// <summary>
        /// Creates a <see cref="Color24BppRgb"/> structure whose <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties are equal to a <see cref=" Color"/>'s <see
        /// cref="Color.R"/>, <see cref="Color.G"/>, and <see cref="Color.B"/>
        /// properties, respectively.
        /// </summary>
        /// <param name="color">
        /// The <see cref="Color"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color24BppRgb"/> whose <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties are equal to a
        /// <see cref=" Color"/>'s <see cref="Color.R"/>, <see
        /// cref="Color.G"/>, and <see cref="Color.B"/> properties,
        /// respectively of <paramref name="color"/>.
        /// </returns>
        public static explicit operator Color24BppRgb(Color color)
        {
            return new Color24BppRgb(
                color.R,
                color.G,
                color.B);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> structure whose <see cref="
        /// Color.R"/>, <see cref="Color.G"/>, and <see cref="Color.B"/>
        /// properties are equal to a <see cref="Color24BppRgb"/>'s <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref=" Blue"/>
        /// properties, respectively. The <see cref=" Color.A"/> is set to its
        /// default value.
        /// </summary>
        /// <param name="color24">
        /// The <see cref="Color24BppRgb"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color"/> whose <see cref="Color.R"/>, <see
        /// cref="Color.G"/>, and <see cref="Color.B"/> properties are equal to
        /// a <see cref="Color24BppRgb"/>'s <see cref=" Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties, respectively of
        /// <paramref name="color24"/>. The <see cref="Color.A"/> is set to its
        /// default value.
        /// </returns>
        public static implicit operator Color(Color24BppRgb color24)
        {
            return Color.FromArgb(color24.Value);
        }

        /// <summary>
        /// Creates a <see cref="Color24BppRgb"/> structure whose <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties are equal to a <see cref=" ColorF"/>'s <see
        /// cref="ColorF.Red"/>, <see cref="ColorF. Green"/>, and <see
        /// cref="ColorF.Blue"/> properties, respectively.
        /// </summary>
        /// <param name="colorF">
        /// The <see cref="ColorF"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color24BppRgb"/> whose <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties are equal to a
        /// <see cref=" ColorF"/>'s <see cref="ColorF.Red"/>, <see
        /// cref="ColorF. Green"/>, and <see cref="ColorF.Blue"/> properties,
        /// respectively of <paramref name="colorF"/>.
        /// </returns>
        public static explicit operator Color24BppRgb(ColorF colorF)
        {
            return new Color24BppRgb(
                (int)Round(colorF.Red * Byte.MaxValue),
                (int)Round(colorF.Green * Byte.MaxValue),
                (int)Round(colorF.Blue * Byte.MaxValue));
        }

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure whose <see cref="
        /// ColorF.Red"/>, <see cref="ColorF.Green"/>, and <see cref="
        /// ColorF.Blue"/> properties are equal to a <see cref="
        /// Color24BppRgb"/>'s <see cref="Red"/>, <see cref="Green"/>, and <see
        /// cref="Blue"/> properties, respectively. The <see
        /// cref="ColorF.Alpha"/> is set to its default value.
        /// </summary>
        /// <param name="color24">
        /// The <see cref="Color24BppRgb"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="ColorF"/> whose <see cref=" ColorF.Red"/>, <see
        /// cref="ColorF.Green"/>, and <see cref=" ColorF.Blue"/> properties
        /// are equal to a <see cref=" Color24BppRgb"/>'s <see cref="Red"/>,
        /// <see cref="Green"/>, and <see cref="Blue"/> properties,
        /// respectively of <paramref name="color24"/>. The <see cref="
        /// ColorF.Alpha"/> is set to its default value.
        /// </returns>
        public static implicit operator ColorF(Color24BppRgb color24)
        {
            return ColorF.FromArgb(
                color24.Red / (float)Byte.MaxValue,
                color24.Green / (float)Byte.MaxValue,
                color24.Blue / (float)Byte.MaxValue);
        }

        /// <summary>
        /// Compares to <see cref="Color24BppRgb"/> values. The result
        /// specifies whether the values of the <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref=" Blue"/> properties of the two <see
        /// cref="Color24BppRgb"/> are equal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color24BppRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color24BppRgb"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> values of <paramref
        /// name="left"/> and <paramref name=" right"/> are equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator ==(
            Color24BppRgb left,
            Color24BppRgb right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares to <see cref="Color24BppRgb"/> values. The result
        /// specifies whether the values of the <see cref="Red"/>, <see
        /// cref="Green"/>, or <see cref=" Blue"/> properties of the two <see
        /// cref="Color24BppRgb"/> are unequal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color24BppRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color24BppRgb"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> values of <paramref
        /// name="left"/> or <paramref name=" right"/> are unequal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator !=(
            Color24BppRgb left,
            Color24BppRgb right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a
        /// specified <see cref="Color24BppRgb"/> value.
        /// </summary>
        /// <param name="obj">
        /// A <see cref="Color24BppRgb"/> value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> has the same value
        /// as this instance; otherwise, <see langword=" false"/>.
        /// </returns>
        public bool Equals(Color24BppRgb obj)
        {
            return
                Red.Equals(obj.Red) &&
                Green.Equals(obj.Green) &&
                Blue.Equals(obj.Blue);
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
        /// <see cref="Color24BppRgb"/> and equals the value of this instance;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Color24BppRgb value ? Equals(value) : false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return Value;
        }

        /// <summary>
        /// Converts the value of the current <see cref=" Color24BppRgb"/>
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
