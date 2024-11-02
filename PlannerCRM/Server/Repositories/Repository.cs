namespace PlannerCRM.Server.Repositories;

public class Repository<TInput>(AppDbContext context) : IRepository<TInput>
    where TInput : class
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(TInput model)
    {
        await _context.Set<TInput>().AddAsync(model);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _context.Set<TInput>().FindAsync(id);
        
        _context.Set<TInput>().Remove(model);

        await _context.SaveChangesAsync();
    }

    public async Task EditAsync(TInput model, int id)
    {
        var item = await _context.Set<TInput>().FindAsync(id);
        
        item = model;

        _context.Set<TInput>().Update(item);

        await _context.SaveChangesAsync();
    }

    public async Task<TInput> GetByIdAsync(int id)
    {
        return await _context.Set<TInput>().FindAsync(id);
    }

    public async Task<ICollection<TInput>> GetWithPagination(int offset, int limit)
    {
        return await _context
            .Set<TInput>()
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }
}
