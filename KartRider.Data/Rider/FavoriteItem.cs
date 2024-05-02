using System;
using System.Collections.Generic;
using System.IO;
using KartRider.IO;
using KartRider;
using System.Xml;

namespace RiderData
{
	public static class FavoriteItem
	{
		public static List<List<short>> FavoriteItemList = new List<List<short>>();
		public static List<List<string>> FavoriteTrackList = new List<List<string>>();

		public static void Favorite_Item()
		{
			if (File.Exists(@"Profile\Favorite.xml"))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(@"Profile\Favorite.xml");
				if (!(doc.GetElementsByTagName("Title") == null))
				{
					XmlNodeList lis = doc.GetElementsByTagName("Title");
					FavoriteItemList = new List<List<short>>();
					foreach (XmlNode xn in lis)
					{
						XmlElement xe = (XmlElement)xn;
						short item = short.Parse(xe.GetAttribute("item"));
						short id = short.Parse(xe.GetAttribute("id"));
						short sn = short.Parse(xe.GetAttribute("sn"));
						List<short> AddList = new List<short>();
						AddList.Add(item);
						AddList.Add(id);
						AddList.Add(sn);
						FavoriteItemList.Add(AddList);
					}
					PrFavoriteItemGet(FavoriteItemList);
				}
			}
		}

		public static void Favorite_Item_Add(short item, short id, short sn)
		{
			int Add = -1;
			for (var i = 0; i < FavoriteItemList.Count; i++)
			{
				if (FavoriteItemList[i][0] == item && FavoriteItemList[i][1] == id && FavoriteItemList[i][2] == sn)
				{
					Add = i;
					break;
				}
			}
			if (Add == -1)
			{
				List<short> AddList = new List<short>();
				AddList.Add(item);
				AddList.Add(id);
				AddList.Add(sn);
				FavoriteItemList.Add(AddList);
				Save_ItemList(FavoriteItemList);
			}
		}

		public static void Favorite_Item_Del(short item, short id, short sn)
		{
			int Dell = -1;
			for (var i = 0; i < FavoriteItemList.Count; i++)
			{
				if (FavoriteItemList[i][0] == item && FavoriteItemList[i][1] == id && FavoriteItemList[i][2] == sn)
				{
					Dell = i;
					break;
				}
			}
			if (Dell > -1)
			{
				FavoriteItemList.RemoveAt(Dell);
				Save_ItemList(FavoriteItemList);
			}
		}

