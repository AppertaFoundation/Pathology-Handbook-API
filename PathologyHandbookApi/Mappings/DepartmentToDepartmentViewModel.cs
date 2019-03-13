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

namespace PathologyHandbookApi.Mappings
{
    public static class DepartmentToDepartmentViewModel
    {
        public static DepartmentViewModel DepartmentToViewModel(this Department department)
        {
            if (Equals(department, null))
                return null;

            var departmentViewModel = new DepartmentViewModel()
            {
                Name = department.Name,
                Id = department.Id,
                Active = department.Active,
                OpeningHours = department.OpeningHours
            };

            if (department.Contacts.Any())
            {
                foreach (var c in department.Contacts)
                {
                    departmentViewModel.Contacts.Add(c.ContactToContactViewModel());
                }
            }

            return departmentViewModel;
        }
    }
}