using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomatedComponentTestWriter.Models
{
    public class Attribute
    {
        public string PropertyName { get; set; }
        public string DataType { get; set; }
        public bool? Required { get; set; }
        public string DefaultValue { get; set; }
        public string[][] Parameters { get; set; }
    }
   
}