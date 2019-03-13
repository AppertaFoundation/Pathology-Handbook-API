//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using System.Linq;
using PathologyHandbookApi.Models;
using PathologyHandbookApi.ViewModels;

namespace PathologyHandbookApi.Mappings
{
    public static class TestToTestViewModel
    {
        public static TestViewModel ToTestViewModel(this Test test)
        {
            if (Equals(test, null))
                return null;

            var testViewModel = new TestViewModel()
            {
                Id = test.Id,
                Department = test?.Department.DepartmentToViewModel(),
                Active = test.Active,
                NameDescription = test.NameDescription,
                GeneralDetails = test.GeneralDetails,
                DepartmentId = test.DepartmentId,
                CostPerTest = test.CostPerTest,
                GpAcuteTaT = test.GpAcuteTaT,
                GpNormalTaT = test.GpNormalTaT,
                Interpretation = test.Interpretation,
                Guidelines = test.Guidelines,
                IpAcuteTaT = test.IpAcuteTaT,
                IpNormalTaT = test.IpNormalTaT,
                MainCode = test.MainCode,
                ReferenceIntervalFemale = test.ReferenceIntervalFemale,
                ReferenceIntervalMale = test.ReferenceIntervalMale,
                ReferenceIntervalNotes = test.ReferenceIntervalNotes,
                ReferenceIntervalPaediatric = test.ReferenceIntervalPaediatric,
                UnitsOfMeasurement = test.UnitsOfMeasurement,
                LabNotes = test.LabNotes,
                LabProcessNotes = test.LabProcessNotes,
                LabStorageNotes = test.LabStorageNotes
            };

            if (test.ConcurrentTests.Any())
            {
                foreach (var t in test.ConcurrentTests)
                {
                    testViewModel.ConcurrentTests.Add(t.ToTestViewModel());
                }
            }

            if (test.Tags.Any())
            {
                foreach (var s in test.Tags)
                {
                    testViewModel.Tags.Add(s.TagToTagViewModel());
                }
            }

            if (test.Containers.Any())
            {
                foreach (var c in test.Containers)
                {
                    testViewModel.Containers.Add(c.ContainerDetailsToViewModel());
                }
            }

            if (test.RefRanges.Any())
            {
                foreach (var rr in test.RefRanges)
                {
                    testViewModel.RefRanges.Add(rr.ToRefRangeViewModel());
                }
            }

            return testViewModel;
        }
    }
}