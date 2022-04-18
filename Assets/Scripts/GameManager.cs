using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI tapText;
    public TextMeshProUGUI logText;
    public Button buttonUp;
    public Button buttonDown;
    public Button buttonLeft;
    public Button buttonRight;
    public Canvas promptUI;
    public Slider bar;
    public Image handle;
    public Image prompt;
    public Image back;
    public GameObject fish;
    
    public Sprite[] sprites;
    public Sprite[] barBack;
    public Sprite[] icon;

    public static bool isGameActive;
    public static bool isLuring;
    public static bool isMovingRight;
    public static bool isPausing;

    private int change;
    private int tik;
    private int wait;
    private int spriteIndex;

    // Start is called before the first frame update
    void Start()
    {
        isGameActive = false;
        isLuring = true;
        isMovingRight = true;
        isPausing = false;
        tik = 0;
        wait = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPausing)
        {
            if (!isGameActive)
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isGameActive = true;
                    tapText.gameObject.SetActive(false);
                    promptUI.gameObject.SetActive(true);
                }
            }
            else
            {
                if (isLuring)
                {
                    logText.text = "Tap When the Fish\nis in the Middle";
                    if (isMovingRight)
                    {
                        bar.value += 0.005f;
                    }
                    else
                    {
                        bar.value -= 0.005f;
                    }
                    if (bar.value >= 0.95f || bar.value <= 0.05f)
                    {
                        isMovingRight = !isMovingRight;
                    }

                    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        if (bar.value > 0.44f && bar.value < 0.56f)
                        {
                            logText.text = "Tap the Button\nto Wrestle";
                            SetPanel(true);
                            spriteIndex = Random.Range(0, 4);
                            prompt.sprite = sprites[spriteIndex];
                            handle.gameObject.transform.localScale = new Vector3(3.0f, 5.0f, 0.0f);
                            InvokeRepeating(nameof(ChangePrompt), 0.0f, 1.0f);
                            InvokeRepeating(nameof(ChangeIcon), 0.0f, 0.5f);
                            isLuring = false;
                            back.sprite = barBack[1];
                        }
                        else
                        {
                            bar.value = 0.05f;
                            logText.text = "Fish Escaped!";
                            promptUI.gameObject.SetActive(false);
                            tapText.gameObject.SetActive(true);
                            isGameActive = false;
                            isMovingRight = true;
                        }
                    }
                }
                else
                {
                    bar.value -= 0.0025f;

                    if (bar.value < 0.07f)
                    {
                        logText.text = "Fish Escaped!";
                    
                        SetPanel(false);
                        CancelInvoke(nameof(ChangePrompt));
                        CancelInvoke(nameof(ChangeIcon));
                    
                        bar.value = 0.05f;
                        handle.gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 0.0f);
                        promptUI.gameObject.SetActive(false);
                        tapText.gameObject.SetActive(true);
                        isGameActive = false;
                        isMovingRight = true;
                        isLuring = true;
                        tik = 0;
                        handle.sprite = icon[0];
                        back.sprite = barBack[0];
                    }

                    if (bar.value > 0.95f)
                    {
                        logText.text = "Fish Catched!";
                    
                        SetPanel(false);
                        CancelInvoke(nameof(ChangePrompt));
                        CancelInvoke(nameof(ChangeIcon));

                        promptUI.gameObject.SetActive(false);
                        isPausing = true;
                        fish.gameObject.SetActive(true);
                        InvokeRepeating(nameof(Pause), 0.0f, 1.0f);
                    }
                }
            }
        }
    }

    public void Pause()
    {
        wait++;
        if (wait == 3)
        {
            fish.gameObject.SetActive(false);
            wait = 0;
            isPausing = false;
            bar.value = 0.05f;
            handle.gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 0.0f);
            tapText.gameObject.SetActive(true);
            logText.text = "";
            isGameActive = false;
            isMovingRight = true;
            isLuring = true;
            tik = 0;
            handle.sprite = icon[0];
            back.sprite = barBack[0];
            CancelInvoke(nameof(Pause));
        }
    }

    public void SetPanel(bool status)
    {
        buttonUp.gameObject.SetActive(status);
        buttonDown.gameObject.SetActive(status);
        buttonLeft.gameObject.SetActive(status);
        buttonRight.gameObject.SetActive(status);
        prompt.gameObject.SetActive(status);
    }

    public void ChangePrompt()
    {
        change = Random.Range(0, 2);
        if (change == 1)
        {
            spriteIndex = Random.Range(0, 4);
            prompt.sprite = sprites[spriteIndex];
        }
    }

    public void ChangeIcon()
    {
        tik += 1;
        if (tik == 5)
        {
            tik = 1;
        }
        if (tik == 4)
        {
            handle.sprite = icon[2];
        }
        else
        {
            handle.sprite = icon[tik];
        }
    }

    public void TapUp()
    {
        if (spriteIndex == 0)
        {
            bar.value += (float) 0.05;
        }
    }

    public void TapDown()
    {
        if (spriteIndex == 1)
        {
            bar.value += (float) 0.05;
        }
    }

    public void TapLeft()
    {
        if (spriteIndex == 2)
        {
            bar.value += (float) 0.05;
        }
    }

    public void TapRight()
    {
        if (spriteIndex == 3)
        {
            bar.value += (float) 0.05;
        }
    }
}
