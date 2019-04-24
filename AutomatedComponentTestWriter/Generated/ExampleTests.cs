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
    using System.Net;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    
    public class ExampleUnitTests
    {
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestRandomDocID()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            Example.docID = "NxszXKA5uT";
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Success"));
        }
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestNullDocID()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            Example.docID = null;
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Failed. DocID can't null!"));
        }
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestDocID()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Success"));
        }
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestBlankDocID()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            Example.docID. = string.Empty;
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Failed. DocID can't be null."));
        }
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestRandomDocIDInvalidLength()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            Example.docID = "yB6a";
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Could not process request"));
        }
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestStatus()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Success"));
        }
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestRandomName1()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            Example.firstName = "gDZQ";
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Success"));
        }
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestRandomName2()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            Example.firstName = "Gxd481";
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Success"));
        }
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestRandomName3()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            Example.firstName = "sdao1wJqvB";
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Success"));
        }
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestRandomName4()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            Example.firstName = "7pLokUJ";
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Success"));
        }
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestRandomName5()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            Example.firstName = "Zu";
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Success"));
        }
        
        [TestMethod()]
        [TestCategory("Integration")]
        public async Task AutoGeneratedUnitTest_TestNullSales()
        {
            var _uri = "www.example.com/exampleapi/exampleendpoint";
            var request = new Example();
            Example.sales = null;
            var response = await ApiActions.Post(_uri, request).ConfigureAwait(false);
            var errormessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(errormessage);
            var guidstring = JsonConvert.ToString(errormessage);
            Assert.IsTrue(guidstring.Contains(@"Failed request. Sales must not be null"));
        }
    }
}