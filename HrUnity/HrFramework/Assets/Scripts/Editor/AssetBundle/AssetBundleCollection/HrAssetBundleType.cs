using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Editor
{
    /// <summary>
    /// 资源包类型。
    /// </summary>
    public enum HrAssetBundleType
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 存放资源的资源包。
        /// </summary>
        Asset,

        /// <summary>
        /// 存放场景的资源包。
        /// </summary>
        Scene,
    }
}