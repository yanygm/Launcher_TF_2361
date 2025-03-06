using System;
using System.Linq;
using ExcData;
using KartRider.IO;
using Set_Data;

namespace KartRider
{
	public static class GameSupport
	{
		public static void PcFirstMessage()
		{
			uint first_val = 3595571486;
			uint second_val = 2168420743;
			using (OutPacket outPacket = new OutPacket("PcFirstMessage"))
			{
				outPacket.WriteUShort(SessionGroup.usLocale);
				outPacket.WriteUShort(1);
				outPacket.WriteUShort(SetGameOption.Version);
				outPacket.WriteString("http://kartupdate.tiancity.cn/patch/ATYGSTKPEWUHSKA");
				outPacket.WriteUInt(first_val);
				outPacket.WriteUInt(second_val);
				outPacket.WriteByte(SessionGroup.nClientLoc);
				outPacket.WriteString("+B1K8NAOvJd3cXFieRWTkRNj2rlv2qVmALSUdXFpNl0=");
				outPacket.WriteHexString("00 00 00 00 00 00 00 00 0F 00 00 00 00 00 00 00 00 2E 31 2E 31 37 2E 36 00 00 00 00 00 00 00");
				outPacket.WriteString("TwKtPFLx+3AuKg5PFa021r3hKyFDK2sFBzQJJCI26wA=");
				RouterListener.MySession.Client.Send(outPacket);
			}
			RouterListener.MySession.Client._RIV = first_val ^ second_val;
			RouterListener.MySession.Client._SIV = first_val ^ second_val;
		}

		public static void OnDisconnect()
		{
			RouterListener.MySession.Client.Disconnect();
		}

		public static void SpRpLotteryPacket()
		{
			using (OutPacket outPacket = new OutPacket("SpRpLotteryPacket"))
			{
				outPacket.WriteHexString("05 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00");
				RouterListener.MySession.Client.Send(outPacket);
			}
		}

		public static void PrGetGameOption()
		{
			using (OutPacket outPacket = new OutPacket("PrGetGameOption"))
			{
				outPacket.WriteFloat(SetGameOption.Set_BGM);
				outPacket.WriteFloat(SetGameOption.Set_Sound);
				outPacket.WriteByte(SetGameOption.Main_BGM);
				outPacket.WriteByte(SetGameOption.Sound_effect);
				outPacket.WriteByte(SetGameOption.Full_screen);
				outPacket.WriteByte(SetGameOption.Unk1);
				outPacket.WriteByte(SetGameOption.Unk2);
				outPacket.WriteByte(SetGameOption.Unk3);
				outPacket.WriteByte(SetGameOption.Unk4);
				outPacket.WriteByte(SetGameOption.Unk5);
				outPacket.WriteByte(SetGameOption.Unk6);
				outPacket.WriteByte(SetGameOption.Unk7);
				outPacket.WriteByte(SetGameOption.Unk8);
				outPacket.WriteByte(SetGameOption.Unk9);
				outPacket.WriteByte(SetGameOption.Unk10);
				outPacket.WriteByte(SetGameOption.Unk11);
				outPacket.WriteByte(SetGameOption.BGM_Check);
				outPacket.WriteByte(SetGameOption.Sound_Check);
				outPacket.WriteByte(SetGameOption.Unk12);
				outPacket.WriteByte(SetGameOption.Unk13);
				outPacket.WriteByte(SetGameOption.GameType);
				outPacket.WriteByte(SetGameOption.SetGhost);
				outPacket.WriteByte(SetGameOption.SpeedType);
				outPacket.WriteByte(SetGameOption.Unk14);
				outPacket.WriteByte(SetGameOption.Unk15);
				outPacket.WriteByte(SetGameOption.Unk16);
				outPacket.WriteByte(SetGameOption.Unk17);
				//outPacket.WriteByte(SetGameOption.Set_screen);
				//outPacket.WriteByte(SetGameOption.Unk18);
				outPacket.WriteBytes(new byte[40]);
				RouterListener.MySession.Client.Send(outPacket);
			}
		}

