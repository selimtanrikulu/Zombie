using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameLevel : MonoBehaviour
{
    private IPrefabCreator _prefabCreator;
    private GameLevelUI _gameLevelUI;
    
    
    [Inject]
    void Inject(IPrefabCreator prefabCreator)
    {
        _prefabCreator = prefabCreator;
    }
    
    void Start()
    {
        _gameLevelUI = FindObjectOfType<GameLevelUI>();
        GameObject playerGameObject = _prefabCreator.InstantiatePrefab(PrefabType.Player,new Vector3(),false);
        if (playerGameObject.TryGetComponent(out Player player))
        {
            _gameLevelUI.PlayerCreated(player);
        }
    }
    
}
