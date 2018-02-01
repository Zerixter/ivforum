using IVForum.API.Data;
using IVForum.API.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.Controllers
{
	[Authorize]
    [Route("api/Forums")]
	[Produces("application/json")]
    public class ForumsController : Controller
    {
        private readonly DbHandler _context;

        public ForumsController(DbHandler context)
        {
            _context = context;
        }

        // GET: api/Forums
        [HttpGet]
        public IEnumerable<Forum> GetForums()
        {
            return _context.Forums;
        }

        // GET: api/Forums/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForum([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.Id == id);

            if (forum == null)
            {
                return NotFound();
            }

            return Ok(forum);
        }

        // PUT: api/Forums/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForum([FromRoute] Guid id, [FromBody] Forum forum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != forum.Id)
            {
                return BadRequest();
            }

            _context.Entry(forum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Forums
        [HttpPost]
        public async Task<IActionResult> PostForum([FromBody] Forum forum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Forums.Add(forum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetForum", new { id = forum.Id }, forum);
        }

        // DELETE: api/Forums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForum([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.Id == id);
            if (forum == null)
            {
                return NotFound();
            }

            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();

            return Ok(forum);
        }

        private bool ForumExists(Guid id)
        {
            return _context.Forums.Any(e => e.Id == id);
        }
    }
}