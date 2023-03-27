namespace Du.Blazor.Components;

// fa 쓸라면: <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

public class Icon : ComponentProp
{
	/// <summary>아이콘 이름</summary>
	[Parameter] public string? Name { get; set; }

	//
	private string? _icon;

	//
	public Icon()
	{
		// 아이콘은 span이라 텍스트다
		TagRole = ComponentRole.Text;
	}

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		if (Name?.Length > 3)
		{
			_icon = Name[2] == '-' 
				? $"{Name[..2]} {Name}" // oi / bi / fa 같은거는 세번째 빼기가 있다
				: Name; // 걍 넣음
		}
		else
		{
			// 내장 아이콘 https://icon-sets.iconify.design/oi/bolt/
			_icon = "dzi dzib";
		}
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "span");
		builder.AddAttribute(1, "class", Cssc.Class(_icon, Class));
		builder.AddAttribute(2, "aria-hidden", "true");
		builder.CloseElement();
	}
}

