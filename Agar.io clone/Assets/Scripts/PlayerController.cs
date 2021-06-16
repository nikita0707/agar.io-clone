using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Vector2 mousePos;
    private Vector2 randVector;
    private Vector3 vecScale;
    private float camSize;
    private float scale;
    private bool isAlive;
    private bool onPause;
    private uint score;
    private string playerName;

    public float mass;
    public Camera cam;
    public float speed;
    public SpriteRenderer spriteRend;
    public GameObject panel;
    public Text scoreText;
    public Text menuText;

    void Start()
    {
        mass = 1;
        vecScale.Set(1, 1, 1);
        camSize = 8;
        speed = 1;
        spriteRend.color = Color.HSVToRGB(PlayerPrefs.GetFloat("color"), 1, 1);
        isAlive = true;
        onPause = false;
        score = 0;
        playerName = PlayerPrefs.GetString("nickname");
    }

    void Update()
    {
        if (!onPause && isAlive)
        {
            scale = mass / 200 + 0.95f;
            vecScale.Set(scale, scale, 1);
            transform.localScale = vecScale;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos -= (Vector2)transform.position;
            speed = 1 / Mathf.Sqrt(scale);
            transform.Translate(mousePos * Time.deltaTime * speed);
            mass -= 0.00000002f * mass * mass;
            if (mass < 1)
                mass = 1;
            if (cam.orthographicSize > camSize)
            {
                if (cam.orthographicSize - 1 > camSize)
                    cam.orthographicSize = camSize;
                else
                    cam.orthographicSize -= 0.0001f;
            }
            else if (cam.orthographicSize < camSize)
            {
                if (cam.orthographicSize + 1 < camSize)
                    cam.orthographicSize = camSize;
                else
                    cam.orthographicSize += 0.0001f;
            }
        }
        if (isAlive && Input.GetKeyDown(KeyCode.Escape))
        {
            menuText.text = "Μενώ";
            panel.SetActive(!onPause);
            onPause = !onPause;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Eat")
        {
            mass++;
            score++;
            scoreText.text = score.ToString();
            randVector.Set(Random.Range(-99.5f, 99.5f), Random.Range(-99.5f, 99.5f));
            collision.gameObject.transform.position = randVector;
            camSize += 0.002f;
        }
        else if (collision.gameObject.tag == "Fight")
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy.mass < mass * 1.2f)
            {
                mass += enemy.mass;
                score += enemy.score;
                scoreText.text = score.ToString();
                camSize += enemy.mass * 0.002f;
                Destroy(collision.gameObject);
            }
            else if (enemy.mass > mass * 1.2f)
            {
                menuText.text = "Game over!";
                panel.SetActive(true);
                isAlive = false;
            }
        }
    }
    private void OnDisable()
    {
        SaveScore();
    }

    public void SaveScore()
    {
        GameObject highscore = GameObject.FindGameObjectWithTag("Scoreboard");
        HighscoreTable table = highscore.GetComponent<HighscoreTable>();
        ScoreboardItem item = new ScoreboardItem();
        item.name = playerName;
        item.score = score;
        item.time = System.DateTime.Now;
        table.scoreboardList.Add(item);
        table.SaveScoreboard();
    }    
}
