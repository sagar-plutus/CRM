using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
   
    

        public class gmapRowTO
        {
            List<gmapElementsTO> elements;
            public List<gmapElementsTO> Elements { get => elements; set => elements = value; }
        }

        public class gmapElementsTO
        {
            textVal distance;
            textVal duration;
            String status;
            public textVal Distance { get => distance; set => distance = value; }
            public textVal Duration { get => duration; set => duration = value; }
            public String Status { get => status; set => status = value; }
        }

        public class textVal
        {
            String text;
            Int32 value;
            public string Text { get => text; set => text = value; }
            public int Value { get => value; set => this.value = value; }

        }
    

}
