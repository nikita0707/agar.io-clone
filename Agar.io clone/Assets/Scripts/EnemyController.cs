using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float scale;
    private Vector3 vecScale;
    private GameObject[] food;
    private GameObject nearest;
    private bool hasTarget;
    private Vector2 randVector;
    private Vector2 foodPos;
    private float speed;
    private float quotient;
    private float delta;

    public float mass;
    public SpriteRenderer spriteRend;
    public uint score;

    // Start is called before the first frame update
    void Start()
    {
        mass = 1;
        vecScale.Set(1, 1, 1);
        spriteRend.color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
        hasTarget = false;
        delta = 5;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scale = mass / 200 + 0.95f;
        vecScale.Set(scale, scale, 1);
        transform.localScale = vecScale;
        if (!hasTarget)
            FindNearestFood();
        if (hasTarget)
        {
            foodPos = nearest.transform.position;
            foodPos -= (Vector2)transform.position;
            quotient = foodPos.magnitude / delta;
            foodPos /= quotient;
            speed = 1 / Mathf.Sqrt(scale);
            transform.Translate(foodPos * Time.deltaTime * speed);
        }
        mass -= 0.00000002f * mass * mass;
    }

    void FindNearestFood()
    {
        food = GameObject.FindGameObjectsWithTag("Eat");
        float distance = Mathf.Infinity;
        Vector2 position = transform.position;
        foreach (GameObject f in food)
        {
            Vector2 diff = (Vector2)f.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                nearest = f;
                hasTarget = true;
                distance = curDistance;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Eat")
        {
            hasTarget = false;
            mass++;
            score++;
            randVector.Set(Random.Range(-99.5f, 99.5f), Random.Range(-99.5f, 99.5f));
            collision.gameObject.transform.position = randVector;
        }
    }
}
