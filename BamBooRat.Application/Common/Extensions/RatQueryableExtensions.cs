public static class RatQueryableExtensions
{
    public static IQueryable<Rat> ApplySorting(
        this IQueryable<Rat> query,
        string? orderBy)
    {
        return orderBy?.ToLower() switch
        {
            "name" => query.OrderBy(x => x.Name),
            "code" => query.OrderBy(x => x.Code),
            "name desc" => query.OrderByDescending(x => x.Name),
            "createddate" => query.OrderBy(x => x.CreatedDate),
            "createddate desc" => query.OrderByDescending(x => x.CreatedDate),
            _ => query.OrderBy(x => x.Name)
        };
    }
}