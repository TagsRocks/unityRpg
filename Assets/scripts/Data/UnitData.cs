
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
/*
 * 配置的数据均为百分比数据  需要从相关的曲线数据中获取时机的数据
 * 例如HP = 100 表示100% 的生命值， 根据Level 在 Health_monster 表格中查到对应Level怪兽的数据 接着按照百分比调整即可 
 */
    public class UnitData
    {
        MonsterFightConfigData config = null;
        RoleUpgradeConfigData playerConfig = null;
        RoleJobDescriptionsData jobConfig = null;

        public int ID
        {
            get
            {
                if (IsPlayer)
                {
                    return jobConfig.id;
                } else
                {
                    return config.id;
                }
            }
        }

        public float MoveSpeed
        {
            get
            {
                if (IsPlayer)
                {
                    return jobConfig.moveSpeed / 10.0f;
                } else
                {
                    return config.moveSpeed / 10.0f;
                }
            }
        }

        public int Level
        {
            get
            {
                return playerConfig.level;
            }
        }

        public string SpawnEffect
        {
            get
            {
                return config.spawnParticle;
            }
        }

        SimpleJSON.JSONArray skList = null;

        public SimpleJSON.JSONArray GetSkillList()
        {
            if (skList == null)
            {
                Log.AI("MonsterConfig " + config.name + " " + config.skillList);
                var p = SimpleJSON.JSON.Parse(config.skillList);
                if (p != null)
                {
                    skList = p.AsArray;
                    int min = 0;
                    foreach (SimpleJSON.JSONNode j in skList)
                    {
                        j ["min"].AsInt = min;
                        j ["max"].AsInt = min + j ["chance"].AsInt;
                        min += j ["chance"].AsInt;
                    }
                } else
                {
                    skList = new SimpleJSON.JSONArray();
                }
            }
            return skList;
        }

        public bool AttachToMaster
        {
            get
            {
                return config.attachToMaster;
            }
        }

        public string AITemplate
        {
            get
            {
                return config.LogicTemplate;
            }
        }
        public class Treasure
        {
            public ItemData itemData;
            public int min;
            public int max;
            public int Weight = 0;
        }

        public string name
        {
            get
            {
                if (IsPlayer)
                {
                    return jobConfig.job;
                } else
                {
                    return config.name;
                }
            }
        }

        //人物升级需要经验
        public long MaxExp
        {
            get
            {
                return playerConfig.exp; 
            }
        }

        //怪物掉落经验
        public int Exp
        {
            get
            {
                return config.exp;
            }
        }

        public string ModelName
        {
            get
            {
                if (IsPlayer)
                {
                    return jobConfig.ModelName;
                } else
                {
                    return config.model;
                }
            }
        }
        ///<summary>
        /// 人物自身属性都是静态的，装备只提供加成不会影响静态的基础
        /// </summary> 
        public int HP
        {
            get
            {
                if (IsPlayer)
                {
                    return playerConfig.maxHp;
                } else
                {
                    return config.hp;
                }
            }
        }

        public int MP
        {
            get
            {
                if (IsPlayer)
                {
                    return playerConfig.maxMp;
                } else
                {
                    //throw new System.Exception();
                    return 0;
                }
            }
        }

        public int Damage
        {
            get
            {
                if (IsPlayer)
                {
                    return playerConfig.attack;
                } else
                {
                    return config.attack;
                }
            }
        }

        //怪物给的经验值
        public int XP
        {
            get
            {
                if (IsPlayer)
                {
                    throw new System.NotImplementedException();
                }

                return config.exp;
            }
        }

        public int Armor
        {
            get
            {
                if (IsPlayer)
                {
                    return playerConfig.defense;
                } else
                {
                    return config.defense;
                }
            }
        }

        //TODO:怪物死亡后掉落物品 机制
        public List<Treasure> TreasureData
        {
            get
            {
                return new List<Treasure>();
            }
        }

        bool IsPlayer = false;

        public bool GetIsPlayer()
        {
            return IsPlayer;
        }

        public int CriticalHit
        {
            get
            {
                if (IsPlayer)
                {
                    return jobConfig.criticalHit;
                }
                return config.criticalHit;
            }
        }
        /*
         * 碰撞体配置在模型上面
         */

        public string TextureReplace
        {
            get
            {
                if (!IsPlayer)
                {
                    return config.textureReplace;
                } else
                {
                    return "";
                }
            }
        }

        //TODO:增加怪物技能的配置信息
        public List<ChuMeng.SkillData> Skills
        {
            get
            {
                return null;
            }
        }

        public int ApproachDistance
        {
            get
            {
                return config.warnRange;
            }
        }

        public int AttackRange
        {
            get
            {
                if (IsPlayer)
                {
                    return jobConfig.attackRange;
                } else
                {
                    return config.attackRange;
                }
            }
        }

        //默认攻击技能
        //远程装备上装备之后 装备技能
        //还是职业属性技能
        public int baseSkill
        {
            get
            {
                if (IsPlayer)
                {
                    return jobConfig.baseSkill;
                } else
                {
                    return config.baseSkill;
                }
            }
        }

        //远程技能的子弹 配置到技能里面

        public float WalkAniSpeed
        {
            get
            {
                return 1;
            }
        }

        public float AttackAniSpeed
        {
            get
            {
                return jobConfig.AttackAniSpeed / 1000.0f;
            }
        }

        /*
         * Flee On My Friend Dead
         */ 
        public bool Flee
        {
            get
            {
                if (IsPlayer)
                {
                    return false;
                }
                return config.flee;
            }
        }

        public Job job
        {
            get
            {
                return (Job)jobConfig.id;
            }
        }

        //TODO:获取掉落物品信息   单人副本获取掉落物品
        public List<float>  GetRandomDrop()
        {
            var dropList = config.drop;
            var drop = Util.ParseConfig(dropList);
            var rd = Random.Range(0, 1.0f);
            var lastRd = 0.0f;
            foreach (var d in drop)
            {
                Log.Sys("random "+rd+" last "+lastRd+" d "+d[1]);

                var nextRd = lastRd+d[1];
                if(rd < nextRd){
                    return d;
                }
                lastRd = nextRd;
            }
            return null;
        }

        public string GetDefaultWardrobe()
        {
            return jobConfig.DefaultWardrobe;
        }

        public UnitData(bool isPlayer, int mid, int level)
        {
            IsPlayer = isPlayer;
            Log.Important("Init Unit Data is " + isPlayer + " " + mid + " " + level);
            if (!isPlayer)
            {
                config = GMDataBaseSystem.SearchIdStatic<MonsterFightConfigData>(GameData.MonsterFightConfig, mid);

            } else
            {
                jobConfig = GMDataBaseSystem.SearchIdStatic<RoleJobDescriptionsData>(GameData.RoleJobDescriptions, mid);
                foreach (RoleUpgradeConfigData r in GameData.RoleUpgradeConfig)
                {
                    if (r.job == mid && r.level == level)
                    {
                        playerConfig = r;
                        break;
                    }
                }
                Log.Important("jobConfig " + jobConfig + " playerConfig " + playerConfig);
            }
        }

    }

}