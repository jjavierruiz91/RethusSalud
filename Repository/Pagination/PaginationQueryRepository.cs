using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using rethus_backend.Models.Dto.Comments;

public static class QueryHelper
{
    private static readonly Dictionary<
        string,
        Func<Expression, Expression, BinaryExpression>
    > operators = new Dictionary<string, Func<Expression, Expression, BinaryExpression>>
    {
        { "=", Expression.Equal },
        { ">", Expression.GreaterThan },
        { ">=", Expression.GreaterThanOrEqual },
        { "<", Expression.LessThan },
        { "<=", Expression.LessThanOrEqual }
    };

    public static IQueryable<T> ApplyFilters<T>(
        IQueryable<T> query,
        CommonQueryParametersDto parameters
    )
    {
        var parameterExpression = Expression.Parameter(typeof(T), "q");
        Expression finalExpression = null;

        // Lista para almacenar expresiones de filtro individuales
        var filterExpressions = new List<Expression>();

        foreach (var property in typeof(CommonQueryParametersDto).GetProperties())
        {
            var propertyName = property.Name;
            var propertyValue = property.GetValue(parameters);

            // Verifica si la propiedad es el operador lógico o los operadores de comparación
            if (
                propertyName == nameof(parameters.LogicalOperator)
                || propertyName == nameof(parameters.ComparisonOperators)
            )
                continue;

            if (propertyValue != null)
            {
                var comparisonOperator =
                    parameters.ComparisonOperators != null
                    && parameters.ComparisonOperators.ContainsKey(propertyName)
                        ? parameters.ComparisonOperators[propertyName]
                        : "="; // Operador predeterminado si no se especifica

                if (operators.TryGetValue(comparisonOperator, out var operatorExpression))
                {
                    var propertyExpression = Expression.Property(parameterExpression, propertyName);
                    var constantExpression = Expression.Constant(
                        propertyValue,
                        propertyExpression.Type
                    );
                    var comparisonExpression = operatorExpression(
                        propertyExpression,
                        constantExpression
                    );

                    filterExpressions.Add(comparisonExpression);
                }
                else
                {
                    throw new ArgumentException($"Unsupported operator: {comparisonOperator}");
                }
            }
        }

        if (filterExpressions.Count > 0)
        {
            // Combinar todas las expresiones de filtro usando el operador lógico especificado
            finalExpression =
                parameters.LogicalOperator == "OR"
                    ? filterExpressions.Aggregate(Expression.OrElse)
                    : filterExpressions.Aggregate(Expression.AndAlso);

            var lambdaExpression = Expression.Lambda<Func<T, bool>>(
                finalExpression,
                parameterExpression
            );
            query = query.Where(lambdaExpression);
        }

        return query;
    }
}
