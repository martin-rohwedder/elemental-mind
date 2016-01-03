using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GaugeBehaviorScript : MonoBehaviour {

    public float duration = 3.0f;
    public GameObject gauge;
    public GameObject gaugeMarker;

    private bool gaugeIsReady = true;
    private bool stopGauge = false;

    // Use this for initialization
    void Start () {
        gauge.GetComponent<Image>().fillAmount = 0;
        gaugeMarker.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(6, 38, 0);
	}

	// Update is called once per frame
	void Update () {
        if (gaugeIsReady)
        {
            StartCoroutine(FillGauge());
        }
	}

    IEnumerator FillGauge() {
        float pointInTime = 0.0f;
        gaugeIsReady = false;

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
    }

    public void Reset()
    {
        gauge.GetComponent<Image>().fillAmount = 0;
        gaugeMarker.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(6, 38, 0);
        gaugeIsReady = true;
    }

    public void StopGauge()
    {
        stopGauge = true;
    }
}
