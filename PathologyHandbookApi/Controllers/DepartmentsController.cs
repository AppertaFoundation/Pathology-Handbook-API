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
    [Route("api/Departments")]
    public class DepartmentsController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public DepartmentsController(PathologyHandbookContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Department> GetDepartments()
        {
            return _context.Departments.OrderBy(d => d.Name);
        }

        // GET: api/Departments/Active
        [AllowAnonymous]
        [HttpGet]
        [Route("/api/Departments/Active")]
        public IEnumerable<Department> GetDepartmentsActiveOnly()
        {
            return _context.Departments.Where(d => d.Active).OrderBy(d => d.Name);
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var department = await _context.Departments.SingleOrDefaultAsync(m => m.Id == id);

            if (department == null)
                return NotFound();

            return Ok(department);
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment([FromRoute] int id, [FromBody] Department department)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != department.Id)
                return BadRequest();

            if (DuplicateValues(department))
                return BadRequest(department);

            department.DateModified = DateTime.Now;
            department.ModifiedBy = HttpContext.User.Identity.Name;

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/Departments
        [HttpPost]
        public async Task<IActionResult> PostDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (DuplicateValues(department))
                return BadRequest(department);

            department.CreatedBy = HttpContext.User.Identity.Name;
            department.DateCreated = DateTime.Now;

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
        }

        // POST: api/Departments/query
        [HttpPost]
        [Route("/api/Departments/query")]
        public async Task<IActionResult> PostDepartmentQuery([FromBody] QueryObject queryObj)
        {
            var queryResult = new QueryResults<Department>();

            var query = _context.Departments.AsQueryable();

            query = query.Where(ct => ct.Active == queryObj.Active);
            query = query.Where(ct => ct.Name.Contains(queryObj.SearchTerm));


            var columnsMap = new Dictionary<string, Expression<Func<Department, object>>>()
            {
                ["name"] = d => d.Name
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

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var department = await _context.Departments.SingleOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }

        private bool DuplicateValues(Department department)
        {
            var alreadyExists = _context.Departments.Where(d => d.Name == department.Name && d.Id != department.Id);

            return alreadyExists.Any();
        }
    }
}