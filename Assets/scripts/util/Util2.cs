
using UnityEngine;
using System.Collections.Generic;
using Google.ProtocolBuffers;


namespace ChuMeng {
public partial class Util {
	public delegate IMessageLite MsgDelegate(ByteString buf); 
	

	static IMessageLite GetGCGetKeyValue(ByteString buf) {
		var retMsg = ChuMeng.GCGetKeyValue.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCListBranchinges(ByteString buf) {
		var retMsg = ChuMeng.GCListBranchinges.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGLoginAccount(ByteString buf) {
		var retMsg = ChuMeng.CGLoginAccount.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGGetKeyValue(ByteString buf) {
		var retMsg = ChuMeng.CGGetKeyValue.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushFightModeChangeWithMap(ByteString buf) {
		var retMsg = ChuMeng.GCPushFightModeChangeWithMap.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPickUpLootReward(ByteString buf) {
		var retMsg = ChuMeng.GCPickUpLootReward.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCSaveGuideStep(ByteString buf) {
		var retMsg = ChuMeng.GCSaveGuideStep.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGForgotPassword(ByteString buf) {
		var retMsg = ChuMeng.CGForgotPassword.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGPropsRevive(ByteString buf) {
		var retMsg = ChuMeng.CGPropsRevive.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGChangeScreen4Point(ByteString buf) {
		var retMsg = ChuMeng.CGChangeScreen4Point.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGChangeFightMode(ByteString buf) {
		var retMsg = ChuMeng.CGChangeFightMode.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCBindingSession(ByteString buf) {
		var retMsg = ChuMeng.GCBindingSession.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGGetCharacterInfo(ByteString buf) {
		var retMsg = ChuMeng.CGGetCharacterInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushAttribute2Members(ByteString buf) {
		var retMsg = ChuMeng.GCPushAttribute2Members.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGRegisterAccount(ByteString buf) {
		var retMsg = ChuMeng.CGRegisterAccount.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGSetKeyValue(ByteString buf) {
		var retMsg = ChuMeng.CGSetKeyValue.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushPlayerModifyName(ByteString buf) {
		var retMsg = ChuMeng.GCPushPlayerModifyName.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCGetCharacterInfo(ByteString buf) {
		var retMsg = ChuMeng.GCGetCharacterInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushAttrChange(ByteString buf) {
		var retMsg = ChuMeng.GCPushAttrChange.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCSelectCharacter(ByteString buf) {
		var retMsg = ChuMeng.GCSelectCharacter.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGListBranchinges(ByteString buf) {
		var retMsg = ChuMeng.CGListBranchinges.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGAutoRegisterAccount(ByteString buf) {
		var retMsg = ChuMeng.CGAutoRegisterAccount.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGModifyPassword(ByteString buf) {
		var retMsg = ChuMeng.CGModifyPassword.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushNotice2Kick(ByteString buf) {
		var retMsg = ChuMeng.GCPushNotice2Kick.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushLevelUpgrade(ByteString buf) {
		var retMsg = ChuMeng.GCPushLevelUpgrade.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGHeartBeat(ByteString buf) {
		var retMsg = ChuMeng.CGHeartBeat.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGPlayerCmd(ByteString buf) {
		var retMsg = ChuMeng.CGPlayerCmd.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushLootReward(ByteString buf) {
		var retMsg = ChuMeng.GCPushLootReward.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGBindingSession(ByteString buf) {
		var retMsg = ChuMeng.CGBindingSession.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPropsRevive(ByteString buf) {
		var retMsg = ChuMeng.GCPropsRevive.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCChangeFightMode(ByteString buf) {
		var retMsg = ChuMeng.GCChangeFightMode.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGModifyPlayerName(ByteString buf) {
		var retMsg = ChuMeng.CGModifyPlayerName.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGCreateCharacter(ByteString buf) {
		var retMsg = ChuMeng.CGCreateCharacter.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCModifyPassword(ByteString buf) {
		var retMsg = ChuMeng.GCModifyPassword.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushLevel(ByteString buf) {
		var retMsg = ChuMeng.GCPushLevel.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCEnterScene(ByteString buf) {
		var retMsg = ChuMeng.GCEnterScene.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushNotify(ByteString buf) {
		var retMsg = ChuMeng.GCPushNotify.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushLootRewardRemove(ByteString buf) {
		var retMsg = ChuMeng.GCPushLootRewardRemove.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCForgotPassword(ByteString buf) {
		var retMsg = ChuMeng.GCForgotPassword.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGPickItem(ByteString buf) {
		var retMsg = ChuMeng.CGPickItem.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushExpChange(ByteString buf) {
		var retMsg = ChuMeng.GCPushExpChange.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGPlayerMove(ByteString buf) {
		var retMsg = ChuMeng.CGPlayerMove.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushPlayerPower(ByteString buf) {
		var retMsg = ChuMeng.GCPushPlayerPower.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGDelCharacter(ByteString buf) {
		var retMsg = ChuMeng.CGDelCharacter.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGSaveGuideStep(ByteString buf) {
		var retMsg = ChuMeng.CGSaveGuideStep.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCSettingClothShow(ByteString buf) {
		var retMsg = ChuMeng.GCSettingClothShow.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGAddProp(ByteString buf) {
		var retMsg = ChuMeng.CGAddProp.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCRegisterAccount(ByteString buf) {
		var retMsg = ChuMeng.GCRegisterAccount.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCChangeScreen4Point(ByteString buf) {
		var retMsg = ChuMeng.GCChangeScreen4Point.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGEnterScene(ByteString buf) {
		var retMsg = ChuMeng.CGEnterScene.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGSettingClothShow(ByteString buf) {
		var retMsg = ChuMeng.CGSettingClothShow.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCDelCharacter(ByteString buf) {
		var retMsg = ChuMeng.GCDelCharacter.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCAutoRegisterAccount(ByteString buf) {
		var retMsg = ChuMeng.GCAutoRegisterAccount.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushPlayerToilState(ByteString buf) {
		var retMsg = ChuMeng.GCPushPlayerToilState.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCCreateCharacter(ByteString buf) {
		var retMsg = ChuMeng.GCCreateCharacter.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushPlayerResurrect(ByteString buf) {
		var retMsg = ChuMeng.GCPushPlayerResurrect.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPlayerMove(ByteString buf) {
		var retMsg = ChuMeng.GCPlayerMove.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPlayerCmd(ByteString buf) {
		var retMsg = ChuMeng.GCPlayerCmd.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCModifyPlayerName(ByteString buf) {
		var retMsg = ChuMeng.GCModifyPlayerName.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGSelectCharacter(ByteString buf) {
		var retMsg = ChuMeng.CGSelectCharacter.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGPickUpLootReward(ByteString buf) {
		var retMsg = ChuMeng.CGPickUpLootReward.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGSetProp(ByteString buf) {
		var retMsg = ChuMeng.CGSetProp.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushPlayerDressAttributeChanges(ByteString buf) {
		var retMsg = ChuMeng.GCPushPlayerDressAttributeChanges.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCLoginAccount(ByteString buf) {
		var retMsg = ChuMeng.GCLoginAccount.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCCopyInfo(ByteString buf) {
		var retMsg = ChuMeng.GCCopyInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGCopyInfo(ByteString buf) {
		var retMsg = ChuMeng.CGCopyInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCCopyReward(ByteString buf) {
		var retMsg = ChuMeng.GCCopyReward.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushLevelOpen(ByteString buf) {
		var retMsg = ChuMeng.GCPushLevelOpen.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCListUserEquip(ByteString buf) {
		var retMsg = ChuMeng.GCListUserEquip.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCCheckout4Storage(ByteString buf) {
		var retMsg = ChuMeng.GCCheckout4Storage.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCSellUserProps(ByteString buf) {
		var retMsg = ChuMeng.GCSellUserProps.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushGoodsCountChange(ByteString buf) {
		var retMsg = ChuMeng.GCPushGoodsCountChange.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGSellUserProps(ByteString buf) {
		var retMsg = ChuMeng.CGSellUserProps.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushPackInfo(ByteString buf) {
		var retMsg = ChuMeng.GCPushPackInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGAutoAdjustPack(ByteString buf) {
		var retMsg = ChuMeng.CGAutoAdjustPack.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGSplitUserProps(ByteString buf) {
		var retMsg = ChuMeng.CGSplitUserProps.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCLoadShortcutsInfo(ByteString buf) {
		var retMsg = ChuMeng.GCLoadShortcutsInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCUserDressEquip(ByteString buf) {
		var retMsg = ChuMeng.GCUserDressEquip.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGSwapShortcuts(ByteString buf) {
		var retMsg = ChuMeng.CGSwapShortcuts.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCUnbindingGoods(ByteString buf) {
		var retMsg = ChuMeng.GCUnbindingGoods.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGUseUserProps(ByteString buf) {
		var retMsg = ChuMeng.CGUseUserProps.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGUnbindingGoods(ByteString buf) {
		var retMsg = ChuMeng.CGUnbindingGoods.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushEquipDataUpdate(ByteString buf) {
		var retMsg = ChuMeng.GCPushEquipDataUpdate.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushPlayerDressInfo(ByteString buf) {
		var retMsg = ChuMeng.GCPushPlayerDressInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGLoadShortcutsInfo(ByteString buf) {
		var retMsg = ChuMeng.CGLoadShortcutsInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCModifyShortcutsInfo(ByteString buf) {
		var retMsg = ChuMeng.GCModifyShortcutsInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCDressCloth(ByteString buf) {
		var retMsg = ChuMeng.GCDressCloth.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGPut2Storage(ByteString buf) {
		var retMsg = ChuMeng.CGPut2Storage.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGCheckout4Storage(ByteString buf) {
		var retMsg = ChuMeng.CGCheckout4Storage.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCTakeOffCloth(ByteString buf) {
		var retMsg = ChuMeng.GCTakeOffCloth.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGLevelUpEquip(ByteString buf) {
		var retMsg = ChuMeng.CGLevelUpEquip.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCMergeUserProps(ByteString buf) {
		var retMsg = ChuMeng.GCMergeUserProps.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCUseUserProps(ByteString buf) {
		var retMsg = ChuMeng.GCUseUserProps.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGModifyShortcutsInfo(ByteString buf) {
		var retMsg = ChuMeng.CGModifyShortcutsInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGListUserEquip(ByteString buf) {
		var retMsg = ChuMeng.CGListUserEquip.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGLoadPackInfo(ByteString buf) {
		var retMsg = ChuMeng.CGLoadPackInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGDressCloth(ByteString buf) {
		var retMsg = ChuMeng.CGDressCloth.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPut2Storage(ByteString buf) {
		var retMsg = ChuMeng.GCPut2Storage.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCSplitUserProps(ByteString buf) {
		var retMsg = ChuMeng.GCSplitUserProps.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGQueryUserEquipInfo(ByteString buf) {
		var retMsg = ChuMeng.CGQueryUserEquipInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCSwapShortcuts(ByteString buf) {
		var retMsg = ChuMeng.GCSwapShortcuts.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCLoadPackInfo(ByteString buf) {
		var retMsg = ChuMeng.GCLoadPackInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGUserDressEquip(ByteString buf) {
		var retMsg = ChuMeng.CGUserDressEquip.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCAutoAdjustPack(ByteString buf) {
		var retMsg = ChuMeng.GCAutoAdjustPack.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushShortcutsInfo(ByteString buf) {
		var retMsg = ChuMeng.GCPushShortcutsInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushPlayerDressedEquipChange(ByteString buf) {
		var retMsg = ChuMeng.GCPushPlayerDressedEquipChange.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGTakeOffCloth(ByteString buf) {
		var retMsg = ChuMeng.CGTakeOffCloth.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCQueryUserEquipInfo(ByteString buf) {
		var retMsg = ChuMeng.GCQueryUserEquipInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGLevelUpGem(ByteString buf) {
		var retMsg = ChuMeng.CGLevelUpGem.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGMergeUserProps(ByteString buf) {
		var retMsg = ChuMeng.CGMergeUserProps.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGResetSkillPoint(ByteString buf) {
		var retMsg = ChuMeng.CGResetSkillPoint.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGSkillLevelUp(ByteString buf) {
		var retMsg = ChuMeng.CGSkillLevelUp.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGInjectPropsLevelUp(ByteString buf) {
		var retMsg = ChuMeng.CGInjectPropsLevelUp.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGLoadInjectPropsLevelUpInfo(ByteString buf) {
		var retMsg = ChuMeng.CGLoadInjectPropsLevelUpInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushMemberSkillCD(ByteString buf) {
		var retMsg = ChuMeng.GCPushMemberSkillCD.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGLoadSkillPanel(ByteString buf) {
		var retMsg = ChuMeng.CGLoadSkillPanel.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGSkillLevelDown(ByteString buf) {
		var retMsg = ChuMeng.CGSkillLevelDown.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCInjectPropsLevelUp(ByteString buf) {
		var retMsg = ChuMeng.GCInjectPropsLevelUp.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCLoadSkillPanel(ByteString buf) {
		var retMsg = ChuMeng.GCLoadSkillPanel.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushActivateSkill(ByteString buf) {
		var retMsg = ChuMeng.GCPushActivateSkill.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushUnitAddBuffer(ByteString buf) {
		var retMsg = ChuMeng.GCPushUnitAddBuffer.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCLoadInjectPropsLevelUpInfo(ByteString buf) {
		var retMsg = ChuMeng.GCLoadInjectPropsLevelUpInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCResetSkillPoint(ByteString buf) {
		var retMsg = ChuMeng.GCResetSkillPoint.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushSkillPoint(ByteString buf) {
		var retMsg = ChuMeng.GCPushSkillPoint.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCSkillLevelUp(ByteString buf) {
		var retMsg = ChuMeng.GCSkillLevelUp.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCSkillLevelDown(ByteString buf) {
		var retMsg = ChuMeng.GCSkillLevelDown.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGSendChat(ByteString buf) {
		var retMsg = ChuMeng.CGSendChat.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGLoadMChatShowInfo(ByteString buf) {
		var retMsg = ChuMeng.CGLoadMChatShowInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCViewChatGoods(ByteString buf) {
		var retMsg = ChuMeng.GCViewChatGoods.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCLoadMChatShowInfo(ByteString buf) {
		var retMsg = ChuMeng.GCLoadMChatShowInfo.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushChat2Client(ByteString buf) {
		var retMsg = ChuMeng.GCPushChat2Client.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCSendChat(ByteString buf) {
		var retMsg = ChuMeng.GCSendChat.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushSendNotice(ByteString buf) {
		var retMsg = ChuMeng.GCPushSendNotice.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetGCPushNotice(ByteString buf) {
		var retMsg = ChuMeng.GCPushNotice.ParseFrom(buf);
		return retMsg;
	}	

	static IMessageLite GetCGViewChatGoods(ByteString buf) {
		var retMsg = ChuMeng.CGViewChatGoods.ParseFrom(buf);
		return retMsg;
	}	


	static Dictionary<string, MsgDelegate> msgMap = new Dictionary<string, MsgDelegate>(){

	{"GCGetKeyValue", GetGCGetKeyValue},

	{"GCListBranchinges", GetGCListBranchinges},

	{"CGLoginAccount", GetCGLoginAccount},

	{"CGGetKeyValue", GetCGGetKeyValue},

	{"GCPushFightModeChangeWithMap", GetGCPushFightModeChangeWithMap},

	{"GCPickUpLootReward", GetGCPickUpLootReward},

	{"GCSaveGuideStep", GetGCSaveGuideStep},

	{"CGForgotPassword", GetCGForgotPassword},

	{"CGPropsRevive", GetCGPropsRevive},

	{"CGChangeScreen4Point", GetCGChangeScreen4Point},

	{"CGChangeFightMode", GetCGChangeFightMode},

	{"GCBindingSession", GetGCBindingSession},

	{"CGGetCharacterInfo", GetCGGetCharacterInfo},

	{"GCPushAttribute2Members", GetGCPushAttribute2Members},

	{"CGRegisterAccount", GetCGRegisterAccount},

	{"CGSetKeyValue", GetCGSetKeyValue},

	{"GCPushPlayerModifyName", GetGCPushPlayerModifyName},

	{"GCGetCharacterInfo", GetGCGetCharacterInfo},

	{"GCPushAttrChange", GetGCPushAttrChange},

	{"GCSelectCharacter", GetGCSelectCharacter},

	{"CGListBranchinges", GetCGListBranchinges},

	{"CGAutoRegisterAccount", GetCGAutoRegisterAccount},

	{"CGModifyPassword", GetCGModifyPassword},

	{"GCPushNotice2Kick", GetGCPushNotice2Kick},

	{"GCPushLevelUpgrade", GetGCPushLevelUpgrade},

	{"CGHeartBeat", GetCGHeartBeat},

	{"CGPlayerCmd", GetCGPlayerCmd},

	{"GCPushLootReward", GetGCPushLootReward},

	{"CGBindingSession", GetCGBindingSession},

	{"GCPropsRevive", GetGCPropsRevive},

	{"GCChangeFightMode", GetGCChangeFightMode},

	{"CGModifyPlayerName", GetCGModifyPlayerName},

	{"CGCreateCharacter", GetCGCreateCharacter},

	{"GCModifyPassword", GetGCModifyPassword},

	{"GCPushLevel", GetGCPushLevel},

	{"GCEnterScene", GetGCEnterScene},

	{"GCPushNotify", GetGCPushNotify},

	{"GCPushLootRewardRemove", GetGCPushLootRewardRemove},

	{"GCForgotPassword", GetGCForgotPassword},

	{"CGPickItem", GetCGPickItem},

	{"GCPushExpChange", GetGCPushExpChange},

	{"CGPlayerMove", GetCGPlayerMove},

	{"GCPushPlayerPower", GetGCPushPlayerPower},

	{"CGDelCharacter", GetCGDelCharacter},

	{"CGSaveGuideStep", GetCGSaveGuideStep},

	{"GCSettingClothShow", GetGCSettingClothShow},

	{"CGAddProp", GetCGAddProp},

	{"GCRegisterAccount", GetGCRegisterAccount},

	{"GCChangeScreen4Point", GetGCChangeScreen4Point},

	{"CGEnterScene", GetCGEnterScene},

	{"CGSettingClothShow", GetCGSettingClothShow},

	{"GCDelCharacter", GetGCDelCharacter},

	{"GCAutoRegisterAccount", GetGCAutoRegisterAccount},

	{"GCPushPlayerToilState", GetGCPushPlayerToilState},

	{"GCCreateCharacter", GetGCCreateCharacter},

	{"GCPushPlayerResurrect", GetGCPushPlayerResurrect},

	{"GCPlayerMove", GetGCPlayerMove},

	{"GCPlayerCmd", GetGCPlayerCmd},

	{"GCModifyPlayerName", GetGCModifyPlayerName},

	{"CGSelectCharacter", GetCGSelectCharacter},

	{"CGPickUpLootReward", GetCGPickUpLootReward},

	{"CGSetProp", GetCGSetProp},

	{"GCPushPlayerDressAttributeChanges", GetGCPushPlayerDressAttributeChanges},

	{"GCLoginAccount", GetGCLoginAccount},

	{"GCCopyInfo", GetGCCopyInfo},

	{"CGCopyInfo", GetCGCopyInfo},

	{"GCCopyReward", GetGCCopyReward},

	{"GCPushLevelOpen", GetGCPushLevelOpen},

	{"GCListUserEquip", GetGCListUserEquip},

	{"GCCheckout4Storage", GetGCCheckout4Storage},

	{"GCSellUserProps", GetGCSellUserProps},

	{"GCPushGoodsCountChange", GetGCPushGoodsCountChange},

	{"CGSellUserProps", GetCGSellUserProps},

	{"GCPushPackInfo", GetGCPushPackInfo},

	{"CGAutoAdjustPack", GetCGAutoAdjustPack},

	{"CGSplitUserProps", GetCGSplitUserProps},

	{"GCLoadShortcutsInfo", GetGCLoadShortcutsInfo},

	{"GCUserDressEquip", GetGCUserDressEquip},

	{"CGSwapShortcuts", GetCGSwapShortcuts},

	{"GCUnbindingGoods", GetGCUnbindingGoods},

	{"CGUseUserProps", GetCGUseUserProps},

	{"CGUnbindingGoods", GetCGUnbindingGoods},

	{"GCPushEquipDataUpdate", GetGCPushEquipDataUpdate},

	{"GCPushPlayerDressInfo", GetGCPushPlayerDressInfo},

	{"CGLoadShortcutsInfo", GetCGLoadShortcutsInfo},

	{"GCModifyShortcutsInfo", GetGCModifyShortcutsInfo},

	{"GCDressCloth", GetGCDressCloth},

	{"CGPut2Storage", GetCGPut2Storage},

	{"CGCheckout4Storage", GetCGCheckout4Storage},

	{"GCTakeOffCloth", GetGCTakeOffCloth},

	{"CGLevelUpEquip", GetCGLevelUpEquip},

	{"GCMergeUserProps", GetGCMergeUserProps},

	{"GCUseUserProps", GetGCUseUserProps},

	{"CGModifyShortcutsInfo", GetCGModifyShortcutsInfo},

	{"CGListUserEquip", GetCGListUserEquip},

	{"CGLoadPackInfo", GetCGLoadPackInfo},

	{"CGDressCloth", GetCGDressCloth},

	{"GCPut2Storage", GetGCPut2Storage},

	{"GCSplitUserProps", GetGCSplitUserProps},

	{"CGQueryUserEquipInfo", GetCGQueryUserEquipInfo},

	{"GCSwapShortcuts", GetGCSwapShortcuts},

	{"GCLoadPackInfo", GetGCLoadPackInfo},

	{"CGUserDressEquip", GetCGUserDressEquip},

	{"GCAutoAdjustPack", GetGCAutoAdjustPack},

	{"GCPushShortcutsInfo", GetGCPushShortcutsInfo},

	{"GCPushPlayerDressedEquipChange", GetGCPushPlayerDressedEquipChange},

	{"CGTakeOffCloth", GetCGTakeOffCloth},

	{"GCQueryUserEquipInfo", GetGCQueryUserEquipInfo},

	{"CGLevelUpGem", GetCGLevelUpGem},

	{"CGMergeUserProps", GetCGMergeUserProps},

	{"CGResetSkillPoint", GetCGResetSkillPoint},

	{"CGSkillLevelUp", GetCGSkillLevelUp},

	{"CGInjectPropsLevelUp", GetCGInjectPropsLevelUp},

	{"CGLoadInjectPropsLevelUpInfo", GetCGLoadInjectPropsLevelUpInfo},

	{"GCPushMemberSkillCD", GetGCPushMemberSkillCD},

	{"CGLoadSkillPanel", GetCGLoadSkillPanel},

	{"CGSkillLevelDown", GetCGSkillLevelDown},

	{"GCInjectPropsLevelUp", GetGCInjectPropsLevelUp},

	{"GCLoadSkillPanel", GetGCLoadSkillPanel},

	{"GCPushActivateSkill", GetGCPushActivateSkill},

	{"GCPushUnitAddBuffer", GetGCPushUnitAddBuffer},

	{"GCLoadInjectPropsLevelUpInfo", GetGCLoadInjectPropsLevelUpInfo},

	{"GCResetSkillPoint", GetGCResetSkillPoint},

	{"GCPushSkillPoint", GetGCPushSkillPoint},

	{"GCSkillLevelUp", GetGCSkillLevelUp},

	{"GCSkillLevelDown", GetGCSkillLevelDown},

	{"CGSendChat", GetCGSendChat},

	{"CGLoadMChatShowInfo", GetCGLoadMChatShowInfo},

	{"GCViewChatGoods", GetGCViewChatGoods},

	{"GCLoadMChatShowInfo", GetGCLoadMChatShowInfo},

	{"GCPushChat2Client", GetGCPushChat2Client},

	{"GCSendChat", GetGCSendChat},

	{"GCPushSendNotice", GetGCPushSendNotice},

	{"GCPushNotice", GetGCPushNotice},

	{"CGViewChatGoods", GetCGViewChatGoods},

	};

	public static IMessageLite GetMsg(int moduleId, int messageId, ByteString buf) {
		var module = SaveGame.saveGame.getModuleName(moduleId);
		var msg = SaveGame.saveGame.getMethodName(module, messageId);
		Debug.LogWarning ("modulename "+module+" "+msg);

		return msgMap[msg](buf);
	}
}

}
