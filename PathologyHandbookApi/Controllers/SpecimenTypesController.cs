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
    [Route("api/SpecimenTypes")]
    public class SpecimenTypesController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public SpecimenTypesController(PathologyHandbookContext context)
        {
            _context = context;
        }

        // GET: api/SpecimenTypes
        [HttpGet]
        public IEnumerable<SpecimenType> GetSpecimenTypes()
        {
            return _context.SpecimenTypes.OrderByDescending(st => st.Description);
        }

        // GET: api/SpecimenTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecimenType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var specimenType = await _context.SpecimenTypes.SingleOrDefaultAsync(m => m.Id == id);

            if (specimenType == null)
                return NotFound();

            return Ok(specimenType);
        }

        // PUT: api/SpecimenTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecimenType([FromRoute] int id, [FromBody] SpecimenType specimenType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != specimenType.Id)
                return BadRequest();

            if (DuplicateValues(specimenType))
                return BadRequest(specimenType);

            specimenType.DateModified = DateTime.Now;
            specimenType.ModifiedBy = HttpContext.User.Identity.Name;

            _context.Entry(specimenType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecimenTypeExists(id))
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

        // POST: api/SpecimenTypes
        [HttpPost]
        public async Task<IActionResult> PostSpecimenType([FromBody] SpecimenType specimenType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (DuplicateValues(specimenType))
                return BadRequest(specimenType);

            specimenType.DateCreated = DateTime.Now;
            specimenType.CreatedBy = HttpContext.User.Identity.Name;

            _context.SpecimenTypes.Add(specimenType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpecimenType", new { id = specimenType.Id }, specimenType);
        }

        // POST: api/SpecimenTypes
        [HttpPost]
        [Route("/api/SpecimenTypes/query")]
        public async Task<IActionResult> GetSpecimenTypeQuery([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<SpecimenType>();

            var query = _context.SpecimenTypes.AsQueryable();

            query = query.Where(tt => tt.Active == queryObj.Active);
            query = query.Where(tt => tt.Description.Contains(queryObj.SearchTerm) || tt.Code.Contains(queryObj.SearchTerm));


            var columnsMap = new Dictionary<string, Expression<Func<SpecimenType, object>>>()
            {
                ["description"] = a => a.Description,
                ["code"] = a => a.Code,
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


        // DELETE: api/SpecimenTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecimenType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var specimenType = await _context.SpecimenTypes.SingleOrDefaultAsync(m => m.Id == id);

            if (specimenType == null)
                return NotFound();

            _context.SpecimenTypes.Remove(specimenType);
            await _context.SaveChangesAsync();

            return Ok(specimenType);
        }

        private bool SpecimenTypeExists(int id)
        {
            return _context.SpecimenTypes.Any(e => e.Id == id);
        }

        private bool DuplicateValues(SpecimenType specimenType)
        {
            var specimenTypeAlreadyExists =
                _context.SpecimenTypes.Where(st => (st.Description == specimenType.Description || st.Code == specimenType.Code) && st.Id != specimenType.Id);

            return specimenTypeAlreadyExists.Any();
        }
    }
}