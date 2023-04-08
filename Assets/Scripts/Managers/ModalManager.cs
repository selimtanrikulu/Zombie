using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;


[Serializable]
public enum ModalType
{
    RankModal,
    ItemModal,
    ShopModal,
    SpinModal,
    GoalsModal,
    DailyModal,
    FreeModal,
    SettingsModal,
    EquipGunModal,
}

public interface IModal
{
    void SelfUpdate();
}


public interface IModalManager
{
    void OpenModal(ModalType modalType,bool over);
    void CloseModal();
}
public class ModalManager : IModalManager
{

    private Stack<GameObject> _openedModals = new Stack<GameObject>();
    private readonly Stack<Stack<GameObject>> _backUpModalPacks = new Stack<Stack<GameObject>>();
    private Canvas _canvas;


    private readonly IPrefabCreator _prefabCreator;
    
    ModalManager(IPrefabCreator prefabCreator)
    {
        _prefabCreator = prefabCreator;
    }
    
    

    public void CloseModal()
    {
        if (_openedModals.Count < 1)
        {
            Debug.LogError("Error ! Wrong Usage...");
            return;
        }

        var modalToClose = _openedModals.Pop();

        Object.Destroy(modalToClose);

        if (_openedModals.Count == 0)
        {
            if (_backUpModalPacks.Count > 0)
            {
                _openedModals = _backUpModalPacks.Pop();
                foreach (var modal in _openedModals)
                {
                    modal.SetActive(true);
                    modal.transform.DOScale(Vector3.zero, .4f).From().SetEase(Ease.OutBack).SetUpdate(true);
                    
                }
            }
        }


        foreach (GameObject modal in _openedModals)
        {
            if (modal.TryGetComponent(out IModal iModal))
            {
                iModal.SelfUpdate();
            }
        }
        
    }

    public void OpenModal(ModalType modalType,bool over)
    {
        if (!_canvas)
        {
            _canvas = Object.FindObjectOfType<Canvas>();
            if (!_canvas)
            {
                Debug.LogError("Canvas not found");
                return;
            }
        }


        // will be instantiated
        var newModal = _prefabCreator.InstantiateModal(modalType);
        newModal.transform.SetParent(_canvas.transform,false);

        if (over)
        {
            _openedModals.Push(newModal);
        }
        else
        {
            if (_openedModals.Count > 0)
            {
                foreach (var modal in _openedModals)
                {
                    modal.SetActive(false);
                }

                Stack<GameObject> pushBackUp = new Stack<GameObject>();
                while (_openedModals.Count > 0)
                {
                    pushBackUp.Push(_openedModals.Pop());
                }

                Stack<GameObject> pushBackUp2 = new Stack<GameObject>();
                while (pushBackUp.Count > 0)
                {
                    pushBackUp2.Push(pushBackUp.Pop());
                }

                _backUpModalPacks.Push(pushBackUp2);
            }

            _openedModals.Push(newModal);
        }
        
        newModal.transform.DOScale(Vector3.zero, .4f).From().SetEase(Ease.OutBack).SetUpdate(true);
        //Below add listeners to the buttons on the modal (for all modals)
        var modalButtons = newModal.GetComponentsInChildren<Button>();
        Button xButton = modalButtons.FirstOrDefault(x => x.name == "XButton");

        if (xButton != null)
        {
            xButton.onClick.AddListener(CloseModal);
        }
    }
}


