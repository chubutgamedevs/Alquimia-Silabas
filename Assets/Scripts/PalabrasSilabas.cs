using System;
using System.Collections.Generic;

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
}