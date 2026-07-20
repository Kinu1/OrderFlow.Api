namespace OrderFlow.Api.Application.DTOs.Common;

public class PagedRespondeDto<T>
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalItems { get; set; }

    public int TotalPages { get; set; }

    public List<T> Items { get; set; } = new();

}