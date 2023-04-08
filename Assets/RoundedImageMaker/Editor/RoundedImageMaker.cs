/*
 *  Created by 4litasci
 *  Checkout https://github.com/4litasci/Unity-Rounded-Image-Maker
 *  1.0.0
 */
using System.IO;
using UnityEditor;
using UnityEngine;

public class RoundedImageMaker : EditorWindow
{
    private int width = 256;
    private int height = 256;
    private int radius = 64;
    private float feather = 1f;
    private Color color = Color.white;
    private Texture _texture;
    private Texture2D _texture2D;
    
    [MenuItem("Window/Rounded Image Maker")]
    public static void ShowWindows()
    {
        GetWindow<RoundedImageMaker>("Rounded Image Maker");
    }
    
    private void OnGUI()
    {
        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);
        radius = EditorGUILayout.IntField("Radius", radius);
        feather = EditorGUILayout.FloatField("Feather", feather);
        color = EditorGUILayout.ColorField("Color", color);
        _texture = (Texture)EditorGUILayout.ObjectField("Override Texture", _texture, typeof(Texture), false);
        _texture2D = (Texture2D)EditorGUILayout.ObjectField("Override Texture2D", _texture2D, typeof(Texture2D), false);
        if (width < 1)
        {
            width = 1;
        }

        if (height < 1)
        {
            height = 1;
        }
        
        if (radius * 2 > width)
        {
            radius = width / 2;
        }
        
        if (radius * 2 > height)
        {
            radius = height / 2;
        }

        if (GUILayout.Button("Create"))
        {
            Create();
        }
    }

    void Create()
    {
        string overridePath = "";
        
        if (_texture != null)
        {
            overridePath = AssetDatabase.GetAssetPath(_texture);
        }
        
        if (_texture2D != null)
        {
            overridePath = AssetDatabase.GetAssetPath(_texture2D);
        }
        
        Texture2D image = new Texture2D(width, height, TextureFormat.RGBA32, true);
        for (int i = 0; i < image.width; i++)
        {
            for (int j = 0; j < image.height; j++)
            {
                image.SetPixel(i,j,color);
            }
        }

        var vector00 = new Vector2(radius, radius);
        for (int i = 0; i < radius; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                var current = new Vector2(i, j);
                if (Vector2.Distance(vector00,current)>radius+feather)
                {
                    image.SetPixel(i,j,Color.clear);
                }
                else if (Vector2.Distance(vector00,current) <=radius+feather && Vector2.Distance(vector00,current)>radius)
                {
                    var percent = (Vector2.Distance(vector00, current) - radius) / feather;
                    image.SetPixel(i,j,new Color(color.r,color.g,color.b,1-percent));
                }
            }
        }
        
        var vector01 = new Vector2(radius, image.height - radius);
        for (int i = 0; i < radius; i++)
        {
            for (int j = image.height-radius; j < image.height; j++)
            {
                var current = new Vector2(i, j);
                if (Vector2.Distance(vector01,current)>radius+feather)
                {
                    image.SetPixel(i,j,Color.clear);
                }
                else if (Vector2.Distance(vector01,current) <=radius+feather && Vector2.Distance(vector01,current)>radius)
                {
                    var percent = (Vector2.Distance(vector01, current) - radius) / feather;
                    image.SetPixel(i,j,new Color(color.r,color.g,color.b,1-percent));
                }
            }
        }
        var vector11 = new Vector2(image.width - radius, image.height - radius);
        for (int i = image.width-radius; i < image.width; i++)
        {
            for (int j = image.height-radius; j < image.height; j++)
            {
                var current = new Vector2(i, j);
                if (Vector2.Distance(vector11,current)>radius+feather)
                {
                    image.SetPixel(i,j,Color.clear);
                } 
                else if (Vector2.Distance(vector11,current) <=radius+feather && Vector2.Distance(vector11,current)>radius)
                {
                    var percent = (Vector2.Distance(vector11, current) - radius) / feather;
                    image.SetPixel(i,j,new Color(color.r,color.g,color.b,1-percent));
                }
                
            }
        }
        
        var vector10 = new Vector2(image.width - radius, radius);
        for (int i = image.width-radius; i < image.width; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                var current = new Vector2(i, j);
                if (Vector2.Distance(vector10,current)>radius+feather)
                {
                    image.SetPixel(i,j,Color.clear);
                }
                else if (Vector2.Distance(vector10,current) <=radius+feather && Vector2.Distance(vector10,current)>radius)
                {
                    var percent = (Vector2.Distance(vector10, current) - radius) / feather;
                    image.SetPixel(i,j,new Color(color.r,color.g,color.b,1-percent));
                }
                
            }
        }
        
        var bytes = image.EncodeToPNG();
        var path = string.IsNullOrEmpty(overridePath) ? Path.Combine(Application.dataPath,"EditorImageMaker",width+"-"+height+"-"+radius+"-"+feather+".png") : overridePath;
        
        File.WriteAllBytes(path,bytes);
        
        Debug.Log($"Created {path}");
        AssetDatabase.Refresh();
    }
}
