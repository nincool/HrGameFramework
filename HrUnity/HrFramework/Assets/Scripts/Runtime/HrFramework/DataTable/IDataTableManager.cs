using Hr.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.DataTable
{
    public interface IDataTableManager
    {
        /// <summary>
        /// 设置加载资源管理器 用以加载Data
        /// </summary>
        /// <param name="resourceManager"></param>
        void SetResourceManager(IResourceManager resourceManager);
    }
}
