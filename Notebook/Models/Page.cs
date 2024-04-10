namespace Notebook.Models;


/// <summary>
/// Represents a page in the notebook.
/// </summary>
public class Page
{
    /// <summary>
    /// Gets or sets the unique identifier of the page.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the page.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of the page.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the page in bytes.
    /// </summary>
    public long SizeInBytes { get; set; }

    /// <summary>
    /// Gets or sets the creation date and time of the page.
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the optional modification date and time of the page.
    /// </summary>
    public DateTimeOffset? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the section to which the page belongs.
    /// </summary>
    public int SectionId { get; set; }

    /// <summary>
    /// Gets or sets the reference to the section to which the page belongs.
    /// </summary>
    public Section Section { get; set; } = new();
}
