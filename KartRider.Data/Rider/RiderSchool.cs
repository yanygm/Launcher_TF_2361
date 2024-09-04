using System;
using KartRider.IO;
using KartRider;
using ExcData;

namespace RiderData
{
	public static class RiderSchool
	{
		public static void PrStartRiderSchool()
		{
			using (OutPacket oPacket = new OutPacket("PrStartRiderSchool"))
			{
				oPacket.WriteByte(1);
				oPacket.WriteEncFloat(1.1f);
				oPacket.WriteEncInt(2000);
				oPacket.WriteEncFloat(1.4f);
				oPacket.WriteEncInt(500);
				oPacket.WriteEncFloat(350f);
				oPacket.WriteEncByte(2);
				oPacket.WriteEncByte(2);
				oPacket.WriteEncByte(1);
				oPacket.WriteEncByte((byte)((true ? 1 : 0)));
				oPacket.WriteEncByte((byte)((false ? 1 : 0)));
				oPacket.WriteEncByte((byte)((true ? 1 : 0)));
				oPacket.WriteEncFloat(100f);
				oPacket.WriteEncFloat(3f);
				oPacket.WriteEncFloat(0.672f + SpeedPatch.DragFactor);
				oPacket.WriteEncFloat(2300f + SpeedPatch.ForwardAccelForce);
				oPacket.WriteEncFloat(1825f);
				oPacket.WriteEncFloat(2070f);
				oPacket.WriteEncFloat(1415f);
				oPacket.WriteEncFloat(10f);
				oPacket.WriteEncFloat(24.65f);
				oPacket.WriteEncFloat(5f);
				oPacket.WriteEncFloat(5f);
				oPacket.WriteEncFloat(0.2f);
				oPacket.WriteEncFloat(0.2f);
				oPacket.WriteEncFloat(0.2f);
				oPacket.WriteEncFloat(4140f + SpeedPatch.DriftEscapeForce);
				oPacket.WriteEncFloat(0.248f + SpeedPatch.CornerDrawFactor);
				oPacket.WriteEncFloat(0.06f);
				oPacket.WriteEncFloat(0.01f);
				oPacket.WriteEncFloat(3740f + SpeedPatch.DriftMaxGauge);
				oPacket.WriteEncFloat(2900f);
				oPacket.WriteEncFloat(3000f);
				oPacket.WriteEncFloat(4250f);
				oPacket.WriteEncFloat(4000f);
				oPacket.WriteEncFloat(3500f);
				oPacket.WriteEncFloat(1.8455f + SpeedPatch.TransAccelFactor);
				oPacket.WriteEncFloat(1.494f + SpeedPatch.BoostAccelFactor);
				oPacket.WriteEncFloat(1000f);
				oPacket.WriteEncFloat(1500f);
				oPacket.WriteEncFloat(2300f + SpeedPatch.StartForwardAccelForceItem);
				oPacket.WriteEncFloat(3723.235f + SpeedPatch.StartForwardAccelForceSpeed);
				oPacket.WriteEncFloat(0.5f);
				oPacket.WriteEncByte((byte)((false ? 1 : 0)));
				oPacket.WriteEncFloat(1.5f);
				oPacket.WriteEncFloat(0.92f);
				oPacket.WriteEncByte((byte)((false ? 1 : 0)));
				oPacket.WriteEncInt(40);
				oPacket.WriteEncInt(60);
				oPacket.WriteEncFloat(1.06f);
				oPacket.WriteEncFloat(100f);
				oPacket.WriteEncByte((byte)((true ? 1 : 0)));
				oPacket.WriteEncByte((byte)((true ? 1 : 0)));
				oPacket.WriteEncByte((byte)((true ? 1 : 0)));
				oPacket.WriteEncByte((byte)((true ? 1 : 0)));
				oPacket.WriteEncByte((byte)((true ? 1 : 0)));
				oPacket.WriteEncByte((byte)((true ? 1 : 0)));
				oPacket.WriteEncFloat(0.02f);
				oPacket.WriteEncFloat(0.02f);
				oPacket.WriteEncFloat(0.2f);
				oPacket.WriteEncFloat(1.25f);
				oPacket.WriteEncInt(0);
				oPacket.WriteEncFloat(0f);
				oPacket.WriteEncFloat(0f);
				oPacket.WriteEncFloat(200f);
				oPacket.WriteEncFloat(50f);
				oPacket.WriteEncByte((byte)((false ? 1 : 0)));
				oPacket.WriteEncInt(0);
				oPacket.WriteEncFloat(200f);
				oPacket.WriteEncFloat(200f);
				oPacket.WriteEncFloat(50f);
				RouterListener.MySession.Client.Send(oPacket);
			}
		}
	}
}