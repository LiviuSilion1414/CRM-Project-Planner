namespace PlannerCRM.Client.Services.Crud;

public class CrudService<TEntity>
{
    public void Add(TEntity entity)
    { 
        throw new NotImplementedException();
    }

    public void Edit(TEntity entity) 
    { 
        throw new NotImplementedException();
    }

    public void Delete(int entityId) 
    { 
        throw new NotImplementedException();
    }

    public Task<TEntity> GetById(int entityId)
    { 
        throw new NotImplementedException();
    }

    public ICollection<Task<TEntity>> GetWithPagination(int offset, int limit)
    { 
        throw new NotImplementedException();
    }
}
