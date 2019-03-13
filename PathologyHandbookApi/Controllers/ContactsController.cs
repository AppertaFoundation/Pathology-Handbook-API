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
using PathologyHandbookApi.Mappings;
using PathologyHandbookApi.Models;
using PathologyHandbookApi.ViewModels;

namespace PathologyHandbookApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public ContactsController(PathologyHandbookContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public IEnumerable<Contact> GetContacts()
        {
            return _context.Contacts.Include(c => c.Department);
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contact = await _context.Contacts
                .Include(c => c.ContactDetails)
                .Include(c => c.Department).SingleOrDefaultAsync(m => m.Id == id);

            if (contact == null)
                return NotFound();

            return Ok(contact);
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact([FromRoute] int id, [FromBody] Contact contact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != contact.Id)
                return BadRequest();

            if (DuplicateValues(contact))
                return BadRequest(contact);

            contact.DateModified = DateTime.Now;
            contact.ModifiedBy = HttpContext.User.Identity.Name;

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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

        // POST: api/Contacts
        [HttpPost]
        public async Task<IActionResult> PostContact([FromBody] Contact contact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (DuplicateValues(contact))
                return BadRequest();

            contact.CreatedBy = HttpContext.User.Identity.Name;
            contact.DateCreated = DateTime.Now;

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
        }

        // POST: api/Contacts/query
        [HttpPost]
        [Route("/api/Contacts/query")]
        public async Task<IActionResult> PostContactQuery([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<ContactViewModel>();

            var query = _context.Contacts.AsQueryable();

            query = query.Where(c => c.Active == queryObj.Active);
            query = query.Where(c => c.Name.Contains(queryObj.SearchTerm));


            var columnsMap = new Dictionary<string, Expression<Func<Contact, object>>>()
            {
                ["name"] = c => c.Name
            };

            if (string.IsNullOrWhiteSpace(queryObj.SortBy))
                queryObj.SortBy = "name";

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
            query = query.Include(c => c.Department).Include(c => c.ContactDetails).ThenInclude(cd => cd.ContactType);

            query.ToList().ForEach(c => { queryResult.Items.Add( c.ContactToContactViewModel()); });

            queryResult.CurrentPage = queryObj.Page;

            return Ok(queryResult);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contact = await _context.Contacts.SingleOrDefaultAsync(m => m.Id == id);

            if (contact == null)
                return NotFound();

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return Ok(contact);
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }

        private bool DuplicateValues(Contact contact)
        {
            var contactDetailsValues = contact.ContactDetails.Select(cd => cd.ContactValue).ToList();

            var contactAlreadyExists = _context.Contacts.Where(c => c.Name == contact.Name && c.Id != contact.Id);
            var contactDetailsValueAlreadyExists =
                _context.ContactDetails.Where(cd => contactDetailsValues.Any(cdv => cdv.Equals(cd.ContactValue)));

            return (contactAlreadyExists.Any() || contactDetailsValueAlreadyExists.Any());
        }

    }
}