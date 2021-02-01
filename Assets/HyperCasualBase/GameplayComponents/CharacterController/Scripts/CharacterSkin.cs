using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class CharacterSkin : MonoBehaviour
{
    [ValueDropdown("GetSkins")]
    public string SkinID;

#if UNITY_EDITOR
    private static List<string> GetSkins()
    {
        string[] guid = UnityEditor.AssetDatabase.FindAssets("t:SkinData");
        List<SkinData> skinDatas = new List<SkinData>();
        for (int i = 0; i < guid.Length; i++)
        {
            skinDatas.Add(UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(guid[i]), typeof(SkinData)) as SkinData);
        }
        List<string> keys = new List<string>();
        foreach (var item in skinDatas)
        {
            foreach (var skin in item.Skins)
            {
                if (!keys.Contains(skin.SkinID))
                    keys.Add(skin.SkinID);
            }
            
        }
        return keys;
    }
#endif

    SkinnedMeshRenderer meshRenderer;
    SkinnedMeshRenderer MeshRenderer { get { return (meshRenderer == null) ? meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>() : meshRenderer; } }

    [Button]
    public void OpenSkin()
    {
        MeshRenderer.enabled = true;
    }

    [Button]
    public void CloseSkin()
    {
        MeshRenderer.enabled = false;
    }
}
