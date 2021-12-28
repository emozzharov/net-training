using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IQueryableTask
{
    public class SqlExpressionVisitor : ExpressionVisitor
    {
        private StringBuilder sb;
        private string whereClause = string.Empty;
        private string containsCondition = string.Empty;
        
        public SqlExpressionVisitor()
        {
        }

        public string GetQuery(Expression expression)
        {
            this.sb = new StringBuilder();
            this.Visit(expression);
            whereClause = this.sb.ToString();
            return whereClause;
        }

        public string WhereClause
        {
            get
            {
                return whereClause;
            }
        }

        public string ContainsCondistion
        {
            get
            {
                return containsCondition;
            }
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
            {
                this.Visit(m.Arguments[0]);

                LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);

                var body = lambda.Body;

                if (body.ToString().IndexOf("FullName", StringComparison.InvariantCultureIgnoreCase) >= 0
                    || body.ToString().IndexOf("LastName", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    throw new NotSupportedException("Such filters are not supported");
                }

                this.Visit(body);
                return m;
            } else if (m.Method.Name == "Contains")
            {
                string nameOfPorperty = m.Object.ToString().Split('.').Last();

                if (nameOfPorperty.Equals("FullName", StringComparison.InvariantCultureIgnoreCase) 
                    || nameOfPorperty.Equals("LastName", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new NotSupportedException("Such names filters are not supported");
                }

                sb.Append(nameOfPorperty);
                sb.Append(" like ");

                // The following block gets argument of the SQL request.
                var innerLambda = Expression.Lambda<Func<object>>(Expression.Convert(StripQuotes(m.Arguments[0]), typeof(object)));
                var argument = innerLambda.Compile().Invoke().ToString();

                argument = "%" + argument + "%";
                argument = "'" + argument + "'";

                sb.Append(argument);

                return m;
            }

            throw new InvalidOperationException(string.Format("The method is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator is not supported", u.NodeType));
            }

            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            this.Visit(b.Left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.Or:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.OrElse:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.Equal:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS ");
                    }
                    else
                    {
                        sb.Append(" = ");
                    }
                    break;

                case ExpressionType.NotEqual:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS NOT ");
                    }
                    else
                    {
                        sb.Append(" <> ");
                    }
                    break;

                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;

                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;

                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));

            }

            this.Visit(b.Right);
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;

            if (q == null && c.Value == null)
            {
                sb.Append("NULL");
            }
            else if (q == null)
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)c.Value) ? 1 : 0);
                        break;

                    case TypeCode.String:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;

                    case TypeCode.DateTime:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;

                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));

                    default:
                        sb.Append(c.Value);
                        break;
                }
            }

            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(m.Member.Name);
                return m;
            }

            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }

        protected bool IsNullConstant(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Constant && ((ConstantExpression)exp).Value == null);
        }

        private bool ParseContainsExpression(MethodCallExpression methodCallExpression)
        {
            ConstantExpression constantExpression = (ConstantExpression)methodCallExpression.Arguments[0];

            if (!string.IsNullOrWhiteSpace(constantExpression.Value?.ToString()))
            {
                containsCondition = constantExpression.Value.ToString();
                return true;
            }

            return false;
        }
    }
}
