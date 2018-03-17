using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {
    public float transitionSpeed = 0.5f;

    public GameObject three;
    public GameObject two;
    public GameObject one;
    public GameObject go;

    private float opacityOne;
    private float opacityTwo;
    private float opacityThree;
    private float opacityGo;

    private float goTime = 1.0f;
    private bool musicPlaying = false;

	// Use this for initialization
	void Start () {
        opacityOne = 0f;
        opacityTwo = 0f;
        opacityThree = 0f;
        opacityGo = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        if(TrackTargets.timeLeft <= 0) {
            if (goTime > 0)
            {
                if (opacityGo < 1.0f)
                {
                    one.SetActive(false);
                    opacityGo += transitionSpeed * Time.deltaTime;
                    showGradually(go, opacityGo);
                }

                if (!musicPlaying)
                {
                    GetComponent<AudioSource>().Play();
                    musicPlaying = true;
                }

                goTime -= Time.deltaTime;
            }
            else
            {
                go.SetActive(false);
            }
        } else if (TrackTargets.timeLeft <= 1 && TrackTargets.timeLeft > 0)
        {
            two.SetActive(false);
            showGradually(one, opacityOne);

            if (opacityOne < 1.0f)
            {
                opacityOne += transitionSpeed * Time.deltaTime;
            }
        }
        else if (TrackTargets.timeLeft <= 2.0f && TrackTargets.timeLeft > 1.0f)
        {
            three.SetActive(false);
            showGradually(two, opacityTwo);

            if (opacityTwo < 1.0f)
            {
                opacityTwo += transitionSpeed * Time.deltaTime;
            }
        }
        else if (TrackTargets.timeLeft <= 3.0f && TrackTargets.timeLeft > 2.0f)
        {
            showGradually(three, opacityThree);

            if (opacityThree < 1.0f)
            {
                opacityThree += transitionSpeed * Time.deltaTime;
            }
        }
	}

    public static void showGradually(GameObject obj, float opacity)
    {
        Image img = obj.GetComponent<Image>();
        Color c = img.color;

        img.color = new Color(c.r, c.g, c.b, opacity);
        obj.SetActive(true);
    }
}
