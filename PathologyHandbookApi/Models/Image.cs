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
    public class Image
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(255)]
        public string FullsizeFileName { get; set; }
        [StringLength(255)]
        public string ThumbnailFileName { get; set; }
        public int CollectionContainerTypeId { get; set; }
    }
}
