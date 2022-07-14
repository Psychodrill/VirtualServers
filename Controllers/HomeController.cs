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
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace VirtualServers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public VirtualServersDbContext Context=> Startup.Context;

        private static List<VirtualServer> Servers;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            
#if DEBUG
            if (Servers == null)
            {
                Servers = new List<VirtualServer> { new VirtualServer{VirtualServerId=666, CreateDateTime = Convert.ToDateTime("03.04.2021"),RemoveDateTime= Convert.ToDateTime("04.06.2022")},
                                                                  new VirtualServer{VirtualServerId=777, CreateDateTime = Convert.ToDateTime("26.05.2021") } };
            }
#else
            Servers = Context.GetAllServers();
#endif
            return View(Servers);
        }
        
        public IActionResult Add()
        {

#if DEBUG
            Servers.Add(new VirtualServer { VirtualServerId = new Random().Next(0, 1000), CreateDateTime = DateTime.Now });
#else
            Context.AddServer();
            Servers = Context.GetAllServers();

#endif
            return View();
        }
        [HttpPost]
        public void Remove(string servers)
        {

            string format = "dd/MM/yyyy H:mm:ss"; 

            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            

            List<VirtualServer> removedServers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VirtualServer>>(servers,dateTimeConverter);

            foreach (VirtualServer server in removedServers)
            {
#if DEBUG
                VirtualServer virtualServer = Servers.Find(x => x.VirtualServerId == server.VirtualServerId);
                virtualServer.Refresh();
#else
                Context.MarkAsRemoved(server.VirtualServerId);
#endif
            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
