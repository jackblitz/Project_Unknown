using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHealthBar : MonoBehaviour
{
    public Transform TargetTransform;
    public Image ForegroundImage;
    public Image BackgroundImage;

    public Vector3 Offset = Vector3.zero;
    //Makes sure our target has updated before syncing position
    private void LateUpdate()
    {
        Vector3 direction = (TargetTransform.position - Camera.main.transform.position).normalized;

        bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;

        ForegroundImage.enabled = !isBehind;
        BackgroundImage.enabled = !isBehind;

        transform.position = Camera.main.WorldToScreenPoint(TargetTransform.position + Offset); 
    }

    public void SetHealthBarPercent(float percent)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percent;

        ForegroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
