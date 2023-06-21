using System.Data;
using System.Text.Json.Serialization;
using Marten;

namespace WebApplication1.Services.SupplierRepository;

public class SupplierRepository : SupplierRepository.ISupplierRepository
{
    private readonly IDocumentSession _documentSession;
    private readonly ILogger<SupplierRepository> _logger;

    public SupplierRepository(IDocumentSession documentSession, ILogger<SupplierRepository> logger )
    {
        _documentSession = documentSession;
        _logger = logger;
    }

    public SupplierAggregate? Find(Guid id)
    {
        return _documentSession.Load<SupplierAggregate>(id);
    }

    public void PersistUpsert(SupplierAggregate supplierAggregateModel)
    {
        Console.WriteLine($"msg='trying to save to db', supplierId={supplierAggregateModel.Id}");
        _documentSession.Store(supplierAggregateModel);
        _documentSession.SaveChanges();
        Console.WriteLine($"msg='trying to save to db', supplierId={supplierAggregateModel.Id}");
    }

    public class SupplierAggregate 
    {
        
        //Used by Marten https://martendb.io/documents/identity.html
        public Guid Id { get; set; }

       
        [JsonConstructor]
        public SupplierAggregate(Guid id)
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
        SupplierAggregate? Find(Guid id);
        void PersistUpsert(SupplierAggregate supplierAggregateModel);
    }
    
}
