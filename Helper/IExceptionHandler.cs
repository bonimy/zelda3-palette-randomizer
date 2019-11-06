// <copyright file="IExceptionHandler.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Provides methods for showing and handling a caught <see cref="
    /// Exception"/> to the user.
    /// </summary>
    public interface IExceptionHandler : IComponent
    {
        /// <summary>
        /// Show <see cref="Exception"/> info to the user.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to show.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ex"/> is <see langword="null"/>.
        /// </exception>
        void ShowException(Exception ex);

        /// <summary>
        /// Show <see cref="Exception"/> info to the user and prompts the user
        /// to retry the exceptional action.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to show.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the user selects to retry the process
        /// that threw the exception; otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ex"/> is <see langword="null"/>.
        /// </exception>
        bool ShowExceptionAndRetry(Exception ex);
    }
}
