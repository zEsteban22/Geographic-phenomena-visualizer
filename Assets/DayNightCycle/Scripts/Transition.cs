using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

[System.Serializable]
public class Transition 
{
	public string transitionName = "";
	public TimeStamp startTime, stopTime;
	public Material skyBox;
	public Color fogColor;
	[Range(0f,1f)]
	public float fogDensity;
	[Range(0f,8f)]
	public float ambientIntensity;
	public AudioClip[] ambientSound;
	private float transitSpeed = 0f;
	private float prevFogDensity, prevAmbientIntensity;
	private Color prevFogColor;
	

	public Transition()
	{

	}
	public float TransitSpeed
	{
		get
		{
			if(transitSpeed >= 1f)
				transitSpeed = 0f;
			TimeStamp time = new TimeStamp();
			time += WorldClock.CurrentTime - startTime;
			float cnt = stopTime.TimeInSeconds-startTime.TimeInSeconds;
			transitSpeed = 1-(1f/cnt)*(cnt - time.TimeInSeconds);

			return transitSpeed;
		}
	}
	public void SetUpRenderSettings()
	{
		if(skyBox != null)
		{

			RenderSettings.skybox.SetTexture("_FrontTex", skyBox.GetTexture("_FrontTex"));
			RenderSettings.skybox.SetTexture("_FrontTex2", skyBox.GetTexture("_FrontTex"));
			RenderSettings.skybox.SetTexture("_BackTex", skyBox.GetTexture("_BackTex"));
			RenderSettings.skybox.SetTexture("_BackTex2", skyBox.GetTexture("_BackTex"));
			RenderSettings.skybox.SetTexture("_LeftTex", skyBox.GetTexture("_LeftTex"));
			RenderSettings.skybox.SetTexture("_LeftTex2", skyBox.GetTexture("_LeftTex"));
			RenderSettings.skybox.SetTexture("_RightTex", skyBox.GetTexture("_RightTex"));
			RenderSettings.skybox.SetTexture("_RightTex2", skyBox.GetTexture("_RightTex"));
			RenderSettings.skybox.SetTexture("_UpTex", skyBox.GetTexture("_UpTex"));
			RenderSettings.skybox.SetTexture("_UpTex2", skyBox.GetTexture("_UpTex"));
			RenderSettings.skybox.SetTexture("_DownTex", skyBox.GetTexture("_DownTex"));
			RenderSettings.skybox.SetTexture("_DownTex2", skyBox.GetTexture("_DownTex"));
		}
//		else 
//			RenderSettings.skybox = null;
		RenderSettings.fogColor = fogColor;
		RenderSettings.fogDensity = fogDensity;
		RenderSettings.ambientIntensity = ambientIntensity;
	}

	public void Subscribe()
	{
		if(skyBox != null)
		{
			if(RenderSettings.skybox == null || RenderSettings.skybox.shader != Shader.Find("Skybox/Blended"))
			{
				RenderSettings.skybox = new Material(Shader.Find("Skybox/Blended"));
				Texture2D tex = new Texture2D(128,128);
				for(int i=0; i<tex.height; i++)
				{
					for(int j=0; j<tex.width; j++)
					{
						tex.SetPixel(i,j,new Color(0f,0f,0f));
					}
				}
				tex.Apply();
				RenderSettings.skybox.SetTexture("_FrontTex", tex);
				RenderSettings.skybox.SetTexture("_BackTex", tex);
				RenderSettings.skybox.SetTexture("_LeftTex", tex);
				RenderSettings.skybox.SetTexture("_RightTex", tex);
				RenderSettings.skybox.SetTexture("_UpTex", tex);
				RenderSettings.skybox.SetTexture("_DownTex", tex);
				RenderSettings.skybox.SetTexture("_FrontTex2", skyBox.GetTexture("_FrontTex"));
				RenderSettings.skybox.SetTexture("_BackTex2", skyBox.GetTexture("_BackTex"));
				RenderSettings.skybox.SetTexture("_LeftTex2", skyBox.GetTexture("_LeftTex"));
				RenderSettings.skybox.SetTexture("_RightTex2", skyBox.GetTexture("_RightTex"));
				RenderSettings.skybox.SetTexture("_UpTex2", skyBox.GetTexture("_UpTex"));
				RenderSettings.skybox.SetTexture("_DownTex2", skyBox.GetTexture("_DownTex"));
			}
			else
			{
				Material prevSkyBox = RenderSettings.skybox;
				RenderSettings.skybox.SetTexture("_FrontTex", prevSkyBox.GetTexture("_FrontTex2"));
				RenderSettings.skybox.SetTexture("_FrontTex2", skyBox.GetTexture("_FrontTex"));
				RenderSettings.skybox.SetTexture("_BackTex", prevSkyBox.GetTexture("_BackTex2"));
				RenderSettings.skybox.SetTexture("_BackTex2", skyBox.GetTexture("_BackTex"));
				RenderSettings.skybox.SetTexture("_LeftTex", prevSkyBox.GetTexture("_LeftTex2"));
				RenderSettings.skybox.SetTexture("_LeftTex2", skyBox.GetTexture("_LeftTex"));
				RenderSettings.skybox.SetTexture("_RightTex", prevSkyBox.GetTexture("_RightTex2"));
				RenderSettings.skybox.SetTexture("_RightTex2", skyBox.GetTexture("_RightTex"));
				RenderSettings.skybox.SetTexture("_UpTex", prevSkyBox.GetTexture("_UpTex2"));
				RenderSettings.skybox.SetTexture("_UpTex2", skyBox.GetTexture("_UpTex"));
				RenderSettings.skybox.SetTexture("_DownTex", prevSkyBox.GetTexture("_DownTex2"));
				RenderSettings.skybox.SetTexture("_DownTex2", skyBox.GetTexture("_DownTex"));
			}
		}
		else if(RenderSettings.skybox == null || RenderSettings.skybox.shader != Shader.Find("Skybox/Procedural"))
		{
			RenderSettings.skybox = new Material(Shader.Find("Skybox/Procedural"));
			//SetUpRenderSettings();
		}

		prevFogColor = RenderSettings.fogColor;
		prevFogDensity = RenderSettings.fogDensity;
		prevAmbientIntensity = RenderSettings.ambientIntensity;
		EnvironmentEventHandler.runChanges += Transit;
	}
	public void UnSubscribe()
	{
		EnvironmentEventHandler.runChanges -= Transit;
	}
//	public T Transit<T>(T start, T end)
//	{
//		T result = new T();
//		result = Mathf.Lerp(start, end, Transitspeed)
//		return result;
//	}
	public void Transit()
	{
		//Debug.Log (TransitSpeed);
		RenderSettings.fogDensity = Mathf.Lerp(prevFogDensity, fogDensity, TransitSpeed);
		RenderSettings.fogColor = Color.Lerp(prevFogColor, fogColor, TransitSpeed);
		RenderSettings.ambientIntensity = Mathf.Lerp(prevAmbientIntensity, ambientIntensity, TransitSpeed);
		RenderSettings.skybox.SetFloat("_Blend", TransitSpeed);
	}
}
