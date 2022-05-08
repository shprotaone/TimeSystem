using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notice : MonoBehaviour
{
    [SerializeField] private TMP_Text _noticeText;
    
    public IEnumerator SetNoticeRoutine(string text,Color color)
    {
        _noticeText.enabled = true;
        _noticeText.text = text;
        _noticeText.color = color;
        yield return new WaitForSeconds(3f);

        _noticeText.enabled = false;
    }
}
