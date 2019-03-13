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
    public static class CollectionContainerTypeToContainerTypeViewModel
    {
        public static CollectionContainerTypeViewModel CollectionContainerTypeToViewModel(
            this CollectionContainerType collectionContainerType)
        {
            if (Equals(collectionContainerType, null))
                return null;

            var containerTypeViewModel = new CollectionContainerTypeViewModel()
            {
                Id = collectionContainerType.Id,
                Active = collectionContainerType.Active,
                Name = collectionContainerType.Name,
                Description = collectionContainerType.Description,
                ColourHex = collectionContainerType.ColourHex,
                GeneralDetails = collectionContainerType.GeneralDetails,
                Mix = collectionContainerType.Mix
            };

            return containerTypeViewModel;
        }
    }
}