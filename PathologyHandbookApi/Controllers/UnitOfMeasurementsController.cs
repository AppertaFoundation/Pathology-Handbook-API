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
    [Route("api/UnitOfMeasurements")]
    public class UnitOfMeasurementsController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public UnitOfMeasurementsController(PathologyHandbookContext context)
        {
            _context = context;
        }

        // GET: api/UnitOfMeasurements
        [HttpGet]
        public IEnumerable<UnitOfMeasurement> GetUnitOfMeasurements()
        {
            return _context.UnitOfMeasurements.OrderByDescending(u => u.Description);
        }

        // GET: api/UnitOfMeasurements/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitOfMeasurement([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unitOfMeasurement = await _context.UnitOfMeasurements.SingleOrDefaultAsync(m => m.Id == id);

            if (unitOfMeasurement == null)
            {
                return NotFound();
            }

            return Ok(unitOfMeasurement);
        }

        // PUT: api/UnitOfMeasurements/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnitOfMeasurement([FromRoute] int id, [FromBody] UnitOfMeasurement unitOfMeasurement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != unitOfMeasurement.Id)
            {
                return BadRequest();
            }

            _context.Entry(unitOfMeasurement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnitOfMeasurementExists(id))
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
        // POST: api/UnitOfMeasurements
        [HttpPost]
        [Route("/api/UnitOfMeasurements/query")]
        public async Task<IActionResult> GetUnitOfMeasurementsQuery([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<UnitOfMeasurement>();

            var query = _context.UnitOfMeasurements.AsQueryable();

            query = query.Where(tt => tt.Active == queryObj.Active);
            query = query.Where(tt => tt.Description.Contains(queryObj.SearchTerm) || tt.Code.Contains(queryObj.SearchTerm));


            var columnsMap = new Dictionary<string, Expression<Func<UnitOfMeasurement, object>>>()
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

        // POST: api/UnitOfMeasurements
        [HttpPost]
        public async Task<IActionResult> PostUnitOfMeasurement([FromBody] UnitOfMeasurement unitOfMeasurement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UnitOfMeasurements.Add(unitOfMeasurement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUnitOfMeasurement", new { id = unitOfMeasurement.Id }, unitOfMeasurement);
        }

        // DELETE: api/UnitOfMeasurements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnitOfMeasurement([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unitOfMeasurement = await _context.UnitOfMeasurements.SingleOrDefaultAsync(m => m.Id == id);
            if (unitOfMeasurement == null)
            {
                return NotFound();
            }

            _context.UnitOfMeasurements.Remove(unitOfMeasurement);
            await _context.SaveChangesAsync();

            return Ok(unitOfMeasurement);
        }

        private bool UnitOfMeasurementExists(int id)
        {
            return _context.UnitOfMeasurements.Any(e => e.Id == id);
        }
    }
}