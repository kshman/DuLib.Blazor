namespace Du.Blazor.Components;

/// <summary>
/// 이미지 태그
/// </summary>
public class TagImage : ComponentProp
{
	/// <summary>이미지 URL</summary>
	[Parameter] public string? Image { get; set; }
	/// <summary>별명(이게 없으면 브라우저에서 욕함)</summary>
	[Parameter] public string Text { get; set; } = "Picture";
	/// <summary>가로 너비</summary>
	[Parameter] public int? Width { get; set; }
	/// <summary>세로 높이</summary>
	[Parameter] public int? Height { get; set; }
	/// <summary>클릭</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "img");
		builder.AddAttribute(1, "class", ActualClass);
		builder.AddAttribute(2, "src", Image);
		builder.AddAttribute(3, "alt", Text);
		builder.AddAttribute(4, "width", Width);
		builder.AddAttribute(5, "height", Height);

		if (OnClick.HasDelegate)
		{
			builder.AddAttribute(6, "onclick", InvokeOnClick);
			builder.AddEventStopPropagationAttribute(7, "onclick", true);
		}

		builder.CloseElement();
	}

	//
	protected virtual Task InvokeOnClick(MouseEventArgs e) => OnClick.InvokeAsync(e);
}
