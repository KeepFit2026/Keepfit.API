namespace KeepFit.Backend.Application.DTOs.Requests;

public class PaginationFilter
{
    private int _pageNumber;
    private int _pageSize;

    public int PageNumber { get => _pageNumber; set => _pageNumber = value < 1 ? 1 : value;}

    public int PageSize { get => _pageSize; set => _pageSize = value > 100 ? 100 : value; }

    public PaginationFilter()
    {
        PageNumber = 1;
        PageSize = 10;
    }

    public PaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}