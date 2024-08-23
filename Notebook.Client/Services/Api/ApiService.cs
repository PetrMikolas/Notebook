using Notebook.Client.Exceptions;
using Notebook.Shared.Exceptions;

namespace Notebook.Client.Services.Api;

/// <summary>
///  Service for interacting with the notebook API, implementing the <see cref="IApiService"/> interface.
/// </summary>
/// <param name="apiClient">The API client.</param>
public sealed class ApiService(IApiClient apiClient) : IApiService
{
    public async Task<List<SectionDto>> GetSectionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await apiClient.GetSectionsAsync(cancellationToken);
        }
        catch (ApiException ex)
        {
			if (ex.StatusCode == 401)
			{				
				throw new NotAuthorizedException("Vaše přihlášení vypršelo. Přihlaste se a zkuste to znovu.");
			}

			_ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new ApiNotebookResponseException("Nepodařilo se načíst data. Zkuste prosím znovu načíst stránku.");
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException("Nepodařilo se načíst data. Zkontrolujte prosím připojení k internetu a zkuste to znovu.");
        }
        catch (Exception ex)
        {
            _ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new Exception("Neočekávaná chyba. O problému víme a pracujeme na nápravě.");
        }
    }

    public async Task<List<SectionDto>> SearchSectionsAndPagesAsync(string searchText, CancellationToken cancellationToken = default)
    {
        try
        {
            return await apiClient.SearchSectionsAndPagesAsync(searchText, cancellationToken);
        }
        catch (ApiException ex)
        {
			if (ex.StatusCode == 401)
			{
				throw new NotAuthorizedException("Vaše přihlášení vypršelo. Přihlaste se a zkuste to znovu.");
			}

			_ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new ApiNotebookResponseException("Nepodařilo se načíst data. Zkuste to prosím později.");
        }
        catch (HttpRequestException)
        {
			throw new HttpRequestException("Nepodařilo se načíst data. Zkontrolujte prosím připojení k internetu a zkuste to znovu.");
		}
        catch (Exception ex)
        {
            _ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new Exception("Neočekávaná chyba. O problému víme a pracujeme na nápravě.");
        }
    }

    public async Task CreateSectionAsync(SectionDto section, CancellationToken cancellationToken = default)
    {
        try
        {
            await apiClient.CreateSectionAsync(section, cancellationToken);
        }
        catch (ApiException ex)
        {
			if (ex.StatusCode == 401)
			{
				throw new NotAuthorizedException("Vaše přihlášení vypršelo. Přihlaste se a zkuste to znovu.");
			}

			_ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new ApiNotebookResponseException("Nepodařilo se uložit oddíl. Zkuste to prosím později.");
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException("Nepodařilo se uložit oddíl. Zkontrolujte prosím připojení k internetu a zkuste to znovu.");
        }
        catch (Exception ex)
        {
            _ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new Exception("Neočekávaná chyba. O problému víme a pracujeme na nápravě.");
        }
    }

    public async Task UpdateSectionAsync(SectionDto section, CancellationToken cancellationToken = default)
    {
        try
        {
            await apiClient.UpdateSectionAsync(section, cancellationToken);
        }
        catch (ApiException ex)
        {
			if (ex.StatusCode == 401)
			{
				throw new NotAuthorizedException("Vaše přihlášení vypršelo. Přihlaste se a zkuste to znovu.");
			}

			_ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new ApiNotebookResponseException("Nepodařilo se editovat oddíl. Zkuste to prosím později.");
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException("Nepodařilo se editovat oddíl. Zkontrolujte prosím připojení k internetu a zkuste to znovu.");
        }
        catch (Exception ex)
        {
            _ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new Exception("Neočekávaná chyba. O problému víme a pracujeme na nápravě.");
        }
    }

    public async Task DeleteSectionAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await apiClient.DeleteSectionAsync(id, cancellationToken);
        }
        catch (ApiException ex)
        {
			if (ex.StatusCode == 401)
			{
				throw new NotAuthorizedException("Vaše přihlášení vypršelo. Přihlaste se a zkuste to znovu.");
			}

			_ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new ApiNotebookResponseException("Nepodařilo se vymazat oddíl. Zkuste to prosím později.");
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException("Nepodařilo se vymazat oddíl. Zkontrolujte prosím připojení k internetu a zkuste to znovu.");
        }
        catch (Exception ex)
        {
            _ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new Exception("Neočekávaná chyba. O problému víme a pracujeme na nápravě.");
        }
    }

    public async Task<string> GetPageContentAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await apiClient.GetPageContentAsync(id, cancellationToken);
        }
        catch (ApiException ex)
        {
			if (ex.StatusCode == 401)
			{
				throw new NotAuthorizedException("Vaše přihlášení vypršelo. Přihlaste se a zkuste to znovu.");
			}

			_ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new ApiNotebookResponseException("Nepodařilo se načíst obsah stránky. Zkuste to prosím později.");
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException("Nepodařilo se načíst obsah stránky. Zkontrolujte prosím připojení k internetu a zkuste to znovu.");
        }
        catch (Exception ex)
        {
            _ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new Exception("Neočekávaná chyba. O problému víme a pracujeme na nápravě.");
        }
    }

    public async Task AddPageAsync(PageDto page, CancellationToken cancellationToken = default)
    {
        try
        {
            await apiClient.AddPageAsync(page, cancellationToken);
        }
        catch (ApiException ex)
        {
			if (ex.StatusCode == 401)
			{
				throw new NotAuthorizedException("Vaše přihlášení vypršelo. Přihlaste se a zkuste to znovu.");
			}

			_ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new ApiNotebookResponseException("Nepodařilo se uložit stránku. Zkuste to prosím později.");
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException("Nepodařilo se uložit stránku. Zkontrolujte prosím připojení k internetu a zkuste to znovu.");
        }
        catch (Exception ex)
        {
            _ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new Exception("Neočekávaná chyba. O problému víme a pracujeme na nápravě.");
        }
    }

    public async Task UpdatePageAsync(PageDto page, CancellationToken cancellationToken = default)
    {
        try
        {
            await apiClient.UpdatePageAsync(page, cancellationToken);
        }
        catch (ApiException ex)
        {
			if (ex.StatusCode == 401)
			{
				throw new NotAuthorizedException("Vaše přihlášení vypršelo. Přihlaste se a zkuste to znovu.");
			}

			_ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new ApiNotebookResponseException("Nepodařilo se editovat stránku. Zkuste to prosím později.");
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException("Nepodařilo se editovat stránku. Zkontrolujte prosím připojení k internetu a zkuste to znovu.");
        }
        catch (Exception ex)
        {
            _ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new Exception("Neočekávaná chyba. O problému víme a pracujeme na nápravě.");
        }
    }

    public async Task DeletePageAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await apiClient.DeletePageAsync(id, cancellationToken);
        }
        catch (ApiException ex)
        {
			if (ex.StatusCode == 401)
			{
				throw new NotAuthorizedException("Vaše přihlášení vypršelo. Přihlaste se a zkuste to znovu.");
			}

			_ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new ApiNotebookResponseException("Nepodařilo se vymazat stránku. Zkuste to prosím později.");
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException("Nepodařilo se vymazat stránku. Zkontrolujte prosím připojení k internetu a zkuste to znovu.");
        }
        catch (Exception ex)
        {
            _ = apiClient.SendErrorAsync(ex.ToString(), cancellationToken);
            throw new Exception("Neočekávaná chyba. O problému víme a pracujeme na nápravě.");
        }
    }
}