		public static void ChRpEnterMyRoomPacket()
		{
			if (GameType.EnterMyRoomType == 0)
			{
				using (OutPacket outPacket = new OutPacket("ChRpEnterMyRoomPacket"))
				{
					outPacket.WriteString(SetRider.Nickname);
					outPacket.WriteByte(0);
					outPacket.WriteShort(SetMyRoom.MyRoom);
					outPacket.WriteByte(SetMyRoom.MyRoomBGM);
					outPacket.WriteByte(SetMyRoom.UseRoomPwd);
					outPacket.WriteByte(0);
					outPacket.WriteByte(SetMyRoom.UseItemPwd);
					outPacket.WriteByte(SetMyRoom.TalkLock);
					outPacket.WriteString(SetMyRoom.RoomPwd);
					outPacket.WriteString("");
					outPacket.WriteString(SetMyRoom.ItemPwd);
					outPacket.WriteShort(SetMyRoom.MyRoomKart1);
					outPacket.WriteShort(SetMyRoom.MyRoomKart2);
					RouterListener.MySession.Client.Send(outPacket);
				}
			}
			else
			{
				using (OutPacket outPacket = new OutPacket("ChRpEnterMyRoomPacket"))
				{
					outPacket.WriteString("");
					outPacket.WriteByte(GameType.EnterMyRoomType);
					outPacket.WriteShort(0);
					outPacket.WriteByte(0);
					outPacket.WriteByte(0);
					outPacket.WriteByte(0);
					outPacket.WriteByte(0);
					outPacket.WriteByte(1);
					outPacket.WriteString("");//RoomPwd
					outPacket.WriteString("");
					outPacket.WriteString("");//ItemPwd 
					outPacket.WriteShort(0);
					outPacket.WriteShort(0);
					RouterListener.MySession.Client.Send(outPacket);
				}
			}
		}

		public static void RmNotiMyRoomInfoPacket()
		{
			using (OutPacket outPacket = new OutPacket("RmNotiMyRoomInfoPacket"))
			{
				outPacket.WriteShort(SetMyRoom.MyRoom);
				outPacket.WriteByte(SetMyRoom.MyRoomBGM);
				outPacket.WriteByte(SetMyRoom.UseRoomPwd);
				outPacket.WriteByte(0);
				outPacket.WriteByte(SetMyRoom.UseItemPwd);
				outPacket.WriteByte(SetMyRoom.TalkLock);
				outPacket.WriteString(SetMyRoom.RoomPwd);
				outPacket.WriteString("");
				outPacket.WriteString(SetMyRoom.ItemPwd);
				outPacket.WriteShort(SetMyRoom.MyRoomKart1);
				outPacket.WriteShort(SetMyRoom.MyRoomKart2);
				RouterListener.MySession.Client.Send(outPacket);
			}
		}

		public static void PrCheckMyClubStatePacket()
		{
			using (OutPacket outPacket = new OutPacket("PrCheckMyClubStatePacket"))
			{
				if (SetRider.ClubMark_LOGO == 0)
				{
					outPacket.WriteInt(0);
					outPacket.WriteString("");
					outPacket.WriteInt(0);
					//outPacket.WriteInt(0);
				}
				else
				{
					outPacket.WriteInt(SetRider.ClubCode);
					outPacket.WriteString(SetRider.ClubName);
					outPacket.WriteInt(SetRider.ClubMark_LOGO);
					//outPacket.WriteInt(SetRider.ClubMark_LINE);
				}
				outPacket.WriteShort(5);//Grade
				outPacket.WriteString(SetRider.Nickname);
				outPacket.WriteInt(1);//ClubMember
				outPacket.WriteByte(5);//Level
				RouterListener.MySession.Client.Send(outPacket);
			}
		}

