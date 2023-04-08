using ScriptableObjects.Weapons.Guns;
using ScriptableObjects.Weapons.Melees;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EquipmentGridItem : MonoBehaviour
{
    private EquipmentModal _equipmentModal;
    public Item item;
    
    [SerializeField] private Button itemButton;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text requiredRankText;

    [SerializeField] private Color selectedColor;
    [SerializeField] private Color lockedColor;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color equippedColor;
    
    
    private IItemManager _itemManager;
    private IRankManager _rankManager;


    [Inject]
    void Inject(IItemManager itemManager,IRankManager rankManager)
    {
        _rankManager = rankManager;
        _itemManager = itemManager;
    }
    
    public void StartAs(Item item)
    {
        this.item = item;
        itemButton.onClick.AddListener(ItemButtonOnClick);
        itemImage.sprite = item.itemSprite;
        
        _equipmentModal = FindObjectOfType<EquipmentModal>();
        
        SelfUpdate();
    }

    private void ItemButtonOnClick()
    {
        _itemManager.SetSelectedItem(item);
        _equipmentModal.SelfUpdate();
    }


    public void SelfUpdate()
    {
        ColorUpdate();
        TextUpdate();
    }

    private void TextUpdate()
    {
        if (_rankManager.GetCurrentRank()<item.requiredRank)
        {
            requiredRankText.text = "Req. Rank " +item.requiredRank;
        }
        else if (_itemManager.IsItemEquipped(item))
        {
            requiredRankText.text = "Equipped";
        }
        else
        {
            requiredRankText.text = "Unlocked";
        }
    }


    private void ColorUpdate()
    {
        if (_itemManager.GetSelectedItem() == item)
        {
            backgroundImage.color = selectedColor;
        }
        else if(_itemManager.IsItemEquipped(item))
        {
            backgroundImage.color = equippedColor;
        }
        else if (_rankManager.GetCurrentRank() >= item.requiredRank)
        {
            backgroundImage.color = unlockedColor;
        }
        else
        {
            backgroundImage.color = lockedColor;
        }
    }
    
}
