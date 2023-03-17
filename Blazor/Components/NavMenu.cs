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
	[Parameter] public LayoutExpand? Expanded { get; set; }
	[Parameter] public string Home { get; set; } = "/";
	[Parameter] public string? MenuIcon { get; set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		ComponentClass = Cssc.Class(Variant?.ToCss(VrtLead.Down), "nvmnu");
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
			<nav class="nvmnu" id="id">
				<div class="x">
					<span class="닫기 아이콘"></span>
				</div>
				<div class="nv-brd"> <!--브랜드-->
					브랜드
				</div>
				<div class="nv-mnu"><!-- 메뉴 -->
					<a class="active" href="링크">링크 제목</a>
				</div>
			</nav>
		 */

		builder.OpenElement(0, "nav");
		builder.AddAttribute(1, "class", ComponentClass);
		builder.AddMultipleAttributes(2, UserAttrs);

		builder.OpenElement(5, "div");
		builder.AddAttribute(6, "class", Cssc.Class("nv-tgl", MenuIcon));
		builder.CloseElement();

		if (Brand is not null)
		{
			builder.OpenElement(10, "a");
			builder.AddAttribute(11, "class", "nv-brd");
			builder.AddAttribute(12, "href", Home);
			builder.AddContent(13, Brand);
			builder.CloseElement();
		}

		if (Menu is not null)
		{
			builder.OpenElement(20, "div");
			builder.AddAttribute(21, "class", "nv-mnu");

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


