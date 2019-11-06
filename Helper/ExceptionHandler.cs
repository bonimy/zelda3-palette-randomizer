// <copyright file="ExceptionHandler.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;
    using System.ComponentModel;

    public abstract class ExceptionHandler : Component, IExceptionHandler
    {
        protected ExceptionHandler()
        {
        }

        protected ExceptionHandler(IContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        /// <summary>
        /// Displays a message box that shows <see cref="Exception"/> info to
        /// the user.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to show.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ex"/> is <see langword="null"/>.
        /// </exception>
        public abstract void ShowException(Exception ex);

        /// <summary>
        /// Displays a message box that shows <see cref="Exception"/> info to
        /// the user and prompts the user to retry the exceptional action
        /// again.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to show.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the user selects Retry, otherwise <see
        /// langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ex"/> is <see langword="null"/>.
        /// </exception>
        public abstract bool ShowExceptionAndRetry(Exception ex);
    }
}
