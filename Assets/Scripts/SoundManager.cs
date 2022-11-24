using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

	public AudioClip bonk;
	public AudioClip silabasJoining;
	public AudioClip palabraDescubierta;

	// Audio players components.
	public AudioSource EffectsSource;
	public AudioSource MusicSource;
	// Random pitch adjustment range.
	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;

    #region eventos
	
	void OnEnable()
	{
		EventManager.martilloGolpea += reproducirBonk;
		EventManager.silabasUnidas += reproducirJoin;
		EventManager.palabraFormada += reproducirPalabraDescubierta;
	}

	void OnDisable()
	{
		EventManager.martilloGolpea -= reproducirBonk;
		EventManager.silabasUnidas -= reproducirJoin;
		EventManager.palabraFormada -= reproducirPalabraDescubierta;
	}

    #endregion

    #region singleton
    // Singleton instance.
    public static SoundManager Instance = null;

	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null)
		{
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);
	}
    #endregion

	// Play a single clip through the sound effects source.
	public void Play(AudioClip clip)
	{
		EffectsSource.PlayOneShot(clip);
	}
	// Play a single clip through the music source.
	public void PlayMusic(AudioClip clip)
	{
		MusicSource.clip = clip;
		MusicSource.Play();
	}

	void reproducirBonk(Vector3 _unused)
    {
        reproducirBonk();
    }

    void reproducirBonk()
    {
		Play(bonk);
    }

	void reproducirJoin(SilabaController _unused, SilabaController _unused2)
    {
		reproducirJoin();
    }
	void reproducirJoin()
    {
		Play(silabasJoining);
    }

	void reproducirPalabraDescubierta(PalabraController _unused, string _unused2)
	{
		Play(palabraDescubierta);
	}

}
