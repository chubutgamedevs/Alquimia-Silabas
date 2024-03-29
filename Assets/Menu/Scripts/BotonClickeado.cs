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
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadScene("Niveles");
        
    }
    public void Nivel1(){
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadScene(2);
    }
    public void Nivel2(){
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadScene(3);
    }
    public void Nivel3(){
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadScene(4);
    }
    public void Nivel4(){
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadScene(5);
    }
    public void Nivel5(){
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadScene(6);
    }
}
