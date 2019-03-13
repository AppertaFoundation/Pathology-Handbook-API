//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tag = Swashbuckle.AspNetCore.Swagger.Tag;

namespace PathologyHandbookApi.ViewModels
{
    public class TagTypeViewModel
    {
        public TagTypeViewModel()
        {
            Tags = new List<TagViewModel>();
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<TagViewModel> Tags { get; set; }
    }
}
