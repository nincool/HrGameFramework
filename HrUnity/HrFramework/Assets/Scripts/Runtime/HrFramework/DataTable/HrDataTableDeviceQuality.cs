using Hr.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.DataTable
{
    public class HrDeviceQualityData
    {
        [DataTableMember("ID")]
        public int ID
        {
            get;
            set;
        }

        [DataTableMember("Model")]
        public string Name
        {
            get;
            set;
        }

        [DataTableMember("QualityLevel")]
        public int QualityLevel
        {
            get;
            set;
        }
    }

    public class HrDataTableDeviceQuality : HrDataTable
    {
        public static string DataTableDefaultName
        {
            get
            {
                return "DeviceQuality";
            }
        }

        public Dictionary<int, HrDeviceQualityData> m_dicDeviceQuality = new Dictionary<int, HrDeviceQualityData>();

        public override void ParseDataResource(HrResourceBinary resBinary)
        {
            HrDataTableHelper<HrDeviceQualityData> dataTableHelper = new HrDataTableHelper<HrDeviceQualityData>();
            List<HrDeviceQualityData> lisDeviceQualityData = dataTableHelper.CreateDataInfo(resBinary);
            foreach (var itemDeviceQualityData in lisDeviceQualityData)
            {
                if (m_dicDeviceQuality.ContainsKey(itemDeviceQualityData.ID))
                {
                    throw new HrException("data table DeviceQuality parse data error! same ID!");
                }
                m_dicDeviceQuality.Add(itemDeviceQualityData.ID, itemDeviceQualityData);
            }
            HrLogger.Log("parse datatable DeviceQuality success!");
        }

        public int GetDefalutQualityLevel()
        {
            string strModelName = SystemInfo.deviceModel;
            foreach (var itemValue in m_dicDeviceQuality.Values)
            {
                if (itemValue.Name == strModelName)
                {
                    if (0 <= itemValue.QualityLevel && itemValue.QualityLevel < QualitySettings.names.Length)
                    {
                        return itemValue.QualityLevel;
                    }
                }
            }
            return QualitySettings.names.Length - 1;
        }
    }

}
