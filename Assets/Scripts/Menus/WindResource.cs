using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindResource : MonoBehaviour {
    public bool isFull;
    public WindResource prevBar;
    public WindResource nextBar;

    bool _isDraining;
    RectTransform _meter;
    float _meterHeight;

    Image _image;
    Color _fullColor = new Color32(0, 160, 255, 255);
    Color _usedColor = new Color32(0, 100, 160, 255);

    // Use this for initialization
    void Start () {
        isFull = true;
        _isDraining = false;

        _meter = GetComponent<RectTransform>();
        _meterHeight = 67f;

        _image = GetComponent<Image>();
        _image.color = _fullColor;
    }

    // Update is called once per frame
    void Update () {
		if((prevBar == null || (prevBar != null && prevBar.isFull)) && !isFull && !_isDraining) {
            // Fill up the bar
            _meter.sizeDelta = new Vector2(_meter.sizeDelta.x,
                                            _meter.sizeDelta.y + (5.0f * Time.deltaTime));
            if(_meter.sizeDelta.y >= _meterHeight) {
                // Filled
                _meter.sizeDelta = new Vector2(_meter.sizeDelta.x, _meterHeight);
                isFull = true;
                _image.color = _fullColor;
                GetComponent<AudioSource>().Play();
            }
        } else if(_isDraining) {
            // Drain the bar
            _meter.sizeDelta = new Vector2(_meter.sizeDelta.x,
                                            _meter.sizeDelta.y - (_meterHeight * Time.deltaTime));
            if(_meter.sizeDelta.y <= 0) {
                // Drained
                _meter.sizeDelta = new Vector2(_meter.sizeDelta.x, 0);
                _isDraining = false;
            }
        }
	}

    public void Drain() {
        isFull = false;
        _isDraining = true;

        // Darken colors?
        _image.color = _usedColor;
    }
}
