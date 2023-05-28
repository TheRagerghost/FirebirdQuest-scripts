using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public List<RectTransform> subviews;
    public float opacityFadeSpeed = 20f;

    public RectTransform currentSubview;
    public RectTransform targetSubview;

    public CanvasGroup currentGroup;

    public float subview_opacity = 0;

    public void Awake()
    {
        if (subviews.Count > 0)
        {
            targetSubview = currentSubview = subviews[0];
            currentSubview.gameObject.SetActive(true);
            currentGroup = currentSubview.GetComponent<CanvasGroup>();
            currentGroup.alpha = subview_opacity;
        }
    }

    void Update()
    {
        if (targetSubview != null)
        {
            if (currentSubview != targetSubview && subview_opacity < 0.05f)
            {
                currentSubview.gameObject.SetActive(false);
                currentSubview = targetSubview;
                currentSubview.gameObject.SetActive(true);
                currentGroup = currentSubview.GetComponent<CanvasGroup>();
                currentGroup.alpha = subview_opacity;
            }
            else if (currentSubview == targetSubview && subview_opacity < 1f)
            {
                subview_opacity = Mathf.Lerp(subview_opacity, 1f, opacityFadeSpeed * Time.deltaTime);
            } 
            else if (currentSubview != targetSubview && subview_opacity > 0f)
            {
                subview_opacity = Mathf.Lerp(subview_opacity, 0f, opacityFadeSpeed * Time.deltaTime);
            }

            currentGroup.alpha = subview_opacity;
        }
    }

    public void SwitchView(int id)
    {
        if(subviews.Count > id && id >= 0)
        {
            targetSubview = subviews[id];
        }
    }
}
