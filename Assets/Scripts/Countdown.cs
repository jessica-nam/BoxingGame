using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Countdown : MonoBehaviour
{
    public static Countdown instance;
    public int countdownTime;
    public TextMeshProUGUI countDownDisplay;
    public bool CountDownDone;
    [SerializeField] private Image Round;
    public AudioSource source;
    public AudioClip clip;
    //public AudioSource source1;
    public AudioClip clip1;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(CountdownToStart());
        StartCoroutine(SetupGame());
    }

    private IEnumerator SetupGame()
    {
        yield return new WaitForSeconds(3f);

        source.PlayOneShot(clip1);
    }



    IEnumerator CountdownToStart()
    {
        source.PlayOneShot(clip);
        while (countdownTime > 0)
        {
            countDownDisplay.text = countdownTime.ToString();
            Round.enabled = true;
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countDownDisplay.text = "GO!";
        Round.enabled = false;
        yield return new WaitForSeconds(1f);
        countDownDisplay.gameObject.SetActive(false);

        if (countdownTime == 0)
        {
            CountDownDone = true;
        }
    }

}
