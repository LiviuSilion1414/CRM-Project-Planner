namespace PlannerCRM.Server.Repositories;

public class Repository<TInput, TOutput>(AppDbContext context, IMapper mapper) : IRepository<TInput, TOutput>
    where TInput : class
    where TOutput : class
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public virtual async Task AddAsync(TInput model)
    {
        await _context.Set<TInput>().AddAsync(model);

        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var model = await _context.Set<TInput>().FindAsync(id);
        
        _context.Set<TInput>().Remove(model);

        await _context.SaveChangesAsync();
    }

    public virtual async Task EditAsync(TInput model, int id)
    {
        var item = await _context.Set<TInput>().FindAsync(id);
        
        item = model;

        _context.Set<TInput>().Update(item);

        await _context.SaveChangesAsync();
    }

    public virtual async Task<TOutput> GetByIdAsync(int id)
    {
        var item = await _context.Set<TInput>().FindAsync(id);

        return _mapper.Map<TOutput>(item);
    }

    public virtual async Task<ICollection<TOutput>> GetWithPagination(int offset, int limit)
    {
        var items = await _context
            .Set<TInput>()
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return items
            .Select(_mapper.Map<TOutput>)
            .ToList();
    }
}
