// <copyright file="Color32BppArgb.cs" company="Public Domain">
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
    /// Represents a 32-bit pixel in ARGB 8 bit per channel format.
    /// </summary>
    /// <remarks>
    /// The purpose of this data type is to ease the process of bitmap drawing.
    /// For 32-bit ARGB bitmaps, its pixel buffer pointer can be cast to type
    /// <see cref="Color32BppArgb"/>* for simplified color operations.
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct Color32BppArgb :
        IEquatable<Color32BppArgb>
    {
        /// <summary>
        /// The size, in bytes, of the <see cref="Color32BppArgb"/> data type.
        /// </summary>
        public const int SizeOf = sizeof(int);

        /// <summary>
        /// The byte index of the <see cref="Alpha"/> component.
        /// </summary>
        public const int AlphaIndex = 3;

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
        /// Represent a <see cref="Color32BppArgb"/> that is black.
        /// </summary>
        public static readonly Color32BppArgb Empty = default;

        /// <summary>
        /// The number of bits that represent the intensity of the <see
        /// cref="Alpha"/> component.
        /// </summary>
        internal const int BitsPerAlpha = BitsPerChannel;

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
        /// The number of bits to shift left to reach the <see cref=" Alpha"/>
        /// component.
        /// </summary>
        private const int AlphaShift = BitsPerAlpha * AlphaIndex;

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
        /// The alpha intensity.
        /// </summary>
        [FieldOffset(AlphaIndex)]
        private byte _alpha;

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
        /// Initializes a new instance of the <see cref=" Color32BppArgb"/>
        /// struct with given red, green, and blue intensities. The alpha
        /// component is set to max, representing a fully opaque color.
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
        public Color32BppArgb(int red, int green, int blue)
            : this(Byte.MaxValue, red, green, blue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=" Color32BppArgb"/>
        /// struct with given alpha, red, green, and blue intensities.
        /// </summary>
        /// <param name="alpha">
        /// The alpha intensity.
        /// </param>
        /// <param name="red">
        /// The red intensity.
        /// </param>
        /// <param name="green">
        /// The green intensity.
        /// </param>
        /// <param name="blue">
        /// The blue intensity.
        /// </param>
        public Color32BppArgb(int alpha, int red, int green, int blue)
        {
            _alpha = (byte)alpha;
            _red = (byte)red;
            _green = (byte)green;
            _blue = (byte)blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=" Color32BppArgb"/>
        /// struct from a 32-bit integer value.
        /// </summary>
        /// <param name="value">
        /// The 32-bit ARGB value.
        /// </param>
        private Color32BppArgb(int value)
            : this(
            value >> AlphaShift,
            value >> RedShift,
            value >> GreenShift,
            value >> BlueShift)
        {
        }

        /// <summary>
        /// Gets or sets the alpha intensity.
        /// </summary>
        public byte Alpha
        {
            get
            {
                return _alpha;
            }

            set
            {
                _alpha = value;
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
        /// cref="Color32BppArgb"/>'s ARGB components.
        /// </summary>
        public int Value
        {
            get
            {
                return
                    (Alpha << (BitsPerChannel * AlphaIndex)) |
                    (Red << (BitsPerChannel * RedIndex)) |
                    (Green << (BitsPerChannel * GreenIndex)) |
                    (Blue << (BitsPerChannel * BlueIndex));
            }

            set
            {
                this = new Color32BppArgb(value);
            }
        }

        /// <summary>
        /// Creates a <see cref="Color32BppArgb"/> with a specified <see
        /// cref="Value"/> parameter.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Value"/> parameter to use for this <see
        /// cref="Color32BppArgb"/> instance.
        /// </param>
        /// <returns>
        /// A <see cref="Color32BppArgb"/> structure with a <see cref="
        /// Value"/> equal to <paramref name="value"/>.
        /// </returns>
        public static implicit operator Color32BppArgb(int value)
        {
            return new Color32BppArgb(value);
        }

        /// <summary>
        /// Creates a 32-bit signed integer equal to a <see cref=" Value"/>
        /// parameter.
        /// </summary>
        /// <param name="color32">
        /// The <see cref="Color32BppArgb"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Int32"/> whose value is the <see cref="Value"/> of
        /// <paramref name="color32"/>.
        /// </returns>
        public static implicit operator int(Color32BppArgb color32)
        {
            return color32.Value;
        }

        /// <summary>
        /// Creates a <see cref="Color32BppArgb"/> structure whose <see
        /// cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, and <see
        /// cref="Blue"/> properties are equal to a <see cref=" Color"/>'s <see
        /// cref="Color.A"/>, <see cref="Color.R"/>, <see cref="Color.G"/>, and
        /// <see cref="Color.B"/> properties, respectively.
        /// </summary>
        /// <param name="color">
        /// The <see cref="Color"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color32BppArgb"/> whose <see cref="Alpha"/>, <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties are equal to a <see cref=" Color"/>'s <see
        /// cref="Color.A"/>, <see cref="Color.R"/>, <see cref="Color.G"/>, and
        /// <see cref="Color.B"/> properties, respectively of <paramref
        /// name="color"/>.
        /// </returns>
        public static explicit operator Color32BppArgb(Color color)
        {
            return new Color32BppArgb(
                color.A,
                color.R,
                color.G,
                color.B);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> structure whose <see cref="
        /// Color.A"/>, <see cref="Color.R"/>, <see cref="Color.G"/>, and <see
        /// cref="Color.B"/> properties are equal to a <see
        /// cref="Color32BppArgb"/>'s <see cref="Alpha"/>, <see cref=" Red"/>,
        /// <see cref="Green"/>, and <see cref="Blue"/> properties,
        /// respectively.
        /// </summary>
        /// <param name="color32">
        /// The <see cref="Color32BppArgb"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color"/> whose <see cref=" Color.A"/>, <see
        /// cref="Color.R"/>, <see cref="Color.G"/>, and <see cref="Color.B"/>
        /// properties are equal to a <see cref="Color32BppArgb"/>'s <see
        /// cref="Alpha"/>, <see cref=" Red"/>, <see cref="Green"/>, and <see
        /// cref="Blue"/> properties, respectively of <paramref
        /// name="color32"/>.
        /// </returns>
        public static implicit operator Color(Color32BppArgb color32)
        {
            return Color.FromArgb(color32.Value);
        }

        /// <summary>
        /// Creates a <see cref="Color32BppArgb"/> structure whose <see
        /// cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, and <see
        /// cref="Blue"/> properties are equal to a <see cref=" ColorF"/>'s
        /// <see cref="ColorF.Alpha"/>, <see cref="ColorF. Red"/>, <see
        /// cref="ColorF.Green"/>, and <see cref="ColorF. Blue"/> properties,
        /// respectively.
        /// </summary>
        /// <param name="colorF">
        /// The <see cref="ColorF"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color32BppArgb"/> whose <see cref="Alpha"/>, <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties are equal to a <see cref=" ColorF"/>'s <see
        /// cref="ColorF.Alpha"/>, <see cref="ColorF. Red"/>, <see
        /// cref="ColorF.Green"/>, and <see cref="ColorF. Blue"/> properties,
        /// respectively of <paramref name=" colorF"/>.
        /// </returns>
        public static explicit operator Color32BppArgb(ColorF colorF)
        {
            return new Color32BppArgb(
                (int)Round(colorF.Alpha * Byte.MaxValue),
                (int)Round(colorF.Red * Byte.MaxValue),
                (int)Round(colorF.Green * Byte.MaxValue),
                (int)Round(colorF.Blue * Byte.MaxValue));
        }

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure whose <see cref="
        /// ColorF.Alpha"/>, <see cref="ColorF.Red"/>, <see cref="
        /// ColorF.Green"/>, and <see cref="ColorF.Blue"/> properties are equal
        /// to a <see cref="Color32BppArgb"/>'s <see cref=" Alpha"/>, <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties, respectively.
        /// </summary>
        /// <param name="color32">
        /// The <see cref="Color32BppArgb"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="ColorF"/> whose <see cref=" ColorF.Alpha"/>, <see
        /// cref="ColorF.Red"/>, <see cref=" ColorF.Green"/>, and <see
        /// cref="ColorF.Blue"/> properties are equal to a <see
        /// cref="Color32BppArgb"/>'s <see cref=" Alpha"/>, <see cref="Red"/>,
        /// <see cref="Green"/>, and <see cref="Blue"/> properties,
        /// respectively of <paramref name=" color32"/>.
        /// </returns>
        public static implicit operator ColorF(Color32BppArgb color32)
        {
            return ColorF.FromArgb(
                color32.Alpha / (float)Byte.MaxValue,
                color32.Red / (float)Byte.MaxValue,
                color32.Green / (float)Byte.MaxValue,
                color32.Blue / (float)Byte.MaxValue);
        }

        /// <summary>
        /// Compares to <see cref="Color32BppArgb"/> values. The result
        /// specifies whether the values of the <see cref="Alpha"/>, <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref=" Blue"/>
        /// properties of the two <see cref="Color32BppArgb"/> are equal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color32BppArgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color32BppArgb"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Alpha"/>, <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/> values of
        /// <paramref name="left"/> and <paramref name=" right"/> are equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(
            Color32BppArgb left,
            Color32BppArgb right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares to <see cref="Color32BppArgb"/> values. The result
        /// specifies whether the values of the <see cref="Alpha"/>, <see
        /// cref="Red"/>, <see cref="Green"/>, or <see cref=" Blue"/>
        /// properties of the two <see cref="Color32BppArgb"/> are unequal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color32BppArgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color32BppArgb"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Alpha"/>, <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/> values of
        /// <paramref name="left"/> or <paramref name=" right"/> are unequal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(
            Color32BppArgb left,
            Color32BppArgb right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a
        /// specified <see cref="Color32BppArgb"/> value.
        /// </summary>
        /// <param name="obj">
        /// A <see cref="Color32BppArgb"/> value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> has the same value
        /// as this instance; otherwise, <see langword=" false"/>.
        /// </returns>
        public bool Equals(Color32BppArgb obj)
        {
            return Value.Equals(obj.Value);
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
        /// <see cref="Color32BppArgb"/> and equals the value of this instance;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Color32BppArgb value ? Equals(value) : false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Converts the value of the current <see cref=" Color32BppArgb"/>
        /// object to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the value of this object.
        /// </returns>
        public override string ToString()
        {
            return GetString(
                "{{A:{0},R:{1},G:{2},B:{3}}}",
                Alpha,
                Red,
                Green,
                Blue);
        }
    }
}
