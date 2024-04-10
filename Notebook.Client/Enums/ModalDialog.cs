namespace Notebook.Client.Enums;

/// <summary>
/// Represents the types of modal dialogs.
/// </summary>
/// <remarks>
/// Possible values:
/// <list type="bullet">
/// <item><description><see cref="None"/>: No modal dialog.</description></item>
/// <item><description><see cref="Yes"/>: Modal dialog with "Yes" option.</description></item>
/// <item><description><see cref="No"/>: Modal dialog with "No" option.</description></item>
/// </list>
/// </remarks>
public enum ModalDialog
{
    /// <summary>
    /// No modal dialog.
    /// </summary>
    None,

    /// <summary>
    /// Modal dialog with "Yes" option.
    /// </summary>
    Yes,

    /// <summary>
    /// Modal dialog with "No" option.
    /// </summary>
    No
}