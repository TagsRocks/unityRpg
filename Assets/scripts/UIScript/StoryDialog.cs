﻿using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class StoryDialog : IUserInterface
    {
        string[] text;
        UILabel label;
        GameObject button;
        void Awake(){
            label = GetLabel("Label");
            button = GetName("Next");
            SetCallback("Next", OnNext);
            text = new string[]{
                @"炎历435年，华夏大陆上，神月湖畔，一对夫妻在夕阳下相依着。
一个孩童独自在林中玩耍。
黑夜中，不知从哪里射出两支神箭，月夜下，泛着悠悠蓝光。
这突然之物，瞬间让夫妻俩惊异非常，两人相视片刻，急忙向远山处奔去。",
                @"孩童也被这锐利的箭声所惊动，急忙向湖边父母那里跑去。
那里却没有了父母的身影。
月夜里，孩童呜呜的啜泣。",
                @"漫长的夜，终将过去。
当第一缕阳光照射在湖面上时，强烈的光刺射在孩子的脸上。
泪痕清晰可见，昨夜不知何时，沉沉睡去，始终不见父母的身影。
一个人，慢慢的走向家的方向。",
            };
        }
        bool onNext = false;
        void OnNext(GameObject g){
            onNext = true;
            BackgroundSound.Instance.PlayEffect("sheet_opencenter");
        }

        // Use this for initialization
        void Start()
        {
            StartCoroutine(ShowContent());
        }
        IEnumerator ShowContent(){
            int curId = 0;
            while(curId < text.Length){
                yield return StartCoroutine(ShowText(curId));
                yield return StartCoroutine(WaitForNext());
                curId++;
            }
            EnterGame();
        }
        void EnterGame(){
            var setValue = CGSetKeyValue.CreateBuilder();
            var kv = KeyValue.CreateBuilder();
            kv.Key = GameBool.FINISH_NEW;
            kv.Value = "true";
            setValue.Kv = kv.Build();
            KBEngine.Bundle.sendImmediate(setValue);
            WorldManager.worldManager.WorldChangeScene(2, false);
        }

        IEnumerator ShowText(int curId){
            button.SetActive(false);
            var t = text[curId];
            for(int c = 0; c<= t.Length; c++){
                label.text = t.Substring(0, c);
                yield return new WaitForSeconds(0.1f);
                BackgroundSound.Instance.PlayEffect("pickup");
            }
            button.SetActive(true);
        }
    
        IEnumerator WaitForNext(){
            while(!onNext){
                yield return new WaitForSeconds(0.1f);
            }

            onNext = false;
        }
        // Update is called once per frame
        void Update()
        {
    
        }
    }

}