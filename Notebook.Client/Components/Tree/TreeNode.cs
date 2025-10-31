namespace Notebook.Client.Components.Tree;

/// <summary>
/// Represents a node in a tree view.
/// </summary>
public class TreeNode
{
    /// <summary>
    /// Gets or sets the unique identifier of the node.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the optional identifier of the parent node.
    /// Null if the node is a root node.
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// Gets or sets the display text of the node.
    /// </summary>
    public string Text { get; set; } = string.Empty;             

    /// <summary>
    /// Gets or sets an optional object associated with the node.
    /// Can store the original data.
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Gets or sets the child nodes of this node.
    /// </summary>
    public List<TreeNode> Children { get; set; } = [];
}
