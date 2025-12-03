using System;
using Hertzole.UnityToolbox;
using UnityEngine;

public class MinMaxValues : MonoBehaviour
{
	public Vector2 vector;
	public MinMaxByte bytes;
	public MinMaxSByte sbytes;
	public MinMaxShort shorts;
	public MinMaxUShort ushorts;
	public MinMaxInt ints;
	public MinMaxUInt uints;
	public MinMaxLong longs;
	public MinMaxLong ulongs;
	public MinMaxFloat floats;
	public MinMaxDouble doubles;

	private void Awake()
	{
		double a = 0;
		byte b = 0;

		// b = a;

		(byte, byte) mA = default;
		MinMaxInt mB = default;

        mB = mA;
	}
}