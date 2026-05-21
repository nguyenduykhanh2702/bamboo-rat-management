public static class CageQueryableExtensions
{
    public static IQueryable<Cage> ApplySorting(
        this IQueryable<Cage> query,
        string? orderBy)
    {
        return orderBy?.ToLower() switch
        {
            "name" => query.OrderBy(x => x.Name),
            "name desc" => query.OrderByDescending(x => x.Name),
            "capacity" => query.OrderBy(x => x.Capacity),
            "capacity desc" => query.OrderByDescending(x => x.Capacity),
            "createddate" => query.OrderBy(x => x.CreatedDate),
            "createddate desc" => query.OrderByDescending(x => x.CreatedDate),
            _ => query.OrderBy(x => x.Name)
        };
    }
}