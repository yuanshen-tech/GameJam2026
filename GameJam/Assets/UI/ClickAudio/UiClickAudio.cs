using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class UiClickAudio : MonoBehaviour
{
    private static UiClickAudio player;
    private AudioSource source;
    private bool quitting;

    private static UiClickAudio Player
    {
        get
        {
            if (player == null)
            {
                var host = new GameObject("UI Click Audio");
                DontDestroyOnLoad(host);
                player = host.AddComponent<UiClickAudio>();
            }
            return player;
        }
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.ignoreListenerPause = true;
        source.volume = 0.8f;
    }

    public static void Play(AudioClip clip)
    {
        if (clip == null || Player.quitting)
            return;
        player.source.PlayOneShot(clip);
    }

    public static void QuitAfterClick(AudioClip clip)
    {
        var audio = Player;
        if (audio.quitting)
            return;
        Play(clip);
        audio.quitting = true;
        audio.StartCoroutine(audio.FinishQuit(clip));
    }

    private IEnumerator FinishQuit(AudioClip clip)
    {
        if (clip != null)
            yield return new WaitForSecondsRealtime(clip.length);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
