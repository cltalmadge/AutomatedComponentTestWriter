using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomatedComponentTestWriter.Models
{
    public class ComponentTestDTO
    {
        public ComponentTestDTO()
        {
            Properties = new List<Property>();
        }
        public APIAction APIAction { get; set; }
        public string APIEndpointURL { get; set; }
        public string DTOName { get; set; }
        public List<Property> Properties { get; set; }
    }

    public class Property
    {
        public string PropertyName { get; set; }
        public string DataType { get; set; }
        public string Required { get; set; }
        public string DefaultValue { get; set; }
        public ComplexObject ComplexType { get; set; }
        public List<Parameter> Parameters { get; set; }

        public Property()
        {
            Parameters = new List<Parameter>();
        }
    }

    public class Parameter
    {
        public string ExpectedMessage { get; set; }
        public string HTTPResponse { get; set; }
        public string RandomParam { get; set; } = "False";
        public string NullParam { get; set; } = "False";
        public string BlankParam { get; set; } = "False";
        public string ValueLength { get; set; }
        public string TestName { get; set; }
    }

    public class ComplexObject
    {
        public string ObjectName { get; set; }
        public List<ComplexObjectMember> ComplexMembers { get; set; }

        public ComplexObject()
        {
            ComplexMembers = new List<ComplexObjectMember>();
        }
    }

    public class ComplexObjectMember
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }
    }

    public enum APIAction
    {
        POST,
        PUT
    }

    public enum HTTPResponse
    {
        BadRequest,
        Unauthorized,
        NotFound,
        OK,
        InternalServerError
    }
}