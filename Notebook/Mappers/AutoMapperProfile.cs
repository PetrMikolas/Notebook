using AutoMapper;
using Notebook.Models;

namespace Notebook.Mappers;

/// <summary>
/// Configures AutoMapper mappings between entities and DTOs for the Notebook application.
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMapperProfile"/> class.
    /// Configures mappings between entities and DTOs.
    /// </summary>
    public AutoMapperProfile()
    {     
        CreateMap<Section, SectionDto>();
        CreateMap<Page, PageDto>();      
        CreateMap<SectionDto, Section>();       
        CreateMap<PageDto, Page>();
    }
}