//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace PathologyHandbookApi.Models
{
    public class QueryObject
    {
        public string SortBy { get; set; }
        public string FilterBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool Active { get; set; }
        public string SearchTerm { get; set; }
    }
}
