using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BotonToggleClickeado : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _untoggled, _toggled;
    [SerializeField] private AudioClip _compressClip, _unCompressClip;
    [SerializeField] private AudioSource _source;

    private bool toggled = false;

    private void Start()
    {
        _source = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _source.PlayOneShot(_compressClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        toggle();
        if (toggled)
        {
            _img.sprite = _toggled;
        }
        else
        {
            _img.sprite = _untoggled;
        }

        _source.PlayOneShot(_unCompressClip);
    }

    void toggle()
    {
        this.toggled = !this.toggled;
    }
    
}
