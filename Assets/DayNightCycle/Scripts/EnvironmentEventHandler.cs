using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentEventHandler : MonoBehaviour 
{
	public delegate void Run();
	public static event Run runChanges;
	//public List<Run> eventList = new List<Run>();
	//public DayNightCycle cycle;
	[HideInInspector]
	public bool loop;
	[HideInInspector]
	public List<Transition> transitionsQeueu = new List<Transition>();
	TimeStamp current;

	void Start()
	{
		if(loop)
			transitionsQeueu[transitionsQeueu.Count-1].SetUpRenderSettings();
	}
	void Update()
	{
		current = WorldClock.CurrentTime;
		for(int i=0; i<transitionsQeueu.Count; i++)
		{
			if(transitionsQeueu[i].startTime - current == TimeStamp.Zero)
			{
				transitionsQeueu[i].Subscribe();
				if(runChanges.GetInvocationList().Length > 0)
					StartCoroutine("Change", transitionsQeueu[i]);
			}
		}
	}
	IEnumerator Change(Transition t)
	{
		while(current.TimeInSeconds <= t.stopTime.TimeInSeconds)
		{
			runChanges();
			yield return null;
		}
		runChanges -= t.Transit;
//		foreach (var d in runChanges.GetInvocationList())
//			runChanges -= (d as Run);
	}
}
