using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class DayNightEditor : EditorWindow 
{
	static DayNightEditor window;
	static GameObject envManager;
	DayNightCycle dNC;
	EnvironmentEventHandler eEH;
	static Light sun, moon;
	Transition trans;
	Vector2 scroll, scroll1;
	GameObject cam;

	[MenuItem ("GameEngines2/Add DayNightSystem %#d")]
	static void Init ()
	{
		window = EditorWindow.GetWindow<DayNightEditor>();
		window.minSize = new Vector2(500,500);
		GUIContent gc = new GUIContent();
		gc.text = "Day/Night Cycle";
		window.titleContent = gc;
		window.Show();

	}
	void OnFocus()
	{
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		FlareLayer f = cam.GetComponent<FlareLayer>();
		if(f == null)
		{
			cam.AddComponent<FlareLayer>();
		}
		envManager = GameObject.Find("EnvironmentManager");
		GameObject g = GameObject.Find("Directional Light");
		if(g == null)
			g = GameObject.Find("Sun");
		else
			g.name = "Sun";
		if(g != null)
			sun = g.GetComponent<Light>();
		g = GameObject.Find("Moon");
		if(g != null)
			moon = g.GetComponent<Light>();
		if(envManager == null)
		{
			envManager = new GameObject("EnvironmentManager");
			dNC = envManager.AddComponent<DayNightCycle>();
			eEH = envManager.AddComponent<EnvironmentEventHandler>();
		}
		else
		{
			eEH = envManager.GetComponent<EnvironmentEventHandler>();
			dNC = envManager.GetComponent<DayNightCycle>();
		}
	}
	void OnGUI()
	{
		//Debug.Log(dNC.name);
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		SunMoon ();
		DayNight();
		GUILayout.EndVertical();
		EnvironmentHandler();
		GUILayout.EndHorizontal();

	}
	void SunMoon()
	{
		GUILayout.BeginHorizontal();
		if(sun == null)
		{
			if(GUILayout.Button("New Sun"))
			{
				GameObject obj = new GameObject("Sun", typeof(Light));
				sun = obj.GetComponent<Light>();
			}
		}
		else if(sun.transform.eulerAngles.y == 0)
			sun.transform.eulerAngles += new Vector3(0,90,0);
		if(moon == null)
		{
			if(GUILayout.Button("New Moon"))
			{
				GameObject obj = new GameObject("Moon", typeof(Light));
				moon = obj.GetComponent<Light>();
			}
		}
		else
			moon.transform.eulerAngles = -sun.transform.eulerAngles;
		if(sun != null)
		{
			GUILayout.BeginVertical("Box", GUILayout.MaxWidth(200));
			GUILayout.Label ("SUN Properties");
			LightProperties(sun);
			GUILayout.EndVertical();
		}
		if(moon != null)
		{
			GUILayout.BeginVertical("Box", GUILayout.MaxWidth(200));
			GUILayout.Label ("MOON Properties");
			LightProperties(moon);
			GUILayout.EndVertical();
		}
		GUILayout.EndHorizontal();
		
	}
	void LightProperties(Light l)
	{
		GUILayout.Label ("Light Type");
		l.type = (LightType)EditorGUILayout.EnumPopup(l.type);
		GUILayout.Label ("Color");
		l.color = EditorGUILayout.ColorField(l.color);
		GUILayout.Label ("Intesnity");
		l.intensity = EditorGUILayout.FloatField(l.intensity);
		GUILayout.Label ("Shadow");
		l.shadows = (LightShadows)EditorGUILayout.EnumPopup(l.shadows);
		GUILayout.Label ("Flare");
		sun.flare = (Flare)EditorGUILayout.ObjectField(sun.flare, typeof(Flare));
	}
	void DayNight()
	{
		if(envManager != null)
			dNC = envManager.GetComponent<DayNightCycle>();
		
		if(dNC != null)
		{
			GUILayout.BeginVertical("Box", GUILayout.MaxWidth(400));

			GUILayout.Label("DAY NIGHT CYCLE Properties");
			scroll = EditorGUILayout.BeginScrollView(scroll);
			
			GUILayout.BeginHorizontal("Box");
			GUILayout.Label("Game Start");
			
			GUILayout.BeginVertical();
			GUILayout.Label("Hour: "+dNC.gameStart.hour.ToString());
			dNC.gameStart.hour = (int)GUILayout.HorizontalSlider(dNC.gameStart.hour, 0, 23);
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			GUILayout.Label("Minute: "+dNC.gameStart.minute.ToString());
			dNC.gameStart.minute = (int)GUILayout.HorizontalSlider(dNC.gameStart.minute, 0, 59);
			GUILayout.EndVertical();

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal("Box");
			GUILayout.Label("Seconds in 1 day");
			dNC.secondsInADay = EditorGUILayout.IntField(dNC.secondsInADay);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal("Box");
			
			GUILayout.BeginVertical();
			GUILayout.Label("Sun");
			dNC.sun = (Light)EditorGUILayout.ObjectField(sun, typeof(Light), true);
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			GUILayout.Label("Moon");
			dNC.moon = (Light)EditorGUILayout.ObjectField(moon, typeof(Light), true);
			GUILayout.EndVertical();

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal("Box");
			GUILayout.Label("Sun Height");
			dNC.radius = EditorGUILayout.IntField(dNC.radius);
			GUILayout.EndHorizontal();
			
			GUILayout.Label("Real Time");
			dNC.realTime = EditorGUILayout.Toggle(dNC.realTime);
			EditorGUILayout.EndScrollView();
			
			GUILayout.EndVertical();
		}
	}
	void EnvironmentHandler()
	{
		if(envManager != null)
			eEH = envManager.GetComponent<EnvironmentEventHandler>();

		if(eEH != null)
		{
			EditorUtility.SetDirty(eEH);
			GUILayout.BeginVertical("Box", GUILayout.MaxWidth(400));
			GUILayout.Label("ENVIRONMENTAL TRANSITIONS Properties");
			eEH.loop = GUILayout.Toggle(eEH.loop,new GUIContent("Set to loop", "This will set rendersettings to the settings of last transition in the list"));
			scroll1 = EditorGUILayout.BeginScrollView(scroll1);
			for(int i=0; i<eEH.transitionsQeueu.Count; i++)
			{
				GUILayout.BeginVertical("Box");

				GUILayout.BeginHorizontal();

				GUILayout.BeginVertical(GUILayout.MaxWidth(200));
				GUILayout.BeginVertical("Box");
				GUILayout.Label("Transition Name");
				eEH.transitionsQeueu[i].transitionName = GUILayout.TextField(eEH.transitionsQeueu[i].transitionName);
				GUILayout.EndVertical();

				GUILayout.BeginVertical("Box");
				GUILayout.Label("Transition Start");
				GUILayout.Label("Hour: "+eEH.transitionsQeueu[i].startTime.hour.ToString());
				eEH.transitionsQeueu[i].startTime.hour = (int)GUILayout.HorizontalSlider(eEH.transitionsQeueu[i].startTime.hour, 0, 23);
				GUILayout.Label("Minute: "+eEH.transitionsQeueu[i].startTime.minute.ToString());
				eEH.transitionsQeueu[i].startTime.minute = (int)GUILayout.HorizontalSlider(eEH.transitionsQeueu[i].startTime.minute, 0, 59);
				GUILayout.Label("Transition End");
				GUILayout.Label("Hour: "+eEH.transitionsQeueu[i].stopTime.hour.ToString());
				eEH.transitionsQeueu[i].stopTime.hour = (int)GUILayout.HorizontalSlider(eEH.transitionsQeueu[i].stopTime.hour, 0, 23);
				GUILayout.Label("Minute: "+eEH.transitionsQeueu[i].stopTime.minute.ToString());
				eEH.transitionsQeueu[i].stopTime.minute = (int)GUILayout.HorizontalSlider(eEH.transitionsQeueu[i].stopTime.minute, 0, 59);
				GUILayout.EndVertical();

				GUILayout.EndVertical();

				GUILayout.BeginVertical();
				GUILayout.BeginVertical("Box");
				GUILayout.Label("Skybox");
				eEH.transitionsQeueu[i].skyBox = (Material)EditorGUILayout.ObjectField(eEH.transitionsQeueu[i].skyBox, typeof(Material));
				GUILayout.EndVertical();

				GUILayout.BeginVertical("Box");
				GUILayout.Label("Fog Color");
				eEH.transitionsQeueu[i].fogColor = EditorGUILayout.ColorField(eEH.transitionsQeueu[i].fogColor);
				GUILayout.EndVertical();

				GUILayout.BeginVertical("Box");
				GUILayout.Label("Fog Density: "+eEH.transitionsQeueu[i].fogDensity);
				eEH.transitionsQeueu[i].fogDensity = GUILayout.HorizontalSlider(eEH.transitionsQeueu[i].fogDensity, 0f, 1f);
				GUILayout.EndVertical();

				GUILayout.BeginVertical("Box");
				GUILayout.Label("Ambient Intensity: "+eEH.transitionsQeueu[i].ambientIntensity);
				eEH.transitionsQeueu[i].ambientIntensity = GUILayout.HorizontalSlider(eEH.transitionsQeueu[i].ambientIntensity, 0f, 8f);
				GUILayout.EndVertical();
				if(GUILayout.Button("Remove"))
				{
					eEH.transitionsQeueu.Remove(eEH.transitionsQeueu[i]);
					EditorUtility.SetDirty(eEH);
					return;
				}
				GUILayout.EndVertical();

				GUILayout.EndHorizontal();

				GUILayout.EndVertical();
			}
			if(GUILayout.Button("new Transition"))
			{
				trans = new Transition();
				eEH.transitionsQeueu.Add(trans);
				EditorUtility.SetDirty(eEH);
			}
			EditorGUILayout.EndScrollView();
			GUILayout.EndVertical();
		}
	}
}
