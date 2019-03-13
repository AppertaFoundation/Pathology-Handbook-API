//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PathologyHandbookApi.Models
{
    public class RefRange
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        [StringLength(255)]
        public string AgeRange { get; set; }
        public int Age { get; set; }
        public int? AgeEndRange { get; set; }
        [StringLength(255)]
        public string RefHigh { get; set; }
        [StringLength(255)]
        public string RefLow { get; set; }
        [StringLength(255)]
        public string Gender { get; set; }
        [StringLength(55)]
        public string DayMonthYear { get; set; }
        public string DayMonthYearAgeEnd { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool Active { get; set; }
    }
}
