//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿namespace PathologyHandbookApi.ViewModels
{
    public class SpecimenTypeViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }
    }
}