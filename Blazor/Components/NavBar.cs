namespace Du.Blazor.Components;

/// <summary>
/// 나브 메뉴
/// </summary>
public class NavBar : NavBarBase,
	IComponentAgent,
	IComponentResponse
{
	#region IComponentAgent
	/// <inheritdoc />
	bool IComponentAgent.SelfClose => false;

	/// <inheritdoc />
	string? IComponentAgent.GetRoleClass(ComponentRole role, string? baseClass) => role switch
	{
		ComponentRole.Block or
			ComponentRole.Text or
			ComponentRole.Image => "cnvbm",
		ComponentRole.Link => "cnvbb",
		_ => null,
	};
	#endregion

	#region IComponentResponse
	/// <inheritdoc />
	Task IComponentResponse.OnResponseAsync(ComponentProp component)
	{
		if (_visible)
		{
			_visible = false;

			_toggle?.Invalidate();
			StateHasChanged();
		}

		return Task.CompletedTask;
	}
	#endregion
}


/// <summary>
/// 나브바 베이스. 인터페이스 땜시 분리
/// </summary>
public abstract class NavBarBase : ComponentProp,
	IComponentToggle,
	IComponentRenderer
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public VarLead? Lead { get; set; }
	[Parameter] public Responsive? Response { get; set; }
	[Parameter] public string Home { get; set; } = "/";

	//
	[Inject] private ILogger<NavBarBase> Logger { get; set; } = default!;

	//
	protected bool _visible;
	protected Toggle? _toggle;

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		LogIf.ArgumentRequired(Logger, Response, nameof(Response));
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
			<div class="cnvb">
				<a class="cnvbd">
					브랜드
				</a>
				<button class="cnvbt">
					아이콘: ic-m / ic-x
				</button>
				<nav class="cnvbn">
					<div class=cnvbm">메뉴</div>
					<a class="cnvbb">메뉴1</a>
					<a class="cnvbb active">메뉴 활성</a>
				</nav>
			</div>
		 */

		var css = Cssc.Class(
			Variant?.ToCss(Lead ?? VarLead.Down),
			"cnv cnvb",
			Response?.ToCssNavBar(),
			Class);

		builder.OpenElement(0, "nav"); // nav, 시작
		builder.AddAttribute(1, "class", css);
		builder.AddMultipleAttributes(2, UserAttrs);

		// 렌더러만 잡히도록 함
		builder.OpenComponent<CascadingValue<NavBarBase>>(3);
		builder.AddAttribute(4, "Value", this);
		builder.AddAttribute(5, "IsFixed", true);
		builder.AddAttribute(6, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(7, ChildContent)));
		builder.CloseComponent(); // CascadingValue<NavBarBase>

		builder.CloseElement();
	}

	#region IComponentToggle
	/// <inheritdoc />
	Task IComponentToggle.OnToggleAsync(ComponentProp component)
	{
		_visible = !_visible;
		StateHasChanged();

		return Task.CompletedTask;
	}

	/// <inheritdoc />
	void IComponentToggle.SetToggle(ComponentProp component)
	{
		_toggle = (Toggle)component;
	}
	#endregion

	#region IComponentRenderer
	/// <inheritdoc />
	bool IComponentRenderer.OnRender(ComponentRole role, ComponentBlock component, RenderTreeBuilder builder) => role switch
	{
		ComponentRole.Lead => RenderBrand((Content)component, builder),
		ComponentRole.Menu => RenderMenu((Content)component, builder),
		ComponentRole.Toggle => RenderToggle((Toggle)component, builder),
		_ => false
	};

	// 브랜드
	private bool RenderBrand(Content brand, RenderTreeBuilder builder)
	{
		/*
			<a class="cnvbd">
				브랜드
			</a>
		 */

		if (Home.WhiteSpace())
			builder.OpenElement(0, "div");
		else
		{
			builder.OpenElement(0, "a");
			builder.AddAttribute(1, "href", Home);
		}

		builder.AddAttribute(2, "class", Cssc.Class("cnvbd", brand.Class));
		builder.AddContent(3, brand.ChildContent);
		builder.CloseElement();

		return true;
	}

	// 메뉴, 메뉴 버튼도 여기서 그림
	private bool RenderMenu(Content menu, RenderTreeBuilder builder)
	{
		/*
			<nav class="cnvbn">
				<div class=cnvbm">메뉴</div>
				<a class="cnvbb">메뉴1</a>
				<a class="cnvbb active">메뉴 활성</a>
			</nav>
		 */

		// 메뉴
		builder.OpenElement(0, "nav"); // nav, 메뉴
		builder.AddAttribute(1, "class", Cssc.Class(
			"cnvbn",
			menu.Trailing.ToCssMarginAuto(),
			_visible.IfTrue("rsp"),
			menu.Class));
		builder.AddMultipleAttributes(2, menu.UserAttrs);

		// 렌더러는 의미 없음
		builder.OpenComponent<CascadingValue<NavBar>>(3); // NavBarBase가 아니고 NavBar가 맞음
		builder.AddAttribute(4, "Value", this);
		builder.AddAttribute(5, "IsFixed", true);
		builder.AddAttribute(6, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(7, menu.ChildContent)));
		builder.CloseComponent(); // CascadingValue<NavBar>

		builder.CloseElement();

		return true;
	}

	// 토글
	private bool RenderToggle(Toggle toggle, RenderTreeBuilder builder)
	{
		/*
			<button class="cnvbt">
				아이콘: ic-m / ic-x
			</button>
		 */

		builder.OpenElement(0, "button");
		builder.AddAttribute(1, "class", Cssc.Class("cnvbt", toggle.Class));
		builder.AddAttribute(2, "type", "button");
		builder.AddAttribute(3, "aria-expanded", _visible.ToHtml());
		builder.AddAttribute(4, "aria-label", "Navbar toggle");
		builder.AddAttribute(5, "onclick", toggle.InternalHandleOnClick);
		builder.AddEventStopPropagationAttribute(6, "onclick", true);

		// 토글 아이콘
		if (toggle.ChildContent is not null)
			builder.AddContent(7, toggle.ChildContent);
		else
		{
			builder.OpenElement(7, "span"); // span, 아이콘
			builder.AddAttribute(8, "class", _visible ? "ic-x" : "ic-m");
			builder.CloseElement();
		}

		builder.CloseElement(); // button, 토글

		return true;
	}
	#endregion
}
