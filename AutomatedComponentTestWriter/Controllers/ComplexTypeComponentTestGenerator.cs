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
    public class ComplexTypeComponentTestGenerator
    {
        private Random random = new Random();
        private ComplexObject complexType;
        private ComponentTestDTO dto;

        private CodeTypeDeclaration testSuite;
        
        public ComplexTypeComponentTestGenerator(CodeTypeDeclaration componentTestClass, ComponentTestDTO dataTransferObject)
        {
            testSuite = componentTestClass;
            dto = dataTransferObject;
        }

        public void GenerateTestsForComplexType(ComplexObject complexObject)
        {
            complexType = complexObject;

            foreach(Parameter param in complexType.Parameters)
            {
                CreateSingleUnitTestForComplexType(param, complexObject);
            }
        }

        private void CreateSingleUnitTestForComplexType(Parameter param, ComplexObject complex)
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
            SetDTOPropertyNameValue(param, complex, paramUnitTest);

            // Create the response message then add it to the DOM graph. This is an await action that posts to the api endpoint.
            CodeSnippetExpression responseExpression = new CodeSnippetExpression("await ApiActions.Post(_uri, request).ConfigureAwait(false)");
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
            CodeVariableReferenceExpression assertIsTrueExpression = new CodeVariableReferenceExpression("Assert.IsTrue(guidstring.Contains(\"" + param.ExpectedMessage + "\"))");
            paramUnitTest.Statements.Add(assertIsTrueExpression);

            testSuite.Members.Add(paramUnitTest);
        }

        private void SetDTOPropertyNameValue(Parameter param, ComplexObject complex, CodeMemberMethod paramUnitTest)
        {
            if (param.NullParam.Equals("True"))
            {
                if (param.ComplexMemberSpecifier.ToLower().Equals("entire type"))
                {
                    CodeVariableReferenceExpression requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + complex.ObjectName + " = null");
                    paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                }
                else
                {
                    CodeVariableReferenceExpression requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + complex.ObjectName + "." + param.ComplexMemberSpecifier + " = null");
                    paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                }
            }
            else if (param.BlankParam.Equals("True"))
            {

            }
            else if (param.RandomParam.Equals("True"))
            {
                // For each complex member, check to see if the name of the variable for that member is the same as the variable specified in the parameter.
                foreach (ComplexObjectMember member in complex.ComplexMembers)
                {
                    if (param.ComplexMemberSpecifier.Equals(member.Key))
                    {
                        ResolveRandomValue(param, complex, member.DataType, paramUnitTest);
                    }
                }
            }
        }

        private void ResolveRandomValue(Parameter param, ComplexObject complex, string dataType, CodeMemberMethod paramUnitTest)
        {
            CodeVariableReferenceExpression requestPropertyFieldExpression;
            switch (dataType.ToLower())
            {
                case "string":

                    // If no value length was assigned by the user, we pass "0", which means it creates strings of various random lengths instead of random strings of fixed length.
                    if (param.ValueLength.Equals(""))
                    {
                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + complex.ObjectName + "." + param.ComplexMemberSpecifier + " = \"" + CreateRandomString(0) + "\"");
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    else
                    {
                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + complex.ObjectName + "." + param.ComplexMemberSpecifier + " = \"" + CreateRandomString(int.Parse(param.ValueLength)) + "\"");
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    break;
                case "decimal":
                    requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + complex.ObjectName + "." + param.ComplexMemberSpecifier + " = " + CreateRandomDecimal() + "M");
                    paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    break;
                case "datetime":
                    requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + complex.ObjectName + "." + param.ComplexMemberSpecifier + " = DateTime.Parse(\"" + CreateRandomDate() + "\")");
                    paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    break;
                case "int":

                    if (param.ValueLength.Equals(""))
                    {
                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + complex.ObjectName + "." + param.ComplexMemberSpecifier + " = " + CreateRandomIntegerOfLength(0));
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    else
                    {
                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + complex.ObjectName + "." + param.ComplexMemberSpecifier + " = " + CreateRandomIntegerOfLength(int.Parse(param.ValueLength)));
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    break;
                case "bool":

                    Random random = new Random();
                    int zeroOrOne = random.Next(0, 1);

                    if (zeroOrOne == 0)
                    {
                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + complex.ObjectName + "." + param.ComplexMemberSpecifier + " = false");
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    else
                    {

                        requestPropertyFieldExpression = new CodeVariableReferenceExpression(dto.DTOName + "." + complex.ObjectName + "." + param.ComplexMemberSpecifier + " = true");
                        paramUnitTest.Statements.Add(requestPropertyFieldExpression);
                    }
                    break;
                default:
                    break;
            }
        }

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
                valueLength = random.Next();
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
