using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;


public enum PrefabType
{
    Player,
    Zombie1,
    Zombie2,
    MuzzleFlash1,
    AudioSourceContainer,
    TextPrefab,
    ItemFieldPrefab,
    EquipmentModalGridItem,
    
    
    
    //etc....
}

public interface IPrefabCreator
{
    GameObject InstantiatePrefab(PrefabType prefabType,Vector3 position,bool dontDestroyOnLoad);
    GameObject InstantiatePrefab(PrefabType prefabType, Vector3 position, bool inverseDirection, float lifeTime, bool dontDestroyOnLoad);

    GameObject InstantiateModal(ModalType modalType);

}

public class PrefabCreator:IPrefabCreator
{
    private readonly PrefabsPack _prefabsPack;
    private readonly DiContainer _diContainer;


    private Camera _camera;
    private readonly ModalsPack _modalsPack;



    PrefabCreator(PrefabsPack prefabsPack,DiContainer diContainer,ModalsPack modalsPack)
    {
        _modalsPack = modalsPack;
        _diContainer = diContainer;
        _prefabsPack = prefabsPack;
    }


    public GameObject InstantiatePrefab(PrefabType prefabType,Vector3 position,bool dontDestroyOnLoad)
    {
        GameObject prefab = _prefabsPack.GamePrefabs.Find(x => x.PrefabType == prefabType).Prefab;
        GameObject go = _diContainer.InstantiatePrefab(prefab,position,Quaternion.identity, null);
        
        if (!dontDestroyOnLoad)
        {
            //to avoid dontdestroy on load, parentize the object out of DontDestroyOnLoad
            if(_camera == null)_camera =Camera.main;
            if (_camera != null) go.transform.SetParent(_camera.transform);
        }
        
        return go;
    }
    
    
    public GameObject InstantiatePrefab(PrefabType prefabType,Vector3 position,bool inverseDirection,float lifeTime,bool dontDestroyOnLoad)
    { 
        if(_camera == null)_camera =Camera.main;
        
        GameObject prefab = _prefabsPack.GamePrefabs.Find(x => x.PrefabType == prefabType).Prefab;
        GameObject go = _diContainer.InstantiatePrefab(prefab,position,Quaternion.identity, null);
        Vector3 localScale = go.transform.localScale;
        localScale.x = inverseDirection ? -localScale.x : localScale.x;
        go.transform.localScale = localScale;
        Object.Destroy(go,lifeTime);
        
        
        if (!dontDestroyOnLoad)
        {
            //to avoid dontdestroy on load, parentize the object out of DontDestroyOnLoad
            if(_camera == null)_camera =Camera.main;
            if (_camera != null) go.transform.SetParent(_camera.transform);
        }
        
        
        return go;
    }

    public GameObject InstantiateModal(ModalType modalType)
    {
        GameModal gameModal = _modalsPack.GameModals.Find(x => x.ModalType == modalType);
        GameObject prefab = gameModal.Prefab;
        GameObject go = _diContainer.InstantiatePrefab(prefab);
        return go;
    }

    
}