﻿using AutomatedComponentTestWriter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace AutomatedComponentTestWriter.Controllers
{
    public class DTOGenerator
    {

        private ComponentTestDTO dataTransferObject;
        private CodeCompileUnit dataTransferObjectTemplate;
        private CodeNamespace dataTransferObjectNamespace;
        private CodeTypeDeclaration dataTransferObjectClass;

        private string fileName;

        public string FileName { get => fileName; set => fileName = value; }
        public CodeCompileUnit DtoTemplate { get => dataTransferObjectTemplate; set => dataTransferObjectTemplate = value; }
        public CodeNamespace DtoNamespace { get => dataTransferObjectNamespace; set => dataTransferObjectNamespace = value; }
        public CodeTypeDeclaration DtoClass { get => dataTransferObjectClass; set => dataTransferObjectClass = value; }
        internal ComponentTestDTO Dto { get => dataTransferObject; set => dataTransferObject = value; }

        // Constructor accepts an object detailing information about the DTO. This would be
        // what you get from the client in the asp.net application.
        public DTOGenerator(ComponentTestDTO dataObject, string fileN)
        {
            dataTransferObject = dataObject;
            fileName = fileN;

            dataTransferObjectTemplate = new CodeCompileUnit();
            dataTransferObjectNamespace = new CodeNamespace("AutoGeneratedTests");

            // Add namespace imports to the dataTransferObject class.
            dataTransferObjectNamespace.Imports.Add(new CodeNamespaceImport("System"));
            dataTransferObjectNamespace.Imports.Add(new CodeNamespaceImport("System.Linq"));
            dataTransferObjectNamespace.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));
            dataTransferObjectTemplate.Namespaces.Add(dataTransferObjectNamespace);

            // Declare the class and make it public.
            dataTransferObjectClass = new CodeTypeDeclaration
            {
                Name = dataTransferObject.DTOName,
                IsClass = true,
                TypeAttributes = System.Reflection.TypeAttributes.Public
            };

            AddPropertiesToDTOClass();
        }

        // This function generates the source file.
        public void GenerateCSharpCode()
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";

            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Generated");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (StreamWriter sourceWriter = new StreamWriter(Path.Combine(path, fileName), false, System.Text.Encoding.UTF8))
            {
                provider.GenerateCodeFromCompileUnit(dataTransferObjectTemplate, sourceWriter, options);
            }
        }

        // This function is a higher level abstraction to handle the logic of deciding if a property is a "simple" type or a complex type.
        private void AddPropertiesToDTOClass()
        {
            // For each property defined in our DTO, create a CodeSnippetField by calling smaller functions.
            foreach (var property in dataTransferObject.Properties)
            {
                if (property.DataType.ToLower().Equals("complex"))
                {
                    // A complex type is a special case.
                    CodeSnippetTypeMember propField = CreateComplexPropertyField(property);
                    dataTransferObjectClass.Members.Add(propField);
                }
                else
                {
                    CodeSnippetTypeMember propField = CreatePropertyField(property);
                    dataTransferObjectClass.Members.Add(propField);
                }
            }
            dataTransferObjectNamespace.Types.Add(dataTransferObjectClass);
        }

        //This function handles code snippets for "simple" types.
        private CodeSnippetTypeMember CreatePropertyField(Property prop)
        {
            CodeSnippetTypeMember propertyField = new CodeSnippetTypeMember();

            // A property is either required and thus not able to be set to null or it is not required, and is thus set to System.Nullable<T>.
            if (prop.Required.Equals("True"))
            {
                if (prop.DataType.ToLower().Equals("datetime"))
                {
                    CodeSnippetTypeMember dateTimeDefault = new CodeSnippetTypeMember
                    {
                        Text = "\t\tprivate System.DateTime_" + prop.PropertyName + " = " + "\"" + prop.DefaultValue + "\";"
                    };
                    dataTransferObjectClass.Members.Add(dateTimeDefault);
                    propertyField.Text = "\t\tpublic System.DateTime " + prop.PropertyName + " { get { return _" + prop.PropertyName + "; } set { _" + prop.PropertyName + " = value; } }\n";
                    return propertyField;
                }
                else
                {
                    propertyField.Text = "\t\tpublic " + prop.DataType.ToLower() + " " + prop.PropertyName + " { get; set; } = " + DefaultValue(prop) + ";\n";
                }
            }
            else
            {
                if (prop.DataType.ToLower().Equals("datetime"))
                {
                    CodeSnippetTypeMember dateTimeNowSnippet = new CodeSnippetTypeMember
                    {
                        Text = "\t\tprivate System.Nullable<System.DateTime> _" + prop.PropertyName + " = DateTime.Now;"
                    };

                    dataTransferObjectClass.Members.Add(dateTimeNowSnippet);

                    propertyField.Text = "\t\tpublic System.Nullable<System.DateTime> " + prop.PropertyName + " { get { return _" + prop.PropertyName + "; } set { _" + prop.PropertyName + " = value; } }\n";
                    return propertyField;
                }
                else
                {
                    propertyField.Text = "\t\tpublic System.Nullable<" + prop.DataType.ToLower() + "> " + prop.PropertyName + " { get; set; } = " + DefaultValue(prop) + ";\n";
                }
            }

            return propertyField;
        }

        // This handles the assignment of default values to properties.
        private string DefaultValue(Property prop)
        {
            string defaultValue = ""; 

            switch(prop.DataType.ToLower())
            {
                case "string":
                    defaultValue = "@\"" + prop.DefaultValue + "\"";
                    break;
                case "bool":
                    if (prop.DefaultValue.ToLower().Equals("true"))
                    {
                        defaultValue = "true";
                    }
                    else
                    {
                        defaultValue = "false";
                    }
                    defaultValue = prop.DefaultValue.ToLower();
                    break;
                case "int":
                    defaultValue = prop.DefaultValue;
                    break;
                case "decimal":
                    defaultValue = prop.DefaultValue;
                    break;
                default:
                    break;
            }
            return defaultValue;
        }

        // This function handles the creation of object/type classes to represent complex types.
        // These classes are just appended to the Data Transfer Object class as of current implementation.
        private CodeSnippetTypeMember CreateComplexPropertyField(Property prop)
        {
            ComplexTypeClassGenerator complexClassGen = new ComplexTypeClassGenerator(prop.ComplexType);
            CodeSnippetTypeMember complexTypeSnippet = null;

            if (prop.Required.ToLower().Equals("true"))
            {
                complexTypeSnippet = new CodeSnippetTypeMember()
                {
                    Text = "\t\tpublic " + prop.ComplexType.ObjectName + " " + prop.ComplexType.ObjectName.ToLower() + " { get; set; }"
                };
            }
            else
            {
                complexTypeSnippet = new CodeSnippetTypeMember
                {
                    Text = "\t\tpublic System.Nullable<" + prop.ComplexType.ObjectName + "> " + prop.ComplexType.ObjectName.ToLower() + " { get; set; }"
                };
            }

            DtoNamespace.Types.Add(complexClassGen.ComplexTypeClass);

            return complexTypeSnippet;
        }
    }
}