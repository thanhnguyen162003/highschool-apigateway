var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Yarp_APIGateway>("yarp-apigateway");

builder.Build().Run();
