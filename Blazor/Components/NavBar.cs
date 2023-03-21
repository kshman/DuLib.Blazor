namespace Du.Blazor.Components;

/// <summary>
/// 나브 메뉴
/// </summary>
/// <remarks>
/// ChildContent 없다
/// </remarks>
public class NavBar : ComponentProp, IComponentAgent, IComponentResponse
{
    [Parameter] public RenderFragment? Toggle { get; set; }
    [Parameter] public RenderFragment? Brand { get; set; }
    [Parameter] public RenderFragment? Menu { get; set; }

    [Parameter] public Variant? Variant { get; set; }
    [Parameter] public LayoutExpand? Expand { get; set; }
    [Parameter] public string Home { get; set; } = "/";
    [Parameter] public string? BrandClass { get; set; }
    [Parameter] public string? MenuClass { get; set; }

    //
    [Inject] private ILogger<NavBar> Logger { get; set; } = default!;

    //
    private bool _visible = false;

    ///// <inheritdoc />
    //protected override void OnInitialized()
    //{
    //    FillInternalId();
    //}

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        LogIf.ArgumentRequired(Logger, Expand, nameof(Expand));

        ComponentClass = Cssc.Class(
            Variant?.ToCss(VrtLead.Down), 
            "cnvb",
            Expand?.ToCssNavBar());
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

        builder.OpenElement(0, "div"); // div, 시작
        builder.AddAttribute(1, "class", ComponentClass);
        builder.AddMultipleAttributes(2, UserAttrs);

        if (Brand is not null)
        {
            if (Home.WhiteSpace())
            {
                builder.OpenElement(10, "div"); // div, 브랜드
                builder.AddAttribute(11, "class", Cssc.Class("cnvbd", BrandClass));
                builder.AddContent(12, Brand); // 브랜드 프래그먼트
                builder.CloseElement();
            }
            else
            {
                builder.OpenElement(10, "a"); // a, 브랜드
                builder.AddAttribute(11, "class", Cssc.Class("cnvbd", BrandClass));
                builder.AddAttribute(12, "href", Home);
                builder.AddContent(13, Brand); // 브랜드 프래그먼트
                builder.CloseElement();
            }
        }

        builder.OpenElement(20, "button"); // button, 토글
        builder.AddAttribute(21, "class", "cnvbt");
        builder.AddAttribute(22, "type", "button");
        builder.AddAttribute(23, "aria-expanded", _visible.ToHtml());
        builder.AddAttribute(24, "aria-label", "Navbar toggle");
        builder.AddAttribute(25, "onclick", HandleButtonOnClick);
        builder.AddEventStopPropagationAttribute(26, "onclick", true);
        if (Toggle is not null)
            builder.AddContent(28, Toggle); // 토글 프래그먼트
        else
        {
            builder.OpenElement(28, "span"); // span, 아이콘
            builder.AddAttribute(29, "class", _visible ? "ic-x" : "ic-m");
            builder.CloseElement();
        }
        builder.CloseElement();

        if (Menu is not null)
        {
            builder.OpenElement(30, "nav"); // nav, 메뉴
            builder.AddAttribute(31, "class", Cssc.Class("cnvbn", _visible.IfTrue("rsp"), MenuClass));

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
        ComponentRole.Link =>"cnvbb",
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
}


