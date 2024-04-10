using System.ComponentModel.DataAnnotations;

namespace Notebook.Models;

/// <summary>
/// Represents the data object of a section in the notebook.
/// </summary>
public class SectionDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the section.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the section.
    /// </summary>
    [Required(ErrorMessage = "povinný údaj")]
    [MaxLength(30, ErrorMessage = "maximální délka 30 znaků")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the list of page data objects belonging to the section.
    /// </summary>
    public List<PageDto> Pages { get; } = [];
}