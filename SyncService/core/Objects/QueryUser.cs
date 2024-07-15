using core.Objects.Enum;

namespace core.Objects;

public class QueryUser
{
    public string? Name { get; set; } = null;
    public UserSearchByEnum? SortBy { get; set; } = 0;
    public bool IsDecsending { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}