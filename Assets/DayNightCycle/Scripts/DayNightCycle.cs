using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

	[HideInInspector]
	public TimeStamp gameStart;
	[HideInInspector]
	public int secondsInADay;
	[HideInInspector]
	public Light sun, moon;
	[HideInInspector]
	public int radius = 500;
	[HideInInspector]
	public bool realTime;
	[HideInInspector]
	public Vector3 offset;
	private Vector3 center;

	void Start () 
	{
		WorldClock.SetUp(realTime, secondsInADay, gameStart);

		center = transform.position;

		sun.transform.LookAt(center);
		moon.transform.LookAt(center);
		sun.transform.position = -UpdatePosition();//Vector3.MoveTowards(sun.transform.position, UpdatePosition(), Time.deltaTime*(360f/(float)dayInSeconds));
		moon.transform.position = -sun.transform.position;

	}
	void OnGUI()
	{
		GUILayout.Label(WorldClock.CurrentTimeNotation);
		GUILayout.Label(WorldClock.CurrentTimeOfDay.ToString());

	}

	void Update () 
	{
		WorldClock.Run();
		sun.transform.LookAt(center);
		moon.transform.LookAt(center);
		sun.transform.position = -UpdatePosition();//Vector3.MoveTowards(sun.transform.position, UpdatePosition(), Time.deltaTime*(360f/(float)dayInSeconds));
		moon.transform.position = -sun.transform.position;


		//Debug.Log(WorldClock.GetCurrentTime);
		//Debug.Log(WorldClock.GetTimeInSeconds);
		
	}

	Vector3 UpdatePosition()
	{
		Vector3 position;

		float angle = (360f/86400)*WorldClock.CurrentTimeInSeconds;
		float z = center.z+radius * Mathf.Sin(angle * Mathf.Deg2Rad);
		float y = center.y+radius * Mathf.Cos(angle * Mathf.Deg2Rad);
		position = new Vector3(center.x, y, z)+offset;
		return position;
	}


}

