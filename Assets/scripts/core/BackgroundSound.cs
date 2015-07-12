using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class BackgroundSound : MonoBehaviour
    {
        public static BackgroundSound Instance;
        AudioSource source;
        GameObject player;

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            source = GetComponent<AudioSource>();
        }

        public void PlaySound(string sound)
        {
            var clip = Resources.Load<AudioClip>("sound/" + sound);
            source.clip = clip;
            source.Play();
        }

        public void PlayEffectPos(string sound , Vector3 pos){
            var clip = Resources.Load<AudioClip>("sound/" + sound);
            AudioSource.PlayClipAtPoint(clip, pos);
        }
        public void PlayEffect(string sound){
            var clip = Resources.Load<AudioClip>("sound/" + sound);
            source.PlayOneShot(clip);
        }

        void Update()
        {
            if (player == null)
            {
                player = ObjectManager.objectManager.GetMyPlayer();
            }
            if (player != null)
            {
                transform.position = player.transform.position;
            }
        }
    }
}
