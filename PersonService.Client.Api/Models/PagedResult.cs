namespace PersonService.Client.Api.Models;

public class PagedResult<T> where T : class
{
    public T Value { get; set; }
    public int TotalCount { get; set; }
}
