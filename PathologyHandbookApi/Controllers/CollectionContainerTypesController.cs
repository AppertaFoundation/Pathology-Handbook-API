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
    [Route("api/CollectionContainerTypes")]
    public class CollectionContainerTypesController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public CollectionContainerTypesController(PathologyHandbookContext context)
        {
            _context = context;
        }

        // GET: api/CollectionContainerTypes
        [HttpGet]
        public IEnumerable<CollectionContainerType> GetCollectionContainerTypes()
        {
            return _context.CollectionContainerTypes.OrderByDescending(cct => cct.Description);
        }

        // GET: api/CollectionContainerTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCollectionContainerType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var collectionContainerType = await _context.CollectionContainerTypes.Include(cct => cct.Images).SingleOrDefaultAsync(cct => cct.Id == id);

            if (collectionContainerType == null)
                return NotFound();

            return Ok(collectionContainerType);
        }

        // PUT: api/CollectionContainerTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollectionContainerType([FromRoute] int id, [FromBody] CollectionContainerType collectionContainerType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != collectionContainerType.Id)
                return BadRequest();

            if (DuplicateValues(collectionContainerType))
                return BadRequest(collectionContainerType);

            collectionContainerType.ModifiedBy = HttpContext.User.Identity.Name;
            collectionContainerType.DateModified = DateTime.Now;

            _context.Entry(collectionContainerType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollectionContainerTypeExists(id))
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

        // POST: api/CollectionContainerTypes
        [HttpPost]
        public async Task<IActionResult> PostCollectionContainerType([FromBody] CollectionContainerType collectionContainerType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (DuplicateValues(collectionContainerType))
                return BadRequest(collectionContainerType);

            collectionContainerType.CreatedBy = HttpContext.User.Identity.Name;
            collectionContainerType.DateCreated = DateTime.Now;

            _context.CollectionContainerTypes.Add(collectionContainerType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCollectionContainerType", new { id = collectionContainerType.Id }, collectionContainerType);
        }

        // POST: api/ContactTypes/query
        [HttpPost]
        [Route("/api/CollectionContainerTypes/query")]
        public async Task<IActionResult> PostCollectionContainerTypeQuery([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<CollectionContainerType>();

            var query = _context.CollectionContainerTypes.AsQueryable();

            query = query.Where(ct => ct.Active == queryObj.Active);
            query = query.Where(ct => ct.Description.Contains(queryObj.SearchTerm));
            query = query.Where(ct => ct.Name.Contains(queryObj.SearchTerm));


            var columnsMap = new Dictionary<string, Expression<Func<CollectionContainerType, object>>>()
            {
                ["description"] = cct => cct.Description,
                ["name"] = cct => cct.Name,
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

            queryResult.Items = query.ToList();
            queryResult.CurrentPage = queryObj.Page;

            return Ok(queryResult);
        }

        // DELETE: api/CollectionContainerTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollectionContainerType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var collectionContainerType = await _context.CollectionContainerTypes.SingleOrDefaultAsync(m => m.Id == id);

            if (collectionContainerType == null)
                return NotFound();

            _context.CollectionContainerTypes.Remove(collectionContainerType);
            await _context.SaveChangesAsync();

            return Ok(collectionContainerType);
        }

        private bool CollectionContainerTypeExists(int id)
        {
            return _context.CollectionContainerTypes.Any(e => e.Id == id);
        }

        private bool DuplicateValues(CollectionContainerType collectionContainerType)
        {
            var alreadyExists = _context.CollectionContainerTypes.Where(ct => ct.Name == collectionContainerType.Name && ct.Id != collectionContainerType.Id);

            return alreadyExists.Any();
        }
    }
}