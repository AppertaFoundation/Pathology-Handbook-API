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
    public static class ContactToViewModel
    {
        public static ContactViewModel ContactToContactViewModel(this Contact contact)
        {
            if (Equals(contact, null))
                return null;

            var contactViewModel = new ContactViewModel()
            {
                Id = contact.Id,
                Active = contact.Active,
                DepartmentId = contact.DepartmentId,
                Name = contact.Name,
                Role = contact.Role,
                DepartmentName = contact?.Department.Name,
                ContactDetails = contact?.ContactDetails
            };

            return contactViewModel;
        }
    }
}