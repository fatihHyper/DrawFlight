using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField]
    [OnValueChanged("CreateGroup")]
    private Vector3Int areaSize { get { return Vector3Int.CeilToInt(new Vector3(transform.localScale.x, transform.position.y, transform.localScale.z)); } }

    [ValueDropdown("CharacterDatas")]
    [OnValueChanged("CreateGroup")]
    public CharacterData CharacterData;

#if UNITY_EDITOR
    private static IEnumerable CharacterDatas()
    {
        return UnityEditor.AssetDatabase.FindAssets("t:CharacterData")
            .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
            .Select(x => new ValueDropdownItem(x, UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterData>(x)));
    }
#endif

    [HideInInspector]
    public List<Vector3> SpawnPoints;

    [Range(0, 3)]
    public float spaceX = 1.0f;
    [Range(0, 3)]
    public float spaceZ = 1.0f;

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.AddListener(CreateCharacters);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.RemoveListener(CreateCharacters);
    }


    private void OnDrawGizmos()
    {

        if(CharacterData.CharacterControlType == CharacterControlType.Player)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.2f);
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f);

        Vector3 startPos = new Vector3(transform.position.x, areaSize.y, transform.position.z) - (Vector3)areaSize / 2.0f;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, startPos.y, transform.position.z), new Vector3(areaSize.x, 0.2f, areaSize.z));
        float distX = 0;
        float distZ = 0;
        for (int x = 0; x <= areaSize.x; x++)
        {
            for (int z = 0; z <= areaSize.z; z++)
            {
                Gizmos.DrawWireSphere(startPos + new Vector3(distX, 0, distZ), 0.1f);
                distZ += spaceZ;
            }
            distZ = 0;
            distX += spaceX;
        }
    }

    [Button]
    public void CreateGroup()
    {
        SpawnPoints = new List<Vector3>();
        if (CharacterData.CharacterControlType == CharacterControlType.Player)
        {
            SpawnPoints.Add(transform.position);
            return;
        }

        Vector3 startPos = transform.position - (Vector3)areaSize / 2.0f;

        Vector3 dist = Vector3.zero;
        for (int x = 0; x <= areaSize.x; x++)
        {
            for (int z = 0; z <= areaSize.z; z++)
            {
                SpawnPoints.Add(startPos + dist);
                dist.z += spaceZ;
            }
            dist.z = 0;
            dist.x += spaceX;
        }
    }

    public void CreateCharacters()
    {
        CreateGroup();

        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            CharacterManager.Instance.CreateCharacter(CharacterData, SpawnPoints[i]);
        }
    }



}
