using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance;

    [Header("Audio Settings")]
    [SerializeField] private int poolSize = 10;
    [SerializeField] private AudioSource audioSourcePrefab;

    private Queue<AudioSource> audioPool = new Queue<AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = Instantiate(audioSourcePrefab, transform);
            source.playOnAwake = false;
            audioPool.Enqueue(source);
        }
    }

    AudioSource GetSource()
    {
        if (audioPool.Count > 0)
        {
            return audioPool.Dequeue();
        }
        else
        {
            AudioSource source = Instantiate(audioSourcePrefab, transform);
            source.playOnAwake = false;
            return source;
        }
    }

    void ReturnSource(AudioSource source)
    {
        audioPool.Enqueue(source);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetSource();
        source.clip = clip;
        source.volume = volume;

        source.Play();
        StartCoroutine(ReturnToPool(source, clip.length));
    }

    public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetSource();
        source.transform.position = position;

        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1f; // 3D sound

        source.Play();
        StartCoroutine(ReturnToPool(source, clip.length));
    }

    System.Collections.IEnumerator ReturnToPool(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);

        source.Stop();
        source.clip = null;
        source.spatialBlend = 0f;

        ReturnSource(source);
    }
}
