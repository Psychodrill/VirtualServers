using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VirtualServers.Models;
using VirtualServers;

namespace VirtualServers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public VirtualServersDbContext Context=> Startup.Context;

        public List<VirtualServer> Servers;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            Context.GetAllServers();
            //Servers = new List<VirtualServer> { new VirtualServer{VirtualServerId=666, CreateDateTime = Convert.ToDateTime("03.04.2021"),RemoveDateTime= Convert.ToDateTime("04.06.2022")},
            //                                                      new VirtualServer{VirtualServerId=777, CreateDateTime = Convert.ToDateTime("26.05.2021") } };
            return View(Servers);
        }
        
        public IActionResult Add()
        {
            Context.AddServer();
            return View();
        }
        
        public IActionResult Remove(IEnumerable<VirtualServer> servers)
        {
            var removedServers = servers.Where(x => x.SelectedForRemove == true).ToList();
            foreach(VirtualServer removedServer in removedServers)
            {
                Context.MarkAsRemoved(removedServer.VirtualServerId);
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
