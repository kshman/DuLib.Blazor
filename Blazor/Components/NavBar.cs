namespace Du.Blazor.Components;

/// <summary>
/// 나브 메뉴
/// </summary>
/// <remarks>
/// ChildContent 없다
/// </remarks>
public class NavBar : ComponentProp, IComponentAgent, IComponentResponse
{
	[Parameter] public RenderFragment? Icon { get; set; }
	[Parameter] public RenderFragment? Brand { get; set; }
	[Parameter] public RenderFragment? Menu { get; set; }

	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public LayoutExpand? Expand { get; set; }
	[Parameter] public string Home { get; set; } = "/";
	[Parameter] public string? BrandClass { get; set; }
	[Parameter] public string? MenuClass { get; set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		ComponentClass = Cssc.Class(Variant?.ToCss(VrtLead.Down), "cnvba");
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
			<div class="cnmnu">
				<button>
					아이콘
				</button>
				<a>
					브랜드
				</a>
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

		builder.OpenElement(10, "button"); // button, 아이콘
		builder.AddAttribute(11, "type", "button");

		if (Icon is not null)
			builder.AddContent(18, Icon); // 아이콘 프래그먼트
		else
		{
			builder.OpenElement(18, "span"); // span, 아이콘
			builder.AddAttribute(19, "class", "mico");
			builder.CloseElement();
		}

		builder.CloseElement();

		if (Brand is not null)
		{
			if (Home.WhiteSpace())
			{
				builder.OpenElement(20, "div"); // div, 브랜드
				builder.AddAttribute(21, "class", BrandClass);
				builder.AddContent(22, Brand); // 브랜드 프래그먼트
				builder.CloseElement();
			}
			else
			{
				builder.OpenElement(20, "a"); // a, 브랜드
				builder.AddAttribute(21, "class", BrandClass);
				builder.AddAttribute(22, "href", Home);
				builder.AddContent(23, Brand); // 브랜드 프래그먼트
				builder.CloseElement();
			}
		}

		if (Menu is not null)
		{
			builder.OpenElement(30, "nav"); // nav, 메뉴
			builder.AddAttribute(31, "class", MenuClass);

			builder.OpenComponent<CascadingValue<NavBar>>(32);
			builder.AddAttribute(33, "Value", this);
			builder.AddAttribute(34, "IsFixed", true);
			builder.AddAttribute(35, "ChildContent", (RenderFragment)((b) =>
				b.AddContent(36, Menu))); // 메뉴 프래그먼트
			builder.CloseComponent(); // CascadingValue<NavMenu>, 콘텐트

			builder.CloseElement();
		}

		builder.CloseElement();
	}

	#region IComponentAgent
	/// <inheritdoc />
	bool IComponentAgent.RefineBaseClass => true;
	/// <inheritdoc />
	bool IComponentAgent.SelfClose => false;
	#endregion

	#region IComponentResponse
	/// <inheritdoc />
	Task IComponentResponse.OnResponseAsync(ComponentProp component)
	{
		// 여기서 닫을건 닫아야함
		return Task.CompletedTask;
	} 
	#endregion
}


