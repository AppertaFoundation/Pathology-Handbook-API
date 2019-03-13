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
    [Route("api/ContactDetails")]
    public class ContactDetailsController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public ContactDetailsController(PathologyHandbookContext context)
        {
            _context = context;
        }

        // GET: api/ContactDetails
        [HttpGet]
        public IEnumerable<ContactDetail> GetContactDetails()
        {
            return _context.ContactDetails;
        }

        // GET: api/ContactDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactDetail([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contactDetail = await _context.ContactDetails.SingleOrDefaultAsync(m => m.Id == id);

            if (contactDetail == null)
                return NotFound();

            return Ok(contactDetail);
        }

        // PUT: api/ContactDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactDetail([FromRoute] int id, [FromBody] ContactDetail contactDetail)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != contactDetail.Id)
                return BadRequest();

            if (DuplicateValues(contactDetail))
                return BadRequest(contactDetail);

            contactDetail.DateModified = DateTime.Now;
            contactDetail.ModifiedBy = HttpContext.User.Identity.Name;

            _context.Entry(contactDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactDetailExists(id))
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

        // POST: api/ContactDetails
        [HttpPost]
        public async Task<IActionResult> PostContactDetail([FromBody] ContactDetail contactDetail)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (DuplicateValues(contactDetail))
                return BadRequest(contactDetail);

            contactDetail.CreatedBy = HttpContext.User.Identity.Name;
            contactDetail.DateCreated = DateTime.Now;

            _context.ContactDetails.Add(contactDetail);
            await _context.SaveChangesAsync();

            var contactType = _context.ContactTypes.Find(contactDetail.ContactTypeId);
            contactDetail.ContactType = contactType;

            return CreatedAtAction("GetContactDetail", new { id = contactDetail.Id }, contactDetail);
        }

        // POST: api/ContactDetails/query
        [HttpPost]
        [Route("/api/ContactDetails/query")]
        public async Task<IActionResult> PostContactDetailsQuery([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<ContactDetail>();

            var query = _context.ContactDetails.AsQueryable();

            //query = query.Where(cd => cd.Active == queryObj.Active);
            query = query.Where(cd => cd.ContactValue.Contains(queryObj.SearchTerm));


            var columnsMap = new Dictionary<string, Expression<Func<ContactDetail, object>>>()
            {
                ["contactValue"] = a => a.ContactValue
            };

            if (string.IsNullOrWhiteSpace(queryObj.SortBy))
                queryObj.SortBy = "contactValue";

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
        // DELETE: api/ContactDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactDetail([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contactDetail = await _context.ContactDetails.SingleOrDefaultAsync(m => m.Id == id);

            if (contactDetail == null)
                return NotFound();

            _context.ContactDetails.Remove(contactDetail);
            await _context.SaveChangesAsync();

            return Ok(contactDetail);
        }

        private bool ContactDetailExists(int id)
        {
            return _context.ContactDetails.Any(e => e.Id == id);
        }

        private bool DuplicateValues(ContactDetail contactDetail)
        {
            var alreadyExists = _context.ContactDetails.Where(cd => cd.ContactValue == contactDetail.ContactValue && cd.Id != contactDetail.Id);

            return alreadyExists.Any();
        }
    }
}