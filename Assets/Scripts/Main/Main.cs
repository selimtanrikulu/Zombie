using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class Main : MonoBehaviour
{
    private IModalManager _modalManager;
    private IItemManager _itemManager;

    [SerializeField] private Button rankButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button rollButton;
    
    [SerializeField] private Button spinButton;
    [SerializeField] private Button goalsButton;
    [SerializeField] private Button dailyButton;
    [SerializeField] private Button freeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button playButton;
    


    [Inject]
    void Inject(IModalManager modalManager,IItemManager itemManager)
    {
        _itemManager = itemManager;
        _modalManager = modalManager;
    }

    private void Start()
    {
        rankButton.onClick.AddListener(() => OpenModal(ModalType.RankModal,true));
        itemButton.onClick.AddListener(() => OpenModal(ModalType.ItemModal,true));
        shopButton.onClick.AddListener(() => OpenModal(ModalType.ShopModal,true));
        spinButton.onClick.AddListener(() => OpenModal(ModalType.SpinModal,true));
        goalsButton.onClick.AddListener(() => OpenModal(ModalType.GoalsModal,true));
        dailyButton.onClick.AddListener(() => OpenModal(ModalType.DailyModal,true));
        freeButton.onClick.AddListener(() => OpenModal(ModalType.FreeModal,true));
        settingsButton.onClick.AddListener(() => OpenModal(ModalType.SettingsModal,true));
        playButton.onClick.AddListener(PlayButtonOnClick);
        rollButton.onClick.AddListener(RollButtonOnClick);
    }

    private void RollButtonOnClick()
    {
        _itemManager.ResetItemGrades();
    }


    public void OpenModal(ModalType modalType,bool over)
    {
        _modalManager.OpenModal(modalType,over);
    }
    
    private void PlayButtonOnClick()
    {
        SceneManager.LoadScene("GameLevel");
    }
}
