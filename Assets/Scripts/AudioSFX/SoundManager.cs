using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // AudioSource for playing background music
    public AudioSource bgmSource;

    // AudioSource for playing sound effects
    public AudioSource sfxSource;

    // AudioClip for walk sound
    public AudioClip walkSoundClip;

    // AudioClip for UI sound
    public AudioClip uiSoundClip;

    public AudioClip refreshClip;

    // Array of background music clips
    public AudioClip[] bgmClips;


    // Current index of background music
    private int currentBgmIndex = 0;


    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
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

    // Function to check if an SFX is currently playing
    public bool IsPlayingSFX()
    {
        return sfxSource.isPlaying;
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

}
