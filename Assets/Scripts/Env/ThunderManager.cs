using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderManager : MonoBehaviour {


    public List<AudioClip> sounds;
	
    private AudioSource source;
    // Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!source.isPlaying && sounds.Count > 0)
        {
            source.clip = sounds[Random.Range(0, sounds.Count)];
            source.Play();
        }
    }
}
