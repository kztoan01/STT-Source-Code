using Elastic.Clients.Elasticsearch;
using service.Service.Interfaces;

namespace service.Service;

public class ElasticService<T> : IElasticService<T> where T : class
{
    private readonly ElasticsearchClient _elasticClient;

    public ElasticService(ElasticsearchClient elasticsearchClient)
    {
        _elasticClient = elasticsearchClient;
    }

    public async Task<string> CreateIndex()
    {
        var response = await _elasticClient.Indices.CreateAsync("testhehe");
        return response.IsValidResponse ? "create index success" : "false";
    }

    public async Task<string> CreateDocumentAsync(T document)
    {
        var response = await _elasticClient.IndexAsync(document);
        return response.IsValidResponse ? "Create document success" : "Create document false";
    }

    public async Task<T?> GetDocumentAsync(int id)
    {
        var response = await _elasticClient.GetAsync<T>(1, idx => idx.Index("test"));
        return response.IsValidResponse ? response.Source : null;
    }

    public async Task<IEnumerable<T>> GetAllDocumentAsync()
    {
        var searchResponse = await _elasticClient.SearchAsync<T>(s => s
            .Index("my-index")
            .Query(q => q
                .MatchAll(s => { })
            )
        );
        return searchResponse.Documents;
    }

    public async Task<string> UpdateDocumentAsync(T document)
    {
        var response = await _elasticClient.IndexAsync(document);
        return response.IsValidResponse ? "Update document success" : "Update document false";
    }

    public Task<string> DeleteDocumentAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> SearchDocument(string field, string value)
    {
        var searchResponse = await _elasticClient.SearchAsync<T>(s => s
            .From(0)
            .Size(100)
            .Query(q => q
                .Match(m => m
                    .Field(field!)
                    .Query(value)
                )
            )
        );
        return searchResponse.Documents;
    }
}