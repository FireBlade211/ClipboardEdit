using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsAero.TaskDialog;

namespace ClipboardEdit.Helpers
{
    /// <summary>
    /// Provides constants for special task dialog buttons.
    /// </summary>
    public static class TaskDialogSpecialButtons
    {
        /// <summary>
        /// Gets a <see cref="CommonButton"/> value for the Continue button.
        /// </summary>
        public const CommonButton Continue = (CommonButton)0x00080000;

        /// <summary>
        /// Gets a <see cref="CommonButton"/> value for the Ignore button.
        /// </summary>
        public const CommonButton Ignore = (CommonButton)0x00020000;

        /// <summary>
        /// Gets a <see cref="CommonButton"/> value for the Abort button.
        /// </summary>
        public const CommonButton Abort = (CommonButton)0x00010000;

        /// <summary>
        /// Gets a <see cref="CommonButton"/> value for the Help button.
        /// </summary>
        public const CommonButton Help = (CommonButton)0x00100000;

        /// <summary>
        /// Gets a value for the ID of the Continue button.
        /// </summary>
        public const int ContinueResult = 11;
    }
}