		public static void Save_ItemList(List<List<short>> SaveFavorite)
		{
			File.Delete(@"Profile\Favorite.xml");
			XmlTextWriter writer = new XmlTextWriter(@"Profile\Favorite.xml", System.Text.Encoding.UTF8);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument();
			writer.WriteStartElement("Favorite");
			writer.WriteEndElement();
			writer.Close();
			for (var i = 0; i < SaveFavorite.Count; i++)
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(@"Profile\Favorite.xml");
				XmlNode root = xmlDoc.SelectSingleNode("Favorite");
				XmlElement xe1 = xmlDoc.CreateElement("Title");
				xe1.SetAttribute("item", SaveFavorite[i][0].ToString());
				xe1.SetAttribute("id", SaveFavorite[i][1].ToString());
				xe1.SetAttribute("sn", SaveFavorite[i][2].ToString());
				root.AppendChild(xe1);
				xmlDoc.Save(@"Profile\Favorite.xml");
			}
		}

		public static void Favorite_Track()
		{
			if (File.Exists(@"Profile\FavoriteTrack.xml"))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(@"Profile\FavoriteTrack.xml");
				if (!(doc.DocumentElement == null))
				{
					XmlNode rootNode = doc.DocumentElement;
					List<string> Name = new List<string>();
					FavoriteTrackList = new List<List<string>>();
					foreach (XmlNode node in rootNode.ChildNodes)
					{
						XmlElement xe = (XmlElement)node;
						List<string> AddList = new List<string>();
						AddList.Add(node.Name);
						AddList.Add(xe.GetAttribute("track"));
						FavoriteTrackList.Add(AddList);
						if (Name.Count == 0)
						{
							Name.Add(node.Name);
						}
						else
						{
							for (int i = 0; i < Name.Count; i++)
							{
								if (node.Name != Name[i])
								{
									Name.Add(node.Name);
								}
							}
						}
					}
					for (int i = 0; i < Name.Count; i++)
					{
						using (OutPacket outPacket = new OutPacket("PrFavoriteTrackMapGet"))
						{
							if (!(doc.GetElementsByTagName(Name[i]) == null))
							{
								XmlNodeList lis = doc.GetElementsByTagName(Name[i]);
								string theme = Name[i].Replace("theme", "");
								outPacket.WriteInt(1);
								outPacket.WriteInt(int.Parse(theme)); //主题代码
								outPacket.WriteInt(lis.Count); //赛道数量
								foreach (XmlNode xn in lis)
								{
									XmlElement xe = (XmlElement)xn;
									int track = int.Parse(xe.GetAttribute("track"));
									outPacket.WriteShort(short.Parse(theme)); //主题代码
									outPacket.WriteInt(track); //赛道代码
									outPacket.WriteByte(0);
								}
							}
							else
							{
								outPacket.WriteInt(0);
							}
							RouterListener.MySession.Client.Send(outPacket);
						}
					}
				}
				else
				{
					using (OutPacket outPacket = new OutPacket("PrFavoriteTrackMapGet"))
					{
						outPacket.WriteInt(0);
						RouterListener.MySession.Client.Send(outPacket);
					}
				}
			}
			else
			{
				using (OutPacket outPacket = new OutPacket("PrFavoriteTrackMapGet"))
				{
					outPacket.WriteInt(0);
					RouterListener.MySession.Client.Send(outPacket);
				}
			}
		}

		public static void Favorite_Track_Add(short theme, int track)
		{
			int Add = -1;
			for (var i = 0; i < FavoriteTrackList.Count; i++)
			{
				if (FavoriteTrackList[i][0] == "theme" + theme.ToString() && FavoriteTrackList[i][1] == track.ToString())
				{
					Add = i;
					break;
				}
			}
			if (Add == -1)
			{
				List<string> AddList = new List<string>();
				AddList.Add("theme" + theme.ToString());
				AddList.Add(track.ToString());
				FavoriteTrackList.Add(AddList);
				Save_TrackList(FavoriteTrackList);
			}
		}

		public static void Favorite_Track_Del(short theme, int track)
		{
			int Dell = -1;
			for (var i = 0; i < FavoriteTrackList.Count; i++)
			{
				if (FavoriteTrackList[i][0] == "theme" + theme.ToString() && FavoriteTrackList[i][1] == track.ToString())
				{
					Dell = i;
					break;
				}
			}
			if (Dell > -1)
			{
				FavoriteTrackList.RemoveAt(Dell);
				Save_TrackList(FavoriteTrackList);
			}
		}

		public static void Save_TrackList(List<List<string>> SaveFavorite)
		{
			File.Delete(@"Profile\FavoriteTrack.xml");
			XmlTextWriter writer = new XmlTextWriter(@"Profile\FavoriteTrack.xml", System.Text.Encoding.UTF8);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument();
			writer.WriteStartElement("FavoriteTrack");
			writer.WriteEndElement();
			writer.Close();
			for (var i = 0; i < SaveFavorite.Count; i++)
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(@"Profile\FavoriteTrack.xml");
				XmlNode root = xmlDoc.SelectSingleNode("FavoriteTrack");
				XmlElement xe1 = xmlDoc.CreateElement(SaveFavorite[i][0]);
				xe1.SetAttribute("track", SaveFavorite[i][1]);
				root.AppendChild(xe1);
				xmlDoc.Save(@"Profile\FavoriteTrack.xml");
			}
		}

		public static void PrFavoriteItemGet(List<List<short>> Favorite)
		{
			int range = 10;//分批次数
			int times = Favorite.Count / range + (Favorite.Count % range > 0 ? 1 : 0);
			for (int i = 0; i < times; i++)
			{
				var tempList = Favorite.GetRange(i * range, (i + 1) * range > Favorite.Count ? (Favorite.Count - i * range) : range);
				using (OutPacket oPacket = new OutPacket("PrFavoriteItemGet"))
				{
					oPacket.WriteInt(tempList.Count);
					for (int f = 0; f < tempList.Count; f++)
					{
						oPacket.WriteShort(Favorite[f][0]);
						oPacket.WriteShort(Favorite[f][1]);
						oPacket.WriteShort(Favorite[f][2]);
						oPacket.WriteByte(0);
					}
					RouterListener.MySession.Client.Send(oPacket);
				}
			}
		}
	}
}