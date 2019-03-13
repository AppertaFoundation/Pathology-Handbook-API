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
using PathologyHandbookApi.Migrations;
using PathologyHandbookApi.Models;

namespace PathologyHandbookApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Tests")]
    public class TestsController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public TestsController(PathologyHandbookContext context)
        {
            _context = context;
        }

        private void UpdateAllTests()
        {
            var allTests = _context.Tests.Include(t => t.Department);

            foreach (var test in allTests)
            {
                var tag = new Tag()
                {
                    TestId = test.Id,
                    Value = test.DepartmentId.ToString(),
                    Description = test.NameDescription,
                    TagTypeId = _context.TagTypes.FirstOrDefault(tt => tt.Code == TagTypeEnums.Department.ToString()).Id,
                    Active = true,
                    CreatedBy = HttpContext.User.Identity.Name,
                    DateCreated = DateTime.Now
                };

                _context.Tags.Add(tag);

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        // GET: api/Tests
        [HttpGet]
        public IEnumerable<Test> GetTests()
        {
            UpdateAllTests();
            return _context.Tests;
        }

        // GET: api/Tests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var test = await _context.Tests
                .Include(t => t.Department)
                    .ThenInclude(d => d.Contacts)
                        .ThenInclude(c => c.ContactDetails)
                            .ThenInclude(cd => cd.ContactType)
                .Include(t => t.Containers)
                    .ThenInclude(c => c.SpecimenType)
                .Include(t => t.Containers)
                    .ThenInclude(c => c.CollectionContainerType)
                        .ThenInclude(ct => ct.Images)
                .Include(t => t.Tags)
                    .ThenInclude(tt => tt.TagType)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (test == null)
                return NotFound();

            test.RefRanges = SortRefRanges(id);

            return Ok(test);
        }

        private List<RefRange> SortRefRanges(int testId)
        {
            var refRanges = _context.RefRanges.Where(rr => rr.TestId == testId).ToList();

            if (refRanges.Any())
            {
                var maleDayRefRanges = refRanges.Where(rr => rr.Gender == "M" && rr.DayMonthYear == "D").OrderBy(a => a.Age);
                var femaleDayRefRanges = refRanges.Where(rr => rr.Gender == "F" && rr.DayMonthYear == "D").OrderBy(a => a.Age);
                var nsDayRefRanges = refRanges.Where(rr => rr.Gender == "N/S" && rr.DayMonthYear == "D").OrderBy(a => a.Age);

                var maleWeekRefRanges = refRanges.Where(rr => rr.Gender == "M" && rr.DayMonthYear == "W").OrderBy(a => a.Age);
                var femaleWeekRefRanges = refRanges.Where(rr => rr.Gender == "F" && rr.DayMonthYear == "W").OrderBy(a => a.Age);
                var nsWeekRefRanges = refRanges.Where(rr => rr.Gender == "N/S" && rr.DayMonthYear == "W").OrderBy(a => a.Age);

                var maleMonthRefRanges = refRanges.Where(rr => rr.Gender == "M" && rr.DayMonthYear == "M").OrderBy(a => a.Age);
                var femaleMonthRefRanges = refRanges.Where(rr => rr.Gender == "F" && rr.DayMonthYear == "M").OrderBy(a => a.Age);
                var nsMonthRefRanges = refRanges.Where(rr => rr.Gender == "N/S" && rr.DayMonthYear == "M").OrderBy(a => a.Age);

                var maleYearRefRanges = refRanges.Where(rr => rr.Gender == "M" && rr.DayMonthYear == "Y").OrderBy(a => a.Age);
                var femaleYearRefRanges = refRanges.Where(rr => rr.Gender == "F" && rr.DayMonthYear == "Y").OrderBy(a => a.Age);
                var nsYearRefRanges = refRanges.Where(rr => rr.Gender == "N/S" && rr.DayMonthYear == "Y").OrderBy(a => a.Age);

                var sortedRefRanges = new List<RefRange>();

                sortedRefRanges.AddRange(femaleDayRefRanges);
                sortedRefRanges.AddRange(femaleWeekRefRanges);
                sortedRefRanges.AddRange(femaleMonthRefRanges);
                sortedRefRanges.AddRange(femaleYearRefRanges);
                sortedRefRanges.AddRange(maleDayRefRanges);
                sortedRefRanges.AddRange(maleWeekRefRanges);
                sortedRefRanges.AddRange(maleMonthRefRanges);
                sortedRefRanges.AddRange(maleYearRefRanges);
                sortedRefRanges.AddRange(nsDayRefRanges);
                sortedRefRanges.AddRange(nsWeekRefRanges);
                sortedRefRanges.AddRange(nsMonthRefRanges);
                sortedRefRanges.AddRange(nsYearRefRanges);

                return sortedRefRanges;

            }

            return new List<RefRange>();
        }

        // GET: api/Tests/Active/5
        [AllowAnonymous]
        [HttpGet("/api/Tests/Active/{id}")]
        public async Task<IActionResult> GetActiveTest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activeTest = _context.Tests.Where(t => t.Active).SingleOrDefault(t => t.Id == id);

            if (activeTest == null)
                return NotFound();
            // Department
            var department = _context.Departments.Where(d => d.Active).SingleOrDefault(d => d.Id == activeTest.DepartmentId);

            if (!Equals(department, null))
                activeTest.Department = department;

            var contacts = _context.Contacts.Where(c => c.Active && c.DepartmentId == activeTest.DepartmentId).ToList();
            // Contacts
            if (contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    var contactDetails = _context.ContactDetails.Where(cd => cd.Active && cd.ContactId == contact.Id)
                        .Include(ct => ct.ContactType)
                        .ToList();
                    contact.ContactDetails = contactDetails;
                }

                activeTest.Department.Contacts = contacts;
            }
            // Container Types
            var containerTypes = _context.ContainerDetails.Where(cds => cds.Active && cds.TestId == activeTest.Id)
                .Include(cds => cds.SpecimenType)
                .Include(cds => cds.CollectionContainerType)
                    .ThenInclude(ct => ct.Images)
                .ToList();

            activeTest.Containers = containerTypes;
            // Tags
            var tags = _context.Tags.Where(t => t.Active && t.TestId == activeTest.Id).Include(t => t.TagType).ToList();

            activeTest.Tags = tags;
            // RefRanges
            activeTest.RefRanges = SortRefRanges(id);

            return Ok(activeTest);
        }

        private void UpdateTestTags(Test test)
        {
            var mainCodeTag = _context.Tags.Include(t => t.TagType)
                .FirstOrDefault(t => t.TestId == test.Id && t.TagType.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.Code));

            if (mainCodeTag.Value != test.MainCode)
                mainCodeTag.Value = test.MainCode;

            var descriptionTag = _context.Tags.Include(t => t.TagType)
                .FirstOrDefault(t => t.TestId == test.Id && t.TagType.Code == Enum.GetName(typeof(TagTypeEnums), TagTypeEnums.MainTestName));

            var nameChanged = descriptionTag.Value != test.NameDescription;

            if (nameChanged)
                descriptionTag.Value = test.NameDescription;

            var tags = _context.Tags.Where(t => t.TestId == test.Id);

            if (test.Active)
            {
                foreach (var tag in tags)
                {
                    tag.Active = true;
                    if (nameChanged)
                        tag.Description = test.NameDescription;
                }
            }
            else
            {
                foreach (var tag in tags)
                {
                    tag.Active = false;
                    if (nameChanged)
                        tag.Description = test.NameDescription;
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // PUT: api/Tests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTest([FromRoute] int id, [FromBody] Test test)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != test.Id)
                return BadRequest();

            test.DateModified = DateTime.Now;
            test.ModifiedBy = HttpContext.User.Identity.Name;

            _context.Entry(test).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            UpdateTestTags(test);

            test = await _context.Tests
                .Include(t => t.Department)
                    .ThenInclude(d => d.Contacts)
                        .ThenInclude(c => c.ContactDetails)
                            .ThenInclude(cd => cd.ContactType)
                .Include(t => t.Containers)
                    .ThenInclude(c => c.SpecimenType)
                .Include(t => t.Containers)
                    .ThenInclude(c => c.CollectionContainerType)
                .Include(t => t.Tags)
                    .ThenInclude(tt => tt.TagType)
                .Include(t => t.RefRanges)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (test == null)
                return NotFound();

            return Ok(test);
        }

        // POST: api/Tests
        [HttpPost]
        public async Task<IActionResult> PostTest([FromBody] Test test)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (DuplicateValues(test))
                return BadRequest(test);

            test.DateCreated = DateTime.Now;
            test.CreatedBy = HttpContext.User.Identity.Name;

            _context.Tests.Add(test);
            _context.SaveChanges();

            var dept = _context.Departments.SingleOrDefault(d => d.Id == test.DepartmentId);

            var createMainCodeTag = new Tag()
            {
                TestId = test.Id,
                Value = test.MainCode,
                Description = test.NameDescription,
                TagTypeId = _context.TagTypes.FirstOrDefault(tt => tt.Code == TagTypeEnums.Code.ToString()).Id,
                Active = test.Active,
                CreatedBy = HttpContext.User.Identity.Name,
                DateCreated = DateTime.Now
            };

            var createTestMainNameTag = new Tag()
            {
                TestId = test.Id,
                Value = test.NameDescription,
                Description = test.NameDescription,
                TagTypeId = _context.TagTypes.FirstOrDefault(tt => tt.Code == TagTypeEnums.MainTestName.ToString()).Id,
                Active = test.Active,
                CreatedBy = HttpContext.User.Identity.Name,
                DateCreated = DateTime.Now
            };

            _context.Tags.AddRange(createMainCodeTag, createTestMainNameTag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTest", new { id = test.Id }, test);
        }

        // POST: api/Tests/query
        [HttpPost]
        [Route("/api/Tests/query")]
        public async Task<IActionResult> PostTestQuery([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<Test>();


            var tagIds = _context.Tags.Where(t => t.Description.Contains(queryObj.SearchTerm) && t.TestId > 0).Select(t => t.TestId);

            var query = _context.Tests.Include(d => d.Department).AsQueryable();

            query = query.Where(t => tagIds.Contains(t.Id));

            query = query.Where(t => t.Active == queryObj.Active);
            //query = query.Where(t => t.NameDescription.Contains(queryObj.SearchTerm));

            var columnsMap = new Dictionary<string, Expression<Func<Test, object>>>()
            {
                ["name"] = d => d.NameDescription
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


        // DELETE: api/Tests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var test = await _context.Tests.SingleOrDefaultAsync(m => m.Id == id);

            if (test == null)
                return NotFound();

            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();

            return Ok(test);
        }

        private bool TestExists(int id)
        {
            return _context.Tests.Any(e => e.Id == id);
        }

        private bool DuplicateValues(Test test)
        {
            var alreadyExists = _context.Tests.Where(t => (t.NameDescription == test.NameDescription) && t.Id != test.Id);

            return alreadyExists.Any();
        }

    }
}