using JasperFx.CodeGeneration;
using Marten;
using Marten.Schema.Identity;
using Marten.Services.Json;
using Weasel.Core;
using WebApplication1;
using WebApplication1.Services;
using WebApplication1.Services.SupplierRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DummyTenancyContext>();
builder.Services.AddScoped<SupplierRepository.ISupplierRepository, SupplierRepository>();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddMarten(opts =>
        {
            // Establish the connection string to your Marten database

            opts.MultiTenantedDatabases(x =>
            {
                //input connectionstring and run
                x.AddSingleTenantDatabase("connectionString",
                    "tenant1");
                x.AddSingleTenantDatabase("connectionString",
                    "tenant2");
            });
            opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            opts.RegisterDocumentType<SupplierRepository.SupplierAggregate>();
            
            opts.UseDefaultSerialization(
                serializerType: SerializerType.SystemTextJson,
                // Optionally override the enum storage
                enumStorage: EnumStorage.AsString,
                // Optionally override the member casing
                casing: Casing.Default,
                //marten can serialize private classes with private ctors and properties
                nonPublicMembersStorage: NonPublicMembersStorage.NonPublicConstructor 
            );
            
            opts.Policies.ForAllDocuments(m =>
            {
                if (m.IdType == typeof(Guid))
                {
                    m.IdStrategy = new CombGuidIdGeneration();
                }
                m.Metadata.Headers.Enabled = true;
                m.UseOptimisticConcurrency = true;
            });
            
            //dependent on Environment.IsDevelopment()
            opts.GeneratedCodeMode = TypeLoadMode.Auto;
        })
    .UseLightweightSessions()
    .BuildSessionsWith<TenantedSessionFactory>(ServiceLifetime.Scoped);

builder.Services.AddHostedService<Test>();

var app = builder.Build();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();