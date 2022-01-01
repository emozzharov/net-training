
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;


namespace IQueryableTask
{
    public class PeopleDbQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            // TODO: Implement CreateQuery
            //throw new NotImplementedException();
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(PersonService)
                    .MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            // TODO: Implement CreateQuery
            //throw new NotImplementedException();
            return new People<TResult>(this, expression);
        }

        public object Execute(Expression expression)
        {
            // TODO: Implement Execute
            //throw new NotImplementedException();
            return TerraServerQueryContext.Execute(expression, false);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            // TODO: Implement Execute
            //throw new NotImplementedException();

            // HINT: Use GetSqlQuery to build query and pass the query to PersonService
            bool IsEnumerable = (typeof(TResult).Name == "IEnumerable`1");

            return (TResult)TerraServerQueryContext.Execute(expression, IsEnumerable);

        }

        /// <summary>
        /// Generates YQL Query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns></returns>
        public string GetSqlQuery(Expression expression)
        {
            // TODO: Implement GetYqlQuery
            throw new NotImplementedException();

            // HINT: This method is not part of IQueryProvider interface and is used here only for tests.
            // HINT: To transform expression to sql query create a class derived from ExpressionVisitor
            // HINT: Read the tutorial https://msdn.microsoft.com/en-us/library/bb546158.aspx for more info
        }
    }
}
