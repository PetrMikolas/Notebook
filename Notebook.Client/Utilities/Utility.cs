namespace Notebook.Client.Utilities;

/// <summary>
/// Provides utility methods
/// </summary>
public static class Utility
{
    /// <summary>
    /// Formats the given number of bytes into a human-readable string representation.
    /// </summary>
    /// <param name="bytes">The number of bytes to be formatted.</param>
    /// <returns>A string representing the formatted size.</returns>
    public static string FormatBytes(long bytes)
    {
        const int kilobyte = 1024;
        const int megabyte = 1024 * 1024;
        const int gigabyte = 1024 * 1024 * 1024;

        if (bytes < kilobyte)
        {
            return $"{bytes} B";
        }
        else if (bytes < megabyte)
        {
            return $"{bytes / kilobyte} kB";
        }
        else if (bytes < gigabyte)
        {
            return $"{bytes / (double)megabyte:F2} MB";
        }
        else
        {
            return $"{bytes / (double)gigabyte:F2} GB";
        }
    }
}