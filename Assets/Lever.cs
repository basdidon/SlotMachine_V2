using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Lever : MonoBehaviour
{
    SpriteRenderer SpriteRenderer { get; set; }
    [field: SerializeField] Sprite LeverUp { get; set; }
    [field: SerializeField] Sprite LeverDown { get; set; }

    [SerializeField] bool isLeverDown;
    public bool IsLeverDown {
        get => isLeverDown;
        set
        {
            isLeverDown = value;
            if (IsLeverDown)
                SpriteRenderer.sprite = LeverDown;
            else
                SpriteRenderer.sprite = LeverUp;
        }
    }

    private void Awake()
    {
        if(TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            SpriteRenderer = spriteRenderer;
            IsLeverDown = false;
        }
    }
}
