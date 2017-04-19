using System.Collections;
using System.Collections.Generic;

namespace Hr
{
    public class HrGameWorld : HrSingleton<HrGameWorld>
    {
        public HrUIManager UIManager { get; private set; }

        public HrSceneManager SceneManager { get; private set; }

        public HrGameWorld()
        {
            UIManager = new HrUIManager();
            SceneManager = new HrSceneManager();    
        }

        public void Init()
        {
            UIManager.Init();
            SceneManager.Init();
        }

        public void ChangeScene(EnumSceneType sceneType)
        {
            SceneManager.SwitchToScene(sceneType);
        }
    }

}
