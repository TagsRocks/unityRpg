using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class WaterEnvLoader : MonoBehaviour
    {
        void Start() {
            LoadWater();
        }

        void LoadWater()
        {
            var sceneId = WorldManager.worldManager.GetActive().def.id;
            LevelConfigData.Init();
            var configLists = LevelConfigData.LevelLayout [sceneId];
            var first = configLists [0];

            if (LevelConfigData.envConfig.ContainsKey(first.type))
            {
                var d = LevelConfigData.envConfig[first.type];
                var bottom = Resources.Load<GameObject>(d.waterBottom);
                var b = GameObject.Instantiate(bottom) as GameObject;
                Util.InitGameObject(b);

                var bottom2 = Resources.Load<GameObject>(d.waterFace);
                var b2 = GameObject.Instantiate(bottom) as GameObject;
                Util.InitGameObject(b2);
            }
        }

    }

}