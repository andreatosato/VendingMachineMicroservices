# Domain Drive Design
- vending machine 
	- machine type: macchina, tipo, modello
	- machine item: (soldi raccolti dall'installazione, soldi raccolti nel mese, soldi raccolti nell'anno, 
							 lista prodotti inseriti nel tempo, lista prodotti ancora attivi, posizione gps,
							 ultima data caricamento prodotti, ultima data pulizia, ultima data raccolta monete)
	Machine Type: descrive il singolo modello della macchina.
	Machine item è la macchina installata presso il cliente
- products
	- product: [prodotto base] (caratteristiche, marca, tipo di erogazione, prezzo, temperature di conservazione)
	- product items: [prodotto a listino] (data acquisto, data scadenza, *vending machine item*).
	Products descrive il prodotto
	Product item descrive il singolo prodotto nella vending machine.
- orders
	- order: (product items list, soldi inseriti, resto, vending machine item, data acquisto)


MachineItem
```cs
public class MachineItem : Entity, IAggregateRoot
{
    private readonly DateTime _dataCreated;
    private readonly DateTime? _dataUpdated;

    public MapPoint Position { get; set; }
    public decimal? Temperature { get; set; }
    // Start or Stop or Not Set
    public bool? Status { get; set; }

    public MachineType MachineType { get; private set; }

    public List<ProductConsumed> HistoryProducts { get; } = new List<ProductConsumed>();
    public List<Product> ActiveProducts { get; } = new List<Product>();
    public DateTimeOffset? LatestLoadedProducts { get; private set; }
    public DateTimeOffset? LatestCleaningMachine { get; private set; }
    public DateTimeOffset? LatestMoneyCollection { get; private set; }
    public decimal CoinsInMachine { get; private set; }
    public decimal CoinsCurrentSupply { get; private set; }

    public MachineItem(MachineType machineType)
        : this()
    {
        MachineType = machineType;
    }

    // Testing only
    protected MachineItem()
    {
        _dataCreated = DateTime.UtcNow;
    }

    /// <summary>
    /// Update quantity in machine, no money, no rest. Only products in machine data.
    /// </summary>
    /// <param name="productToBuy"></param>
    public void BuyProducts(List<Product> productToBuy)
    {
        DateTimeOffset providedDate = DateTimeOffset.UtcNow;
        foreach (var p in productToBuy)
        {
            BuyProduct(p, providedDate);
        }
        AddDomainEvent(new MachineItemUpdatedEvent(Id));
    }

    protected void BuyProduct(Product productToBuy, DateTimeOffset providedDate)
    {
        var productConsumed = new ProductConsumed(productToBuy);
        productConsumed.SetProvidedDate(providedDate);
        HistoryProducts.Add(productConsumed);
        var productToRemove = ActiveProducts.Find(t => t.Id == productToBuy.Id);
        ActiveProducts.Remove(productToRemove);
    }

    public void LoadProducts(List<Product> productsToLoad, DateTimeOffset? dateToLoad = null)
    {
        var loadProductDate = dateToLoad ?? DateTimeOffset.UtcNow;
        LatestLoadedProducts = loadProductDate;
        foreach (var p in productsToLoad)
        {
            LoadProduct(p, loadProductDate);
        }
        AddDomainEvent(new MachineItemUpdatedEvent(Id));
    }

    protected void LoadProduct(Product productToLoad, DateTimeOffset loadDate)
    {
        if(!productToLoad.IsActive)
            productToLoad.SetActivationDate(loadDate);

        if(ActiveProducts.Find(t => t.Id == productToLoad.Id) == null)
        {
            ActiveProducts.Add(productToLoad);
            HistoryProducts.Add(new ProductConsumed(productToLoad));
        }
        else
        {
            // Product already exists
        }
    }

    public void AddCoins(decimal coinAdded)
    {
        CoinsInMachine += coinAdded;
        CoinsCurrentSupply += coinAdded;
        AddDomainEvent(new MachineItemUpdatedEvent(Id));
    }

    public decimal RestCoins()
    {
        decimal coinsToRest = CoinsCurrentSupply;
        CoinsInMachine -= CoinsCurrentSupply;
        CoinsCurrentSupply = 0;
        AddDomainEvent(new MachineItemUpdatedEvent(Id));
        return coinsToRest;
    }

    // When Buy Products and confirm, we must subtract supply coin to transactions
    public void SupplyCoins(decimal coinsSupply)
    {
        CoinsCurrentSupply -= coinsSupply;
        CoinsInMachine -= coinsSupply;
        AddDomainEvent(new MachineItemUpdatedEvent(Id));
    }

    public void CollectMoney()
    {
        LatestMoneyCollection = DateTimeOffset.UtcNow;
        CoinsInMachine = 0;
        AddDomainEvent(new MachineItemUpdatedEvent(Id));
    }

    public void CleanMachine()
    {
        LatestCleaningMachine = DateTimeOffset.UtcNow;
        AddDomainEvent(new MachineItemUpdatedEvent(Id));
    }
}
```

MachineType

```cs
public class MachineType : Entity, IAggregateRoot
{
    public string Model { get; }
    public MachineVersion Version { get; }
    public MachineType(string model, MachineVersion version)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentNullException("Machine type model must be a value");
        }
        Model = model;
        Version = version;
    }
    public enum MachineVersion
    {
        Coffee,
        Frigo,
        FrigoAndCoffee
    }
}
```

# Domain Events
I Domain Events consentono di notificare a tutti gli interessati (Applicazioni realtime ma anche applicazioni di business) che un particolare oggetto è stato modificato, creato o aggiunto.
Gli eventi di dominio vengono elaborati subito dopo il Salvataggio del contesto di EF. Vedi DbContext.

```cs
public class MachineItemCreatedEvent: INotification
{
    public int Id { get; set; }
}

public class MachineItemUpdatedEvent : INotification
{
    public MachineItemUpdatedEvent(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}
```