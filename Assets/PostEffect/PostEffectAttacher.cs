using UnityEngine;

public class PostEffectAttacher : MonoBehaviour
{
    public Shader shader = null;
    private Material ShaderMaterial = null;

    private void Awake()
    {
        ShaderMaterial = new Material(shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, ShaderMaterial);
    }
}
