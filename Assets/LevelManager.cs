using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int playerCash = 0;
    public int PlayerCash {
        get => playerCash;
        set {
            playerCash = value;
            UpdateCashText();
        }
    }
    [field: SerializeField] public int Cost{ get; set; }
    public bool WasSpin { get; set; }
    [field: SerializeField] ReelControl Reel_1 { get; set; }
    [field: SerializeField] ReelControl Reel_2 { get; set; }
    [field: SerializeField] ReelControl Reel_3 { get; set; }
    [field: SerializeField] Lever Lever { get; set; }

    [field: SerializeField] SymbolData Seven { get; set; }
    [field: SerializeField] SymbolData Bar { get; set; }
    [field: SerializeField] SymbolData Bell { get; set; }
    [field: SerializeField] SymbolData Cherry { get; set; }

    [field: SerializeField] TextMeshProUGUI CashTxt { get; set; }
    void UpdateCashText()=> CashTxt.SetText($"{PlayerCash} $");

    [Header("InputRef")]
    public InputActionReference spaceInputRef;

    public InputAction SpaceInputAction => spaceInputRef.action;

    private void OnEnable() => SpaceInputAction.Enable();
    private void OnDisable() => SpaceInputAction.Disable();

    private void Awake()
    {
        Lever = transform.GetComponentInChildren<Lever>();

        SpaceInputAction.performed += _ =>
        {
            if (WasSpin)
            {
                if (Reel_1.IsSpining)
                {
                    Reel_1.Stop();
                }
                else if (Reel_2.IsSpining)
                {
                    Reel_2.Stop();
                }
                else if (Reel_3.IsSpining)
                {
                    Reel_3.Stop();
                    CheckAllPayline();
                    WasSpin = false;
                }
            }
            else
            {
                if(Cost < PlayerCash)
                {
                    PlayerCash -= Cost;
                    Lever.IsLeverDown = true;
                }
                else
                {
                    Debug.Log("Game Over !!!!");
                }
            }
        };

        SpaceInputAction.canceled += _ => {
            if (!WasSpin && Lever.IsLeverDown)
            {
                Reel_1.Move();
                Reel_2.Move();
                Reel_3.Move();
                WasSpin = true;
                Lever.IsLeverDown = false;
            }
        };

        UpdateCashText();
    }

    public void CheckAllPayline()
    {
        PaylineCheck(1);
    }

    void PaylineCheck(int idx)
    {
        var value_1 = Reel_1.SymbolList[idx].SymbolData.name;
        var value_2 = Reel_2.SymbolList[idx].SymbolData.name;
        var value_3 = Reel_3.SymbolList[idx].SymbolData.name;

        int pay = 0;

        if (value_1 == Seven.name && value_2 == Seven.name && value_3 == Seven.name)
        {
            pay += 777;
        }
        else if (value_1 == Bar.name && value_2 == Bar.name && value_3 == Bar.name)
        {
            pay += 300;
        }
        else if (value_1 == Bell.name && value_2 == Bell.name && value_3 == Bell.name)
        {
            pay += 100;
        }
        else
        {
            int countCherry = 0;
            countCherry += value_1 == Cherry.name ? 1 : 0;
            countCherry += value_2 == Cherry.name ? 1 : 0;
            countCherry += value_3 == Cherry.name ? 1 : 0;

            if (countCherry == 3)
            {
                pay += 10;
            }
            else if (countCherry == 2)
            {
                pay += 5;
            }
            else if (countCherry == 1)
            {
                pay += 2;
            }
        }

        Debug.Log($"{ value_1},{value_2},{value_3} : {pay}");
        PlayerCash += pay;
    }
}