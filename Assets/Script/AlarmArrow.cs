using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlarmArrow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{ 
    [SerializeField] private Alarm _alarm;

    private RectTransform _rectTransform;

    private bool _pm;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _pm = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        ArrowMovement(eventData);
        _alarm.FillInput(_rectTransform.localEulerAngles.z, _pm);
    }

    public void OnEndDrag(PointerEventData eventData)
    {        
        _pm = false;
    }

    private void ArrowMovement(PointerEventData eventData)
    {
        Vector2 dir = eventData.position - (Vector2)transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        transform.rotation = rotation;

        if(angle > 85 && angle < 95)
        {
            _pm = true;
        }
    }

    public void ParseTimeToAngle(float time)
    {
        transform.rotation = Quaternion.AngleAxis(time, Vector3.forward);
    }
}
