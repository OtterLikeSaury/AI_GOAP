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
        roleCountText.text = $"角色数量:{count}";
    }
    public void SetReserveFoodCount(int count)
    {
        reserveFoodCount.text = $"储备食物:{count}";
    }
    public void SetRipeBerryCount(int count)
    {
        ripeBerryCount.text = $"成熟浆果:{count}";
    }
}
