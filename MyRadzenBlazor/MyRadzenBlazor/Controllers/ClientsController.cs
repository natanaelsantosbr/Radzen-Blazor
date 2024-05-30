using Microsoft.AspNetCore.Mvc;
using MyRadzenBlazor.Entities;
using MyRadzenBlazor.Services;
using System.Collections.Generic;

namespace MyRadzenBlazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ClientAppService _clientAppService;

        public ClientsController(ClientAppService clientAppService)
        {
            _clientAppService = clientAppService;
        }

        [HttpGet]
        public ActionResult<List<Cliente>> GetClients()
        {
            return Ok(_clientAppService.GetClients());
        }

        [HttpGet("{id}")]
        public ActionResult<Cliente> GetClientById(int id)
        {
            var client = _clientAppService.GetClientById(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public ActionResult CreateClient(Cliente client)
        {
            _clientAppService.CreateClient(client);
            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateClient(int id, Cliente client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            var existingClient = _clientAppService.GetClientById(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            _clientAppService.UpdateClient(client);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteClient(int id)
        {
            var client = _clientAppService.GetClientById(id);
            if (client == null)
            {
                return NotFound();
            }

            _clientAppService.DeleteClient(id);
            return NoContent();
        }
    }
}
