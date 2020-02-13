# Repository
Il concetto di repository è già intrinseco in EF, ma può tornare utile astrarsi in taluni casi. Ad esempio se fosse necessario pre-caricare le navigation propery questo è il posto corretto.


Unit of work
```cs
public class MachinesUoW : IMachinesUoW
{
    private readonly MachineContext db;
    private readonly ILogger logger;

    public IRepository<MachineItem> MachineRepository { get; }
    public IRepository<MachineType> MachineTypeRepository { get; }

    public MachinesUoW(MachineContext db, 
        IRepository<MachineItem> machineRepository, 
        IRepository<MachineType> machineTypeRepository,
        ILoggerFactory loggerFactory)
    {
        this.db = db;
        this.logger = loggerFactory.CreateLogger(typeof(IMachinesUoW));
        MachineRepository = machineRepository;
        MachineTypeRepository = machineTypeRepository;
    }

    public void Save()
    {
        this.SaveAsync().Wait();
    }

    public async Task SaveAsync()
    {
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException duUpdateEx)
        {
            logger.LogError(duUpdateEx, "Error to save data in context id: {contextId}", db.ContextId);
            throw duUpdateEx;
        }
    }
}
```


```cs
public class MachineItemRepository : IRepository<MachineItem>
{
    private readonly MachineContext db;

    public MachineItemRepository(MachineContext db)
    {
        this.db = db;
    }

    public async Task<MachineItem> AddAsync(MachineItem element)
    {
        var entityResult = (await db.AddAsync(element).ConfigureAwait(false)).Entity;
        return entityResult;
    }

    public async Task<MachineItem> DeleteAsync(MachineItem element)
    {
        var resultEntity = db.Remove(element).Entity;
        db.RemoveRange(resultEntity.ActiveProducts);
        db.RemoveRange(resultEntity.HistoryProducts);
        return await Task.FromResult(resultEntity);
    }

    public async Task<MachineItem> FindAsync(int id)
    {
        return await db.Machines
            .Include(x => x.ActiveProducts)
            .Include(x => x.HistoryProducts)
            .FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);
    }

    public async Task<MachineItem> UpdateAsync(MachineItem element)
    {
        var resultEntity = db.Machines.Update(element).Entity;
        return await Task.FromResult(resultEntity);
    }
}
```