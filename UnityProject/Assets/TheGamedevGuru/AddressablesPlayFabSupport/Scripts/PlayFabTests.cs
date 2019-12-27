using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace TheGamedevGuru
{
public class PlayFabTests : MonoBehaviour
{
    [SerializeField] private AssetReference sceneReference = null;
    
    public IEnumerator Start()
    {
        yield return LoginToPlayFab();
        yield return InitializeAddressables();
        yield return TestRemoteAddressablesScene();
    }

    private IEnumerator InitializeAddressables()
    {
        Addressables.ResourceManager.ResourceProviders.Add(new AssetBundleProvider());;
        Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageHashProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageAssetBundleProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageJsonAssetProvider());
        
        yield return Addressables.InitializeAsync();
    }
    
    private IEnumerator LoginToPlayFab()
    {
        var loginSuccessful = false;
        var request = new LoginWithCustomIDRequest {CustomId = "MyPlayerId", CreateAccount = true};
        PlayFabClientAPI.LoginWithCustomID(request, result => loginSuccessful = true, error => error.GenerateErrorReport());
        yield return new WaitUntil(() => loginSuccessful);
    }
    
    private IEnumerator TestRemoteAddressablesScene()
    {
        var asyncOperation = sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        yield return asyncOperation;
        var scene = asyncOperation.Result;
        yield return new WaitForSeconds(3);
        yield return Addressables.UnloadSceneAsync(scene);
    }
}
}