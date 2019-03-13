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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PathologyHandbookApi.Models
{
    public class PathologyHandbookContext : DbContext
    {
        public PathologyHandbookContext(DbContextOptions<PathologyHandbookContext> options) : base(options)
        {
            
        }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CollectionContainerType> CollectionContainerTypes { get; set; }
        public DbSet<ContactDetail> ContactDetails { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<ContainerDetails> ContainerDetails { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagType> TagTypes { get; set; }
        public DbSet<SpecimenType> SpecimenTypes { get; set; }
        public DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<RefRange> RefRanges { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TagType>().HasData(
                new TagType() { Id = 1, Code = "Code", CreatedBy = "Initial", DateCreated = DateTime.Now, Description = "Code", Active = true },
                new TagType() { Id = 2, Code = "Synonym", CreatedBy = "Initial", DateCreated = DateTime.Now, Description = "Synonym", Active = true },
                new TagType() { Id = 3, Code = "MainTestName", CreatedBy = "Initial", DateCreated = DateTime.Now, Description = "Main Test Name", Active = true },
                new TagType() { Id = 4, Code = "ConcurrentTest", CreatedBy = "Initial", DateCreated = DateTime.Now, Description = "Concurrent Test", Active = true },
                new TagType() { Id = 5, Code = "Department", CreatedBy = "Initial", DateCreated = DateTime.Now, Description = "Department", Active = true }
            );
        }
    }
}
