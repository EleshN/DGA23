using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // AudioSource for playing background music
    public AudioSource bgmSource;

    // AudioSource for playing sound effects
    public AudioSource sfxSource;

    // Array of background music clips
    public AudioClip[] bgmClips;

    // Current index of background music
    private int currentBgmIndex = 0;

    // Adjusts the master volume (affects all sounds)
    public float masterVolume
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = Mathf.Clamp(value, 0f, 1f); }
    }

    void Start()
    {
        // Start playing background music
        StartCoroutine(BackgroundMusicPlayer());
    }

    // Coroutine to cycle through the playlist of background sounds
    private IEnumerator BackgroundMusicPlayer()
    {
        while (true)
        {
            // Play the current background music clip
            bgmSource.clip = bgmClips[currentBgmIndex];
            bgmSource.Play();

            // Wait for the current clip to finish before moving to the next one
            yield return new WaitForSeconds(bgmSource.clip.length);

            // Move to the next clip in the playlist
            currentBgmIndex = (currentBgmIndex + 1) % bgmClips.Length;
        }
    }

    // Function to play a sound effect
    // This can be called from other scripts
    public void PlaySFX(AudioClip clip)
    {
        // sfxSource.PlayOneShot(clip);
    }
}
