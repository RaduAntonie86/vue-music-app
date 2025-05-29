using Dapper;
using System.Data;

public class ListLongHandler : SqlMapper.TypeHandler<List<long>>
{
    public override void SetValue(IDbDataParameter parameter, List<long> value)
    {
        // This assumes PostgreSQL array support; adjust if needed for other databases
        parameter.Value = value.ToArray();
    }

    public override List<long> Parse(object value)
    {
        if (value is long[] longArray)
            return longArray.ToList();

        if (value is IEnumerable<long> enumerable)
            return enumerable.ToList();

        throw new DataException($"Cannot convert {value?.GetType().Name} to List<long>.");
    }
}