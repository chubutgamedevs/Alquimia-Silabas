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
    [SerializeField] private RectTransform _textTransform;
    private Vector2 _textTransformInitialAnchoredPosition;
    [SerializeField] private Vector2 _hardcodedAnchoredTransformOffset = new Vector2(0,-10);

    private void Start()
    {
        _source = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        if(_textTransform != null)
        {
            _textTransformInitialAnchoredPosition = _textTransform.anchoredPosition;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _img.sprite = _pressed;
        _source.PlayOneShot(_compressClip);

        if(_textTransform != null)
        {
            _textTransform.anchoredPosition = _textTransformInitialAnchoredPosition + _hardcodedAnchoredTransformOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        _img.sprite = _default;
        _source.PlayOneShot(_unCompressClip);

        if (_textTransform != null)
        {
            _textTransform.anchoredPosition = _textTransformInitialAnchoredPosition;
        }
    }
    public void IWasClicked(){
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync("Niveles");
        
    }
    public void Nivel1(){
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync(2);
    }
    public void Nivel2(){
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync(3);
    }
    public void Nivel3(){
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync(4);
    }
    public void Nivel4(){
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync(5);
    }
    public void Nivel5(){
        Debug.Log("Clicked");
        SoundManager.Instance.cleanUp();
        SceneManager.LoadSceneAsync(6);
    }
}
