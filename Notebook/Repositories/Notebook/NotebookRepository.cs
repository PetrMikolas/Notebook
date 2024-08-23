using Microsoft.EntityFrameworkCore;
using Notebook.Databases.Notebook;
using Notebook.Exceptions;
using Notebook.Models;

namespace Notebook.Repositories.Notebook;

/// <summary>
/// Repository for managing notebook sections and pages in the database.
/// </summary>
internal sealed class NotebookRepository(NotebookDbContext dbContext) : INotebookRepository
{
    public async Task<IEnumerable<Section>> GetSectionsAsync(string userId, CancellationToken cancellationToken)
    {
        var sectionsQuery = dbContext.Sections.AsNoTracking()
            .Where(section => section.UserId == userId)
            .Select(section => new Section
            {
                Id = section.Id,
                Name = section.Name,
                Pages = section.Pages.Select(page => new Page
                {
                    Id = page.Id,
                    Title = page.Title,
                    SizeInBytes = page.SizeInBytes,
                    CreatedAt = page.CreatedAt,
                    UpdatedAt = page.UpdatedAt,
                    SectionId = page.SectionId,
                }).ToList()
            });

        return await sectionsQuery.ToListAsync(cancellationToken);
    }

    public async Task CreateSectionAsync(Section? entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        dbContext.Sections.Add(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateSectionAsync(Section? entity, string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var numberUpdated = await dbContext.Sections
            .Where(section => section.Id == entity.Id && section.UserId == userId)
            .ExecuteUpdateAsync(seters => seters
                .SetProperty(section => section.Name, entity.Name),
                cancellationToken
            );

        if (numberUpdated == 0)
        {
            throw new EntityNotFoundException(nameof(Page));
        }
    }

    public async Task DeleteSectionAsync(int id, string userId, CancellationToken cancellationToken)
    {
        var section = await dbContext.Sections.AsNoTracking()
            .Where(s => s.Id == id && s.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Section));

        dbContext.Sections.Remove(section);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> GetPageContentById(int id, string userId, CancellationToken cancellationToken)
    {
        var content = await dbContext.Pages.AsNoTracking()
            .Where(page => page.Id == id && page.Section.UserId == userId)
            .Select(page => page.Content)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Page));

        return content;
    }

    public async Task AddPageAsync(Page? entity, string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var section = await dbContext.Sections
            .Where(s => s.Id == entity.SectionId && s.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Section));

        section.Pages.Add(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePageAsync(Page? entity, string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var numberUpdated = await dbContext.Pages
            .Where(page => page.Id == entity.Id && page.Section.UserId == userId)
            .ExecuteUpdateAsync(seters => seters
                .SetProperty(page => page.Title, entity.Title)
                .SetProperty(page => page.Content, entity.Content)
                .SetProperty(page => page.SizeInBytes, entity.SizeInBytes)
                .SetProperty(page => page.UpdatedAt, entity.UpdatedAt),
                cancellationToken
            );

        if (numberUpdated == 0)
        {
            throw new EntityNotFoundException(nameof(Page));
        }
    }

    public async Task DeletePageAsync(int id, string userId, CancellationToken cancellationToken)
    {
        var numberDeleted = await dbContext.Pages
            .Where(page => page.Id == id && page.Section.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);

        if (numberDeleted == 0)
        {
            throw new EntityNotFoundException(nameof(Page));
        }
    }
}