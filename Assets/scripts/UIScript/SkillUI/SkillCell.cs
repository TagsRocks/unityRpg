using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class SkillCell : IUserInterface 
    {
        UILabel nameLabel;
        UILabel learnOrLevel;
        GameObject buy;
        void Awake() {
            nameLabel = GetLabel("Name");
            learnOrLevel = GetLabel("Label");
            buy = GetName("Buy");
            //SetCallback("Buy", OnLearn);    
        }

        public void SetSkillName(string n, int lev, int needLev, string desc, int maxLev){
            if(lev <= 0){
                nameLabel.text = string.Format("[959595]{0}[-]\n[959595]需要等级:{1}[-]\n[959595]{2}[-]", n, needLev, desc);
                SetLearn(0);
            }else if(lev < maxLev) {
                SetLearn(1);
                nameLabel.text = string.Format("[ff9500]{0}[-]\n[0098fc]等级：{1}[-]\n[73d216]下一级需要：{2}[-]\n[f85818]{3}[-]", n, lev, needLev, desc);
            }else {
                SetLearn(2);
                nameLabel.text = string.Format("[ff9500]{0}[-]\n[0098fc]等级：{1}满级[-]\n[f85818]{2}[-]", n, lev, desc);
            }
        }

        void SetLearn(int b){
            buy.SetActive(true);
            if(b == 0) {
                learnOrLevel.text = "学习";
            }else if(b == 1) {
                learnOrLevel.text = "升级";
            }else {
                //learnOrLevel.text = "最高级";
                buy.SetActive(false);
            }
        }
                    
       
        public void SetCb(EmptyDelegate cb ){
            SetCallback("Buy", delegate(GameObject g) {
                cb();
           });
        }
    }

}