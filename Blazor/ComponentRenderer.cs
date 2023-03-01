using Du.Blazor.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor;

/// <summary>
/// 컴포넌트 렌더러
/// </summary>
public static class ComponentRenderer
{
	/// <summary>
	/// 태그를 이용해서 자식 콘텐트를 그린다
	/// <example><code>
	/// &lt;tag class="@CssClass" @attributes="UserAttrs"&gt;
	///     @ChildContent
	/// &lt;/tag&gt;
	/// </code></example>
	/// </summary>
	/// <param name="component"></param>
	/// <param name="builder"></param>
	/// <param name="tag"></param>
	public static void TagFragment(ComponentFragment component, RenderTreeBuilder builder, string tag = "div")
	{
		/*
		 * <tag class="@CssClass" @attributes="UserAttrs">
		 *    @ChildContent
		 * </tag>
		 */

		builder.OpenElement(0, tag);
		builder.AddAttribute(1, "class", component.CssClass);
		builder.AddMultipleAttributes(2, component.UserAttrs);
		builder.AddContent(3, component.ChildContent);
		builder.CloseElement(); // tag
	}

	/// <summary>
	/// 태그를 이용해서 자식 콘텐트를 캐스케이딩해서 그린다
	/// <example><code>
	/// &lt;tag class="@CssClass" @attributes="@UserAttrs"&gt;
	///     &lt;CascadingValue Value="this" IsFixed="true&gt;
	///         @Content
	///     &lt;/CascadingValue&gt;
	/// &lt;/tag&gt;
	/// </code></example>
	/// </summary>
	/// <typeparam name="TType"></typeparam>
	/// <param name="component"></param>
	/// <param name="builder"></param>
	/// <param name="tag"></param>
	public static void CascadingTagFragment<TType>(ComponentFragment component, RenderTreeBuilder builder, string tag = "div")
	{
		/*
		 * <tag class="@CssClass" @attributes="@UserAttrs">
		 *     <CascadingValue Value="this" IsFixed="true>
		 *         @ChildContent
		 *     </CascadingValue>
		 * </tag>
		 */
		builder.OpenElement(0, tag);
		builder.AddAttribute(1, "class", component.CssClass);
		builder.AddMultipleAttributes(2, component.UserAttrs);

		builder.OpenComponent<CascadingValue<TType>>(3);
		builder.AddAttribute(4, "Value", component);
		builder.AddAttribute(5, "IsFixed", true);
		builder.AddAttribute(6, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(7, component.ChildContent)));
		builder.CloseComponent(); // CascadingValue<TType>

		builder.CloseElement(); // tag
	}

	/// <summary>
	/// TagTextBase용 렌더러<br/>
	/// <see cref="TagTextBase.Text"/>와 <see cref="ComponentFragment.ChildContent"/>를
	/// 동시에 그린다
	/// </summary>
	/// <param name="component"></param>
	/// <param name="builder"></param>
	public static void TagText(TagTextBase component, RenderTreeBuilder builder)
	{
		/*
		 * <div class="@CssClass" @attributes="@UserAttrs">
		 *     @Text
		 *     @ChildContent
		 * </div>
		 */
		builder.OpenElement(0, component.Tag);
		builder.AddAttribute(1, "class", component.CssClass);

		if (component.OnClick.HasDelegate)
		{
			builder.AddAttribute(2, "role", "button");
			builder.AddAttribute(3, "onclick", component.InternalOnClick);
			builder.AddEventStopPropagationAttribute(4, "onclick", true);
		}

		builder.AddMultipleAttributes(5, component.UserAttrs);
		builder.AddContent(6, component.Text);
		builder.AddContent(7, component.ChildContent);
		builder.CloseElement(); // tag
	}

	/// <summary>
	/// TagItem용 태그로 감싸고 렌더링
	/// </summary>
	/// <param name="component"></param>
	/// <param name="builder"></param>
	/// <param name="tag"></param>
	public static void SurroundTagText(TagItem component, RenderTreeBuilder builder, string tag) 
	{
		/*
		 * 	<li>
		 * 		<div class="@CssClass" @attributes="@UserAttrs">
		 * 			@Text
		 * 			@ChildContent
		 * 		</div>
		 * 	</li>
		 */

		builder.OpenElement(0, tag);

		if (component.ListClass.IsHave())
			builder.AddAttribute(1, component.ListClass);

		builder.OpenElement(2, component.Tag);
		builder.AddAttribute(3, "class", component.CssClass);

		if (component.OnClick.HasDelegate)
		{
			builder.AddAttribute(4, "role", "button");
			builder.AddAttribute(5, "onclick", component.InternalOnClick);
			builder.AddEventStopPropagationAttribute(6, "onclick", true);
		}

		builder.AddMultipleAttributes(7, component.UserAttrs);
		builder.AddContent(8, component.Text);
		builder.AddContent(9, component.ChildContent);
		builder.CloseElement(); // tag

		builder.CloseElement(); // li
	}
}
