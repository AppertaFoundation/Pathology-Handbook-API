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
using Tag = Swashbuckle.AspNetCore.Swagger.Tag;

namespace PathologyHandbookApi.Mappings
{
    public static class TagTypeToTagTypeViewModel 
    {
        public static TagTypeViewModel ToTagTypeViewModel(this TagType tagType)
        {
            if (Equals(tagType, null))
                return null;

            var tagTypeViewModel = new TagTypeViewModel()
            {
                Description = tagType.Description,
                Id = tagType.Id,
                Code = tagType.Code,
                Active = tagType.Active
            };

            if (tagType.Tags.Any())
            {
                foreach (var tag in tagType.Tags)
                {
                    tagTypeViewModel.Tags.Add(tag.TagToTagViewModel());
                }
            }

            return tagTypeViewModel;
        }
    }
}