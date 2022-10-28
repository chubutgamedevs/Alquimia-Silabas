using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PalabrasDeserializer
{
    //sacados de palabras.json
    public Dictionary<string, List<string>> palabrasYSilabas;
    //lista de palabras
    List<string> wordList;

    public PalabrasDeserializer(string nombre = "json/silabas") //por defecto obtiene todas las palabras
    {
        cargarPalabrasYSilabas(nombre);
    }

    #region json handling
    private void cargarPalabrasYSilabas(string nombre)
    {
        TextAsset json = Resources.Load<TextAsset>(nombre);
        Palabras palabras = JsonUtility.FromJson<Palabras>(json.ToString());
        palabrasYSilabas = palabras.palabras.ToDictionary(x => x.palabra, x => x.silabas);
        wordList = new List<string>(palabrasYSilabas.Keys);
    }
    #endregion


    public List<PalabraSilabas> generarPalabrasTargetRandom(int cantPalabras)
    {
        List<PalabraSilabas> palabrasAux = new List<PalabraSilabas>();

        for (int i = 0; i < cantPalabras; i++)
        {
            PalabraSilabas obtenida = nuevaPalabraRandom();
            if(obtenida != null)
            {
                palabrasAux.Add(obtenida);
            }
            else
            {
                EventManager.onGanaste();
                break;
            }
        }

        return palabrasAux;
    }


    public PalabraSilabas nuevaPalabraRandom()
    {
        if(wordList.Count == 0)
        {
            return null;
        }
        int randomIndex = UnityEngine.Random.Range(0, wordList.Count);

        PalabraSilabas seleccionada = new PalabraSilabas(wordList[randomIndex], palabrasYSilabas[wordList[randomIndex]]);

        //vamos achicando la cantidad de silabas
        palabrasYSilabas.Remove(wordList[randomIndex]);
        wordList.RemoveAt(randomIndex);

        return seleccionada;
    }

    public List<PalabraSilabas> generarPalabrasTargetEnOrden(int cantPalabras)
    {
        List<PalabraSilabas> palabrasAux = new List<PalabraSilabas>();

        for (int i = 0; i < cantPalabras; i++)
        {
            PalabraSilabas obtenida = nuevaPalabraEnOrden();
            if (obtenida != null)
            {
                palabrasAux.Add(obtenida);
            }
            else
            {
                EventManager.onGanaste();
                break;
            }
        }

        return palabrasAux;
    }


    public PalabraSilabas nuevaPalabraEnOrden()
    {
        if (wordList.Count == 0)
        {
            return null;
        }

        PalabraSilabas seleccionada = new PalabraSilabas(wordList[0], palabrasYSilabas[wordList[0]]);

        //vamos achicando la cantidad de silabas
        palabrasYSilabas.Remove(wordList[0]);
        wordList.RemoveAt(0);

        return seleccionada;
    }


}

[Serializable]
public class Palabras
{
    public List<PalabraSilabas> palabras;
}

[Serializable]
public class PalabraSilabas
{
    public string palabra;
    public List<string> silabas = new List<string>();

    public PalabraSilabas(string palabra, List<string> silabas)
    {
        this.palabra = palabra;
        this.silabas = silabas;
    }

    public bool contieneSilaba(string silaba)
    {
        return silabas.Contains(silaba.ToLower());
    }
}