using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProyectoController : Controller
    {
        private readonly Context _context;

        public ProyectoController(Context context)
        {
            _context = context;

            Proyecto proyecto = new Proyecto
            {
                Name = "Johnyy",
                Email = "johnyy@gmail.com"
            };

            if (_context.Proyectos.Count() == 0)
            {
                _context.Proyectos.Add(proyecto);
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Proyecto> GetAll()
        {
            return _context.Proyectos.ToList();
        }

        [HttpGet("{id}", Name = "GetProyecto")]
        public IActionResult GetById(int id)
        {
            var item = _context.Proyectos.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
    }
}