﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectData.Models
{
    public class Region
    {
        public int region_id { get; set; }

        public int country_id { get; set; }

        public string name { get; set; }
    }
}
