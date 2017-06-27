using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatScript : MonoBehaviour {

	public Text statsCount;
	public ButtonScript buttonUp;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateStats(int stats) {
		this.statsCount.text = "" + stats;
	}
}
