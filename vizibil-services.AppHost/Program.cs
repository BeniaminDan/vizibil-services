var builder = DistributedApplication.CreateBuilder(args);

var user = builder.AddParameter("DBUser", secret: true);
var password = builder.AddParameter("DBPassword", secret: true);

var postgres = builder.AddPostgres("postgres", user, password)
    .WithEnvironment("POSTGRES_DB", "vizibil")
    .WithDataVolume()
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

var db = postgres.AddDatabase("vizibil");
var rabbitmq = builder.AddRabbitMQ("messaging");

builder.AddProject<Projects.vizibil_services_ApiService>("vizibil-api")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(rabbitmq)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.vizibil_services_EmailService>("vizibil-email")
    .WithReference(rabbitmq);

builder.Build().Run();