//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using System.Collections.Generic;
using PathologyHandbookApi.Models;

namespace PathologyHandbookApi.ViewModels
{
    public class TestViewModel
    {
        public TestViewModel()
        {
            ConcurrentTests = new HashSet<TestViewModel>();
            Tags = new HashSet<TagViewModel>();
            Containers = new HashSet<ContainerDetailsViewModel>();
            RefRanges = new List<RefRangeViewModel>();
        }

        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public bool Active { get; set; }
        public string NameDescription { get; set; }
        public string Guidelines { get; set; }
        public string Interpretation { get; set; }
        public string MainCode { get; set; }
        public string ReferenceIntervalMale { get; set; }
        public string ReferenceIntervalFemale { get; set; }
        public string ReferenceIntervalPaediatric { get; set; }
        public string ReferenceIntervalNotes { get; set; }
        public string UnitsOfMeasurement { get; set; }
        public string GeneralDetails { get; set; }
        public string GpNormalTaT { get; set; }
        public string GpAcuteTaT { get; set; }
        public string IpNormalTaT { get; set; }
        public string IpAcuteTaT { get; set; }
        public string CostPerTest { get; set; }
        public string LabNotes { get; set; }
        public string LabProcessNotes { get; set; }
        public string LabStorageNotes { get; set; }

        public virtual DepartmentViewModel Department { get; set; }

        public virtual ICollection<TestViewModel> ConcurrentTests { get; set; }
        public virtual ICollection<TagViewModel> Tags { get; set; }
        public virtual ICollection<ContainerDetailsViewModel> Containers { get; set; }
        public virtual ICollection<RefRangeViewModel> RefRanges { get; set; }

    }
}