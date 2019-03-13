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
    [Route("api/RefRanges")]
    public class RefRangesController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public RefRangesController(PathologyHandbookContext context)
        {
            _context = context;
        }

        // GET: api/RefRanges
        [HttpGet]
        public IEnumerable<RefRange> GetRefRanges()
        {
            return _context.RefRanges;
        }

        // GET: api/RefRanges/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRefRange([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var refRange = await _context.RefRanges.SingleOrDefaultAsync(m => m.Id == id);

            if (refRange == null)
            {
                return NotFound();
            }

            return Ok(refRange);
        }

        // POST: api/RefRanges/query
        [HttpPost]
        [Route("/api/RefRanges/query")]
        public async Task<IActionResult> PostRefRangeQuery([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<RefRange>();

            IQueryable<RefRange> query;

            query = _context.RefRanges
                .Where(rr => rr.Active == queryObj.Active);

            var columnsMap = new Dictionary<string, Expression<Func<RefRange, object>>>()
            {
                ["testId"] = rr => rr.TestId,

            };

            if (string.IsNullOrWhiteSpace(queryObj.SortBy))
                queryObj.SortBy = "testId";

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

        // PUT: api/RefRanges/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRefRange([FromRoute] int id, [FromBody] RefRange refRange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != refRange.Id)
            {
                return BadRequest();
            }

            refRange.DateModified = DateTime.Now;
            refRange.ModifiedBy = HttpContext.User.Identity.Name;

            _context.Entry(refRange).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RefRangeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(refRange);
        }

        // POST: api/RefRanges
        [HttpPost]
        public async Task<IActionResult> PostRefRange([FromBody] RefRange refRange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            refRange.DateCreated = DateTime.Now;
            refRange.ModifiedBy = HttpContext.User.Identity.Name;
            refRange.Active = true;

            _context.RefRanges.Add(refRange);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRefRange", new { id = refRange.Id }, refRange);
        }

        // DELETE: api/RefRanges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRefRange([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var refRange = await _context.RefRanges.SingleOrDefaultAsync(m => m.Id == id);
            if (refRange == null)
            {
                return NotFound();
            }

            _context.RefRanges.Remove(refRange);
            await _context.SaveChangesAsync();

            return Ok(refRange);
        }

        private bool RefRangeExists(int id)
        {
            return _context.RefRanges.Any(e => e.Id == id);
        }
    }
}