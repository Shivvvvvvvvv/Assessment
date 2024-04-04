using Serilog;
using Serilog.Formatting.Compact;
using Serilog.ThrowContext;
using TA.TopUp.API.Middleware;
using TA.TopUp.ApplicationService;
using TA.TopUp.Core.Interfaces.Repositories;
using TA.TopUp.Core.Interfaces.Services;
using TA.TopUp.Infrastructure.DataAccessAbstractions;
using TA.TopUp.Infrastructure.ExternalServices;
using TA.TopUp.Infrastructure.Repositories;
using TA.TopUp.Shared.Options;

Log.Logger = new LoggerConfiguration()
    .Enrich.With<ThrowContextEnricher>()
    .MinimumLevel.Debug()
    .WriteTo.Console(new CompactJsonFormatter())
    .CreateBootstrapLogger();
try
{

    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
    builder.Services.AddDatabase(connectionString);
    builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom
    .Configuration(context.Configuration)
    .Enrich.With<ThrowContextEnricher>()
    .Enrich.FromLogContext());

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IBeneficiaryService,BeneficiaryService>();
    //builder.Services.AddScoped<WalletService>();
    builder.Services.AddHttpClient<WalletService>();
    builder.Services.AddScoped<ITopUPService, TopUpService>();
    builder.Services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
    builder.Services.AddScoped<IUserTransactionsRepository, UserTransactionsRepository>();
    builder.Services.AddScoped<IUserWalletBalancesRepository, UserWalletBalancesRepository>();
    builder.Services.AddScoped<ITopUpOptionsRepository, TopUpOptionsRepository>();
    builder.Services.Configure<BeneficiariesTopUpValidation>(builder.Configuration.GetSection(nameof(BeneficiariesTopUpValidation)));
    builder.Services.Configure<WalletServiceConfig>(builder.Configuration.GetSection(nameof(WalletServiceConfig)));

    // Global error handler
    

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    app.UseHttpsRedirection();
    app.MapControllers();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.Run();
}
catch(Exception ex)
{
    Log.Error(ex, "Application Terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

