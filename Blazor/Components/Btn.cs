using Microsoft.AspNetCore.Components.Forms;

namespace Du.Blazor.Components;

/// <summary>
/// 버튼
/// </summary>
public class Btn : ComponentContent
{
	/// <summary>리스트 에이전시</summary>
	[CascadingParameter] public IListAgent? ListAgent { get; set; }
	/// <summary>콘텐트 핸들러</summary>
	[CascadingParameter] public IContentHandler? ContentHandler { get; set; }
	/// <summary>에디트 컨텍스트</summary>
	[CascadingParameter] public EditContext? EditContext { get; set; }

	/// <summary>텍스트</summary>
	[Parameter] public string? Text { get; set; }
	/// <summary>URL 링크 지정.</summary>
	[Parameter] public string? Link { get; set; }
	/// <summary>타겟 지정.</summary>
	[Parameter] public string? Target { get; set; }
	/// <summary>바리언트.</summary>
	[Parameter] public TagVariant? Variant { get; set; }

	/// <summary>마우스 눌린 이벤트 지정.</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	/// <summary>에디트 폼 ValidClick.</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnValidClick { get; set; }
	/// <summary>에디트 폼 InvalidClick.</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnInvalidClick { get; set; }

	//
	private bool _handle_click;
	private BtnType _btn_type;

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		_btn_type = Link is not null ? BtnType.Link
			: EditContext is not null ? BtnType.Submit : BtnType.Button;

		ComponentClass = Cssc.Class("nulo", Variant?.ToString("F").ToLower());
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (ListAgent?.Tag is not null)
			builder.OpenElement(0, ListAgent.Tag); // wrap

		if (_btn_type == BtnType.Link)
		{
			builder.OpenElement(10, "a");
			builder.AddAttribute(11, "class", ActualClass);
			builder.AddAttribute(12, "href", Link);
			builder.AddAttribute(13, "target", Target);
		}
		else
		{
			builder.OpenElement(10, "button");
			builder.AddAttribute(11, "type", _btn_type == BtnType.Submit ? "submit" : "button");
			builder.AddAttribute(12, "class", ActualClass);
			builder.AddAttribute(13, "role", "button");
			builder.AddAttribute(14, "formtarget", Target);
		}

		builder.AddAttribute(15, "id", Id);
		builder.AddAttribute(16, "onclick", HandleOnClickAsync);
		// 17, 버튼 속성
		// 18, 버튼 속성
		builder.AddMultipleAttributes(19, UserAttrs);
		builder.AddContent(20, Text);
		builder.AddContent(21, ChildContent);
		builder.CloseElement(); // a 또는 button

		if (ListAgent?.Tag is not null)
			builder.CloseElement(); // wrap
	}

	// 마우스 핸들러
	protected async Task HandleOnClickAsync(MouseEventArgs e)
	{
		if (!_handle_click)
		{
			_handle_click = true;

			if (OnClick.HasDelegate)
				await InvokeOnClickAsync(e);
			else if (EditContext != null && _btn_type == BtnType.Submit)
				switch (EditContext.Validate())
				{
					case true when OnValidClick.HasDelegate:
						await InvokeOnValidClickAsync(e);
						break;

					case false when OnInvalidClick.HasDelegate:
						await InvokeOnInvalidClickAsync(e);
						break;
				}

			_handle_click = false;
		}
	}

	//
	protected virtual Task InvokeOnClickAsync(MouseEventArgs e) => OnClick.InvokeAsync(e);
	protected virtual Task InvokeOnValidClickAsync(MouseEventArgs e) => OnValidClick.InvokeAsync(e);
	protected virtual Task InvokeOnInvalidClickAsync(MouseEventArgs e) => OnInvalidClick.InvokeAsync(e);

	//
	private enum BtnType
	{
		Link,
		Button,
		Submit,
	}
}
