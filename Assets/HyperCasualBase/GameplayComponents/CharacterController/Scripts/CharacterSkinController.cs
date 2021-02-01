using System.Collections.Generic;
using UnityEngine;

public class CharacterSkinController : MonoBehaviour
{
    Character character;
    Character Character { get { return (character == null) ? character = GetComponentInParent<Character>() : character; } }

    private List<CharacterSkin> characterSkins;
    public List<CharacterSkin> CharacterSkins { get { return (characterSkins == null) ? characterSkins = new List<CharacterSkin>(GetComponentsInChildren<CharacterSkin>(true)) : characterSkins; } }


    private void OnEnable()
    {
        Character.OnCharacterSet.AddListener(SetCharacter);
        Character.OnSkinChange.AddListener(SetSkin);
    }

    private void OnDisable()
    {
        Character.OnCharacterSet.RemoveListener(SetCharacter);
        Character.OnSkinChange.RemoveListener(SetSkin);
    }


    public void SetCharacter()
    {
        switch (Character.CharacterControlType)
        {
            case CharacterControlType.Player:
                SetPlayer();
                break;
            case CharacterControlType.AI:
                SetAI();
                break;
            case CharacterControlType.None:
                SetPlayer();
                break;
            default:
                SetPlayer();
                break;
        }
    }


    private void SetPlayer()
    {
        PlayerData playerData = SaveLoadManager.LoadPDP<PlayerData>(SavedFileNameHolder.PlayerData, new PlayerData());

        if (playerData == null) return;

        AnalitycsManager.Instance.LogEvent("Skin_Event", "PlayerWearingSkin", playerData.CurrentSkin);

        SkinType skinType;
        if (playerData.CurrentSkin != null && SkinManager.Instance.GetSkin(playerData.CurrentSkin, out skinType))
        {
            switch (skinType)
            {
                case SkinType.FullBody:
                    CloseAllSkins();
                    OpenSkin(playerData.CurrentSkin, skinType);
                    return;
                default:
                    OpenSkin(playerData.CurrentSkin, skinType);
                    break;
            }
        }
    }

    private void SetAI()
    {
        bool isFullBody = Random.Range(0f, 1f) > 0.5f;
        List<string> skins = new List<string>();

        PlayerData playerData = SaveLoadManager.LoadPDP<PlayerData>(SavedFileNameHolder.PlayerData, new PlayerData());

        CloseAllSkins();
        //Get a full body skin and set it
        skins = GetSkinIDs(SkinType.FullBody);
        skins.Remove(playerData.CurrentSkin);
        if (skins.Count > 0)
        {
            OpenSkin(skins[Random.Range(0, skins.Count)], SkinType.FullBody);
            skins.Clear();
        }

        #region CobineItems
        //if (isFullBody)
        //{
        //    CloseAllSkins();
        //    //Get a full body skin and set it
        //    skins = GetSkinIDs(SkinType.FullBody);
        //    if (skins.Count > 0)
        //    {
        //        OpenSkin(skins[Random.Range(0, skins.Count)], SkinType.FullBody);
        //        skins.Clear();
        //    }
        //}
        //else
        //{
        //    bool isActivePart = Random.Range(0f, 1f) > 0.5f;
        //    // Mix multiple skins
        //    //Set Hat
        //    if (isActivePart)
        //    {
        //        skins = GetSkinIDs(SkinType.Hat);
        //        if (skins.Count > 0)
        //        {
        //            OpenSkin(skins[Random.Range(0, skins.Count)], SkinType.Hat);
        //            skins.Clear();
        //        }
        //    }
        //    isActivePart = Random.Range(0f, 1f) > 0.5f;
        //    //Set Face
        //    if (isActivePart)
        //    {
        //        skins = GetSkinIDs(SkinType.Face); if (skins.Count > 0)
        //        {
        //            OpenSkin(skins[Random.Range(0, skins.Count)], SkinType.Face);
        //            skins.Clear();
        //        }
        //    }
        //    //isActivePart = Random.Range(0f, 1f) > 0.5f;
        //    ////Set Pants
        //    //if(isActivePart)
        //    //{
        //    //    skins = GetSkinIDs(SkinType.Pants);
        //    //    OpenSkin(skins[Random.Range(0, skins.Count)], SkinType.Pants);
        //    //    skins.Clear();
        //    //}
        //}
        #endregion
    }

    private void SetSkin(Skin skin)
    {
        OpenSkin(skin.SkinID, skin.SkinType);
    }

    private void OpenSkin(string skinID, SkinType skinType)
    {
        CloseSkin(skinType);
        foreach (var skin in CharacterSkins)
        {
            if (string.Equals(skin.SkinID, skinID))
                skin.OpenSkin();
        }
    }

    public void CloseSkin(SkinType skinType)
    {
        foreach (var skin in CharacterSkins)
        {
            if(SkinManager.Instance.GetSkinByType(skin.SkinID, skinType))
                skin.CloseSkin();
        }
    }

    public void CloseAllSkins()
    {
        for (int i = 0; i < CharacterSkins.Count; i++)
        {
            CharacterSkins[i].CloseSkin();
        }
    }

    private List<string> GetSkinIDs(SkinType skinType)
    {
        List<string> skins = new List<string>();
        for (int i = 0; i < CharacterSkins.Count; i++)
        {
            if (SkinManager.Instance.GetSkinByType(CharacterSkins[i].SkinID, skinType))
                skins.Add(CharacterSkins[i].SkinID);
        }
        return skins;
    }
}
