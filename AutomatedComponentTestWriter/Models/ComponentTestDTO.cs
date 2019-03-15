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
        public Property()
        {
            Parameters = new List<Parameter>();
        }
        public string PropertyName { get; set; }
        public string DataType { get; set; }
        public string Required { get; set; }
        public string DefaultValue { get; set; }
        public List<Parameter> Parameters { get; set; }
    }

    public class Parameter
    {
        public string ExpectedMessage { get; set; }
        public string HTTPResponse { get; set; }
        public string RandomParam { get; set; }
        public string NullParam { get; set; }
        public string BlankParam { get; set; }
        public string ValueLength { get; set; }
        public string TestName { get; set; }
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