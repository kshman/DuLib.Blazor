namespace Du.Blazor.Supp;

internal static class Renderer
{
	internal static void Tag(RenderTreeBuilder builder, string tag,
		string? css, RenderFragment? content, IDictionary<string, object>? attrs)
	{
		/*
		 * <tag class="@CssClass" id="@Id" @attributes="UserAttrs">
		 *    @ChildContent
		 * </tag>
		 */

		builder.OpenElement(0, tag);
		builder.AddAttribute(1, "class", css);
		builder.AddMultipleAttributes(2, attrs);
		if (content is not null)
			builder.AddContent(3, content);
		builder.CloseElement(); // tag
	}

	// 태그로 자식 콘텐츠를 캐스캐이딩해서 그린다
	internal static void CascadingTag<TComponent>(RenderTreeBuilder builder, TComponent self, string tag,
		string? css, RenderFragment? content, IDictionary<string, object>? attrs)
	{
		/*
		 * <tag class="@CssClass" id="@Id" @attributes="@UserAttrs">
		 *     <CascadingValue Value="this" IsFixed="true>
		 *         @ChildContent
		 *     </CascadingValue>
		 * </tag>
		 */
		builder.OpenElement(0, tag);
		builder.AddAttribute(1, "class", css);
		builder.AddMultipleAttributes(3, attrs);

		if (content is not null)
		{
			builder.OpenComponent<CascadingValue<TComponent>>(4);
			builder.AddAttribute(5, "Value", self);
			builder.AddAttribute(6, "IsFixed", true);
			builder.AddAttribute(7, "ChildContent", (RenderFragment)((b) =>
				b.AddContent(8, content)));
			builder.CloseComponent(); // CascadingValue<TType>
		}

		builder.CloseElement(); // tag
	}
}
