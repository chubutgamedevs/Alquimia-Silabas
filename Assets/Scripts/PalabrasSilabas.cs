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

    public PalabrasDeserializer(string nombre = "silabas")
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

    public List<(string, List<string>)> generarPalabrasTargetRandom(int v)
    {
        List<(string, List<string>)> palabrasAux = new List<(string, List<string>)>();

        for (int i = 0; i < v; i++)
        {
            palabrasAux.Add(nuevaPalabraRandom());
        }

        return palabrasAux;
    }

    public List<PalabraSilabas> generarPalabrasTargetRandomConSilabas(int cantPalabras, int cantSilabas)
    {
        List<PalabraSilabas> palabrasAux = new List<PalabraSilabas>();

        for (int i = 0; i < cantPalabras; i++)
        {
            palabrasAux.Add(nuevaPalabraRandomConSilabas(cantSilabas));
        }

        return palabrasAux;
    }

    public (string, List<string>) nuevaPalabraRandom() //retorna tupla, tupla go brr
    {

        int randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        while (palabrasYSilabas[wordList[randomIndex]].Count == 1)
        {
            randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        }
        return (wordList[randomIndex], palabrasYSilabas[wordList[randomIndex]]);
    }

    public PalabraSilabas nuevaPalabraRandomConSilabas(int cantSilabas)
    {
        int randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        while (palabrasYSilabas[wordList[randomIndex]].Count != cantSilabas)
        {
            randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        }
        return new PalabraSilabas(wordList[randomIndex], palabrasYSilabas[wordList[randomIndex]]);
    }

    public (string, List<string>) nuevaPalabraRandomMaxSilabas(int maxSilabas)
    {
        int randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        while (palabrasYSilabas[wordList[randomIndex]].Count < maxSilabas)
        {
            randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        }
        return (wordList[randomIndex], palabrasYSilabas[wordList[randomIndex]]);
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