using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
  public float MinLaunchDelay = 1;
  public float MaxLaunchDelay = 3;
  public float MinLaunchSpeed = 10;
  public float MaxLaunchSpeed = 25;
  public float MinLaunchRotation = 10;
  public float MaxLaunchRotation = 20;

  public float MinGravity = 10;
  public float MaxGravity = 25;

  public float MinRotationSpeed = 5;
  public float MaxRotationSpeed = 15;

  public Vector2 StartAnchoredPosition;
  public float StartRotation;
  public GameObject ToLaunch;

  float TimeTillLaunch;

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    TimeTillLaunch -= Time.deltaTime;
    if (TimeTillLaunch < 0)
    {
      TimeTillLaunch = Random.Range(MinLaunchDelay, MaxLaunchDelay);

      Launch();
    }
  }

  void Launch()
  {
    if (ToLaunch == null) return;
    
    GameObject obj = Instantiate<GameObject>(ToLaunch);
    obj.transform.SetParent(transform);
    RectTransform rect = obj.GetComponent<RectTransform>();
    if (rect)
    {
      rect.anchoredPosition = StartAnchoredPosition;
      float startRot = StartRotation;
      rect.localRotation = rect.localRotation * Quaternion.AngleAxis(startRot, Vector3.forward);
    }
    obj.transform.SetParent(transform.parent, true);
    transform.SetAsLastSibling();

    Launchable launch = obj.GetComponent<Launchable>();

    float launchRot = Random.Range(MinLaunchRotation, MaxLaunchRotation);
    launchRot *= Mathf.Deg2Rad;
    Vector2 direction = new Vector2(Mathf.Cos(launchRot), Mathf.Sin(launchRot));
    float speed = Random.Range(MinLaunchSpeed, MaxLaunchSpeed);
    launch.Velocity = direction * speed;

    launch.Gravity = Random.Range(MinGravity, MaxGravity);

    launch.RotationSpeed = Random.Range(MinRotationSpeed, MaxRotationSpeed);
  }
}
