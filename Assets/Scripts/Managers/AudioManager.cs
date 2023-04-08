using UnityEngine;

public interface IAudioManager
{
    void PlayAudioClip(AudioClip audioClip);

}

public class AudioManager : IAudioManager
{
    private readonly IPrefabCreator _prefabContainer;

    private AudioSource _audioSource;
    
    AudioManager(IPrefabCreator prefabCreator)
    {
        _prefabContainer = prefabCreator;
        GameObject audioSourceContainer = _prefabContainer.InstantiatePrefab(PrefabType.AudioSourceContainer, new Vector3(),true);
        _audioSource = audioSourceContainer.GetComponent<AudioSource>();
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }
    
}

