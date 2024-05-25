using Microsoft.AspNetCore.Mvc;

namespace Moto.Controllers.DTO
{
    public class PageQueryParameters
    {
        [FromQuery] public int Page { get; set; }
        [FromQuery] public int PageSize { get; set; } = 20;
        [FromQuery] public float MinRating { get; set; } = 0;
        [FromQuery] public float MaxRating { get; set; } = 5;
        [FromQuery] public float MinCost { get; set; } = 0;
        [FromQuery] public float MaxCost { get; set; } = -1;
        [FromQuery] public string? NameQuery { get; set; }
    }
}
