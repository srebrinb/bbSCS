using System;
using System.Reflection;

namespace Html5WebSCSTrayApp
{
    internal class RestActionAttribute : Attribute
    {

        private string name;
        private string methods = "POST,GET,OPTION";
        public RestActionAttribute(string name)
        {
            this.name = name;
            this.desc = "";
        }
           
        public RestActionAttribute(string name, string desc)
        {
            this.name = name;
            this.desc = desc;
        }
        public string Methods
        {
            get
            {
                return methods;
            }
            set
            {
                methods = value;
            }
        }
        private bool protect = false;
        

        public bool Protect
        {
            get
            {
                return protect;
            }
            set
            {
                protect = value;
            }
        }
        private string desc="";
        public string Desc
        {
            get
            {
                return desc;
            }
            set
            {
                desc = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public override string ToString()
        {
            string value = "Method : " + Name;
            value += " desc : " + desc;
            return value;
        }
        public static RestActionAttribute getRestActionAttribute(MemberInfo member)
        {
            foreach (object attribute in member.GetCustomAttributes(true))
            {
                if (attribute is RestActionAttribute)
                {
                    return (RestActionAttribute)attribute;
                }
            }
            return null;
        }
        public static bool IsRestActionAttribute(MemberInfo member)
        {
            foreach (object attribute in member.GetCustomAttributes(true))
            {
                if (attribute is RestActionAttribute)
                {
                    return true;
                }
            }
            return false;
        }

    }
}