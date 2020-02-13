# Commands
Il comando altro non è che un "Ordine di esecuzione" che talvolta può generare una risposta.

```cs
public class AddCoinsMachineCommand : IRequest
{
    public int MachineId { get; set; }
    public decimal CoinsAdded { get; set; }
}
```


# Events
Gli eventi, non sono ordini, ma bensì notifiche. Gli eventi possono essere persi mentre gli ordini devono arrivare a destinazione!

```cs
public class CoinNotificationEvent : INotification
{
    public CoinNotificationEvent(int machineId, decimal coins, CoinOperation operation)
    {
        MachineId = machineId;
        Coins = coins;
        Operation = operation;
    }
    public int MachineId { get; }
    public decimal Coins { get; }
    public CoinOperation Operation { get; }
}

public enum CoinOperation
{
    Add,
    Subtract,
    Collect,
    Sell
}
```


### Handler
Un handler è quella classe che gestisce tutti i tipi di evento. Sia comandi che eventi.
```cs
public class CoinsHandler :
    IRequestHandler<AddCoinsMachineCommand, Unit>,    
    INotificationHandler<CoinNotificationEvent>
{
    private readonly IMediator mediator;
    private readonly IMachinesUoW machinesUoW;

    public CoinsHandler(IMediator mediator, IMachinesUoW machinesUoW)
    {
        this.mediator = mediator;
        this.machinesUoW = machinesUoW;
    }

    // Add coins in machine
    public async Task<Unit> Handle(AddCoinsMachineCommand request, CancellationToken cancellationToken)
    {
        var machine = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
        machine.AddCoins(request.CoinsAdded);
        await machinesUoW.SaveAsync().ConfigureAwait(false);

        await mediator
            .Publish(new CoinNotificationEvent(request.MachineId, request.CoinsAdded, CoinOperation.Add))
            .ConfigureAwait(false);
        return new Unit();
    }

    public async Task Handle(CoinNotificationEvent notification, CancellationToken cancellationToken)
    {
        // Send Payload to SignalR
        switch (notification.Operation)
        {
            case CoinOperation.Add:
                break;
            case CoinOperation.Subtract:
                break;
            case CoinOperation.Collect:
                break;
        }
        await Task.FromResult(1);
    }
}
```