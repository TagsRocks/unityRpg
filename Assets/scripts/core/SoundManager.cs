using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        List<AudioSource> allSources = new List<AudioSource>();
        void Awake() {
            Instance = this;
        }
        void Start(){
           // StartCoroutine(RemoveSound());
        }
        /*
        IEnumerator RemoveSound(){
            while(true){
                yield return new WaitForSeconds(1);
                if(allSources.Count )
            }
        }
        */
        public void PlaySound(string name)
        {
            var clip = Resources.Load<AudioClip>("sound/"+name);
            var source = gameObject.AddComponent<AudioSource>();
            allSources.Add(source);
            source.clip = clip;
            source.Play();

        }
    }

}