using WebApplication1.Services.SupplierRepository;

namespace WebApplication1;

public class Test : BackgroundService
{
    
    public SupplierRepository.ISupplierRepository _supplierRepository;

    public Test(IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope(); 
        var supplierRepository = scope.ServiceProvider.GetRequiredService<SupplierRepository.ISupplierRepository>();
        _supplierRepository = supplierRepository;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        for ( var i = 0; i < 10; i++ )
        {
            Console.WriteLine($"iterations for loop {i}");
            var model = SupplierRepository.SupplierAggregate.CreateSupplierAggregate(new Guid("0255bc61-e334-598c-8721-208558128c98"));
            _supplierRepository.PersistUpsert(model);
        }
        return Task.CompletedTask;
    }
}