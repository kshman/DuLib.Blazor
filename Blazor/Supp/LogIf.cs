namespace Du.Blazor.Supp;

internal static class LogIf
{
    //
    private static void ThrowBySetting()
    {
        if (Settings.ThrowOnLog)
        {
            throw new Exception(Settings.UseLocaleMesg
                ? "설정(Settings.ThrowOnLog)에 의해 예외가 발동했어요."
                : "Throw by user setting in Settings.ThrowOnLog.");
        }
    }

    // 컨테이너가 널이면 로그
    internal static void ContainerIsNull<TItem, TContainer>(ILogger<TItem> logger, object item, TContainer? container)
    {
        if (container is not null)
            return;

        logger.LogWarning(Settings.UseLocaleMesg
            ? "{item}: 컨테이너가 없어요. 이 컴포넌트는 반드시 <{container}> 컨테이너 아래 있어야 해요."
            : "{item}: No container found. This component must be contained within <{container}> component.",
            item.GetType().Name, typeof(TContainer).Name);

        ThrowBySetting();
    }

    //
    internal static void FailWithMessage<TItem>(ILogger<TItem> logger, bool condition, string message)
    {
        if (condition)
            return;

        logger.LogWarning(message);

        ThrowBySetting();
    }

    //
    internal static T ArgumentOutOfRange<T>(object v, string name)
    {
        System.Diagnostics.Debug.WriteLine(Settings.UseLocaleMesg
            ? $"인수 {name}의 값이 범위를 벗어났어요. (값: {v})"
            : $"Argument {name} is out of range. (value: {v})");

        ThrowBySetting();

        return default!;
    }

    //
    internal static void ArgumentRequired<TItem>(ILogger<TItem> logger, object? v, string name)
    {
        if (v is not null)
            return;

        logger.LogError(Settings.UseLocaleMesg
            ? "{name}: 인수를 넣지 않았어요. 이 인수는 사용하는 것이 좋습니다." 
            : "{name}: argument is null. This argument is required.", 
            name);

        ThrowBySetting();
    }
}
