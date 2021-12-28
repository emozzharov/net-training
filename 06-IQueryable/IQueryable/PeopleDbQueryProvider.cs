using System;
using System.Linq;
using System.Linq.Expressions;

namespace IQueryableTask
{
    public class PeopleDbQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            return new People(expression);
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return (IQueryable<TResult>)new People(expression);
        }

        public object Execute(Expression expression)
        {
            return Execute<People>(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            PersonService service = new PersonService();
            return (TResult)service.Search(GetSqlQuery(expression)); //???????

            // HINT: Use GetSqlQuery to build query and pass the query to PersonService
        }

        /// <summary>
        /// Generates YQL Query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns></returns>
        public string GetSqlQuery(Expression expression)
        {
            var visitor = new SqlExpressionVisitor();

            string sqlQuery = 
                "select * from person where " + 
                visitor.GetQuery(expression);

            return sqlQuery;

            // HINT: This method is not part of IQueryProvider interface and is used here only for tests.
            // HINT: To transform expression to sql query create a class derived from ExpressionVisitor
            // HINT: Read the tutorial https://msdn.microsoft.com/en-us/library/bb546158.aspx for more info
        }
    }
}
