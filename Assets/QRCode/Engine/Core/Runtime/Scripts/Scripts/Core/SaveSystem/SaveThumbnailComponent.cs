namespace QRCode.Engine.Core.SaveSystem
{
    using System.Threading.Tasks;
    using Toolbox;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.UI;

    public class SaveThumbnailComponent : SerializedMonoBehaviour
    {
        [TitleGroup(Constants.InspectorGroups.References)] 
        [SerializeField] private Image m_image = null;

        [TitleGroup(Constants.InspectorGroups.Settings)] 
        [SerializeField] private string m_thumbnailName;

        public async Task<Texture2D> LoadThumbnailAsync()
        {
            var imageLoader = new ImageLoader();
            var fullPath = SaveServiceSettings.Instance.FullPath + "/" + m_thumbnailName + ".png";
            var loadedTexture = await imageLoader.LoadImage(fullPath);
            var sprite = Sprite.Create(loadedTexture, new Rect(0.0f, 0.0f, loadedTexture.width, loadedTexture.height), new Vector2(0.5f, 0.5f));
            m_image.sprite = sprite;
            return loadedTexture;
        }
        
        [Button]
        public async void LoadThumbnail()
        {
            await LoadThumbnailAsync();
        }

        public async Task SaveThumbnailAsync(Texture2D texture)
        {
            var imageSaver = new ImageSaver();
            await imageSaver.SaveImage(texture, m_thumbnailName, ".png");
        }

        [Button]
        public async void SaveThumbnail(Texture2D texture)
        {
            await SaveThumbnailAsync(texture);
        }
    }
}
