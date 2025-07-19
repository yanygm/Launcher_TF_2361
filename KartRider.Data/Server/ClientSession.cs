﻿using KartRider.Common.Network;
using KartRider.Common.Utilities;
using KartRider.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KartRider_TrackName;
using ExcData;
using Set_Data;
using RiderData;
using System.Xml;
using System.Linq;

namespace KartRider
{
	public class ClientSession : Session
	{
		public SessionGroup Parent
		{
			get;
			set;
		}

		public ClientSession(SessionGroup parent, System.Net.Sockets.Socket socket) : base(socket)
		{
			this.Parent = parent;
		}

		public override void OnDisconnect()
		{
			this.Parent.Client.Disconnect();
		}

		public int[] DataTime()
		{
			DateTime dt = DateTime.Now;
			DateTime time = new DateTime(1900, 1, 1, 0, 0, 0);
			TimeSpan t = dt.Subtract(time);
			double totalSeconds = dt.TimeOfDay.TotalSeconds / 4;
			int Month = (dt.Year - 1900) * 12;
			int MonthCount = Month + dt.Month;
			double tempResult = (double)MonthCount / 2;
			int oddMonthCount;
			if (tempResult % 1 != 0)
			{
				oddMonthCount = (int)tempResult + 1;
			}
			else
			{
				oddMonthCount = (int)tempResult;
			}
			return new int[] { t.Days, (int)totalSeconds, oddMonthCount };
		}

