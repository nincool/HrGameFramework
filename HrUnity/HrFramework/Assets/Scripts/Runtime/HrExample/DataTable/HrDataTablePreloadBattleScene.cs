using Hr.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.DataTable
{
    public class HrPreloadBattleSceneData
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


    public class HrDataTablePreloadBattleScene : HrDataTable
    {
        public static string DataTableDefaultName
        {
            get
            {
                return "PreloadBattleScene";
            }
        }
        private Dictionary<int, HrPreloadBattleSceneData> m_dicPreloadBattleScene = new Dictionary<int, HrPreloadBattleSceneData>();
        public Dictionary<int, HrPreloadBattleSceneData> PreloadBattleSceneInfo
        {
            get
            {
                return m_dicPreloadBattleScene;
            }
        }

        public override void ParseDataResource(HrResourceBinary resBinary)
        {
            HrDataTableHelper<HrPreloadBattleSceneData> dataTableHelper = new HrDataTableHelper<HrPreloadBattleSceneData>();
            List<HrPreloadBattleSceneData> lisData = dataTableHelper.CreateDataInfo(resBinary);
            foreach (var itemData in lisData)
            {
                if (m_dicPreloadBattleScene.ContainsKey(itemData.ID))
                {
                    throw new HrException("data table HrPreloadBattleSceneData parse data error! same ID!");
                }
                m_dicPreloadBattleScene.Add(itemData.ID, itemData);
            }
            HrLogger.Log("parse datatable HrPreloadBattleSceneData success!");
        }
    }

}
