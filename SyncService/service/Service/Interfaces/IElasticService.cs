namespace service.Service.Interfaces;

public interface IElasticService<T>
{
    Task<string> CreateIndex();
    Task<string> CreateDocumentAsync(T document);
    Task<T?> GetDocumentAsync(int id);
    Task<IEnumerable<T>> GetAllDocumentAsync();
    Task<string> UpdateDocumentAsync(T document);
    Task<string> DeleteDocumentAsync(int id);
    Task<IEnumerable<T>> SearchDocument(string field, string value);
}