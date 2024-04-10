using Notebook.Models;
using System.Globalization;
using System.Text;

namespace Notebook.Helpers;

/// <summary>
/// Provides helper methods for various operations within the Notebook application.
/// </summary>
public class Helper
{
    /// <summary>
    /// Checks if the provided SectionDto object is valid.
    /// </summary>
    /// <param name="sectionDto">The SectionDto object to validate.</param>
    /// <param name="httpMethod">The HTTP method used.</param>
    /// <param name="error">The error message if validation fails.</param>
    /// <returns>True if the SectionDto object is valid; otherwise, false.</returns>
    public static bool IsValidSectionDto(SectionDto? sectionDto, HttpMethod httpMethod, out string error)
    {
        error = string.Empty;

        if (sectionDto is null)
        {
            error = $"Parametr <{nameof(sectionDto)}> nemůže být null.";
            return false ;
        }

        if (httpMethod == HttpMethod.Post && sectionDto.Id != 0)
        {
            error = $"Parametr <{nameof(sectionDto.Id)}> musí mít hodnotu 0. ";
        }

        if (httpMethod != HttpMethod.Post && sectionDto.Id <= 0)
        {
            error = $"Parametr <{nameof(sectionDto.Id)}> musí mít hodnotu větší než 0. ";
        }

        if (sectionDto.Name is null)
        {
            error += $"Parametr <{nameof(sectionDto.Name)}> nemůže být null. ";
        }
        else if (sectionDto.Name.Length <= 0 || sectionDto.Name.Length > 30)
        {
            error += $"Parametr <{nameof(sectionDto.Name)}> může mít délku 1 až 30 znaků. ";
        }                

        return error == string.Empty;
    }

    /// <summary>
    /// Checks if the provided PageDto object is valid.
    /// </summary>
    /// <param name="pageDto">The PageDto object to validate.</param>
    /// <param name="httpMethod">The HTTP method used.</param>
    /// <param name="error">The error message if validation fails.</param>
    /// <returns>True if the PageDto object is valid; otherwise, false.</returns>
    public static bool IsValidPageDto(PageDto? pageDto, HttpMethod httpMethod, out string error)
    {
        error = string.Empty;

        if (pageDto is null)
        {
            error = $"Parametr <{nameof(pageDto)}> nemůže být null.";
            return false;
        }

        if (pageDto.SectionId <= 0)
        {
            error = $"Parametr <{nameof(pageDto.SectionId)}> musí mít hodnotu větší než 0. ";
        }

        if (httpMethod == HttpMethod.Post && pageDto.Id != 0)
        {
            error += $"Parametr <{nameof(pageDto.Id)}> musí mít hodnotu 0. ";
        }                

        if (httpMethod != HttpMethod.Post && pageDto.Id <= 0)
        {
            error += $"Parametr <{nameof(pageDto.Id)}> musí mít hodnotu větší než 0. ";
        }        

        if (pageDto.Title is null)
        {
            error += $"Parametr <{nameof(pageDto.Title)}> nemůže být null. ";
        }
        else if (pageDto.Title.Length <= 0 || pageDto.Title.Length > 30)
        {
            error += $"Parametr <{nameof(pageDto.Title)}> může mít délku 1 až 30 znaků. ";
        }

        if (pageDto.Content is null)
        {
            error += $"Parametr <{nameof(pageDto.Content)}> nemůže být null";
        }
        
        return error == string.Empty;
    }

    /// <summary>
    /// Removes diacritics from the specified text.
    /// </summary>
    /// <param name="text">The text from which to remove diacritics.</param>
    /// <returns>The text without diacritics.</returns>
    public static string RemoveDiacritics(string text)
    {
        return new string(text.Normalize(NormalizationForm.FormD)
                              .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                              .ToArray()
        );
    }
}