using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryController : MapObjectBase
{
    public MeshRenderer meshRenderer;
    public Material noramlMaterial;
    public Material ripeMaterial;
    public float time;
    private bool isRipe;
    private float timer;
    public float ripeSpeed = 1;

    public bool IsRipe
    {
        get => isRipe;
        set
        {

            if (isRipe!=value)
            {
                if (!value)
                {
                    timer = time;
                }

                isRipe = value;
                meshRenderer.material = isRipe ? ripeMaterial : noramlMaterial;
                CheckRipeState();

            }
        }
    }
    private void Update()
    {
        if (!isRipe)
        {
            timer -= Time.deltaTime * ripeSpeed;
            if (timer <= 0)
            {
                IsRipe = true;
            }
        }
    }
    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        IsRipe = true;
    }
    public void OnPick()
    {
        IsRipe = false;
    }

    public void CheckRipeState()
    {
        if (isRipe) MapManager.Instance.OnBerryRipe(this);
        else MapManager.Instance.RemoveBerryRipe(this);
    }
}
