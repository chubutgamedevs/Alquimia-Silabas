using System.Collections.Generic;
using UnityEngine;

public class Ubicador
{
    private static List<Vector3> puntos = new List<Vector3>();

    private static Vector3 generateNuevoPunto()
    {
        return new Vector3(Random.Range(Constants.minX, Constants.maxX), Random.Range(Constants.minY, Constants.maxY), 0);
    }
    public static Vector3 nuevoPunto()
    {
        Vector3 nuevoPunto;
        do
        {
            nuevoPunto = generateNuevoPunto();
        } while (puntos.Contains(nuevoPunto));

        puntos.Add(nuevoPunto);

        return nuevoPunto * Constants.factorAgrandarGrid;
    }

    public static bool estaDentroDelJuego(Vector3 punto)
    {
        return (punto.x > Constants.minX && punto.x < Constants.maxX && punto.y > Constants.minY && punto.y < Constants.maxY);
    }
}
