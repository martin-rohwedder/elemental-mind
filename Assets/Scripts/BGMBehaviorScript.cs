using UnityEngine;
using System.Collections;

public class BGMBehaviorScript : MonoBehaviour {

    public AudioClip[] clips;

    private AudioSource audioSource;
    private int index;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        index = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    if (!audioSource.isPlaying)
        {
            audioSource.clip = clips[index];
            audioSource.Play(44100);

            Debug.Log("Playing clip: " + clips[index].name);

            index++;
            if (index == clips.Length)
            {
                index = 0;
            }

            Debug.Log("New Index: " + index);
        }
	}
}
