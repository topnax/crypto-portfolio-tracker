using System;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoStatsSource;
using Database;
using ElectronNET.API;
using ElectronNET.API.Entities;
using MatBlazor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Services;
using SqlKata.Compilers;

namespace WebFrontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<HttpClient>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMatBlazor();
            
            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomCenter;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });

            services.AddSingleton<ICryptoStatsSource, CoingeckoSource>();

            // TODO ensure that SqlKataDatabase gets disposed
            var dbConnection = new SqliteConnection("Data Source=data.db");
            var db = new SqlKataDatabase(dbConnection, new SqliteCompiler());
            services.AddSingleton(ctx => db);

            services.AddSingleton<ICryptocurrencyResolver, CryptocurrencyResolverImpl>();
            services.AddSingleton<ISummaryService, SummaryServiceImpl>();
            
            services.AddSingleton<IPortfolioRepository, SqlKataPortfolioRepository>();
            services.AddSingleton<IMarketOrderRepository, SqlKataMarketOrderRepository>();
            services.AddSingleton<IPortfolioEntryRepository, SqlKataPortfolioEntryRepository>();

            services.AddSingleton<IPortfolioService, PortfolioServiceImpl>();
            services.AddSingleton<IMarketOrderService, MarketOrderServiceImpl>();
            services.AddSingleton<IPortfolioEntryService, PortfolioEntryServiceImpl>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            if (HybridSupport.IsElectronActive)
            {
                ElectronBootstrap();
            }            
        }
        
        public async void ElectronBootstrap()
        {
            var url = new BrowserWindowOptions();
            url.Show = false;
            url.Height = 940;
            url.Width = 1152;
            var browserWindow = await Electron.WindowManager.CreateWindowAsync(url);
            await browserWindow.WebContents.Session.ClearCacheAsync();
            browserWindow.RemoveMenu();
            browserWindow.OnReadyToShow += () => browserWindow.Show();
            browserWindow.SetTitle("Crypto Portfolio Tracker");
        }
    }
}