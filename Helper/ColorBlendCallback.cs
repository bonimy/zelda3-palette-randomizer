// <copyright file="ColorBlendCallback.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    /// <summary>
    /// References a method to be called to perform a blend operation between
    /// two <see cref="ColorF"/> structs.
    /// </summary>
    /// <param name="top">
    /// The top layer <see cref="ColorF"/>.
    /// </param>
    /// <param name="bottom">
    /// The bottom layer <see cref="ColorF"/>.
    /// </param>
    /// <returns>
    /// The return value of the method that this delegate encapsulates.
    /// </returns>
    public delegate ColorF ColorBlendCallback(ColorF top, ColorF bottom);
}
