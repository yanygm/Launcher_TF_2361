using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartRider;
using ExcData;
using Set_Data;

namespace KartRider_SN
{
	public class KartSN_Parts
	{
		public static void KartSN_Data()
		{
			Console.WriteLine("-------------------------------------------------------------");
			if (PartSpec.Item_Cat_Id == 63)
			{
				TransAccelFactor_Count.TransAccelFactor = 0;
				TransAccelFactor_Count.TransAccelFactor = PartSpec.TransAccelFactor;
			}
			else if (PartSpec.Item_Cat_Id == 64)
			{
				SteerConstraint_Count.SteerConstraint = 0;
				SteerConstraint_Count.SteerConstraint = PartSpec.SteerConstraint;
			}
			else if (PartSpec.Item_Cat_Id == 65)
			{
				DriftEscapeForce_Count.DriftEscapeForce = 0;
				DriftEscapeForce_Count.DriftEscapeForce = PartSpec.DriftEscapeForce;
			}
			else if (PartSpec.Item_Cat_Id == 66)
			{
				NormalBoosterTime_Count.NormalBoosterTime = 0;
				NormalBoosterTime_Count.NormalBoosterTime = PartSpec.NormalBoosterTime;
			}
			Console.WriteLine("TransAccelFactor: {0}", TransAccelFactor_Count.TransAccelFactor);
			Console.WriteLine("SteerConstraint: {0}", SteerConstraint_Count.SteerConstraint);
			Console.WriteLine("DriftEscapeForce: {0}", DriftEscapeForce_Count.DriftEscapeForce);
			Console.WriteLine("NormalBoosterTime: {0}", NormalBoosterTime_Count.NormalBoosterTime);
			PartSpec.TransAccelFactor = TransAccelFactor_Count.TransAccelFactor;
			PartSpec.SteerConstraint = SteerConstraint_Count.SteerConstraint;
			PartSpec.DriftEscapeForce = DriftEscapeForce_Count.DriftEscapeForce;
			PartSpec.NormalBoosterTime = NormalBoosterTime_Count.NormalBoosterTime;
			Console.WriteLine("-------------------------------------------------------------");
		}
	}
}