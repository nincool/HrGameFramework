using System.Collections;
using System.Collections.Generic;

namespace Hr.EventSystem
{
    public partial class HrEventType
    {
        public const int EVENT_LOAD_SCENE_RESOURCE_SUCCESS = 1;    //加载SceneAssetBundle成功

        public const int EVENT_SCENE_UNLOAD_SCENE = 10;            //切换场景
        public const int EVENT_SCENE_LOADED_SCENE = 11;            //载入场景

        public static readonly int EVENT_UI_SHOW = 100;
        public static readonly int EVENT_UI_HIDE = 101;
    }
}
