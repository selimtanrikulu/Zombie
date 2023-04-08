using System;
using System.Collections.Generic;
using ScriptableObjects.Weapons.Guns;
using ScriptableObjects.Weapons.Melees;
using UnityEngine;
using Zenject;

[Serializable]
public class ItemPack
{
    public List<Gun> Guns;
    public List<Melee> Melees;
    public List<Character> Characters;
    
}


[Serializable]

public struct PrefabsPack
{
    public List<GamePrefab> GamePrefabs;
}
    
[Serializable]
public struct GamePrefab
{
    public PrefabType PrefabType;
    public GameObject Prefab;
}

[Serializable]
public struct ModalsPack
{
    public List<GameModal> GameModals;
}

[Serializable]
public struct GameModal
{
    public ModalType ModalType;
    public GameObject Prefab;
}
    
    
public class GameInstaller : MonoInstaller
{
        

    [SerializeField] private ItemPack itemPack;
    [SerializeField] private PrefabsPack prefabsPack;
    [SerializeField] private ModalsPack modalsPack;
    public override void InstallBindings()
    {

        Container.BindInstance(itemPack);
        Container.BindInstance(prefabsPack);
        Container.BindInstance(modalsPack);
        
        Container.Bind<IItemManager>().To<ItemManager>().AsSingle();
        Container.Bind<IPrefabCreator>().To<PrefabCreator>().AsSingle();
        Container.Bind<IAudioManager>().To<AudioManager>().AsSingle();
        Container.Bind<ITextCreator>().To<TextCreator>().AsSingle();
        Container.Bind<IModalManager>().To<ModalManager>().AsSingle();
        Container.Bind<IRankManager>().To<RankManager>().AsSingle();
    }
}