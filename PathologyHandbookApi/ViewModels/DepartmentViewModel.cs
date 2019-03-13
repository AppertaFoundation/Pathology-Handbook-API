//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using System.Collections.Generic;

namespace PathologyHandbookApi.ViewModels
{
    public class DepartmentViewModel
    {
        public DepartmentViewModel()
        {
            Contacts = new HashSet<ContactViewModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string OpeningHours { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<ContactViewModel> Contacts { get; set; }

    }
}