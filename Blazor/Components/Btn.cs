using System.Diagnostics.SymbolStore;
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
		InternalType = AgentHandler is not null || Link is not null
			? Link.WhiteSpace() ? NuloType.Action : NuloType.Link
			: EditContext is not null ? NuloType.Submit : NuloType.Button;

		ComponentClass = GetNuloClassName();
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (InternalType is NuloType.Link or NuloType.Action)
		{
			builder.OpenElement(0, "a");
			builder.AddAttribute(1, "class", ActualClass);
			if (InternalType is NuloType.Action)
				builder.AddAttribute(2, "role", "button");
			else
			{
				builder.AddAttribute(3, "href", Link);
				builder.AddAttribute(4, "target", Target);
			}
		}
		else
		{
			builder.OpenElement(0, "button");
			builder.AddAttribute(1, "class", ActualClass);
			builder.AddAttribute(2, "type", InternalType == NuloType.Submit ? "submit" : "button");
			builder.AddAttribute(3, "formtarget", Target);
		}

		builder.AddAttribute(5, "id", Id);
		builder.AddAttribute(6, "onclick", HandleOnClickAsync);
		if (StopPropagation)
			builder.AddEventStopPropagationAttribute(7, "onclick", true);
		if (InternalType is NuloType.Action)
			builder.AddEventPreventDefaultAttribute(8, "onclick", true);
		builder.AddMultipleAttributes(9, UserAttrs);
		if (ChildContent is null)
			builder.AddContent(10, Text);
		else
			builder.AddContent(11, ChildContent);
		builder.CloseElement(); // a 또는 button
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
	private Task InvokeOnValidClickAsync(MouseEventArgs e) => OnValidClick.InvokeAsync(e);
	private Task InvokeOnInvalidClickAsync(MouseEventArgs e) => OnInvalidClick.InvokeAsync(e);
}


/// <summary>
/// 눌러 -> 버튼 밑단
/// </summary>
public abstract class Nulo : ComponentContent
{
	/// <summary>나브 처리기</summary>
	[CascadingParameter] public IComponentAgent? AgentHandler { get; set; }
	/// <summary>반응 처리기</summary>
	[CascadingParameter] public IComponentResponse? ResponseHandler { get; set; }

	/// <summary>텍스트</summary>
	[Parameter] public string? Text { get; set; }
	/// <summary>바리언트.</summary>
	[Parameter] public Variant? Variant { get; set; }
	/// <summary>사용자 설정 정의(User setting pretend)</summary>
	[Parameter] public bool Pseudo { get; set; }
	/// <summary>이벤트 재전송 금지. 기본값은 참</summary>
	[Parameter] public bool StopPropagation { get; set; } = true;

	/// <summary>마우스 눌린 이벤트 지정.</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	internal NuloType InternalType { get; set; }

	//
	protected bool _handle_click;

	//
	protected string? GetNuloClassName(string? baseClass = "cbtn", string? additional = null, bool defVariant = true)
	{
		if (AgentHandler is null)
			return Cssc.Class(
				Pseudo.IfTrue("usp"),
				defVariant
					? (Variant ?? Settings.Variant).ToCss()
					: Variant?.ToCss(),
				baseClass,
				additional);
		else
			return Cssc.Class(
				Pseudo.IfTrue("usp"),
				Variant?.ToCss(),
				AgentHandler.RefineBaseClass.IfFalse(baseClass),
				additional);
	}

	//
	protected string? GetNuloClassName(string? baseClass, bool defVariant) =>
		GetNuloClassName(baseClass, null, defVariant);

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
}
