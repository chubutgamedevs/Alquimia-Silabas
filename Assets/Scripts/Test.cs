using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{

	public float radius = 1;
	public Vector2 regionSize = Vector2.one;
	public int rejectionSamples = 10;
	public float displayRadius = 1;

	List<Vector2> points;
    private void Start()
    {
		points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
		Debug.Log("cantidad de puntos:");
		Debug.Log(points.Count);
		foreach(Vector2 point in points)
        {
			Debug.Log(point);
		}
	}
    void OnValidate()
	{
		
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(regionSize / 2, regionSize);
		if (points != null)
		{
			foreach (Vector2 point in points)
			{
				Gizmos.DrawSphere(new Vector3(point.x,point.y,1), displayRadius);
			}
		}
	}
}
