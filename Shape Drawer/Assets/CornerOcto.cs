using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerOcto : MonoBehaviour
{

    private void OnEnable()
    {
        transform.localPosition = new Vector3(-660, 1200, 0);
        OnHideDone();
    }

    void OnHideDone()
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", new Vector3(-330, 850, 0),
            "time", 1f,
            "delay", UnityEngine.Random.Range(5, 10),
            "islocal", true,
            "oncomplete", "OnShowDone"
        ));
    }

    void OnShowDone()
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", new Vector3(-660, 1200, 0),
            "time", 1f,
            "delay", UnityEngine.Random.Range(5, 10),
            "islocal", true,
            "oncomplete", "OnHideDone"
            ));
    }

}
