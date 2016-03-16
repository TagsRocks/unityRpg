using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class WaterEnvLoader : MonoBehaviour
    {
        void Awake()
        {
            Log.Sys("WaterEnvLoader");
        }

        void Start()
        {
            LoadWater();
        }

        void LoadWater()
        {
            var sceneId = WorldManager.worldManager.GetActive().def.id;
            LevelConfigData.Init();
            var configLists = LevelConfigData.LevelLayout [sceneId];
            var first = configLists [0];

            Log.Sys("LoadWater " + first.type);
            if (LevelConfigData.envConfig.ContainsKey(first.type))
            {
                Log.Sys("LoadWater " + first.type);
                var d = LevelConfigData.envConfig [first.type];
                if (!string.IsNullOrEmpty(d.waterBottom))
                {
                    var bottom = Resources.Load<GameObject>(d.waterBottom);
                    var b = GameObject.Instantiate(bottom) as GameObject;
                    Log.Sys("WaterObj " + b);
                }

                if (!string.IsNullOrEmpty(d.waterFace))
                {
                    var bottom2 = Resources.Load<GameObject>(d.waterFace);
                    var b2 = GameObject.Instantiate(bottom2) as GameObject;
                    b2.transform.localPosition = new Vector3(0, d.offY, 0);
                }
                if (!string.IsNullOrEmpty(d.skyBox))
                {
                    var skybox = Resources.Load<GameObject>(d.skyBox);
                    var s2 = GameObject.Instantiate(skybox) as GameObject;
                    s2.transform.localPosition = Vector3.zero;
                }
            }
        }

    }

}