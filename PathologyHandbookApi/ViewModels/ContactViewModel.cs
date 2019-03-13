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
    public class ContactViewModel
    {
        public ContactViewModel()
        {
            ContactDetails = new List<ContactDetail>();
        }
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string DepartmentName { get; set; }
        public bool Active { get; set; }
        public IList<ContactDetail> ContactDetails { get; set; }

    }
}