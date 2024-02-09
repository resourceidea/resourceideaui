namespace EastSeat.ResourceIdea.Application.Tests.Setup;

public static class TestEntityBuilder
{
    public static T Create<T>(Func<T> createInstance)
    {
        return createInstance();
    }

    public static T With<T>(this T entity, Action<T> setup) where T : class
    {
        setup(entity);
        return entity;
    }
}
