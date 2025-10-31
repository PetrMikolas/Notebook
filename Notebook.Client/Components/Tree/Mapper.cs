using Notebook.Client.Services.Api;

namespace Notebook.Client.Components.Tree;

public static class Mapper
{
    /// <summary>
    /// Converts a collection of sections and their pages into a tree of nodes for display in a tree component.
    /// </summary>
    public static List<TreeNode> ToTreeNodes(this List<SectionDto> sections)
    {
        return [.. sections.Select(section => new TreeNode
        {
            Id = section.Id,
            Text = section.Name,
            Data = section, 
            Children = [.. section.Pages.Select(page => new TreeNode
            {
                Id = page.Id,
                ParentId = section.Id,
                Text = page.Title,
                Data = page 
            })]
        })];
    }
}
