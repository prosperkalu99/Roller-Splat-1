using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public GroundPiece[] allGroundPieces;

    public GameObject explosionFx;

    public AudioClip newLevelSound;
    public AudioClip levelCompleteSound;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelCompletedText;

    public Button restartButton;
    private TextMeshProUGUI restartButtonText;

    private AudioSource playerAudio;
    private AudioSource gameMusicAudio;

    private void Start()
    {
        SetupNewLevel();
    }

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();

        restartButtonText = restartButton.GetComponentInChildren<TextMeshProUGUI>();

        levelText.gameObject.SetActive(true);
        levelText.text = $"Level {SceneManager.GetActiveScene().buildIndex + 1}";

        playerAudio = GetComponent<AudioSource>();
        playerAudio.PlayOneShot(newLevelSound, 0.2f);

        gameMusicAudio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        gameMusicAudio.Play();
    }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;

        for (int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if (isFinished)
        {
            gameMusicAudio.Stop();

            playerAudio.PlayOneShot(levelCompleteSound, 0.2f);

            Instantiate(explosionFx, transform.position, explosionFx.transform.rotation);

            levelCompletedText.text = $"Level {SceneManager.GetActiveScene().buildIndex + 1} Completed";
            levelCompletedText.gameObject.SetActive(true);

            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                restartButtonText.text = "Restart";
            }
            restartButton.gameObject.SetActive(true);
        }

    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
