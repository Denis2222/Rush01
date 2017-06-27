using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ressources : Item {

	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	public override bool use() {
		return false;
	}
}
