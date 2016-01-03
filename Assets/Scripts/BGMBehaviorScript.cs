using UnityEngine;

public class BGMBehaviorScript : MonoBehaviour {

    public AudioClip[] clips;

    private AudioSource audioSource;
    private int index;

	// Use this for initialization
	void Start () {
        
        audioSource = GetComponent<AudioSource>();
        index = 0;
        Shuffle(clips);
	}
	
	// Update is called once per frame
	void Update () {
	    if (!audioSource.isPlaying)
        {
            audioSource.clip = clips[index];
            audioSource.PlayDelayed(1);

            Debug.Log("Playing clip: " + clips[index].name);

            index++;
            if (index == clips.Length)
            {
                index = 0;
            }

            Debug.Log("New Index: " + index);
        }
	}

    /// <summary>
    /// Shuffle the array.
    /// </summary>
    /// <typeparam name="T">Array element type.</typeparam>
    /// <param name="array">Array to shuffle.</param>
    static void Shuffle<T>(T[] array)
    {
        System.Random random = new System.Random();
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            int r = i + (int)(random.NextDouble() * (n - i));
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }
}
