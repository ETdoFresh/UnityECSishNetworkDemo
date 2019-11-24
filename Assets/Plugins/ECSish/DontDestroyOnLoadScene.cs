using UnityEngine;
using UnityEngine.SceneManagement;

namespace ECSish
{
    public class DontDestroyOnLoadScene : MonoBehaviour
    {
        public static DontDestroyOnLoadScene singleton;
        public Scene scene;

        public static Scene Scene => singleton ? singleton.scene : default;

        private void Awake()
        {
            singleton = this;
            GameObject temp = null;
            try
            {
                temp = new GameObject();
                Object.DontDestroyOnLoad(temp);
                scene = temp.scene;
                Object.DestroyImmediate(temp);
                temp = null;
            }
            finally
            {
                if (temp != null)
                    Object.DestroyImmediate(temp);
            }
        }
    }
}