		public static void ChRequestChStaticReplyPacket()
		{
			using (OutPacket outPacket = new OutPacket("ChRequestChStaticReplyPacket"))
			{
				outPacket.WriteHexString("01 14 03 00 00 53 01 FD 63 08 2E D4 0D 00 00 78 DA A5 56 69 53 13 41 10 7D 72 E5 80 EC 66 93 80 04 F1 BE 6F 05 14 51 0E 85 1C C6 2A 2D 0B FC 6E 19 B2 44 8A 0D A1 92 28 FA EF ED 9E 30 64 17 9A 9D 41 2A 55 BB 3B D3 AF 5F 1F E9 99 EE 1A 80 0F 2E 3D BA E8 C1 C7 0F 04 F4 FE 89 9A 5A B5 E8 BD 8F 06 76 91 CA 28 C8 01 6D FA B4 A1 B7 2B A4 C0 8A 43 CC B0 7B A4 F3 85 9E 87 C7 0C DF 14 6B 0B C3 67 43 34 DB 48 DE C8 A2 0D 8E E6 8D 6C 1A 3A 96 8A B8 AE 99 12 29 31 A2 64 46 44 6B B2 74 36 56 EC AB E7 78 36 36 59 7D D0 84 67 C1 34 87 8C 67 C1 35 07 67 D2 98 BA FE 6E 9D 50 3E DC 49 63 FA C2 F0 AC 2B FA 1A 86 78 AE E8 67 18 92 E3 B4 34 F1 8B 16 81 12 4A E6 F3 E9 08 68 C0 58 70 48 B0 4D A5 C9 96 0F 48 DC 26 13 5D 6C D2 FB AF 2A DB C9 31 02 D4 E9 B3 47 BF 80 74 A6 78 63 87 E4 2D A2 0B 48 70 39 A9 36 F8 B3 79 1C C4 74 78 53 BB 5D 94 FF 41 7E EF D0 7B 5F 39 3F 23 D7 42 14 74 25 1D CA B4 86 54 49 69 D6 13 04 65 D2 EA C7 B8 4F 12 86 5D 1D 17 8D B0 E8 9A 1B 5B 41 0C B9 9E B3 28 32 06 DE 70 54 DA 3B CA 72 83 36 3A E4 DB 1F 7C 3F F6 F0 A6 7B 06 60 C0 7E 2B 6F 84 E8 44 DE 36 43 B5 AF 77 66 AC 59 A3 99 BF 5B 34 84 24 D7 FD BD F3 A9 69 37 EF 7B AA FA DA D8 A3 27 97 5B FF FB 24 FB 83 78 98 66 7B 58 14 61 5B 86 90 1F 99 D4 E4 1A 7D 9C B3 B4 F6 24 67 C9 FF 54 BE 0D B6 54 69 37 48 E9 10 CF E4 EA 0D 43 9E 4F 19 59 A2 81 BC 98 32 72 46 15 5E 26 48 21 50 C9 D7 EC 73 83 2D AD 3F 3F 11 3A AA 9F 8F 94 35 7E 41 12 6A CD 57 EC 50 4F DD 7E 1D 12 F2 DD C4 C7 9B 41 4D B1 84 5F DB 2A 68 0B 8B B3 B1 0A F1 B5 FE E6 7F 94 B5 E5 A5 78 E5 40 C1 F8 26 F7 85 4A 7A CB 71 FE 56 AA 5D 82 74 89 A0 4D 02 5F 59 ED 43 36 D4 CD 5D 57 6B 1F EF BC 33 14 BE D2 AA BF C7 EE 2E 8F A8 2E 11 D0 B2 8E 15 27 B4 D8 24 6F B6 95 81 41 12 57 33 22 40 C7 BD C6 25 DA 21 3B 5C 3C EB 04 6B 13 60 2F C2 F0 9E 4D 34 54 A8 3C 36 0D D2 A0 B3 B4 9E 3D 35 5B 9D CE C6 46 4E E8 05 35 D5 03 FC A3 BB A8 A7 14 4A E1 21 AA 26 26 AA EC 0A 5C 51 48 45 EE 6E 51 50 55 EE 6E 51 D0 C7 82 45 9F E4 7F AA C3 27 AC 60 D1 2F 35 F8 93 13 0A A3 44 7F 50 FB 84 69 EE 20 97 60 0E B6 4C 08 A4 60 9E 60 38 5E 0C C1 9C E1 12 03 87 61 93 9F 2A 43 47 60 93 EF 0A 43 47 61 33 78 F0 2D 8F 31 D8 8C 1F DC 3C 91 C0 F9 A6 43 4E 3D 92 38 DF 8C C8 79 43 1A E6 31 90 0F 32 C6 71 72 5A 63 6B 98 40 F8 FC 2E F3 56 06 A6 53 BC C2 30 07 F1 67 79 95 41 2E A4 61 6F 9A 45 59 5C E4 3A 5B 62 0A 0F 17 B9 8B 17 99 22 87 B8 56 33 CF 90 3C E2 1A CE 02 43 FE 01 3B DC 63 FE");
				RouterListener.MySession.Client.Send(outPacket);
			}
		}

