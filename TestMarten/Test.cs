using WebApplication1.Services;
using WebApplication1.Services.SupplierRepository;

namespace WebApplication1;

public class Test : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;


    public Test(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        
        // simulate injecting tenant id
        var tenancyContext = scope.ServiceProvider.GetRequiredService<DummyTenancyContext>();
        tenancyContext.TenantId = "tenant1";
        
        var supplierRepository = scope.ServiceProvider.GetRequiredService<SupplierRepository.ISupplierRepository>();

        var id = new Guid("0255bc61-e334-598c-8721-208558128c98");
        
        for ( var i = 0; i < 10; i++ )
        {
            Console.WriteLine($"iterations for loop {i}");
            
            var model = supplierRepository.Find(id) ?? SupplierRepository.SupplierAggregate.CreateSupplierAggregate(id);
            supplierRepository.PersistUpsert(model);
        }
        return Task.CompletedTask;
    }
}