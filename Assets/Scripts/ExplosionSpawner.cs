using UnityEngine;

public class ExplosionSpawner : MonoBehaviour
{


    public GameObject explosionPrefab;

    void OnEnable()
    {
        EventManager.onMartilloGolpea += nuevaExplosion;
        EventManager.onExplosionFinJuego += nuevaExplosion;
    }

    void OnDisable()
    {
        EventManager.onMartilloGolpea -= nuevaExplosion;
        EventManager.onExplosionFinJuego -= nuevaExplosion;
    }

    void nuevaExplosion(Vector3 pos)
    {
        Instantiate(explosionPrefab, pos , new Quaternion(0, 0, 0, 0), transform.parent);
    }

}
