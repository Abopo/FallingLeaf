using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public Text rotateText;
    public Text windText;

    float _timer;
    float _time = 1.0f;

    int tutorialIndex;

    float startTime;
    Vector2 startPos;
    bool couldBeSwipe;
    float comfortZone = 300;
    float minSwipeDist = 75;
    float maxSwipeTime = 0.5f;

    // Use this for initialization
    void Start () {
        _timer = 0;
        tutorialIndex = 0;

#if UNITY_STANDALONE
        rotateText.text = "Press the A or D keys to rotate.";
        windText.text = "Press Spacebar to blow wind.";
#endif
    }

    // Update is called once per frame
    void Update () {
        if (PlayerPrefs.GetInt("FirstTime") == 1) {
            // Wait a short time before showing tutorial
            _timer += Time.deltaTime;
            if (_timer >= _time) {
                if (tutorialIndex == 0 && rotateText.gameObject.activeSelf == false) {
                    // Disply first tutorial message
                    ShowRotateText();
                } else if (tutorialIndex == 1 && windText.gameObject.activeSelf == false) {
                    ShowWindText();
                }
            }

            if (tutorialIndex == 0 && Time.timeScale == 0 && 
                (Input.GetKey(KeyCode.A) || TouchingLeft() || Input.GetKey(KeyCode.D) || TouchingRight())) {
                // rotate left
                HideRotateText();
            }
            if(tutorialIndex == 1 && Time.timeScale == 0 &&
                (Input.GetKey(KeyCode.Space) || SwipedUp())) {
                HideWindText();
            }
        }
    }

    void ShowRotateText() {
        // Pause the game
        Time.timeScale = 0;
        rotateText.gameObject.SetActive(true);
    }

    public void HideRotateText() {
        // Unpause game
        Time.timeScale = 1;
        rotateText.gameObject.SetActive(false);
        _timer = -8f;
        tutorialIndex = 1;
    }

    void ShowWindText() {
        // Pause the game
        Time.timeScale = 0;
        windText.gameObject.SetActive(true);
    }

    public void HideWindText() {
        // Unpause game
        Time.timeScale = 1;
        windText.gameObject.SetActive(false);
        tutorialIndex = 2;

        PlayerPrefs.SetInt("FirstTime", 2);
    }

    // Touch input stuff
    bool TouchingLeft() {
        if (Input.touchCount > 0 && Input.GetTouch(0).position.x < Screen.width / 2) {
            return true;
        }

        return false;
    }

    bool TouchingRight() {
        if (Input.touchCount > 0 && Input.GetTouch(0).position.x > Screen.width / 2) {
            return true;
        }

        return false;
    }

    bool SwipedUp() {
        if (Input.touchCount > 0) {
            Touch touch = Input.touches[0];

            switch (touch.phase) {
                case TouchPhase.Began:
                    couldBeSwipe = true;
                    startPos = touch.position;
                    startTime = Time.time;

                    break;
                case TouchPhase.Moved:
                    if (Mathf.Abs(touch.position.x - startPos.x) > comfortZone) {
                        couldBeSwipe = false;
                    }
                    break;
                case TouchPhase.Stationary:
                    //couldBeSwipe = false;
                    break;
                case TouchPhase.Ended:
                    float swipeTime = Time.time - startTime;
                    float swipeDist = (touch.position - startPos).magnitude;

                    if (couldBeSwipe && swipeTime < maxSwipeTime && swipeDist > minSwipeDist) {
                        // it's a swipe!
                        float swipeDir = Mathf.Sign(touch.position.y - startPos.y);

                        // If the swipe was upward
                        if (swipeDir > 0) {
                            return true;
                        }
                    }
                    break;
            }
        }

        return false;
    }

}
