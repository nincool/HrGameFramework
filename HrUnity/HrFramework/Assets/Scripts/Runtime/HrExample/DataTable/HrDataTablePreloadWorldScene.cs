using Hr.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.DataTable
{
    public class HrPreloadWorldSceneData
    {
        [DataTableMember("ID")]
        public int ID
        {
            get;
            set;
        }

        [DataTableMember("ResourceID")]
        public int ResourceID
        {
            get;
            set;
        }

        [DataTableMember("ResourceName")]
        public string ResourceName
        {
            get;
            set;
        }
    }

    public class HrDataTablePreloadWorldScene : HrDataTable
    {
        public static string DataTableDefaultName
        {
            get
            {
                return "PreloadWorldScene";
            }
        }
        private Dictionary<int, HrPreloadWorldSceneData> m_dicPreloadWorldScene = new Dictionary<int, HrPreloadWorldSceneData>();
        public Dictionary<int, HrPreloadWorldSceneData> PreloadWorldSceneInfo
        {
            get
            {
                return m_dicPreloadWorldScene;
            }
        }

        public override void ParseDataResource(HrResourceBinary resBinary)
        {
            HrDataTableHelper<HrPreloadWorldSceneData> dataTableHelper = new HrDataTableHelper<HrPreloadWorldSceneData>();
            List<HrPreloadWorldSceneData> lisData = dataTableHelper.CreateDataInfo(resBinary);
            foreach (var itemData in lisData)
            {
                if (m_dicPreloadWorldScene.ContainsKey(itemData.ID))
                {
                    throw new HrException("data table PreloadWorldSceneData parse data error! same ID!");
                }
                m_dicPreloadWorldScene.Add(itemData.ID, itemData);
            }
            HrLogger.Log("parse datatable PreloadWorldSceneData success!");
        }
    }

}
