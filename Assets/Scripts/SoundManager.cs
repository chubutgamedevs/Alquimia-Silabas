using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

	public AudioClip bonk;
	public AudioClip silabasJoining;
	public AudioClip palabraDescubierta;
	public AudioClip[] explosions;

	// Audio players components.
	public AudioSource EffectsSource;
	public AudioSource MusicSource;
	// Random pitch adjustment range.
	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;

	private bool sonidoActivado = true;

	[SerializeField] AudioClip[] musicas;

    #region eventos
	
	void OnEnable()
	{
		EventManager.onMartilloGolpea += reproducirBonk;
		EventManager.onMartilloGolpea += reproducirExplosion;
		EventManager.onExplosionFinJuego += reproducirExplosion;
		EventManager.onSilabasUnidas += reproducirJoin;
		EventManager.onPalabraFormada += reproducirPalabraDescubierta;
	}

	void OnDisable()
	{
		EventManager.onMartilloGolpea -= reproducirBonk;
		EventManager.onMartilloGolpea -= reproducirExplosion;
		EventManager.onExplosionFinJuego -= reproducirExplosion;
		EventManager.onSilabasUnidas -= reproducirJoin;
		EventManager.onPalabraFormada -= reproducirPalabraDescubierta;
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


	void Start(){
		MusicaNivel();
	}

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

	void reproducirExplosion(Vector3 _)
    {
		int randomIndex = Random.Range(0, explosions.Length);
		Play(explosions[randomIndex]);
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


	public void toggleSonido()
    {
		sonidoActivado = !sonidoActivado;

        if (sonidoActivado)
        {
			activarSonido();
        }
        else
        {
			desactivarSonido();
        }
		
    }
	void desactivarSonido()
    {
		setVolumen(0f);
	}

	void activarSonido()
	{
		setVolumen(1f);
	}

	void setVolumen(float v)
    {
		this.EffectsSource.volume = v;
		this.MusicSource.volume = v;
	}
	
	public void MusicaNivel(){
		if (SceneManager.GetActiveScene().buildIndex > 1){
			Play(musicas[SceneManager.GetActiveScene().buildIndex-2]);
			Debug.Log(musicas[SceneManager.GetActiveScene().buildIndex-2].name);	
		}
	}
	public void cleanUp()
    {
        Destroy(this.gameObject);
    }

}
