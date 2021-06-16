using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    private Vector2 randVector;

    public GameObject food;
    public GameObject enemy;

    private void Awake()
    {
        for (int i = 0; i < 5000; i++)
        {
            randVector.Set(Random.Range(-99.5f, 99.5f), Random.Range(-99.5f, 99.5f));
            Instantiate(food, randVector, Quaternion.identity);
        }
        for (int i = 0; i < 50; i++)
        {
            randVector.Set(Random.Range(-99.5f, 99.5f), Random.Range(-99.5f, 99.5f));
            Instantiate(enemy, randVector, Quaternion.identity);
        }
    }
}
