using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] Image _seedIcon;
    [SerializeField] TextMeshProUGUI _moneyText;
    [SerializeField] Image _waterBar;

    private void Awake()
    {
        Player.OnSeedChanged += OnSeedChanged;
        Player.OnMoneyChanged += OnMoneyChanged;
        Player.OnWaterInCanChanged += ChangeWater;
    }

    private void OnDestroy()
    {
        Player.OnSeedChanged -= OnSeedChanged;
        Player.OnMoneyChanged -= OnMoneyChanged;
        Player.OnWaterInCanChanged -= ChangeWater;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSeedChanged(Sprite seedIcon)
    {
        _seedIcon.sprite = seedIcon;
    }

    private void OnMoneyChanged(int money)
    {
        _moneyText.text = money.ToString();
    }

    private void ChangeWater(int currentWater, int maxWater)
    {
        _waterBar.fillAmount = 1.0f * currentWater / maxWater;
    }
}
