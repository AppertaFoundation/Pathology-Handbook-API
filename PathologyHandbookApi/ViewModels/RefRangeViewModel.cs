//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿namespace PathologyHandbookApi.ViewModels
{
    public class RefRangeViewModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int Age { get; set; }
        public int AgeEndRange { get; set; }
        public string AgeRange { get; set; }
        public string DayMonthYear { get; set; }
        public string RefHigh { get; set; }
        public string RefLow { get; set; }
        public string Gender { get; set; }
        public string Notes { get; set; }
    }
}