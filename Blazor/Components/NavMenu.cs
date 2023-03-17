namespace Du.Blazor.Components;

/// <summary>
/// 나브 메뉴
/// </summary>
/// <remarks>
/// ChildContent 없다
/// </remarks>
public class NavMenu : ComponentProp, IComponentNav, IComponentList
{
	[Parameter] public RenderFragment? Brand { get; set; }
	[Parameter] public RenderFragment? Menu { get; set; }

	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public LayoutExpand? Expanded { get; set; }
	[Parameter] public string? MenuIcon { get; set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		ComponentClass = Cssc.Class(Variant?.ToCss("d"), "nvmnu");
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
			<nav class="nvmnu" id="id">
				<div class="x">
					<span class="닫기 아이콘"></span>
				</div>
				<div class="b"> <!--브랜드-->
					브랜드
				</div>
				<div class="m"><!-- 메뉴 -->
					<a class="active" href="링크">링크 제목</a>
				</div>
			</nav>
		 */

		builder.OpenElement(0, "nav");
		builder.AddAttribute(1, "class", ComponentClass);
		builder.AddMultipleAttributes(2, UserAttrs);

		builder.OpenElement(5, "div");
		builder.AddAttribute(6, "class", Cssc.Class("nvtgl", MenuIcon));
		builder.CloseElement();

		if (Brand is not null)
		{
			builder.OpenElement(10, "div");
			builder.AddAttribute(11, "class", "b");
			builder.AddContent(12, Brand);
			builder.CloseElement();
		}

		if (Menu is not null)
		{
			builder.OpenElement(20, "div");
			builder.AddAttribute(21, "class", "m");

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

	#region IComponentNav
	/// <inheritdoc />
	public bool IsOpen => false;
	#endregion

	#region IComponentList
	/// <inheritdoc />
	string? IComponentList.Tag => null;
	/// <inheritdoc />
	string IComponentList.Class => string.Empty;
	#endregion
}


