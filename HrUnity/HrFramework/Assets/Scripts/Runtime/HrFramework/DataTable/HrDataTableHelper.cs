using Hr.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hr.DataTable
{
    public class HrDataTableHelper<T> where T : new()
    {
        private Dictionary<string, PropertyInfo> m_dicProperty = new Dictionary<string, PropertyInfo>();
        private Dictionary<string, DataTableMemberAttribute> m_dicByteMember = new Dictionary<string, DataTableMemberAttribute>();

        public HrDataTableHelper()
        {
            GetProperty();
        }

        private void GetProperty()
        {
            Type type = typeof(T);
            PropertyInfo[] memeberInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var memInfo in memeberInfo)
            {
                DataTableMemberAttribute byteMember = Attribute.GetCustomAttribute(memInfo, typeof(DataTableMemberAttribute)) as DataTableMemberAttribute;
                if (byteMember != null)
                {
                    string strName = byteMember.Name;
                    if (!m_dicProperty.ContainsKey(strName))
                    {
                        m_dicProperty.Add(strName, memInfo);
                        m_dicByteMember.Add(strName, byteMember);
                    }
                }
            }
        }

        public List<T> CreateDataInfo(HrResourceBinary resBinary)
        {
            if (resBinary.DataRowCount <= 0)
            {
                return new List<T>();
            }

            List<T> lisDataInfo = CreateDefaultLengthList(resBinary.DataRowCount);

            for (var i = 0; i < resBinary.HeadNames.Count; ++i)
            {
                string strHeadName = resBinary.HeadNames[i];
                string strStringType = resBinary.StringTypes[i];

                var propertyInfo = m_dicProperty.HrTryGet(strHeadName);
                if (propertyInfo == null)
                {
                    HrLogger.LogError(string.Format("create data info error! res '{0}' head '{1}'", resBinary.AssetName, strHeadName));
                    continue;
                }
                DataTableMemberAttribute dataTableMember = m_dicByteMember.HrTryGet(strHeadName);
                if (dataTableMember == null)
                {
                    HrLogger.LogError(string.Format("create data info error! can not find the DataTableMemberAttribute res '{0}' head '{1}'", resBinary.AssetName, strHeadName));
                    continue;
                }
                
                for (var nRowIndex = 0; nRowIndex < resBinary.DataRowCount; ++nRowIndex)
                {
                    T dataTable = lisDataInfo[nRowIndex];
                    var valueObj = resBinary.GetDataValue(nRowIndex, i);
                    try
                    {
                        propertyInfo.SetValue(dataTable, valueObj, null);
                    }
                    catch (Exception e)
                    {
                        HrLogger.LogError(e.Message);
                    }
                }
            }

            return lisDataInfo;
        }

        private List<T> CreateDefaultLengthList(int nLength)
        {
            List<T> lisData = new List<T>();
            for (var i = 0; i < nLength; ++i)
            {
                lisData.Add(new T());
            }
            return lisData;
        }
    }
}
