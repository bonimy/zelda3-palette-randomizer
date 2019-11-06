// <copyright file="ColorF.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using static System.Diagnostics.Debug;
    using static System.Math;
    using static MathHelper;
    using static StringHelper;
    using static ThrowHelper;

    /// <summary>
    /// Represents a color using floating point ARGB values with valid ranges
    /// from 0 to 1 inclusive.
    /// </summary>
    /// <remarks>
    /// This structure provides a vastly superior set of color manipulation
    /// algorithms over <see cref="Color"/> such as color blending, and
    /// creating instances from color spaces other than ARGB, such as CMYK and
    /// hue, saturation, luminosity.
    /// </remarks>
    public partial struct ColorF : IEquatable<ColorF>
    {
        /// <summary>
        /// The influence of <see cref="Red"/> intensity when calculating <see
        /// cref="Luma"/> intensity.
        /// </summary>
        public const float LumaRedWeight = 0.299f;

        public const float LumaWeight = LumaRedWeight + LumaGreenWeight + LumaBlueWeight;

        /// <summary>
        /// The influence of <see cref="Green"/> intensity when calculating
        /// <see cref="Luma"/> intensity.
        /// </summary>
        public const float LumaGreenWeight = 0.587f;

        /// <summary>
        /// The influence of <see cref="Blue"/> intensity when calculating
        /// <see cref="Luma"/> intensity.
        /// </summary>
        public const float LumaBlueWeight = 0.114f;

        /// <summary>
        /// The total number of channels (alpha, red, green blue) that
        /// comprise a <see cref="ColorF"/> color.
        /// </summary>
        public const int NumberOfChannels = 4;

        /// <summary>
        /// The total number of color channels (red, green, blue) that
        /// comprise a <see cref="ColorF"/> color.
        /// </summary>
        public const int NumberOfColorChannels = NumberOfChannels - 1;

        /// <summary>
        /// Represents a <see cref="ColorF"/> that is black.
        /// </summary>
        public static readonly ColorF Empty = default;

        private const float GrayscaleWeight = 1f / NumberOfColorChannels;

        /// <summary>
        /// A dictionary of <see cref="ColorBlendCallback"/> delegate values
        /// keys by <see cref="BlendMode"/>.
        /// </summary>
        private static readonly IReadOnlyDictionary
            <BlendMode, ColorBlendCallback> BlendDictionary =
            new ReadOnlyDictionary<BlendMode, ColorBlendCallback>(
                new Dictionary<BlendMode, ColorBlendCallback>()
                {
                    { BlendMode.Alpha, AlphaBlend },
                    { BlendMode.Grayscale, Grayscale },
                    { BlendMode.Multiply, Multiply },
                    { BlendMode.Screen, Screen },
                    { BlendMode.Overlay, Overlay },
                    { BlendMode.HardLight, HardLight },
                    { BlendMode.SoftLight, SoftLight },
                    { BlendMode.ColorDodge, ColorDodge },
                    { BlendMode.LinearDodge, LinearDodge },
                    { BlendMode.ColorBurn, ColorBurn },
                    { BlendMode.LinearBurn, LinearBurn },
                    { BlendMode.VividLight, VividLight },
                    { BlendMode.LinearLight, LinearLight },
                    { BlendMode.Difference, Difference },
                    { BlendMode.Darken, Darken },
                    { BlendMode.Lighten, Lighten },
                    { BlendMode.DarkerColor, DarkerColor },
                    { BlendMode.LighterColor, LighterColor },
                    { BlendMode.Hue, HueBlend },
                    { BlendMode.Saturation, SaturationBlend },
                    { BlendMode.Luminosity, LuminosityBlend },
                    { BlendMode.Divide, Divide },
                });

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorF"/> struct with
        /// given alpha, red, green, and blue parameters.
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
        /// <remarks>
        /// If <paramref name="alpha"/>, <paramref name="red"/>, <paramref
        /// name="green"/>, or <paramref name="blue"/> are less than 0 or
        /// greater than 1, then that value is clamped to 0 or 1 using <see
        /// cref="Clamp(Single, Single, Single, Single)"/>.
        /// <para/>
        /// If <paramref name="alpha"/>, <paramref name="red"/>, <paramref
        /// name="green"/>, or <paramref name="blue"/> are <see
        /// cref="Single.NaN"/>, an <see cref="Assert(Boolean, String)"/>
        /// failure is called whose message states which color component is
        /// <see cref="Single.NaN"/>.
        /// </remarks>
        private ColorF(float alpha, float red, float green, float blue)
        {
            Assert(!Single.IsNaN(alpha), "Invalid alpha value");
            Assert(!Single.IsNaN(red), "Invalid red value");
            Assert(!Single.IsNaN(green), "Invalid green value");
            Assert(!Single.IsNaN(blue), "Invalid blue value");

            Alpha = Clamp(alpha, 0, 1);
            Red = Clamp(red, 0, 1);
            Green = Clamp(green, 0, 1);
            Blue = Clamp(blue, 0, 1);
        }

        /// <summary>
        /// Gets the alpha intensity of this <see cref="ColorF"/> instance.
        /// </summary>
        public float Alpha
        {
            get;
        }

        /// <summary>
        /// Gets the red intensity of this <see cref="ColorF"/> instance.
        /// </summary>
        public float Red
        {
            get;
        }

        /// <summary>
        /// Gets the green intensity of this <see cref="ColorF"/> instance.
        /// </summary>
        public float Green
        {
            get;
        }

        /// <summary>
        /// Gets the blue intensity of this <see cref="ColorF"/> instance.
        /// </summary>
        public float Blue
        {
            get;
        }

        /// <summary>
        /// Gets the cyan intensity of this <see cref="ColorF"/> instance.
        /// </summary>
        public float Cyan
        {
            get
            {
                return 1 - Red;
            }
        }

        /// <summary>
        /// Gets the magenta intensity of this <see cref="ColorF"/> instance.
        /// </summary>
        public float Magenta
        {
            get
            {
                return 1 - Green;
            }
        }

        /// <summary>
        /// Gets the yellow intensity of this <see cref="ColorF"/> instance.
        /// </summary>
        public float Yellow
        {
            get
            {
                return 1 - Blue;
            }
        }

        /// <summary>
        /// Gets the maximum value of <see cref="Red"/>, <see cref=" Green"/>,
        /// and <see cref="Blue"/>.
        /// </summary>
        public float Max
        {
            get
            {
                return Max(Red, Green, Blue);
            }
        }

        /// <summary>
        /// Gets the minimum value of <see cref="Red"/>, <see cref=" Green"/>,
        /// and <see cref="Blue"/>.
        /// </summary>
        public float Min
        {
            get
            {
                return Min(Red, Green, Blue);
            }
        }

        /// <summary>
        /// Gets the hue of this <see cref="ColorF"/> instance.
        /// </summary>
        /// <remarks>
        /// Valid values are from 0 inclusive to 1 exclusive. A value of 0
        /// means pure red, 1/3 is pure green, and 2/3 is pure blue.
        /// </remarks>
        public float Hue
        {
            get
            {
                // When chroma is zero, there is no hue, as the color is not
                // any more red, green, or blue. Technically, NaN should be
                // returned, but this causes a lot of exceptional
                // circumstances that need to be checked, especially in
                // blending. So we just say the hue is vacuously zero. This
                // doesn't hurt the structure in any way and lets us make
                // assumptions during other calculations.
                if (Chroma == 0)
                {
                    return 0;
                }

                var hue = 0f;
                if (Max == Red)
                {
                    hue = (Green - Blue) / Chroma;
                    if (hue < 0)
                    {
                        hue += 6;
                    }
                }
                else if (Max == Green)
                {
                    hue = ((Blue - Red) / Chroma) + 2;
                }
                else if (Max == Blue)
                {
                    hue = ((Red - Green) / Chroma) + 4;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                return hue / 6.0f;
            }
        }

        /// <summary>
        /// Gets <see cref="Hue"/> in measure of degrees from 0 inclusive to
        /// 360 exclusive.
        /// </summary>
        public float HueDegrees
        {
            get
            {
                return 360 * Hue;
            }
        }

        /// <summary>
        /// Gets the saturation of this <see cref="ColorF"/> instance.
        /// </summary>
        public float Saturation
        {
            get
            {
                // Like hue, saturation should be NaN if chroma is zero, but
                // we just say it's zero with no penalty.
                return Lightness != 1 && Lightness != 0
                    ? Clamp(Chroma / (1 - Abs((2 * Lightness) - 1)), 0, 1)
                    : 0;
            }
        }

        /// <summary>
        /// Gets the lightness of this <see cref="ColorF"/> instance.
        /// </summary>
        public float Lightness
        {
            get
            {
                return (Max + Min) / 2;
            }
        }

        /// <summary>
        /// Gets the luma of this <see cref="ColorF"/> instance.
        /// </summary>
        public float Luma
        {
            get
            {
                return
                    (LumaRedWeight * Red) +
                    (LumaGreenWeight * Green) +
                    (LumaBlueWeight * Blue);
            }
        }

        /// <summary>
        /// Gets the chroma of this <see cref="ColorF"/> instance.
        /// </summary>
        public float Chroma
        {
            get
            {
                return Max - Min;
            }
        }

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure whose <see
        /// cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, and <see
        /// cref="Blue"/> properties are equal to a <see cref=" Color"/>'s
        /// <see cref="Color.A"/>, <see cref="Color.R"/>, <see
        /// cref="Color.G"/>, and <see cref="Color.B"/> properties,
        /// respectively.
        /// </summary>
        /// <param name="color">
        /// The <see cref="Color"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="ColorF"/> whose <see cref="Alpha"/>, <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties are equal to a <see cref=" Color"/>'s <see
        /// cref="Color.A"/>, <see cref="Color.R"/>, <see cref="Color.G"/>,
        /// and <see cref="Color.B"/> properties, respectively of <paramref
        /// name="color"/>.
        /// </returns>
        public static explicit operator ColorF(Color color)
        {
            // This is an explicit operator because we lose information if
            // parameter color is a known/named color.
            return FromArgb(
                color.A / (float)Byte.MaxValue,
                color.R / (float)Byte.MaxValue,
                color.G / (float)Byte.MaxValue,
                color.B / (float)Byte.MaxValue);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> structure whose <see cref="
        /// Color.A"/>, <see cref="Color.R"/>, <see cref="Color.G"/>, and <see
        /// cref="Color.B"/> properties are equal to a <see cref="Color"/>'s
        /// <see cref="Alpha"/>, <see cref=" Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> properties, respectively.
        /// </summary>
        /// <param name="colorF">
        /// The <see cref="ColorF"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Color"/> whose <see cref=" Color.A"/>, <see
        /// cref="Color.R"/>, <see cref="Color.G"/>, and <see cref="Color.B"/>
        /// properties are equal to a <see cref="ColorF"/>'s <see
        /// cref="Alpha"/>, <see cref=" Red"/>, <see cref="Green"/>, and <see
        /// cref="Blue"/> properties, respectively of <paramref
        /// name="colorF"/>.
        /// </returns>
        public static explicit operator Color(ColorF colorF)
        {
            return Color.FromArgb(
                (int)Round(colorF.Alpha * Byte.MaxValue),
                (int)Round(colorF.Red * Byte.MaxValue),
                (int)Round(colorF.Green * Byte.MaxValue),
                (int)Round(colorF.Blue * Byte.MaxValue));
        }

        /// <summary>
        /// Performs the <see cref="Add(ColorF, ColorF)"/> blend operation of
        /// two <see cref="ColorF"/> values.
        /// </summary>
        /// <param name="left">
        /// The top layer <see cref="ColorF"/>.
        /// </param>
        /// <param name="right">
        /// The bottom layer <see cref="ColorF"/> to add to <paramref
        /// name="left"/>.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> that is the result of the <see
        /// cref="Add(ColorF, ColorF)"/> blend operation of <paramref
        /// name="left"/> and <paramref name="right"/>.
        /// </returns>
        public static ColorF operator +(ColorF left, ColorF right)
        {
            return Add(left, right);
        }

        /// <summary>
        /// Performs the <see cref="Subtract(ColorF, ColorF)"/> blend
        /// operation of one <see cref="ColorF"/> from another.
        /// </summary>
        /// <param name="left">
        /// The top layer <see cref="ColorF"/>.
        /// </param>
        /// <param name="right">
        /// The bottom layer <see cref="ColorF"/> to subtract from <paramref
        /// name="left"/>.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> that is the result of the <see
        /// cref="Subtract(ColorF, ColorF)"/> blend operation of <paramref
        /// name="right"/> from <paramref name="left"/>.
        /// </returns>
        public static ColorF operator -(ColorF left, ColorF right)
        {
            return Subtract(left, right);
        }

        /// <summary>
        /// Performs the <see cref="Multiply(ColorF, ColorF)"/> blend
        /// operation of two <see cref="ColorF"/> values.
        /// </summary>
        /// <param name="left">
        /// The top layer <see cref="ColorF"/>.
        /// </param>
        /// <param name="right">
        /// The bottom layer <see cref="ColorF"/> to multiply to <paramref
        /// name="left"/>.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> that is the result of the <see
        /// cref="Multiply(ColorF, ColorF)"/> blend operation of <paramref
        /// name="left"/> and <paramref name="right"/>.
        /// </returns>
        public static ColorF operator *(ColorF left, ColorF right)
        {
            return Multiply(left, right);
        }

        /// <summary>
        /// Performs the <see cref="Divide(ColorF, ColorF)"/> blend operation
        /// of one <see cref="ColorF"/> into another.
        /// </summary>
        /// <param name="left">
        /// The top layer <see cref="ColorF"/>.
        /// </param>
        /// <param name="right">
        /// The bottom layer <see cref="ColorF"/> to divide into <paramref
        /// name="left"/>.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> that is the result of the <see
        /// cref="Divide(ColorF, ColorF)"/> blend operation of <paramref
        /// name="right"/> into <paramref name="left"/>.
        /// </returns>
        public static ColorF operator /(ColorF left, ColorF right)
        {
            return Divide(left, right);
        }

        /// <summary>
        /// Compares to <see cref="ColorF"/> values. The result specifies
        /// whether the values of the <see cref="Alpha"/>, <see cref="Red"/>,
        /// <see cref="Green"/>, and <see cref=" Blue"/> properties of the two
        /// <see cref="ColorF"/> are equal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorF"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorF"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Alpha"/>, <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/> values
        /// of <paramref name="left"/> and <paramref name=" right"/> are
        /// equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(ColorF left, ColorF right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares to <see cref="ColorF"/> values. The result specifies
        /// whether the values of the <see cref="Alpha"/>, <see cref="Red"/>,
        /// <see cref="Green"/>, or <see cref=" Blue"/> properties of the two
        /// <see cref="ColorF"/> are unequal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorF"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorF"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Alpha"/>, <see
        /// cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/> values
        /// of <paramref name="left"/> or <paramref name=" right"/> are
        /// unequal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(ColorF left, ColorF right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Performs the <see cref="Negate(ColorF)"/> operation of a <see
        /// cref="ColorF"/>.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorF"/> to negate.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> that is the result of the <see
        /// cref="Negate(ColorF)"/> blend operation on <paramref
        /// name="color"/>.
        /// </returns>
        public static ColorF operator ~(ColorF color)
        {
            return Negate(color);
        }

        /// <summary>
        /// Perform the <see cref="Invert"/> operation on a <see
        /// cref="ColorF"/> instance.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorF"/> to invert.
        /// </param>
        /// <returns>
        /// The result of <see cref="Invert"/> of <paramref name=" color"/>.
        /// </returns>
        public static ColorF Negate(ColorF color)
        {
            return color.Invert();
        }

        /// <summary>
        /// Add the RGB color channels of two <see cref="ColorF"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorF"/> instance.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorF"/> instance.
        /// </param>
        /// <returns>
        /// A <see cref="ColorF"/> instance whose <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties are the
        /// restricted sums of the respective components of <paramref
        /// name="left"/> and <paramref name="right"/>. The <see
        /// cref="Alpha"/> is a standard alpha blend of <paramref
        /// name="left"/> and <paramref name="right"/>.
        /// </returns>
        public static ColorF Add(ColorF left, ColorF right)
        {
            return LinearDodge(left, right);
        }

        /// <summary>
        /// Subtracts the RGB color channels of a <see cref="ColorF"/> values
        /// from another.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorF"/> instance.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorF"/> instance.
        /// </param>
        /// <returns>
        /// A <see cref="ColorF"/> instance whose <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties are the
        /// restricted differences of the respective components of <paramref
        /// name="right"/> from <paramref name="left"/>. The <see
        /// cref="Alpha"/> is a standard alpha blend of <paramref
        /// name="left"/> and <paramref name="right"/>.
        /// </returns>
        public static ColorF Subtract(ColorF left, ColorF right)
        {
            return Difference(left, right);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by an RGB
        /// color space and is fully opaque.
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
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties are <paramref
        /// name="red"/>, <paramref name="green"/>, and <paramref
        /// name="blue"/>, respectively and that is fully opaque.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="red"/>, <paramref name="green"/>, or <paramref
        /// name="blue"/> is equal to <see cref="Single. NaN"/>.
        /// </exception>
        public static ColorF FromArgb(float red, float green, float blue)
        {
            return FromArgb(1, red, green, blue);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> that is equal to
        /// another instance but with a different <see cref="Alpha"/>
        /// component.
        /// </summary>
        /// <param name="alpha">
        /// The new <see cref="Alpha"/> value to assign.
        /// </param>
        /// <param name="color">
        /// The <see cref="ColorF"/> value to copy the RGB values from.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="color"/> whose <see cref="Alpha"/>
        /// component is equal to <paramref name="alpha"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="alpha"/> is equal to <see cref="Single. NaN"/>.
        /// </exception>
        public static ColorF FromArgb(float alpha, ColorF color)
        {
            return FromArgb(
                alpha,
                color.Red,
                color.Green,
                color.Blue);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by an ARGB
        /// color space.
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
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref=" Alpha"/>,
        /// <see cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/>
        /// properties are <paramref name="alpha"/>, <paramref name="red"/>,
        /// <paramref name="green"/>, and <paramref name="blue"/>,
        /// respectively.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="alpha"/>, <paramref name="red"/>, <paramref
        /// name="green"/>, or <paramref name="blue"/> is equal to <see
        /// cref="Single.NaN"/>.
        /// </exception>
        public static ColorF FromArgb(
            float alpha,
            float red,
            float green,
            float blue)
        {
            if (Single.IsNaN(alpha))
            {
                throw ValueIsNaN(nameof(alpha));
            }

            if (Single.IsNaN(red))
            {
                throw ValueIsNaN(nameof(red));
            }

            if (Single.IsNaN(green))
            {
                throw ValueIsNaN(nameof(green));
            }

            if (Single.IsNaN(blue))
            {
                throw ValueIsNaN(nameof(blue));
            }

            return new ColorF(alpha, red, green, blue);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by a CMY color
        /// space and is fully opaque.
        /// </summary>
        /// <param name="cyan">
        /// The cyan intensity.
        /// </param>
        /// <param name="magenta">
        /// The magenta intensity.
        /// </param>
        /// <param name="yellow">
        /// The yellow intensity.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref="Cyan"/>, <see
        /// cref="Magenta"/>, and <see cref="Yellow"/> properties are
        /// <paramref name=" cyan"/>, <paramref name="magenta"/>, and
        /// <paramref name="yellow"/>, respectively and that is fully opaque.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="cyan"/>, <paramref name="magenta"/>, or <paramref
        /// name="yellow"/> is equal to <see cref="Single. NaN"/>.
        /// </exception>
        public static ColorF FromCmy(
            float cyan,
            float magenta,
            float yellow)
        {
            return FromCmy(1, cyan, magenta, yellow);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by an ACMY
        /// color space.
        /// </summary>
        /// <param name="alpha">
        /// The alpha intensity.
        /// </param>
        /// <param name="cyan">
        /// The cyan intensity.
        /// </param>
        /// <param name="magenta">
        /// The magenta intensity.
        /// </param>
        /// <param name="yellow">
        /// The yellow intensity.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref=" Alpha"/>,
        /// <see cref="Cyan"/>, <see cref="Magenta"/>, and <see
        /// cref="Yellow"/> properties are <paramref name="alpha"/>, <paramref
        /// name="cyan"/>, <paramref name="magenta"/>, and <paramref
        /// name="yellow"/>, respectively.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="alpha"/>, <paramref name="cyan"/>, <paramref
        /// name="magenta"/>, or <paramref name="yellow"/> is equal to <see
        /// cref="Single.NaN"/>.
        /// </exception>
        public static ColorF FromCmy(
            float alpha,
            float cyan,
            float magenta,
            float yellow)
        {
            if (Single.IsNaN(alpha))
            {
                throw ValueIsNaN(nameof(alpha));
            }

            if (Single.IsNaN(cyan))
            {
                throw ValueIsNaN(nameof(cyan));
            }

            if (Single.IsNaN(magenta))
            {
                throw ValueIsNaN(nameof(magenta));
            }

            if (Single.IsNaN(yellow))
            {
                throw ValueIsNaN(nameof(yellow));
            }

            var red = 1 - cyan;
            var green = 1 - magenta;
            var blue = 1 - yellow;

            return new ColorF(alpha, red, green, blue);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by a CMYK
        /// color space and is fully opaque.
        /// </summary>
        /// <param name="cyan">
        /// The cyan intensity.
        /// </param>
        /// <param name="magenta">
        /// The magenta intensity.
        /// </param>
        /// <param name="yellow">
        /// The yellow intensity.
        /// </param>
        /// <param name="black">
        /// The black intensity.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref="Cyan"/>, <see
        /// cref="Magenta"/>, and <see cref="Yellow"/> properties are
        /// <paramref name="cyan"/>, <paramref name="magenta"/>, and <paramref
        /// name="yellow"/>, respectively scaled by amount <paramref
        /// name="black"/> and that is fully opaque.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="cyan"/>, <paramref name="magenta"/>, <paramref
        /// name="yellow"/>, or <paramref name="black"/> is equal to <see
        /// cref="Single.NaN"/>.
        /// </exception>
        public static ColorF FromCmyk(
            float cyan,
            float magenta,
            float yellow,
            float black)
        {
            return FromCmyk(1, cyan, magenta, yellow, black);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by an ACMYK
        /// color space and is fully opaque.
        /// </summary>
        /// <param name="alpha">
        /// The alpha intensity.
        /// </param>
        /// <param name="cyan">
        /// The cyan intensity.
        /// </param>
        /// <param name="magenta">
        /// The magenta intensity.
        /// </param>
        /// <param name="yellow">
        /// The yellow intensity.
        /// </param>
        /// <param name="black">
        /// The black intensity.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref="Cyan"/>, <see
        /// cref="Magenta"/>, and <see cref="Yellow"/> properties are
        /// <paramref name="cyan"/>, <paramref name="magenta"/>, and <paramref
        /// name="yellow"/>, respectively scaled by amount <paramref
        /// name="black"/> and that is fully opaque. The <see cref="Alpha"/>
        /// component is equal to <paramref name="alpha"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="alpha"/>, <paramref name="cyan"/>, <paramref
        /// name="magenta"/>, <paramref name="yellow"/>, or <paramref
        /// name="black"/> is equal to <see cref="Single.NaN"/>.
        /// </exception>
        public static ColorF FromCmyk(
            float alpha,
            float cyan,
            float magenta,
            float yellow,
            float black)
        {
            if (Single.IsNaN(alpha))
            {
                throw ValueIsNaN(nameof(alpha));
            }

            if (Single.IsNaN(cyan))
            {
                throw ValueIsNaN(nameof(cyan));
            }

            if (Single.IsNaN(magenta))
            {
                throw ValueIsNaN(nameof(magenta));
            }

            if (Single.IsNaN(yellow))
            {
                throw ValueIsNaN(nameof(yellow));
            }

            if (Single.IsNaN(black))
            {
                throw ValueIsNaN(nameof(black));
            }

            var white = 1 - black;
            var red = white * (1 - cyan);
            var green = white * (1 - magenta);
            var blue = white * (1 - yellow);

            return new ColorF(alpha, red, green, blue);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by an HCY
        /// color space and is fully opaque.
        /// </summary>
        /// <param name="hue">
        /// The <see cref="ColorF"/>'s hue.
        /// </param>
        /// <param name="chroma">
        /// The chromaticity value.
        /// </param>
        /// <param name="luma">
        /// The luma value.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref="Hue"/>, <see
        /// cref="Chroma"/>, and <see cref="Luma"/> properties are <paramref
        /// name=" hue"/>, <paramref name="chroma"/>, and <paramref
        /// name="luma"/>, respectively and that is fully opaque.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="hue"/>, <paramref name="chroma"/>, or <paramref
        /// name="luma"/> is equal to <see cref="Single. NaN"/>.
        /// </exception>
        public static ColorF FromHcy(
            float hue,
            float chroma,
            float luma)
        {
            return FromHcy(1, hue, chroma, luma);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by an AHCY
        /// color space.
        /// </summary>
        /// <param name="alpha">
        /// The alpha intensity.
        /// </param>
        /// <param name="hue">
        /// The <see cref="ColorF"/>'s hue.
        /// </param>
        /// <param name="chroma">
        /// The chromaticity value.
        /// </param>
        /// <param name="luma">
        /// The luma value.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref=" Alpha"/>,
        /// <see cref="Hue"/>, <see cref="Chroma"/>, and <see cref="Luma"/>
        /// properties are <paramref name="alpha"/>, <paramref name="hue"/>,
        /// <paramref name="chroma"/>, and <paramref name="luma"/>,
        /// respectively.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="alpha"/>, <paramref name="hue"/>, <paramref
        /// name="chroma"/>, or <paramref name="luma"/> is equal to <see
        /// cref="Single.NaN"/>.
        /// </exception>
        public static ColorF FromHcy(
            float alpha,
            float hue,
            float chroma,
            float luma)
        {
            if (Single.IsNaN(alpha))
            {
                throw ValueIsNaN(nameof(alpha));
            }

            if (Single.IsNaN(hue))
            {
                throw ValueIsNaN(nameof(hue));
            }

            if (Single.IsNaN(chroma))
            {
                throw ValueIsNaN(nameof(chroma));
            }

            if (Single.IsNaN(luma))
            {
                throw ValueIsNaN(nameof(luma));
            }

            chroma = Clamp(chroma, 0, 1);
            luma = Clamp(luma, 0, 1);
            var (r, g, b) = GetBaseRgb(hue, chroma);

            var lumaR = r * LumaRedWeight;
            var lumaG = g * LumaGreenWeight;
            var lumaB = b * LumaBlueWeight;
            var baseLuma = lumaR + lumaG + lumaB;
            var min = Clamp(luma - baseLuma, 0, 1);
            return new ColorF(alpha, min + r, min + g, min + b);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by an HSL
        /// color space and is fully opaque.
        /// </summary>
        /// <param name="hue">
        /// The <see cref="ColorF"/>'s hue.
        /// </param>
        /// <param name="saturation">
        /// The saturation value.
        /// </param>
        /// <param name="lightness">
        /// The lightness value.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref="Hue"/>, <see
        /// cref="Saturation"/>, and <see cref="Lightness"/> properties are
        /// <paramref name=" hue"/>, <paramref name="saturation"/>, and
        /// <paramref name="lightness"/>, respectively and that is fully
        /// opaque.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="hue"/>, <paramref name="saturation"/>, or
        /// <paramref name="lightness"/> is equal to <see cref="Single.
        /// NaN"/>.
        /// </exception>
        public static ColorF FromHsl(
            float hue,
            float saturation,
            float lightness)
        {
            return FromHsl(1, hue, saturation, lightness);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by an AHSL
        /// color space.
        /// </summary>
        /// <param name="alpha">
        /// The alpha intensity.
        /// </param>
        /// <param name="hue">
        /// The <see cref="ColorF"/>'s hue.
        /// </param>
        /// <param name="saturation">
        /// The saturation value.
        /// </param>
        /// <param name="lightness">
        /// The lightness value.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref=" Alpha"/>,
        /// <see cref="Hue"/>, <see cref="Saturation"/>, and <see
        /// cref="Lightness"/> properties are <paramref name=" alpha"/>,
        /// <paramref name="hue"/>, <paramref name=" saturation"/>, and
        /// <paramref name="lightness"/>, respectively.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="alpha"/>, <paramref name="hue"/>, <paramref
        /// name="saturation"/>, or <paramref name=" lightness"/> is equal to
        /// <see cref="Single.NaN"/>.
        /// </exception>
        public static ColorF FromHsl(
            float alpha,
            float hue,
            float saturation,
            float lightness)
        {
            if (Single.IsNaN(alpha))
            {
                throw ValueIsNaN(nameof(alpha));
            }

            if (Single.IsNaN(hue))
            {
                throw ValueIsNaN(nameof(hue));
            }

            if (Single.IsNaN(saturation))
            {
                throw ValueIsNaN(nameof(saturation));
            }

            if (Single.IsNaN(lightness))
            {
                throw ValueIsNaN(nameof(lightness));
            }

            saturation = Clamp(saturation, 0, 1);
            lightness = Clamp(lightness, 0, 1);

            var chroma = (1 - Abs((2 * lightness) - 1)) * saturation;
            var (r, g, b) = GetBaseRgb(hue, chroma);
            var match = lightness - (chroma / 2.0f);

            return new ColorF(alpha, r + match, g + match, b + match);
        }

        /// <summary>
        /// Blend two <see cref="ColorF"/> values using a specified <see
        /// cref="BlendMode"/>.
        /// </summary>
        /// <param name="top">
        /// The top layer <see cref="ColorF"/> value.
        /// </param>
        /// <param name="bottom">
        /// The bottom layer <see cref="ColorF"/> value.
        /// </param>
        /// <param name="blendMode">
        /// An enum to specify which blending mode algorithm to use.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="ColorF"/> that is the blended result
        /// of <paramref name="bottom"/> to <paramref name=" top"/> through an
        /// algorithm corresponding to <paramref name="blendMode"/>.
        /// </returns>
        /// <remarks>
        /// Refer to https://en.wikipedia.org/wiki/Blend_modes for information
        /// on all of the blend algorithms.
        /// </remarks>
        public static ColorF Blend(
           ColorF top,
           ColorF bottom,
           BlendMode blendMode)
        {
            if (BlendDictionary.TryGetValue(blendMode, out var blend))
            {
                return blend(top, bottom);
            }

            throw new InvalidEnumArgumentException(
                nameof(blendMode),
                (int)blendMode,
                typeof(BlendMode));
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> that is an
        /// alpha-composite blend of two <see cref="ColorF"/> values.
        /// </summary>
        /// <param name="top">
        /// The top layer <see cref="ColorF"/> value.
        /// </param>
        /// <param name="bottom">
        /// The bottom layer <see cref="ColorF"/> value.
        /// </param>
        /// <returns>
        /// An alpha composite blend of <paramref name="top"/> and <paramref
        /// name="bottom"/> based on the opaqueness/ transparency of the two
        /// <see cref="ColorF"/> values.
        /// </returns>
        public static ColorF AlphaBlend(ColorF top, ColorF bottom)
        {
            if (top.Alpha == 0)
            {
                return bottom;
            }

            if (top.Alpha == 1)
            {
                return top;
            }

            var t = top.Alpha;
            var u = bottom.Alpha;

            var a = t + (u * (1 - t));
            var r = Mix(top.Red, bottom.Red);
            var g = Mix(top.Green, bottom.Green);
            var b = Mix(top.Blue, bottom.Blue);

            return new ColorF(a, r, g, b);

            float Mix(float x, float y)
            {
                return ((x * t) + (y * u * (1 - t))) / a;
            }
        }

        public static ColorF AlphaBlend(ColorF top, ColorF bottom, float alpha)
        {
            return AlphaBlend(FromArgb(alpha, top), bottom);
        }

        /// <summary>
        /// Blend two <see cref="ColorF"/> values using a specified <see
        /// cref="ChannelBlendCallback"/> delegate.
        /// </summary>
        /// <param name="top">
        /// The top layer <see cref="ColorF"/> value.
        /// </param>
        /// <param name="bottom">
        /// The bottom layer <see cref="ColorF"/> value.
        /// </param>
        /// <param name="rule">
        /// A delegate to specify how to mix two color channels. use.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="ColorF"/> that is the blended result
        /// of <paramref name="bottom"/> to <paramref name=" top"/> through an
        /// algorithm corresponding to <paramref name="rule"/>.
        /// </returns>
        public static ColorF Blend(
            ColorF top,
            ColorF bottom,
            ChannelBlendCallback rule)
        {
            if (rule is null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            var a = top.Alpha;
            var r = rule(top.Red, bottom.Red);
            var g = rule(top.Green, bottom.Green);
            var b = rule(top.Blue, bottom.Blue);
            var result = new ColorF(a, r, g, b);

            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance <see cref="ColorF"/> that is a grayscale blend
        /// of two colors.
        /// </summary>
        /// <param name="top">
        /// The top layer <see cref="ColorF"/> value.
        /// </param>
        /// <param name="bottom">
        /// The bottom layer <see cref="ColorF"/> value.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> that is similar to <paramref
        /// name="top"/> but with zero chromaticity.
        /// </returns>
        public static ColorF Grayscale(ColorF top, ColorF bottom)
        {
            var gray = GrayscaleWeight * (
                (top.Red * bottom.Red) +
                (top.Green * bottom.Green) +
                (top.Blue * bottom.Blue));

            return Blend(top, bottom, (x, y) => gray);
        }

        public static ColorF Multiply(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, (x, y) => x * y);
        }

        public static ColorF Divide(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                return x == 0 ? 1 : y / x;
            }
        }

        public static ColorF Screen(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                return 1 - ((1 - x) * (1 - y));
            }
        }

        public static ColorF Overlay(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                return y < 0.5f
                    ? 2 * x * y
                    : 1 - (2 * (1 - x) * (1 - y));
            }
        }

        public static ColorF HardLight(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                return y > 0.5f
                    ? 2 * x * y
                    : 1 - (2 * (1 - x) * (1 - y));
            }
        }

        public static ColorF SoftLight(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                var result = 2 * y;
                if (y < 0.5f)
                {
                    result *= x;
                    result += x * x * (1 - (2 * y));
                }
                else
                {
                    result *= 1 - x;
                    result += (float)Sqrt(y) * ((2 * x) - 1);
                }

                return result;
            }
        }

        public static ColorF ColorDodge(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                return x == 1
                    ? 1
                    : y / (1 - x);
            }
        }

        public static ColorF LinearDodge(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, (x, y) => x + y);
        }

        public static ColorF ColorBurn(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                return x == 0
                    ? 0
                    : 1 - ((1 - y) / x);
            }
        }

        public static ColorF LinearBurn(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, (x, y) => x + y - 1);
        }

        public static ColorF Difference(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                return x > y ? (x - y) : (y - x);
            }
        }

        public static ColorF Darken(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Math.Min);
        }

        public static ColorF Lighten(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Math.Max);
        }

        public static ColorF VividLight(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                return x == 1
                    ? 1
                    : x == 0
                    ? 0
                    : x > 0.5f
                    ? y / (1 - x)
                    : 1 - ((1 - x) / y);
            }
        }

        public static ColorF LinearLight(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, (x, y) => (2 * x) + y - 1);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> that is the darker of
        /// two colors.
        /// </summary>
        /// <param name="top">
        /// The top layer <see cref="ColorF"/> value.
        /// </param>
        /// <param name="bottom">
        /// The bottom layer <see cref="ColorF"/> value.
        /// </param>
        /// <returns>
        /// If the <see cref="Luma"/> of <paramref name="top"/> is less than
        /// that of <paramref name="bottom"/>'s, then <paramref name="top"/>
        /// is returned; otherwise, <paramref name="bottom"/>.
        /// </returns>
        public static ColorF DarkerColor(ColorF top, ColorF bottom)
        {
            var result = top.Luma < bottom.Luma ? top : bottom;
            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> that is the lighter of
        /// two colors.
        /// </summary>
        /// <param name="top">
        /// The top layer <see cref="ColorF"/> value.
        /// </param>
        /// <param name="bottom">
        /// The bottom layer <see cref="ColorF"/> value.
        /// </param>
        /// <returns>
        /// If the <see cref="Luma"/> of <paramref name="top"/> is greater
        /// than that of <paramref name="bottom"/>'s, then <paramref
        /// name="top"/> is returned; otherwise, <paramref name="bottom"/>.
        /// </returns>
        public static ColorF LighterColor(ColorF top, ColorF bottom)
        {
            var result = top.Luma > bottom.Luma ? top : bottom;
            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> that has the <see
        /// cref="Hue"/> of one layer and the <see cref="Luma"/> and <see
        /// cref="Chroma"/> of another layer.
        /// </summary>
        /// <param name="top">
        /// The top layer <see cref="ColorF"/> value.
        /// </param>
        /// <param name="bottom">
        /// The bottom layer <see cref="ColorF"/> value.
        /// </param>
        /// <returns>
        /// A <see cref="ColorF"/> value whose <see cref="Hue"/> is the same
        /// value as <paramref name="top"/>'s and <see cref=" Luma"/> and <see
        /// cref="Chroma"/> are the same as <paramref name="bottom"/>'s and
        /// that is alpha blended with <paramref name="bottom"/>.
        /// </returns>
        public static ColorF HueBlend(ColorF top, ColorF bottom)
        {
            var result = FromHcy(top.Alpha, bottom.Hue, top.Chroma, top.Luma);
            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> that has the <see
        /// cref="Chroma"/> of one layer and the <see cref="Hue"/> and <see
        /// cref="Luma"/> of another layer.
        /// </summary>
        /// <param name="top">
        /// The top layer <see cref="ColorF"/> value.
        /// </param>
        /// <param name="bottom">
        /// The bottom layer <see cref="ColorF"/> value.
        /// </param>
        /// <returns>
        /// A <see cref="ColorF"/> value whose <see cref="Chroma"/> is = the
        /// same value as <paramref name="top"/>'s and <see cref=" Hue"/> and
        /// <see cref="Luma"/> are the same as <paramref name="bottom"/>'s and
        /// that is alpha blended with <paramref name="bottom"/>.
        /// </returns>
        public static ColorF SaturationBlend(ColorF top, ColorF bottom)
        {
            var result = FromHcy(top.Alpha, top.Hue, bottom.Chroma, top.Luma);
            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> that has the <see
        /// cref="Luma"/> of one layer and the <see cref="Hue"/> and <see
        /// cref="Chroma"/> of another layer.
        /// </summary>
        /// <param name="top">
        /// The top layer <see cref="ColorF"/> value.
        /// </param>
        /// <param name="bottom">
        /// The bottom layer <see cref="ColorF"/> value.
        /// </param>
        /// <returns>
        /// A <see cref="ColorF"/> value whose <see cref="Luma"/> is the same
        /// value as <paramref name="top"/>'s and <see cref="Hue"/> and <see
        /// cref="Chroma"/> are the same as <paramref name=" bottom"/>'s and
        /// that is alpha blended with <paramref name="bottom"/>.
        /// </returns>
        public static ColorF LuminosityBlend(ColorF top, ColorF bottom)
        {
            var result = FromHcy(top.Alpha, top.Hue, top.Chroma, bottom.Luma);
            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> similar to this
        /// instance but with its chroma set to zero.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="ColorF"/> with the same <see
        /// cref="Lightness"/> as this instance but with zero <see
        /// cref="Chroma"/>.
        /// </returns>
        public ColorF Grayscale()
        {
            return Grayscale(this, FromArgb(1, 1, 1));
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> similar to this
        /// instance but with its chroma offset by a given amount.
        /// </summary>
        /// <param name="bottom">
        /// The bottom layer.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref=" Chroma"/>
        /// and <see cref="Luma"/> properties are that of <paramref
        /// name="bottom"/> and whose <see cref="Hue"/> is equal to this
        /// instance's offset by the <see cref="Hue"/> of <paramref
        /// name="bottom"/>. The <see cref="Alpha"/> property is the same as
        /// this instance's.
        /// </returns>
        public ColorF RotateHue(ColorF bottom)
        {
            var hue = Hue + bottom.Hue;
            var result = FromHcy(Alpha, hue, Chroma, Luma);
            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> that is the inverse of
        /// this instance.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="ColorF"/> whose <see cref="Red"/>, <see
        /// cref="Green"/>, and <see cref="Blue"/> properties are the inverse
        /// of this instance's. The <see cref="Alpha"/> value is unchanged
        /// during inversion.
        /// </returns>
        public ColorF Invert()
        {
            return new ColorF(Alpha, 1 - Red, 1 - Green, 1 - Blue);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a
        /// specified <see cref="ColorF"/> value.
        /// </summary>
        /// <param name="obj">
        /// A <see cref="ColorF"/> value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> has the same
        /// value as this instance; otherwise, <see langword=" false"/>.
        /// </returns>
        public bool Equals(ColorF obj)
        {
            return
                Alpha.Equals(obj.Alpha) &&
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
        /// <see cref="ColorF"/> and equals the value of this instance;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is ColorF other
                ? Equals(other)
                : false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return ((Color)this).ToArgb().GetHashCode();
        }

        /// <summary>
        /// Converts the value of the current <see cref=" ColorF"/> object to
        /// its equivalent string representation.
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

        /// <summary>
        /// Gets the base RGB values from a hue and chroma before applying
        /// luma correction.
        /// </summary>
        /// <param name="hue">
        /// The hue of the color definition.
        /// </param>
        /// <param name="chroma">
        /// the chroma of the color definition.
        /// </param>
        /// <returns>
        /// A tuple of the base red, green, and blue parameters.
        /// </returns>
        private static (float r, float g, float b) GetBaseRgb(
            float hue,
            float chroma)
        {
            if (chroma == 0)
            {
                return (0, 0, 0);
            }

            while (hue < 0)
            {
                hue += 1;
            }

            while (hue >= 1)
            {
                hue -= 1;
            }

            hue *= 6;
            var x = chroma * (1 - Abs((hue % 2) - 1));
            return hue <= 1
                ? ((float r, float g, float b))(chroma, x, 0)
                : hue <= 2
                ? ((float r, float g, float b))(x, chroma, 0)
                : hue <= 3
                ? ((float r, float g, float b))(0, chroma, x)
                : hue <= 4
                ? ((float r, float g, float b))(0, x, chroma)
                : hue <= 5
                ? ((float r, float g, float b))(x, 0, chroma)
                : ((float r, float g, float b))(chroma, 0, x);
        }
    }
}
