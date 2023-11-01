namespace Domain.Extensions.Collections;

public sealed class DynamicOrderParamsException : Exception
{
    public DynamicOrderParamsException(string dynamicOrderParams)
        : base($"The provided {nameof(dynamicOrderParams)} '{dynamicOrderParams}' don't meet the criteria: '<property>:<asc(ending)/desc(ending)>;<property>:<asc(ending)/desc(ending)>;...<property>:<asc(ending)/(desc)ending>'.") { }
}
