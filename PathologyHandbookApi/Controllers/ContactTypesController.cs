//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathologyHandbookApi.Models;

namespace PathologyHandbookApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/ContactTypes")]
    public class ContactTypesController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public ContactTypesController(PathologyHandbookContext context)
        {
            _context = context;
        }

        // GET: api/ContactTypes
        [HttpGet]
        public IEnumerable<ContactType> GetContactTypes()
        {
            return _context.ContactTypes;
        }

        // GET: api/ContactTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contactType = await _context.ContactTypes.SingleOrDefaultAsync(m => m.Id == id);

            if (contactType == null)
                return NotFound();

            return Ok(contactType);
        }

        // PUT: api/ContactTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactType([FromRoute] int id, [FromBody] ContactType contactType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != contactType.Id)
                return BadRequest();

            if (DuplicateValues(contactType))
                return BadRequest(contactType);

            contactType.ModifiedBy = HttpContext.User.Identity.Name;
            contactType.DateModified = DateTime.Now;

            _context.Entry(contactType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactTypeExists(id))
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

        // POST: api/ContactTypes
        [HttpPost]
        public async Task<IActionResult> PostContactType([FromBody] ContactType contactType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (DuplicateValues(contactType))
                return BadRequest(contactType);

            contactType.CreatedBy = HttpContext.User.Identity.Name;
            contactType.DateCreated = DateTime.Now;

            _context.ContactTypes.Add(contactType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactType", new { id = contactType.Id }, contactType);
        }

        //POST: api/ContactTypes/query
       [HttpPost]
       [Route("/api/ContactTypes/query")]
        public async Task<IActionResult> PostContactTypeQuery([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<ContactType>();

            var query = _context.ContactTypes.AsQueryable();

            query = query.Where(ct => ct.Active == queryObj.Active);
            query = query.Where(ct => ct.Description.Contains(queryObj.SearchTerm));


            var columnsMap = new Dictionary<string, Expression<Func<ContactType, object>>>()
            {
                ["description"] = a => a.Description
            };

            if (string.IsNullOrWhiteSpace(queryObj.SortBy))
                queryObj.SortBy = "description";

            if (queryObj.IsSortAscending)
            {
                query = query.OrderBy(columnsMap[queryObj.SortBy]);
            }
            else
            {
                query = query.OrderByDescending(columnsMap[queryObj.SortBy]);
            }

            queryResult.TotalItems = query.Count();
            queryResult.TotalPages = Math.Ceiling((double)queryResult.TotalItems / queryObj.PageSize);

            if (queryObj.Page <= 0)
                queryObj.Page = 1;

            if (queryObj.PageSize <= 0)
                queryObj.PageSize = 10;

            query = query.Skip((queryObj.Page - 1) * queryObj.PageSize).Take(queryObj.PageSize);

            queryResult.Items = query.ToList();
            queryResult.CurrentPage = queryObj.Page;

            return Ok(queryResult);
        }

        // DELETE: api/ContactTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contactType = await _context.ContactTypes.SingleOrDefaultAsync(m => m.Id == id);

            if (contactType == null)
                return NotFound();

            _context.ContactTypes.Remove(contactType);
            await _context.SaveChangesAsync();

            return Ok(contactType);
        }

        private bool ContactTypeExists(int id)
        {
            return _context.ContactTypes.Any(e => e.Id == id);
        }

        private bool DuplicateValues(ContactType contactType)
        {
            var contactTypeAlreadyExist = _context.ContactTypes.Where(ct => ct.Description == contactType.Description && ct.Id != contactType.Id);

            return contactTypeAlreadyExist.Any();
        }
    }
}