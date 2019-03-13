//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PathologyHandbookApi.Models;

namespace PathologyHandbookApi.Migrations
{
    [DbContext(typeof(PathologyHandbookContext))]
    [Migration("20190205165151_AddDayMonthYearAgeEndToRefRange")]
    partial class AddDayMonthYearAgeEndToRefRange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PathologyHandbookApi.Models.CollectionContainerType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("ColourHex");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<string>("GeneralDetails");

                    b.Property<string>("Mix")
                        .HasMaxLength(55);

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("CollectionContainerTypes");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("DepartmentId");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<string>("Role");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.ContactDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int>("ContactId");

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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<string>("ModifiedBy");

                    b.HasKey("Id");

                    b.ToTable("ContactTypes");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.ContainerDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("CollectionConditions");

                    b.Property<int>("CollectionContainerTypeId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("DietaryRequirements");

                    b.Property<int?>("DrawOrder");

                    b.Property<string>("GeneralDetails");

                    b.Property<string>("ModifiedBy");

                    b.Property<int>("NumberOfCollectionContainersRequired");

                    b.Property<int>("SpecimenTypeId");

                    b.Property<string>("StorageConditions");

                    b.Property<int>("TestId");

                    b.Property<string>("TransportConditions");

                    b.HasKey("Id");

                    b.HasIndex("CollectionContainerTypeId");

                    b.HasIndex("SpecimenTypeId");

                    b.HasIndex("TestId");

                    b.ToTable("ContainerDetails");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

            modelBuilder.Entity("PathologyHandbookApi.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CollectionContainerTypeId");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("FullsizeFileName")
                        .HasMaxLength(255);

                    b.Property<string>("ThumbnailFileName")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("CollectionContainerTypeId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("DisplayMessage");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.RefRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int>("Age");

                    b.Property<int?>("AgeEndRange");

                    b.Property<string>("AgeRange")
                        .HasMaxLength(255);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("DayMonthYear")
                        .HasMaxLength(55);

                    b.Property<string>("DayMonthYearAgeEnd");

                    b.Property<string>("Gender")
                        .HasMaxLength(255);

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Notes");

                    b.Property<string>("RefHigh")
                        .HasMaxLength(255);

                    b.Property<string>("RefLow")
                        .HasMaxLength(255);

                    b.Property<int>("TestId");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("RefRanges");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.SpecimenType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<string>("ModifiedBy");

                    b.HasKey("Id");

                    b.ToTable("SpecimenTypes");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<string>("LabNotes");

                    b.Property<string>("LabProcessNotes");

                    b.Property<string>("LabStorageNotes");

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

            modelBuilder.Entity("PathologyHandbookApi.Models.UnitOfMeasurement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<string>("ModifiedBy");

                    b.HasKey("Id");

                    b.ToTable("UnitOfMeasurements");
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Contact", b =>
                {
                    b.HasOne("PathologyHandbookApi.Models.Department", "Department")
                        .WithMany("Contacts")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.ContactDetail", b =>
                {
                    b.HasOne("PathologyHandbookApi.Models.Contact", "Contact")
                        .WithMany("ContactDetails")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade);

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

                    b.HasOne("PathologyHandbookApi.Models.SpecimenType", "SpecimenType")
                        .WithMany()
                        .HasForeignKey("SpecimenTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PathologyHandbookApi.Models.Test")
                        .WithMany("Containers")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Image", b =>
                {
                    b.HasOne("PathologyHandbookApi.Models.CollectionContainerType")
                        .WithMany("Images")
                        .HasForeignKey("CollectionContainerTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.RefRange", b =>
                {
                    b.HasOne("PathologyHandbookApi.Models.Test")
                        .WithMany("RefRanges")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PathologyHandbookApi.Models.Tag", b =>
                {
                    b.HasOne("PathologyHandbookApi.Models.TagType", "TagType")
                        .WithMany("Tags")
                        .HasForeignKey("TagTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PathologyHandbookApi.Models.Test", "Test")
                        .WithMany("Tags")
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
