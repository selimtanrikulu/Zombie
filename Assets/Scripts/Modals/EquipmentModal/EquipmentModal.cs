using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ScriptableObjects.Weapons;
using ScriptableObjects.Weapons.Guns;
using ScriptableObjects.Weapons.Melees;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;



public class EquipmentModal : MonoBehaviour,IModal
{
    [SerializeField] private VerticalLayoutGroup gunsGrid;
    [SerializeField] private VerticalLayoutGroup meleesGrid;
    [SerializeField] private VerticalLayoutGroup charactersGrid;
    [SerializeField] private VerticalLayoutGroup selectedItemFieldContainer;
    [SerializeField] private Image selectedItemImage;
    [SerializeField] private TMP_Text selectedItemNameText;
    [SerializeField] private TMP_Text selectedItemGradeText;
    [SerializeField] private TMP_Text myRankText;
    [SerializeField] private Sprite defaultWeaponSprite;

    
    [SerializeField] private Button equipButton;
    [SerializeField] private Button upgradeButton;
    
    [SerializeField] private Image selectedCharacterImage;
    [SerializeField] private Image selectedMelee;
    [SerializeField] private Image selectedGun1Image;
    [SerializeField] private Image selectedGun2Image;
    [SerializeField] private Image selectedGun3Image;

    private IItemManager _itemManager;
    private IPrefabCreator _prefabCreator;
    private IModalManager _modalManager;
    private ITextCreator _textCreator;
    private IRankManager _rankManager;


    [Inject]
    void Inject(IPrefabCreator prefabCreator, IItemManager itemManager,IModalManager modalManager,ITextCreator textCreator,IRankManager rankManager)
    {
        _rankManager = rankManager;
        _textCreator = textCreator;
        _prefabCreator = prefabCreator;
        _itemManager = itemManager;
        _modalManager = modalManager;
    }
    
    void Start()
    {
        CreateGunPrefabs();
        CreateCharacterPrefabs();
        CreateMeleePrefabs();
        SelfUpdate();
    }

    
    private void ClearSelectedItemFieldContainer()
    {
        foreach (Transform child in selectedItemFieldContainer.transform)
        {
            if (child.name.StartsWith("ItemField"))
            {
                Destroy(child.gameObject);
            }
            
        }
    }


    private void AddGunToGunsGrid(Weapon weapon)
    {
        GameObject confWeaponPrefab = _prefabCreator.InstantiatePrefab(PrefabType.EquipmentModalGridItem,new Vector3(),false);
        confWeaponPrefab.transform.SetParent(gunsGrid.transform,true);
        confWeaponPrefab.transform.localScale = new Vector3(1,1,1);
        EquipmentGridItem configurationLevelWeapon = confWeaponPrefab.GetComponent<EquipmentGridItem>();
        configurationLevelWeapon.StartAs(weapon);

    }

    
    private void AddMeleeToMeleesGrid(Melee melee)
    {
        GameObject confWeaponPrefab = _prefabCreator.InstantiatePrefab(PrefabType.EquipmentModalGridItem,new Vector3(),false);
        confWeaponPrefab.transform.SetParent(meleesGrid.transform,true);
        confWeaponPrefab.transform.localScale = new Vector3(1,1,1);
        EquipmentGridItem configurationLevelWeapon = confWeaponPrefab.GetComponent<EquipmentGridItem>();
        configurationLevelWeapon.StartAs(melee);
    }


    private void AddCharacterToCharactersGrid(Character character)
    {
        GameObject confCharacterPrefab = _prefabCreator.InstantiatePrefab(PrefabType.EquipmentModalGridItem,new Vector3(),false);
        confCharacterPrefab.transform.SetParent(charactersGrid.transform,true);
        confCharacterPrefab.transform.localScale = new Vector3(1,1,1);
        EquipmentGridItem configurationLevelCharacter = confCharacterPrefab.GetComponentInChildren<EquipmentGridItem>();
        configurationLevelCharacter.StartAs(character);
    }


    private void CreateGunPrefabs()
    {
        ItemPack itemPack = _itemManager.GetItemPack();
        foreach (Gun gun in itemPack.Guns)
        {

            AddGunToGunsGrid(gun);

        }
    }
    
    private void CreateMeleePrefabs()
    {
        ItemPack itemPack = _itemManager.GetItemPack();
        foreach (Melee melee in itemPack.Melees)
        {
            AddMeleeToMeleesGrid(melee);
        }

    }
    
    private void CreateCharacterPrefabs()
    {
        ItemPack itemPack = _itemManager.GetItemPack();
        foreach (Character character in itemPack.Characters)
        {
            AddCharacterToCharactersGrid(character);
        }
    }


