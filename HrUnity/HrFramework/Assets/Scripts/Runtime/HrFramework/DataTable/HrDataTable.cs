using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.Text.RegularExpressions;
#endif

namespace Hr.DataTable
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    public class DataTableMemberAttribute : Attribute
    {
        public string Name
        {
            get;
            private set;
        }

        public DataTableMemberAttribute(string str)
        {
            Name = str;
#if UNITY_EDITOR
            const string strMathNamePattern = @"^[a-z]+$";
            if (!Regex.IsMatch(str, strMathNamePattern))
            {
                Debug.LogError("DataTableMemberAttribute name error! name:" + str);
            }
#endif
        }
    }

    public class HrDataTable
    {


    }

}
