using Microsoft.AspNetCore.Components.Forms;

namespace Du.Blazor.Components;

/// <summary>
/// 버튼
/// </summary>
public class Btn : Nulo
{
	/// <summary>콘텐트 핸들러</summary>
	[CascadingParameter] public ITagContentHandler? ContentHandler { get; set; }
	/// <summary>에디트 컨텍스트</summary>
	[CascadingParameter] public EditContext? EditContext { get; set; }

	/// <summary>URL 링크 지정.</summary>
	[Parameter] public string? Link { get; set; }
	/// <summary>타겟 지정.</summary>
	[Parameter] public string? Target { get; set; }

	/// <summary>에디트 폼 ValidClick.</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnValidClick { get; set; }
	/// <summary>에디트 폼 InvalidClick.</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnInvalidClick { get; set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		InternalType = Link is not null ? NuloType.Link
			: EditContext is not null ? NuloType.Submit : NuloType.Button;

		ComponentClass = GetNuloClassName();
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (ListAgent?.Tag is not null)
			builder.OpenElement(0, ListAgent.Tag); // wrap

		if (InternalType == NuloType.Link)
		{
			builder.OpenElement(10, "a");
			builder.AddAttribute(11, "class", ActualClass);
			if (Link.TestHave(true))
				builder.AddAttribute(12, "href", Link);
			builder.AddAttribute(13, "target", Target);
		}
		else
		{
			builder.OpenElement(10, "button");
			builder.AddAttribute(11, "type", InternalType == NuloType.Submit ? "submit" : "button");
			builder.AddAttribute(12, "class", ActualClass);
			builder.AddAttribute(13, "role", "button");
			builder.AddAttribute(14, "formtarget", Target);
		}

		builder.AddAttribute(15, "id", Id);
		builder.AddAttribute(16, "onclick", HandleOnClickAsync);
		// 17, 버튼 속성
		// 18, 버튼 속성
		builder.AddMultipleAttributes(19, UserAttrs);
		if (ChildContent is null)
			builder.AddContent(20, Text);
		else
			builder.AddContent(21, ChildContent);
		builder.CloseElement(); // a 또는 button

		if (ListAgent?.Tag is not null)
			builder.CloseElement(); // wrap
	}

	/// <inheritdoc />
	protected override async Task HandleOnClickAsync(MouseEventArgs e)
	{
		if (!_handle_click)
		{
			_handle_click = true;

			if (ResponseHandler is not null)
			{
				// 반응이 있으면 거기서 일해
				await ResponseHandler.OnResponseAsync(this);
			}
			else if (OnClick.HasDelegate)
			{
				// 클릭이 있으면 거기서 일해
				await InvokeOnClickAsync(e);
			}
			else if (EditContext != null && InternalType == NuloType.Submit)
			{
				// 그것도 아니고 폼이면 폼잡아
				switch (EditContext.Validate())
				{
					case true when OnValidClick.HasDelegate:
						await InvokeOnValidClickAsync(e);
						break;

					case false when OnInvalidClick.HasDelegate:
						await InvokeOnInvalidClickAsync(e);
						break;
				}

			}

			_handle_click = false;
		}
	}

	//
	protected virtual Task InvokeOnValidClickAsync(MouseEventArgs e) => OnValidClick.InvokeAsync(e);
	protected virtual Task InvokeOnInvalidClickAsync(MouseEventArgs e) => OnInvalidClick.InvokeAsync(e);
}


/// <summary>
/// 눌러 -> 버튼 밑단
/// </summary>
public abstract class Nulo : ComponentContent
{
	/// <summary>리스트 에이전시</summary>
	[CascadingParameter] public IComponentList? ListAgent { get; set; }
	/// <summary>나브 처리기</summary>
	[CascadingParameter] public IComponentNav? NavAgent { get; set; }
	/// <summary>반응 처리기</summary>
	[CascadingParameter] public IComponentResponse? ResponseHandler { get; set; }

	/// <summary>텍스트</summary>
	[Parameter] public string? Text { get; set; }
	/// <summary>바리언트.</summary>
	[Parameter] public Variant? Variant { get; set; }
	/// <summary>사용자 설정 정의(User setting pretend)</summary>
	[Parameter] public bool Pseudo { get; set; }

	/// <summary>마우스 눌린 이벤트 지정.</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	internal NuloType InternalType { get; set; }

	//
	protected bool _handle_click;

	//
	protected string? GetNuloClassName(string? baseClass = "nulo", bool defVariant = true, string? additional = null)
	{
		return Cssc.Class(
			Pseudo.IfTrue("usp"), 
			defVariant ? (Variant ?? Settings.Variant).ToCss() : Variant?.ToCss(), 
			ListAgent?.Class ?? baseClass,
			additional);
	}

	// 마우스 핸들러
	protected virtual async Task HandleOnClickAsync(MouseEventArgs e)
	{
		if (!_handle_click)
		{
			_handle_click = true;

			if (OnClick.HasDelegate)
				await InvokeOnClickAsync(e);

			_handle_click = false;
		}
	}

	//
	protected virtual Task InvokeOnClickAsync(MouseEventArgs e) => OnClick.InvokeAsync(e);

	//
	internal enum NuloType
	{
		Link,
		Button,
		Submit,
	}
}
