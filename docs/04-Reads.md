# Read Models
Il Read models consente la lettura dei dati, ma mai la scrittura.

Il dominio di lettura a volte richiede molta complessità ed EF non sempre ci consente di selezionare i dati corretti.
Ecco quindi che la scelta di un ulteriore framework ci può aiutare in questo.
**Dapper** è un ottimo Micro ORM e ci aiuta a fare query e a mappare i dati sugli oggetti in automatico.

```cs
public class MachinesQuery : IMachineQuery
{
    private readonly string machineConnectionString;

    public MachinesQuery(string machineConnectionString)
    {
        this.machineConnectionString = machineConnectionString;
    }

    public async Task<ProductsReadModel> GetProductsInMachineAsync(int machineId)
    {
        ProductsReadModel result = new ProductsReadModel();
        using (SqlConnection connection = new SqlConnection(machineConnectionString))
        {
            var article = await connection
                .QueryAsync<ProductReadModel>(@"SELECT Id
                    FROM dbo.ActiveProducts
                    Where MachineItemId = @Id", new { Id = machineId })
                .ConfigureAwait(false);
            result.Products.AddRange(article);
        }
        return result;
    }
}
```