		public override void OnPacket(InPacket iPacket)
		{
			int ALLnum;
			lock (this.Parent.m_lock)
			{
				iPacket.Position = 0;
				uint hash = iPacket.ReadUInt();
				if (hash == Adler32Helper.GenerateAdler32_ASCII("PqCnAuthenLogin", 0))
				{
					using (OutPacket outPacket = new OutPacket("PrCnAuthenLogin"))
					{
						outPacket.WriteInt(1);
						outPacket.WriteString("pnlcdfngkdjfdhdermnkicqknmqrnjnkrlpdirerjrqkcllhpckngophnrrfclgiojmopomonkjilgmheoldpmmcdokgdqljqcnkrplffhflqdnchherghnhoihgfnon");
						outPacket.WriteByte(0);
						outPacket.WriteString("https://www.tiancity.com/agreement");
						this.Parent.Client.Send(outPacket);
					}
					return;
				}
				if ((hash == Adler32Helper.GenerateAdler32(Encoding.ASCII.GetBytes("PcReportRaidOccur"), 0) ? false : hash != 1340475309))//PqGameReportMyBadUdp
				{
					if (hash == Adler32Helper.GenerateAdler32_ASCII("GrRiderTalkPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqEnterMagicHatPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("LoPingRequestPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqGetRiderQuestUX2ndData", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqAddTimeEventInitPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqCountdownBoxPeriodPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqServerSideUdpBindCheck", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqVipGradeCheck", 0))
					{
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("LoRqUpdateRiderSchoolDataPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("RmRiderTalkPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqNeedTimerGiftEvent", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PcReportStateInGame", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("GameBoosterAddPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("LoRqCheckReplayItemPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqGetRecommandChatServerInfo", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("LoCheckLoginEvent", 0))
					{
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqBlockWordLogPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqWriteActionLogPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqMissionAttendPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqAddTimeEventTimerPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqTimeShopOpenTimePacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqItemPresetSlotDataList", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("VipPlaytimeCheck", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("LoRqEventRewardPacket", 0))
					{
						//PqGetRecommandChatServerInfo = 라이더 챗
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("LoRqAddRacingTimePacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("LoRpAddRacingTimePacket"))
						{
							outPacket.WriteHexString("FF FF FF FF 00 00 00 00 00 00 00 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("LoRqUploadFilePacket", 0))
					{
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("LoRqStartSinglePacket", 0))
					{
						int TimeAttackStartTicks = iPacket.ReadInt();
						this.Parent.TimeAttackStartTicks = TimeAttackStartTicks;
						this.Parent.PlaneCheck1 = (byte)this.Parent.TimeAttackStartTicks;
						uint key = CryptoConstants.GetKey(CryptoConstants.GetKey((uint)this.Parent.TimeAttackStartTicks)) % 5 + 6;
						ALLnum = (int)key;
						this.Parent.SendPlaneCount = (int)key;
						this.Parent.TotalSendPlaneCount = 0;
						Console.WriteLine("PlaneCheckMax: {0}", this.Parent.SendPlaneCount);
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("GameSlotPacket", 0))
					{
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("GameReportPacket", 0))
					{
						byte[] DateTime1 = iPacket.ReadBytes(18);
						int GetItem = iPacket.ReadInt();
						int UseItem = iPacket.ReadInt();
						int UseBooster = iPacket.ReadEncodedInt();
						int[][] hashArray1 = new int[20][];
						for (int k = 0; k < 20; k++)
						{
							hashArray1[k] = new int[3];
							hashArray1[k][0] = iPacket.ReadEncodedInt();
							hashArray1[k][1] = iPacket.ReadEncodedInt();
							hashArray1[k][2] = iPacket.ReadEncodedInt();
						}
						int hash1 = iPacket.ReadEncodedInt();
						int hash2 = iPacket.ReadEncodedInt();
						int hash3 = iPacket.ReadEncodedInt();
						float single1 = iPacket.ReadEncodedFloat();
						float single2 = iPacket.ReadEncodedFloat();
						float single3 = iPacket.ReadEncodedFloat();
						int PlaneCheck = iPacket.ReadInt();
						byte[] hashArray2 = iPacket.ReadBytes(20);
						int hash4 = iPacket.ReadInt();
						byte[] hashArray3 = iPacket.ReadBytes(16);
						this.Parent.TotalSendPlaneCount += PlaneCheck;
						Console.WriteLine("PlaneCheck: {0}, Total: {1}, Max: {2}, Dist: {3}", PlaneCheck, this.Parent.TotalSendPlaneCount, this.Parent.SendPlaneCount, single3);
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqRenameRidPacket", 0))
					{
						SetRider.Nickname = iPacket.ReadString(false);
						using (OutPacket outPacket = new OutPacket("SpRpRenameRidPacket"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						SetGameData.Save_Nickname();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetRider", 0))
					{
						NewRider.LoadItemData();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("LoRqGetRiderItemPacket", 0))
					{
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("LoRqSetRiderItemOnPacket", 0))
					{
						SetRiderItem.Set_Character = iPacket.ReadShort();
						SetRiderItem.Set_Paint = iPacket.ReadShort();
						SetRiderItem.Set_Kart = iPacket.ReadShort();
						SetRiderItem.Set_Plate = iPacket.ReadShort();
						SetRiderItem.Set_Goggle = iPacket.ReadShort();
						SetRiderItem.Set_Balloon = iPacket.ReadShort();
						iPacket.ReadShort();
						SetRiderItem.Set_HeadBand = iPacket.ReadShort();
						SetRiderItem.Set_HeadPhone = iPacket.ReadShort();
						SetRiderItem.Set_HandGearL = iPacket.ReadShort();
						iPacket.ReadShort();
						SetRiderItem.Set_Uniform = iPacket.ReadShort();
						SetRiderItem.Set_Decal = iPacket.ReadShort();
						SetRiderItem.Set_Pet = iPacket.ReadShort();
						SetRiderItem.Set_FlyingPet = iPacket.ReadShort();
						SetRiderItem.Set_Aura = iPacket.ReadShort();
						SetRiderItem.Set_SkidMark = iPacket.ReadShort();
						iPacket.ReadShort();
						SetRiderItem.Set_RidColor = iPacket.ReadShort();
						SetRiderItem.Set_BonusCard = iPacket.ReadShort();
						iPacket.ReadShort();
						short Set_KartPlant1 = iPacket.ReadShort();
						short Set_KartPlant2 = iPacket.ReadShort();
						short Set_KartPlant3 = iPacket.ReadShort();
						short Set_KartPlant4 = iPacket.ReadShort();
						iPacket.ReadShort();
						iPacket.ReadShort();
						SetRiderItem.Set_Tachometer = iPacket.ReadShort();
						SetRiderItem.Set_Dye = iPacket.ReadShort();
						SetRiderItem.Set_KartSN = iPacket.ReadShort();
						iPacket.ReadByte();
						short Set_KartCoating = iPacket.ReadShort();
						short Set_KartTailLamp = iPacket.ReadShort();
						//SetRiderItem.Set_slotBg = iPacket.ReadShort();
						//iPacket.ReadShort();
						Console.WriteLine($"Set_KartPlant1: {Set_KartPlant1}");
						Console.WriteLine($"Set_KartPlant2: {Set_KartPlant2}");
						Console.WriteLine($"Set_KartPlant3: {Set_KartPlant3}");
						Console.WriteLine($"Set_KartPlant4: {Set_KartPlant4}");
						Console.WriteLine($"Set_KartCoating: {Set_KartCoating}");
						Console.WriteLine($"Set_KartTailLamp: {Set_KartTailLamp}");
						SetRiderItem.Save_SetRiderItem();
						ExcSpec.Use_PartsSpec(SetRiderItem.Set_Kart, SetRiderItem.Set_KartSN);
						ExcSpec.Use_ExcSpec(SetRiderItem.Set_Kart, SetRiderItem.Set_KartSN);
						ExcSpec.Use_PlantSpec(SetRiderItem.Set_Kart, SetRiderItem.Set_KartSN);
						ExcSpec.Use_KartLevelSpec(SetRiderItem.Set_Kart, SetRiderItem.Set_KartSN);
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetRiderInfo", 0))
					{
						iPacket.ReadInt();
						iPacket.ReadInt();
						string Nickname = iPacket.ReadString(false);
						if (Nickname == SetRider.Nickname)
						{
							//GameSupport.PrGetRiderInfo();
							using (OutPacket outPacket = new OutPacket("PrGetRiderInfo"))
							{
								outPacket.WriteByte(1);
								outPacket.WriteUInt(SetRider.UserNO);
								outPacket.WriteString(SetRider.UserID);
								outPacket.WriteString(SetRider.Nickname);
								outPacket.WriteUShort((ushort)DataTime()[0]);
								outPacket.WriteUShort((ushort)DataTime()[1]);
								GameSupport.GetRider(outPacket);
								outPacket.WriteString(SetRider.Card);
								outPacket.WriteUInt(SetRider.RP);
								outPacket.WriteInt(0);
								outPacket.WriteByte(6);//Licenses
								outPacket.WriteUShort((ushort)DataTime()[0]);
								outPacket.WriteUShort((ushort)DataTime()[1]);
								outPacket.WriteBytes(new byte[17]);
								outPacket.WriteShort(SetRider.Emblem1);
								outPacket.WriteShort(SetRider.Emblem2);
								outPacket.WriteShort(0);
								outPacket.WriteString(SetRider.RiderIntro);
								outPacket.WriteInt(SetRider.Premium);
								outPacket.WriteByte(1);
								if (SetRider.Premium == 0)
									outPacket.WriteInt(0);
								else if (SetRider.Premium == 1)
									outPacket.WriteInt(10000);
								else if (SetRider.Premium == 2)
									outPacket.WriteInt(30000);
								else if (SetRider.Premium == 3)
									outPacket.WriteInt(60000);
								else if (SetRider.Premium == 4)
									outPacket.WriteInt(120000);
								else if (SetRider.Premium == 5)
									outPacket.WriteInt(200000);
								else
									outPacket.WriteInt(0);
								if (SetRider.ClubMark_LOGO == 0)
								{
									outPacket.WriteInt(0);
									outPacket.WriteInt(0);
									//outPacket.WriteInt(0);
									outPacket.WriteString("");
								}
								else
								{
									outPacket.WriteInt(SetRider.ClubCode);
									outPacket.WriteInt(SetRider.ClubMark_LOGO);
									//outPacket.WriteInt(SetRider.ClubMark_LINE);
									outPacket.WriteString(SetRider.ClubName);
								}
								outPacket.WriteInt(0);
								outPacket.WriteByte(SetRider.Ranker);
								outPacket.WriteInt(0);
								outPacket.WriteInt(0);
								outPacket.WriteInt(0);
								outPacket.WriteInt(0);
								outPacket.WriteInt(0);
								outPacket.WriteByte(0);
								outPacket.WriteByte(0);
								outPacket.WriteByte(0);
								RouterListener.MySession.Client.Send(outPacket);
							}
						}
						else
						{
							using (OutPacket outPacket = new OutPacket("PrGetRiderInfo"))
							{
								outPacket.WriteByte(0);
								this.Parent.Client.Send(outPacket);
							}
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqUpdateRiderIntro", 0))
					{
						SetRider.RiderIntro = iPacket.ReadString(false);
						SetGameData.Save_RiderIntro();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqUpdateRiderSchoolLevelPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrUpdateRiderSchoolLevelPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqSetPlaytimeEventTick", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrSetPlaytimeEventTick"))
						{
							outPacket.WriteByte(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqUpdateGameOption", 0))
					{
						SetGameOption.Set_BGM = iPacket.ReadFloat();
						SetGameOption.Set_Sound = iPacket.ReadFloat();
						SetGameOption.Main_BGM = iPacket.ReadByte();
						SetGameOption.Sound_effect = iPacket.ReadByte();
						SetGameOption.Full_screen = iPacket.ReadByte();
						SetGameOption.Unk1 = iPacket.ReadByte();
						SetGameOption.Unk2 = iPacket.ReadByte();
						SetGameOption.Unk3 = iPacket.ReadByte();
						SetGameOption.Unk4 = iPacket.ReadByte();
						SetGameOption.Unk5 = iPacket.ReadByte();
						SetGameOption.Unk6 = iPacket.ReadByte();
						SetGameOption.Unk7 = iPacket.ReadByte();
						SetGameOption.Unk8 = iPacket.ReadByte();//오토 레디
						SetGameOption.Unk9 = iPacket.ReadByte();//아이템 설명
						SetGameOption.Unk10 = iPacket.ReadByte();//녹화 품질
						SetGameOption.Unk11 = iPacket.ReadByte();
						SetGameOption.BGM_Check = iPacket.ReadByte();//배경음
						SetGameOption.Sound_Check = iPacket.ReadByte();//효과음
						SetGameOption.Unk12 = iPacket.ReadByte();
						SetGameOption.Unk13 = iPacket.ReadByte();
						SetGameOption.GameType = iPacket.ReadByte();//팀전 개인전 여부
						SetGameOption.SetGhost = iPacket.ReadByte();//고스트 사용여부
						SetGameOption.SpeedType = iPacket.ReadByte();//채널 속도
						SetGameOption.Unk14 = iPacket.ReadByte();
						SetGameOption.Unk15 = iPacket.ReadByte();
						SetGameOption.Unk16 = iPacket.ReadByte();
						SetGameOption.Unk17 = iPacket.ReadByte();
						//SetGameOption.Set_screen = iPacket.ReadByte();
						//SetGameOption.Unk18 = iPacket.ReadByte();
						SetGameOption.Save_SetGameOption();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetGameOption", 0))
					{
						GameSupport.PrGetGameOption();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqVipInfo", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrVipInfo"))
						{
							outPacket.WriteInt(1);
							for (int i = 0; i < 10; i++)
							{
								outPacket.WriteInt(0);
							}
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqLoginVipInfo", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrLoginVipInfo"))
						{
							outPacket.WriteInt(SetRider.Premium);
							outPacket.WriteByte(1);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						using (OutPacket outPacket = new OutPacket("LoRpEventRewardPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						using (OutPacket outPacket = new OutPacket("PcSlaveNotice"))
						{
							outPacket.WriteString("單機版完全免費，GitHub：https://github.com/yanygm/Launcher_TF_2361");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("ChReRqEnterMyRoomPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("ChRqEnterRandomMyRoomPacket", 0))
					{
						if (hash == 1733888222)//ChReRqEnterMyRoomPacket
						{
							GameType.EnterMyRoomType = 0;
						}
						else if (hash == 2423851656)//ChRqEnterRandomMyRoomPacket
						{
							GameType.EnterMyRoomType = 5;
						}
						GameSupport.ChRpEnterMyRoomPacket();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("ChRqEnterMyRoomPacket", 0))
					{
						string Nickname = iPacket.ReadString(false);
						if (Nickname == SetRider.Nickname)
						{
							GameType.EnterMyRoomType = 0;
						}
						else
						{
							GameType.EnterMyRoomType = 3;
						}
						GameSupport.ChRpEnterMyRoomPacket();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("RmFirstRequestPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("RmSlotDataPacket"))
						{
							outPacket.WriteUInt(SetRider.UserNO);
							outPacket.WriteBytes(new byte[12]);
							outPacket.WriteString(SetRider.Nickname);
							GameSupport.GetRider(outPacket);
							outPacket.WriteUInt(SetRider.RP);
							outPacket.WriteBytes(new byte[910]);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("RmNotiMyRoomInfoPacket", 0))
					{
						SetMyRoom.MyRoom = iPacket.ReadShort();
						SetMyRoom.MyRoomBGM = iPacket.ReadByte();
						SetMyRoom.UseRoomPwd = iPacket.ReadByte();
						iPacket.ReadByte();
						SetMyRoom.UseItemPwd = iPacket.ReadByte();
						SetMyRoom.TalkLock = iPacket.ReadByte();
						SetMyRoom.RoomPwd = iPacket.ReadString(false);
						iPacket.ReadString(false);
						SetMyRoom.ItemPwd = iPacket.ReadString(false);
						SetMyRoom.MyRoomKart1 = iPacket.ReadShort();
						SetMyRoom.MyRoomKart2 = iPacket.ReadShort();
						SetMyRoom.Save_SetMyRoom();
						GameSupport.RmNotiMyRoomInfoPacket();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("ChRqSecedeMyRoomPacket", 0))
					{
						//마이룸 나갈때
						using (OutPacket outPacket = new OutPacket("ChRpSecedeMyRoomPacket"))
						{
							outPacket.WriteByte(1);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqStartScenario", 0))
					{
						GameType.ScenarioType = iPacket.ReadInt();
						using (OutPacket outPacket = new OutPacket("PrStartScenario"))
						{
							outPacket.WriteInt(GameType.ScenarioType);
							outPacket.WriteByte(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqCompleteScenarioSingle", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrCompleteScenarioSingle"))
						{
							outPacket.WriteInt(GameType.ScenarioType);
							outPacket.WriteByte(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqKartSpec", 0))
					{
						StartGameData.StartTimeAttack_SpeedType = iPacket.ReadByte();
						StartGameData.Kart_id = iPacket.ReadShort();
						StartGameData.FlyingPet_id = iPacket.ReadShort();
						GameType.StartType = 1;
						SpeedType.SpeedTypeData();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqChapterInfoPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrChapterInfoPacket"))
						{
							outPacket.WriteInt(56);
							for (int i = 1; i <= 56; i++)
							{
								outPacket.WriteInt(i | 0x1000000);
								outPacket.WriteInt((int)(Math.Pow(2, 30) - 1));
								outPacket.WriteInt(0);
								outPacket.WriteByte(0);
							}
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqChallengerInfoPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrChallengerInfoPacket"))
						{
							int stage = 40;
							outPacket.WriteInt(stage);
							for (int i = 0; i < stage; i++)
							{
								outPacket.WriteShort(55);
							}
							outPacket.WriteInt(0);
							outPacket.WriteByte(1);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqStartChallenger", 0))
					{
						int stage_id = iPacket.ReadInt();
						int GameType = iPacket.ReadInt();
						short Kart = iPacket.ReadShort();
						byte Unk1 = iPacket.ReadByte();
						byte Unk2 = iPacket.ReadByte();
						byte Unk3 = iPacket.ReadByte();
						using (OutPacket outPacket = new OutPacket("PrStartChallenger"))
						{
							outPacket.WriteInt(stage_id);
							outPacket.WriteInt(GameType);
							outPacket.WriteByte(0);
							outPacket.WriteByte(1);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqchallengerKartSpec", 0))
					{
						StartGameData.StartTimeAttack_SpeedType = iPacket.ReadByte();
						StartGameData.Kart_id = iPacket.ReadShort();
						StartGameData.FlyingPet_id = iPacket.ReadShort();
						GameType.StartType = 2;
						SpeedType.SpeedTypeData();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqCompleteChallenger", 0))
					{
						int stage = 40;
						byte StageType = iPacket.ReadByte();
						iPacket.ReadInt();
						int EndType = iPacket.ReadInt();
						using (OutPacket outPacket = new OutPacket("PrCompleteChallenger"))
						{
							outPacket.WriteByte(StageType);
							outPacket.WriteInt(0);
							outPacket.WriteInt(EndType);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(stage);
							for (int i = 0; i < stage; i++)
							{
								outPacket.WriteShort(55);
							}
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteByte(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqExChangePacket", 0))
					{
						GameSupport.OnDisconnect();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqKartLevelPointClear", 0))
					{
						short Kart = iPacket.ReadShort();
						short SN = iPacket.ReadShort();
						using (OutPacket outPacket = new OutPacket("PrKartLevelPointClear"))
						{
							outPacket.WriteInt(1);
							outPacket.WriteShort(Kart);
							outPacket.WriteShort(SN);
							outPacket.WriteShort(5);
							outPacket.WriteShort(35);
							outPacket.WriteShort(0);
							outPacket.WriteShort(0);
							outPacket.WriteShort(0);
							outPacket.WriteShort(0);
							outPacket.WriteShort(0);
							outPacket.WriteUInt(SetRider.Koin);
							this.Parent.Client.Send(outPacket);
						}
						KartExcData.AddLevelList(Kart, SN, 5, 35, 0, 0, 0, 0, 0);
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqDisassembleXPartsItem", 0))
					{
						iPacket.ReadByte();
						iPacket.ReadShort();
						short Kart = iPacket.ReadShort();
						iPacket.ReadShort();
						short SN = iPacket.ReadShort();
						iPacket.ReadBytes(6);
						byte[] data = iPacket.ReadBytes(28);
						using (OutPacket outPacket = new OutPacket("PrDisassembleXPartsItem"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteShort(3);
							outPacket.WriteShort(0);
							outPacket.WriteShort(0);
							outPacket.WriteShort(0);
							outPacket.WriteInt(0);
							outPacket.WriteShort(0);
							outPacket.WriteShort(Kart);
							outPacket.WriteShort(1);
							outPacket.WriteShort(0);
							outPacket.WriteByte(1);//Grade
							outPacket.WriteByte(0);//X-1 V1-2
							outPacket.WriteShort(0);
							outPacket.WriteShort(0);
							outPacket.WriteBytes(data);
							outPacket.WriteUInt(SetRider.Lucci);
							outPacket.WriteUInt(SetRider.Koin);
							this.Parent.Client.Send(outPacket);
						}
						KartExcData.AddPartsList(Kart, SN, 63, 0, 0, 0);
						KartExcData.AddPartsList(Kart, SN, 64, 0, 0, 0);
						KartExcData.AddPartsList(Kart, SN, 65, 0, 0, 0);
						KartExcData.AddPartsList(Kart, SN, 66, 0, 0, 0);
						KartExcData.AddPartsList(Kart, SN, 68, 0, 0, 0);
						KartExcData.AddPartsList(Kart, SN, 69, 0, 0, 0);
						KartExcData.AddPlantList(Kart, SN, 43, 0);
						KartExcData.AddPlantList(Kart, SN, 44, 0);
						KartExcData.AddPlantList(Kart, SN, 45, 0);
						KartExcData.AddPlantList(Kart, SN, 46, 0);
						ExcSpec.Use_PartsSpec(Kart, SN);
						ExcSpec.Use_PlantSpec(Kart, SN);
						//GameSupport.OnDisconnect();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqKartLevelUpProbText", 0))
					{
						short Kart = iPacket.ReadShort();
						short SN = iPacket.ReadShort();
						iPacket.ReadShort();
						iPacket.ReadShort();
						using (OutPacket outPacket = new OutPacket("PrKartLevelUpProbText"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqKartLevelUp", 0))
					{
						short Kart = iPacket.ReadShort();
						short SN = iPacket.ReadShort();
						short Kart2 = iPacket.ReadShort();
						short SN2 = iPacket.ReadShort();
						using (OutPacket outPacket = new OutPacket("PrKartLevelUp"))
						{
							outPacket.WriteUShort((ushort)DataTime()[0]);
							outPacket.WriteUShort((ushort)DataTime()[1]);
							outPacket.WriteInt(1);
							outPacket.WriteShort(Kart);
							outPacket.WriteShort(SN);
							outPacket.WriteShort(5);
							outPacket.WriteShort(35);
							outPacket.WriteShort(0);
							outPacket.WriteShort(0);
							outPacket.WriteShort(0);
							outPacket.WriteShort(0);
							outPacket.WriteInt(0);
							outPacket.WriteShort(0);//Kart2
							outPacket.WriteShort(0);//SN2
							outPacket.WriteUInt(SetRider.Koin);
							outPacket.WriteUInt(SetRider.Lucci);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						KartExcData.AddLevelList(Kart, SN, 5, 35, 0, 0, 0, 0, 0);
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqKartLevelPointUpdate", 0))
					{
						short Kart = iPacket.ReadShort();
						short SN = iPacket.ReadShort();
						short v1 = iPacket.ReadShort();
						short v2 = iPacket.ReadShort();
						short v3 = iPacket.ReadShort();
						short v4 = iPacket.ReadShort();
						short pointleft = 0;
						short Effect = 0;
						using (OutPacket outPacket = new OutPacket("PrKartLevelPointUpdate"))
						{
							outPacket.WriteInt(1);
							outPacket.WriteShort(Kart);
							outPacket.WriteShort(SN);
							outPacket.WriteShort(5);
							outPacket.WriteShort(pointleft);
							outPacket.WriteShort(v1);
							outPacket.WriteShort(v2);
							outPacket.WriteShort(v3);
							outPacket.WriteShort(v4);
							outPacket.WriteShort(Effect);
							this.Parent.Client.Send(outPacket);
						}
						ExcSpec.Use_KartLevelSpec(Kart, SN);
						var existingLevelList = KartExcData.LevelList.FirstOrDefault(list => list[0] == Kart && list[1] == SN);
						if (existingLevelList == null)
						{
							pointleft = (short)(35 - v1 - v2 - v3 - v4);
							KartExcData.AddLevelList(Kart, SN, 5, pointleft, v1, v2, v3, v4, 0);
						}
						else
						{
							pointleft = (short)(existingLevelList[3] - v1 - v2 - v3 - v4);
							short v1New = (short)(existingLevelList[4] + v1);
							short v2New = (short)(existingLevelList[5] + v2);
							short v3New = (short)(existingLevelList[6] + v3);
							short v4New = (short)(existingLevelList[7] + v4);
							short effect = existingLevelList[8];
							KartExcData.AddLevelList(Kart, SN, 5, pointleft, v1New, v2New, v3New, v4New, effect);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqGetMaxGiftIdPacket", 0))
					{
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqKartLevelSpecialSlotUpdate", 0))
					{
						short Kart = iPacket.ReadShort();
						short SN = iPacket.ReadShort();
						short Effect = iPacket.ReadShort();

						var existingLevelList = KartExcData.LevelList.FirstOrDefault(list => list[0] == Kart && list[1] == SN);

						using (OutPacket outPacket = new OutPacket("PrKartLevelSpecialSlotUpdate"))
						{
							outPacket.WriteInt(1);
							outPacket.WriteShort(Kart);
							outPacket.WriteShort(SN);
							if (existingLevelList != null)
							{
								outPacket.WriteShort(existingLevelList[2]);
								outPacket.WriteShort(existingLevelList[3]);
								outPacket.WriteShort(existingLevelList[4]);
								outPacket.WriteShort(existingLevelList[5]);
								outPacket.WriteShort(existingLevelList[6]);
								outPacket.WriteShort(existingLevelList[7]);
								outPacket.WriteShort(Effect);
								KartExcData.AddLevelList(Kart, SN, existingLevelList[2], existingLevelList[3], existingLevelList[4], existingLevelList[5], existingLevelList[6], existingLevelList[7], Effect);
							}
							else
							{
								outPacket.WriteShort(5);
								outPacket.WriteShort(0);
								outPacket.WriteShort(10);
								outPacket.WriteShort(10);
								outPacket.WriteShort(10);
								outPacket.WriteShort(5);
								outPacket.WriteShort(Effect);
								KartExcData.AddLevelList(Kart, SN, 5, 0, 10, 10, 10, 5, Effect);
							}
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqUseSocketItem", 0))
					{
						short Item = iPacket.ReadShort();
						short Item_Id = iPacket.ReadShort();
						short Kart = iPacket.ReadShort();
						iPacket.ReadShort();
						short KartSN = iPacket.ReadShort();
						using (OutPacket outPacket = new OutPacket("PrUseSocketItem"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteShort(Item);
							outPacket.WriteShort(Item_Id);
							outPacket.WriteShort(Kart);
							outPacket.WriteShort(KartSN);
							outPacket.WriteShort(KartSN);
							outPacket.WriteShort(2);
							outPacket.WriteHexString("00 00 00 00 FF FF 00 00 00 00 00 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						KartExcData.AddTuneList(Kart, KartSN, 0, 0, 0, -1, 0, -1, 0);
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqUseTuneItem", 0))
					{
						short Item = iPacket.ReadShort();
						short Item_Id = iPacket.ReadShort();
						short Kart = iPacket.ReadShort();
						iPacket.ReadShort();
						short KartSN = iPacket.ReadShort();
						Random random = new Random();
						var existingList = KartExcData.TuneList.FirstOrDefault(list => list[0] == Kart && list[1] == KartSN);
						if (existingList != null)
						{
							if (Item == 5)
							{
								using (OutPacket outPacket = new OutPacket("PrUseTuneItem"))
								{
									outPacket.WriteInt(0);
									outPacket.WriteShort(Item);
									outPacket.WriteShort(Item_Id);
									outPacket.WriteShort(Kart);
									outPacket.WriteShort(KartSN);
									outPacket.WriteShort(0);
									outPacket.WriteShort(603);
									outPacket.WriteShort(703);
									outPacket.WriteShort(903);
									outPacket.WriteShort(existingList[5]);
									outPacket.WriteShort(existingList[6]);
									outPacket.WriteShort(existingList[7]);
									outPacket.WriteShort(existingList[8]);
									this.Parent.Client.Send(outPacket);
								}
								existingList[2] = 603;
								existingList[3] = 703;
								existingList[4] = 903;
								KartExcData.SaveTuneList(KartExcData.TuneList);
								ExcSpec.Use_ExcSpec(Kart, KartSN);
							}
							else
							{
								List<short> tuneList1 = new List<short> { existingList[2], existingList[3], existingList[4] };
								List<short> tuneList2 = new List<short>();
								using (OutPacket outPacket = new OutPacket("PrUseTuneItem"))
								{
									outPacket.WriteInt(0);
									outPacket.WriteShort(Item);
									outPacket.WriteShort(Item_Id);
									outPacket.WriteShort(Kart);
									outPacket.WriteShort(KartSN);
									outPacket.WriteShort(0);
									if (existingList[2] != 0)
									{
										outPacket.WriteShort(existingList[2]);
									}
									else
									{
										while (tuneList1.Count < 4)
										{
											short number = short.Parse(random.Next(1, 10).ToString() + "03");
											if (!tuneList1.Contains(number))
											{
												outPacket.WriteShort(number);
												existingList[2] = number;
												tuneList1.Add(number);
												tuneList2.Add(number);
											}
										}
										tuneList1.RemoveAt(3);
									}
									if (existingList[3] != 0)
									{
										outPacket.WriteShort(existingList[3]);
									}
									else
									{
										while (tuneList1.Count < 4)
										{
											short number = short.Parse(random.Next(1, 10).ToString() + "03");
											if (!tuneList1.Contains(number))
											{
												if (!tuneList2.Contains(number))
												{
													outPacket.WriteShort(number);
													existingList[3] = number;
													tuneList1.Add(number);
													tuneList2.Add(number);
												}
											}
										}
										tuneList1.RemoveAt(3);
									}
									if (existingList[4] != 0)
									{
										outPacket.WriteShort(existingList[4]);
									}
									else
									{
										while (tuneList1.Count < 4)
										{
											short number = short.Parse(random.Next(1, 10).ToString() + "03");
											if (!tuneList1.Contains(number))
											{
												if (!tuneList2.Contains(number))
												{
													outPacket.WriteShort(number);
													existingList[4] = number;
													tuneList1.Add(number);
													tuneList2.Add(number);
												}
											}
										}
										tuneList1.RemoveAt(3);
									}
									outPacket.WriteShort(existingList[5]);
									outPacket.WriteShort(existingList[6]);
									outPacket.WriteShort(existingList[7]);
									outPacket.WriteShort(existingList[8]);
									this.Parent.Client.Send(outPacket);
								}
							}
							KartExcData.SaveTuneList(KartExcData.TuneList);
							ExcSpec.Use_ExcSpec(Kart, KartSN);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqUseProtectSpannerItem", 0))
					{
						short Protect = iPacket.ReadShort();
						short v2 = iPacket.ReadShort();
						short Item_Id = iPacket.ReadShort();
						short Kart = iPacket.ReadShort();
						short KartSN = iPacket.ReadShort();
						iPacket.ReadShort();
						short slot = iPacket.ReadShort();
						var existingList = KartExcData.TuneList.FirstOrDefault(list => list[0] == Kart && list[1] == KartSN);
						if (existingList != null)
						{
							using (OutPacket outPacket = new OutPacket("PrUseProtectSpannerItem"))
							{
								outPacket.WriteInt(0);
								outPacket.WriteShort(Protect);
								outPacket.WriteShort(v2);
								outPacket.WriteShort(Item_Id);
								outPacket.WriteShort(Kart);
								outPacket.WriteShort(KartSN);
								outPacket.WriteShort(0);
								outPacket.WriteShort(0);
								outPacket.WriteShort(0);
								outPacket.WriteShort(existingList[2]);
								outPacket.WriteShort(existingList[3]);
								outPacket.WriteShort(existingList[4]);
								if (Protect == 49)
								{
									outPacket.WriteShort(slot);
									outPacket.WriteShort(4);
									outPacket.WriteShort(existingList[7]);
									outPacket.WriteShort(existingList[8]);
									existingList[5] = slot;
									existingList[6] = 4;
								}
								else if (Protect == 53)
								{
									outPacket.WriteShort(existingList[5]);
									outPacket.WriteShort(existingList[6]);
									outPacket.WriteShort(slot);
									outPacket.WriteShort(3);
									existingList[7] = slot;
									existingList[8] = 3;
								}
								else
								{
									outPacket.WriteShort(existingList[5]);
									outPacket.WriteShort(existingList[6]);
									outPacket.WriteShort(existingList[7]);
									outPacket.WriteShort(existingList[8]);
								}
								this.Parent.Client.Send(outPacket);
								KartExcData.SaveTuneList(KartExcData.TuneList);
							}
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqUseResetSocketItem", 0))
					{
						short Item = iPacket.ReadShort();
						short Item_Id = iPacket.ReadShort();
						short Kart = iPacket.ReadShort();
						iPacket.ReadShort();
						short KartSN = iPacket.ReadShort();
						var existingList = KartExcData.TuneList.FirstOrDefault(list => list[0] == Kart && list[1] == KartSN);
						if (existingList != null)
						{
							using (OutPacket outPacket = new OutPacket("PrUseResetSocketItem"))
							{
								outPacket.WriteInt(0);
								outPacket.WriteShort(Item);
								outPacket.WriteShort(Item_Id);
								outPacket.WriteShort(Kart);
								outPacket.WriteShort(KartSN);
								outPacket.WriteShort(34);
								outPacket.WriteShort(76);
								outPacket.WriteShort(1);
								outPacket.WriteShort(1);
								outPacket.WriteShort(existingList[2]);
								outPacket.WriteShort(existingList[3]);
								outPacket.WriteShort(existingList[4]);
								outPacket.WriteShort(existingList[5]);
								outPacket.WriteShort(existingList[6]);
								outPacket.WriteShort(existingList[7]);
								outPacket.WriteShort(existingList[8]);
								this.Parent.Client.Send(outPacket);
							}
							List<short> secure = new List<short>();
							if (existingList[6] == 0)
							{
								existingList[5] = -1;
							}
							else
							{
								existingList[6] = (short)((int)existingList[6] - 1);
							}
							if (existingList[8] == 0)
							{
								existingList[7] = -1;
							}
							else
							{
								existingList[8] = (short)((int)existingList[8] - 1);
							}
							Console.WriteLine("TuneProtect: " + existingList[5] + ", " + existingList[7]);
							Console.WriteLine("TuneProtectCount: " + existingList[6] + ", " + existingList[8]);
							for (int i = 2; i <= 4; i++)
							{
								if (existingList[5] != -1 && (int)(existingList[5]) + 2 == i)
								{
								}
								else if (existingList[7] != -1 && (int)(existingList[7]) + 2 == i)
								{
								}
								else
								{
									existingList[i] = 0;
								}
							}
							Console.WriteLine("TuneList: " + existingList[2] + ", " + existingList[3] + ", " + existingList[4]);
							KartExcData.SaveTuneList(KartExcData.TuneList);
							ExcSpec.Use_ExcSpec(Kart, KartSN);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqEquipTuningExPacket", 0))
					{
						short Item = iPacket.ReadShort();
						short Item_Id = iPacket.ReadShort();
						short Kart = iPacket.ReadShort();
						short Kart_Id = iPacket.ReadShort();
						short SN = iPacket.ReadShort();
						KartExcData.AddPlantList(Kart_Id, SN, Item, Item_Id);
						ExcSpec.Use_PlantSpec(Kart_Id, SN);
						using (OutPacket outPacket = new OutPacket("PrEquipTuningPacket"))
						{
							outPacket.WriteByte(1);
							outPacket.WriteShort(SN);
							outPacket.WriteShort(SN);
							outPacket.WriteShort(Kart_Id);
							outPacket.WriteShort(Item);
							outPacket.WriteShort(Item_Id);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqUnequipXPartsItem", 0))
					{
						short Kart = iPacket.ReadShort();
						short SN = iPacket.ReadShort();
						short item = iPacket.ReadShort();
						using (OutPacket outPacket = new OutPacket("PrUnequipXPartsItem"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteShort(Kart);
							outPacket.WriteShort(SN);
							outPacket.WriteShort(item);
							this.Parent.Client.Send(outPacket);
						}
						KartExcData.AddPartsList(Kart, SN, item, 0, 0, 0);
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqEquipXPartsItem", 0))
					{
						short Kart = iPacket.ReadShort();
						short KartSN = iPacket.ReadShort();
						short Item_Cat_Id = iPacket.ReadShort();
						short Item_Id = iPacket.ReadShort();
						short Quantity = iPacket.ReadShort();
						short Unk1 = iPacket.ReadShort();
						byte Grade = iPacket.ReadByte();
						byte Unk2 = iPacket.ReadByte();
						short PartsValue = iPacket.ReadShort();
						short Unk3 = iPacket.ReadShort();
						using (OutPacket outPacket = new OutPacket("PrEquipXPartsItem"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteShort(Kart);
							outPacket.WriteShort(KartSN);
							outPacket.WriteShort(Item_Cat_Id);
							outPacket.WriteShort(Item_Id);
							outPacket.WriteShort(Quantity);
							outPacket.WriteShort(Unk1);
							outPacket.WriteByte(Grade);
							outPacket.WriteByte(Unk2);
							outPacket.WriteShort(PartsValue);
							outPacket.WriteShort(Unk3);
							this.Parent.Client.Send(outPacket);
						}
						KartExcData.AddPartsList(Kart, KartSN, Item_Cat_Id, Item_Id, Grade, PartsValue);
						Console.WriteLine("ClientSession : Kart: {0}, KartSN: {1}, Item: {2}:{3}, Quantity: {4}, Grade: {5}, PartsValue: {6}", Kart, KartSN, Item_Cat_Id, Item_Id, Quantity, Grade, PartsValue);
						ExcSpec.Use_PartsSpec(Kart, KartSN);
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetTrainingMission", 0))
					{
						int type = iPacket.ReadInt();
						uint track = iPacket.ReadUInt();
						//PrGetTrainingMission 00 08 B7 51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
						using (OutPacket outPacket = new OutPacket("PrGetTrainingMission"))
						{
							outPacket.WriteInt(type);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetDuelMissionBulk", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetDuelMissionBulk"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteUShort((ushort)DataTime()[0]);
							outPacket.WriteUShort((ushort)DataTime()[1]);
							outPacket.WriteHexString("0F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqAdjustDuelMissionDifficulty", 0))
					{
						Console.WriteLine("PqAdjustDuelMissionDifficulty: {0}", iPacket);
						int type = iPacket.ReadInt();
						int unk = iPacket.ReadInt();
						using (OutPacket outPacket = new OutPacket("PrAdjustDuelMissionDifficulty"))
						{
							outPacket.WriteInt(type);
							outPacket.WriteInt(unk);
							outPacket.WriteInt(0);
							outPacket.WriteShort(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqBlueMarble", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrBlueMarble"))
						{
							outPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("LoRqGetTrackRankPacket", 0))
					{
						uint track = iPacket.ReadUInt();
						byte SpeedType = iPacket.ReadByte();
						byte GameType = iPacket.ReadByte();
						using (OutPacket outPacket = new OutPacket("LoRpGetTrackRankPacket"))
						{
							outPacket.WriteUInt(track);
							outPacket.WriteByte(SpeedType);
							outPacket.WriteByte(GameType);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqFavoriteTrackUpdate", 0))
					{
						iPacket.ReadByte();
						int tracks = iPacket.ReadInt(); //赛道数量
						for (int i = 0; i < tracks; i++)
						{
							short theme = iPacket.ReadShort(); //主题代码
							int track = iPacket.ReadInt(); //赛道代码
							byte Add_Del = iPacket.ReadByte(); //1添加，2删除
							if (Add_Del == 1)
							{
								FavoriteItem.Favorite_Track_Add(theme, track);
							}
							else if (Add_Del == 2)
							{
								FavoriteItem.Favorite_Track_Del(theme, track);
							}
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("LoRqDecLucciPacket", 0))
					{
						iPacket.ReadByte();
						uint Lucci = iPacket.ReadUInt();
						SetRider.Lucci -= Lucci;
						SetGameData.Save_TimeAttackDecLucci();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqStartTimeAttack", 0))
					{
						StartGameData.StartTimeAttack_Unk1 = iPacket.ReadInt();
						StartGameData.StartTimeAttack_Unk2 = iPacket.ReadInt();
						StartGameData.StartTimeAttack_Track = iPacket.ReadUInt();
						StartGameData.StartTimeAttack_SpeedType = iPacket.ReadByte();
						StartGameData.StartTimeAttack_GameType = iPacket.ReadByte();
						StartGameData.Kart_id = iPacket.ReadShort();
						StartGameData.FlyingPet_id = iPacket.ReadShort();
						StartGameData.StartTimeAttack_StartType = iPacket.ReadByte();
						StartGameData.StartTimeAttack_Unk3 = iPacket.ReadInt();
						StartGameData.StartTimeAttack_Unk4 = iPacket.ReadInt();
						StartGameData.StartTimeAttack_Unk5 = iPacket.ReadByte();
						StartGameData.StartTimeAttack_RankingTimaAttackType = iPacket.ReadByte();
						StartGameData.StartTimeAttack_TimaAttackMpdeType = iPacket.ReadByte();
						StartGameData.StartTimeAttack_TimaAttackMpde = iPacket.ReadInt();
						//StartGameData.StartTimeAttack_RandomTrackGameType = iPacket.ReadByte();
						TrackName trackName = (TrackName)StartGameData.StartTimeAttack_Track;
						if (StartGameData.StartTimeAttack_TimaAttackMpdeType == 1)
						{
							SetRider.Lucci -= 1000;
							SetGameData.Save_TimeAttackDecLucci();
						}
						Console.WriteLine("StartTimeAttack: {0} / {1} / {2} / {3} / {4} / {5} / {6} / {7}", StartGameData.StartTimeAttack_SpeedType, StartGameData.StartTimeAttack_GameType, StartGameData.Kart_id, StartGameData.FlyingPet_id, trackName, StartGameData.StartTimeAttack_StartType, StartGameData.StartTimeAttack_RankingTimaAttackType, StartGameData.StartTimeAttack_TimaAttackMpdeType);
						GameType.StartType = 3;
						//RandomTrack.SetGameType();
						SpeedType.SpeedTypeData();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqFinishTimeAttack", 0))
					{
						int type = iPacket.ReadInt();
						iPacket.ReadInt();
						GameType.RewardType = iPacket.ReadByte();
						iPacket.ReadInt();
						iPacket.ReadInt();
						iPacket.ReadInt();
						iPacket.ReadInt();
						int Time = iPacket.ReadInt();
						GameType.min = Time / 60000;
						int sec = Time - GameType.min * 60000;
						GameType.sec = sec / 1000;
						GameType.mil = Time % 1000;
						if (GameType.RewardType == 0)
						{
							GameType.TimeAttack_RP = 10;
							GameType.TimeAttack_Lucci = 20;
						}
						else if (GameType.RewardType == 1)
						{
							GameType.TimeAttack_RP = 20;
							GameType.TimeAttack_Lucci = 50;
						}
						SetRider.RP += GameType.TimeAttack_RP;
						SetRider.Lucci += GameType.TimeAttack_Lucci;
						TrackName trackName = (TrackName)StartGameData.StartTimeAttack_Track;
						Console.WriteLine("FinishTimeAttack: {0} / {1} / {2} / {3} / {4}:{5}:{6}", GameType.RewardType, GameType.TimeAttack_RP, GameType.TimeAttack_Lucci, trackName, GameType.min, GameType.sec, GameType.mil);
						using (OutPacket outPacket = new OutPacket("PrFinishTimeAttack"))
						{
							outPacket.WriteInt(type);
							outPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00");
							outPacket.WriteUInt(GameType.TimeAttack_RP);//RP
							outPacket.WriteUInt(GameType.TimeAttack_Lucci);//LUCCI
							this.Parent.Client.Send(outPacket);
						}
						SetGameData.Save_RewardTimeAttack();
						SetGameData.Save_RecordTimeAttack();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqRewardTimeAttack", 0))
					{
						byte RewardType = iPacket.ReadByte();
						int RP = iPacket.ReadInt();
						int Lucci = iPacket.ReadInt();
						int TimeAttack_StartTicks = iPacket.ReadInt();
						uint Track = iPacket.ReadUInt();
						TrackName trackName = (TrackName)Track;
						Console.WriteLine("RewardTimeAttack : ResultType: {0}, RP: {1}, Lucci: {2}, Track: {3}", RewardType, RP, Lucci, trackName);
						if (RewardType == 0)
						{
							SetRider.RP += 10;
							SetRider.Lucci += 20;
						}
						else if (RewardType == 1)
						{
							SetRider.RP += 20;
							SetRider.Lucci += 50;
						}
						SetGameData.Save_RewardTimeAttack();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("LoRqUseItemPacket", 0))
					{
						short ItemType = iPacket.ReadShort();
						short Type = iPacket.ReadShort();
						SetRider.SlotChanger = iPacket.ReadShort();
						if (Type == 1)
						{
							SetGameData.Save_SlotChanger();
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqQuestUX2ndPacket", 0))
					{
						GameSupport.PrQuestUX2ndPacket();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGameOutRunUX2ndClearPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGameOutRunUX2ndClearPacket"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetTrainingMissionReward", 0))
					{
						Console.WriteLine("PqGetTrainingMissionReward: {0}", iPacket);
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqBingoGachaPacket", 0))
					{
						int BingoType = iPacket.ReadInt();
						if (BingoType == 0)
						{
							using (OutPacket outPacket = new OutPacket("SpRpBingoGachaPacket"))
							{
								outPacket.WriteInt(BingoType);
								for (int i = 0; i < 5; i++)
								{
									outPacket.WriteInt(0);
								}
								outPacket.WriteByte(0);
								outPacket.WriteByte(0);
								outPacket.WriteByte(0);
								this.Parent.Client.Send(outPacket);
							}
						}
						else if (BingoType == 4)
						{
							using (OutPacket outPacket = new OutPacket("SpRpBingoGachaPacket"))
							{
								outPacket.WriteInt(BingoType);
								for (int i = 0; i < 5; i++)
								{
									outPacket.WriteInt(0);
								}
								outPacket.WriteByte(0);
								outPacket.WriteByte(0);
								outPacket.WriteByte(0);
								this.Parent.Client.Send(outPacket);
							}
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqCheckMyClubStatePacket", 0))
					{
						GameSupport.PrCheckMyClubStatePacket();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqCheckMyLeaveDatePacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrCheckMyLeaveDatePacket"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetUserWaitingJoinClubPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetUserWaitingJoinClubPacket"))
						{
							outPacket.WriteInt(1);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqCheckCreateClubConditionPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrCheckCreateClubConditionPacket"))
						{
							outPacket.WriteInt(3);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqClubChannelSwitch", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrInitClubPacket"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqInitClubInfoPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrInitClubInfoPacket"))
						{
							outPacket.WriteHexString("D8 74 E0 12 00 00 00 00 00 00 10 CA D8 00 00 00 00 00 02 00 00 00 80 80 E0 12 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 FF FF 26 00 0A 00 03 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqSearchClubListPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrSearchClubListPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetClubListCountPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetClubListCountPacket"))
						{
							outPacket.WriteHexString("7F F7 00 00 01 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetClubWaitingCrewCountPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetClubWaitingCrewCountPacket"))
						{
							outPacket.WriteHexString("32 00 00 00 32 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqLotteryPacket", 0))
					{
						short Lottery_Item = iPacket.ReadShort();
						byte Unk = iPacket.ReadByte();
						int Type = iPacket.ReadInt();
						GameSupport.SpRpLotteryPacket();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqEventBuyCount", 0))
					{
						EventBuyCount.BuyCount = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 1) EventBuyCount.ShopItem1 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 2) EventBuyCount.ShopItem2 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 3) EventBuyCount.ShopItem3 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 4) EventBuyCount.ShopItem4 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 5) EventBuyCount.ShopItem5 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 6) EventBuyCount.ShopItem6 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 7) EventBuyCount.ShopItem7 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 8) EventBuyCount.ShopItem8 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 9) EventBuyCount.ShopItem9 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 10) EventBuyCount.ShopItem10 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 11) EventBuyCount.ShopItem11 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 12) EventBuyCount.ShopItem12 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 13) EventBuyCount.ShopItem13 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 14) EventBuyCount.ShopItem14 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 15) EventBuyCount.ShopItem15 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 16) EventBuyCount.ShopItem16 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 17) EventBuyCount.ShopItem17 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 18) EventBuyCount.ShopItem18 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 19) EventBuyCount.ShopItem19 = iPacket.ReadInt();
						if (EventBuyCount.BuyCount >= 20) EventBuyCount.ShopItem20 = iPacket.ReadInt();
						EventBuyCount.PrEventBuyCount();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetRiderTaskContext", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetRiderTaskContext"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqVersusModeRankOnePacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrVersusModeRankOnePacket"))
						{
							outPacket.WriteHexString("00 FF FF FF FF FF FF FF FF");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqRiderSchoolDataPacket", 0))
					{
						//RiderSchool.PrRiderSchoolData();
						using (OutPacket outPacket = new OutPacket("PrRiderSchoolDataPacket"))
						{
							outPacket.WriteByte(6);//라이센스 등급
							outPacket.WriteByte(36);//마지막 클리어
							outPacket.WriteUShort((ushort)DataTime()[0]);
							outPacket.WriteUShort((ushort)DataTime()[1]);
							outPacket.WriteInt(0);
							outPacket.WriteByte(0);
							RouterListener.MySession.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqRiderSchoolProPacket", 0))
					{
						//RiderSchool.PrRiderSchoolPro();
						int stepcount = 3;
						int remainder = DataTime()[2] % stepcount;
						if (remainder == 0) remainder = stepcount;
						byte step;
						switch (remainder)
						{
							case 1:
								step = 31;
								break;
							case 2:
								step = 33;
								break;
							case 3:
								step = 35;
								break;
							default:
								step = 31;
								break;
						}
						using (OutPacket oPacket = new OutPacket("PrRiderSchoolProPacket"))
						{
							oPacket.WriteByte(1);//엠블럼 체크
							oPacket.WriteByte(step);
							oPacket.WriteByte(6);
							oPacket.WriteByte((byte)((int)step + 1));
							oPacket.WriteInt(0);
							oPacket.WriteInt(0);
							oPacket.WriteInt(0);
							RouterListener.MySession.Client.Send(oPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqStartRiderSchool", 0))
					{
						RiderSchool.PrStartRiderSchool();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqRiderSchoolExpiredCheck", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrRiderSchoolExpiredCheck"))
						{
							outPacket.WriteBytes(new byte[10]);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqRankerInfoPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrRankerInfoPacket"))
						{
							outPacket.WriteByte(0);
							outPacket.WriteByte(SetRider.Ranker);
							outPacket.WriteHexString("00 00 C8 42 00 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqRequestExtradata", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrRequestExtradata"))
						{
							outPacket.WriteShort(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("ChRequestChStaticRequestPacket", 0))
					{
						GameSupport.ChRequestChStaticReplyPacket();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqDynamicCommand", 0))
					{
						GameSupport.PrDynamicCommand();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqPubCommandPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrPubCommandPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqWebEventCompleteCheckPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrWebEventCompleteCheckPacket"))
						{
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqKoinBalance", 0))
					{
						using (OutPacket outPacket = new OutPacket("SpRpKoinBalance"))
						{
							outPacket.WriteUInt(SetRider.Koin);
							outPacket.WriteUInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqFavoriteTrackMapGet", 0))
					{
						FavoriteItem.Favorite_Track();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetFavoriteChannel", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetFavoriteChannel"))
						{
							outPacket.WriteHexString("02 00 00 00 00 00 00 00 00 00 01 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqKartPassInitPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrKartPassInitPacket"))
						{
							outPacket.WriteInt(3);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqGetCashInventoryPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("SpRpGetCashInventoryPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteByte(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqRemainCashPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("SpRpRemainCashPacket"))
						{
							outPacket.WriteUInt(0);
							outPacket.WriteUInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqRemainTcCashPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("SpRpRemainTcCashPacket"))
						{
							outPacket.WriteUInt(99);
							outPacket.WriteUInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpReqNormalShopBuyItemPacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("SpReqItemPresetShopBuyItemPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("SpRepBuyItemPacket"))
						{
							outPacket.WriteHexString("01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetCurrentRid", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetCurrentRid"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetMyCouponList", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetMyCouponList"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqDisassembleFeeInfo", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrDisassembleFeeInfo"))
						{
							outPacket.WriteHexString("00 00 00 00 06 00 00 00 00 00 E8 03 01 00 F4 01 00 00 E8 03 01 00 F4 01 00 00 E8 03 01 00 F4 01");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqRequestExchangeInitPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrRequestExchangeInitPacket"))
						{
							outPacket.WriteHexString("01 03 00 00 00 F4 01 00 00 01 00 00 00 02 00 00 00 03 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqRequestPeriodExchangeInitPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrRequestPeriodExchangeInitPacket"))
						{
							outPacket.WriteBytes(new byte[22]);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqEnterRewardBoxStage", 0))
					{
						using (OutPacket outPacket = new OutPacket("SpRpEnterRewardBoxStage"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteByte(1);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqExitRewardBoxStage", 0))
					{
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqGetGiftListIncomingPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("SpRpGetGiftListIncomingPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqGetGiftListReceivedPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("SpRpGetGiftListReceivedPacket"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetCompetitiveRankInfo", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetCompetitiveRankInfo"))
						{
							outPacket.WriteHexString("01 00 00 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetCompetitiveSlotInfo", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetCompetitiveSlotInfo"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetCompetitiveCount", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetCompetitiveCount"))
						{
							outPacket.WriteHexString("B3 02 52 1B 00 00 B4 02 54 1B 00 00 B9 02 82 1B 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqSearchCompetitiveRankPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrSearchCompetitiveRankPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetCompetitivePreRankPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetCompetitivePreRankPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("RmRequestEmblemsPacket", 0))
					{
						Emblem.RmOwnerEmblemPacket();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("RmRqUpdateMainEmblemPacket", 0))
					{
						SetRider.Emblem1 = iPacket.ReadShort();
						SetRider.Emblem2 = iPacket.ReadShort();
						using (OutPacket outPacket = new OutPacket("RmRpUpdateMainEmblemPacket"))
						{
							outPacket.WriteByte(1);
							outPacket.WriteByte(0);
							this.Parent.Client.Send(outPacket);
						}
						SetGameData.Save_Emblem();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqSyncDictionaryInfoPacket", 0))
					{
						List<List<short>> dictionary = new List<List<short>>();
						XmlDocument Item = new XmlDocument();
						Item.Load(@"Profile\Item.xml");
						if (Item.GetElementsByTagName("Character") != null)
						{
							XmlNodeList lis = Item.GetElementsByTagName("Character");
							foreach (XmlNode xn in lis)
							{
								XmlElement xe = (XmlElement)xn;
								short i = short.Parse(xe.GetAttribute("id"));
								List<short> add = new List<short> { 1, i };
								dictionary.Add(add);
							}
						}
						if (Item.GetElementsByTagName("Paint") != null)
						{
							XmlNodeList lis = Item.GetElementsByTagName("Paint");
							foreach (XmlNode xn in lis)
							{
								XmlElement xe = (XmlElement)xn;
								short i = short.Parse(xe.GetAttribute("id"));
								List<short> add = new List<short> { 2, i };
								dictionary.Add(add);
							}
						}
						if (Item.GetElementsByTagName("Goggle") != null)
						{
							XmlNodeList lis = Item.GetElementsByTagName("Goggle");
							foreach (XmlNode xn in lis)
							{
								XmlElement xe = (XmlElement)xn;
								short i = short.Parse(xe.GetAttribute("id"));
								List<short> add = new List<short> { 8, i };
								dictionary.Add(add);
							}
						}
						if (Item.GetElementsByTagName("Balloon") != null)
						{
							XmlNodeList lis = Item.GetElementsByTagName("Balloon");
							foreach (XmlNode xn in lis)
							{
								XmlElement xe = (XmlElement)xn;
								short i = short.Parse(xe.GetAttribute("id"));
								List<short> add = new List<short> { 9, i };
								dictionary.Add(add);
							}
						}
						if (Item.GetElementsByTagName("HeadBand") != null)
						{
							XmlNodeList lis = Item.GetElementsByTagName("HeadBand");
							foreach (XmlNode xn in lis)
							{
								XmlElement xe = (XmlElement)xn;
								short i = short.Parse(xe.GetAttribute("id"));
								List<short> add = new List<short> { 11, i };
								dictionary.Add(add);
							}
						}
						if (Item.GetElementsByTagName("Pet") != null)
						{
							XmlNodeList lis = Item.GetElementsByTagName("Pet");
							foreach (XmlNode xn in lis)
							{
								XmlElement xe = (XmlElement)xn;
								short i = short.Parse(xe.GetAttribute("id"));
								List<short> add = new List<short> { 21, i };
								dictionary.Add(add);
							}
						}
						if (Item.GetElementsByTagName("Aura") != null)
						{
							XmlNodeList lis = Item.GetElementsByTagName("Aura");
							foreach (XmlNode xn in lis)
							{
								XmlElement xe = (XmlElement)xn;
								short i = short.Parse(xe.GetAttribute("id"));
								List<short> add = new List<short> { 26, i };
								dictionary.Add(add);
							}
						}
						if (Item.GetElementsByTagName("SkidMark") != null)
						{
							XmlNodeList lis = Item.GetElementsByTagName("SkidMark");
							foreach (XmlNode xn in lis)
							{
								XmlElement xe = (XmlElement)xn;
								short i = short.Parse(xe.GetAttribute("id"));
								List<short> add = new List<short> { 27, i };
								dictionary.Add(add);
							}
						}
						if (Item.GetElementsByTagName("FlyingPet") != null)
						{
							XmlNodeList lis = Item.GetElementsByTagName("FlyingPet");
							foreach (XmlNode xn in lis)
							{
								XmlElement xe = (XmlElement)xn;
								short i = short.Parse(xe.GetAttribute("id"));
								List<short> add = new List<short> { 52, i };
								dictionary.Add(add);
							}
						}
						XmlDocument NewKart = new XmlDocument();
						NewKart.Load(@"Profile\NewKart.xml");
						if (!(NewKart.GetElementsByTagName("Kart") == null))
						{
							XmlNodeList lis = NewKart.GetElementsByTagName("Kart");
							foreach (XmlNode xn in lis)
							{
								XmlElement xe = (XmlElement)xn;
								short i = short.Parse(xe.GetAttribute("id"));
								List<short> add = new List<short> { 3, i };
								dictionary.Add(add);
							}
						}
						using (OutPacket outPacket = new OutPacket("PrSyncDictionaryInfoPacket"))
						{
							outPacket.WriteInt(1);
							outPacket.WriteInt(1);
							outPacket.WriteInt(dictionary.Count);
							foreach (var item in dictionary)
							{
								outPacket.WriteShort(item[0]);
								outPacket.WriteShort(item[1]);
							}
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetDictionaryRewardInfoPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetDictionaryRewardInfoPacket"))
						{
							outPacket.WriteShort(56);
							outPacket.WriteInt(1);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqNewCareerListPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrNewCareerListPacket"))
						{
							for (int i = 0; i < 5; i++)
							{
								outPacket.WriteInt(0);
							}
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("LoRqDeleteItemPacket", 0))
					{
						iPacket.ReadInt();
						iPacket.ReadInt();
						//iPacket.ReadInt();
						//iPacket.ReadShort();
						short ItemType = iPacket.ReadShort();
						short ItemID = iPacket.ReadShort();
						short SN = iPacket.ReadShort();
						using (OutPacket outPacket = new OutPacket("LoRpDeleteItemPacket"))
						{
							this.Parent.Client.Send(outPacket);
						}
						if (ItemType == 3)
						{
							XmlDocument doc = new XmlDocument();
							doc.Load(@"Profile\NewKart.xml");
							XmlElement elementToRemove = doc.SelectSingleNode("//Kart[@id='" + ItemID + "' and @sn='" + SN + "']") as XmlElement;
							if (elementToRemove != null)
							{
								elementToRemove.ParentNode.RemoveChild(elementToRemove);
							}
							doc.Save(@"Profile\NewKart.xml");
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqQueryCoupon", 0))
					{
						using (OutPacket outPacket = new OutPacket("SpRpQueryCoupon"))
						{
							outPacket.WriteInt(1);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqShopCashPage", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrShopCashPage"))
						{
							outPacket.WriteString("https://ripay.nexon.com/Payment/Index");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqShopURLPage", 0))
					{
						int URLPageType = iPacket.ReadInt();
						using (OutPacket outPacket = new OutPacket("PrShopURLPage"))
						{
							outPacket.WriteInt(URLPageType);
							outPacket.WriteString("https://pay.tiancity.com/InnerGame/IndexII.aspx");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqBingoSync", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrBingoSync"))
						{
							outPacket.WriteByte(0);
							outPacket.WriteUShort(0);
							outPacket.WriteUShort(0);
							for (int i = 0; i < 15; i++)
							{
								outPacket.WriteByte(0);
							}
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqEnterKartPassPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrEnterKartPassPacket"))
						{
							outPacket.WriteHexString("00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqKartPassPlayTimePacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrKartPassPlayTimePacket"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqKartPassRewardPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrKartPassRewardPacket"))
						{
							outPacket.WriteHexString("00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqEnterSeasonPassPacket", 0))
					{
						byte SeasonPassType = iPacket.ReadByte();
						using (OutPacket outPacket = new OutPacket("PrEnterSeasonPassPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteByte(SeasonPassType);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqSeasonPassRewardPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrSeasonPassRewardPacket"))
						{
							outPacket.WriteHexString("00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqCheckPassword", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrCheckPassword"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqUnLockedItem", 0))
					{
						int useType = iPacket.ReadInt();
						int stringType = iPacket.ReadInt();
						for (int i = 0; i < useType; i++)
						{
							iPacket.ReadString(false);
						}
						byte Type = iPacket.ReadByte();
						using (OutPacket outPacket = new OutPacket("PrUnLockedItem"))
						{
							outPacket.WriteByte(Type);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqFavoriteItemGet", 0)) //즐겨 찾기 목록
					{
						FavoriteItem.Favorite_Item();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqFavoriteItemUpdate", 0))
					{
						iPacket.ReadByte();
						int j = iPacket.ReadShort();
						iPacket.ReadShort();
						for (int i = 0; i < j; i++)
						{
							short item = iPacket.ReadShort();
							short id = iPacket.ReadShort();
							short sn = iPacket.ReadShort();
							byte Add_Del = iPacket.ReadByte();
							if (Add_Del == 1)
							{
								FavoriteItem.Favorite_Item_Add(item, id, sn);
							}
							else if (Add_Del == 2)
							{
								FavoriteItem.Favorite_Item_Del(item, id, sn);
							}
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqLockedItemGet", 0))//아이템 보호
					{
						using (OutPacket outPacket = new OutPacket("PrLockedItemGet"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqLockedItemUpdate", 0))
					{
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqSimGameSimpleInfoAndRankPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrSimGameSimpleInfoAndRankPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqSimGameEnterPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrSimGameEnterPacket"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqExchangeXPartsItem", 0))
					{
						short KartType = iPacket.ReadShort();
						iPacket.ReadShort();
						byte Grade = iPacket.ReadByte();
						Random random = new Random();
						short[] numbers = { 63, 64, 65, 66 };
						int randomIndex = random.Next(0, numbers.Length);
						short randomNumber = numbers[randomIndex];
						using (OutPacket outPacket = new OutPacket("PrExchangeXPartsItem"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(SetRider.SlotChanger);
							outPacket.WriteShort(randomNumber);
							outPacket.WriteShort(KartType);
							outPacket.WriteShort(1);
							outPacket.WriteShort(0);
							outPacket.WriteByte(Grade);
							outPacket.WriteByte(1);
							outPacket.WriteShort(1200);
							outPacket.WriteShort(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqChannelSwitch", 0))
					{
						using (OutPacket outPacket = new OutPacket("ChGetCurrentGpReplyPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteByte(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("ChCreateRoomRequestPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("ChCreateRoomReplyPacket"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqTimeShopPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("SpRpTimeShopPacket"))
						{
							outPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF 47 00 00 00 00 00 47 00 00 00 00 00 00 00 02 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqSecretShopEnterPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrSecretShopEnterPacket"))
						{
							outPacket.WriteHexString("00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqEnterUpgradeGearPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrEnterUpgradeGearPacket"))
						{
							outPacket.WriteHexString("05 00 00 00 03 00 00 00 05 00 00 00 00 00 00 00 00 00 00 00");
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqBlockCatchEnterPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrBlockCatchEnterPacket"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							outPacket.WriteInt(3);
							outPacket.WriteInt(3);
							outPacket.WriteInt(0);
							outPacket.WriteInt(5);
							outPacket.WriteInt(1);
							outPacket.WriteInt(7);
							outPacket.WriteInt(2);
							outPacket.WriteInt(600);
							outPacket.WriteInt(300);
							outPacket.WriteInt(200);
							outPacket.WriteInt(100);
							outPacket.WriteInt(-100);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("RqEnterFishingStagePacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("RpEnterFishingStagePacket"))
						{
							outPacket.WriteByte(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqPcCafeShowcaseCoupon", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrPcCafeShowcaseCoupon"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetRiderCareerSummary", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetRiderCareerSummary"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("checkSecondAuthenPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("checkSecondAuthenPacket"))
						{
							outPacket.WriteInt(2);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqTestServerAddItemPacket", 0))
					{
						TestServer.Type = iPacket.ReadShort();
						TestServer.ItemID = iPacket.ReadShort();
						TestServer.Amount = iPacket.ReadShort();
						TestServer.TestServerAddItem();
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqServerTime", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrServerTime"))
						{
							outPacket.WriteUShort((ushort)DataTime()[0]);
							outPacket.WriteUShort((ushort)DataTime()[1]);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqLogin", 0))
					{
						GameDataReset.DataReset();
						using (OutPacket outPacket = new OutPacket("PrLogin"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteUShort((ushort)DataTime()[0]);
							outPacket.WriteUShort((ushort)DataTime()[1]);
							outPacket.WriteUInt(SetRider.UserNO);
							outPacket.WriteString(SetRider.UserID);
							outPacket.WriteByte(2);
							outPacket.WriteByte(1);
							outPacket.WriteByte(0);
							outPacket.WriteInt(0);
							outPacket.WriteByte(0);
							outPacket.WriteHexString("FF FF 5F 54");
							outPacket.WriteUInt(SetRider.pmap);
							for (int i = 0; i < 11; i++)
							{
								outPacket.WriteInt(0);
							}
							outPacket.WriteByte(0);
							outPacket.WriteEndPoint(IPAddress.Parse(RouterListener.forceConnect), 39311);
							outPacket.WriteEndPoint(IPAddress.Parse(RouterListener.forceConnect), 39312);
							outPacket.WriteInt(0);
							outPacket.WriteString("");
							outPacket.WriteInt(0);
							outPacket.WriteByte(1);
							outPacket.WriteString("content");
							outPacket.WriteInt(0);
							outPacket.WriteInt(1);
							outPacket.WriteString("cc");
							outPacket.WriteString(SessionGroup.Service);
							outPacket.WriteInt(5);
							outPacket.WriteString("content");
							outPacket.WriteInt(0);
							outPacket.WriteInt(2);
							outPacket.WriteString("name");
							outPacket.WriteString("dynamicPpl");
							outPacket.WriteString("enable");
							outPacket.WriteString("false");
							outPacket.WriteInt(1);
							outPacket.WriteString("region");
							outPacket.WriteInt(0);
							outPacket.WriteInt(1);
							outPacket.WriteString("szId");
							outPacket.WriteString(SessionGroup.usLocale.ToString());
							outPacket.WriteInt(0);
							outPacket.WriteString("content");
							outPacket.WriteInt(0);
							outPacket.WriteInt(3);
							outPacket.WriteString("name");
							outPacket.WriteString("endingBanner");
							outPacket.WriteString("enable");
							outPacket.WriteString("false");
							outPacket.WriteString("value");
							outPacket.WriteString("http://popkart.tiancity.com/homepage/endbanner.html");
							outPacket.WriteInt(0);
							outPacket.WriteString("content");
							outPacket.WriteInt(0);
							outPacket.WriteInt(3);
							outPacket.WriteString("name");
							outPacket.WriteString("themeXyy");
							outPacket.WriteString("enable");
							outPacket.WriteString("true");
							outPacket.WriteString("visible");
							outPacket.WriteString("true");
							outPacket.WriteInt(0);
							outPacket.WriteString("content");
							outPacket.WriteInt(0);
							outPacket.WriteInt(3);
							outPacket.WriteString("name");
							outPacket.WriteString("themeKorea");
							outPacket.WriteString("enable");
							outPacket.WriteString("true");
							outPacket.WriteString("visible");
							outPacket.WriteString("true");
							outPacket.WriteInt(0);
							outPacket.WriteString("content");
							outPacket.WriteInt(0);
							outPacket.WriteInt(5);
							outPacket.WriteString("name");
							outPacket.WriteString("timeAttack");
							outPacket.WriteString("enable");
							outPacket.WriteString("true");
							outPacket.WriteString("visible");
							outPacket.WriteString("true");
							outPacket.WriteString("value");
							outPacket.WriteString("village_R01");
							outPacket.WriteString("maxReplayFileCount");
							outPacket.WriteString("250");
							outPacket.WriteInt(0);
							outPacket.WriteByte(0);
							//outPacket.WriteByte(SetGameOption.Set_screen);
							//outPacket.WriteByte(SetRider.IdentificationType);
							RouterListener.MySession.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqSyncJackpotPointCS", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrSyncJackpotPointCS"))
						{
							outPacket.WriteByte(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqEnterShopPacket", 0))
					{
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqGetlotteryMileageInfoPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrGetlotteryMileageInfoPacket"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqTcCashEventUserInfoPacket", 0))
					{
						int unk1 = iPacket.ReadInt();
						using (OutPacket outPacket = new OutPacket("PrTcCashEventUserInfoPacket"))
						{
							outPacket.WriteInt(unk1);
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqServerSideUdpBindCheck", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqMissionAttendUserStatePacket", 0) || hash == Adler32Helper.GenerateAdler32_ASCII("PqMissionAttendNRUserStatePacket", 0))
					{
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqBoomhillExchangeInfo", 0))
					{
						short Type = iPacket.ReadShort();
						iPacket.ReadInt();
						using (OutPacket outPacket = new OutPacket("PrBoomhillExchangeInfo"))
						{
							outPacket.WriteInt();
							outPacket.WriteShort(Type);
							outPacket.WriteInt();
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqBoomhillExchangeNeedNotice", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrBoomhillExchangeNeedNotice"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteByte(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqMixItemExchangeCount", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrMixItemExchangeCount"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("RqBoomhillExchangeKoin", 0))
					{
						using (OutPacket outPacket = new OutPacket("RpBoomhillExchangeKoin"))
						{
							outPacket.WriteInt(0);
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("SpRqUseInitialCardPacket", 0))
					{
						SetRider.Card = iPacket.ReadString();
						SetGameData.Save_Card();
						using (OutPacket outPacket = new OutPacket("SpRpUseInitialCardPacket"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
					else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqQuestUX2ndForShutDownPacket", 0))
					{
						using (OutPacket outPacket = new OutPacket("PrQuestUX2ndForShutDownPacket"))
						{
							outPacket.WriteInt(0);
							this.Parent.Client.Send(outPacket);
						}
						return;
					}
				}
			}
		}
	}
}
