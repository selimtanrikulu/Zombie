using System.Linq;
using ScriptableObjects.Weapons.Guns;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EquipGunModal : MonoBehaviour
{
    [SerializeField] private Button gun1Socket;
    [SerializeField] private Button gun2Socket;
    [SerializeField] private Button gun3Socket;
    private IItemManager _itemManager;
    private IModalManager _modalManager;

    [Inject]
    void Inject(IItemManager itemManager,IModalManager modalManager)
    {
        _itemManager = itemManager;
        _modalManager = modalManager;
    }
    
    void Start()
    {
        MapGunImages();
        
        
        gun1Socket.onClick.AddListener(Gun1SocketOnClick);
        gun2Socket.onClick.AddListener(Gun2SocketOnClick);
        gun3Socket.onClick.AddListener(Gun3SocketOnClick);
    }

    private void MapGunImages()
    {
        Image gunImage1 = gun1Socket.GetComponentsInChildren<Image>().ToList().Find(x=>x.name == "GunImage");
        Image gunImage2 = gun2Socket.GetComponentsInChildren<Image>().ToList().Find(x=>x.name == "GunImage");
        Image gunImage3 = gun3Socket.GetComponentsInChildren<Image>().ToList().Find(x=>x.name == "GunImage");

        gunImage1.sprite = _itemManager.GetEquipment().GunSocket1 ? _itemManager.GetEquipment().GunSocket1.itemSprite : gunImage1.sprite;
        gunImage2.sprite = _itemManager.GetEquipment().GunSocket2 ? _itemManager.GetEquipment().GunSocket2.itemSprite : gunImage2.sprite;
        gunImage3.sprite = _itemManager.GetEquipment().GunSocket3 ? _itemManager.GetEquipment().GunSocket3.itemSprite : gunImage3.sprite;

    }


    private void Gun1SocketOnClick()
    {
        if (_itemManager.GetSelectedItem() is Gun gun)
        {
            _itemManager.WearGun(gun.gunID,GunSocketID.Socket1);
        }
        
        _modalManager.CloseModal();
    }
    private void Gun2SocketOnClick()
    {
        if (_itemManager.GetSelectedItem() is Gun gun)
        {
            _itemManager.WearGun(gun.gunID,GunSocketID.Socket2);
        }
        
        _modalManager.CloseModal();
    }
    private void Gun3SocketOnClick()
    {
        if (_itemManager.GetSelectedItem() is Gun gun)
        {
            _itemManager.WearGun(gun.gunID,GunSocketID.Socket3);
        }
        
        _modalManager.CloseModal();
    }

    

    
}
