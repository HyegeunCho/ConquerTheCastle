using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HGPlugins;
using HGPlugins.Singleton;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Castle[] _castles;

    private List<int> _testCommands = new List<int>();
    private void Update()
    {
        if (_testCommands.Count >= 2)
        {
            Castle from = FindCastle(_testCommands[0]);
            Castle to = FindCastle(_testCommands[1]);

            if (from == null || to == null)
            {
                _testCommands.Clear();
                return;
            }

            if (!from.Equals(to))
            {
                if (RoadManager.Instance.IsConnected(from, to))
                {
                    from.DisconnectFrom(to);
                }
                else
                {
                    from.ConnectTo(to);
                }
            }
            
            _testCommands.Clear();
            return;
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            _testCommands.Add(1);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            _testCommands.Add(2);
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            _testCommands.Add(3);
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            _testCommands.Add(4);
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            _testCommands.Add(5);
        }

        if (Input.GetKeyUp(KeyCode.Keypad1))
        {
            Castle target = FindCastle(1);
            if (target == null) return;
            SoldierManager.Instance.GetSoldier(Enums.EColor.Blue, target);
        }

        if (Input.GetKeyUp(KeyCode.Keypad2))
        {
            Castle target = FindCastle(2);
            if (target == null) return;
            SoldierManager.Instance.GetSoldier(Enums.EColor.Green, target);
        }

        if (Input.GetKeyUp(KeyCode.Keypad3))
        {
            Castle target = FindCastle(3);
            if (target == null) return;
            SoldierManager.Instance.GetSoldier(Enums.EColor.Yellow, target);
        }
    }

    private Castle FindCastle(int inId)
    {
        if (_castles == null || _castles.Length == 0) return null;
        return _castles.FirstOrDefault(v => v.ID == inId);
    }
}
