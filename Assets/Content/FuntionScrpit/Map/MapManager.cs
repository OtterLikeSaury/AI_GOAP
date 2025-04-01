using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public Transform CampfirePoint;
    public Stack<Vector2Int> backupCells = new Stack<Vector2Int>(); // 放置物品的备用格子
    public int currentCircleNum = 2;    // 圈数
    public float cellSize = 2f;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ReserveFoodCount = 10;
        RoleCount = 0;
        UIManager.Instance.SetRipeBerryCount(ripeBerryCount);
    }
    private void Update()
    {
        UpdateSpawnRole();
    }
    private void CreateBackupCells()
    {
        currentCircleNum += 1;
        int minX = -currentCircleNum;
        int minY = -currentCircleNum;
        int sideLenght = currentCircleNum * 2 + 1; // 一侧的长度
        // 遍历左边 x固定最小值
        for (int y = minY; y < minY + sideLenght; y++)
        {
            backupCells.Push(new Vector2Int(minX, y));
        }
        // 遍历上边 y是固定最大值
        int maxY = minY + sideLenght - 1;
        for (int x = minX + 1; x < minX + sideLenght; x++)
        {
            backupCells.Push(new Vector2Int(x, maxY));
        }
        // 遍历右边 x是固定最大值
        int maxX = minX + sideLenght - 1;
        for (int y = maxY - 1; y >= minY; y--)
        {
            backupCells.Push(new Vector2Int(maxX, y));
        }
        // 遍历下边 y是固定的最小值
        for (int x = maxX - 1; x > minX; x--)
        {
            backupCells.Push(new Vector2Int(x, minY));
        }
    }

    public Vector3 GetCellPosition(Vector2Int coord)
    {
        return new Vector3(coord.x * cellSize, 0, coord.y * cellSize);
    }
    public Vector2Int GetNextBuildCoord()
    {
        if (backupCells.Count == 0)
        {
            CreateBackupCells();
        }
        return backupCells.Pop();
    }

    #region 浆果

    [Header("浆果")]
    public Transform berryRoot;
    public GameObject berryPrefab;
    [ReadOnly] public HashSet<BerryController> allBerries = new HashSet<BerryController>();
    public HashSet<BerryController> waitingAssignBerries = new HashSet<BerryController>();
    [ReadOnly] public int ripeBerryCount
    {
        get
        {
            int counter=0;
            foreach (BerryController item in allBerries)
            {
                if (item.IsRipe == true) counter++;

            }
            return counter;
        }
    }

    public BerryController SpawnBerry(Vector2Int coord)
    {
        BerryController berry = GameObject.Instantiate(berryPrefab, GetCellPosition(coord), Quaternion.identity, berryRoot).GetComponent<BerryController>();

        allBerries.Add(berry);    //将新生成的浆果丛加入管理
        if (berry.IsRipe==true)
        {
            waitingAssignBerries.Add(berry);
        }

        return berry;
    }

    public void OnBerryRipe(BerryController berryController)
    {
        if (waitingAssignBerries.Add(berryController))
        {
            UIManager.Instance.SetRipeBerryCount(ripeBerryCount);
            GOAPGlobal.instance.GlobalStates.GetState<IntState>("成熟浆果的数量").SetValue(ripeBerryCount);
        }
    }  //浆果成熟

    public void RemoveBerryRipe(BerryController berryController)
    {
        if (waitingAssignBerries.Contains(berryController)) waitingAssignBerries.Remove(berryController);

        UIManager.Instance.SetRipeBerryCount(ripeBerryCount);
        GOAPGlobal.instance.GlobalStates.GetState<IntState>("成熟浆果的数量").SetValue(ripeBerryCount);
    }  //浆果不成熟

    /// <summary>
    /// 请求分配一个空浆果丛
    /// </summary>
    /// <returns></returns>
    public BerryController RoleTryGetRipeBerry()
    {
        if (waitingAssignBerries.Count == 0) return null;
        BerryController berry = null;
        foreach (BerryController item in waitingAssignBerries)
        {
            berry = item;
            break;
        }

        if (berry!=null)
        {
            waitingAssignBerries.Remove(berry);
        }
        else
        {
            Debug.Log(waitingAssignBerries.Count);
        }

        return berry;
    }

    #endregion

    #region 食物
    private int reserveFoodCount;
    public int ReserveFoodCount
    {
        get => reserveFoodCount;
        set
        {
            reserveFoodCount = value;
            UIManager.Instance.SetReserveFoodCount(reserveFoodCount);
            GOAPGlobal.instance.GlobalStates.GetState<IntState>("储备食物的数量").SetValue(reserveFoodCount);
        }
    }

    public void AddFood(int count)
    {
        ReserveFoodCount += count;
    }

    #endregion

    #region 村民
    [Header("村民")]
    public GameObject rolePrefab;
    public Transform roleRoot;
    public int maxRoleCount = 10;
    public float spawnRoleInterval = 3;
    private int roleCount;
    public int RoleCount
    {
        get => roleCount;
        set
        {
            roleCount = value;
            UIManager.Instance.SetRoleCount(roleCount);
        }
    }


    private float spawnRoleTimer;

    private void UpdateSpawnRole()
    {
        if (roleCount >= maxRoleCount) return;
        spawnRoleTimer -= Time.deltaTime;
        if (spawnRoleTimer <= 0)
        {
            spawnRoleTimer = spawnRoleInterval;
            if (reserveFoodCount > roleCount * 3) // 食物是人口的三倍则产生新人口
            {
                Vector3 pos = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                GameObject.Instantiate(rolePrefab, pos, Quaternion.identity, roleRoot);
                RoleCount += 1;
            }
        }
    }

    public void OnRoleDie()
    {
        RoleCount -= 1;
    }

    public void OnRoleEat()
    {
        ReserveFoodCount -= 1;
    }


    #endregion
}