    public void SelfUpdate()
    {
        selectedGun1Image.sprite = _itemManager.GetEquipment().GunSocket1 != null ? _itemManager.GetEquipment().GunSocket1.itemSprite : defaultWeaponSprite;

        selectedGun2Image.sprite = _itemManager.GetEquipment().GunSocket2 != null ? _itemManager.GetEquipment().GunSocket2.itemSprite : defaultWeaponSprite;

        selectedGun3Image.sprite = _itemManager.GetEquipment().GunSocket3 != null ? _itemManager.GetEquipment().GunSocket3.itemSprite : defaultWeaponSprite;

        selectedCharacterImage.sprite = _itemManager.GetEquipment().Character.itemSprite;
        selectedMelee.sprite = _itemManager.GetEquipment().MeleeSocket.itemSprite;


        if (_itemManager.GetSelectedItem() == null)
        {
            _itemManager.SetSelectedItem(_itemManager.GetEquipment().Character);
        }
        
        ClearSelectedItemFieldContainer();
        List<Field> allfields = _itemManager.GetSelectedItem().GetAllFields();

        foreach (Field field in allfields)
        {
            GameObject itemGameObject = _prefabCreator.InstantiatePrefab(PrefabType.ItemFieldPrefab, new Vector3(), false);
            Slider fieldSlider = itemGameObject.GetComponentInChildren<Slider>();
            TMP_Text itemFieldName = itemGameObject.GetComponentsInChildren<TMP_Text>().FirstOrDefault(x=>x.name == "ItemFieldName");
            TMP_Text currentValue = itemGameObject.GetComponentsInChildren<TMP_Text>()
                .FirstOrDefault(x => x.name == "CurrentValue_txt");

            fieldSlider.value = 0;
            fieldSlider.image.DOFillAmount((float)field.CurrentValue / field.MaxValue,0.3f);

            //fieldSlider.value = (float)field.CurrentValue / field.MaxValue;
            if (itemFieldName != null) itemFieldName.text = field.Name;
            if (currentValue != null) currentValue.text = field.CurrentValue.ToString();

            itemGameObject.transform.SetParent(selectedItemFieldContainer.transform,false);
        }

        selectedItemImage.sprite = _itemManager.GetSelectedItem()?.itemSprite;

        
        if(_rankManager.GetCurrentRank() >= _itemManager.GetSelectedItem().requiredRank)
        {
            selectedItemGradeText.text = "Grade "  +_itemManager.GetSelectedItem().Grade;
        }
        else
        {
            selectedItemGradeText.text = "";
        }
        
        
        
        
        selectedItemNameText.text = _itemManager.GetSelectedItem().ItemName;
        myRankText.text = "Rank " + _rankManager.GetCurrentRank();

        equipButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        
        
        TMP_Text equipText = equipButton.GetComponentInChildren<TMP_Text>();

        if (_rankManager.GetCurrentRank() >= _itemManager.GetSelectedItem().requiredRank)
        {
            if (_itemManager.IsItemEquipped(_itemManager.GetSelectedItem()))
            {
                if (_itemManager.GetSelectedItem() is Gun gun && _itemManager.GunEquippedOtherThan(gun))
                {
                    equipText.text = "Unequip";
                    equipButton.interactable = true;
                    equipButton.onClick.AddListener(UnequipButtonOnClick);
                }
                else
                {
                    equipText.text = "Equipped";
                    equipButton.interactable = false;
                }
                
               
            }
            else
            {
                equipText.text = "Equip";
                equipButton.interactable = true;
                equipButton.onClick.AddListener(EquipButtonOnClick);
            }
            
            
            upgradeButton.onClick.AddListener(UpgradeButtonOnClick);
            upgradeButton.gameObject.SetActive(true);
        }
        else
        {
            upgradeButton.gameObject.SetActive(false);
            
            equipText.text = "Req. Rank " + _itemManager.GetSelectedItem().requiredRank;
            equipButton.interactable = false;
        }



        EquipmentGridItem[] gunGridItems = gunsGrid.GetComponentsInChildren<EquipmentGridItem>();
        EquipmentGridItem[] meleeGridItems = meleesGrid.GetComponentsInChildren<EquipmentGridItem>();
        EquipmentGridItem[] characterGridItems = charactersGrid.GetComponentsInChildren<EquipmentGridItem>();

        foreach (EquipmentGridItem equipmentGridItem in gunGridItems)
        {
            equipmentGridItem.SelfUpdate();
        }
        
        foreach (EquipmentGridItem equipmentGridItem in meleeGridItems)
        {
            equipmentGridItem.SelfUpdate();
        }
        
        foreach (EquipmentGridItem equipmentGridItem in characterGridItems)
        {
            equipmentGridItem.SelfUpdate();
        }

    }

    private void UpgradeButtonOnClick()
    {
       _itemManager.UpgradeItem(_itemManager.GetSelectedItem());
       SelfUpdate();
    }

    private void UnequipButtonOnClick()
    {
        if (_itemManager.GetSelectedItem() is Gun gun)
        {
            _itemManager.UnWearGun(gun);
        }
        SelfUpdate();
    }

    private void EquipButtonOnClick()
    {
        if (_itemManager.GetSelectedItem() is Gun gun)
        {
            _modalManager.OpenModal(ModalType.EquipGunModal,true);
        }
        else if (_itemManager.GetSelectedItem() is Melee melee)
        {
            _itemManager.WearMelee(melee.meleeID);
            SelfUpdate();
        }
        else if (_itemManager.GetSelectedItem() is Character character)
        {
            _itemManager.WearCharacter(character.characterID);
            SelfUpdate();
        }
        
        
    }


    
}
