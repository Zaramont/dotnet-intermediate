using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;
        readonly string queriesDelimiter = " AndAlso ";

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }
            else if (node.Method.DeclaringType == typeof(string))
            {
                var constant = node.Arguments[0];

                Visit(node.Object);

                _resultStringBuilder.Append("(");
                if (node.Method.Name == "Contains" || node.Method.Name == "EndsWith") _resultStringBuilder.Append("*");

                Visit(constant);

                if (node.Method.Name == "Contains" || node.Method.Name == "StartsWith") _resultStringBuilder.Append("*");
                _resultStringBuilder.Append(")");

                return node;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:

                    if (node.Left.NodeType != ExpressionType.MemberAccess && node.Left.NodeType != ExpressionType.Constant)
                    {
                        throw new NotSupportedException($"Left operand should be property or field or constant: {node.NodeType}");
                    }

                    if (node.Right.NodeType != ExpressionType.MemberAccess && node.Right.NodeType != ExpressionType.Constant)
                    {
                        throw new NotSupportedException($"Right operand should be property or field or constant: {node.NodeType}");
                    }

                    if (node.Left.NodeType == ExpressionType.MemberAccess && node.Right.NodeType != ExpressionType.Constant)
                        throw new NotSupportedException($"Right operand should be constant: {node.NodeType}");

                    if (node.Left.NodeType == ExpressionType.Constant && node.Right.NodeType != ExpressionType.MemberAccess)
                        throw new NotSupportedException($"Right operand should be property or field: {node.NodeType}");

                    var member = node.Left.NodeType == ExpressionType.MemberAccess ? node.Left : node.Right;
                    var constant = node.Left.NodeType == ExpressionType.Constant ? node.Left : node.Right;

                    Visit(member);
                    _resultStringBuilder.Append("(");
                    Visit(constant);
                    _resultStringBuilder.Append(")");
                    break;

                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    _resultStringBuilder.Append(queriesDelimiter);
                    Visit(node.Right);
                    break;
                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
