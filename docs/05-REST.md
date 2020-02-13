# Controller
Il controller, con l'utilizzo del pattern Mediatr, viene completamente svuotato di logica.
```cs
[ApiController]
[Route("[controller]")]    
public class CoinsController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly ILogger logger;

    public CoinsController(IMediator mediator, ILoggerFactory loggerFactory)
    {
        this.mediator = mediator;
        this.logger = loggerFactory.CreateLogger<CoinsController>();
    }

    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Add([FromBody] AddCoinsViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await mediator.Send(new AddCoinsMachineCommand()
                {
                    CoinsAdded = model.Coins,
                    MachineId = model.MachineId
                });
                logger.LogInformation("Coins Added: {@Coins} in machine: {@MachineId}", model.Coins, model.MachineId);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("AddCoins", ex);
                throw;
            }
        }
        return BadRequest(ModelState);
    }
}
```

# Validation
La validazione di ASP .NET Core è troppo basica, a volte è necessario definire logiche più complesse ed ecco che FluentValidator può essere utilizzato!
```cs
public class AddCoinsValidation : AbstractValidator<AddCoinsViewModel>
{
    public AddCoinsValidation()
    {
        RuleFor(t => t.Coins).NotEmpty().GreaterThan(0);
        RuleFor(t => t.MachineId).NotEmpty().GreaterThan(0);
    }
}
```
Vedere **MachineItemValidation** per ulteriori meccanismi.


# Binder
Vedere il flusso della classe: **BuyProductsViewModel** che prende dalla rotta il MachineId e lo inserisce nel modello per poi richiamare una unica logica di Validazione.
