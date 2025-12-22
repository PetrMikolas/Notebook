using System.Runtime.CompilerServices;

namespace Notebook.Helpers;

/// <summary>
/// Helper methods.
/// </summary>
public static class Helper
{
    /// <summary>
    /// Creates an <see cref="ILogger"/> using the specified <see cref="IServiceProvider"/>, generating the logger category automatically from the calling class and member name.
    /// </summary>
    /// <remarks>
    /// The logger category is constructed in the form "<c>{ClassName}.{MemberName}</c>", where <c>ClassName</c> is derived from <paramref name="callerFilePath"/> and
    /// <c>MemberName</c> is derived from <paramref name="callerMemberName"/>. Both parameters are supplied automatically by the compiler.
    /// </remarks>
    /// <param name="provider"> The <see cref="IServiceProvider"/> used to resolve the <see cref="ILoggerFactory"/>.</param>
    /// <param name="callerFilePath">Automatically supplied path of the source file containing the caller.</param>
    /// <param name="callerMemberName">Automatically supplied name of the calling method or property.</param>
    /// <returns>An <see cref="ILogger"/> instance with a category name derived from the calling file and member.</returns>
    public static ILogger CreateLogger(this IServiceProvider provider, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
    {
        var factory = provider.GetRequiredService<ILoggerFactory>();
        return CreateLoggerCore(factory, callerFilePath, callerMemberName);
    }

    /// <summary>
    /// Creates an <see cref="ILogger"/> for the specified <see cref="WebApplication"/>, using the calling file and member to determine the logger category.
    /// </summary>
    /// <remarks>
    /// The logger category is constructed as "<c>{ClassName}.{MemberName}</c>", where <c>ClassName</c> is derived from the calling file name and
    /// <c>MemberName</c> is the name of the calling method or property. Both values are supplied automatically by the compiler.
    /// </remarks>
    /// <param name="app">The <see cref="WebApplication"/> from whose services the logger factory is resolved.</param>
    /// <param name="callerFilePath">Automatically supplied path of the source file containing the caller.</param>
    /// <param name="callerMemberName">Automatically supplied name of the calling method or property.</param>
    /// <returns>An <see cref="ILogger"/> instance with a category name derived from the calling file and member.</returns>
    public static ILogger CreateLogger(this WebApplication app, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
    {
        var factory = app.Services.GetRequiredService<ILoggerFactory>();
        return CreateLoggerCore(factory, callerFilePath, callerMemberName);
    }

    private static ILogger CreateLoggerCore(ILoggerFactory factory, string callerFilePath, string callerMemberName)
    {
        var className = Path.GetFileNameWithoutExtension(callerFilePath);
        var categoryName = $"{className}.{callerMemberName}";

        return factory.CreateLogger(categoryName);
    }
}