namespace Notebook.Client.Enums;

/// <summary>
/// Options for the selected item in a tree.
/// </summary>
/// <remarks>
/// Possible values:
/// <list type="bullet">
/// <item><description><see cref="None"/>: No selected item.</description></item>
/// <item><description><see cref="Section"/>: Selected section.</description></item>
/// <item><description><see cref="Page"/>: Selected page.</description></item>
/// </list>
/// </remarks>
public enum SelectedNodeOptions
{
    /// <summary>
    /// No selected item.
    /// </summary>
    None,

    /// <summary>
    /// Selected section.
    /// </summary>
    Section,

    /// <summary>
    /// Selected page.
    /// </summary>
    Page
}