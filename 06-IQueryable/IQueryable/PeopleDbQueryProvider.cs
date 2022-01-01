﻿
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;


namespace IQueryableTask
{
    public class PeopleDbQueryProvider : IQueryProvider
    {
        private readonly IQueryProvider _provider;

        public IQueryable CreateQuery(Expression expression)
        {
            // TODO: Implement CreateQuery
            //throw new NotImplementedException();
            return new People(expression);

        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            // TODO: Implement CreateQuery
            //throw new NotImplementedException();
            return (IQueryable<TResult>)new People(expression);
        }

        public object Execute(Expression expression)
        {
            // TODO: Implement Execute
            //throw new NotImplementedException();
            return Person.Execute(expression, false);

        }

        //public object Execute(Expression expression) => _provider.Execute(expression);

        public TResult Execute<TResult>(Expression expression)
        {
            // TODO: Implement Execute
            throw new NotImplementedException();

            // HINT: Use GetSqlQuery to build query and pass the query to PersonService
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
            throw new NotImplementedException();

            // HINT: This method is not part of IQueryProvider interface and is used here only for tests.
            // HINT: To transform expression to sql query create a class derived from ExpressionVisitor
            // HINT: Read the tutorial https://msdn.microsoft.com/en-us/library/bb546158.aspx for more info
        }

        internal class InnermostWhereFinder : ExpressionVisitor
        {
            private MethodCallExpression innermostWhereExpression;

            public MethodCallExpression GetInnermostWhere(Expression expression)
            {
                Visit(expression);
                return innermostWhereExpression;
            }

            protected override Expression VisitMethodCall(MethodCallExpression expression)
            {
                if (expression.Method.Name == "Where")
                    innermostWhereExpression = expression;

                Visit(expression.Arguments[0]);

                return expression;
            }
        }
    }
}
