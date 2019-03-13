//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using System;
using System.Collections.Generic;

namespace PathologyHandbookApi.Models
{
    public class ContactType
    {
        public ContactType()
        {
            ContactDetails = new HashSet<ContactDetail>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool Active { get; set; }
        
        public virtual ICollection<ContactDetail> ContactDetails { get; set; }
    }
}