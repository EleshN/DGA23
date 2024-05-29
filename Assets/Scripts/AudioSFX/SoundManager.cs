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

    int clipIndex = 0;


    // Current index of background music
    //private int currentBgmIndex = 0;


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
        if(bgmClips.Length > 1)
        {
            bgmSource.loop = false;
            StartCoroutine(BackgroundMusicPlayer());
        }
        else
        {
            bgmSource.loop = true;
            bgmSource.clip = bgmClips[0];
            bgmSource.Play();
        }
    }

    // Function to check if an SFX is currently playing
    public bool IsPlayingSFX()
    {
        return sfxSource.isPlaying;
    }

    // Coroutine to cycle through the playlist of background sounds
    IEnumerator BackgroundMusicPlayer()
    {
        while (true) {
            bgmSource.clip = bgmClips[clipIndex];
            bgmSource.Play();
            Debug.Log("Play Clip");
            yield return new WaitForSeconds(2);
            Debug.Log("Play Next Clip");
            clipIndex++;
            if (clipIndex >= bgmClips.Length)
            {
                clipIndex = 0;
            }
        }
    }

}
