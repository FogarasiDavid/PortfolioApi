
using Microsoft.EntityFrameworkCore;
using Portfolio.Infrastructure.Database;

namespace Portfolio.Api.BackgroundServices
{
    public class ProjectStatsWorker : BackgroundService
    {
        private readonly ILogger<ProjectStatsWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProjectStatsWorker(ILogger<ProjectStatsWorker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ProjectStatsWorker elindult!");
            //ameddig le nem áll
            while (!stoppingToken.IsCancellationRequested)
            {
                //kiiras
                try
                {
                    //scope keszitese
                    using(var scope = _scopeFactory.CreateScope())
                    {
                        //adatbazis lekerese
                        var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
                        //countolni a projectet
                        var projectCount = await dbContext.Project.CountAsync(stoppingToken);

                        _logger.LogInformation($"[Statisztika] Jelenleg: {projectCount} darab project van tárolva. Idő: {DateTime.Now}");
                    }
                    //ido beállítása
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Hiba történt a statisztika számolása közben!");
                }
            }
            _logger.LogInformation("ProjectStatsWorker leált!");
        }
    }
}
