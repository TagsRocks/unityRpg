using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class SyncPosWithTarget : MonoBehaviour
    {
        public GameObject target;
        void Start()
        {
    
        }
    
        // Update is called once per frame
        void Update()
        {
            transform.position = target.transform.position;
        }
    }

}