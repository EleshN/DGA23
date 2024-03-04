using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestPointer : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    private RectTransform pointerRectTransform;
    private Vector3 targetPosition;
    // [SerializeField] private Camera uiCamera;

    private void Awake()
    {
        targetPosition = targetTransform.position;
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 end = targetPosition;
        Vector3 origin = Camera.main.transform.position;
        Vector3 dir = (end - origin).normalized;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle + 45);
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= 0 || targetPositionScreenPoint.x >= Screen.width || targetPositionScreenPoint.y <= 0 || targetPositionScreenPoint.y >= Screen.height;


        // Debug.Log(isOffScreen + " " + targetPositionScreenPoint);
        Debug.Log(angle);

        /**
        if (isOffScreen)
        {
            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, 0, Screen.width);
            cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, 0, Screen.height);
            Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
            pointerRectTransform.position = pointerWorldPosition;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
        }
        */
    }
}
