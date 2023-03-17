namespace Du.Blazor.Components;

/// <summary>
/// 나브 메뉴
/// </summary>
/// <remarks>
/// ChildContent 없다
/// </remarks>
public class NavMenu : ComponentProp, IComponentAgent
{
	[Parameter] public RenderFragment? Brand { get; set; }
	[Parameter] public RenderFragment? Menu { get; set; }

	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public LayoutExpand? Expand { get; set; }
	[Parameter] public string Home { get; set; } = "/";
	[Parameter] public string? IconClass { get; set; }
	[Parameter] public string? BrandClass { get; set; }
	[Parameter] public string? MenuClass { get; set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		ComponentClass = Cssc.Class(Variant?.ToCss(VrtLead.Down), "cnvmn");
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
			<div class="cnmnu">
				<span>아이콘</span>
				<a>브랜드</a>
				<nav>
					메뉴
					<a>메뉴1</a>
					<a class="active">메뉴 활성</a>
				</nav>
			</div>
		 */

		builder.OpenElement(0, "div"); // div, 시작
		builder.AddAttribute(1, "class", ComponentClass);
		builder.AddMultipleAttributes(2, UserAttrs);

		builder.OpenElement(5, "div"); // div, 아이콘
		builder.AddAttribute(6, "class", Cssc.Class("tgm", IconClass));
		builder.CloseElement();

		if (Brand is not null)
		{
			if (Home.WhiteSpace())
			{
				builder.OpenElement(10, "div"); // div, 브랜드
				builder.AddAttribute(11, "class", Cssc.Class("brd", BrandClass));
				builder.AddContent(12, Brand);
				builder.CloseElement();
			}
			else
			{
				builder.OpenElement(10, "a"); // a, 브랜드
				builder.AddAttribute(11, "class", Cssc.Class("brd", BrandClass));
				builder.AddAttribute(12, "href", Home);
				builder.AddContent(13, Brand);
				builder.CloseElement();
			}
		}

		if (Menu is not null)
		{
			builder.OpenElement(20, "nav"); // nav, 메뉴
			builder.AddAttribute(21, "class", MenuClass);

			builder.OpenComponent<CascadingValue<NavMenu>>(22);
			builder.AddAttribute(23, "Value", this);
			builder.AddAttribute(24, "IsFixed", true);
			builder.AddAttribute(25, "ChildContent", (RenderFragment)((b) =>
				b.AddContent(26, Menu)));
			builder.CloseComponent(); // CascadingValue<NavMenu>, 콘텐트

			builder.CloseElement();
		}

		builder.CloseElement();
	}

	#region IComponentAgent
	/// <inheritdoc />
	public bool AgentRefineBaseClass => true;
	#endregion
}


