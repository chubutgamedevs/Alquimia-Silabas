using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BotonClickeado : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default, _pressed;
    [SerializeField] private AudioClip _compressClip, _unCompressClip;
    [SerializeField] private AudioSource _source;

    private void Start()
    {
        _source = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _img.sprite = _pressed;
        _source.PlayOneShot(_compressClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        _img.sprite = _default;
        _source.PlayOneShot(_unCompressClip);
    }
    public void IWasClicked(){
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync("Niveles");
        
    }
    public void Nivel1(){
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync(2);
    }
    public void Nivel2(){
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync(3);
    }
    public void Nivel3(){
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync(4);
    }
    public void Nivel4(){
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync(5);
    }
    public void Nivel5(){
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync(6);
    }
    public void Menu(){
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync("Menu");
    }
    public void Creditos(){
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync("Creditos");
    }
}
