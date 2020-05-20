// <copyright file="ColorF.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
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
        /// The total number of non-alpha channels (red, green, blue) that
        /// comprise a <see cref="ColorF"/> color.
        /// </summary>
        public const int NumberOfColorChannels = NumberOfChannels - 1;

        /// <summary>
        /// Represents a <see cref="ColorF"/> that is black.
        /// </summary>
        public static readonly ColorF Empty = default;

        private const float GrayscaleWeight = 1f / NumberOfColorChannels;

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

                float hue;
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
                else
                {
                    // Max == Blue
                    hue = ((Red - Green) / Chroma) + 4;
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
        public static ColorF operator +(ColorF left, ColorF right)
        {
            return Add(left, right);
        }

        /// <summary>
        /// Performs the <see cref="Subtract(ColorF, ColorF)"/> blend
        /// operation of one <see cref="ColorF"/> from another.
        /// </summary>
        public static ColorF operator -(ColorF left, ColorF right)
        {
            return Subtract(left, right);
        }

        /// <summary>
        /// Performs the <see cref="Multiply(ColorF, ColorF)"/> blend
        /// operation of two <see cref="ColorF"/> values.
        /// </summary>
        public static ColorF operator *(ColorF left, ColorF right)
        {
            return Multiply(left, right);
        }

        /// <summary>
        /// Performs the <see cref="Divide(ColorF, ColorF)"/> blend operation
        /// of one <see cref="ColorF"/> into another.
        /// </summary>
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
        public static bool operator ==(in ColorF left, in ColorF right)
        {
            return
                left.Alpha == right.Alpha &&
                left.Red == right.Red &&
                left.Green == right.Green &&
                left.Blue == right.Blue;
        }

        /// <summary>
        /// Compares to <see cref="ColorF"/> values. The result specifies
        /// whether the values of the <see cref="Alpha"/>, <see cref="Red"/>,
        /// <see cref="Green"/>, or <see cref=" Blue"/> properties of the two
        /// <see cref="ColorF"/> are unequal.
        /// </summary>
        public static bool operator !=(in ColorF left, in ColorF right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Performs the <see cref="Negate(ColorF)"/> operation of a <see
        /// cref="ColorF"/>.
        /// </summary>
        public static ColorF operator -(ColorF color)
        {
            return Negate(color);
        }

        public static ColorF FromColor(Color color)
        {
            return (ColorF)color;
        }

        public static Color ToColor(ColorF color)
        {
            return (Color)color;
        }

        /// <summary>
        /// Perform the <see cref="Invert"/> operation on a <see
        /// cref="ColorF"/> instance.
        /// </summary>
        public static ColorF Negate(ColorF color)
        {
            return color.Invert();
        }

        /// <summary>
        /// Add the RGB color channels of two <see cref="ColorF"/> values.
        /// </summary>
        public static ColorF Add(ColorF left, ColorF right)
        {
            return LinearDodge(left, right);
        }

        /// <summary>
        /// Subtracts the RGB color channels of a <see cref="ColorF"/> values
        /// from another.
        /// </summary>
        public static ColorF Subtract(ColorF left, ColorF right)
        {
            return Difference(left, right);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by an RGB
        /// color space and is fully opaque.
        /// </summary>
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
                throw UnexpectedNaNArgumentExeption(nameof(alpha));
            }

            if (Single.IsNaN(red))
            {
                throw UnexpectedNaNArgumentExeption(nameof(red));
            }

            if (Single.IsNaN(green))
            {
                throw UnexpectedNaNArgumentExeption(nameof(green));
            }

            if (Single.IsNaN(blue))
            {
                throw UnexpectedNaNArgumentExeption(nameof(blue));
            }

            return new ColorF(alpha, red, green, blue);
        }

        /// <summary>
        /// Returns an instance of <see cref="ColorF"/> defined by a CMY color
        /// space and is fully opaque.
        /// </summary>
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
                throw UnexpectedNaNArgumentExeption(nameof(alpha));
            }

            if (Single.IsNaN(cyan))
            {
                throw UnexpectedNaNArgumentExeption(nameof(cyan));
            }

            if (Single.IsNaN(magenta))
            {
                throw UnexpectedNaNArgumentExeption(nameof(magenta));
            }

            if (Single.IsNaN(yellow))
            {
                throw UnexpectedNaNArgumentExeption(nameof(yellow));
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
                throw UnexpectedNaNArgumentExeption(nameof(alpha));
            }

            if (Single.IsNaN(cyan))
            {
                throw UnexpectedNaNArgumentExeption(nameof(cyan));
            }

            if (Single.IsNaN(magenta))
            {
                throw UnexpectedNaNArgumentExeption(nameof(magenta));
            }

            if (Single.IsNaN(yellow))
            {
                throw UnexpectedNaNArgumentExeption(nameof(yellow));
            }

            if (Single.IsNaN(black))
            {
                throw UnexpectedNaNArgumentExeption(nameof(black));
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
                throw UnexpectedNaNArgumentExeption(nameof(alpha));
            }

            if (Single.IsNaN(hue))
            {
                throw UnexpectedNaNArgumentExeption(nameof(hue));
            }

            if (Single.IsNaN(chroma))
            {
                throw UnexpectedNaNArgumentExeption(nameof(chroma));
            }

            if (Single.IsNaN(luma))
            {
                throw UnexpectedNaNArgumentExeption(nameof(luma));
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
                throw UnexpectedNaNArgumentExeption(nameof(alpha));
            }

            if (Single.IsNaN(hue))
            {
                throw UnexpectedNaNArgumentExeption(nameof(hue));
            }

            if (Single.IsNaN(saturation))
            {
                throw UnexpectedNaNArgumentExeption(nameof(saturation));
            }

            if (Single.IsNaN(lightness))
            {
                throw UnexpectedNaNArgumentExeption(nameof(lightness));
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

        public static ColorF Blend(
            ColorF top,
            ColorF bottom,
            ChannelBlendCallback channelBlend)
        {
            if (channelBlend is null)
            {
                throw new ArgumentNullException(nameof(channelBlend));
            }

            var a = top.Alpha;
            var r = channelBlend(top.Red, bottom.Red);
            var g = channelBlend(top.Green, bottom.Green);
            var b = channelBlend(top.Blue, bottom.Blue);
            var result = new ColorF(a, r, g, b);

            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance <see cref="ColorF"/> that is a grayscale blend
        /// of two colors.
        /// </summary>
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
            return Blend(top, bottom, DivideChannelBlend);

            static float DivideChannelBlend(float x, float y)
            {
                return x == 0 ? 1 : y / x;
            }
        }

        public static ColorF Screen(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, ScreenChannelBlend);

            static float ScreenChannelBlend(float x, float y)
            {
                return 1 - ((1 - x) * (1 - y));
            }
        }

        public static ColorF Overlay(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, OverlayChannelBlend);

            static float OverlayChannelBlend(float x, float y)
            {
                return y < 0.5f
                    ? 2 * x * y
                    : 1 - (2 * (1 - x) * (1 - y));
            }
        }

        public static ColorF HardLight(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, HardLightChannelBlend);

            static float HardLightChannelBlend(float x, float y)
            {
                return y > 0.5f
                    ? 2 * x * y
                    : 1 - (2 * (1 - x) * (1 - y));
            }
        }

        public static ColorF SoftLight(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, SoftLightChannelBlend);

            static float SoftLightChannelBlend(float x, float y)
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
            return Blend(top, bottom, ColorDodgeChannelBlend);

            static float ColorDodgeChannelBlend(float x, float y)
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
            return Blend(top, bottom, ColorBurnChannelBlend);

            static float ColorBurnChannelBlend(float x, float y)
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
            return Blend(top, bottom, DifferenceChannelBlend);

            static float DifferenceChannelBlend(float x, float y)
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
            return Blend(top, bottom, VividLightChannelBlend);

            static float VividLightChannelBlend(float x, float y)
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
        public static ColorF DarkerColor(ColorF top, ColorF bottom)
        {
            var result = top.Luma < bottom.Luma ? top : bottom;
            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> that is the lighter of
        /// two colors.
        /// </summary>
        public static ColorF LighterColor(ColorF top, ColorF bottom)
        {
            var result = top.Luma > bottom.Luma ? top : bottom;
            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> that has the <see
        /// cref="Hue"/> of the bottom layer and the <see cref="Luma"/> and
        /// <see cref="Chroma"/> of the top layer.
        /// </summary>
        public static ColorF HueBlend(ColorF top, ColorF bottom)
        {
            var result = FromHcy(top.Alpha, bottom.Hue, top.Chroma, top.Luma);
            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> that has the <see
        /// cref="Chroma"/> of the bottom layer and the <see cref="Hue"/> and
        /// <see cref="Luma"/> of the top layer.
        /// </summary>
        public static ColorF SaturationBlend(ColorF top, ColorF bottom)
        {
            var result = FromHcy(top.Alpha, top.Hue, bottom.Chroma, top.Luma);
            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> that has the <see
        /// cref="Luma"/> of the bottom layer and the <see cref="Hue"/> and
        /// <see cref="Chroma"/> of the top layer.
        /// </summary>
        public static ColorF LuminosityBlend(ColorF top, ColorF bottom)
        {
            var result = FromHcy(top.Alpha, top.Hue, top.Chroma, bottom.Luma);
            return AlphaBlend(result, bottom);
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> similar to this
        /// instance but with its chroma set to zero.
        /// </summary>
        public ColorF Grayscale()
        {
            return Grayscale(this, FromArgb(1, 1, 1));
        }

        /// <summary>
        /// Create a grayscale using luma weighting.
        /// </summary>
        public ColorF LumaGrayscale()
        {
            return Grayscale(
                this,
                FromArgb(LumaRedWeight, LumaGreenWeight, LumaBlueWeight));
        }

        /// <summary>
        /// Create an instance of <see cref="ColorF"/> similar to this
        /// instance but with its chroma offset by a given amount.
        /// </summary>
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
        public ColorF Invert()
        {
            return new ColorF(Alpha, 1 - Red, 1 - Green, 1 - Blue);
        }

        public bool Equals(ColorF obj)
        {
            return this == obj;
        }

        public override bool Equals(object obj)
        {
            return obj is ColorF other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Alpha, Red, Green, Blue);
        }

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
