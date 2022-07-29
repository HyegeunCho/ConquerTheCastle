using System.Collections;
using System.Collections.Generic;
using HGPlugins.Singleton;
using UnityEngine;

public class SoldierManager : MonoSingleton<SoldierManager>
{
    private GameObject _soldierPrefab;
    private Queue<Soldier> _soldierPool;

    private GameObject prototype
    {
        get
        {
            if (_soldierPrefab == null)
            {
                _soldierPrefab = Resources.Load<GameObject>("Soldier/Prefab/Soldier");
            }

            return _soldierPrefab;
        }
    } 
    
    public override void Awake()
    {
        base.Awake();
        _soldierPool = new Queue<Soldier>();
    }

    private Soldier CreateSoldier()
    {
        Soldier result = Instantiate(prototype, transform, true).GetComponent<Soldier>();
        return result;
    }

    public Soldier GetSoldier(Enums.EColor inColor, Castle inAt)
    {
        if (inAt == null || !inAt.IsAlive) return null;
        
        Soldier result = _soldierPool.Count > 0 ?_soldierPool.Dequeue() :  CreateSoldier();
        if (result != null)
        {
            Vector3 targetAt = inAt.transform.position;
            targetAt.Set(targetAt.x, 0.1f, targetAt.z);
            
            result.transform.SetPositionAndRotation(targetAt, Quaternion.identity);
            result.SetColor(inColor);
            result.SetHomeCastle(inAt);
            result.gameObject.SetActive(true);
        }

        return result;
    }

    public void ReturnSoldier(Soldier inSoldier)
    {
        inSoldier.gameObject.SetActive(false);
        _soldierPool.Enqueue(inSoldier);
    }
}
