namespace LemonLaw.Shared.DTOs;

public class CommonResponseDto<T>
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public string ExceptionMessage { get; set; } = string.Empty;
    public List<string> ValidationErrors { get; set; } = new();
    public T? Data { get; set; }
}

public class PagedResponseDto<T> : CommonResponseDto<T>
{
    public int TotalRecords { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
