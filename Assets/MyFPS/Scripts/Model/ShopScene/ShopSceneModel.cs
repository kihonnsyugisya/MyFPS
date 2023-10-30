using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopSceneModel : MonoBehaviour
{
    public Transform photoSpot;
    public Camera faceCamera;

    public Transform avatarSpot;

    public GameObject contentTile;
    public GameObject meshTileContentParent;

    private　List<Sprite> sprites = new List<Sprite>();
    private Transform currentSelectedAvatar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TakePictures(List<GameObject> avatars)
    {
        //We need to turn all the meshes off so we can get some pictures of the faces

        //Loop through all skinned Mesh Renderers
        foreach (var skin in avatars)
        {
            var avatar = Instantiate(skin, photoSpot.position, Quaternion.Euler(0, 180, 0), photoSpot);
            avatar.SetActive(false);

            SkinnedMeshRenderer skinnedMesh = skin.GetComponentInChildren<SkinnedMeshRenderer>();
            //An extra wait to ensure we don't accidently take a picture of two characters
            yield return new WaitForEndOfFrame();
            //Turn on the face
            avatar.SetActive(true);

            //Wait until the end of the frame to do the rendering
            yield return new WaitForEndOfFrame();

            //Create a render texture from the face camera
            RenderTexture.active = faceCamera.targetTexture;
            //Store the texture so that it does not change mid way through this function
            var faceRenderTexture = faceCamera.targetTexture;

            //All done rendering so we can turn off the skinned Mesh now
            avatar.SetActive(false);


            //Create a new texture2D
            var newTexture = new Texture2D(faceRenderTexture.width, faceRenderTexture.height);
            newTexture.ReadPixels(new Rect(0, 0, faceRenderTexture.width, faceRenderTexture.height), 0, 0);
            newTexture.Apply();

            //Create a sprite out of the texture2D
            Rect rec = new Rect(0, 0, newTexture.width, newTexture.height);
            var newSprite = Sprite.Create(newTexture, rec, new Vector2(0, 0), 1);
            newSprite.name = skinnedMesh.name;
            sprites.Add(newSprite);

            ////Spawn some tiles for the faces and assign them with textures and the name of the character
            var newTile = Instantiate(contentTile, meshTileContentParent.transform);
            newTile.GetComponentsInChildren<Image>().First(x => x.name == "Face Sprite").sprite = newSprite;
            newTile.GetComponentInChildren<Text>().text = skinnedMesh.transform.name;
            newTile.GetComponent<Button>().onClick.AddListener(() => SelectTile(avatar.transform));
        }
    }

    private void SelectTile(Transform avatar)
    {
        if (currentSelectedAvatar)
        {
            currentSelectedAvatar.gameObject.SetActive(false);
        }
        avatar.position = avatarSpot.position;
        currentSelectedAvatar = avatar;
        currentSelectedAvatar.gameObject.SetActive(true);
    }

    public async Task MoveToStartSceneAsync()
    {
        if (currentSelectedAvatar)
        {
            if (FireStoreModel.userDataCash.Avatar != currentSelectedAvatar.name)
            {
                await FireStoreModel.UpdateAvatar(currentSelectedAvatar.name.Replace("(Clone)", ""));
            }
        }
        SceneManager.LoadScene("StartScene");
    }

}



//facecameraのレンダーテクスチャーのアタッチがまちがってる？
//    レンダーテクスチャーについてググる
//    一応それっぽいやつアタッチはしてみたが他にも色々あった
//    三体とも重なって、setactive false 聞いていないっぽいからどこかうまくいってない