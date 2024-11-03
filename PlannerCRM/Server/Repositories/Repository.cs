namespace PlannerCRM.Server.Repositories;

public class Repository(AppDbContext context, IMapper mapper) : IRepository
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync<TInput>(TInput model)
        where TInput : class
    {
        await _context.Set<TInput>().AddAsync(model);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync<TInput>(int id)
        where TInput : class
    {
        var model = await _context.Set<TInput>().FindAsync(id);
        
        _context.Set<TInput>().Remove(model);

        await _context.SaveChangesAsync();
    }

    public async Task EditAsync<TInput>(TInput model, int id)
        where TInput : class
    {
        var item = await _context.Set<TInput>().FindAsync(id);
        
        item = model;

        _context.Set<TInput>().Update(item);

        await _context.SaveChangesAsync();
    }

    public async Task<TOutput> GetByIdAsync<TInput, TOutput>(int id)
        where TInput : class
        where TOutput : class
    {
        var item = await _context.Set<TInput>().FindAsync(id);

        return _mapper.Map<TOutput>(item);
    }

    public async Task<ICollection<TOutput>> GetWithPagination<TInput, TOutput>(int offset, int limit)
        where TInput : class
        where TOutput : class
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
