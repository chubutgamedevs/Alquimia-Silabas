using UnityEngine;

public class ExplosionSpawner : MonoBehaviour
{


    public GameObject explosionPrefab;

    void OnEnable()
    {
        EventManager.martilloGolpea += nuevaExplosion;
    }

    void OnDisable()
    {
        EventManager.martilloGolpea -= nuevaExplosion;
    }

    void nuevaExplosion(Vector3 pos)
    {
        Instantiate(explosionPrefab, pos , new Quaternion(0, 0, 0, 0), transform.parent);
    }

}
