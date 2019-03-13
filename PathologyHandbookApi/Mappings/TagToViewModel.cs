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
    public static class TagToViewModel
    {
        public static TagViewModel TagToTagViewModel(this Tag tag)
        {
            if (Equals(tag, null))
                return null;

            var tagViewModel = new TagViewModel()
            {
                Id = tag.Id,
                Active = tag.Active,
                Description = tag.Description,
                TagTypeId = tag.TagTypeId,
                TestId = tag.TestId,
                Value = tag.Value
            };

            return tagViewModel;
        }
    }
}