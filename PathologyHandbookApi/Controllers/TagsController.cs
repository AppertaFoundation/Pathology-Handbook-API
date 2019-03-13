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
using PathologyHandbookApi.Models;

namespace PathologyHandbookApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Tags")]
    public class TagsController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public TagsController(PathologyHandbookContext context)
        {
            _context = context;
        }

        // GET: api/Tags
        [HttpGet]
        public IEnumerable<Tag> GetTags()
        {
            return _context.Tags;
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTag([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tag = await _context.Tags.SingleOrDefaultAsync(m => m.Id == id);

            if (tag == null)
                return NotFound();

            return Ok(tag);
        }

        // POST: api/Tags/A
        [AllowAnonymous]
        [HttpPost("/api/Tags/Letter")]
        public async Task<IActionResult> GetTagsByLetter([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<Tag>();

            var departmentId = 0;
            Int32.TryParse(queryObj.FilterBy, out departmentId);

            IQueryable<Tag> query;

            if (!string.IsNullOrWhiteSpace(queryObj.SearchTerm))
            {
                var letter = queryObj.SearchTerm[0];

                if (departmentId > 0)
                {
                    query = FilterTagsByDepartment(departmentId, queryObj);

                    query = query.Where(t => t.Active == queryObj.Active)
                        .Where(t => t.TagType.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.MainTestName))
                        .Where(t => t.Description.ToLower().StartsWith(char.ToLower(letter))).AsQueryable();
                }
                else
                {
                    query = _context.Tags.Include(t => t.TagType)
                        .Where(t => t.Active == queryObj.Active)
                        .Where(t => t.TagType.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.MainTestName))
                        .Where(t => t.Description.ToLower().StartsWith(char.ToLower(letter))).AsQueryable();
                }
            }
            else
            {
                if (departmentId > 0)
                {
                    query = FilterTagsByDepartment(departmentId, queryObj);
                }
                else
                {
                    query = FilterTags(queryObj);
                }
            }

            var columnsMap = new Dictionary<string, Expression<Func<Tag, object>>>()
            {
                ["description"] = t => t.Description,
                ["value"] = t => t.Value

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
        private IQueryable<Tag> FilterTagsByDepartment(int departmentId, QueryObject queryObj)
        {
            var testsInDepartmentIds = _context.Tests.Where(t => t.DepartmentId == departmentId).Select(t => t.Id).ToList();

            var testNameQuery = _context.Tags.Include(t => t.TagType)
                .Where(t => testsInDepartmentIds.Contains(t.TestId))
                .Where(t => t.Active == queryObj.Active)
                .Where(t => t.TagType.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.MainTestName))
                .Where(t => t.Description.Contains(queryObj.SearchTerm) || t.Value.Contains(queryObj.SearchTerm)).ToList();

            var codeQuery = _context.Tags.Include(t => t.TagType)
                .Where(t => testsInDepartmentIds.Contains(t.TestId))
                .Where(t => t.Active == queryObj.Active)
                .Where(t => t.TagType.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.Code))
                .Where(t => t.Description.Contains(queryObj.SearchTerm) || t.Value.Contains(queryObj.SearchTerm)).ToList();

            var synonymQuery = _context.Tags.Include(t => t.TagType)
                .Where(t => testsInDepartmentIds.Contains(t.TestId))
                .Where(t => t.Active == queryObj.Active)
                .Where(t => t.TagType.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.Synonym))
                .Where(t => t.Description.Contains(queryObj.SearchTerm) || t.Value.Contains(queryObj.SearchTerm)).ToList();

            var unionQuery = testNameQuery.Union(codeQuery, new TagTestIdComparer());
            unionQuery = unionQuery.Union(synonymQuery, new TagTestIdComparer());

            return unionQuery.AsQueryable();
        }
        private IQueryable<Tag> FilterTags(QueryObject queryObj)
        {
            var testNameQuery = _context.Tags.Include(t => t.TagType)
                .Where(t => t.Active == queryObj.Active)
                .Where(t => t.TagType.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.MainTestName))
                .Where(t => t.Description.Contains(queryObj.SearchTerm) || t.Value.Contains(queryObj.SearchTerm)).ToList();

            var codeQuery = _context.Tags.Include(t => t.TagType)
                .Where(t => t.Active == queryObj.Active)
                .Where(t => t.TagType.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.Code))
                .Where(t => t.Description.Contains(queryObj.SearchTerm) || t.Value.Contains(queryObj.SearchTerm)).ToList();

            var synonymQuery = _context.Tags.Include(t => t.TagType)
                .Where(t => t.Active == queryObj.Active)
                .Where(t => t.TagType.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.Synonym))
                .Where(t => t.Description.Contains(queryObj.SearchTerm) || t.Value.Contains(queryObj.SearchTerm)).ToList();

            var unionQuery = testNameQuery.Union(codeQuery, new TagTestIdComparer());
            unionQuery = unionQuery.Union(synonymQuery, new TagTestIdComparer());

            return unionQuery.AsQueryable();
        }
        internal class TagTestIdComparer : IEqualityComparer<Tag>
        {
            public bool Equals(Tag x, Tag y)
            {
                if (Equals(x.TestId, y.TestId))
                {
                    return true;
                }
                return false;
            }

            public int GetHashCode(Tag obj)
            {
                return obj.TestId.GetHashCode();
            }
        }

        // POST: api/Tags/Tests/query
        [AllowAnonymous]
        [HttpPost]
        [Route("/api/Tags/Tests/query")]
        public async Task<IActionResult> PostTagTestsQuery([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<Tag>();

            IQueryable<Tag> query;

            var departmentId = 0;
            Int32.TryParse(queryObj.FilterBy, out departmentId);

            if (departmentId > 0)
            {
                query = FilterTagsByDepartment(departmentId, queryObj);
                query = query.Where(t => t.Active == queryObj.Active);
            }
            else
            {
                query = FilterTags(queryObj);
                query = query.Where(t => t.Active == queryObj.Active);
            }

        var columnsMap = new Dictionary<string, Expression<Func<Tag, object>>>()
            {
                ["description"] = t => t.Description,
                ["value"] = t => t.Value

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

        // PUT: api/Tags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag([FromRoute] int id, [FromBody] Tag tag)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != tag.Id)
                return BadRequest();

            tag.DateModified = DateTime.Now;
            tag.ModifiedBy = HttpContext.User.Identity.Name;

            _context.Entry(tag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                tag.TagType =
                    _context.TagTypes.SingleOrDefault(tt => tt.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.Synonym));

                return Ok(tag);
            }
            catch (Exception e)
            {
                if (!TagExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Tags
        [HttpPost]
        public async Task<IActionResult> PostTag([FromBody] Tag tag)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tagTypeId = _context.TagTypes.FirstOrDefault(tt => tt.Code == tag.TagType.Code).Id;

            tag.TagTypeId = tagTypeId;
            tag.TagType = null;
            tag.CreatedBy = HttpContext.User.Identity.Name;
            tag.DateCreated = DateTime.Now;

            try
            {
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return CreatedAtAction("GetTag", new { id = tag.Id }, tag);
        }

        [HttpPost]
        [Route("ParentTestId/{parentTestId:int}/ConcurrentTestId/{concurrentTestId:int}")]
        public async Task<IActionResult> PostConcurrentTestTag(int parentTestId, int concurrentTestId)
        {
            if (parentTestId <= 0 || concurrentTestId <= 0)
                return BadRequest();

            if (parentTestId == concurrentTestId)
                return BadRequest();

            var concurrentTestAlreadyAssociatedWithTest = _context.Tags
                .Where(tag => tag.TestId == parentTestId && tag.TagTypeId == (int)TagTypeEnums.ConcurrentTest).AsQueryable();

            var alreadyAssigned =
                concurrentTestAlreadyAssociatedWithTest.Any(t => t.Value == concurrentTestId.ToString());

            if (alreadyAssigned)
                return BadRequest();

            var newTag = new Tag()
            {
                Active = true,
                TestId = parentTestId,
                CreatedBy = HttpContext.User.Identity.Name,
                DateCreated = DateTime.Now,
                TagTypeId = (int)TagTypeEnums.ConcurrentTest,
                Description = _context.Tests.SingleOrDefault(t => t.Id == concurrentTestId).NameDescription,
                Value = concurrentTestId.ToString()
            };

            try
            {
                _context.Tags.Add(newTag);
                await _context.SaveChangesAsync();
                newTag.TagType =
                    _context.TagTypes.SingleOrDefault(tt => tt.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.ConcurrentTest));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return CreatedAtAction("GetTag", new { id = newTag.Id }, newTag);
        }

        [HttpPost]
        [Route("Synonym")]
        public async Task<IActionResult> PostSynonymTestTag([FromBody] Tag tag)
        {
            if (Equals(tag, null))
                return BadRequest();

            if (tag.TestId <= 0)
                return BadRequest();

            tag.CreatedBy = HttpContext.User.Identity.Name;
            tag.DateCreated = DateTime.Now;
            tag.TagTypeId = _context.TagTypes.SingleOrDefault(tt =>
                tt.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.Synonym)).Id;

            try
            {
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
                tag.TagType =
                    _context.TagTypes.SingleOrDefault(tt => tt.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.Synonym));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return CreatedAtAction("GetTag", new { id = tag.Id }, tag);
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tag = await _context.Tags.SingleOrDefaultAsync(m => m.Id == id);

            if (tag == null)
                return NotFound();

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return Ok(tag);
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }
    }
}