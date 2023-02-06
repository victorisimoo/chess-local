using Autofac;
using Autofac.Extensions.DependencyInjection;
using chessAPI;
using chessAPI.business.interfaces;
using chessAPI.models.player;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using chessAPI.models.game;
using Serilog.Events;

//Serilog logger (https://github.com/serilog/serilog-aspnetcore)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("chessAPI starting");
    var builder = WebApplication.CreateBuilder(args);

    var connectionStrings = new connectionStrings();
    builder.Services.AddOptions();
    builder.Services.Configure<connectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
    builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

    // Two-stage initialization (https://github.com/serilog/serilog-aspnetcore)
    builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom
             .Configuration(context.Configuration)
             .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning).ReadFrom
             .Services(services).Enrich
             .FromLogContext().WriteTo
             .Console());

    // Autofac como inyección de dependencias
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new chessAPI.dependencyInjection<int, int>()));
    var app = builder.Build();
    app.UseSerilogRequestLogging();
    app.UseMiddleware(typeof(chessAPI.customMiddleware<int>));
    app.MapGet("/", () =>
    {
        return "hola mundo";
    });


    /**
        * Player    
    */
    app.MapGet("player/{id}",
    [AllowAnonymous] async(IPlayerBusiness<int> bs, int id) => {
        var player = await bs.getPlayerById(id);
        if (player != null) return Results.Ok(player);
        return Results.NotFound(id);
    });

    app.MapPut("player/{id}",
    [AllowAnonymous] async(IPlayerBusiness<int> bs, int id, clsNewPlayer newPlayer) => {
        var player = await bs.updatePlayer(id, newPlayer);
        if (player != null) return Results.Ok(player);
        return Results.NotFound(id);
    });

    app.MapPost("player", 
    [AllowAnonymous] async(IPlayerBusiness<int> bs, clsNewPlayer newPlayer) => Results.Ok(await bs.addPlayer(newPlayer)));

    /**
        * Game
    */

    app.MapGet("game/{id}",
    [AllowAnonymous] async(IGameBusiness<int> bs, int id) => {
        var game = await bs.getGameById(id);
        if (game != null) return Results.Ok(game);
        return Results.NotFound(id);
    });

    app.MapPost("game",
    [AllowAnonymous] async(IGameBusiness<int> bs, clsNewGame newGame) => {
        var game = await bs.createGame(newGame);
        if (game != null) return Results.Ok(game);
        return Results.NotFound(newGame.whites);
    });

    app.MapPut("game/{id}",
    [AllowAnonymous] async(IGameBusiness<int> bs, int id, clsUpdateGame newGame) => {
        var game = await bs.updateGame(id, newGame);
        if (game != null) return Results.Ok(game);
        return Results.NotFound(id);
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "chessAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
