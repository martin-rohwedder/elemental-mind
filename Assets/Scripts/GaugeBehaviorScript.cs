using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GaugeBehaviorScript : MonoBehaviour {

    enum States
    {
        HOME_SCREEN,
        BEFORE_GAME,
        DURING_GAME,
        GAME_OVER
    };

    public float duration = 3.0f;
    public GameObject canvas;
    public GameObject homePanel;
    public GameObject gamePanel;
    public GameObject gauge;
    public GameObject gaugeMarker;
    public GameObject numberMarker;
    public GameObject stopButton;
    public GameObject continueButton;
    public GameObject restartButton;
    public GameObject goToHomeButton;
    public Text messageText;
    public Text pointsText;
    public Text lifeText;
    public Text bestScoreText;
    public AudioClip beepSFX;
    public AudioClip startBeepSFX;

    private AudioSource audioSource;
    private bool gaugeIsReady = true;
    private bool stopGauge = false;
    private int randomNumber = 0;
    private bool delayHasBegun = false;
    private States currentState = States.HOME_SCREEN;
    private int points = 0;
    private int life = 30;
    private Animator canvasAnimator;

    // Use this for initialization
    void Start () {
        canvasAnimator = canvas.GetComponent<Animator>();
        canvasAnimator.SetBool("FadeInHome", true);

        gamePanel.SetActive(false);
        homePanel.SetActive(true);

        gauge.GetComponent<Image>().fillAmount = 0;
        gaugeMarker.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(6, 38, 0);
        numberMarker.SetActive(false);

        pointsText.text = "" + points;
        lifeText.text = "" + life;

        randomNumber = Random.Range(10, 98);
        messageText.text = "Stop at " + randomNumber + "%";

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;

        GetBestScore();
	}

	// Update is called once per frame
	void Update () {
        if (currentState == States.HOME_SCREEN)
        {

        }
        else if (currentState == States.BEFORE_GAME)
        {
            if (!delayHasBegun)
            {
                StartCoroutine(StartDelay());
            }
        }
        else if (currentState == States.DURING_GAME)
        {
            if (gaugeIsReady)
            {
                StartCoroutine(FillGauge());
            }
        }
        else if (currentState == States.GAME_OVER)
        {
            restartButton.SetActive(true);
            goToHomeButton.SetActive(true);
            messageText.text = "Game Over";
            Save();
        }
	}

    IEnumerator FillGauge() {
        float pointInTime = 0.0f;
        gaugeIsReady = false;
        stopButton.SetActive(true);

        while (pointInTime <= duration)
        {
            if (stopGauge)
            {
                stopGauge = false;
                break;
            }

            gaugeMarker.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(((gauge.GetComponent<Image>().fillAmount * 100f) * 3) + 6, 38, 0);
            gauge.GetComponent<Image>().fillAmount = Mathf.Lerp(0f, 1f, pointInTime / duration);
            pointInTime += Time.deltaTime;
            yield return null;
        }

        if (gauge.GetComponent<Image>().fillAmount >= 0.99f)
        {
            StopGauge();
            stopGauge = false;
            yield return null;
        }
    }

    IEnumerator StartDelay()
    {
        delayHasBegun = true;

        audioSource.PlayOneShot(beepSFX);
        yield return new WaitForSeconds(0.7f);
        audioSource.PlayOneShot(beepSFX);
        yield return new WaitForSeconds(0.7f);
        audioSource.PlayOneShot(beepSFX);
        yield return new WaitForSeconds(0.7f);
        audioSource.PlayOneShot(startBeepSFX);

        currentState = States.DURING_GAME;
    }

    //Restart the game (means going on)
    public void Restart()
    {
        continueButton.SetActive(false);

        randomNumber = Random.Range(10, 98);
        messageText.text = "Stop at " + randomNumber + "%";

        gauge.GetComponent<Image>().fillAmount = 0;
        gaugeMarker.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(6, 38, 0);
        numberMarker.SetActive(false);
        gaugeIsReady = true;
        delayHasBegun = false;
        currentState = States.BEFORE_GAME;
    }

    // Stop the gauge and show the number at which the gauge has been stopped.
    public void StopGauge()
    {
        stopGauge = true;
        numberMarker.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(((gauge.GetComponent<Image>().fillAmount * 100f) * 3) + 6, -31, 0);
        numberMarker.GetComponentInChildren<Text>().text = "" + Mathf.CeilToInt(gauge.GetComponent<Image>().fillAmount * 100);
        numberMarker.SetActive(true);

        stopButton.SetActive(false);

        int difference = Mathf.Abs(randomNumber - Mathf.CeilToInt(gauge.GetComponent<Image>().fillAmount * 100));
        life -= difference;
        points++;

        if (life > 0)
        {
            continueButton.SetActive(true);
        }
        else
        {
            life = 0;
            currentState = States.GAME_OVER;
        }

        pointsText.text = "" + points;
        lifeText.text = "" + life;
    }

    public void SetupNewGame()
    {
        restartButton.SetActive(false);
        goToHomeButton.SetActive(false);

        points = 0;
        life = 30;
        pointsText.text = "" + points;
        lifeText.text = "" + life;

        randomNumber = Random.Range(10, 98);
        messageText.text = "Stop at " + randomNumber + "%";

        gauge.GetComponent<Image>().fillAmount = 0;
        gaugeMarker.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(6, 38, 0);
        numberMarker.SetActive(false);
        gaugeIsReady = true;
        delayHasBegun = false;
        currentState = States.BEFORE_GAME;
    }

    public void StartGame()
    {
        SetupNewGame();

        canvasAnimator.SetBool("FadeInHome", false);
        canvasAnimator.SetBool("FadeInGame", true);

        //homePanel.SetActive(false);
        //gamePanel.SetActive(true);

        currentState = States.BEFORE_GAME;
    }

    public void GoToHome()
    {
        SetupNewGame();

        canvasAnimator.SetBool("FadeInGame", false);
        canvasAnimator.SetBool("FadeInHome", true);

        //gamePanel.SetActive(false);
        //homePanel.SetActive(true);

        GetBestScore();

        currentState = States.HOME_SCREEN;
    }

    private void Save()
    {
        if (points > PlayerPrefs.GetInt("player_score"))
        {
            PlayerPrefs.SetInt("player_score", points);
        }

        bestScoreText.text = "Best Score\n\r" + PlayerPrefs.GetInt("player_score");
    }

    private void GetBestScore()
    {
        if (PlayerPrefs.HasKey("player_score"))
        {
            bestScoreText.text = "Best Score\n\r" + PlayerPrefs.GetInt("player_score");
        }
        else
        {
            PlayerPrefs.SetInt("player_score", 0);
            bestScoreText.text = "Best Score\n\r0";
        }
    }
}
