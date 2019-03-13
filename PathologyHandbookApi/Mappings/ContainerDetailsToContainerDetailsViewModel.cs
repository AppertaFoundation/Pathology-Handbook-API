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
    public static class ContainerDetailsToContainerDetailsViewModel
    {
        public static ContainerDetailsViewModel ContainerDetailsToViewModel(this ContainerDetails containerDetails)
        {
            if (Equals(containerDetails, null))
                return null;

            var containerDetailsViewModel = new ContainerDetailsViewModel()
            {
                Id = containerDetails.Id,
                TestId = containerDetails.TestId,
                CollectionContainerType = containerDetails?.CollectionContainerType.CollectionContainerTypeToViewModel(),
                SpecimenType = containerDetails?.SpecimenType.ToSpecimenTypeViewModel(),
                Active = containerDetails.Active,
                GeneralDetails = containerDetails.GeneralDetails,
                CollectionConditions = containerDetails.CollectionConditions,
                CollectionContainerTypeId = containerDetails.CollectionContainerTypeId,
                DietaryRequirements = containerDetails.DietaryRequirements,
                DrawOrder = containerDetails.DrawOrder,
                NumberOfCollectionContainersRequired = containerDetails.NumberOfCollectionContainersRequired,
                SpecimenTypeId = containerDetails.SpecimenTypeId,
                StorageConditions = containerDetails.StorageConditions,
                TransportConditions = containerDetails.TransportConditions
            };

            return containerDetailsViewModel;
        }
    }
}