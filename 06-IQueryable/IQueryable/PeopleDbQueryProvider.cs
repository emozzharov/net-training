
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;

namespace IQueryableTask
{
    public class PeopleDbQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            // TODO: Implement CreateQuery
            return new People(expression);

        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            // TODO: Implement CreateQuery
            return (IQueryable<TResult>)new People(expression);
        }

        public object Execute(Expression expression)
        {
            // TODO: Implement Execute
            PersonService isEnumerable = new PersonService();

            var result = isEnumerable.Search(GetSqlQuery(expression));

            return result;
        }


        public TResult Execute<TResult>(Expression expression)
        {
            // TODO: Implement Execute
            // HINT: Use GetSqlQuery to build query and pass the query to PersonService
            var isEnumerable = new PeopleDbQueryProvider();

            var result = isEnumerable.Execute(expression);

            return (TResult)result;
        }

        //public TResult Execute<TResult>(Expression expression) => _provider.Execute<TResult>(expression);

        /// <summary>
        /// Generates YQL Query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns></returns>
        public string GetSqlQuery(Expression expression)
        {
            // TODO: Implement GetYqlQuery

            // HINT: This method is not part of IQueryProvider interface and is used here only for tests.
            // HINT: To transform expression to sql query create a class derived from ExpressionVisitor
            // HINT: Read the tutorial https://msdn.microsoft.com/en-us/library/bb546158.aspx for more info
            var translator = new MyQueryTranslator();
            string whereClause = translator.Translate(expression);

            return whereClause;
        }
    }
}
