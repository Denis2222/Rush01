using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(DayNightCycleManager))]
public class ObjectBuilderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		DayNightCycleManager myScript = (DayNightCycleManager)target;
		if(GUILayout.Button("set dawn rotation"))
		{
			myScript.dawnRotation = myScript.sunLight.transform.localRotation;
		}
		if(GUILayout.Button("set twilight rotation"))
		{
			myScript.twilightRotation = myScript.sunLight.transform.localRotation;
		}
		if(GUILayout.Button("set midday rotation"))
		{
			myScript.middayRotation = myScript.sunLight.transform.localRotation;
		}
		if(GUILayout.Button("swap dawn / twilight rotations"))
		{
			Quaternion temp = myScript.twilightRotation;
			myScript.twilightRotation = myScript.dawnRotation;
			myScript.dawnRotation = temp;
		}
	}
}
#endif

public class DayNightCycleManager : MonoBehaviour {

	public Light sunLight;
	public Light moonLight;

	public Quaternion dawnRotation;
	public Quaternion twilightRotation;
	public Quaternion middayRotation;

	public Color normalColor;
	public Color dawnColor;
	public Color twilightColor;
	public Color nightColor;

	public float speed;

	private float _prevAngle = 5;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		sunLight.transform.Rotate (new Vector3 (0, speed * Time.deltaTime, 0));
		moonLight.transform.Rotate (new Vector3 (0, speed * Time.deltaTime, 0));

		if (Quaternion.Angle (sunLight.transform.localRotation, dawnRotation) < 10) {
			if (_prevAngle > Quaternion.Angle (sunLight.transform.localRotation, dawnRotation))
				sunLight.color = Color.Lerp (dawnColor, nightColor, Mathf.Abs (Quaternion.Angle (sunLight.transform.localRotation, dawnRotation)) / 10);
			else
				sunLight.color = Color.Lerp (dawnColor, normalColor, Mathf.Abs (Quaternion.Angle (sunLight.transform.localRotation, dawnRotation)) / 10);
			_prevAngle = Quaternion.Angle (sunLight.transform.localRotation, dawnRotation);
		} else if (Quaternion.Angle (sunLight.transform.localRotation, twilightRotation) < 10) {
			if (_prevAngle < Quaternion.Angle (sunLight.transform.localRotation, twilightRotation))
				sunLight.color = Color.Lerp (twilightColor, nightColor, Mathf.Abs (Quaternion.Angle (sunLight.transform.localRotation, twilightRotation)) / 10);
			else
				sunLight.color = Color.Lerp (twilightColor, normalColor, Mathf.Abs (Quaternion.Angle (sunLight.transform.localRotation, twilightRotation)) / 10);
			_prevAngle = Quaternion.Angle (sunLight.transform.localRotation, twilightRotation);
		} else {
			_prevAngle = 10;
		}
	}
}
