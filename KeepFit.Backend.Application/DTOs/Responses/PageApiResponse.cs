namespace KeepFit.Backend.Application.DTOs.Responses;

public class PageApiResponse<T> : ApiResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    
    public bool HasPreviousPage => PageNumber > 1; 
    public bool HasNextPage => PageNumber < TotalPages;
    
    public PageApiResponse(T data, int pageNumber, int pageSize, int totalRecords)
        : base(true, data, null)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        
        TotalPages = (int)Math.Ceiling((double)totalRecords / (double)pageSize);
    }
}