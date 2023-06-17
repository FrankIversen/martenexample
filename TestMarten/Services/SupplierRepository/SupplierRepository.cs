using System.Data;
using Marten;

namespace WebApplication1.Services.SupplierRepository;

public class SupplierRepository : SupplierRepository.ISupplierRepository
{
    private readonly IDocumentStore _documentStore;
    private readonly ILogger<SupplierRepository> _logger;

    public SupplierRepository(IDocumentStore documentStore, ILogger<SupplierRepository> logger )
    {
        _documentStore = documentStore;
        _logger = logger;
    }

    public void PersistUpsert(SupplierAggregate supplierAggregateModel)
    {
        Console.WriteLine($"msg='trying to save to db', supplierId={supplierAggregateModel.Id}");
        using var dbSession = _documentStore.LightweightSession("tenant1", IsolationLevel.Serializable);
        dbSession.Store(supplierAggregateModel);
        dbSession.SaveChanges();
        Console.WriteLine($"msg='trying to save to db', supplierId={supplierAggregateModel.Id}");
    }

    public class SupplierAggregate 
    {
        
        //Used by Marten https://martendb.io/documents/identity.html
        public Guid Id { get; set; }

       
        private SupplierAggregate(Guid id)
        {
            Id = id;
        }

        //For delete purpose
        public static SupplierAggregate CreateSupplierAggregate(Guid id)
        {
            return new SupplierAggregate(id);            
        }

    }
    public interface ISupplierRepository
    {
        void PersistUpsert(SupplierAggregate supplierAggregateModel);
    }
    
}
