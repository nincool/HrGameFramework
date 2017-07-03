using Hr.Resource;
using Hr.Utility;
using System;
using System.Collections.Generic;

namespace Hr.DataTable
{
    public class HrDataTableManager : HrModule, IDataTableManager
    {
        private IResourceManager m_resourceManager;

        private HrLoadResourceCallBack m_loadDataTableCallBack;

        private Dictionary<string, HrDataTable> m_dicDataTables = new Dictionary<string, HrDataTable>();

        public void SetResourceManager(IResourceManager resourceManager)
        {
            m_resourceManager = resourceManager;
            m_loadDataTableCallBack = new HrLoadResourceCallBack(LoadDataTableSuccess, LoadDataTableFailed);
        }

        public override void Init()
        {
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
        }

        public override void Shutdown()
        {

        }

        public void LoadDataTable(string strDataTable)
        {
            m_resourceManager.LoadDataTable(strDataTable, m_loadDataTableCallBack);
        }

        public HrDataTable GetDataTable(string strDataTable)
        {
            return m_dicDataTables.HrTryGet(strDataTable);
        }

        private void LoadDataTableSuccess(HrResource resDataTable)
        {
            HrLogger.Log(string.Format("procedure preload load datable success! name '{0}'", resDataTable.AssetName));

            ParseDataTableResource(resDataTable);
        }

        private void LoadDataTableFailed(string strDataTableName, string strErrorMsg)
        {
            HrLogger.LogError(string.Format("procedure preload error! msg '{0}'", strErrorMsg));
        }

        private void ParseDataTableResource(HrResource resDataTable)
        {
            HrResourceBinary resBinary = resDataTable as HrResourceBinary;
            if (resBinary == null)
            {
                HrLogger.LogError(string.Format("try to parse datatable error! the resource is no the HrResourceBinary '{0}'!", resDataTable.AssetName));
                return;
            }
            string strName = resBinary.AssetName;
            string strTypeName = string.Format("Hr.DataTable.HrDataTable{0}", strName);

#if UNITY_EDITOR
            Type type = GetDataTableType(strTypeName);
            if (type == null)
            {
                HrLogger.LogError(string.Format("data table try to parse dataresource error! can not find data type '{0}'!", strTypeName));
                return;
            }
#endif

            HrDataTable dataTable = (HrDataTable)Activator.CreateInstance(type);
            if (dataTable == null)
            {
                throw new HrException(string.Format("can not create datatable instance! {0}", type.FullName));
            }
            dataTable.ParseDataResource(resBinary);

            m_dicDataTables.Add(resDataTable.AssetName, dataTable);
        }

        private Type GetDataTableType(string strDataTableClass)
        {
            Type dataTableType = null;

            var strTypeNames = HrType.GetTypeNames(typeof(HrDataTable));
            foreach (var typeName in strTypeNames)
            {
                if (typeName.Contains(strDataTableClass))
                {
                    dataTableType = Type.GetType(typeName);
                    break;
                }
            }

            return dataTableType;
        }


    }
}

