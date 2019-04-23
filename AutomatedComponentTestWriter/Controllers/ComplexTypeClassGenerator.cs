using AutomatedComponentTestWriter.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomatedComponentTestWriter.Controllers
{
    public class ComplexTypeClassGenerator
    {
        private ComplexObject complexType;
        private CodeTypeDeclaration complexTypeClass;

        public CodeTypeDeclaration ComplexTypeClass { get => complexTypeClass; set => complexTypeClass = value; }
        internal ComplexObject ComplexType { get => complexType; set => complexType = value; }

        /* Takes a complex object describing a complex type and generates a class for an object 
         * representing the complex type.
         */
        public ComplexTypeClassGenerator(ComplexObject compType)
        {
            complexTypeClass = new CodeTypeDeclaration();
            complexType = compType;
            CreateComplexObjectClass();
        }

        private void CreateComplexObjectClass()
        {
            // Sets the name of the class, makes the class public.
            complexTypeClass.Name = complexType.ObjectName;
            complexTypeClass.Attributes = MemberAttributes.Public;
            
            foreach (ComplexObjectMember complexMember in complexType.ComplexMembers)
            {
                CodeSnippetTypeMember complexFieldTypeMember = new CodeSnippetTypeMember();
                complexFieldTypeMember.Text = "\t\t\tpublic " + complexMember.DataType.ToLower() + " " + complexMember.Key + " { get; set; } = " + DefaultValue(complexMember) + ";";
                complexTypeClass.Members.Add(complexFieldTypeMember);
            }
        }

        // Function that handles the assignment of default values to complex type members.
        private string DefaultValue(ComplexObjectMember member)
        {
            string defaultValue = "";
            switch (member.DataType.ToLower())
            {
                case "string":
                    defaultValue = "\"" + member.Value + "\"";
                    break;
                case "bool":
                    if (member.Value.ToLower().Equals("True"))
                    {
                        defaultValue = "true";
                    }
                    else
                    {
                        defaultValue = "false";
                    }
                    defaultValue = "\"" + member.Value + "\"";
                    break;
                case "int":
                    defaultValue = member.Value;
                    break;
                case "decimal":
                    defaultValue = member.Value;
                    break;
                default:
                    break;
            }
            return defaultValue;
        }
    }
}