		public static void PrDynamicCommand()
		{
			using (OutPacket outPacket = new OutPacket("PrDynamicCommand"))
			{
				outPacket.WriteByte(0);
				outPacket.WriteInt(0);
				RouterListener.MySession.Client.Send(outPacket);
			}
		}

		public static void PrQuestUX2ndPacket()
		{
			using (OutPacket outPacket = new OutPacket("PrQuestUX2ndPacket"))
			{
				outPacket.WriteInt(1);
				outPacket.WriteInt(1);
				outPacket.WriteInt(0);
				RouterListener.MySession.Client.Send(outPacket);
			}
		}

		public static void GetRider(OutPacket outPacket)
		{
			var KartAndSN = new { Kart = SetRiderItem.Set_Kart, SN = SetRiderItem.Set_KartSN };
			outPacket.WriteShort(SetRiderItem.Set_Character);
			outPacket.WriteShort(SetRiderItem.Set_Paint);
			outPacket.WriteShort(SetRiderItem.Set_Kart);
			outPacket.WriteShort(SetRiderItem.Set_Plate);
			outPacket.WriteShort(SetRiderItem.Set_Goggle);
			outPacket.WriteShort(SetRiderItem.Set_Balloon);
			outPacket.WriteShort(0);
			outPacket.WriteShort(SetRiderItem.Set_HeadBand);
			outPacket.WriteShort(SetRiderItem.Set_HeadPhone);
			outPacket.WriteShort(SetRiderItem.Set_HandGearL);
			outPacket.WriteShort(0);
			outPacket.WriteShort(SetRiderItem.Set_Uniform);
			outPacket.WriteShort(SetRiderItem.Set_Decal);
			outPacket.WriteShort(SetRiderItem.Set_Pet);
			outPacket.WriteShort(SetRiderItem.Set_FlyingPet);
			outPacket.WriteShort(SetRiderItem.Set_Aura);
			outPacket.WriteShort(SetRiderItem.Set_SkidMark);
			outPacket.WriteShort(0);
			outPacket.WriteShort(SetRiderItem.Set_RidColor);
			outPacket.WriteShort(SetRiderItem.Set_BonusCard);
			outPacket.WriteShort(0);//bossModeCard
			var existingPlant = KartExcData.PlantList.FirstOrDefault(list => list[0] == KartAndSN.Kart && list[1] == KartAndSN.SN);
			if (existingPlant != null)
			{
				outPacket.WriteShort(existingPlant[3]);
				outPacket.WriteShort(existingPlant[7]);
				outPacket.WriteShort(existingPlant[5]);
				outPacket.WriteShort(existingPlant[9]);
			}
			else
			{
				outPacket.WriteShort(0);
				outPacket.WriteShort(0);
				outPacket.WriteShort(0);
				outPacket.WriteShort(0);
			}
			outPacket.WriteShort(0);
			outPacket.WriteShort(0);
			outPacket.WriteShort(SetRiderItem.Set_Tachometer);
			outPacket.WriteShort(SetRiderItem.Set_Dye);
			outPacket.WriteShort(SetRiderItem.Set_KartSN);
			outPacket.WriteByte(0);
			var existingParts = KartExcData.PartsList.FirstOrDefault(list => list[0] == KartAndSN.Kart && list[1] == KartAndSN.SN);
			if (existingParts != null)
			{
				outPacket.WriteShort(existingParts[14]);
				outPacket.WriteShort(existingParts[15]);
			}
			else
			{
				var existingLevel = KartExcData.LevelList.FirstOrDefault(list => list[0] == KartAndSN.Kart && list[1] == KartAndSN.SN);
				if (existingLevel != null)
				{
					outPacket.WriteShort(7);
					outPacket.WriteShort(0);
				}
				else
				{
					outPacket.WriteShort(0);
					outPacket.WriteShort(0);
				}
			}
		}
	}
}