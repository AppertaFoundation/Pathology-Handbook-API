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
using System.Text;
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
    [Route("api/ContainerDetails")]
    public class ContainerDetailsController : Controller
    {
        private readonly PathologyHandbookContext _context;

        public ContainerDetailsController(PathologyHandbookContext context)
        {
            _context = context;
        }

        // GET: api/ContainerDetails
        [HttpGet]
        public IEnumerable<ContainerDetails> GetContainerDetails()
        {
            return _context.ContainerDetails;
        }

        // GET: api/ContainerDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContainerDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var containerDetails = await _context.ContainerDetails
                                            .Include(cd => cd.CollectionContainerType)
                                            .Include(cd => cd.SpecimenType)
                                            .SingleOrDefaultAsync(cd => cd.Id == id);

            if (containerDetails == null)
                return NotFound();

            return Ok(containerDetails);
        }

        // PUT: api/ContainerDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContainerDetails([FromRoute] int id, [FromBody] ContainerDetails containerDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != containerDetails.Id)
                return BadRequest();

            containerDetails.DateModified = DateTime.Now;
            containerDetails.ModifiedBy = HttpContext.User.Identity.Name;
            containerDetails.GeneralDetails = UpdateGeneralDetails(containerDetails);

            _context.Entry(containerDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContainerDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var updatedContainerDetails = await _context.ContainerDetails
                .Include(cd => cd.CollectionContainerType)
                .Include(cd => cd.SpecimenType)
                .SingleOrDefaultAsync(cd => cd.Id == id);

            if (updatedContainerDetails == null)
                return NotFound();

            return Ok(updatedContainerDetails);
        }

        // PUT: api/ContainerDetails/UpdateDrawOrder
        [HttpPut("/api/ContainerDetails/UpdateDrawOrder")]
        public async Task<IActionResult> PutUpdateDrawOrder([FromBody] List<ContainerDetails> containerDetails)
        {
            if (Equals(containerDetails, null))
                return BadRequest();

            if (containerDetails.Count <= 0)
                return BadRequest();

            for (int i = 0; i < containerDetails.Count; i++)
            {
                var foundCd = _context.ContainerDetails.SingleOrDefault(c => c.Id == containerDetails[i].Id);

                if (!Equals(foundCd, null))
                {
                    foundCd.DrawOrder = i + 1;
                    foundCd.GeneralDetails = UpdateGeneralDetails(foundCd);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            return NoContent();
        }

        private string UpdateGeneralDetails(ContainerDetails containerDetails)
        {
            var sb = new StringBuilder();

            var specimenType = _context.SpecimenTypes.SingleOrDefault(st => st.Id == containerDetails.SpecimenTypeId);

            if (!Equals(specimenType, null))
                sb.Append($"Specimen Type: {specimenType.Description}");

            var containerType =
                _context.CollectionContainerTypes.SingleOrDefault(ct =>
                    ct.Id == containerDetails.CollectionContainerTypeId);

            if (!Equals(containerType, null))
                sb.Append(
                    $", {containerType.Description}");
          

            return sb.ToString();
        }

        // POST: api/ContainerDetails
        [HttpPost]
        public async Task<IActionResult> PostContainerDetails([FromBody] ContainerDetails containerDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (containerDetails.TestId == 0)
                return BadRequest(ModelState);

            containerDetails.CreatedBy = HttpContext.User.Identity.Name;
            containerDetails.DateCreated = DateTime.Now;
            containerDetails.DrawOrder = GetLastContainerDrawOrderForTest(containerDetails.TestId) + 1;
            containerDetails.GeneralDetails = UpdateGeneralDetails(containerDetails);
            // Save to add back after creation
            var containerType = containerDetails.CollectionContainerType;
            var specimenType = containerDetails.SpecimenType;
            // Make them null so that save completes
            containerDetails.CollectionContainerType = null;
            containerDetails.SpecimenType = null;

            try
            {
                _context.ContainerDetails.Add(containerDetails);
                await _context.SaveChangesAsync();

                // Add types back in so they are returned to client
                containerDetails.CollectionContainerType = containerType;
                containerDetails.SpecimenType = specimenType;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return CreatedAtAction("GetContainerDetails", new { id = containerDetails.Id }, containerDetails);
        }

        private int GetLastContainerDrawOrderForTest(int testId)
        {
            var testContainers = _context.ContainerDetails.Where(cd => cd.TestId == testId);

            if (testContainers.Any())
            {
                testContainers = testContainers.OrderBy(cd => cd.DrawOrder);
                return testContainers.Last().DrawOrder.Value;
            }

            return 0;

        }

        // DELETE: api/ContainerDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContainerDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var containerDetails = await _context.ContainerDetails.SingleOrDefaultAsync(m => m.Id == id);

            if (containerDetails == null)
                return NotFound();

            _context.ContainerDetails.Remove(containerDetails);
            await _context.SaveChangesAsync();

            return Ok(containerDetails);
        }

        private bool ContainerDetailsExists(int id)
        {
            return _context.ContainerDetails.Any(e => e.Id == id);
        }
    }
}