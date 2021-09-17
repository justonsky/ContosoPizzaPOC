using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ContosoPizza.Models;
using ContosoPizza.Services;
using ContosoPizza.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoPizza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PizzaController : BaseController
    {
        private IHubContext<CustomerHub> _customerHub;

        public PizzaController(IHubContext<CustomerHub> customerHub)
        {
            _customerHub = customerHub;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            ViewData["PizzaList"] = PizzaService.GetAll();
            return View();
        }

        // GET all action
        [HttpGet]
        public IActionResult GetAll()
        {
            var pizza = PizzaService.GetAll();
            _customerHub.Clients.Client(HubConnectionId).SendAsync("ReceiveMessage", pizza);
            return NoContent();
        }

        // GET by Id action
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            
            var pizza = PizzaService.Get(id);

            if (pizza == null)
                _customerHub.Clients.Client(HubConnectionId).SendAsync("ReceiveMessage", null);
                
            _customerHub.Clients.Client(HubConnectionId).SendAsync("ReceiveMessage", pizza);

            return NoContent();
        }

        // POST action
        [HttpPost]
        public IActionResult Create([Bind("Id,Name,IsGlutenFree")] Pizza pizza)
        {
            Thread.Sleep(5000);
            PizzaService.Add(pizza);
            _customerHub.Clients.Client(HubConnectionId).SendAsync("ReceiveMessage", pizza);
            return NoContent();
        }
    }
}