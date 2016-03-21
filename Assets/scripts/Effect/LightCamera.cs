using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class LightCamera : MonoBehaviour
    {
        public Vector3 CamPos;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        
        }
	
        // Update is called once per frame
        void Update()
        {
            transform.position = CameraController.Instance.transform.position + CamPos;
        }
    }

}