using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PalabrasDeserializer
{
    Batches batchesLevel;
    Batch batchActual;
    public PalabrasDeserializer(string nombre = "json/silabas") //por defecto obtiene todas las palabras
    {
        cargarPalabrasYSilabas(nombre);
    }

    #region json handling
    private void cargarPalabrasYSilabas(string nombre)
    {
        TextAsset json = Resources.Load<TextAsset>(nombre);
        batchesLevel = JsonUtility.FromJson<Batches>(json.ToString());

        batchActual = batchesLevel.batches[0];
    }
    #endregion


    public List<PalabraSilabas> generarPalabrasTargetRandom(int cantPalabras)
    {
        List<PalabraSilabas> palabrasAux = new List<PalabraSilabas>();

        for (int i = 0; i < cantPalabras; i++)
        {
            PalabraSilabas obtenida = nuevaPalabraRandom();
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

    public List<PalabraSilabas> getNuevasPalabrasTarget()
    {
        batchActual = batchesLevel.nuevoBatch();

        return getPalabrasTarget();
    }
    public List<PalabraSilabas> getPalabrasTarget()
    {
        return batchActual.palabras.ToList();
    }

    public List<string> getPoolActual()
    {
        return batchActual.silabas;
    }

    public List<string> getPoolParcialActual(int cantPalabras)
    {
        return batchActual.getSilabasDePalabras(cantPalabras);
    }


    public PalabraSilabas nuevaPalabraRandom()
    {
        if(batchesLevel.batches.Count == 0 || batchActual.palabras.Count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, batchActual.palabras.Count);

        PalabraSilabas seleccionada = new PalabraSilabas(batchActual.palabras[randomIndex].palabra, batchActual.palabras[randomIndex].silabas);

        //vamos achicando la cantidad de palabras
        batchActual.palabras.RemoveAt(randomIndex);

        if(batchActual.palabras.Count == 0)
        {
            batchActual = batchesLevel.nuevoBatch();
        }

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
        if (batchesLevel.batches.Count == 0 || batchActual.palabras.Count == 0)
        {
            return null;
        }

        PalabraSilabas seleccionada = new PalabraSilabas(batchActual.palabras[0].palabra, batchActual.palabras[0].silabas);

        //vamos achicando la cantidad de palabras
        batchActual.palabras.RemoveAt(0);

        return seleccionada;
    }


}

[Serializable]
public class Batches
{
    public List<Batch> batches;

    public Batch nuevoBatch()
    {
        Batch aRetornar = batches[0];
        batches.RemoveAt(0);
        return aRetornar;
    }

}

[Serializable]
public class Batch
{
    public List<string> silabas;
    public List<PalabraSilabas> palabras;

    public List<string> getSilabasDePalabras(int cantPalabras)
    {
        List<string> silabasADevolver = new List<string>();

        if (palabras.Count < cantPalabras)
        {
            cantPalabras = palabras.Count;
        }

        List<PalabraSilabas> palabrasAEliminar = new List<PalabraSilabas>();

        for(int i = 0; i < cantPalabras; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, cantPalabras);

            palabrasAEliminar.Add(palabras[randomIndex]);
            silabasADevolver.AddRange(palabras[randomIndex].silabas);
        }

        foreach(PalabraSilabas pal in palabrasAEliminar)
        {
            palabras.Remove(pal);
        }

        return silabasADevolver;
    }
}


[Serializable]
public class PalabraSilabas
{
    public string palabra;
    public List<string> silabas = new List<string>();

    public PalabraSilabas(string palabra, List<string> silabasRecibidas)
    {
        this.palabra = palabra;
        silabasRecibidas.ForEach(x => x.ToLower().Trim()) ;
        silabas = silabasRecibidas;
    }

    public bool contieneSilaba(string silaba)
    {
        string silAux = silaba.ToLower().Trim();
        bool contiene = silabas.Contains(silAux);
        return contiene;
    }
}