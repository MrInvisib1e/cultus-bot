using CultusBot;
using CultusBot.Data.Repositories;
using CultusBot.Data;
using CultusBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CultusBot.Commands;

CancellationTokenSource cts = new();
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddScoped<IBirthdayService, BirthdayService>();
builder.Services.AddScoped<ITextService, TextService>();
builder.Services.AddScoped<ICommandManager, CommandManager>();
builder.Services.AddScoped<IScheduleManager, ScheduleManager>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();
builder.Services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

using IHost host = builder.Build();

var serviceProvider = host.Services;
var commandManager = serviceProvider.GetRequiredService<ICommandManager>();
var scheduleManager = serviceProvider.GetRequiredService<IScheduleManager>();
var userService = serviceProvider.GetRequiredService<IUserService>();

// Initialize your bot and start handling updates
InitializeBot.Init(commandManager, scheduleManager, userService, cts, builder.Configuration.GetSection("BotId").Value);

await host.RunAsync();
cts.Cancel();