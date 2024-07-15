using core.Objects.Enum;

namespace core.Objects;

public class QueryAlbum
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool IsDescending { get; set; } = false;
    public AlbumSearchByEnum SortBy { get; set; } = AlbumSearchByEnum.AlbumTitle;
    public string SearchTerm { get; set; }
}