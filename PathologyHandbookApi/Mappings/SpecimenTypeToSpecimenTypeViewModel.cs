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
    public static class SpecimenTypeToSpecimenTypeViewModel
    {
        public static SpecimenTypeViewModel ToSpecimenTypeViewModel(this SpecimenType specimenType)
        {
            if (Equals(specimenType, null))
                return null;

            var specimenTypeViewModel = new SpecimenTypeViewModel()
            {
                Description = specimenType.Description,
                Id = specimenType.Id,
                Code = specimenType.Code,
                Active = specimenType.Active
            };

            return specimenTypeViewModel;
        }
    }
}