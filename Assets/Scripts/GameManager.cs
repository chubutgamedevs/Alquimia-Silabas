using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool modoRomper=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivarModoRomper()
    {
        EventManager.onModoRomperDesactivado();
    }

    public void DesActivarModoRomper()
    {
        EventManager.onModoRomperActivado();
    }

    public void ToggleModoRomper()
    {
        if (modoRomper)
        {
            DesActivarModoRomper();
        }
        else
        {
            ActivarModoRomper();
        }

        modoRomper = !modoRomper;

    }

}
