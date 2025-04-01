using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text roleCountText;
    public Text ripeBerryCount;
    public Text reserveFoodCount;
    private void Awake()
    {
        Instance = this;
    }
    public void SetRoleCount(int count)
    {
        roleCountText.text = $"��ɫ����:{count}";
    }
    public void SetReserveFoodCount(int count)
    {
        reserveFoodCount.text = $"����ʳ��:{count}";
    }
    public void SetRipeBerryCount(int count)
    {
        ripeBerryCount.text = $"���콬��:{count}";
    }
}
