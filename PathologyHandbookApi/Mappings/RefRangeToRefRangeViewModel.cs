//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using PathologyHandbookApi.Models;
using PathologyHandbookApi.ViewModels;

namespace PathologyHandbookApi.Mappings
{
    public static class RefRangeToRefRangeViewModel
    {
        public static RefRangeViewModel ToRefRangeViewModel(this RefRange refRange)
        {
            if (Equals(refRange, null))
                return null;

            var refRangeViewModel = new RefRangeViewModel()
            {
                Id = refRange.Id,
                TestId = refRange.TestId,
                Age = refRange.Age,
                AgeRange = refRange.AgeRange,
                Gender = refRange.Gender,
                Notes = refRange.Notes,
                RefHigh = refRange.RefHigh,
                RefLow = refRange.RefLow,
                DayMonthYear = refRange.DayMonthYear
            };

            if (!Equals(refRange.AgeEndRange, null))
                refRangeViewModel.AgeEndRange = refRange.AgeEndRange.Value;

            return refRangeViewModel;
        }
    }
}