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
    public class Tag
    {
        public int Id { get; set; }
        public int TagTypeId { get; set; }
        public int TestId { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }

        public virtual TagType TagType { get; set; }
        public virtual Test Test { get; set; }
    }
}