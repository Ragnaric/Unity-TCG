using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDamageIndicator : MonoBehaviour
{
    public TMP_Text damageTaken;

    public float moveSpeed, displayTime = 3f;

    private RectTransform canvas;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, displayTime);
        canvas = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        canvas.anchoredPosition += new Vector2(0, -moveSpeed * Time.deltaTime);
    }
}
