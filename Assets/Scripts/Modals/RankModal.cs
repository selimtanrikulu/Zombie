using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RankModal : MonoBehaviour
{
    [SerializeField] private Button rankUpButton;
    private IRankManager _rankManager;


    [Inject]
    void Inject(IRankManager rankManager)
    {
        _rankManager = rankManager;
    }
    void Start()
    {
        rankUpButton.onClick.AddListener(RankUpButtonOnClick);
    }

    private void RankUpButtonOnClick()
    {
        _rankManager.RankUp();
    }
}
