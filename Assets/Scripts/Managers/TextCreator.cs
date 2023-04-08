using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public interface ITextCreator
{

    void CreateText(string message, Transform parent,float relativeStartPosY, float relativeEndPosY, float duration, int fontSize,Color color);


}

public class TextCreator : ITextCreator
{
    private readonly IPrefabCreator _prefabCreator;

    
    
    TextCreator(IPrefabCreator prefabCreator)
    {
        _prefabCreator = prefabCreator;
    }


    public void CreateText(string message, Transform parent, float relativeStartPosY,float relativeEndPosY, float duration, int fontSize,Color color)
    {
        GameObject textGameObject =
            _prefabCreator.InstantiatePrefab(PrefabType.TextPrefab, new Vector3(), false, duration, false);


        textGameObject.transform.SetParent(parent,false);
        textGameObject.transform.localPosition = new Vector3(0,relativeStartPosY,0);
        textGameObject.transform.DOLocalMoveY(relativeEndPosY, duration);
        

        
        if (textGameObject.TryGetComponent(out Text text))
        {
            text.fontSize = fontSize;
            text.color = color;
            text.text = message;
        }
    }
}
