﻿using PFA.Data.Localize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFA.Areas.MvcDashboardLocalize.Models.Key
{
    public class EditModel : BaseEditModel<Data.Localize.Key>
    {
        public string? ArgumentNames { get; set; }

        public List<KeyValue> Values { get; set; } = new();

        public string? SourceCulture { get; set; }

        public bool SaveAsCopy { get; set; }

        public Data.Localize.Domain[]? Domains { get; internal set; }

        public bool HasTranslationService { get; internal set; }
        
    }
}
