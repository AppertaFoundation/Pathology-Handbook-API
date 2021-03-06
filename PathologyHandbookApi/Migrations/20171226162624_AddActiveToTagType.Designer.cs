//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PathologyHandbookApi.Models;
using System;

namespace PathologyHandbookApi.Migrations
{
    [DbContext(typeof(PathologyHandbookContext))]
    [Migration("20171226162624_AddActiveToTagType")]
    partial class AddActiveToTagType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PathologyHandbookApi.Models.CollectionContainerType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("ColourHex");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<string>("GeneralDetails");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("CollectionContainerTypes");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("DepartmentId");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.ContactDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ContactId");

                    b.Property<int>("ContactTypeId");

                    b.Property<string>("ContactValue");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("ModifiedBy");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.HasIndex("ContactTypeId");

                    b.ToTable("ContactDetails");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.ContactType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContactTypeDescription");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("ModifiedBy");

                    b.HasKey("Id");

                    b.ToTable("ContactTypes");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.ContainerDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("CollectionConditions");

                    b.Property<int>("CollectionContainerTypeId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<string>("DietaryRequirements");

                    b.Property<int?>("DrawOrder");

                    b.Property<string>("GeneralDetails");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfCollectionContainersRequired");

                    b.Property<string>("SpecimenType");

                    b.Property<string>("StorageConditions");

                    b.Property<int?>("TestId");

                    b.Property<string>("TransportConditions");

                    b.HasKey("Id");

                    b.HasIndex("CollectionContainerTypeId");

                    b.HasIndex("TestId");

                    b.ToTable("ContainerDetails");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<string>("OpeningHours");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<string>("ModifiedBy");

                    b.Property<int>("TagTypeId");

                    b.Property<int>("TestId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("TagTypeId");

                    b.HasIndex("TestId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.TagType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<string>("ModifiedBy");

                    b.HasKey("Id");

                    b.ToTable("TagTypes");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("CostPerTest");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("DepartmentId");

                    b.Property<string>("GeneralDetails");

                    b.Property<string>("GpAcuteTaT");

                    b.Property<string>("GpNormalTaT");

                    b.Property<string>("Guidelines");

                    b.Property<string>("Interpretation");

                    b.Property<string>("IpAcuteTaT");

                    b.Property<string>("IpNormalTaT");

                    b.Property<string>("MainCode");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("NameDescription");

                    b.Property<string>("ReferenceIntervalFemale");

                    b.Property<string>("ReferenceIntervalMale");

                    b.Property<string>("ReferenceIntervalNotes");

                    b.Property<string>("ReferenceIntervalPaediatric");

                    b.Property<int?>("TestId");

                    b.Property<string>("UnitsOfMeasurement");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("TestId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Contact", b =>
                {
                    b.HasOne("PathologyHandbookApi.Models.Department")
                        .WithMany("Contacts")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.ContactDetail", b =>
                {
                    b.HasOne("PathologyHandbookApi.Models.Contact")
                        .WithMany("ContactDetails")
                        .HasForeignKey("ContactId");

                    b.HasOne("PathologyHandbookApi.Models.ContactType", "ContactType")
                        .WithMany("ContactDetails")
                        .HasForeignKey("ContactTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.ContainerDetails", b =>
                {
                    b.HasOne("PathologyHandbookApi.Models.CollectionContainerType", "CollectionContainerType")
                        .WithMany()
                        .HasForeignKey("CollectionContainerTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PathologyHandbookApi.Models.Test")
                        .WithMany("Containers")
                        .HasForeignKey("TestId");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Tag", b =>
                {
                    b.HasOne("PathologyHandbookApi.Models.TagType", "TagType")
                        .WithMany("Tags")
                        .HasForeignKey("TagTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PathologyHandbookApi.Models.Test", "Test")
                        .WithMany("Synonyms")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Test", b =>
                {
                    b.HasOne("PathologyHandbookApi.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PathologyHandbookApi.Models.Test")
                        .WithMany("ConcurrentTests")
                        .HasForeignKey("TestId");
                });
#pragma warning restore 612, 618
        }
    }
}
