using System.IO;

namespace Osmi.Utils;

[PublicAPI]
public static class ImageUtil {
	public static Texture2D ReadToTexture2D(this Stream self) {
		var tex = new Texture2D(2, 2);
		tex.LoadImage(self.ReadToBytes());
		return tex;
	}

	public static Texture2D CreateReadableCopy(this Texture2D self) {
		var renderTex = RenderTexture.GetTemporary(
			self.width,
			self.height,
			0,
			self.graphicsFormat
		);

		Graphics.Blit(self, renderTex);

		RenderTexture prevTex = RenderTexture.active;
		RenderTexture.active = renderTex;

		var newTex = new Texture2D(self.width, self.height);
		newTex.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
		newTex.Apply();

		RenderTexture.active = prevTex;
		RenderTexture.ReleaseTemporary(renderTex);

		return newTex;
	}

	public static Sprite MakeSprite(this Texture2D self, float pixelPerUnit = 64f) =>
		Sprite.Create(
			self,
			new Rect(0f, 0f, self.width, self.height),
			new Vector2(0.5f, 0.5f),
			pixelPerUnit
		);

	public static Sprite ReadToSprite(this Stream self, float pixelPerUnit = 64f) =>
		self.ReadToTexture2D().MakeSprite(pixelPerUnit);
}
