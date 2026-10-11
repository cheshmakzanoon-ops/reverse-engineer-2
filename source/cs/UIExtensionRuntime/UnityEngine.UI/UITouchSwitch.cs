using UnityEngine.EventSystems;

namespace UnityEngine.UI;

public class UITouchSwitch : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public RawImage Display;

	public Texture TextureNormal;

	public Texture TexturePressed;

	public Color ColorNormal = Color.white;

	public Color ColorPressed = Color.white;

	private LayoutElement layoutElement;

	private bool init;

	private bool isDown;

	private void Start()
	{
		if (Display == null)
		{
			Display = GetComponent<RawImage>();
		}
		if (layoutElement == null)
		{
			layoutElement = GetComponent<LayoutElement>();
		}
		if (Display != null)
		{
			if (Display.texture == null)
			{
				SetTexture(TextureNormal);
			}
			init = true;
		}
		isDown = false;
	}

	private void SetTexture(Texture texture)
	{
		if (Display != null)
		{
			Display.texture = texture;
			Display.color = ((texture == TextureNormal) ? ColorNormal : ColorPressed);
			Display.SetNativeSize();
		}
		if (layoutElement != null)
		{
			layoutElement.preferredWidth = Display.texture.width;
			layoutElement.preferredHeight = Display.texture.height;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (init && !isDown)
		{
			SetTexture(TexturePressed);
			isDown = true;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (init && isDown)
		{
			SetTexture(TextureNormal);
			isDown = false;
		}
	}
}
