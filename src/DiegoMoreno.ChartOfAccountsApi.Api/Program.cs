using DiegoMoreno.ChartOfAccountsApi.Api.Endpoints;
using DiegoMoreno.ChartOfAccountsApi.Api.Extensions;
using DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.Services.RegisterServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.UseCors(ApiConfigurations.CorsPolicyName);
app.MapEndpoints();

app.Run();