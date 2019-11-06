// <copyright file="ChannelBlendCallback.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    /// <summary>
    /// References a method to be called to perform a blend operation between
    /// two color channels.
    /// </summary>
    /// <param name="top">
    /// A color channel of the top layer color.
    /// </param>
    /// <param name="bottom">
    /// A color channel of the bottom layer color.
    /// </param>
    /// <returns>
    /// The return value of the method that this delegate encapsulates.
    /// </returns>
    public delegate float ChannelBlendCallback(float top, float bottom);
}
