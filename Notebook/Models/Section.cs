namespace Notebook.Models;

/// <summary>
/// Represents a section in the notebook.
/// </summary>
public class Section
{
    /// <summary>
    /// Gets or sets the unique identifier of the section.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the section.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user identifier associated with the section.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets the list of pages belonging to the section.
    /// </summary>
    public List<Page> Pages { get; } = [];
}