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

            complexTypeClass.Name = complexType.ObjectName;
            complexTypeClass.Attributes = MemberAttributes.Public;
            CodeMemberField complexField = new CodeMemberField();

            foreach (ComplexObjectMember complexMember in complexType.ComplexMembers)
            {
                complexField.Name = complexMember.Key;
                complexField.Attributes = MemberAttributes.Public;


                CodeSnippetTypeMember complexFieldTypeMember = new CodeSnippetTypeMember();
                CodeSnippetTypeMember complexFieldDefaultValue = new CodeSnippetTypeMember();

                complexFieldDefaultValue = CreateDefaultValue(complexMember);

                complexFieldTypeMember.Text = "\t\t public " + complexMember.DataType.ToLower() + " " + complexMember.Key + " { get; set; }";

                complexTypeClass.Members.Add(complexFieldDefaultValue);
                complexTypeClass.Members.Add(complexFieldTypeMember);
            }
        }

        private CodeSnippetTypeMember CreateDefaultValue(ComplexObjectMember member)
        {
            CodeSnippetTypeMember defaultValue = new CodeSnippetTypeMember();

            switch (member.DataType.ToLower())
            {
                case "int":
                    defaultValue.Text = "\t\t[DefaultValue(" + member.Value + ")]";
                    break;
                case "string":
                    defaultValue.Text = "\t\t[DefaultValue(\"" + member.Value + "\")]";
                    break;
                case "bool":
                    defaultValue.Text = "\t\t[DefaultValue(" + bool.Parse(member.Value) + ")]";
                    break;
                case "decimal":
                    defaultValue.Text = "\t\t[DefaultValue(" + decimal.Parse(member.Value) + ")]";
                    break;
                default:
                    break;
            }

            return defaultValue;
        }
    }
}