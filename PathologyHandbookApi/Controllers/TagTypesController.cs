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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathologyHandbookApi.Mappings;
using PathologyHandbookApi.Models;
using Tag = Swashbuckle.AspNetCore.Swagger.Tag;

namespace PathologyHandbookApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/TagTypes")]
    public class TagTypesController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public TagTypesController(PathologyHandbookContext context)
        {
            _context = context;
        }

        // GET: api/TagTypes
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<TagType> GetTagTypes()
        {
            return _context.TagTypes;
        }

        // GET: api/TagTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tagType = await _context.TagTypes.Include(tt => tt.Tags).SingleOrDefaultAsync(m => m.Id == id);

            if (tagType == null)
                return NotFound();

            return Ok(tagType.ToTagTypeViewModel());
        }

        // PUT: api/TagTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTagType([FromRoute] int id, [FromBody] TagType tagType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != tagType.Id)
                return BadRequest();

            if (DuplcateValues(tagType))
                return BadRequest(tagType);

            tagType.ModifiedBy = HttpContext.User.Identity.Name;
            tagType.DateModified = DateTime.Now;

            _context.Entry(tagType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagTypeExists(id))
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

        // POST: api/TagTypes
        [HttpPost]
        public async Task<IActionResult> PostTagType([FromBody] TagType tagType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (DuplcateValues(tagType))
                return BadRequest(tagType);

            tagType.CreatedBy = HttpContext.User.Identity.Name;
            tagType.DateCreated = DateTime.Now;

            _context.TagTypes.Add(tagType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTagType", new { id = tagType.Id }, tagType);
        }

        // POST: api/TagTypes
        [HttpPost]
        [Route("/api/TagTypes/query")]
        public async Task<IActionResult> GetTagTypeQuery([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<TagType>();

            var query = _context.TagTypes.AsQueryable();

            query = query.Where(tt => tt.Active == queryObj.Active);
            query = query.Where(tt => tt.Description.Contains(queryObj.SearchTerm) || tt.Code.Contains(queryObj.SearchTerm));
            

            var columnsMap = new Dictionary<string, Expression<Func<TagType, object>>>()
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

        // DELETE: api/TagTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTagType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tagType = await _context.TagTypes.SingleOrDefaultAsync(m => m.Id == id);

            if (tagType == null)
                return NotFound();

            _context.TagTypes.Remove(tagType);
            await _context.SaveChangesAsync();

            return Ok(tagType);
        }

        private bool TagTypeExists(int id)
        {
            return _context.TagTypes.Any(e => e.Id == id);
        }

        private bool DuplcateValues(TagType tagType)
        {
            var tagTypeAlreadyExists =
                _context.TagTypes
                    .Where(tt => tt.Id != tagType.Id)
                    .Where(tt => tt.Description == tagType.Description || tt.Code == tagType.Code);

            return tagTypeAlreadyExists.Any();
        }
    }
}