//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
using System.Collections.Generic;

namespace PathologyHandbookApi.Models
{
    public class QueryResults<T>
    {
        public QueryResults()
        {
            Items = new List<T>();
        }
        public int TotalItems { get; set; }
        public double TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public IList<T> Items { get; set; }
    }
}