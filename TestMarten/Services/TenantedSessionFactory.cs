using System.Data;
using Marten;

namespace WebApplication1.Services;

public class TenantedSessionFactory: ISessionFactory
{
    private readonly IDocumentStore _store;
    private readonly DummyTenancyContext _tenancyContext;

    // This is important! You will need to use the
    // IDocumentStore to open sessions
    public TenantedSessionFactory(IDocumentStore store, DummyTenancyContext tenancyContext)
    {
        _store = store;
        _tenancyContext = tenancyContext;
    }

    public IQuerySession QuerySession()
    {
        return _store.QuerySession(_tenancyContext.TenantId);
    }

    public IDocumentSession OpenSession()
    {
        // Opting for the "lightweight" session
        // option with no identity map tracking
        // and choosing to use Serializable transactions
        // just to be different
        return _store.LightweightSession(_tenancyContext.TenantId, IsolationLevel.Serializable);
    }
}

public class DummyTenancyContext
{
    // this should be normally taken from the http context or other
    public string? TenantId { get; set; }
}