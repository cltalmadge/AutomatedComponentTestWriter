﻿using AutomatedComponentTestWriter.Models;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedComponentTestWriter.Controllers
{
    class ComponentTestGenerator
    {
        private Random random = new Random();
        private ComponentTestDTO dto;

        // CodeDOM objects
        private CodeCompileUnit componentTestTemplate;
        private CodeNamespace componentTestNamespace;
        private CodeTypeDeclaration componentTestClass;

        // Object to handle generation of component tests for complex types.
        private ComplexTypeComponentTestGenerator complexUnitTestGenerator;

        private string className;

        public ComponentTestGenerator(ComponentTestDTO dataTransferObject, string clsName)
        {
            dto = dataTransferObject;
            className = clsName;

            componentTestTemplate = new CodeCompileUnit();
            componentTestNamespace = new CodeNamespace("AutoGeneratedTests");

            //Add namespace imports to the class. 
            componentTestNamespace.Imports.Add(new CodeNamespaceImport("System"));
            componentTestNamespace.Imports.Add(new CodeNamespaceImport("System.Net"));
            componentTestNamespace.Imports.Add(new CodeNamespaceImport("Newtonsoft.Json"));
            componentTestNamespace.Imports.Add(new CodeNamespaceImport("System.Threading.Tasks"));
            componentTestNamespace.Imports.Add(new CodeNamespaceImport("Microsoft.VisualStudio.TestTools.UnitTesting"));
            componentTestTemplate.Namespaces.Add(componentTestNamespace);

            // Declare the class and make it public.
            componentTestClass = new CodeTypeDeclaration();
            componentTestClass.Name = dto.DTOName + "UnitTests";
            componentTestClass.IsClass = true;
            componentTestClass.TypeAttributes = System.Reflection.TypeAttributes.Public;

            // Add type class for this test suite to an object that will handle complex types for our test suite.
            complexUnitTestGenerator = new ComplexTypeComponentTestGenerator(componentTestClass, dto);
            
            GenerateUnitTests();
        }

        public void GenerateCSharpCode()
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";

            // Map the relative path to the server, then check to see if a directory for created source files doesn't exist already. If it doesn't, create it. If it does, don't do anything.
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Generated");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (StreamWriter sourceWriter = new StreamWriter(Path.Combine(path, className), false, System.Text.Encoding.UTF8))
            {
                provider.GenerateCodeFromCompileUnit(
                    componentTestTemplate, sourceWriter, options);
            }
        }

        private void GenerateUnitTests()
        {
            // For every property, examine it to see if we're handling a complex type or a regular type to generate tests.
            foreach (Property dtoProperty in dto.Properties)
            {
                if (dtoProperty.DataType.ToLower().Equals("complex"))
                {
                    complexUnitTestGenerator.GenerateTestsForComplexType(dtoProperty.ComplexType);
                }
                else
                {
                    foreach (Parameter param in dtoProperty.Parameters)
                    {
                        CreateSingleUnitTest(param, dtoProperty);
                    }
                }
            }

            componentTestNamespace.Types.Add(componentTestClass);
        }

        private void CreateSingleUnitTest(Parameter param, Property prop)
        {
            CodeMemberMethod paramUnitTest = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = "AutoGeneratedUnitTest_" + param.TestName + ""
            };
            paramUnitTest.ReturnType = new CodeTypeReference("async Task");

            // Generates: [TestMethod()]
            paramUnitTest.CustomAttributes.Add(new CodeAttributeDeclaration("TestMethod"));

            // Generates: [TestCategory("Integration")]
            paramUnitTest.CustomAttributes.Add(new CodeAttributeDeclaration(
                "TestCategory",
                new CodeAttributeArgument(new CodePrimitiveExpression("Integration"))));

            // Create the declaration for the URI field and add it to the DOM for the method.
            CodeSnippetExpression uriExpression = new CodeSnippetExpression("\"" + dto.APIEndpointURL + "\"");
            CodeVariableDeclarationStatement uriStatement = new CodeVariableDeclarationStatement("var", "_uri", uriExpression);
            paramUnitTest.Statements.Add(uriStatement);

            // Create the declaration for the dto and add it to the DOM for the method.
            CodeSnippetExpression requestExpression = new CodeSnippetExpression("new " + dto.DTOName + "()");
            CodeVariableDeclarationStatement requestStatement = new CodeVariableDeclarationStatement("var", "request", requestExpression);
            paramUnitTest.Statements.Add(requestStatement);

            // Create the declaration for accessing the dto's property.
            // Add it to the DOM graph for the method afterwards.
            SetDTOPropertyNameValue(param, prop, paramUnitTest);

            // Create the response message then add it to the DOM graph. This is an await action that posts to the api endpoint.
            CodeSnippetExpression responseExpression = new CodeSnippetExpression("await ApiActions." + dto.APIAction + "(_uri, request).ConfigureAwait(false)");
            CodeVariableDeclarationStatement responseStatement = new CodeVariableDeclarationStatement("var", "response", responseExpression);
            paramUnitTest.Statements.Add(responseStatement);

            // Create the await error message and add it to the DOM graph.
            CodeSnippetExpression errormessageExpression = new CodeSnippetExpression("await response.Content.ReadAsStringAsync().ConfigureAwait(false)");
            CodeVariableDeclarationStatement errormessageStatement = new CodeVariableDeclarationStatement("var", "errormessage", errormessageExpression);
            paramUnitTest.Statements.Add(errormessageStatement);

            // Add an assert statement checking two status codes are equal to the DOM graph.
            CodeVariableReferenceExpression assertAreEqualFieldExpression = new CodeVariableReferenceExpression("Assert.AreEqual(HttpStatusCode." + param.HTTPResponse + ", response.StatusCode)");
            paramUnitTest.Statements.Add(assertAreEqualFieldExpression);

            // Add an assert statement confirming that the error message is not null to the DOM graph
            CodeVariableReferenceExpression assertIsNotNullExpression = new CodeVariableReferenceExpression("Assert.IsNotNull(errormessage)");
            paramUnitTest.Statements.Add(assertIsNotNullExpression);

            // Create a guidstring expression that converts the error message to a string.
            CodeSnippetExpression guidstringExpression = new CodeSnippetExpression("JsonConvert.ToString(errormessage)");
            CodeVariableDeclarationStatement guidstringStatement = new CodeVariableDeclarationStatement("var", "guidstring", guidstringExpression);
            paramUnitTest.Statements.Add(guidstringStatement);

            // Assert that the guidstring contains the expected error message.
            CodeVariableReferenceExpression assertIsTrueExpression = new CodeVariableReferenceExpression("Assert.IsTrue(guidstring.Contains(@\"" + param.ExpectedMessage + "\"))");
            paramUnitTest.Statements.Add(assertIsTrueExpression);

            componentTestClass.Members.Add(paramUnitTest);
        }

        private void SetDTOPropertyNameValue(Parameter param, Property prop, CodeMemberMethod paramUnitTest)
        {
            if (param.NullParam.Equals("True"))
            {
                CodeVariableReferenceExpression requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + " = null");
                paramUnitTest.Statements.Add(requestPropertyFieldExpression);
            }
            else if (param.BlankParam.Equals("True"))
            {
                ResolveBlankValue(param, prop, paramUnitTest);
            }
            else if (param.RandomParam.Equals("True"))
            {
                ResolveRandomValue(param, prop, paramUnitTest);
            }
        }

        private void ResolveBlankValue(Parameter param, Property prop, CodeMemberMethod paramUnitTest)
        {
            CodeVariableReferenceExpression requestPropertyFieldExpression;

            switch (prop.DataType.ToLower())
            {
                case "string":
                    requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + "." + param.ComplexMemberSpecifier + " = string.Empty");
                    paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    break;
                case "datetime":
                    requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + "." + param.ComplexMemberSpecifier + " = DateTime.MinValue");
                    paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    break;
                case "int":
                    requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + "." + param.ComplexMemberSpecifier + " = 0");
                    paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    break;
                case "decimal":
                    requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + "." + param.ComplexMemberSpecifier + " = 0M");
                    paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    break;
                case "bool":
                    requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + "." + param.ComplexMemberSpecifier + " = null");
                    paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    break;
                default:
                    break;
            }
        }

        private void ResolveRandomValue(Parameter param, Property prop, CodeMemberMethod paramUnitTest)
        {
            CodeVariableReferenceExpression requestPropertyFieldExpression;
            switch (prop.DataType.ToLower())
            {
                case "string":

                    // If no value length was assigned by the user, we pass "0", which means it creates strings of various random lengths instead of random strings of fixed length.
                    if (param.ValueLength == null)
                    {
                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + " = \"" + CreateRandomString(0) + "\"");
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    else
                    {
                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + " = \"" + CreateRandomString(int.Parse(param.ValueLength)) + "\"");
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    break;
                case "decimal":
                    requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + " = " + CreateRandomDecimal() + "M");
                    paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    break;
                case "datetime":
                    requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + " = DateTime.Parse(\"" + CreateRandomDate() + "\")");
                    paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    break;
                case "int":

                    if (param.ValueLength.Equals(""))
                    {
                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + " = " + CreateRandomIntegerOfLength(0));
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    else
                    {
                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + " = " + CreateRandomIntegerOfLength(int.Parse(param.ValueLength)));
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    break;
                case "bool":

                    Random random = new Random();
                    int zeroOrOne = random.Next(0, 1);

                    if (zeroOrOne == 0)
                    {
                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + " = false");
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    else
                    {

                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + prop.PropertyName + " = true");
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    break;
                default:
                    break;
            }
        }
        
        /* Functions for handling the creation of random values of various types. */
        private string CreateRandomDecimal()
        {
            double randomValue = random.NextDouble();
            decimal theDecimal = (decimal)randomValue;

            string decimalString = theDecimal.ToString();
            return decimalString;
        }

        private string CreateRandomString(int valueLength)
        {
            // A list of allowed characters.
            if (valueLength == 0)
            {
                // Just an arbitrary placeholder to deal with empty value length fields.
                valueLength = random.Next(0, 255);
            }

            string allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[valueLength];


            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = allowed[random.Next(allowed.Length)];
            }

            return new string(stringChars);
        }

        private int CreateRandomIntegerOfLength(int valueLength)
        {
            // A list of allowed characters.
            string allowed = "0123456789";

            // We want an integer of a specific "length."
            if (valueLength == 0)
            {
                return random.Next(); // If no value length specified, just create a random integer.
            }

            if (valueLength > 9)
            {
                valueLength = 9; // To avoid creating integers larger than int32 can store.
            }

            var stringChars = new char[valueLength];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = allowed[random.Next(allowed.Length)];
            }

            return int.Parse(new string(stringChars));
        }

        private string CreateRandomDate()
        {
            DateTime start = new DateTime(1995, 1, 1);

            int range = (DateTime.Today - start).Days;

            return start.AddDays(random.Next(range)).ToString();
        }
    }
}