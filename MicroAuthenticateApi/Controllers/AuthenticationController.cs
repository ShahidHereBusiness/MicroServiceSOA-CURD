using MicroServicesSOA.DbContext;
using MicroServicesSOA.SOA;
using MicroServicesSOA.SOAV.Components;
using Microsoft.AspNetCore.Mvc;

namespace MicroAuthenticateApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthenticationController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
        Diminish.AppName = _config.GetSection("ConnectionStrings:AppName").Value;
        Diminish.AppCode = _config.GetSection("ConnectionStrings:APICode").Value?.ToString() ?? "MineCode";
        Diminish.AppDiminishPath = _config.GetSection("ConnectionStrings:MapPath").Value;
        Diminish.AppTurnaround = Double.Parse(_config.GetSection("ConnectionStrings:APILoad").Value?.ToString() ?? "1");
        Diminish.AppNameSuffix = $"{DateTime.Now.ToString("yyyyMMdd")}";
        Diminish.connectionString = $"Data Source={Diminish.AppDiminishPath}{Diminish.AppCode}{Diminish.AppTurnaround}{Diminish.AppName}{Diminish.AppNameSuffix}.db;Version=3;";
        Diminish.AccountExpiry = int.Parse(_config.GetSection("Accounts:ExpiryDays").Value ?? "30");
    }
    [Route("/Authenticate")]
    [HttpPost]
    public ResponseUser Login(InstanceUser user)
    {
        using (InstanceService? srv = new InstanceService())
        {
            return srv.Indorse(user, _context);
        }
    }
}