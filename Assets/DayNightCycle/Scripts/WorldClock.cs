using UnityEngine;
using System.Collections;

public static class WorldClock 
{
	static bool realTime;
	static int secondsInDay;
	
	public static int days;
	public static float oneHour;
	public static float oneMinute;
	public static float oneSecond, seconds;
	public static TimeStamp morning, noon, evening, night;
	private static TimeOfDay timeOfDay;
	private static TimeStamp currentTime;
	
	public static void SetUp(bool _realTime, int _secondsInDay)
	{
		currentTime = new TimeStamp (0,0,0);
		realTime = _realTime;
		secondsInDay = _secondsInDay;
		morning = new TimeStamp(8, 0, 0);
		noon = new TimeStamp(12, 0, 0);
		evening = new TimeStamp(19, 0, 0);
		night = new TimeStamp(23, 59, 59); 
	}
	public static void SetUp(bool _realTime, int _secondsInDay, TimeStamp _gameStartTime)
	{
		currentTime = _gameStartTime;
		realTime = _realTime;
		secondsInDay = _secondsInDay;
		morning = new TimeStamp(8, 0, 0);
		noon = new TimeStamp(12, 0, 0);
		evening = new TimeStamp(19, 0, 0);
		night = new TimeStamp(23, 59, 59); 
	}
	public static void SetUp(bool _realTime, int _secondsInDay, TimeStamp _gameStartTime, TimeStamp _morning, TimeStamp _noon, TimeStamp _evening, TimeStamp _night)
	{
		currentTime = _gameStartTime;
		realTime = _realTime;
		secondsInDay = _secondsInDay;
		morning = _morning;
		noon = _noon;
		evening = _evening;
		night = _night;
	}
	public static float delta
	{
		get
		{
			float d = (86400f/secondsInDay)*Time.deltaTime;
			return d; 
		}
	}
	public static void Run()
	{
		if(realTime)
		{
			secondsInDay = 86400;
			currentTime.hour = System.DateTime.Now.Hour;
			currentTime.minute = System.DateTime.Now.Minute;
			currentTime.second = System.DateTime.Now.Second;
			//currentTime.TimeInSeconds = (float)(((int)(System.DateTime.Now.Hour)*60*60)+((int)(System.DateTime.Now.Minute)*60)+(int)System.DateTime.Now.Second);
		}
		else
		{
			oneHour = secondsInDay/24f;
			oneMinute = oneHour/60f;
			oneSecond = oneMinute/60f;

			//currentTime.TimeInSeconds += Time.deltaTime*(86400f/secondsInDay);
			seconds += delta;
			currentTime.second = (int)seconds;
			
//			if(currentTime.TimeInSeconds >= 86400f)
//				currentTime.timeInSeconds = 0f;
			if(currentTime.second >= 59)
			{
				currentTime.second = 0;
				seconds = 0;
				currentTime.minute++;
			}
			if(currentTime.minute >= 59)
			{
				currentTime.minute = 0;
				currentTime.hour++;
			}
			if(currentTime.hour >= 23)
			{
				currentTime.hour = 0;
				days++;
			}
		}
		//Debug.Log(hours+":"+minutes+":"+seconds+" / "+timeInSeconds);
		
	}
	public static float CurrentTimeInSeconds
	{
		get{return currentTime.TimeInSeconds;}
	}
	public static string CurrentTimeNotation
	{
		get{return currentTime.hour+":"+currentTime.minute+":"+currentTime.second;}
	}
	public static TimeStamp CurrentTime
	{
		get
		{
			//currentTime += new TimeStamp((int)hours, gameStartTime.minute+(int)minutes, gameStartTime.second+(int)seconds);
			return currentTime;
		}
	}
	public static TimeOfDay CurrentTimeOfDay
	{
		get
		{
			if(CurrentTime.TimeInSeconds >= morning.TimeInSeconds && CurrentTime.TimeInSeconds < noon.TimeInSeconds)
				timeOfDay = TimeOfDay.Morning;
			else if(CurrentTime.TimeInSeconds >= noon.TimeInSeconds && CurrentTime.TimeInSeconds < evening.TimeInSeconds)
				timeOfDay = TimeOfDay.Noon;
			else if(CurrentTime.TimeInSeconds >= evening.TimeInSeconds && CurrentTime.TimeInSeconds < night.TimeInSeconds)
				timeOfDay = TimeOfDay.Evening;
			else
				timeOfDay = TimeOfDay.Night;
			return timeOfDay;
		}
	}
}
public enum TimeOfDay
{
	Morning,
	Noon,
	Evening,
	Night
}
