﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoGeneratedTests
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    
    
    public class sales
    {
        
			public string firstName { get; set; } = "Sally";
			public string lastName { get; set; } = "Green";
			public int age { get; set; } = 27;
    }
    
    public class Example
    {
        
		public string docID { get; set; } = @"20120205-1000-1";

		public System.Nullable<string> status { get; set; } = @"Pending";

		public System.Nullable<string> firstName { get; set; } = @"Caelan";

		public sales sales { get; set; }
    }
}