using System.Collections;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    #region Singleton
    public static SceneHandler Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion

    [HideInInspector]
    public string Level1Scene, Level2Scene, Level3Scene, LobbyScene;

    #if UNITY_EDITOR //only load this field in the editor for developer assignment in the scriptable object
    [field: SerializeField, Tooltip("The scene to load.")]
    public SceneAsset Level1Asset, Level2Asset, Level3Asset, LobbyAsset;

    #endif

    private void OnValidate()
    {
        #if UNITY_EDITOR
            Level1Scene = Level1Asset.name;
            Level2Scene = Level2Asset.name;
            Level3Scene = Level3Asset.name;
            LobbyScene = LobbyAsset.name;
        #endif
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    public void LoadSceneNumber(int sceneIndex)
    {
        switch(sceneIndex)
        {
            case 1:
                LoadSceneAsync(Level1Scene);
                break;
            case 2:
                LoadSceneAsync(Level2Scene);
                break;
            case 3:
                LoadSceneAsync(Level3Scene);
                break;
            case 0:
                LoadSceneAsync(LobbyScene);
                break;

        }
    }

    private Vector3 _playerPosition;
    private Quaternion _playerRotation;
    private void LoadSceneAsync(string sceneName)
    {
        _playerPosition = FindObjectOfType<XROrigin>().transform.position;
        _playerRotation = FindObjectOfType<XROrigin>().transform.rotation;
        AsyncOperation asyncOperator = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        StartCoroutine(WaitForSceneLoaded(asyncOperator));
    }


    private IEnumerator WaitForSceneLoaded(AsyncOperation asyncOperator)
    {
        yield return new WaitUntil(() => asyncOperator.isDone);
        Resources.UnloadUnusedAssets();
        //sceneChanged?.Invoke(CurrentScene);
    }

    private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        StartCoroutine(SceneLoadedIenumerator());
    }

    private IEnumerator SceneLoadedIenumerator()
    {
        yield return new WaitForSeconds(0.05f);
        yield return new WaitUntil(() => FindObjectOfType<TeleportPlayer>() != null);
        FindObjectOfType<XROrigin>().transform.position = _playerPosition; 
        FindObjectOfType<XROrigin>().transform.rotation = _playerRotation;
        //FindObjectOfType<TeleportPlayer>().TeleportTo(_playerPosition);

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
