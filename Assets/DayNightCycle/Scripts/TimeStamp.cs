using System;
using UnityEngine;
//using UnityEngine.Internal;

[System.Serializable]
public struct TimeStamp
{
	[Range(0,23)]
	public int hour;
	[Range(0,59)]
	public int minute;
	[Range(0,59)]
	public int second;

	public TimeStamp(int h, int m, int s)
	{
		hour = h;
		minute = m;
		second = s;
	}
	public int TimeInSeconds
	{
		get
		{
			int timeInSec=0;
			timeInSec = hour*3600+minute*60+second;
			return timeInSec;
		}
	}
	public static TimeStamp Zero
	{
		get{return new TimeStamp(0,0,0);}
	}
	public float magnitude
	{
		get
		{
			return Mathf.Sqrt (this.hour * this.hour + this.minute * this.minute + this.second * this.second);
		}
	}
	public override bool Equals (object other)
	{
		if (!(other is TimeStamp))
		{
			return false;
		}
		TimeStamp timeStamp = (TimeStamp)other;
		return this.hour.Equals (timeStamp.hour) && this.minute.Equals (timeStamp.minute) && this.second.Equals (timeStamp.second);
	}
	
	public override int GetHashCode ()
	{
		return this.hour.GetHashCode () ^ this.minute.GetHashCode () << 2 ^ this.second.GetHashCode () >> 2;
	}

	public static TimeStamp operator +(TimeStamp a, TimeStamp b)
	{
		return new TimeStamp(a.hour+b.hour, a.minute+b.minute, a.second+b.second);
	}
	public static TimeStamp operator -(TimeStamp a, TimeStamp b)
	{
		return new TimeStamp(a.hour-b.hour, a.minute-b.minute, a.second-b.second);
	}
	public static TimeStamp operator *(TimeStamp a, TimeStamp b)
	{
		return new TimeStamp(a.hour*b.hour, a.minute*b.minute, a.second*b.second);
	}
	public static TimeStamp operator /(TimeStamp a, TimeStamp b)
	{
		return new TimeStamp(a.hour/b.hour, a.minute/b.minute, a.second/b.second);
	}
	public static bool operator ==(TimeStamp a, TimeStamp b)
	{
		return a.Equals(b);
	}
	public static bool operator !=(TimeStamp a, TimeStamp b)
	{
		return !a.Equals(b);
	}
	public static bool operator <=(TimeStamp a, TimeStamp b)
	{
		if(a.hour < b.hour && a.minute < b.minute && a.second < b.second)
			return true;
		else 
			return false;
	}
	public static bool operator >=(TimeStamp a, TimeStamp b)
	{
		if(a.hour > b.hour && a.minute > b.minute && a.second > b.second)
			return true;
		else 
			return false;
	}

	public string ToString (string format)
	{
		return String.Format ("({0}, {1}, {2})", new object[]
		                           {
			this.hour.ToString (format),
			this.minute.ToString (format),
			this.second.ToString (format)
		});
	}
	public override string ToString ()
	{
		return String.Format ("({0:F1}, {1:F1}, {2:F1})", new object[]
		                           {
			this.hour,
			this.minute,
			this.second
		});
	}
}
