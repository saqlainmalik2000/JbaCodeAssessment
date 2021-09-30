using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jba.UI.ViewModels
{
    public class PercipitationDataViewModel
    {
        public DateTime Date { get; set; }
        public int XRef { get; set; }
        public int YRef { get; set; }
        public int Value { get; set; }
    }
}
