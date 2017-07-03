
using UnityEngine;

namespace Hr.Environment
{
    public static class HrEnvironment
    {

        public static bool IsEditorMode
        {
            get
            {
                return Application.isEditor;
            }
        }


    }
}
