using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _background;

    // Start is called before the first frame update
    void Start()
    {
        SetBackground();
    }

    public void SetBackground()
    {
        SpriteRenderer spriteRenderer = GameObject.Find("Background").GetComponent<SpriteRenderer>();

        if(spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer not found.");
        }

        int index = Random.Range(0, _background.Length - 1);
        spriteRenderer.sprite = _background[index];
    }
}
