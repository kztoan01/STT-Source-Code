using core.Objects.Enum;

namespace core.Objects;

public class QueryArtist
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool IsDescending { get; set; }
    public ArtistSearchByEnum SortBy { get; set; }
    public string SearchTerm { get; set; }
}