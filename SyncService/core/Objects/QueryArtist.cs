using core.Objects.Enum;

namespace core.Objects;

public class QueryArtist
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public bool IsDescending { get; set; } = true;
    public ArtistSearchByEnum SortBy { get; set; } = 0;
    public string SearchTerm { get; set; } = "";
}