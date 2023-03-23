namespace Du.Blazor.Components;

/// <summary>
/// 나브 메뉴
/// </summary>
public class NavBar : ComponentProp, IComponentAgent, IComponentResponse, IComponentRenderer
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public VarLead? Lead { get; set; }
	[Parameter] public Responsive? Response { get; set; }
	[Parameter] public string Home { get; set; } = "/";

	//
	[Inject] private ILogger<NavBar> Logger { get; set; } = default!;

	//
	private bool _visible;

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
			"cnvb",
			Response?.ToCssNavBar(),
			Class);

		builder.OpenElement(0, "nav"); // nav, 시작
		builder.AddAttribute(1, "class", css);
		builder.AddMultipleAttributes(2, UserAttrs);

		// 렌더러만 잡히도록 함
		builder.OpenComponent<CascadingValue<IComponentRenderer>>(3);
		builder.AddAttribute(4, "Value", this);
		builder.AddAttribute(5, "IsFixed", true);
		builder.AddAttribute(6, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(7, ChildContent)));
		builder.CloseComponent(); // CascadingValue<IComponentRenderer>

		builder.CloseElement();
	}

	//
	private Task HandleButtonOnClick(MouseEventArgs e)
	{
		_visible = !_visible;
		return Task.CompletedTask;
	}

	#region IComponentAgent
	/// <inheritdoc />
	bool IComponentAgent.SelfClose => false;

	/// <inheritdoc />
	string? IComponentAgent.GetRoleClass(ComponentRole role) => role switch
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
			StateHasChanged();
		}

		return Task.CompletedTask;
	}
	#endregion

	#region IComponentRenderer
	/// <inheritdoc />
	bool IComponentRenderer.OnRender(ComponentRole role, ComponentBlock component, RenderTreeBuilder builder) => role switch
	{
		ComponentRole.Lead => RenderBrand((Content)component, builder),
		ComponentRole.Menu => RenderMenu((Content)component, builder),
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
			<button class="cnvbt">
				아이콘: ic-m / ic-x
			</button>
			<nav class="cnvbn">
				<div class=cnvbm">메뉴</div>
				<a class="cnvbb">메뉴1</a>
				<a class="cnvbb active">메뉴 활성</a>
			</nav>
		 */

		// 토글 먼저
		builder.OpenElement(0, "button");
		builder.AddAttribute(1, "class", "cnvbt");
		builder.AddAttribute(2, "type", "button");
		builder.AddAttribute(3, "aria-expanded", _visible.ToHtml());
		builder.AddAttribute(4, "aria-label", "Navbar toggle");
		builder.AddAttribute(5, "onclick", HandleButtonOnClick);
		builder.AddEventStopPropagationAttribute(6, "onclick", true);
		// 토글 프래그먼트를 추가한다면... 넣고 지금은 그냥 기본 사양으로

		// 토글 아이콘
		builder.OpenElement(7, "span"); // span, 아이콘
		builder.AddAttribute(8, "class", _visible ? "ic-x" : "ic-m");
		builder.CloseElement();

		builder.CloseElement(); // button, 토글

		// 메뉴
		builder.OpenElement(10, "nav"); // nav, 메뉴
		builder.AddAttribute(11, "class", Cssc.Class("cnvbn", _visible.IfTrue("rsp"), menu.Class));

		// 렌더러는 의미 없음
		builder.OpenComponent<CascadingValue<NavBar>>(13);
		builder.AddAttribute(14, "Value", this);
		builder.AddAttribute(15, "IsFixed", true);
		builder.AddAttribute(16, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(17, menu.ChildContent)));
		builder.CloseComponent(); // CascadingValue<NavBar>

		builder.CloseElement();

		return true;
	}
	#endregion
}
