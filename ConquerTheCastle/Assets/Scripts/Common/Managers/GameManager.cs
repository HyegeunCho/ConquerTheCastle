using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Castle from = _castles.FirstOrDefault(v => v.ID == _testCommands[0]);
            Castle to = _castles.FirstOrDefault(v => v.ID == _testCommands[1]);

            if (!from.Equals(to))
            {
                if (RoadManager.Instance.IsConnected(from, to))
                {
                    RoadManager.Instance.DisconnectCastle(from, to);    
                }
                else
                {
                    RoadManager.Instance.ConnectCastle(from, to);
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
    }
}
