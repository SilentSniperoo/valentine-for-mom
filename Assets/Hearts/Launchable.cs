using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Launchable : MonoBehaviour
{
  public Vector2 Velocity;
  public float Gravity;
  public float RotationSpeed;

  // Update is called once per frame
  void Update()
  {
    Velocity.y += Gravity * Time.deltaTime;
    RectTransform rect = GetComponent<RectTransform>();
    rect.anchoredPosition += Velocity * Time.deltaTime;
    Quaternion rotation = Quaternion.AngleAxis(RotationSpeed * Time.deltaTime, Vector3.forward);
    rect.localRotation = rect.localRotation * rotation;
  }
}
