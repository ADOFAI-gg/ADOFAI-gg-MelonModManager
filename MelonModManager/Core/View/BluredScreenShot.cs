using UnityEngine;
using UnityEngine.UI;

namespace MelonModManager.Core.View
{
      public class BluredScreenShot : MonoBehaviour
    {


        private const int bufferWidth = 352;
        private const int bufferHeight = 198;
        public int iterations = 2;
        public float blurSpread = 0.6f;
        public RenderTexture blurredTexture;
        public Material mat;
        public static bool isTakeScreenShot = false;

        /// <summary>
        /// 
        /// </summary>
        public void Awake()
        {
            mat = new Material(RDConstants.data.blurEffectConeTap);
        }
        
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (ModWindow.Instance != null)
            {
                if (ModWindow.Instance.CanvasObject != null)
                {
                    if (isTakeScreenShot)
                    {
                        if (this.blurredTexture == null)
                        {
                            this.blurredTexture = new RenderTexture(352, 198, source.depth);
                        }

                        this.mat.mainTexture = source;
                        int width = this.blurredTexture.width / 4;
                        int height = this.blurredTexture.height / 4;
                        RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
                        this.DownSample4x(source, temporary);
                        RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0);
                        for (int i = 0; i < this.iterations; i++)
                        {
                            this.FourTapCone(temporary, temporary2, i);
                            temporary.DiscardContents();
                            Graphics.Blit(temporary2, temporary);
                            temporary2.DiscardContents();
                        }

                        RenderTexture.ReleaseTemporary(temporary2);
                        Graphics.Blit(temporary, this.blurredTexture);
                        RenderTexture.ReleaseTemporary(temporary);
                        var a = ModWindow.Instance.CanvasObject.transform.Find("ModSettingView").transform
                            .Find("Preview")
                            .GetComponent<RawImage>();
                        a.texture = blurredTexture;
                        isTakeScreenShot = false;
                    }
                }
            }

            Graphics.Blit(source, destination);
        }
        
        public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
        {
            float num = 0.5f + (float)iteration * this.blurSpread;
            Graphics.BlitMultiTap(source, dest, this.mat, new Vector2[]
            {
                new Vector2(-num, -num),
                new Vector2(-num, num),
                new Vector2(num, num),
                new Vector2(num, -num)
            });
        }

        // Token: 0x060007F1 RID: 2033 RVA: 0x0003B260 File Offset: 0x00039460
        private void DownSample4x(RenderTexture source, RenderTexture dest)
        {
            float num = 1f;
            Graphics.BlitMultiTap(source, dest, this.mat, new Vector2[]
            {
                new Vector2(-num, -num),
                new Vector2(-num, num),
                new Vector2(num, num),
                new Vector2(num, -num)
            });
        }
    }
}