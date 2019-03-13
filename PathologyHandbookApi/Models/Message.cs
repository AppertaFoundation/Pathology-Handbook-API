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

namespace PathologyHandbookApi.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string DisplayMessage { get; set; }
        public bool Active { get; set; }

    }
}
