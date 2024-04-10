using Radzen;

namespace Notebook.Client.Models;

public sealed class AlertMessage
{
    public string Text { get; set; } = string.Empty;

    public AlertStyle Style { get; set; }

    public bool ShowIcon { get; set; }

    public bool AllowClose { get; set; }
}