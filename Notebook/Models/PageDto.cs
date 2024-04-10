using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Notebook.Models;

/// <summary>
/// Data transfer object representing a page.
/// </summary>
public class PageDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the page.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the page.
    /// </summary>
    [Required(ErrorMessage = "povinný údaj")]
    [MaxLength(30, ErrorMessage = "max délka 30 znaků")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of the page.
    /// </summary>
    [DefaultValue("")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the page in bytes.
    /// </summary>
    public long SizeInBytes { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the section to which the page belongs.
    /// </summary>
    public int SectionId { get; set; }
}