//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using System;

namespace PathologyHandbookApi.Models
{
    public class ContainerDetails
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int CollectionContainerTypeId { get; set; }
        public int SpecimenTypeId { get; set; }
        public int? DrawOrder { get; set; }
        public string GeneralDetails { get; set; }
        public string CollectionConditions { get; set; }
        public string TransportConditions { get; set; }
        public string StorageConditions { get; set; }
        public string DietaryRequirements { get; set; }
        public int NumberOfCollectionContainersRequired { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual SpecimenType SpecimenType { get; set; }
        public virtual CollectionContainerType CollectionContainerType { get; set; }
    }
}