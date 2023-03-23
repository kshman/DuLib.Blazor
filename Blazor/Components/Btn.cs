using Microsoft.AspNetCore.Components.Forms;

namespace Du.Blazor.Components;

/// <summary>
/// 버튼
/// </summary>
public class Btn : Nulo
{
	/// <summary>에디트 컨텍스트</summary>
	[CascadingParameter] public EditContext? EditContext { get; set; }

	/// <summary>자식 콘텐트</summary>
	[Parameter] public RenderFragment? ChildContent { get; set; }
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
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var css = GetNuloCssClass();

		if (InternalType is NuloType.Link or NuloType.Action)
		{
			builder.OpenElement(0, "a");
			builder.AddAttribute(1, "class", css);
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
			builder.AddAttribute(1, "class", css);
			builder.AddAttribute(2, "type", InternalType == NuloType.Submit ? "submit" : "button");
			builder.AddAttribute(3, "formtarget", Target);
		}

		builder.AddAttribute(5, "id", Id);

		if (Tooltip.TestHave(true))
			builder.AddAttribute(6, "tooltip", Tooltip);

		builder.AddAttribute(7, "onclick", HandleOnClickAsync);
		if (StopPropagation)
			builder.AddEventStopPropagationAttribute(8, "onclick", true);
		if (InternalType is NuloType.Action)
			builder.AddEventPreventDefaultAttribute(9, "onclick", true);
		
		builder.AddMultipleAttributes(10, UserAttrs);
		
		if (ChildContent is null)
			builder.AddContent(11, Text ?? "[BUTTON]");
		else
			builder.AddContent(12, ChildContent);

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
				await OnClick.InvokeAsync(e);
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
/// 눌러
/// </summary>
public abstract class Nulo : ComponentProp
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
	/// <summary>툴팁</summary>
	[Parameter] public string? Tooltip { get; set; }

	/// <summary>마우스 눌린 이벤트 지정.</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	/// <summary>이벤트 재전송 금지. 기본값은 참</summary>
	[Parameter] public bool StopPropagation { get; set; } = true;

	//
	internal NuloType InternalType { get; set; }

	//
	protected bool _handle_click;

	//
	protected Nulo(ComponentRole role = ComponentRole.Link)
	{
		TagRole = role;
	}

	//
	protected string? GetNuloCssClass(string? baseClass = "cbtn",
		bool defVariant = true, string? param1 = null, string? param2 = null)
	{
#if false
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
				AgentHandler.GetRoleClass(TagRole) ?? baseClass,
				additional);
#else
		return Cssc.Class(
			Pseudo.IfTrue("usp"),
			defVariant
				? (Variant ?? Settings.Variant).ToCss()
				: Variant?.ToCss(),
			AgentHandler?.GetRoleClass(TagRole) ?? baseClass,
			param1, param2, Class);
#endif
	}

	// 마우스 핸들러
	protected virtual async Task HandleOnClickAsync(MouseEventArgs e)
	{
		if (!_handle_click)
		{
			_handle_click = true;

			if (OnClick.HasDelegate)
				await OnClick.InvokeAsync(e);

			_handle_click = false;
		}
	}
}
