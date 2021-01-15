using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HeartHover : MonoBehaviour
{
  Image Foreground;
  public float FadeTime;
  float CurrentFade = 1;

  // Use this for initialization
  void Start()
  {
    Foreground = GetComponent<Image>();
    Foreground.alphaHitTestMinimumThreshold = 0.1f;
  }

  // Update is called once per frame
  void Update()
  {
    if (MouseIsInside)
    {
      if (FadeTime <= 0) CurrentFade = 0;
      else CurrentFade = Mathf.Max(0, CurrentFade - Time.deltaTime / FadeTime);
    }
    else
    {
      if (FadeTime <= 0) CurrentFade = 1;
      else CurrentFade = Mathf.Min(1, CurrentFade + Time.deltaTime / FadeTime);
    }
    Color color = Foreground.color;
    color.a = CurrentFade;
    Foreground.color = color;
  }

  bool MouseIsInside;
  public void MouseEntered()
  {
    MouseIsInside = true;
  }
  public void MouseExited()
  {
    MouseIsInside = false;
  }
}
