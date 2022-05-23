using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SightIndicator : MonoBehaviour
{
    [SerializeField]
    private Image cross, progress;

    private bool onFlashCross = false;
    
    public void OnPlaceBlock()
    {
        if (!onFlashCross) StartCoroutine(CoFlashCross());
    }

    private IEnumerator CoFlashCross()
    {
        onFlashCross = true;
        var temp = cross.color;
        cross.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        cross.color = temp;
        onFlashCross = false;
    }
    public void SetDigProgress(float value, float max = 1)
    {
        progress.fillAmount = Mathf.Clamp01(value / max);
    }
}
