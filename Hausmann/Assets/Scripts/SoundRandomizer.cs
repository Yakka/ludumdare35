using UnityEngine;
using System.Collections;

public class SoundRandomizer : MonoBehaviour {
    [System.Serializable]
    public struct RandomSound {
        public AudioSource source;
        public float probability;
    }

    public RandomSound[] sounds;
    
	void Update () {
	    foreach(RandomSound sound in sounds) {
            if(!sound.source.isPlaying && Random.Range(0f, 1f) < sound.probability * Time.deltaTime) {
                sound.source.Play();
            }
        }
	}
}
