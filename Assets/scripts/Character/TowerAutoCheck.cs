using UnityEngine;
using System.Collections;

namespace MyLib
{
    public class TowerAutoCheck : MonoBehaviour
    {
        void Start() {
            StartCoroutine(AutoEnemy());
        }

        IEnumerator AutoEnemy() {
            yield break;
            
        }
    }
}
